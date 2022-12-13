using Org.BouncyCastle.Ocsp;
using System.Diagnostics.CodeAnalysis;

namespace NIdentity.Core.X509.Server.Ocsp
{
    /// <summary>
    /// Ocsp Certificate Identity.
    /// </summary>
    public struct OcspCertificateIdentity : IEquatable<OcspCertificateIdentity>
    {
        /// <summary>
        /// Initialize <see cref="OcspCertificateIdentity"/> from <see cref="Req"/>.
        /// </summary>
        public OcspCertificateIdentity(Req Request)
        {
            var CertId = Request.GetCertID();
            SerialNumber = CertId.SerialNumber.ToString(16);
            IssuerNameHash = string.Join("", CertId.GetIssuerNameHash().Select(X => X.ToString("x2")));
            IssuerKeyIdentifier = string.Join("", CertId.GetIssuerKeyHash().Select(X => X.ToString("x2")));
        }

        /// <summary>
        /// Initialize <see cref="OcspCertificateIdentity"/> from <see cref="CertificateReference"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        public OcspCertificateIdentity(CertificateReference Certificate)
        {
            SerialNumber = Certificate.SerialNumber;
            IssuerNameHash = null;
            IssuerKeyIdentifier = Certificate.IssuerKeyIdentifier;
        }

        /// <summary>
        /// Indicates whether this certificate identity is valid or not.
        /// </summary>
        public bool Validity => IsWithNameHash || IsWithKeyIdentifier;

        /// <summary>
        /// Indicates whether this certificate identity is with name hash or not.
        /// </summary>
        public bool IsWithNameHash => !string.IsNullOrWhiteSpace(SerialNumber) && !string.IsNullOrWhiteSpace(IssuerNameHash);

        /// <summary>
        /// Indicates whether this certificate identity is with key identifier or not.
        /// </summary>
        public bool IsWithKeyIdentifier => !string.IsNullOrWhiteSpace(SerialNumber) && !string.IsNullOrWhiteSpace(IssuerKeyIdentifier);

        /// <summary>
        /// Serial Number.
        /// </summary>
        public string SerialNumber { get; }

        /// <summary>
        /// Issuer's Name Hash
        /// </summary>
        public string IssuerNameHash { get; }

        /// <summary>
        /// Issuer's Key Identifier.
        /// </summary>
        public string IssuerKeyIdentifier { get; }

        /// <summary>
        /// Try to get certificate reference.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public bool TryGetCertificateReference(out CertificateReference Identity)
        {
            if (IsWithKeyIdentifier)
            {
                Identity = new CertificateReference(SerialNumber, IssuerKeyIdentifier);
                return true;
            }

            Identity = default;
            return false;
        }

        /// <summary>
        /// Test whether two <see cref="OcspCertificateIdentity"/>s are equal or not.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool Equals(OcspCertificateIdentity Other)
        {
            var SerialNumber = this.SerialNumber ?? string.Empty;
            if (!SerialNumber.Equals(Other.SerialNumber ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return false;

            if (IsWithKeyIdentifier)
            {
                var IssuerKeyIdentifier = this.IssuerKeyIdentifier ?? string.Empty;
                if (!IssuerKeyIdentifier.Equals(Other.IssuerKeyIdentifier ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            else
            {
                var IssuerNameHash = this.IssuerNameHash ?? string.Empty;
                if (!IssuerNameHash.Equals(Other.IssuerNameHash ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object Obj)
        {
            if (Obj is OcspCertificateIdentity Identity)
                return Equals(Identity);

            if (Obj is Certificate Certificate)
                return Equals(new CertificateReference(Certificate));

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode() => ToString().GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Validity)
            {
                var SerialNumber = this.SerialNumber ?? string.Empty;
                if (IsWithKeyIdentifier)
                {
                    var IssuerKeyIdentifier = this.IssuerKeyIdentifier ?? string.Empty;
                    return $"{SerialNumber}@{IssuerKeyIdentifier}";
                }

                else
                {
                    var IssuerNameHash = this.IssuerNameHash ?? string.Empty;
                    return $"{SerialNumber}#{IssuerNameHash}";
                }
            }

            return string.Empty;
        }

        // ----
        public static bool operator ==(OcspCertificateIdentity L, OcspCertificateIdentity R) => L.Equals(R);
        public static bool operator !=(OcspCertificateIdentity L, OcspCertificateIdentity R) => !(L == R);
    }
}
