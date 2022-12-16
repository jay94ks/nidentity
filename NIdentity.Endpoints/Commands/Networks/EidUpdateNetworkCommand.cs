using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;

namespace NIdentity.Endpoints.Commands.Networks
{
    /// <summary>
    /// A command to update the network.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidUpdateNetworkCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidUpdateNetworkCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidUpdateNetworkCommand() : base("ep_net_update")
        {
        }

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

        /// <summary>
        /// Endpoint Type.
        /// </summary>
        [JsonProperty("net_type")]
        public EndpointNetworkType NetworkType { get; set; }

        /// <summary>
        /// Name of this network.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of this network.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
