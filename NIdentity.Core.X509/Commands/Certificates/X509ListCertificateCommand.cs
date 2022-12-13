using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Certificates
{
    [Command(Kind = "x509")]
    public class X509ListCertificateCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509ListCertificateCommand"/> instance.
        /// </summary>
        public X509ListCertificateCommand() : base("cert_list")
        {
        }

        /// <summary>
        /// Offset.
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// Count.
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; } = 20;

        /// <summary>
        /// List result.
        /// </summary>
        public class Result : CommandResult
        {
            /// <summary>
            /// Authority.
            /// </summary>
            [JsonProperty("authority")]
            public CertificateResult Authority { get; set; }

            /// <summary>
            /// Total Items.
            /// </summary>
            [JsonProperty("total_items")]
            public int TotalItems { get; set; }

            /// <summary>
            /// Items that fetched.
            /// </summary>
            [JsonProperty("items")]
            public CertificateResult[] Items { get; set; }
        }
    }
}
