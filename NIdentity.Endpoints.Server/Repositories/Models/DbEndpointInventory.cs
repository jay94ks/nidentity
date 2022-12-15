using Microsoft.EntityFrameworkCore;
using NIdentity.Core.Server.Helpers.Efcores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIdentity.Endpoints.Server.Repositories.Models
{
    /// <summary>
    /// Db Endpoint Inventory.
    /// </summary>
    [Table("EndpointInventories")]
    public class DbEndpointInventory
    {
        /// <summary>
        /// Configure the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public static void Configure(ModelBuilder Mb)
        {
            var Entity = Mb.Entity<DbEndpointInventory>();

            Entity.HasIndex(X => X.Owner, "BY_OWNER");
            Entity.HasIndex(X => X.OwnerKeyIdentifier, "BY_OWNER_KEY_ID");
            Entity.HasIndex(X => X.Name, "BY_NAME");
            Entity.HasIndex(X => X.IsPublic, "BY_PUBLIC");
            Entity.HasIndex(X => X.CautionLevel, "BY_CAUTION");
        }

        /// <summary>
        /// (PK) Inventory Guid.
        /// </summary>
        [Key, GuidAsString]
        public Guid Inventory { get; set; }

        /// <summary>
        /// Owner of the inventory.
        /// </summary>
        [MaxLength(255)]
        public string Owner { get; set; }

        /// <summary>
        /// Owner's key identifier of the inventory.
        /// </summary>
        [MaxLength(41)]
        public string OwnerKeyIdentifier { get; set; }

        /// <summary>
        /// Name of this inventory.
        /// </summary>
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Description of this inventory.
        /// </summary>
        [MaxLength(255)]
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether this inventory is public or not.
        /// If this set true, the inventory identity can be readable from unauthorized requesters.
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// Indicates whether metadatas are public or not.
        /// If this set false, the inventory will remove metadata from response.
        /// like name, description, caution time thats are managemental informations.
        /// For networks, the informations will be merged to single and subnet mask will replaced to -1 (invalid).
        /// </summary>
        public bool IsMetadataPublic { get; set; }

        /// <summary>
        /// Time when this inventory modified.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Time when this inventory modified.
        /// </summary>
        public DateTimeOffset LastUpdateTime { get; set; }

        /// <summary>
        /// Time when this endpoint is marked as specified caution level.
        /// </summary>
        public DateTimeOffset? CautionTime { get; set; }

        /// <summary>
        /// Caution level.
        /// </summary>
        public EndpointCautionLevel CautionLevel { get; set; }
    }
}
