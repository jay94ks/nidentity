using Newtonsoft.Json;

namespace NIdentity.Endpoints.Metas
{
    /// <summary>
    /// Endpoint network info.
    /// </summary>
    public class EndpointNetworkInfo
    {
        /// <summary>
        /// Type of network.
        /// </summary>
        [JsonProperty("type")]
        public EndpointNetworkType Type { get; set; } = EndpointNetworkType.Unknown;

        /// <summary>
        /// IP Address.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Subnet mask.
        /// </summary>
        [JsonProperty("subnet_mask")]
        public int SubnetMask { get; set; }

        /// <summary>
        /// Name of this endpoint.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of this endpoint.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Time when this network is marked as specified caution level.
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
