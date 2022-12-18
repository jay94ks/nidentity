namespace NIdentity.Core.X509.Server
{
    /// <summary>
    /// Mutable certificate permission manager.
    /// </summary>
    public interface IMutableCertificatePermissionManager : ICertificatePermissionManager
    {
        /// <summary>
        /// Set the permissions,
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="Permissions"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> SetAsync(CertificatePermission Permissions, CancellationToken Token = default);

        /// <summary>
        /// Unset the permission defines that accessor's permission for target.
        /// </summary>
        /// <param name="Accessor"></param>
        /// <param name="Target"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> UnsetAsync(CertificateIdentity Accessor, CertificateIdentity Target, CancellationToken Token = default);
    }
}
