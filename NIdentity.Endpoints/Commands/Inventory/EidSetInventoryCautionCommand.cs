using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to set caution level of the inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidSetInventoryCautionCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidSetInventoryCautionCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidSetInventoryCautionCommand() : base("inv_set_caution")
        {
        }

        /// <summary>
        /// Caution level.
        /// </summary>
        [JsonProperty("caution_level")]
        public EndpointCautionLevel CautionLevel { get; set; }
    }
}
