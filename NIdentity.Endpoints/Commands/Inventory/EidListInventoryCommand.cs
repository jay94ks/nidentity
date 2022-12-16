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
    [Command(Kind = "eid", ResultType = typeof(EndpointInventoryListResult))]
    public class EidListInventoryCommand : EidSensitiveCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidListInventoryCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidListInventoryCommand() : base("inv_list")
        {
        }

        /// <summary>
        /// Inventory keyword.
        /// </summary>
        [JsonProperty("keyword")]
        public string Keyword { get; set; }

        /// <summary>
        /// Offset.
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
