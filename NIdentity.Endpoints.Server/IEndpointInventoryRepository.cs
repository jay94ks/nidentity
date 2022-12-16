namespace NIdentity.Endpoints.Server
{
    public interface IEndpointInventoryRepository
    {
        /// <summary>
        /// Get the inventory from repository.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<EndpointInventory> GetAsync(Guid Identity, CancellationToken Token = default);
    }

}
