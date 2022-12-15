using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Networks;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to get the inventory informations.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidGetInventoryCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidQueryNetworkCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidGetInventoryCommand() : base("inv_get")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("identity")]
        public Guid Identity { get; set; }
    }
}
