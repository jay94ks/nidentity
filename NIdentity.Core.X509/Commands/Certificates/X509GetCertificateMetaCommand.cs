using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Certificates
{
    [Command(Kind = "x509")]
    public class X509GetCertificateMetaCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509GetCertificateMetaCommand"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        public X509GetCertificateMetaCommand() : base("cert_get_meta")
        {
        }

        /// <summary>
        /// Result.
        /// </summary>
        public class Result : CertificateResult<Result>
        {
        }
    }
}
