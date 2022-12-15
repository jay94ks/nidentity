using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to set caution level of the inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidSetInventoryCautionCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidSetInventoryCautionCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidSetInventoryCautionCommand() : base("inv_set_caution")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("identity")]
        public Guid Identity { get; set; }

        /// <summary>
        /// Caution level.
        /// </summary>
        [JsonProperty("caution_level")]
        public EndpointCautionLevel CautionLevel { get; set; }
    }
}
