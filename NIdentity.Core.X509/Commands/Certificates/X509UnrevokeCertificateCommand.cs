using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Certificates
{
    [Command(Kind = "x509")]
    public class X509UnrevokeCertificateCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509UnrevokeCertificateCommand"/> instance.
        /// </summary>
        public X509UnrevokeCertificateCommand() : base("cert_unrevoke")
        {
        }

        public class Result : CertificateResult<Result>
        {

        }
    }
}
