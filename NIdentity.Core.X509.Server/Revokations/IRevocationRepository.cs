using NIdentity.Core.X509.Revokations;

namespace NIdentity.Core.X509.Server.Revokations
{
    public interface IRevocationRepository
    {
        /// <summary>
        /// Get the revokation list.
        /// </summary>
        /// <param name="Authority"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<RevokationInventory> GetInventoryAsync(Certificate Authority, CancellationToken Token = default);

        /// <summary>
        /// List revokation entries for the authority.
        /// </summary>
        /// <param name="Authority"></param>
        /// <param name="Offset"></param>
        /// <param name="Count"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Revokation[]> ListRevokationsAsync(RevokationInventory Inventory, int Offset, int Count = 20, CancellationToken Token = default);
    }
}
