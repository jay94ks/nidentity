using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Results
{
    /// <summary>
    /// Command result.
    /// </summary>
    public class EndpointNetworkResult : CommandResult
    {
        /// <summary>
        /// Networks that contains specified range.
        /// </summary>
        [JsonProperty("networks")]
        public EndpointNetworkInfo[] Networks { get; set; }
    }
}
