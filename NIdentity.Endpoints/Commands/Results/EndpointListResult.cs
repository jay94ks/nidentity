using Newtonsoft.Json;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Results
{
    /// <summary>
    /// Command result.
    /// </summary>
    public class EndpointListResult : EndpointNetworkResult
    {
        /// <summary>
        /// Total count of endpoints to list.
        /// </summary>
        [JsonProperty("totals")]
        public int TotalEndpoints { get; set; }

        /// <summary>
        /// Endpoint who queried.
        /// </summary>
        [JsonProperty("endpoints")]
        public EndpointInfo[] Endpoints { get; set; }
    }
}
