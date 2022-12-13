using NIdentity.Core.X509.Helpers;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace NIdentity.Core.X509
{
    public struct CertificateIdentity : IEquatable<CertificateIdentity>
    {
        private readonly string m_KeySHA1;

        /// <summary>
        /// Initialize <see cref="CertificateReference"/> from <see cref="X509Certificate"/>.
        /// </summary>
        /// <param name="X509"></param>
        public CertificateIdentity(X509Certificate X509)
        {
            if (X509 is null)
            {
                Subject = null;
                KeyIdentifier = null;
                m_KeySHA1 = null;
                return;
            }

            Subject = X509.SubjectDN.ToString();
            KeyIdentifier = X509.GetKeyIdentifier().ToLower();
            m_KeySHA1 = HashSHA1($"{KeyIdentifier}:{Subject}");
        }

        /// <summary>
        /// Initialize <see cref="CertificateIdentity"/> using <paramref name="Subject"/>> and <paramref name="KeyIdentifier"/>.
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="KeyIdentifier"></param>
        public CertificateIdentity(string Subject, string KeyIdentifier)
        {
            this.Subject = Subject;
            this.KeyIdentifier = KeyIdentifier;

            if (!string.IsNullOrWhiteSpace(Subject) && !string.IsNullOrWhiteSpace(KeyIdentifier))
            {
                KeyIdentifier = (KeyIdentifier ?? string.Empty).ToLower();
                m_KeySHA1 = HashSHA1($"{KeyIdentifier}:{Subject}");
                return;
            }

            m_KeySHA1 = null;
        }

        /// <summary>
        /// Initialize <see cref="CertificateIdentity"/> using <paramref name="Subject"/>> and <paramref name="KeyIdentifier"/>.
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="KeyIdentifier"></param>
        public CertificateIdentity(string Subject, string KeyIdentifier, string KeySHA1)
        {
            this.Subject = Subject;
            this.KeyIdentifier = KeyIdentifier;

            if (!string.IsNullOrWhiteSpace(Subject) && !string.IsNullOrWhiteSpace(KeyIdentifier))
            {
                m_KeySHA1 = KeySHA1;
                return;
            }

            m_KeySHA1 = null;
        }

        /// <summary>
        /// Initialize <see cref="CertificateIdentity"/> using <paramref name="Subject"/>> and <paramref name="KeyIdentifier"/>.
        /// </summary>
        /// <param name="Subject"></param>
        /// <param name="KeyIdentifier"></param>
        public CertificateIdentity(Certificate Certificate)
        {
            Subject = Certificate.Subject;
            KeyIdentifier = Certificate.KeyIdentifier;
            m_KeySHA1 = Certificate.KeySHA1;
        }

        /// <summary>
        /// Hash to SHA1.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        private static string HashSHA1(string Input)
        {
            using var Sha = SHA1.Create();
            var Temp = Encoding.UTF8.GetBytes($"ID, {Input ?? string.Empty}");
            return string.Join("", Sha.ComputeHash(Temp).Select(X => X.ToString("x2")));
        }

        /// <summary>
        /// Parse the input string and make <see cref="CertificateIdentity"/>.
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static CertificateIdentity Parse(string Input)
        {
            var Eq = Input.Split(':', 2, StringSplitOptions.None);
            if (Eq is null || Eq.Length <= 0)
                return default;

            var KeyIdentifier = Eq.FirstOrDefault();
            var Subject = Eq.LastOrDefault();
            return new CertificateIdentity(Subject, KeyIdentifier);
        }

        /// <summary>
        /// Indicates whether this certificate identity is valid or not.
        /// </summary>
        public bool Validity => !string.IsNullOrWhiteSpace(Subject) && !string.IsNullOrWhiteSpace(KeyIdentifier);

        /// <summary>
        /// Subject.
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Key Identifier.
        /// </summary>
        public string KeyIdentifier { get; }

        /// <summary>
        /// Make KeySHA1.
        /// </summary>
        /// <returns></returns>
        public string MakeKeySHA1() => m_KeySHA1;

        /// <summary>
        /// Test whether this issuer points the certificate or not.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public bool IsExact(Certificate Certificate)
        {
            if (ReferenceEquals(Certificate, null))
                throw new ArgumentNullException(nameof(Certificate));

            return Certificate.Subject == Subject
                && Certificate.KeyIdentifier.Equals(KeyIdentifier ?? string.Empty, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Test whether this issuer signed the certificate or not.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool IsSigned(Certificate Certificate)
        {
            if (ReferenceEquals(Certificate, null))
                throw new ArgumentNullException(nameof(Certificate));

            return Equals(Certificate.Issuer);
        }

        /// <summary>
        /// Test whether two <see cref="CertificateReference"/>s are equal or not.
        /// </summary>
        /// <param name="Other"></param>
        /// <returns></returns>
        public bool Equals(CertificateIdentity Other)
        {
            var Name = Subject ?? string.Empty;
            if (!Name.Equals(Other.Subject ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return false;

            var KeyIdentifier = this.KeyIdentifier ?? string.Empty;
            if (!KeyIdentifier.Equals(Other.KeyIdentifier ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object Obj)
        {
            if (Obj is CertificateIdentity Identity)
                return Equals(Identity);

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Validity)
            {
                var Subject = (this.Subject ?? string.Empty).ToLower();
                var KeyIdentifier = (this.KeyIdentifier ?? string.Empty).ToLower();
                return $"{KeyIdentifier}:{Subject}";
            }

            return string.Empty;
        }

        // ----
        public static bool operator ==(CertificateIdentity L, CertificateIdentity R) => L.Equals(R);
        public static bool operator !=(CertificateIdentity L, CertificateIdentity R) => !(L == R);
    }
}
