using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Networks
{
    /// <summary>
    /// A command to add the network to inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidAddNetworkCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidAddNetworkCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidAddNetworkCommand() : base("ep_net_add")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("inventory")]
        public Guid Inventory { get; set; }

        /// <summary>
        /// Endpoint informations to add.
        /// </summary>
        [JsonProperty("network")]
        public EndpointNetworkInfo Network { get; set; }
    }
}
