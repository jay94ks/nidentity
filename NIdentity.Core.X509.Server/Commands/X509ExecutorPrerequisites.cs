using Newtonsoft.Json;
using NIdentity.Core.X509.Commands.Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509.Server.Commands
{
    /// <summary>
    /// Prerequisites that is required to make <see cref="X509Executor"/> works correctly.
    /// </summary>
    public class X509ExecutorPrerequisites
    {
        /// <summary>
        /// Required Keys.
        /// </summary>
        [JsonProperty("required_keys")]
        public List<X509GenerateCommand> RequiredKeys { get; set; } = new();
    }
}
