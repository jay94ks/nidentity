namespace NIdentity.Core.X509.Server.Revokations
{
    public interface IMutableRevocationRepository : IRevocationRepository
    {
        /// <summary>
        /// Add a revokation entry.
        /// </summary>
        /// <param name="Authority"></param>
        /// <param name="Target"></param>
        /// <param name="Reason"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> AddRevokationAsync(Certificate Authority, CertificateReference Target, CertificateRevokeReason Reason, CancellationToken Token = default);

        /// <summary>
        /// Remove a revokation entry.
        /// </summary>
        /// <param name="Authority"></param>
        /// <param name="Target"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> RemoveRevokationAsync(Certificate Authority, CertificateReference Target, CancellationToken Token = default);
    }
}
