using Newtonsoft.Json;

namespace NIdentity.Core.X509.Commands
{
    /// <summary>
    /// Base class for permission access commands.
    /// </summary>
    public abstract class X509PermissionAccessCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509PermissionAccessCommand"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        protected X509PermissionAccessCommand(string Type) : base(Type)
        {
        }

        /// <summary>
        /// Accessor's Subject.
        /// </summary>
        [JsonProperty("accessor")]
        public string AccessorSubject { get; set; }

        /// <summary>
        /// Accessors' Key Identifier.
        /// </summary>
        [JsonProperty("accessor_key_id")]
        public string AccessorKeyIdentifier { get; set; }

        /// <summary>
        /// Accessor's Identity.
        /// </summary>
        [JsonIgnore]
        public CertificateIdentity ByAccessorIdentity => new CertificateIdentity(Subject, KeyIdentifier);
    }
}
