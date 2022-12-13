using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands.Documents;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Documents
{
    [CommandHandler(typeof(X509ReadDocumentCommand), Kind = "x509")]
    public class X509ReadDocumentCommandHandler : X509DocumentCommandHandler<X509ReadDocumentCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509ReadDocumentCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509ReadDocumentCommandHandler(X509RequesterAccesor Requester) : base(Requester)
        {
        }

        /// <inheritdoc/>
        public override Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;

            if (Request.Revision.HasValue &&
                Context.Document.RevisionNumber != Request.Revision.Value)
                throw new ArgumentException("no revision number matched.");

            var Document = X509ReadDocumentCommand.Result.Make(
                Context.Document, X => X.Data = Context.Document.Data);

            return Task.FromResult<CommandResult>(Document);
        }
    }
}
