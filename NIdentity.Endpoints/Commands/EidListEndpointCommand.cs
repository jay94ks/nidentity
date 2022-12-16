using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;
using NIdentity.Endpoints.Commands.Results;

namespace NIdentity.Endpoints.Commands
{
    /// <summary>
    /// A command to get the endpoint informations.
    /// </summary>
    [Command(Kind = "eid", ResultType = typeof(EndpointListResult))]
    public class EidListEndpointCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidListEndpointCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidListEndpointCommand() : base("ep_list")
        {
        }

        /// <summary>
        /// Offset.
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
