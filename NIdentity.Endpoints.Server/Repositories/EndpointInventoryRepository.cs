using NIdentity.Core.Server.Helpers;
using NIdentity.Endpoints.Server.Repositories.Models;

namespace NIdentity.Endpoints.Server.Repositories
{
    /// <summary>
    /// Endpoint inventory repository.
    /// </summary>
    public class EndpointInventoryRepository : IMutableEndpointInventoryRepository, IEndpointInventoryRepository
    {
        private readonly EndpointContext m_Context;

        /// <summary>
        /// Initialize a new <see cref="EndpointInventoryRepository"/> instance.
        /// </summary>
        /// <param name="Context"></param>
        public EndpointInventoryRepository(EndpointContext Context)
        {
            m_Context = Context;
        }

        /// <inheritdoc/>
        public Task<EndpointInventory> GetAsync(Guid Identity, CancellationToken Token = default)
        {
            var Item = m_Context.Inventories.FirstOrDefault(X => X.Identity == Identity);
            if (Item is null)
                return Task.FromResult<EndpointInventory>(null);

            return Task.FromResult(Item.Make());
        }

        /// <inheritdoc/>
        public Task<bool> AddAsync(EndpointInventory Inventory, CancellationToken Token = default)
        {
            if (Inventory is null)
                throw new ArgumentNullException(nameof(Inventory));

            var Item = DbEndpointInventory.Make(Inventory);
            var Result = m_Context.DbContext.DbCreate(Item);
            return Task.FromResult(Result);
        }

        /// <inheritdoc/>
        public Task<bool> RemoveAsync(Guid Identity, CancellationToken Token = default)
        {
            var Item = m_Context.Inventories.FirstOrDefault(X => X.Identity == Identity);
            if (Item is null)
                return Task.FromResult(false);

            var Result = m_Context.DbContext.DbRemove(Item);
            return Task.FromResult(Result);
        }

        /// <inheritdoc/>
        public Task<bool> SetCautionAsync(Guid Identity, EndpointCautionLevel Level, CancellationToken Token = default)
        {
            var Item = m_Context.Inventories.FirstOrDefault(X => X.Identity == Identity);
            if (Item is null)
                return Task.FromResult(false);

            Item.CautionTime = DateTimeOffset.UtcNow;
            Item.CautionLevel = Level;

            var Result = m_Context.DbContext.DbUpdate(Item);
            return Task.FromResult(Result);
        }

        /// <inheritdoc/>
        public Task<bool> UpdateAsync(EndpointInventory Inventory, CancellationToken Token = default)
        {
            var Identity = Inventory.Identity;
            var Item = m_Context.Inventories.FirstOrDefault(X => X.Identity == Identity);
            if (Item is null)
                return Task.FromResult(false);

            Item.Name = Inventory.Name;
            Item.Description = Inventory.Description;
            Item.IsPublic = Inventory.IsPublic;
            Item.IsMetadataPublic = Inventory.IsMetadataPublic;
            Item.LastUpdateTime = DateTimeOffset.UtcNow;

            var Result = m_Context.DbContext.DbUpdate(Item);
            return Task.FromResult(Result);
        }
    }
}
