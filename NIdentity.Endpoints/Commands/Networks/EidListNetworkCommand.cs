using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands.Networks
{
    /// <summary>
    /// A command to list the network informations.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidListNetworkCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidListNetworkCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidListNetworkCommand() : base("ep_net_list")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("inventory")]
        public Guid Inventory { get; set; }

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
