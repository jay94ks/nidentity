using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;
using NIdentity.Endpoints.Commands.Results;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Networks
{
    /// <summary>
    /// A command to add the network to inventory.
    /// </summary>
    [Command(Kind = "eid", ResultType = typeof(EndpointNetworkResult))]
    public class EidAddNetworkCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidAddNetworkCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidAddNetworkCommand() : base("ep_net_add")
        {
        }

        /// <summary>
        /// Endpoint informations to add.
        /// </summary>
        [JsonProperty("network")]
        public EndpointNetworkInfo Network { get; set; }
    }
}
