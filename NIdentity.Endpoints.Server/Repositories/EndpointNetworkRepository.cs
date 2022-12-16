using NIdentity.Core.Server.Helpers;
using NIdentity.Endpoints.Server.Helpers;
using NIdentity.Endpoints.Server.Repositories.Models;
using System.Net;

namespace NIdentity.Endpoints.Server.Repositories
{
    /// <summary>
    /// Endpoint network repository.
    /// </summary>
    public class EndpointNetworkRepository : IMutableEndpointNetworkRepository, IEndpointNetworkRepository
    {
        private readonly EndpointContext m_Context;

        /// <summary>
        /// Initialize a new <see cref="EndpointNetworkRepository"/> instance.
        /// </summary>
        /// <param name="Context"></param>
        public EndpointNetworkRepository(EndpointContext Context)
        {
            m_Context = Context;
        }

        /// <inheritdoc/>
        public Task<EndpointNetwork> GetAsync(Guid Inventory, IPAddress Address, int SubnetMask, CancellationToken Token = default)
        {
            if (Address is null)
                throw new ArgumentNullException(nameof(Address));

            var AddressString = Address.ToDotBytes();
            var Item = m_Context.Networks
                .Where(X => X.Inventory == Inventory)
                .Where(X => X.Address == AddressString && X.SubnetMask == SubnetMask)
                .FirstOrDefault();

            if (Item != null)
                return Task.FromResult(Item.Make());

            return Task.FromResult<EndpointNetwork>(null);
        }

        /// <inheritdoc/>
        public Task<EndpointNetwork[]> MatchAsync(Guid Inventory, IPAddress Address, int SubnetMask, CancellationToken Token = default)
        {
            if (Address is null)
                throw new ArgumentNullException(nameof(Address));

            if (SubnetMask > 0)
            {
                var Prefix = Address.ToDotBytes(SubnetMask, out var Remainder) + ".";
                var Items = m_Context.Networks
                    .Where(X => X.Inventory == Inventory)
                    .Where(X => X.Address.StartsWith(Prefix) && X.SubnetMask >= SubnetMask)
                    .AsEnumerable().Select(X => X.Make())
                    .ToArray();

                return Task.FromResult(Items);
            }

            else
            {
                var Items = m_Context.Networks
                    .Where(X => X.Inventory == Inventory)
                    .AsEnumerable().Select(X => X.Make())
                    .ToArray();

                return Task.FromResult(Items);
            }
        }

        /// <inheritdoc/>
        public Task<bool> AddAsync(Guid Inventory, EndpointNetwork Network, CancellationToken Token = default)
        {
            if (Network is null)
                throw new ArgumentNullException(nameof(Network));

            if (Network.Address is null)
                throw new ArgumentNullException(nameof(Network.Address));

            if (Network.SubnetMask < 0)
                throw new ArgumentException("Subnet mask should be zero or higher than zero.");

            var Item = DbEndpointNetwork.Make(Network);
            Item.Inventory = Inventory;

            var Result = m_Context.DbContext.DbCreate(Item);
            return Task.FromResult(Result);
        }

        /// <inheritdoc/>
        public Task<bool> RemoveAsync(Guid Inventory, IPAddress Address, int SubnetMask, CancellationToken Token = default)
        {
            if (Address is null)
                throw new ArgumentNullException(nameof(Address));

            if (SubnetMask < 0)
                throw new ArgumentException("Subnet mask should be zero or higher than zero.");

            var AddressString = Address.ToDotBytes();
            var Item = m_Context.Networks
                .Where(X => X.Inventory == Inventory)
                .Where(X => X.Address == AddressString)
                .Where(X => X.SubnetMask == SubnetMask)
                .FirstOrDefault();

            if (Item != null)
            {
                var Result = m_Context.DbContext.DbRemove(Item);
                return Task.FromResult(Result);
            }

            return Task.FromResult(false);
        }

        /// <inheritdoc/>
        public Task<bool> SetCautionAsync(Guid Inventory, IPAddress Address, int SubnetMask, EndpointCautionLevel Level, CancellationToken Token = default)
        {
            if (Address is null)
                throw new ArgumentNullException(nameof(Address));

            if (SubnetMask < 0)
                throw new ArgumentException("Subnet mask should be zero or higher than zero.");

            var AddressString = Address.ToDotBytes();
            var Item = m_Context.Networks
                .Where(X => X.Inventory == Inventory)
                .Where(X => X.Address == AddressString)
                .Where(X => X.SubnetMask == SubnetMask)
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
        public Task<bool> UpdateAsync(Guid Inventory, EndpointNetwork Network, CancellationToken Token = default)
        {
            if (Network is null)
                throw new ArgumentNullException(nameof(Network));

            if (Network.Address is null)
                throw new ArgumentNullException(nameof(Network.Address));

            if (Network.SubnetMask < 0)
                throw new ArgumentException("Subnet mask should be zero or higher than zero.");

            var AddressString = Network.Address.ToDotBytes();
            var SubnetMask = Network.SubnetMask;
            var Item = m_Context.Networks
                .Where(X => X.Inventory == Inventory)
                .Where(X => X.Address == AddressString)
                .Where(X => X.SubnetMask == SubnetMask)
                .FirstOrDefault();

            if (Item != null)
            {
                Item.Type = Network.Type;
                Item.Name = Network.Name ?? string.Empty;
                Item.Description = Network.Description ?? string.Empty;

                var Result = m_Context.DbContext.DbUpdate(Item);
                return Task.FromResult(Result);
            }

            return Task.FromResult(false);
        }
    }
}
