using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands.Networks
{
    /// <summary>
    /// A command to remove the network to inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidRemoveNetworkCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidRemoveNetworkCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidRemoveNetworkCommand() : base("ep_net_remove")
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

        /// <summary>
        /// Subnet Mask.
        /// </summary>
        [JsonProperty("subnet_mask")]
        public int SubnetMask { get; set; }
    }
}
