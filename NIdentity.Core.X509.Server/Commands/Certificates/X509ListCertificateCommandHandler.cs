using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Certificates
{
    [CommandHandler(typeof(X509ListCertificateCommand), Kind = "x509")]
    public class X509ListCertificateCommandHandler : X509CertificateCommandHandler<X509ListCertificateCommand>
    {
        private readonly X509ExecutorSettings m_Settings;

        /// <summary>
        /// Initialize a new <see cref="X509ListCertificateCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509ListCertificateCommandHandler(
            X509RequesterAccesor Requester,
            X509ExecutorSettings Settings) : base(Requester) => m_Settings = Settings;

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
            var Authority = null as Certificate;

            if (Requester is null && !IsSuperAccess)
                throw new AccessViolationException("no `list` permission granted for unauthorized accesses.");

            if (Request.Offset < 0)
                throw new ArgumentException("the request offset should be zero or greater than zero.");

            if (Request.Count <= 0)
                throw new ArgumentException("the request count should not be zero or less than zero.");

            if (Request.ByReference.Validity)
                Authority = await Context.Repository.LoadAsync(Request.ByReference, Aborter);

            else if (Request.ByIdentity.Validity)
                Authority = await Context.Repository.LoadAsync(Request.ByIdentity, Aborter);

            if (Authority is null)
                throw new ArgumentException("no such authority exists.");

            if (Authority.Type == CertificateType.Leaf)
                throw new ArgumentException("the specified certificate is not authority.");

            if (!IsSuperAccess && !await Context.Repository.IsIssuerAsync(Requester, Authority, Aborter))
                throw new AccessViolationException("no permission to list certificates of the specified authority.");

            var Count = Math.Min(m_Settings.MaxCountPerListRequest, Request.Count);
            var Result = new X509ListCertificateCommand.Result();
            var Certificates = await Context.Repository.FindAsync(Authority, Request.Offset, Count, Aborter);

            Result.Authority = X509CertificateAccessCommand.CertificateResult.Make(Authority);
            Result.TotalItems = 0;

            if (Certificates is null || Certificates.Length <= 0)
                Result.Items = new X509CertificateAccessCommand.CertificateResult[0];

            else
            {
                if (Certificates.Length >= Count)
                    Result.TotalItems = await Context.Repository.CountAsync(Authority, Aborter);

                else
                    Result.TotalItems = Certificates.Length;

                Result.Items = Certificates.Select(X => X509CertificateAccessCommand.CertificateResult.Make(X)).ToArray();
            }

            return Result;
        }
    }
}
