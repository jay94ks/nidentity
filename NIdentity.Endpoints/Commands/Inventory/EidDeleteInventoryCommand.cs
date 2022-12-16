using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to delete the inventory.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidDeleteInventoryCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidDeleteInventoryCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidDeleteInventoryCommand() : base("inv_delete")
        {
        }
    }
}
