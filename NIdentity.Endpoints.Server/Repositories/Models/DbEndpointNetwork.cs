using Microsoft.EntityFrameworkCore;
using NIdentity.Core.Server.Helpers.Efcores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace NIdentity.Endpoints.Server.Repositories.Models
{
    [Table("EndpointNetworks")]
    public class DbEndpointNetwork
    {
        /// <summary>
        /// Configure the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public static void Configure(ModelBuilder Mb)
        {
            var Entity = Mb.Entity<DbEndpointNetwork>();

            Entity
                .HasKey(nameof(Inventory), nameof(Address), nameof(SubnetMask))
                .HasName("PK_EP");

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
        /// (PK) Subnet mask.
        /// </summary>
        public int SubnetMask { get; set; }

        /// <summary>
        /// Type of network.
        /// </summary>
        public EndpointNetworkType Type { get; set; } = EndpointNetworkType.Unknown;

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
        public static DbEndpointNetwork Make(EndpointNetwork Endpoint) => new DbEndpointNetwork
        {
            Address = Endpoint.Address.ToString(),
            SubnetMask = Endpoint.SubnetMask,
            Type = Endpoint.Type,
            Name = Endpoint.Name,
            Description = Endpoint.Description,
            CautionTime = Endpoint.CautionTime,
            CautionLevel = Endpoint.CautionLevel
        };

        /// <summary>
        /// Make <see cref="Endpoint"/> instance from this.
        /// </summary>
        /// <returns></returns>
        public EndpointNetwork Make() => new EndpointNetwork
        {
            Address = IPAddress.Parse(Address),
            SubnetMask = SubnetMask,
            Type = Type,
            Name = Name,
            Description = Description,
            CautionTime = CautionTime,
            CautionLevel = CautionLevel
        };
    }
}
