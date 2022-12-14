using Newtonsoft.Json;
using NIdentity.Core.X509;

namespace NIdentity.Connector
{
    public class RemoteCommandExecutorParameters
    {
        /// <summary>
        /// Server Uri.
        /// </summary>
        public Uri ServerUri { get; set; }

        /// <summary>
        /// Disable authority certificate.
        /// If this option set true, no authority operations are available.
        /// But if the server is running as super mode, all operations will work.
        /// </summary>
        public bool DisableAuthorityCertificate { get; set; }

        /// <summary>
        /// Certificate to authorize to the server.
        /// </summary>
        public Certificate Certificate { get; set; }

        /// <summary>
        /// Server Certificate to validate the server's identity.
        /// </summary>
        public Certificate ServerCertificate { get; set; }

        /// <summary>
        /// Cache Repository.
        /// if null, the executor will use default implementation.
        /// </summary>
        public ICertificateCacheRepository CacheRepository { get; set; }

        /// <summary>
        /// Mode how to connect.
        /// </summary>
        public RemoteCommandExecutorMode Mode { get; set; } = RemoteCommandExecutorMode.Https;

        /// <summary>
        /// Timeout of HTTPS requests,
        /// WebSocket's keep-alive interval.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(15);

        /// <summary>
        /// Throw an exception if invalid.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public void ThrowExceptionIfInvalid()
        {
            if (ServerUri is null)
                throw new ArgumentException("no server uri specified.");

            if (!DisableAuthorityCertificate)
            {
                if (string.IsNullOrWhiteSpace(ServerUri.Scheme) || (
                    ServerUri.Scheme.ToLower() != "https" && ServerUri.Scheme.ToLower() != "wss"))
                    throw new ArgumentException("no insecure connection allowed.");

                //http://ocsp.powercrush.kr/api/infra/live

                if (Certificate is null)
                    throw new ArgumentException("no certificate specified.");

                if (Certificate.HasPrivateKey == false)
                    throw new ArgumentException("no private key exists on specified certificate.");
            }
        }
    }
}
