using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Results;

namespace NIdentity.Endpoints.Commands
{
    /// <summary>
    /// A command to get the endpoint informations.
    /// </summary>
    [Command(Kind = "eid", ResultType = typeof(EndpointQueryResult))]
    public class EidQueryEndpointCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidQueryEndpointCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidQueryEndpointCommand() : base("ep_query")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("inventory")]
        public Guid Inventory { get; set; }

        /// <summary>
        /// Address to query database.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
