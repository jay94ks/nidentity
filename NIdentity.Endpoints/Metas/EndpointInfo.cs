using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Endpoints.Metas
{
    /// <summary>
    /// Endpoint Info.
    /// </summary>
    public class EndpointInfo
    {
        /// <summary>
        /// Type of endpoint.
        /// </summary>
        [JsonProperty("type")]
        public EndpointType Type { get; set; }

        /// <summary>
        /// IP Address.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

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
