using NIdentity.Core.X509;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using System.Security.Cryptography;
using System.Text;

namespace NIdentity.Core.X509.Helpers
{
    internal static class X509PropertyHelpers
    {
        /// <summary>
        /// Make SHA-1 hash.
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static byte[] HashSha1(byte[] Value)
        {
            using var Sha = SHA1.Create();
            return Sha.ComputeHash(Value);
        }

        /// <summary>
        /// Get the key identifier of the <see cref="X509Certificate"/> instance.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public static string GetKeyIdentifier(this X509Certificate Certificate)
        {
            var Ski = Certificate.GetExtensionValue(X509Extensions.SubjectKeyIdentifier);
            if (Ski != null)
            {
                var Octets = Asn1OctetString.GetInstance(Ski).GetOctets();
                var Identifier = SubjectKeyIdentifier.GetInstance(Octets).GetKeyIdentifier();
                return string.Join("", Identifier.Select(X => X.ToString("x2")));
            }

            return null;
        }

        /// <summary>
        /// Get the issuer key identifier of the <see cref="X509Certificate"/> instance.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public static string GetIssuerKeyIdentifier(this X509Certificate Certificate)
        {
            var Ski = Certificate.GetExtensionValue(X509Extensions.AuthorityKeyIdentifier);
            if (Ski != null)
            {
                var Octets = Asn1OctetString.GetInstance(Ski).GetOctets();
                var Identifier = AuthorityKeyIdentifier.GetInstance(Octets).GetKeyIdentifier();
                return string.Join("", Identifier.Select(X => X.ToString("x2")));
            }

            return null;
        }

        /// <summary>
        /// Get the thumbprint of the <see cref="X509Certificate"/> instance.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public static string GetThumbprint(this X509Certificate Certificate)
        {
            var Der = Certificate.GetEncoded();
            return string.Join("", HashSha1(Der).Select(X => X.ToString("x2")));
        }

        /// <summary>
        /// Get the <see cref="CertificatePurposes"/> from <see cref="X509Certificate"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public static CertificatePurposes GetKeyPurposes(this X509Certificate Certificate)
        {
            var Value = Certificate.GetExtensionValue(X509Extensions.ExtendedKeyUsage);
            var Eku = ExtendedKeyUsage.GetInstance(Value.GetOctets());
            var Purpose = CertificatePurposes.Unknown;

            Purpose
                |= Eku.HasKeyPurposeId(KeyPurposeID.IdKPServerAuth)
                || Eku.HasKeyPurposeId(KeyPurposeID.IdKPClientAuth)
                ? CertificatePurposes.Networking : CertificatePurposes.Unknown;

            Purpose
                |= Eku.HasKeyPurposeId(KeyPurposeID.IdKPEmailProtection)
                || Eku.HasKeyPurposeId(KeyPurposeID.IdKPCodeSigning)
                ? CertificatePurposes.Protection : CertificatePurposes.Unknown;

            Purpose
                |= Eku.HasKeyPurposeId(KeyPurposeID.IdKPTimeStamping)
                || Eku.HasKeyPurposeId(KeyPurposeID.IdKPOcspSigning)
                ? CertificatePurposes.Stamping : CertificatePurposes.Unknown;

            Purpose
                |= Eku.HasKeyPurposeId(KeyPurposeID.IdKPIpsecEndSystem)
                ? CertificatePurposes.IPSecEndSystem : CertificatePurposes.Unknown;

            Purpose
                |= Eku.HasKeyPurposeId(KeyPurposeID.IdKPIpsecTunnel)
                ? CertificatePurposes.IPSecTunnel : CertificatePurposes.Unknown;

            Purpose
                |= Eku.HasKeyPurposeId(KeyPurposeID.IdKPIpsecUser)
                ? CertificatePurposes.IPSecUser : CertificatePurposes.Unknown;

            return Purpose;
        }

        /// <summary>
        /// Get the <see cref="CertificateType"/> from <see cref="X509Certificate"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public static CertificateType GetKeyType(this X509Certificate Certificate)
        {
            var Value = Certificate.GetBasicConstraints();
            if (Value >= 0)
            {
                if (Certificate.IssuerDN.ToString() == Certificate.SubjectDN.ToString() &&
                    Certificate.GetIssuerKeyIdentifier() == Certificate.GetKeyIdentifier())
                {
                    return CertificateType.Root;
                }

                return CertificateType.Immediate;
            }

            return CertificateType.Leaf;
        }
    }
}
