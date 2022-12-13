﻿using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Server.Commands.Bases;
using NIdentity.Core.X509.Server.Revokations;

namespace NIdentity.Core.X509.Server.Commands.Certificates
{
    [CommandHandler(typeof(X509RevokeCertificateCommand), Kind = "x509")]
    public class X509RevokeCertificateCommandHandler : X509CertificateCommandHandler<X509RevokeCertificateCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509RevokeCertificateCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509RevokeCertificateCommandHandler(X509RequesterAccesor Requester) : base(Requester)
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
            var RevokationRepository = Context.Services.GetRequiredService<IMutableRevocationRepository>();
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;
            var Certificate = null as Certificate;

            if (Request.ByReference.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByReference, Aborter);

            else if (Request.ByIdentity.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByIdentity, Aborter);

            if (Certificate is null)
                throw new ArgumentException("no such certificate exists.");

            if (Certificate.RevokeReason.HasValue)
                throw new InvalidOperationException("the certificate is already revoked.");

            if (!await Context.MutableRepository.RevokeAsync(Certificate, Request.RevokeReason, Aborter))
                throw new InvalidOperationException("the repository rejected to alter revokation status.");

            try
            {
                if (Certificate.IsSelfSigned)
                    await RevokationRepository.AddRevokationAsync(Certificate, Certificate, Request.RevokeReason, Aborter);

                else
                {
                    var Authority = await Context.Repository.LoadAsync(Certificate.Issuer, Aborter);
                    if (Authority != null)
                        await RevokationRepository.AddRevokationAsync(Authority, Certificate, Request.RevokeReason, Aborter);
                }
            }
            catch
            {
                await Context.MutableRepository.RevokeAsync(Certificate, CertificateRevokeReason.None);
                throw;
            }

            if (Request.ByReference.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByReference, Aborter);

            else if (Request.ByIdentity.Validity)
                Certificate = await Context.Repository.LoadAsync(Request.ByIdentity, Aborter);

            return X509RevokeCertificateCommand.Result.Make(Certificate);
        }
    }
}
