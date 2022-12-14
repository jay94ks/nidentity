using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIdentity.Core;
using NIdentity.Core.Commands;
using NIdentity.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Connector.Internals
{
    /// <summary>
    /// Execute commands using HTTPS requests.
    /// </summary>
    internal partial class HttpRequestExecutor : ICommandExecutor, IDisposable
    {
        private readonly RemoteCommandExecutorParameters m_Parameters;
        private readonly HttpClient m_HttpClient;

        private long m_Errors;

        /// <summary>
        /// Initialize a new <see cref="HttpRequestExecutor"/> instance.
        /// </summary>
        /// <param name="Parameters"></param>
        public HttpRequestExecutor(RemoteCommandExecutorParameters Parameters)
        {
            Parameters.ThrowExceptionIfInvalid();

            m_Parameters = Parameters;

            if (!Parameters.DisableAuthorityCertificate)
            {
                m_HttpClient = new HttpClient(new Handler(Parameters.Certificate, Parameter.ServerCertificate), true);
                // --> for optimization. (optional behaviours)
                m_HttpClient.DefaultRequestHeaders.Add("X-NIdentity-KeySHA1", Parameters.Certificate.KeySHA1);
                m_HttpClient.DefaultRequestHeaders.Add("X-NIdentity-RefSHA1", Parameters.Certificate.RefSHA1);
            }
            else
            {
                m_HttpClient = new HttpClient();
            }
        }

        /// <summary>
        /// Error counter.
        /// </summary>
        public long Errors => Interlocked.Read(ref m_Errors);

        /// <inheritdoc/>
        public void Dispose() => m_HttpClient.Dispose();

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

            return Execute(Json, Token);
        }

        /// <inheritdoc/>
        public async Task<CommandResult> Execute(JObject Json, CancellationToken Token = default)
        {
            using var Content = new StringContent(Json.ToString(), Encoding.UTF8, "application/json");
            using var Timeout = CancellationTokenSource.CreateLinkedTokenSource(Token);

            Timeout.CancelAfter(m_Parameters.Timeout);
            try
            {
                using var Message = await m_HttpClient.PostAsync(m_Parameters.ServerUri, Content, Timeout.Token);
                if (Message.IsSuccessStatusCode)
                {
                    var Text = await Message.Content.ReadAsStringAsync(Token);
                    var Response = JsonConvert.DeserializeObject<JObject>(Text);
                    var Result = Response.ToObject<RemoteCommandResult>();

                    Interlocked.Exchange(ref m_Errors, 0);
                    if (Result != null)
                    {
                        Result.Response = Response;
                        return Result;
                    }

                    return new RemoteCommandResult { Success = true };
                }

                Interlocked.Exchange(ref m_Errors, 0);
                return new CommandResult
                {
                    Success = false,
                    Reason = $"{(int)Message.StatusCode} {Message.ReasonPhrase}",
                    ReasonKind = "HttpRequest"
                };
            }

            catch (Exception Error)
            {
                if (!Token.IsCancellationRequested && Error is OperationCanceledException)
                    Error = new TimeoutException("Timeout reached.", Error);

                var Postfix = typeof(Exception).Name;
                var Kind = Error.GetType().Name;
                if (Kind != Postfix && Kind.EndsWith(Postfix))
                    Kind = Kind.Substring(0, Kind.Length - Postfix.Length);

                if (Error is HttpRequestException)
                    Interlocked.Increment(ref m_Errors);

                return new CommandResult
                {
                    Success = false,
                    Reason = Error.Message,
                    ReasonKind = Kind
                };
            }
        }
    }
}
