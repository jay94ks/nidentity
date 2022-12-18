using NIdentity.Core.Helpers;
using NIdentity.Core.X509.Algorithms;
using NIdentity.Core.X509.Helpers;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using System.Text;
using DX509 = System.Security.Cryptography.X509Certificates.X509Certificate2;
using DX509Type = System.Security.Cryptography.X509Certificates.X509ContentType;

namespace NIdentity.Core.X509
{
    public partial class Certificate : IEquatable<Certificate>
    {
        private string m_CachedKeySHA1;
        private string m_CachedRefSHA1;

        private string m_CachedSubject;
        private string m_CachedSerialNumber;
        private string m_CachedThumbprint;
        private string m_CachedKeyIdentifier;

        private string m_CachedSubjectHash;
        private string m_CachedIssuerHash;

        private string[] m_CachedSans;

        private CertificateType? m_CachedType;
        private CertificatePurposes? m_CachedPurposes;
        private CertificateIdentity? m_CachedIssuer;

        private AsymmetricKeyParameter m_CachedPublicKey;

        /// <summary>
        /// Initialize a new <see cref="Certificate"/> instance.
        /// </summary>
        /// <param name="X509"></param>
        /// <param name="PrivateKey"></param>
        internal Certificate(X509Certificate X509, AsymmetricKeyParameter PrivateKey = null)
        {
            this.X509 = X509;
            this.PrivateKey = PrivateKey;
            //X509.IssuerDN.
            CreationTime = this.X509.NotBefore;
            ExpirationTime = this.X509.NotAfter;
        }

        /// <summary>
        /// Make Name Hash.
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        internal static string MakeNameHash(X509Name Name)
        {
            using var Sha = SHA1.Create();

            var Result = Sha.ComputeHash(Name.GetDerEncoded());
            var Value = string.Join("", Result.Select(X => X.ToString("x2")));

            Console.WriteLine(Value);
            return Value;
        }

        /// <summary>
        /// Import CER bytes.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static Certificate Import(byte[] Data) => new Certificate(new X509Certificate(Data));

        /// <summary>
        /// Import PFX bytes.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static Certificate ImportPfx(byte[] Data, string Password = null)
        {
            var Store = CertificateStore.Import(Data, Password);
            return Store.Certificates.OrderByDescending(X => X.HasPrivateKey ? 1 : 0).First();
        }

        /// <summary>
        /// Import PEM string.
        /// Warning: this will not load private key.
        /// To load private key, use <see cref="CertificateStore.ImportPem(string)"/>.
        /// </summary>
        /// <param name="PemText"></param>
        /// <returns></returns>
        public static Certificate ImportPem(string PemText)
        {
            // --> normalize sparators.
            PemText = string.Join("\r\n", PemText.Split(' ', '\n', '\r')
                .Where(X => !string.IsNullOrWhiteSpace(X)));

            using var TextRd = new StringReader(PemText);
            var Pem = new PemReader(TextRd);

            while (true)
            {
                var Obj = Pem.ReadObject();
                if (Obj is null)
                    return null;

                if (Obj is not X509Certificate Read)
                    continue;

                var Cert = Import(Read.GetEncoded());
                if (Cert != null)
                {
                    return Cert;
                }
            }
        }

        /// <summary>
        /// Import PEM string.
        /// Warning: this will not load private key.
        /// To load private key, use <see cref="CertificateStore.ImportPem(byte[])"/>.
        /// </summary>
        /// <param name="PemBytes"></param>
        /// <returns></returns>
        public static Certificate ImportPem(byte[] PemBytes)
            => ImportPem(Encoding.UTF8.GetString(PemBytes));

        /// <summary>
        /// Bouncy Castle X509.
        /// </summary>
        public X509Certificate X509 { get; set; }

        /// <summary>
        /// Bouncy Castle X509 Private Key.
        /// </summary>
        public AsymmetricKeyParameter PrivateKey { get; set; }

        /// <summary>
        /// Bouncy Castle X509 Public Key.
        /// </summary>
        public AsymmetricKeyParameter PublicKey => GetterHelpers.Cached(ref m_CachedPublicKey, () => X509.GetPublicKey());

        /// <summary>
        /// Type.
        /// </summary>
        public CertificateType Type => GetterHelpers.Cached(ref m_CachedType, () => X509.GetKeyType());

        /// <summary>
        /// Purposes.
        /// </summary>
        public CertificatePurposes Purposes => GetterHelpers.Cached(ref m_CachedPurposes, () => X509.GetKeyPurposes());

        /// <summary>
        /// Key SHA1. (Identity SHA1 Hash)
        /// </summary>
        public string KeySHA1 => GetterHelpers.Cached(ref m_CachedKeySHA1, () => new CertificateIdentity(Subject, KeyIdentifier).MakeKeySHA1());

        /// <summary>
        /// Ref SHA1. (Reference SHA1 Hash)
        /// </summary>
        public string RefSHA1 => GetterHelpers.Cached(ref m_CachedRefSHA1, () => new CertificateReference(SerialNumber, Issuer.KeyIdentifier).MakeRefSHA1());

        /// <summary>
        /// Subject.
        /// </summary>
        public string Subject => GetterHelpers.Cached(ref m_CachedSubject, () => X509.SubjectDN.ToString());

        /// <summary>
        /// Subject Hash.
        /// </summary>
        public string SubjectHash => GetterHelpers.Cached(ref m_CachedSubjectHash, () => MakeNameHash(X509.SubjectDN));

        /// <summary>
        /// Serial Number.
        /// </summary>
        public string SerialNumber => GetterHelpers.Cached(ref m_CachedSerialNumber, () => X509.SerialNumber.ToString(16));

        /// <summary>
        /// Key Identifier.
        /// </summary>
        public string KeyIdentifier => GetterHelpers.Cached(ref m_CachedKeyIdentifier, () => X509.GetKeyIdentifier());

        /// <summary>
        /// Thumbprint.
        /// </summary>
        public string Thumbprint => GetterHelpers.Cached(ref m_CachedThumbprint, () => X509.GetThumbprint().ToLower());

        /// <summary>
        /// Self.
        /// </summary>
        public CertificateIdentity Self => new CertificateIdentity(this);

        /// <summary>
        /// Issuer.
        /// </summary>
        public CertificateIdentity Issuer => GetterHelpers.Cached(ref m_CachedIssuer, () => new CertificateIdentity(X509.IssuerDN.ToString(), X509.GetIssuerKeyIdentifier()));

        /// <summary>
        /// Subject Hash.
        /// </summary>
        public string IssuerHash => GetterHelpers.Cached(ref m_CachedIssuerHash, () => MakeNameHash(X509.IssuerDN));

        /// <summary>
        /// SANS, aka, Subject Alternative Names.
        /// </summary>
        public string[] Sans => GetterHelpers.Cached(ref m_CachedSans, () => X509.GetSans().ToArray());

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset CreationTime { get; }

        /// <summary>
        /// Expiration Time.
        /// </summary>
        public DateTimeOffset ExpirationTime { get; }

        /// <summary>
        /// Revoke Reason.
        /// If this has no value means that "not identified".
        /// </summary>
        public CertificateRevokeReason? RevokeReason { get; set; }

        /// <summary>
        /// Revoke Time.
        /// If this has no value means that "not identified".
        /// </summary>
        public DateTimeOffset? RevokeTime { get; set; }

        /// <summary>
        /// Indicates whether the certificate is authority or not.
        /// </summary>
        public bool IsAuthority => X509.GetBasicConstraints() >= 0;

        /// <summary>
        /// Indicates whether the certificate is expired or not.
        /// </summary>
        public bool IsExpired => ExpirationTime <= DateTimeOffset.UtcNow;

        /// <summary>
        /// Indicates whether the certificate is self-signed or not.
        /// </summary>
        public bool IsSelfSigned => Issuer.IsExact(this);

        /// <summary>
        /// Indicates whether the certificate's revokation status identified or not.
        /// </summary>
        public bool IsRevokeIdentified => RevokeReason.HasValue;

        /// <summary>
        /// Indicates whether the certificate has public key or not.
        /// </summary>
        public bool HasPublicKey => X509 != null;

        /// <summary>
        /// Indicates whether the certificate has private key or not.
        /// </summary>
        public bool HasPrivateKey => PrivateKey != null;

        /// <summary>
        /// User specific object tag.
        /// </summary>
        public object UserTag { get; set; }

        /// <summary>
        /// Get the key pair.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        internal AsymmetricCipherKeyPair GetKeyPair()
        {
            if (HasPrivateKey == false)
                throw new InvalidOperationException("this certificate has no private key.");

            return new AsymmetricCipherKeyPair(PublicKey, PrivateKey);
        }

        /// <summary>
        /// Create an <see cref="ISignatureFactory"/> instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public ISignatureFactory CreateSignatureFactory(HashAlgorithmType HashAlgorithm = HashAlgorithmType.Default)
        {
            if (HasPrivateKey == false)
                throw new InvalidOperationException("this certificate has no private key.");

            var Factory = PrivateKey.CreateSignatureFactory(HashAlgorithm);
            if (Factory is null)
                throw new NotSupportedException("this certificate's algorithm is not supported.");

            return Factory;
        }

        /// <summary>
        /// Export to `.cer` file bytes.
        /// This will not include private key.
        /// </summary>
        /// <returns></returns>
        public byte[] Export()
        {
            using var x509 = new DX509(X509.GetEncoded());
            return x509.Export(DX509Type.Cert);
        }

        /// <summary>
        /// Export this certificate as pkcs#12 bytes (pfx, p12).
        /// </summary>
        /// <returns></returns>
        public byte[] ExportPfx(bool ExcludePrivateKey = false) => ExportPfx(null, ExcludePrivateKey);

        /// <summary>
        /// Export this certificate as pkcs#12 bytes (pfx, p12).
        /// </summary>
        /// <returns></returns>
        public byte[] ExportPfx(string Password, bool ExcludePrivateKey = false)
        {
            var Store = new CertificateStore();
            Store.Add(this);

            if (ExcludePrivateKey)
            {
                var PrivateKey = this.PrivateKey;
                try
                {
                    this.PrivateKey = null;
                    return Store.Export(Password);
                }

                finally
                {
                    this.PrivateKey = PrivateKey;
                }
            }

            return Store.Export();
        }

        /// <inheritdoc/>
        public bool Equals(Certificate Other)
        {
            if (ReferenceEquals(Other, null))
                return false;

            return new CertificateReference(this).Equals(Other);
        }

        /// <inheritdoc/>
        public override bool Equals(object Obj)
        {
            if (ReferenceEquals(Obj, null))
                return false;

            if (Obj is Certificate Certificate)
                return Equals(Certificate);

            if (Obj is CertificateReference CertId)
                return CertId.Equals(this);

            return base.Equals(Obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return new CertificateReference(this).GetHashCode();
        }

        // ----
        /// <summary>
        /// Eq
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static bool operator ==(Certificate L, Certificate R) => ReferenceEquals(L, null) ? ReferenceEquals(R, null) : L.Equals(R);

        /// <summary>
        /// Neq
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static bool operator !=(Certificate L, Certificate R) => ReferenceEquals(L, null) ? !ReferenceEquals(R, null) : !L.Equals(R);

        /// <summary>
        /// Eq
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static bool operator ==(Certificate L, CertificateReference R) => ReferenceEquals(L, null) ? !R.Validity : L.Equals(R);

        /// <summary>
        /// Neq
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static bool operator !=(Certificate L, CertificateReference R) => ReferenceEquals(L, null) ? R.Validity : !L.Equals(R);
    }
}