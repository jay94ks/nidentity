using NIdentity.Core.Commands;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Permissions
{
    /// <summary>
    /// List Permissions command.
    /// </summary>
    [CommandHandler(typeof(X509ListPermissionsCommand), Kind = "x509")]
    public class X509ListPermissionsCommandHandler : X509CertificateCommandHandler<X509ListPermissionsCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509ListPermissionsCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509ListPermissionsCommandHandler(X509RequesterAccesor Requester) : base(Requester)
        {
        }

        /// <inheritdoc/>
        public override async Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;
            var Certificate = null as Certificate;

            if (Requester is null && !IsSuperAccess)
                throw new AccessViolationException("no `list` permission granted for unauthorized accesses.");

            if (Request.ByReference.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByReference, Aborter);

            else if (Request.ByIdentity.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByIdentity, Aborter);

            if (Certificate is null)
                throw new ArgumentException("no such certificate exists.");

            // --> check `list` permission granted or not.
            var IsIssuer = await Context.Repository.IsIssuerAsync(Requester, Certificate, Aborter);
            var Perms = await Context.Permissions.QueryAsync(Requester.Self, Certificate.Self, Aborter);
            if (Perms != null)
            {
                if (IsIssuer == false && Perms.CanAlter == false)
                    throw new ArgumentException("no permission granted to list permissions.");

                var IsSelf = Requester.Self.IsExact(Certificate);
                if (IsSelf == false && IsIssuer == true && Perms.CanAuthorityInterfere == true)
                    throw new ArgumentException("no interfere allowed to the sub authority.");
            }

            // --> if no permission exists, this refers certificate tree.
            else if (!IsSuperAccess && IsIssuer == false)
                throw new AccessViolationException("no permission to list certificates of the specified authority.");
        }
    }
}
