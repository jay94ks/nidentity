using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Results
{
    /// <summary>
    /// Command result.
    /// </summary>
    public class EndpointNetworkListResult : CommandResult
    {
        /// <summary>
        /// Total count of networks to list.
        /// </summary>
        [JsonProperty("totals")]
        public int TotalNetworks { get; set; }

        /// <summary>
        /// Networks.
        /// </summary>
        [JsonProperty("networks")]
        public EndpointNetworkInfo[] Networks { get; set; }
    }
}
