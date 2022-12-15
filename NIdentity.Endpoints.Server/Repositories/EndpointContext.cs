using Microsoft.EntityFrameworkCore;
using NIdentity.Core.Server.Helpers;
using NIdentity.Endpoints.Server.Repositories.Models;

namespace NIdentity.Endpoints.Server.Repositories
{
    public class EndpointContext
    {
        /// <summary>
        /// Initialize a new <see cref="EndpointContext"/> instance.
        /// </summary>
        /// <param name="DbContext"></param>
        public EndpointContext(DbContext DbContext) => this.DbContext = DbContext;

        /// <summary>
        /// DbContext.
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// Endpoints.
        /// </summary>
        public DbSet<DbEndpoint> Endpoints => DbContext.Set<DbEndpoint>();

        /// <summary>
        /// DbEndpointNetwork.
        /// </summary>
        public DbSet<DbEndpointNetwork> Networks => DbContext.Set<DbEndpointNetwork>();

        /// <summary>
        /// DbEndpointInventory.
        /// </summary>
        public DbSet<DbEndpointInventory> Inventories => DbContext.Set<DbEndpointInventory>();

        /// <summary>
        /// Configure models to <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public void Configure(ModelBuilder Mb)
        {
            Mb.ApplyNotations<EndpointContext>();
            DbEndpoint.Configure(Mb);
            DbEndpointNetwork.Configure(Mb);
            DbEndpointInventory.Configure(Mb);
        }
    }
}
