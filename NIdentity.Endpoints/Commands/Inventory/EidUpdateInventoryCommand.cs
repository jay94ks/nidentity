using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to update the inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidUpdateInventoryCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidUpdateInventoryCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidUpdateInventoryCommand() : base("inv_update")
        {
        }

        /// <summary>
        /// Name of this inventory.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of this inventory.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether this inventory is public or not.
        /// </summary>
        [JsonProperty("is_public")]
        public bool? IsPublic { get; set; }
    }
}
