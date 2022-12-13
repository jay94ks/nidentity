using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Certificates
{
    [Command(Kind = "x509")]
    public class X509DeleteCertificateCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509DeleteCertificateCommand"/> instance.
        /// </summary>
        public X509DeleteCertificateCommand() : base("cert_delete")
        {
        }

        public class Result : CertificateResult<Result>
        {

        }
    }
}
