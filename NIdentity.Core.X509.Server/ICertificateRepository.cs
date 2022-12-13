using NIdentity.Core.X509.Server.Ocsp;

namespace NIdentity.Core.X509.Server
{
    /// <summary>
    /// Certificate Repository.
    /// </summary>
    public interface ICertificateRepository
    {
        /// <summary>
        /// Load certificate asynchronously.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Certificate> LoadAsync(CertificateReference Identity, CancellationToken Token = default);

        /// <summary>
        /// Load certificate by subject and its certificate identity asynchronously.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Certificate> LoadAsync(CertificateIdentity Identity, CancellationToken Token = default);

        /// <summary>
        /// Load certificate by subject and its ocsp certificate identity asynchronously.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Certificate> LoadAsync(OcspCertificateIdentity Identity, CancellationToken Token = default);

        /// <summary>
        /// Count all certificates that are issued by authority asynchronously.
        /// </summary>
        /// <param name="Authority"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<int> CountAsync(Certificate Authority, CancellationToken Token = default);

        /// <summary>
        /// Get all certificates that are issued by authority asynchronously.
        /// </summary>
        /// <param name="Authority"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Certificate[]> FindAsync(Certificate Authority, int Offset, int Count, CancellationToken Token = default);
    }
}
