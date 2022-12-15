using System.Net;

namespace NIdentity.Endpoints.Server
{
    public interface IEndpointRepository
    {
        /// <summary>
        /// Get the exact endpoint from repository.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Address"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<Endpoint> GetAsync(Guid Inventory, IPAddress Address, CancellationToken Token = default);
    }

}
