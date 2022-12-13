using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIdentity.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Connector
{
    public class RemoteCommandResult : CommandResult
    {
        /// <summary>
        /// Response. (Original JSON)
        /// </summary>
        [JsonIgnore]
        public JObject Response { get; set; }
    }
}
