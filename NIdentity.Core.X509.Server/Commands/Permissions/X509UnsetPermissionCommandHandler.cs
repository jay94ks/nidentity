using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands.Permissions;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Permissions
{
    /// <summary>
    /// Unset Permissions command.
    /// </summary>
    [CommandHandler(typeof(X509UnsetPermissionCommand), Kind = "x509")]
    public class X509UnsetPermissionCommandHandler : X509CertificateCommandHandler<X509UnsetPermissionCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509SetPermissionCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509UnsetPermissionCommandHandler(X509RequesterAccesor Requester) : base(Requester)
        {
        }

        /// <inheritdoc/>
        public override async Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;
            var Certificate = null as Certificate;

            if (Requester is null && !IsSuperAccess)
                throw new AccessViolationException("no `unset` permission granted for unauthorized accesses.");

            if (Request.ByReference.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByReference, Aborter);

            else if (Request.ByIdentity.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByIdentity, Aborter);

            if (Certificate is null)
                throw new ArgumentException("no such certificate exists.");

            // --> check `set` permission granted or not.
            var IsSelf = Requester.Self.IsExact(Certificate);
            var IsIssuer = await Context.Repository.IsIssuerAsync(Requester, Certificate, Aborter);
            var Perms = await Context.Permissions.QueryAsync(Requester.Self, Certificate.Self, Aborter);
            if (Perms != null)
            {
                if (IsIssuer == false && Perms.CanAlter == false)
                    throw new ArgumentException("no permission granted to alter permissions.");

                if (IsSelf == false && IsIssuer == true && Perms.CanAuthorityInterfere == false)
                    throw new ArgumentException("no interfere allowed to the sub authority.");
            }

            // --> if no permission exists, this refers certificate tree.
            else if (!IsSuperAccess && IsIssuer == false)
                throw new AccessViolationException("no permission to alter certificates of the specified authority.");

            var Success = await Context.MutablePermissions.UnsetAsync(Request.ByAccessorIdentity, Request.ByIdentity, Aborter);
            if (Success == false)
                throw new InvalidOperationException("the repository denied to alter the permission.");

            return new CommandResult { Success = true };
        }
    }
}
