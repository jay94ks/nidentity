using Newtonsoft.Json;

namespace NIdentity.Core.Commands
{
    /// <summary>
    /// Base class of command result.
    /// </summary>
    public class CommandResult 
    {
        /// <summary>
        /// Success or not.
        /// </summary>
        [JsonProperty("success")]
        public bool Success { get; set; } = true;

        /// <summary>
        /// Reason if the execution failed.
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Reason Kind.
        /// </summary>
        [JsonProperty("reason_kind")]
        public string ReasonKind { get; set; }
    }
}
