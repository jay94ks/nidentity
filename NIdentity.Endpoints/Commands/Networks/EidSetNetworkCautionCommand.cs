using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands.Networks
{
    /// <summary>
    /// A command to set caution level of the inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidSetNetworkCautionCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidSetNetworkCautionCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidSetNetworkCautionCommand() : base("ep_net_set_caution")
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

        /// <summary>
        /// Caution level.
        /// </summary>
        [JsonProperty("caution_level")]
        public EndpointCautionLevel CautionLevel { get; set; }
    }
}
