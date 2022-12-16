using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands
{
    /// <summary>
    /// A command to add the endpoint to inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidAddEndpointCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidAddEndpointCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidAddEndpointCommand() : base("ep_add")
        {
        }

        /// <summary>
        /// Endpoint informations to add.
        /// </summary>
        [JsonProperty("endpoint")]
        public EndpointInfo Endpoint { get; set; }
    }
}
