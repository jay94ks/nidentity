using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands
{
    /// <summary>
    /// A command to get the endpoint informations.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidListEndpointCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidListEndpointCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidListEndpointCommand() : base("ep_list")
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
