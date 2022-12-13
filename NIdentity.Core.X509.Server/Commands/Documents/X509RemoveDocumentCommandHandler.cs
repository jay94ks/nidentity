using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands.Documents;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Documents
{
    [CommandHandler(typeof(X509RemoveDocumentCommand), Kind = "x509")]
    public class X509RemoveDocumentCommandHandler : X509DocumentCommandHandler<X509RemoveDocumentCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509RemoveDocumentCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509RemoveDocumentCommandHandler(X509RequesterAccesor Requester) : base(Requester)
        {
        }

        /// <inheritdoc/>
        public override async Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;

            if (await Context.MutableRepository.RemoveAsync(Context.Document.Identity, Request.Revision, Aborter) == false)
                throw new AccessViolationException("the repository rejected to remove the document.");

            Context.Document.LastWriteTime = DateTimeOffset.UtcNow;
            return X509RemoveDocumentCommand.Result.Make(Context.Document);
        }
    }
}
