using Newtonsoft.Json;

namespace NIdentity.Endpoints.Metas
{
    /// <summary>
    /// Endpoint inventory info.
    /// </summary>
    public class EndpointInventoryInfo
    {
        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("identity")]
        public Guid Identity { get; set; }

        /// <summary>
        /// Owner of the inventory.
        /// </summary>
        [JsonProperty("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// Owner's key identifier of the inventory.
        /// </summary>
        [JsonProperty("owner_key_id")]
        public string OwnerKeyIdentifier { get; set; }

        /// <summary>
        /// Name of this inventory.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of this inventory.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether this inventory is public or not.
        /// If this set true, the inventory identity can be readable from unauthorized requesters.
        /// </summary>
        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        /// <summary>
        /// Indicates whether metadatas are public or not.
        /// If this set false, the inventory will remove metadata from response.
        /// like name, description, caution time thats are managemental informations.
        /// For networks, the informations will be merged to single and subnet mask will replaced to -1 (invalid).
        /// </summary>
        [JsonProperty("is_metadata_public")]
        public bool IsMetadataPublic { get; set; }

        /// <summary>
        /// Time when this inventory modified.
        /// </summary>
        [JsonProperty("creation_time")]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Time when this inventory modified.
        /// </summary>
        [JsonProperty("last_update_time")]
        public DateTimeOffset LastUpdateTime { get; set; }

        /// <summary>
        /// Time when this endpoint is marked as specified caution level.
        /// </summary>
        [JsonProperty("caution_time")]
        public DateTimeOffset? CautionTime { get; set; }

        /// <summary>
        /// Caution level.
        /// </summary>
        [JsonProperty("caution_level")]
        public EndpointCautionLevel CautionLevel { get; set; }
    }
}
