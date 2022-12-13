using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;

namespace NIdentity.Core.Server.Endpoints
{
    public class HttpExecutorEndpoint
    {
        /// <summary>
        /// Handle HTTP GET requests.
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task HandleGetAsync(HttpContext Http)
        {
            if (!Http.WebSockets.IsWebSocketRequest)
                return;

            var Executor = Http.RequestServices.GetRequiredService<ICommandExecutor>();
            var WebSocket = await Http.WebSockets.AcceptWebSocketAsync(new WebSocketAcceptContext
            {
                KeepAliveInterval = TimeSpan.FromSeconds(5),
                DangerousEnableCompression = false
            });

            if (WebSocket is null)
            {
                Http.Response.StatusCode = 400;
                return;
            }

            try
            {
                var Buffer = new MemoryStream();
                var Temp = new byte[2048];
                while (true)
                {
                    WebSocketReceiveResult Receive = null;

                    try { Receive = await WebSocket.ReceiveAsync(Temp, Http.RequestAborted); }
                    catch
                    {
                    }

                    if (Receive is null || Receive.MessageType != WebSocketMessageType.Text)
                        break;

                    Buffer.Write(Temp, 0, Receive.Count);
                    if (Receive.EndOfMessage)
                    {
                        // --> execute the received command.
                        await ExecuteCommand(Http, Executor, WebSocket, Buffer);
                        try { Buffer.Dispose(); }
                        catch
                        {
                        }

                        Buffer = new MemoryStream();
                    }
                }
            }

            catch { }
            finally
            {
                try
                {
                    await WebSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        string.Empty, default);
                }
                catch { }

                try { WebSocket.Dispose(); }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Handle HTTP POST requests.
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        public static async Task HandlePostAsync(HttpContext Http)
        {
            if (string.IsNullOrWhiteSpace(Http.Request.ContentType))
            {
                Http.Response.StatusCode = 400;
                return;
            }

            JObject Json;
            using (var Buffer = new MemoryStream())
            {
                try { await Http.Request.Body.CopyToAsync(Buffer); }
                catch
                {
                    Http.Response.StatusCode = 400;
                    return;
                }

                var Text = Encoding.UTF8.GetString(Buffer.ToArray());
                Json = JsonConvert.DeserializeObject<JObject>(Text);
            }

            try
            {
                var Executor = Http.RequestServices.GetRequiredService<ICommandExecutor>();
                var Result = await Executor.Execute(Json, Http.RequestAborted);
                var ResultBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Result));

                Http.Response.StatusCode = 200;
                Http.Response.ContentType = Http.Request.ContentType;
                await Http.Response.Body.WriteAsync(ResultBytes);
            }

            catch
            {
                Http.Response.StatusCode = 500;
            }
        }

        /// <summary>
        /// Execute a command.
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Executor"></param>
        /// <param name="WebSocket"></param>
        /// <param name="Buffer"></param>
        /// <returns></returns>
        private static async Task ExecuteCommand(HttpContext Http, ICommandExecutor Executor, WebSocket WebSocket, MemoryStream Buffer)
        {
            var Bytes = Buffer.ToArray();
            var Text = Encoding.UTF8.GetString(Bytes);

            var Json = JsonConvert.DeserializeObject<JObject>(Text);
            var Result = await Executor.Execute(Json, Http.RequestAborted);

            Text = JsonConvert.SerializeObject(Result);
            Bytes = Encoding.UTF8.GetBytes(Text);

            await WebSocket.SendAsync(Bytes, WebSocketMessageType.Text, true, Http.RequestAborted);
        }

    }
}
