using NIdentity.Core.X509.Documents;

namespace NIdentity.Core.X509.Server.Documents
{
    public interface IDocumentRepository
    {
        /// <summary>
        /// List all owned 
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="PathName"></param>
        /// <param name=""></param>
        /// <returns></returns>
        Task<DocumentList> ListAsync(CertificateReference Owner, string PathName = "/", CancellationToken Token = default);

        /// <summary>
        /// Get the <see cref="Document"/> asynchronously.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Document> ReadAsync(DocumentIdentity Identity, long? RevisionNumber = null, CancellationToken Token = default);
    }
}
