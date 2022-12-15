using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Results
{
    /// <summary>
    /// Endpoint query result.
    /// </summary>
    public class EndpointQueryResult : CommandResult
    {
        /// <summary>
        /// Summarized endpoint information.
        /// </summary>
        [JsonProperty("endpoint")]
        public EndpointInfo Endpoint { get; set; }

        /// <summary>
        /// Summarized network informations where this endpoint belongs to.
        /// </summary>
        [JsonProperty("networks")]
        public EndpointNetworkInfo[] Networks { get; set; }
    }
}
