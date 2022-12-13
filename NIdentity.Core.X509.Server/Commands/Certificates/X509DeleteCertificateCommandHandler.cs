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

            if (!await Context.MutableRepository.DeleteAsync(Certificate, Aborter))
                throw new InvalidOperationException("the repository rejected to delete certificate.");

            return X509DeleteCertificateCommand.Result.Make(Certificate);
        }
    }
}
