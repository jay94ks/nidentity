using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Results
{
    /// <summary>
    /// Command result.
    /// </summary>
    public class EndpointResult : CommandResult
    {
        /// <summary>
        /// Endpoint who queried.
        /// </summary>
        [JsonProperty("endpoint")]
        public EndpointInfo Endpoint { get; set; }
    }
}
