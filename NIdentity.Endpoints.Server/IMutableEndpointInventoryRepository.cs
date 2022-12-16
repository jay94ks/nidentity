namespace NIdentity.Endpoints.Server
{
    public interface IMutableEndpointInventoryRepository : IEndpointInventoryRepository
    {
        /// <summary>
        /// Add an endpoint into inventory.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> AddAsync(EndpointInventory Identity, CancellationToken Token = default);

        /// <summary>
        /// Remove an endpoint from inventory.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Guid Identity, CancellationToken Token = default);

        /// <summary>
        /// Update an endpoint from inventory.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(EndpointInventory Identity, CancellationToken Token = default);

        /// <summary>
        /// Set an endpoint as caution.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Level"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> SetCautionAsync(Guid Identity, EndpointCautionLevel Level, CancellationToken Token = default);
    }

}
