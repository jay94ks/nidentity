using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands
{
    /// <summary>
    /// A command to update the endpoint.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidUpdateEndpointCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidUpdateEndpointCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidUpdateEndpointCommand() : base("ep_update")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("identity")]
        public Guid Identity { get; set; }

        /// <summary>
        /// Address to query database.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

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
