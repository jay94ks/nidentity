using NIdentity.Core.X509;
using NIdentity.Core.X509.Commands.Documents;
using NIdentity.Core.X509.Documents;

namespace NIdentity.Connector.X509
{
    /// <summary>
    /// Certificate document related helpers.
    /// </summary>
    public static class X509DocumentHelpers
    {
        /// <summary>
        /// List all documents for the certificate on the specified path.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Identity"></param>
        /// <param name="Path"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static async Task<DocumentList> ListDocumentsAsync(this X509CommandExecutor Executor, CertificateIdentity Identity, string Path, CancellationToken Token = default)
        {
            var Cmd = new X509ListDocumentCommand
            {
                Subject = Identity.Subject,
                KeyIdentifier = Identity.KeyIdentifier,
                PathName = Path
            };

            var Raw = await Executor.ListDocumentsAsync(Cmd, Token);
            if (Raw.Success == false)
                throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

            if (Raw is X509ListDocumentCommand.Result Result && Raw.Success)
            {
                return new DocumentList
                {
                    PathName = Result.PathName,
                    Documents = Result.Children
                };
            }

            return null;
        }

        /// <summary>
        /// Read a document for the certificate on the specified path.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Identity"></param>
        /// <param name="Revision"></param>
        /// <param name="Path"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static async Task<Document> ReadDocumentAsync(this X509CommandExecutor Executor, CertificateIdentity Identity, string Path, long? Revision = null, CancellationToken Token = default)
        {
            var Cmd = new X509ReadDocumentCommand
            {
                Subject = Identity.Subject,
                KeyIdentifier = Identity.KeyIdentifier,
                PathName = Path, Revision = Revision
            };

            var Raw = await Executor.ReadDocumentAsync(Cmd, Token);
            if (Raw.Success == false)
                throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

            if (Raw is X509ReadDocumentCommand.Result Result && Raw.Success)
            {
                return new Document
                {
                    Identity = new DocumentIdentity(Identity, Result.PathName),
                    Access = Result.Access,
                    CreationTime = Result.CreationTime,
                    Data = Result.Data,
                    HasChildren = Result.HasChildren,
                    LastWriteTime = Result.LastWriteTime,
                    MimeType = Result.MimeType,
                    RevisionNumber = Result.Revision
                };
            }

            return null;
        }

        /// <summary>
        /// Write a document for the certificate on the specified path.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Document"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static async Task<Document> WriteDocumentAsync(this X509CommandExecutor Executor, Document Document, CancellationToken Token = default)
        {
            var Cmd = new X509WriteDocumentCommand
            {
                Subject = Document.Identity.Owner.Subject,
                KeyIdentifier = Document.Identity.Owner.KeyIdentifier,
                PathName = Document.Identity.PathName,
                PreviousRevision = Document.RevisionNumber,
                Access = Document.Access,
                MimeType = Document.MimeType,
                Data = Document.Data
            };

            var Raw = await Executor.WriteDocumentAsync(Cmd, Token);
            if (Raw.Success == false)
                throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

            if (Raw is X509WriteDocumentCommand.Result Result && Raw.Success)
            {
                Document.RevisionNumber = Result.Revision;
                Document.MimeType = Result.MimeType;
                Document.CreationTime = Result.CreationTime;
                Document.LastWriteTime = Result.LastWriteTime;
                Document.Access = Result.Access;
                return Document;
            }

            return null;
        }

        /// <summary>
        /// Write a document for the certificate on the specified path.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Identity"></param>
        /// <param name="Path"></param>
        /// <param name="Revision"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static async Task<bool> RemoveDocumentAsync(this X509CommandExecutor Executor, CertificateIdentity Identity, string Path, long? Revision = null, CancellationToken Token = default)
        {
            var Cmd = new X509RemoveDocumentCommand
            {
                Subject = Identity.Subject,
                KeyIdentifier = Identity.KeyIdentifier,
                PathName = Path, Revision = Revision
            };

            var Raw = await Executor.RemoveDocumentAsync(Cmd, Token);
            if (Raw.Success == false)
                throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

            if (Raw is X509RemoveDocumentCommand.Result Result)
                return Result.Success;

            return false;
        }
    }
}
