namespace NIdentity.Core.X509
{
    /// <summary>
    /// Cache repository that stores certificate instances on memory.
    /// To optimize certificate loading.
    /// </summary>
    public interface ICertificateCacheRepository
    {
        /// <summary>
        /// Delete the certificate from cache repository.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task DeleteAsync(Certificate Certificate, CancellationToken Token = default);

        /// <summary>
        /// Get the certificate by its identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Certificate> GetAsync(CertificateIdentity Identity, CancellationToken Token = default);

        /// <summary>
        /// Get the certificate by its reference.
        /// </summary>
        /// <param name="Reference"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Certificate> GetAsync(CertificateReference Reference, CancellationToken Token = default);

        /// <summary>
        /// Set the certificate to cache repository.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <param name="Action"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task SetAsync(Certificate Certificate, Action<Certificate> Action, CancellationToken Token = default);

        /// <summary>
        /// Set the certificate to cache repository.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task SetAsync(Certificate Certificate, CancellationToken Token = default);
    }
}