namespace NIdentity.Endpoints.Server
{
    public interface IMutableEndpointInventoryRepository : IEndpointInventoryRepository
    {
        /// <summary>
        /// Add an endpoint into inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> AddAsync(EndpointInventory Inventory, CancellationToken Token = default);

        /// <summary>
        /// Remove an endpoint from inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> RemoveAsync(Guid Inventory, CancellationToken Token = default);

        /// <summary>
        /// Update an endpoint from inventory.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(EndpointInventory Inventory, CancellationToken Token = default);

        /// <summary>
        /// Set an endpoint as caution.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Level"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> SetCautionAsync(Guid Inventory, EndpointCautionLevel Level, CancellationToken Token = default);
    }

}
