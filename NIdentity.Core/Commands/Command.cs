using Newtonsoft.Json;

namespace NIdentity.Core.Commands
{
    /// <summary>
    /// Base class of command objects.
    /// </summary>
    public abstract class Command
    {
        
        /// <summary>
        /// Initialize a new <see cref="Command"/> instance.
        /// </summary>
        public Command(string Type) => this.Type = Type;

        /// <summary>
        /// Test whether the command object is valid or not.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid() => true;

        /// <summary>
        /// Type of this command.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }

}
