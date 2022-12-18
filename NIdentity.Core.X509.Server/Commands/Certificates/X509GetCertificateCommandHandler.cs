using Microsoft.Extensions.DependencyInjection;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Certificates
{
    [CommandHandler(typeof(X509GetCertificateCommand), Kind = "x509")]
    public class X509GetCertificateCommandHandler : X509CertificateCommandHandler<X509GetCertificateCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509GetCertificateCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509GetCertificateCommandHandler(X509RequesterAccesor Requester) : base(Requester)
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

            if (Certificate.IsRevokeIdentified == false)
                await CheckAuthorityRevokation(Context, Certificate, Aborter);

            return X509GetCertificateCommand.Result.Make(Certificate,
                X => X.CerBase64 = Convert.ToBase64String(Certificate.Export()));
        }

        /// <summary>
        /// Checks authority's revokation.
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Certificate"></param>
        /// <param name="Aborter"></param>
        /// <returns></returns>
        private async Task CheckAuthorityRevokation(X509CommandContext Context, Certificate Certificate, CancellationToken Aborter)
        {
            if (Certificate.IsSelfSigned)
                return;

            var Issuer = Certificate.Issuer;
            var LastIssuer = Issuer;

            while (Issuer.Validity)
            {
                var Current = await Context.Repository.LoadAsync(Issuer, Aborter);
                if (Current is null)
                    break;

                if (Current.IsRevokeIdentified == true)
                {
                    Certificate.RevokeReason = Current.RevokeReason;
                    Certificate.RevokeTime = Current.RevokeTime;
                    return;
                }

                if (Current.IsSelfSigned == true)
                    return;

                LastIssuer = Issuer;
                Issuer = Current.Issuer;
            }

            if (Issuer.Validity == false && LastIssuer.Validity == true)
            {
                Certificate.RevokeReason = CertificateRevokeReason.CACompromised;
                Certificate.RevokeTime = DateTimeOffset.UtcNow;
            }
        }
    }
}
