namespace NIdentity.Core.X509.Server
{
    /// <summary>
    /// Certificate permission manager.
    /// </summary>
    public interface ICertificatePermissionManager
    {
        /// <summary>
        /// Query the <see cref="CertificatePermission"/> about owner for access.
        /// This will summarize the permission informations that including authority defaults.
        /// </summary>
        /// <param name="Accessor"></param>
        /// <param name="Target"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<CertificatePermission> QueryAsync(CertificateIdentity Accessor, CertificateIdentity Target, CancellationToken Token = default);

        /// <summary>
        /// Get the <see cref="CertificatePermission"/> about accessor to owner.
        /// </summary>
        /// <param name="Accessor"></param>
        /// <param name="Owner"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<CertificatePermission> GetAsync(CertificateIdentity Accessor, CertificateIdentity Owner, CancellationToken Token = default);

        /// <summary>
        /// List all permissions that configured for owner. Searching operations should be implemented by requester.
        /// This will not load default permission settings, to load default, use <see cref="GetAsync"/>.
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="Offset"></param>
        /// <param name="Count"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<CertificatePermission[]> ListAsync(CertificateIdentity Owner, int Offset, int Count = 20, CancellationToken Token = default);

    }
}
