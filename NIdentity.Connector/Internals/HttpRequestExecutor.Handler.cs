using NIdentity.Core.X509;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NIdentity.Connector.Internals
{
    internal partial class HttpRequestExecutor
    {
        /// <summary>
        /// Message Handler.
        /// </summary>
        internal class Handler : HttpClientHandler
        {
            /// <summary>
            /// Initialize a new <see cref="Handler"/> instance.
            /// </summary>
            /// <param name="Certificate"></param>
            /// <param name="ServerCertificate"></param>
            public Handler(Certificate Certificate, Certificate ServerCertificate)
            {
                CheckCertificateRevocationList = false;
                
                ClientCertificateOptions = ClientCertificateOption.Manual;
                ClientCertificates.Add(Certificate.ToDotNetCert());
                ServerCertificateCustomValidationCallback = (_1, ReceivedCertificate, _3, Error) =>
                {
                    return ChcekServerCertificate(ServerCertificate, ReceivedCertificate, Error);
                };
            }

            /// <summary>
            /// Check the server certificate.
            /// </summary>
            /// <param name="ServerCertificate"></param>
            /// <param name="ReceivedCertificate"></param>
            /// <param name="Error"></param>
            /// <returns></returns>
            internal static bool ChcekServerCertificate(Certificate ServerCertificate, X509Certificate ReceivedCertificate, SslPolicyErrors Error)
            {
                if (Error == SslPolicyErrors.None || ServerCertificate is null)
                    return true;

                if (ReceivedCertificate is null)
                    return false;

                try
                {
                    var Converted = Certificate.Import(ReceivedCertificate.Export(X509ContentType.Cert));
                    if (Converted is null)
                        return false;

                    return Converted.KeyIdentifier == ServerCertificate.KeyIdentifier
                        && Converted.SerialNumber == ServerCertificate.SerialNumber
                        && Converted.Issuer == ServerCertificate.Issuer
                        && Converted.Thumbprint == ServerCertificate.Thumbprint;
                }
                catch
                {
                }

                return false;
            }
        }
    }
}
