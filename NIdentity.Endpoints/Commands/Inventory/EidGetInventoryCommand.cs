using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;
using NIdentity.Endpoints.Commands.Networks;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to get the inventory informations.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidGetInventoryCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidGetInventoryCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidGetInventoryCommand() : base("inv_get")
        {
        }

    }
}
