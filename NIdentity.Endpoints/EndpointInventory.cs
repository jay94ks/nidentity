using NIdentity.Core.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Endpoints
{
    /// <summary>
    /// Endpoint Inventory.
    /// </summary>
    public class EndpointInventory
    {
        /// <summary>
        /// Inventory identity.
        /// </summary>
        public Guid Identity { get; set; }

        /// <summary>
        /// Owner of this inventory.
        /// </summary>
        public CertificateIdentity Owner { get; set; }

        /// <summary>
        /// Name of this inventory.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of this inventory.
        /// </summary>
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

        /// <summary>
        /// Indicates whether this inventory is makred as caution or not.
        /// </summary>
        public bool IsCautionMarked => CautionLevel != EndpointCautionLevel.Unspecified;
    }
}
