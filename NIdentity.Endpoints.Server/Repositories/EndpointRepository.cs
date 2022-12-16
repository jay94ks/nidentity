using NIdentity.Core.Server.Helpers;
using NIdentity.Endpoints.Server.Repositories.Models;
using System.Net;

namespace NIdentity.Endpoints.Server.Repositories
{
    /// <summary>
    /// Endpoint repository.
    /// </summary>
    public class EndpointRepository : IMutableEndpointRepository
    {
        private readonly EndpointContext m_Context;

        /// <summary>
        /// Initialize a new <see cref="EndpointRepository"/> instance.
        /// </summary>
        /// <param name="Context"></param>
        public EndpointRepository(EndpointContext Context)
        {
            m_Context = Context;
        }

        /// <inheritdoc/>
        public Task<Endpoint> GetAsync(Guid Inventory, IPAddress Address, CancellationToken Token = default)
        {
            if (Address is null)
                throw new ArgumentNullException(nameof(Address));

            var AddressString = Address.ToString();
            var Item = m_Context.Endpoints
                .Where(X => X.Inventory == Inventory)
                .Where(X => X.Address == AddressString)
                .FirstOrDefault();

            if (Item != null)
                return Task.FromResult(Item.Make());

            return Task.FromResult<Endpoint>(null);
        }

        /// <inheritdoc/>
        public Task<bool> AddAsync(Guid Inventory, Endpoint Endpoint, CancellationToken Token = default)
        {
            if (Endpoint is null)
                throw new ArgumentNullException(nameof(Endpoint));

            var Item = DbEndpoint.Make(Endpoint);
            Item.Inventory = Inventory;

            var Result = m_Context.DbContext.DbCreate(Item);
            return Task.FromResult(Result);
        }

        /// <inheritdoc/>
        public Task<bool> RemoveAsync(Guid Inventory, IPAddress Address, CancellationToken Token = default)
        {
            if (Address is null)
                throw new ArgumentNullException(nameof(Address));

            var AddressString = Address.ToString();
            var Item = m_Context.Endpoints
                .Where(X => X.Inventory == Inventory)
                .Where(X => X.Address == AddressString)
                .FirstOrDefault();

            if (Item != null)
            {
                var Result = m_Context.DbContext.DbRemove(Item);
                return Task.FromResult(Result);
            }

            return Task.FromResult(false);
        }

        /// <inheritdoc/>
        public Task<bool> SetCautionAsync(Guid Inventory, IPAddress Address, EndpointCautionLevel Level, CancellationToken Token = default)
        {
            if (Address is null)
                throw new ArgumentNullException(nameof(Address));

            var AddressString = Address.ToString();
            var Item = m_Context.Endpoints
                .Where(X => X.Inventory == Inventory)
                .Where(X => X.Address == AddressString)
                .FirstOrDefault();

            if (Item != null)
            {
                Item.CautionLevel = Level;
                Item.CautionTime = DateTimeOffset.UtcNow;

                var Result = m_Context.DbContext.DbUpdate(Item);
                return Task.FromResult(Result);
            }

            return Task.FromResult(false);
        }

        /// <inheritdoc/>
        public Task<bool> UpdateAsync(Guid Inventory, Endpoint Endpoint, CancellationToken Token = default)
        {
            if (Endpoint is null)
                throw new ArgumentNullException(nameof(Endpoint));

            var AddressString = Endpoint.Address.ToString();
            var Item = m_Context.Endpoints
                .Where(X => X.Inventory == Inventory)
                .Where(X => X.Address == AddressString)
                .FirstOrDefault();

            if (Item != null)
            {
                Item.Name = Endpoint.Name ?? string.Empty;
                Item.Description = Endpoint.Description ?? string.Empty;

                var Result = m_Context.DbContext.DbUpdate(Item);
                return Task.FromResult(Result);
            }

            return Task.FromResult(false);
        }
    }
}
