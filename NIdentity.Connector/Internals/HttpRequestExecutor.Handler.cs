using NIdentity.Core.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DSslProtocols = System.Security.Authentication.SslProtocols;

namespace NIdentity.Connector.Internals
{
    internal partial class HttpRequestExecutor
    {
        /// <summary>
        /// Message Handler.
        /// </summary>
        private class Handler : HttpClientHandler
        {
            /// <summary>
            /// Initialize a new <see cref="Handler"/> instance.
            /// </summary>
            /// <param name="Certificate"></param>
            public Handler(Certificate Certificate)
            {
                CheckCertificateRevocationList = false;
                
                ClientCertificateOptions = ClientCertificateOption.Manual;
                ClientCertificates.Add(Certificate.ToDotNetCert());
                ServerCertificateCustomValidationCallback = (_1, _2, _3, _4) =>
                {
                    return true;
                };
            }
        }
    }
}
