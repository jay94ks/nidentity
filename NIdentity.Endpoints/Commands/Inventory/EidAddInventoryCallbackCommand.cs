using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to add callback to get caution events.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidAddInventoryCallbackCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidAddInventoryCallbackCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidAddInventoryCallbackCommand() : base("inv_add_cb")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("identity")]
        public Guid Identity { get; set; }

        /// <summary>
        /// Callback to get caution events.
        /// </summary>
        [JsonProperty("callback")]
        public string Callback { get; set; }
    }
}
