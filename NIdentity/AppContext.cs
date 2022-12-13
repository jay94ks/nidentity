using Microsoft.EntityFrameworkCore;
using NIdentity.Core.X509.Server.Repositories;

namespace NIdentity
{
    public class AppContext : DbContext
    {
        /// <summary>
        /// Initialize a new <see cref="CoreDbContext"/> instance.
        /// </summary>
        /// <param name="Options"></param>
        public AppContext(DbContextOptions<AppContext> Options) : base(Options)
        {
            X509 = new X509Context(this);
        }

        /// <summary>
        /// X509 context.
        /// </summary>
        public X509Context X509 { get; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder Mb)
        {
            base.OnModelCreating(Mb);
            X509.Configure(Mb);
        }
    }
}
