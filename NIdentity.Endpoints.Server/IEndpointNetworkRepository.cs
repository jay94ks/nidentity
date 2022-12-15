using System.Net;

namespace NIdentity.Endpoints.Server
{
    public interface IEndpointNetworkRepository
    {
        /// <summary>
        /// Get the exact endpoint from repository.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Address"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<EndpointNetwork> GetAsync(Guid Inventory, IPAddress Address, int SubnetMask, CancellationToken Token = default);

        /// <summary>
        /// Get matched networks from repository.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Address"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<EndpointNetwork[]> MatchAsync(Guid Inventory, IPAddress Address, int SubnetMask, CancellationToken Token = default);
    }
}
