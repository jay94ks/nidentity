using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to delete the inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidDeleteInventoryCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidDeleteInventoryCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidDeleteInventoryCommand() : base("inv_delete")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("identity")]
        public Guid Identity { get; set; }
    }
}
