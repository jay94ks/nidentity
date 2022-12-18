using Newtonsoft.Json;

namespace NIdentity.Core.X509.Server.Commands.Permissions
{
    /// <summary>
    /// X509 Permission information (JSON encoded)
    /// </summary>
    public class X509PermissionInfo
    {
        /// <summary>
        /// Make <see cref="X509PermissionInfo"/> from <see cref="CertificatePermission"/>.
        /// </summary>
        /// <param name="Permission"></param>
        /// <returns></returns>
        public static X509PermissionInfo Make(CertificatePermission Permission)
        {
            return new X509PermissionInfo
            {
                Subject = Permission.Owner.Subject,
                KeyIdentifier = Permission.Owner.KeyIdentifier,

                AccessorSubject = Permission.Accessor.Subject,
                AccessorKeyIdentifier = Permission.Accessor.KeyIdentifier,

                CreationTime = Permission.CreationTime,
                LastWriteTime = Permission.LastWriteTime,

                CanAuthorityInterfere = Permission.CanAuthorityInterfere,
                CanGenerate = Permission.CanGenerate,
                CanList = Permission.CanList,
                CanRevoke = Permission.CanRevoke,
                CanDelete = Permission.CanDelete,
                CanAlter = Permission.CanAlter
            };
        }

        /// <summary>
        /// Subject.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Key Identifier.
        /// </summary>
        [JsonProperty("key_id")]
        public string KeyIdentifier { get; set; }

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
        /// Creation Time.
        /// </summary>
        [JsonProperty("creation_time")]
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Last Write Time.
        /// </summary>
        [JsonProperty("last_write_time")]
        public DateTimeOffset LastWriteTime { get; set; }

        /// <summary>
        /// Indicates whether this permission entity is about children default or not.
        /// </summary>
        [JsonIgnore]
        public bool IsChildrenDefault =>
            string.IsNullOrWhiteSpace(AccessorSubject) == true ||
            string.IsNullOrWhiteSpace(AccessorKeyIdentifier) == true;

        /// <summary>
        /// Indicates whether the authority of <see cref="Owner"/> can interfere this intermediate CA's certificates or not.
        /// If this option set true, all accesses from the authority of <see cref="Owner"/> will be denied.
        /// And this option can only be altered by exact owner.
        /// </summary>
        [JsonProperty("can_interfere")]
        public bool CanAuthorityInterfere { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can generate intermediate or leafs or not.
        /// (Leafs can never generate root or intermediates)
        /// </summary>
        /// <returns></returns>
        [JsonProperty("can_interfere")]
        public bool CanGenerate { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can list certificates or not.
        /// </summary>
        [JsonProperty("can_list")]
        public bool CanList { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can revoke certificates or not.
        /// </summary>
        [JsonProperty("can_revoke")]
        public bool CanRevoke { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can delete certificates or not.
        /// </summary>
        [JsonProperty("can_delete")]
        public bool CanDelete { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can alter permissions or not.
        /// </summary>
        [JsonProperty("can_alter")]
        public bool CanAlter { get; set; } = false;
    }
}
