using NIdentity.Core.X509.Algorithms;
using NIdentity.Core.X509.Helpers;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using System.Security.Cryptography;
using System.Text;

namespace NIdentity.Core.X509.Revokations
{
    /// <summary>
    /// Builds CRL files.
    /// </summary>
    public class CRLBuilder
    {
        /// <summary>
        /// CRL number.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Issuer certificate.
        /// </summary>
        public Certificate Issuer { get; set; }

        /// <summary>
        /// Hash Algorithm.
        /// </summary>
        public HashAlgorithmType HashAlgorithm { get; set; } = HashAlgorithmType.Default;

        /// <summary>
        /// This update time. (default: before 1 day)
        /// </summary>
        public DateTimeOffset ThisUpdate { get; set; } = DateTime.Now.Subtract(TimeSpan.FromDays(2));

        /// <summary>
        /// Next update time. (default: after 2 days)
        /// </summary>
        public DateTimeOffset NextUpdate { get; set; } = DateTime.Now.AddDays(2);

        /// <summary>
        /// Revokation Inventory.
        /// </summary>
        public RevokationInventory Inventory { get; set; } = new RevokationInventory();

        /// <summary>
        /// Certificates to include.
        /// </summary>
        public List<Revokation> Revokations { get; } = new List<Revokation>();

        /// <summary>
        /// Hash as SHA1.
        /// </summary>
        /// <param name="Sha1"></param>
        /// <returns></returns>
        private string HashSha1(string Sha1)
        {
            using var Sha = SHA1.Create();
            var Temp = Sha.ComputeHash(Encoding.UTF8.GetBytes(Sha1));
            return string.Join("", Temp.Select(X => X.ToString("x2")));
        }

        /// <summary>
        /// Build CRL bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] Build()
        {
            if (Issuer is null)
                throw new InvalidOperationException("No issuer information specified.");

            if (Issuer.HasPrivateKey == false)
                throw new InvalidOperationException("To generate CRL bytes, issuer's private key is required.");

            var Generator = new X509V2CrlGenerator();
            Generator.SetIssuerDN(new X509Name(Issuer.Subject));

            if (Inventory != null)
                ThisUpdate = Inventory.CreationTime.Subtract(TimeSpan.FromDays(1));

            Generator.SetThisUpdate(ThisUpdate.UtcDateTime);
            Generator.SetNextUpdate(NextUpdate.UtcDateTime);

            var CrlNumber = MakeCrlNumber();
            foreach (var Each in Revokations.OrderBy(X => X.Reference.SerialNumber))
            {
                var Reason = Each.Reason;
                var Time = Each.Time.UtcDateTime;

                Generator.AddCrlEntry(
                    new BigInteger(Each.Reference.SerialNumber, 16),
                    Time, Reason.GetReasonNumber());
            }

            Generator.AddExtension(X509Extensions.CrlNumber, false, new CrlNumber(CrlNumber));
            Generator.AddExtension(X509Extensions.AuthorityKeyIdentifier, false,
                new AuthorityKeyIdentifierStructure(Issuer.X509));

            return Generator.Generate(Issuer.PrivateKey.CreateSignatureFactory(HashAlgorithm)).GetEncoded();
        }

        private BigInteger MakeCrlNumber()
        {
            BigInteger CrlNumber = null;
            try
            {
                if (string.IsNullOrWhiteSpace(Number))
                {
                    var SerialNumbers = string.Join(", ",
                        Revokations.Select(X => X.Reference.SerialNumber));

                    CrlNumber = new BigInteger(HashSha1(string.Join(": ",
                        Issuer.KeySHA1, ThisUpdate.ToString(), NextUpdate.ToString(),
                        SerialNumbers)), 16);
                }

                else
                    CrlNumber = new BigInteger(Number, 16);
            }
            catch { }
            if (CrlNumber is null)
                throw new InvalidOperationException("Number should be hexadecimal number.");
            return CrlNumber;
        }
    }
}
