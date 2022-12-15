using System.Net;

namespace NIdentity.Endpoints.Server
{
    public interface IMutableEndpointRepository 
    { 
        /// <summary>
        /// Add an endpoint into inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Endpoint"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> AddAsync(Guid Inventory, Endpoint Endpoint, CancellationToken Token = default);

        /// <summary>
        /// Remove an endpoint from inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Address"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Guid Inventory, IPAddress Address, CancellationToken Token = default);

        /// <summary>
        /// Update an endpoint from inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Endpoint"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Guid Inventory, Endpoint Endpoint, CancellationToken Token = default);

        /// <summary>
        /// Set an endpoint as caution.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Address"></param>
        /// <param name="Level"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> SetCautionAsync(Guid Inventory, IPAddress Address, EndpointCautionLevel Level, CancellationToken Token = default);
    }

}
