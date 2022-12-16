using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Endpoints.Commands.Bases;
using NIdentity.Endpoints.Commands.Results;

namespace NIdentity.Endpoints.Commands.Networks
{
    /// <summary>
    /// A command to list the network informations.
    /// </summary>
    [Command(Kind = "eid", ResultType = typeof(EndpointNetworkListResult))]
    public class EidListNetworkCommand : EidInventoryCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidListNetworkCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidListNetworkCommand() : base("ep_net_list")
        {
        }
    }
}
