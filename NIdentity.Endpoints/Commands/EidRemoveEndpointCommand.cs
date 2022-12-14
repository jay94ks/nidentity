using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;

namespace NIdentity.Endpoints.Commands
{
    /// <summary>
    /// A command to remove the endpoint from inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidRemoveEndpointCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidRemoveEndpointCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidRemoveEndpointCommand() : base("ep_remove")
        {
        }

        /// <summary>
        /// Address to query database.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
