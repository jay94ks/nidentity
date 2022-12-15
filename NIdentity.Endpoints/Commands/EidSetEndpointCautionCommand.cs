using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands
{
    /// <summary>
    /// A command to set caution level of the endpoint.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidSetEndpointCautionCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidSetEndpointCautionCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidSetEndpointCautionCommand() : base("ep_set_caution")
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
        /// Caution level.
        /// </summary>
        [JsonProperty("caution_level")]
        public EndpointCautionLevel CautionLevel { get; set; }
    }
}
