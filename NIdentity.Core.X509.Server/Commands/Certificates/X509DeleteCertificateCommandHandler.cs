using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Certificates
{
    [CommandHandler(typeof(X509DeleteCertificateCommand), Kind = "x509")]
    public class X509DeleteCertificateCommandHandler : X509CertificateCommandHandler<X509DeleteCertificateCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509DeleteCertificateCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509DeleteCertificateCommandHandler(X509RequesterAccesor Requester) : base(Requester)
        {
        }

        /// <summary>
        /// Get the X509 certificate.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;
            var Certificate = null as Certificate;

            if (Request.ByReference.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByReference, Aborter);

            else if (Request.ByIdentity.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByIdentity, Aborter);

            if (Certificate is null)
                throw new ArgumentException("no such certificate exists.");


            await CheckPermission(Context, Certificate, Aborter);

            if (!await Context.MutableRepository.DeleteAsync(Certificate, Aborter))
                throw new InvalidOperationException("the repository rejected to delete certificate.");

            return X509DeleteCertificateCommand.Result.Make(Certificate);
        }

        /// <summary>
        /// Check permission to delete certificate.
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Certificate"></param>
        /// <param name="Aborter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="AccessViolationException"></exception>
        private async Task CheckPermission(X509CommandContext Context, Certificate Certificate, CancellationToken Aborter)
        {
            // --> check revoke permission granted or not.
            var IsIssuer = await Context.Repository.IsIssuerAsync(Requester, Certificate, Aborter);
            var Perms = await Context.Permissions.QueryAsync(Requester.Self, Certificate.Self, Aborter);
            if (Perms != null)
            {
                if (IsIssuer == false && Perms.CanDelete == false)
                    throw new ArgumentException("no permission granted to delete certificates.");

                var IsSelf = Requester.Self.IsExact(Certificate);
                if (IsSelf == false && IsIssuer == true && Perms.CanAuthorityInterfere == true)
                    throw new ArgumentException("no interfere allowed to the sub authority.");
            }

            else if (!IsSuperAccess && IsIssuer == false)
                throw new AccessViolationException("no permission to delete the specified certificate.");
        }
    }
}
