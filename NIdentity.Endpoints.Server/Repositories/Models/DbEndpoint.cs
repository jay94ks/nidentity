using Microsoft.EntityFrameworkCore;
using NIdentity.Core.Server.Helpers.Efcores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace NIdentity.Endpoints.Server.Repositories.Models
{
    /// <summary>
    /// Db Endpoint.
    /// </summary>
    [Table("Endpoints")]
    public class DbEndpoint
    {
        /// <summary>
        /// Configure the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public static void Configure(ModelBuilder Mb)
        {
            var Entity = Mb.Entity<DbEndpoint>();

            Entity.HasKey(nameof(Inventory), nameof(Address)).HasName("PK_EP");
            Entity.HasIndex(X => X.Address, "BY_ADDR");
            Entity.HasIndex(X => X.Type, "BY_TYPE");
            Entity.HasIndex(X => X.Name, "BY_NAME");
            Entity.HasIndex(X => X.CautionLevel, "BY_CAUTION");
        }

        /// <summary>
        /// (PK) Inventory Guid.
        /// </summary>
        [GuidAsString]
        public Guid Inventory { get; set; }

        /// <summary>
        /// (PK) IP Address.
        /// </summary>
        [MaxLength(255)]
        public string Address { get; set; }

        /// <summary>
        /// Type of endpoint.
        /// </summary>
        public EndpointType Type { get; set; }

        /// <summary>
        /// Name of this endpoint.
        /// </summary>
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Description of this endpoint.
        /// </summary>
        [MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Time when this endpoint is marked as specified caution level.
        /// </summary>
        public DateTimeOffset? CautionTime { get; set; }

        /// <summary>
        /// Caution level.
        /// </summary>
        public EndpointCautionLevel CautionLevel { get; set; }

        /// <summary>
        /// Make <see cref="DbEndpoint"/> from <see cref="Endpoint"/>.
        /// This will not set <see cref="Inventory"/>.
        /// </summary>
        /// <param name="Endpoint"></param>
        /// <returns></returns>
        public static DbEndpoint Make(Endpoint Endpoint) => new DbEndpoint
        {
            Type = Endpoint.Type,
            Address = Endpoint.Address.ToString(),
            Name = Endpoint.Name,
            Description = Endpoint.Description,
            CautionTime = Endpoint.CautionTime,
            CautionLevel = Endpoint.CautionLevel
        };

        /// <summary>
        /// Make <see cref="Endpoint"/> instance from this.
        /// </summary>
        /// <returns></returns>
        public Endpoint Make() => new Endpoint
        {
            Type = Type,
            Address = IPAddress.Parse(Address),
            Name = Name,
            Description = Description,
            CautionTime = CautionTime,
            CautionLevel = CautionLevel
        };
    }
}
