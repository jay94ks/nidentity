using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands.Permissions;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Permissions
{
    /// <summary>
    /// Set Permissions command.
    /// </summary>
    [CommandHandler(typeof(X509SetPermissionCommand), Kind = "x509")]
    public class X509SetPermissionCommandHandler : X509CertificateCommandHandler<X509SetPermissionCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509SetPermissionCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509SetPermissionCommandHandler(X509RequesterAccesor Requester) : base(Requester)
        {
        }

        /// <inheritdoc/>
        public override async Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;
            var Certificate = null as Certificate;

            if (Requester is null && !IsSuperAccess)
                throw new AccessViolationException("no `set` permission granted for unauthorized accesses.");

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

            var OldPerm = await Context.Permissions.GetAsync(Request.ByAccessorIdentity, Request.ByIdentity, Aborter);
            var NewPerm = MakeNewPerm(Request, OldPerm, IsSelf);

            if (await Context.MutablePermissions.SetAsync(NewPerm, Aborter) == false)
                throw new InvalidOperationException("the repository denied to alter the permission.");

            return new CommandResult { Success = true };
        }

        /// <summary>
        /// Make a new permission.
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="IsSelf"></param>
        /// <param name="OldPerm"></param>
        /// <returns></returns>
        /// <exception cref="AccessViolationException"></exception>
        private CertificatePermission MakeNewPerm(X509SetPermissionCommand Request, CertificatePermission OldPerm, bool IsSelf)
        {
            var NewPerm = new CertificatePermission
            {
                Owner = Request.ByIdentity,
                Accessor = Request.ByAccessorIdentity,
                CanAuthorityInterfere = true,
                CanGenerate = OldPerm != null ? OldPerm.CanGenerate : false,
                CanList = OldPerm != null ? OldPerm.CanList : true,
                CanAlter = OldPerm != null ? OldPerm.CanAlter : false,
                CanDelete = OldPerm != null ? OldPerm.CanDelete : false,
                CanRevoke = OldPerm != null ? OldPerm.CanRevoke : false
            };

            if (Request.CanAuthorityInterfere.HasValue == false)
            {
                if (NewPerm.CanAuthorityInterfere == false && IsSelf == false)
                    throw new AccessViolationException("authority's interfere option can only be altered by owner.");

                NewPerm.CanAuthorityInterfere = Request.CanAuthorityInterfere.Value;
            }

            if (Request.CanGenerate.HasValue == false)
                NewPerm.CanGenerate = Request.CanGenerate.Value;

            if (Request.CanList.HasValue == false)
                NewPerm.CanList = Request.CanList.Value;

            if (Request.CanAlter.HasValue == false)
                NewPerm.CanAlter = Request.CanAlter.Value;

            if (Request.CanDelete.HasValue == false)
                NewPerm.CanDelete = Request.CanDelete.Value;

            if (Request.CanRevoke.HasValue == false)
                NewPerm.CanRevoke = Request.CanRevoke.Value;

            return NewPerm;
        }
    }
}
