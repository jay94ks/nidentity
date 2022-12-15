using Newtonsoft.Json;
using NIdentity.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Endpoints.Commands
{
    /// <summary>
    /// A command to get the endpoint informations.
    /// </summary>
    [Command(Kind = "eid")]
    public class EidQueryEndpointCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="EidQueryEndpointCommand"/> instance
        /// </summary>
        /// <param name="Type"></param>
        public EidQueryEndpointCommand() : base("ep_query")
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("inventory")]
        public Guid Inventory { get; set; }

        /// <summary>
        /// Address to query database.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
