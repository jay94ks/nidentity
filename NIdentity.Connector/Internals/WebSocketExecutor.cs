using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIdentity.Core;
using NIdentity.Core.Commands;
using NIdentity.Core.Helpers;
using NIdentity.Core.X509;
using System.Net.Security;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;

namespace NIdentity.Connector.Internals
{
    internal class WebSocketExecutor : ICommandExecutor, IDisposable
    {
        private readonly RemoteCommandExecutorParameters m_Parameters;
        private readonly SemaphoreSlim m_Semaphore = new(1);

        private WebSocket m_WebSocket;
        private MemoryStream m_Buffer;
        private byte[] m_Temp = new byte[256];

        /// <summary>
        /// Initialize a new <see cref="WebSocketExecutor"/> instance.
        /// </summary>
        /// <param name="Parameters"></param>
        public WebSocketExecutor(RemoteCommandExecutorParameters Parameters)
        {
            Parameters.ThrowExceptionIfInvalid();
            m_Parameters = Parameters;
        }

        /// <summary>
        /// Indicates whether the executor is connected to server or not.
        /// </summary>
        public bool IsConnected => m_WebSocket != null;

        /// <inheritdoc/>
        public Task<CommandResult> Execute(Command Command, CancellationToken Token = default)
        {
            var Attr = Command.GetType().GetCustomAttribute<CommandAttribute>();
            var Kind = Attr is null ? string.Empty : (Attr.Kind ?? string.Empty);
            var Json =
                JsonConvert.DeserializeObject<JObject>(
                JsonConvert.SerializeObject(Command));

            var Type = Json.Get<string>("type") ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(Kind))
                Json.Set("type", $"{Kind}.{Type}".Trim('.'));

            return Execute(Json, Attr != null ? Attr.ResultType : null, Token);
        }

        /// <inheritdoc/>
        public Task<CommandResult> Execute(JObject Json, CancellationToken Token = default)
            => Execute(Json, null, Token);

        /// <inheritdoc/>
        private async Task<CommandResult> Execute(JObject Json, Type ExpectedType, CancellationToken Token = default)
        {
            using var Timeout = CancellationTokenSource.CreateLinkedTokenSource(Token);

            Timeout.CancelAfter(m_Parameters.Timeout);
            try
            {
                await m_Semaphore.WaitAsync(Token);

                while (m_WebSocket is null || m_WebSocket.State != WebSocketState.Open)
                {
                    await CloseAsync();
                    m_WebSocket = await ConnectAsync(Timeout.Token);
                }

                return await SendAndReceiveAsync(Json, ExpectedType, Timeout);
            }

            catch (Exception Error)
            {
                if (!Token.IsCancellationRequested && Error is OperationCanceledException)
                    Error = new TimeoutException("Timeout reached.", Error);

                var Postfix = typeof(Exception).Name;
                var Kind = Error.GetType().Name;
                if (Kind != Postfix && Kind.EndsWith(Postfix))
                    Kind = Kind.Substring(0, Kind.Length - Postfix.Length);

                return new CommandResult
                {
                    Success = false,
                    Reason = Error.Message,
                    ReasonKind = Kind
                };
            }

            finally
            {
                try { m_Semaphore.Release(); }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Connect to the websocket endpoint.
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        private async Task<WebSocket> ConnectAsync(CancellationToken Token = default)
        {
            while (true)
            {
                var WebSocket = new ClientWebSocket();

                WebSocket.Options.KeepAliveInterval = m_Parameters.Timeout;
                if (!m_Parameters.DisableAuthorityCertificate)
                {
                    WebSocket.Options.RemoteCertificateValidationCallback = (_, ReceivedCertificate, _2, Error) =>
                    {
                        return HttpRequestExecutor.Handler.ChcekServerCertificate(
                            m_Parameters.ServerCertificate, ReceivedCertificate, Error);
                    };
                    WebSocket.Options.ClientCertificates.Add(m_Parameters.Certificate.ToDotNetCert());
                    WebSocket.Options.SetRequestHeader("X-NIdentity-KeySHA1", m_Parameters.Certificate.KeySHA1);
                    WebSocket.Options.SetRequestHeader("X-NIdentity-RefSHA1", m_Parameters.Certificate.RefSHA1);
                }

                try { await WebSocket.ConnectAsync(m_Parameters.ServerUri, Token); }
                catch
                {
                    Token.ThrowIfCancellationRequested();
                    WebSocket.Dispose();
                    throw;
                }

                return WebSocket;
            }
        }

        /// <summary>
        /// Send the <paramref name="Json"/> and receive the result message.
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="ExpectedType"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        private async Task<CommandResult> SendAndReceiveAsync(JObject Json, Type ExpectedType, CancellationTokenSource Timeout)
        {
            var Bytes = Encoding.UTF8.GetBytes(Json.ToString());
            await m_WebSocket.SendAsync(Bytes, WebSocketMessageType.Text, true, Timeout.Token);

            while (true)
            {
                WebSocketReceiveResult Receive;

                try { Receive = await m_WebSocket.ReceiveAsync(m_Temp, Timeout.Token); }
                catch
                {
                    await CloseAsync();
                    throw;
                }

                if (Receive is null || Receive.MessageType != WebSocketMessageType.Text)
                {
                    await CloseAsync();
                    return new CommandResult
                    {
                        Success = false,
                        ReasonKind = "InvalidOperation",
                        Reason = Receive != null
                            ? "Server sent invalid message."
                            : "Connection lost"
                    };
                }

                if (m_Buffer is null)
                    m_Buffer = new MemoryStream();

                m_Buffer.Write(m_Temp, 0, Receive.Count);

                if (Receive.EndOfMessage)
                {
                    try
                    {
                        var Text = Encoding.UTF8.GetString(m_Buffer.ToArray());
                        var Response = JsonConvert.DeserializeObject<JObject>(Text);
                        if (ExpectedType != null && Response.Get<bool>("success") == true)
                            return (CommandResult)Response.ToObject(ExpectedType);

                        var Result = Response.ToObject<RemoteCommandResult>();
                        if (Result != null)
                        {
                            Result.Response = Response;
                            return Result;
                        }

                        return new RemoteCommandResult { Success = true };
                    }

                    catch
                    {
                        await CloseAsync();
                        throw;
                    }

                    finally
                    {
                        try { m_Buffer.Dispose(); }
                        catch
                        {
                        }

                        m_Buffer = null;
                    }
                }
            }
        }

        /// <summary>
        /// Close the websocket asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task CloseAsync()
        {
            if (m_WebSocket != null)
            {
                try
                {
                    if (m_WebSocket.State == WebSocketState.Open)
                        await m_WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, default);
                }
                catch
                {
                }

                try { m_WebSocket.Dispose(); }
                catch
                {
                }

                m_WebSocket = null;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            try { m_WebSocket?.Dispose(); } catch { }
            try { m_Semaphore.Dispose(); } catch { }
        }
    }
}
