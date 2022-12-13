using NIdentity.Core.X509.Documents;

namespace NIdentity.Core.X509.Server.Documents
{
    public interface IMutableDocumentRepository : IDocumentRepository
    {
        /// <summary>
        /// Post the <see cref="Document"/> asynchronously.
        /// </summary>
        /// <param name="Document"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> WriteAsync(Document Document, CancellationToken Token = default);

        /// <summary>
        /// Remove the <see cref="Document"/> by its identity asynchronously.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="ConcurrencyKey"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(DocumentIdentity Identity, long? RevisionNumber = null, CancellationToken Token = default);
    }
}
