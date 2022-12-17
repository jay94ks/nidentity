using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Server.Commands.Permissions
{
    /// <summary>
    /// A command to list permissions of certificate.
    /// </summary>
    [Command(Kind = "x509")]
    public class X509ListPermissionsCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509ListPermissionsCommand"/> instance.
        /// </summary>
        public X509ListPermissionsCommand() : base("cert_list_perms")
        {
        }
    }
}
