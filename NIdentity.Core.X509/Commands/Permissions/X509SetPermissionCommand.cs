using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Permissions
{
    /// <summary>
    /// A command to set permissions of certificate.
    /// </summary>
    [Command(Kind = "x509")]
    public class X509SetPermissionCommand : X509PermissionAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509SetPermissionCommand"/> instance.
        /// </summary>
        public X509SetPermissionCommand() : base("cert_set_perm")
        {
        }

        /// <summary>
        /// Indicates whether the authority of <see cref="Owner"/> can interfere this intermediate CA's certificates or not.
        /// If this option set true, all accesses from the authority of <see cref="Owner"/> will be denied.
        /// And this option can only be altered by exact owner.
        /// </summary>
        [JsonProperty("can_interfere")]
        public bool? CanAuthorityInterfere { get; set; }

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can generate intermediate or leafs or not.
        /// (Leafs can never generate root or intermediates)
        /// </summary>
        /// <returns></returns>
        [JsonProperty("can_interfere")]
        public bool? CanGenerate { get; set; }

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can list certificates or not.
        /// </summary>
        [JsonProperty("can_list")]
        public bool? CanList { get; set; }

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can revoke certificates or not.
        /// </summary>
        [JsonProperty("can_revoke")]
        public bool? CanRevoke { get; set; }

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can delete certificates or not.
        /// </summary>
        [JsonProperty("can_delete")]
        public bool? CanDelete { get; set; }

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can alter permissions or not.
        /// </summary>
        [JsonProperty("can_alter")]
        public bool? CanAlter { get; set; }
    }
}
