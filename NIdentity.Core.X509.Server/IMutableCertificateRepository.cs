namespace NIdentity.Core.X509.Server
{
    /// <summary>
    /// Mutable Certificate Repository.
    /// </summary>
    public interface IMutableCertificateRepository : ICertificateRepository
    {
        /// <summary>
        /// Store the certificate to the repository asynchronously.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <param name="ExcludePrivateKey"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> StoreAsync(Certificate Certificate, bool ExcludePrivateKey = false, CancellationToken Token = default);

        /// <summary>
        /// Revoke the certificate from the repository asynchronously.
        /// This will not revoke if the certificate is not on the repository.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <param name="Reason"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> RevokeAsync(CertificateReference Identity, CertificateRevokeReason Reason, CancellationToken Token = default);

        /// <summary>
        /// Delete the certificate from the repository asynchronously.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(CertificateReference Identity, CancellationToken Token = default);
    }


}
