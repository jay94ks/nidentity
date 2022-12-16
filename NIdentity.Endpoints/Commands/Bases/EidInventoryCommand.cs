using Newtonsoft.Json;

namespace NIdentity.Endpoints.Commands.Bases
{
    /// <summary>
    /// Base class for sensitive inventory commands
    /// that requires authority's certificate.
    /// </summary>
    public abstract class EidInventoryCommand : EidSensitiveCommand
    {
        /// <summary>
        /// Initialize a new <see cref="EidInventoryCommand"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        protected EidInventoryCommand(string Type) : base(Type)
        {
        }

        /// <summary>
        /// Inventory identity.
        /// </summary>
        [JsonProperty("identity")]
        public Guid Identity { get; set; }
    }
}
