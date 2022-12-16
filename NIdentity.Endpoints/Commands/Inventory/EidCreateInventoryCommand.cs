using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;
using NIdentity.Endpoints.Commands.Results;
using NIdentity.Endpoints.Metas;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to create the inventory.
    /// </summary>
    [Command(Kind = "eid", ResultType = typeof(EndpointInventoryResult))]
    public class EidCreateInventoryCommand : EidSensitiveCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidCreateInventoryCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidCreateInventoryCommand() : base("inv_create")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("inventory")]
        public EndpointInventoryInfo Inventory { get; set; }
    }
}
