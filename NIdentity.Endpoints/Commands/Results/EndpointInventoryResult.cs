using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Results
{
    /// <summary>
    /// Endpoint inventory result.
    /// </summary>
    public class EndpointInventoryResult : CommandResult
    {
        /// <summary>
        /// Inventory information.
        /// </summary>
        [JsonProperty("inventory")]
        public EndpointInventoryInfo Inventory { get; set; }
    }
}
