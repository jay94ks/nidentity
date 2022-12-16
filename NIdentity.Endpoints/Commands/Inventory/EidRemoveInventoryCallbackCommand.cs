using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to remove callback.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidRemoveInventoryCallbackCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidRemoveInventoryCallbackCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidRemoveInventoryCallbackCommand() : base("inv_remove_cb")
        {
        }

        /// <summary>
        /// Callback to remove.
        /// </summary>
        [JsonProperty("callback")]
        public string Callback { get; set; }
    }
}
