using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;
using NIdentity.Endpoints.Commands.Networks;
using NIdentity.Endpoints.Commands.Results;

namespace NIdentity.Endpoints.Commands.Inventory
{
    /// <summary>
    /// A command to get the inventory informations.
    /// </summary>
    [Command(Kind = "eid", ResultType = typeof(EndpointInventoryResult))]
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
