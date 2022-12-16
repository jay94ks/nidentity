using NIdentity.Core.X509.Algorithms;

namespace NIdentity.Core.X509
{
    public partial class Certificate
    {
        /// <summary>
        /// Certificate Subject Type.
        /// </summary>
        public class SubjectBuilder
        {
            private static readonly Algorithm DEFAULT_ALGORITHM
                = Algorithm.MakeRsa(2048);

            /// <summary>
            /// Subject Type.
            /// </summary>
            public CertificateType Type { get; set; } = CertificateType.Leaf;

            /// <summary>
            /// Algorithm to use.
            /// </summary>
            public Algorithm Algorithm { get; set; } = DEFAULT_ALGORITHM;

            /// <summary>
            /// Hash Algorithm to use.
            /// </summary>
            public HashAlgorithmType HashAlgorithm { get; set; } = HashAlgorithmType.Default;

            /// <summary>
            /// Certificate purpose.
            /// </summary>
            public CertificatePurposes Purposes { get; set; } = CertificatePurposes.Networking;

            /// <summary>
            /// Subject.
            /// </summary>
            public string Subject { get; set; }

            /// <summary>
            /// Serial Number.
            /// </summary>
            public string SerialNumber { get; set; }

            /// <summary>
            /// Dns Names.
            /// </summary>
            public HashSet<string> DnsNames { get; } = new();

            /// <summary>
            /// Add certificate purposes.
            /// </summary>
            /// <param name="Purposes"></param>
            /// <returns></returns>
            public SubjectBuilder AddPurposes(CertificatePurposes Purposes)
            {
                this.Purposes |= Purposes;
                return this;
            }

            /// <summary>
            /// Remove certificate purposes.
            /// </summary>
            /// <param name="Purposes"></param>
            /// <returns></returns>
            public SubjectBuilder RemovePurposes(CertificatePurposes Purposes)
            {
                this.Purposes &= ~Purposes;
                return this;
            }

            /// <summary>
            /// Test whether this builder set purposes or not.
            /// </summary>
            /// <param name="Purposes"></param>
            /// <returns></returns>
            public bool HasPurposes(CertificatePurposes Purposes) => (this.Purposes & Purposes) == Purposes;

        }
    }
}