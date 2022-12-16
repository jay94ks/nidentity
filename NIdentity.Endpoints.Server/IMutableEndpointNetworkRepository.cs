using System.Net;

namespace NIdentity.Endpoints.Server
{
    public interface IMutableEndpointNetworkRepository : IEndpointNetworkRepository
    {
        /// <summary>
        /// Add an endpoint into inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Endpoint"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> AddAsync(Guid Inventory, EndpointNetwork Endpoint, CancellationToken Token = default);

        /// <summary>
        /// Remove an endpoint from inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Address"></param>
        /// <param name="SubnetMask"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Guid Inventory, IPAddress Address, int SubnetMask, CancellationToken Token = default);

        /// <summary>
        /// Update an endpoint from inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Endpoint"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Guid Inventory, EndpointNetwork Endpoint, CancellationToken Token = default);

        /// <summary>
        /// Set an endpoint as caution.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Address"></param>
        /// <param name="SubnetMask"></param>
        /// <param name="Level"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> SetCautionAsync(Guid Inventory, IPAddress Address, int SubnetMask, EndpointCautionLevel Level, CancellationToken Token = default);
    }
}
