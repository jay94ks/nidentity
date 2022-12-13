using Newtonsoft.Json;
using NIdentity.Core.X509.Server.Commands.Certificates;

namespace NIdentity.Core.X509.Server.Commands
{
    public class X509ExecutorSettings
    {
        /// <summary>
        /// Http Base Uri.
        /// </summary>
        public Uri HttpBaseUri { get; set; }

        /// <summary>
        /// Maximum Expiration in hours.
        /// </summary>
        public double MaximumExpirationInHours { get; set; } = 120.0 * 365 * 24;

        /// <summary>
        /// Excludes `Leaf` certificate's private keys.
        /// </summary>
        public bool ExcludeLeafPrivateKeys { get; set; } = true;

        /// <summary>
        /// Default Key Algorithm.
        /// </summary>
        public string DefaultKeyAlgorithm { get; set; } = $"rsa-{Algorithms.Algorithm.RsaKeyLengths.First()}";

        /// <summary>
        /// Disallow to generate RootCA.
        /// </summary>
        public bool DisallowGenerateRootCA { get; set; } = false;

        /// <summary>
        /// System certificate who can generate RootCA certificate.
        /// If this set null, it means that any root-ca can generate new root-ca.
        /// </summary>
        public CertificateIdentity? SystemCertificate { get; set; }

        /// <summary>
        /// Http Ocsp endpoint.
        /// </summary>
        public string HttpOcsp { get; set; } = "/api/infra/ocsp";

        /// <summary>
        /// Http CRL distribution endpoint.
        /// </summary>
        public string HttpCRL { get; set; } = "/api/infra/crls";

        /// <summary>
        /// Http CA issuer's certificate endpoint.
        /// </summary>
        public string HttpCAIssuers { get; set; } = "/api/infra/cers";

        /// <summary>
        /// Max Count Per List Request.
        /// </summary>
        public int MaxCountPerListRequest { get; set; } = 100;

        /// <summary>
        /// Prerequisites.
        /// </summary>
        public X509ExecutorPrerequisites Prerequisites { get; set; } = new();
    }
}
