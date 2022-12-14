using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Certificates
{
    /// <summary>
    /// A command to unrevoke a certificate.
    /// </summary>
    [Command(Kind = "x509")]
    public class X509UnrevokeCertificateCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509UnrevokeCertificateCommand"/> instance.
        /// </summary>
        public X509UnrevokeCertificateCommand() : base("cert_unrevoke")
        {
        }

        /// <summary>
        /// Unrevokation result.
        /// </summary>
        public class Result : CertificateResult<Result>
        {

        }
    }
}
