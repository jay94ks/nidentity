using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands.Documents;
using NIdentity.Core.X509.Documents;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Documents
{
    [CommandHandler(typeof(X509WriteDocumentCommand), Kind = "x509")]
    public class X509WriteDocumentCommandHandler : X509DocumentCommandHandler<X509WriteDocumentCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509ReadDocumentCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509WriteDocumentCommandHandler(X509RequesterAccesor Requester) : base(Requester)
        {
        }

        /// <inheritdoc/>
        public override async Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;

            var IsNewDocument = Context.Document is null;
            if (IsNewDocument)
            {
                Context.Document = new Document
                {
                    Identity = new DocumentIdentity(Context.Owner.Self, Request.PathName),
                    Access = DocumentAccess.Private,
                    MimeType = "text/plain",
                    Data = Request.Data
                };
            }

            if (Request.PreviousRevision.HasValue && IsNewDocument == false &&
                Context.Document.RevisionNumber != Request.PreviousRevision.Value)
                throw new ArgumentException("no revision number matched.");

            if (Request.Access.HasValue)
            {
                if (IsSuperAccess == false)
                {
                    if (Request.Access.Value == DocumentAccess.Super)
                        throw new AccessViolationException("no permission to set `Super` access mode.");

                    var IsOwnerAccess = Context.Owner.Self.IsExact(Requester);
                    if (Request.Access.Value == DocumentAccess.Private && IsOwnerAccess == false)
                        throw new AccessViolationException("no permission to set `Private` access mode.");

                    if (Request.Access.Value == DocumentAccess.Authority)
                    {
                        var IsAuthority = await Context.CertificateRepository.IsIssuerAsync(Requester, Context.Owner, Aborter);
                        if (IsAuthority == false || IsOwnerAccess == true && Requester.IsSelfSigned == false)
                            throw new AccessViolationException("no permission to set `Authority` access mode.");
                    }
                }

                Context.Document.Access = Request.Access.Value;
            }

            if (!string.IsNullOrWhiteSpace(Request.MimeType))
                Context.Document.MimeType = Request.MimeType;

            Context.Document.Data = Request.Data;

            if (await Context.MutableRepository.WriteAsync(Context.Document, Aborter) == false)
                throw new AccessViolationException("the repository rejected to write the document.");

            Context.Document = await Context.Repository.ReadAsync(new DocumentIdentity(Context.Owner.Self, Request.PathName), null, Aborter);
            return X509WriteDocumentCommand.Result.Make(Context.Document);
        }
    }
}
