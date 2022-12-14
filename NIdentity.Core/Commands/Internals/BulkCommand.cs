using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIdentity.Core.Helpers;

namespace NIdentity.Core.Commands.Internals
{
    /// <summary>
    /// Bulk command.
    /// </summary>
    public class BulkCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="BulkCommand"/> instance.
        /// </summary>
        public BulkCommand() : base("bulk")
        {
        }

        /// <summary>
        /// Actions.
        /// </summary>
        [JsonProperty("actions")]
        public JObject[] Actions { get; set; }

        /// <summary>
        /// Set actions.
        /// </summary>
        /// <param name="Commands"></param>
        /// <returns></returns>
        public BulkCommand SetActions(params Command[] Commands)
        {
            Actions = Commands.Select(X => X.ToJson()).ToArray();
            return this;
        }

        /// <summary>
        /// Set actions.
        /// </summary>
        /// <param name="Commands"></param>
        /// <returns></returns>
        public BulkCommand SetActions(IEnumerable<Command> Commands)
        {
            Actions = Commands.Select(X => X.ToJson()).ToArray();
            return this;
        }

        /// <summary>
        /// Bulk Result.
        /// </summary>
        public class Result : CommandResult
        {
            /// <summary>
            /// Bulk results.
            /// </summary>
            [JsonProperty("results")]
            public CommandResult[] Results { get; set; }

            /// <summary>
            /// Total Time.
            /// </summary>
            [JsonProperty("total_time")]
            public TimeSpan TotalTime { get; set; }
        }
    }
}
