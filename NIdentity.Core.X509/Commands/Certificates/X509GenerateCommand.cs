using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Algorithms;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Certificates
{
    /// <summary>
    /// Generate Command.
    /// </summary>
    [Command(Kind = "x509")]
    public class X509GenerateCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="X509GenerateCommand"/> instance.
        /// </summary>
        public X509GenerateCommand() : base("cert_generate")
        {
        }

        /// <summary>
        /// Generate Result.
        /// </summary>
        public class Result : X509CertificateAccessCommand.CertificateResult<Result>
        {
            /// <summary>
            /// Generated Certificate PFX file.
            /// </summary>
            [JsonProperty("pfx_base64")]
            public string PfxBase64 { get; set; }
        }

        /// <summary>
        /// Certificate Type.
        /// </summary>
        [JsonProperty("cert_type")]
        public CertificateType KeyType { get; set; }

        /// <summary>
        /// Certificate Hash-algorithm type.
        /// </summary>
        [JsonProperty("cert_hash")]
        public HashAlgorithmType HashType { get; set; } = HashAlgorithmType.Default;

        /// <summary>
        /// Key Algorithm.
        /// </summary>
        [JsonProperty("key_algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        /// Key Purposes.
        /// </summary>
        [JsonProperty("key_purposes")]
        public CertificatePurposes Purposes { get; set; }

        /// <summary>
        /// Expiration in hours.
        /// </summary>
        [JsonProperty("expiration_hrs")]
        public double ExpirationHours { get; set; } = 365 * 24;

        /// <summary>
        /// Subject.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Serial Number. (optional)
        /// </summary>
        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Dns Names. (optional)
        /// </summary>
        [JsonProperty("dns_names")]
        public string[] DnsNames { get; set; }

        /// <summary>
        /// Issuer.
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// Issuer Key Identifier.
        /// </summary>
        [JsonProperty("issuer_key_id")]
        public string IssuerKeyIdentifier { get; set; }

        /// <summary>
        /// Subject reference.
        /// </summary>
        [JsonIgnore]
        public CertificateReference SubjectReference => new CertificateReference(SerialNumber, IssuerKeyIdentifier);

        /// <summary>
        /// Issuer's certificate identity.
        /// </summary>
        [JsonIgnore]
        public CertificateIdentity IssuerIdentity => new CertificateIdentity(Issuer, IssuerKeyIdentifier);

        /// <summary>
        /// With OCSP tracker informations.
        /// </summary>
        [JsonProperty("with_ocsp")]
        public bool WithOcsp { get; set; }

        /// <summary>
        /// With CRL distribution points.
        /// </summary>
        [JsonProperty("with_crl_dists")]
        public bool WithCrlDists { get; set; }

        /// <summary>
        /// With CA's certificate uri information.
        /// </summary>
        [JsonProperty("with_ca_issuers")]
        public bool WithCAIssuers { get; set; }

        /// <summary>
        /// With additional OCSP server uris.
        /// </summary>
        [JsonProperty("ocsp_servers")]
        public string[] AdditionalOcspServers { get; set; }

        /// <summary>
        /// With additional CRL distribution point uris.
        /// </summary>
        [JsonProperty("crl_dists")]
        public string[] AdditionalCrlDists { get; set; }

        /// <summary>
        /// With additional CA issuer uris.
        /// </summary>
        [JsonProperty("ca_issuers")]
        public string[] AdditionalCAIssuers { get; set; }
    }
}
