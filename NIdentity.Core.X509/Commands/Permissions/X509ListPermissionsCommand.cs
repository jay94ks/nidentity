using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Permissions
{
    /// <summary>
    /// A command to list permissions of certificate.
    /// </summary>
    [Command(Kind = "x509", ResultType = typeof(Result))]
    public class X509ListPermissionsCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509ListPermissionsCommand"/> instance.
        /// </summary>
        public X509ListPermissionsCommand() : base("cert_list_perms")
        {
        }

        /// <summary>
        /// Offset of list.
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Count of list.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// Permission Listing Result.
        /// </summary>
        public class Result : CommandResult
        {
            /// <summary>
            /// Permissions
            /// </summary>
            [JsonProperty("perms")]
            public X509PermissionInfo[] Permissions { get; set; }
        }
    }
}
