using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to remove callback.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidRemoveInventoryCallbackCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidRemoveInventoryCallbackCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidRemoveInventoryCallbackCommand() : base("inv_remove_cb")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("identity")]
        public Guid Identity { get; set; }

        /// <summary>
        /// Callback to remove.
        /// </summary>
        [JsonProperty("callback")]
        public string Callback { get; set; }
    }
}
