using Org.BouncyCastle.Math;
using Org.BouncyCastle.X509;
using System.Diagnostics.CodeAnalysis;
using NIdentity.Core.X509.Helpers;

using X509Certificate2 = System.Security.Cryptography.X509Certificates.X509Certificate;
using X509ContentType = System.Security.Cryptography.X509Certificates.X509ContentType;
using System.Security.Cryptography;
using System.Text;

namespace NIdentity.Core.X509
{
    /// <summary>
    /// Certificate Identity.
    /// </summary>
    public struct CertificateReference : IEquatable<CertificateReference>
    {
        private readonly string m_RefSHA1;

        /// <summary>
        /// Initialize <see cref="CertificateReference"/> from <see cref="Certificate"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        public CertificateReference(Certificate Certificate)
        {
            if (ReferenceEquals(Certificate, null))
            {
                SerialNumber = null;
                IssuerKeyIdentifier = null;
                m_RefSHA1 = null;
                return;
            }

            SerialNumber = Certificate.SerialNumber;
            IssuerKeyIdentifier = Certificate.Issuer.KeyIdentifier;
            m_RefSHA1 = Certificate.RefSHA1;
        }

        /// <summary>
        /// Initialize <see cref="CertificateReference"/> from <see cref="X509Certificate2"/>.
        /// </summary>
        /// <param name="X509"></param>
        public CertificateReference(X509Certificate2 X509)
        {
            if (X509 is null)
            {
                SerialNumber = null;
                IssuerKeyIdentifier = null;
                m_RefSHA1 = null;
                return;
            }

            var BcX509 = new X509Certificate(X509.Export(X509ContentType.Cert));
            SerialNumber = BcX509.SerialNumber.ToString(16);
            IssuerKeyIdentifier = BcX509.GetIssuerKeyIdentifier();
            m_RefSHA1 = HashSHA1($"{SerialNumber}@{IssuerKeyIdentifier}");
        }

        /// <summary>
        /// Initialize <see cref="CertificateReference"/> using <paramref name="SerialNumber"/> and <paramref name="IssuerKeyIdentifier"/>.
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="IssuerKeyIdentifier"></param>
        public CertificateReference(string SerialNumber, string IssuerKeyIdentifier)
        {
            this.SerialNumber = SerialNumber;
            this.IssuerKeyIdentifier = IssuerKeyIdentifier;

            if (!string.IsNullOrWhiteSpace(SerialNumber) && !string.IsNullOrWhiteSpace(IssuerKeyIdentifier))
            {
                SerialNumber = (this.SerialNumber ?? string.Empty).ToLower();
                IssuerKeyIdentifier = (this.IssuerKeyIdentifier ?? string.Empty).ToLower();
                m_RefSHA1 = HashSHA1($"{SerialNumber}@{IssuerKeyIdentifier}");
                return;
            }

            m_RefSHA1 = null;
        }

        /// <summary>
        /// Initialize <see cref="CertificateReference"/> using <paramref name="SerialNumber"/> and <paramref name="IssuerKeyIdentifier"/>.
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="IssuerKeyIdentifier"></param>
        public CertificateReference(string SerialNumber, string IssuerKeyIdentifier, string RefSHA1)
        {
            this.SerialNumber = SerialNumber;
            this.IssuerKeyIdentifier = IssuerKeyIdentifier;
            m_RefSHA1 = RefSHA1;
        }

        /// <summary>
        /// Hash to SHA1.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        private static string HashSHA1(string Input)
        {
            using var Sha = SHA1.Create();
            var Temp = Encoding.UTF8.GetBytes($"RF, {Input ?? string.Empty}");
            return string.Join("", Sha.ComputeHash(Temp).Select(X => X.ToString("x2")));
        }

        /// <summary>
        /// Parse the input string and make <see cref="CertificateIdentity"/>.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static CertificateReference Parse(string Input)
        {
            var Eq = Input.Split('@', 2, StringSplitOptions.None);
            if (Eq is null || Eq.Length <= 0)
                return default;

            var SerialNumber = Eq.FirstOrDefault();
            var IssuerKeyIdentifier = Eq.LastOrDefault();
            return new CertificateReference(IssuerKeyIdentifier, SerialNumber);
        }

        /// <summary>
        /// Initialize <see cref="CertificateReference"/> from <see cref="Certificate"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        public static implicit operator CertificateReference(Certificate Certificate) => new(Certificate);

        /// <summary>
        /// Initialize <see cref="CertificateReference"/> from <see cref="Certificate"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        public static implicit operator CertificateReference(X509Certificate2 Certificate) => new(Certificate);

        /// <summary>
        /// Indicates whether this certificate identity is valid or not.
        /// </summary>
        public bool Validity => !string.IsNullOrWhiteSpace(SerialNumber) && !string.IsNullOrWhiteSpace(IssuerKeyIdentifier);

        /// <summary>
        /// Subject.
        /// </summary>
        public string SerialNumber { get; }

        /// <summary>
        /// Key Identifier.
        /// </summary>
        public string IssuerKeyIdentifier { get; }

        /// <summary>
        /// Make RefSHA1.
        /// </summary>
        /// <returns></returns>
        public string MakeRefSHA1() => m_RefSHA1;

        /// <summary>
        /// Test whether two <see cref="CertificateReference"/>s are equal or not.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool Equals(CertificateReference Other)
        {
            var SerialNumber = this.SerialNumber ?? string.Empty;
            if (!SerialNumber.Equals(Other.SerialNumber ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return false;

            var IssuerKeyIdentifier = this.IssuerKeyIdentifier ?? string.Empty;
            if (!IssuerKeyIdentifier.Equals(Other.IssuerKeyIdentifier ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object Obj)
        {
            if (Obj is CertificateReference Identity)
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
                var SerialNumber = (this.SerialNumber ?? string.Empty).ToLower();
                var IssuerKeyIdentifier = (this.IssuerKeyIdentifier ?? string.Empty).ToLower();
                return $"{SerialNumber}@{IssuerKeyIdentifier}";
            }

            return string.Empty;
        }

        // ----
        public static bool operator ==(CertificateReference L, CertificateReference R) => L.Equals(R);
        public static bool operator !=(CertificateReference L, CertificateReference R) => !(L == R);
    }
}
