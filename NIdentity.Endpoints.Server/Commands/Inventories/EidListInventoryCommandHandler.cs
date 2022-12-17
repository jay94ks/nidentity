using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Inventory;
using NIdentity.Endpoints.Server.Commands.Base;

namespace NIdentity.Endpoints.Server.Commands.Inventories
{
    /// <summary>
    /// Lists inventories that owned by requester.
    /// </summary>
    [CommandHandler(typeof(EidListInventoryCommand), Kind = "eid")]
    public class EidListInventoryCommandHandler : EndpointCommandHandler<EidListInventoryCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="EidListInventoryCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public EidListInventoryCommandHandler(EndpointRequesterAccesor Requester) : base(Requester)
        {
        }

        public override Task<CommandResult> ExecuteAsync(EndpointCommandContext Context)
        {
            throw new NotImplementedException();
        }
    }
}
