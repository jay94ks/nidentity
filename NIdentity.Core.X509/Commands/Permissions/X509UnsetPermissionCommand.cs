using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Permissions
{
    /// <summary>
    /// A command to unset permissions of certificate.
    /// </summary>
    [Command(Kind = "x509")]
    public class X509UnsetPermissionCommand : X509PermissionAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509UnsetPermissionCommand"/> instance.
        /// </summary>
        public X509UnsetPermissionCommand() : base("cert_unset_perm")
        {
        }
    }
}
