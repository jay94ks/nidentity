using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Results
{
    /// <summary>
    /// Endpoint inventory result.
    /// </summary>
    public class EndpointInventoryListResult : CommandResult
    {
        /// <summary>
        /// Total count of endpoints to list.
        /// </summary>
        [JsonProperty("totals")]
        public int TotalEndpoints { get; set; }

        /// <summary>
        /// Inventory information.
        /// </summary>
        [JsonProperty("inventories")]
        public EndpointInventoryInfo[] Inventories { get; set; }
    }
}
