using NIdentity.Core.X509.Helpers;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using System.Text;

namespace NIdentity.Core.X509
{
    /// <summary>
    /// Certificate Store.
    /// </summary>
    public class CertificateStore
    {
        private readonly HashSet<Certificate> m_Certificates;

        /// <summary>
        /// Initialize a new <see cref="CertificateStore"/> instance.
        /// </summary>
        public CertificateStore() => m_Certificates = new HashSet<Certificate>();

        /// <summary>
        /// Certificates that stored in this object.
        /// </summary>
        public IReadOnlyCollection<Certificate> Certificates => m_Certificates;

        /// <summary>
        /// Get the certificate by its key identifier.
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <returns></returns>
        public Certificate GetBySerialNumber(string SerialNumber) => m_Certificates.FirstOrDefault(X => X.SerialNumber == SerialNumber);

        /// <summary>
        /// Get the certificate by its key identifier.
        /// </summary>
        /// <param name="KeyIdentifier"></param>
        /// <returns></returns>
        public Certificate GetByKeyIdentifier(string KeyIdentifier) => m_Certificates.FirstOrDefault(X => X.KeyIdentifier.Equals(KeyIdentifier, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Get the certificate by its thumbprint.
        /// </summary>
        /// <param name="Thumbprint"></param>
        /// <returns></returns>
        public Certificate GetByThumbprint(string Thumbprint) => m_Certificates.FirstOrDefault(X => X.Thumbprint.Equals(Thumbprint, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// Get the certificate chain for the specified certificate.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public Certificate[] GetChain(Certificate Certificate)
        {
            if (Certificate is null || (Certificate = GetByThumbprint(Certificate.Thumbprint)) is null)
                throw new ArgumentNullException(nameof(Certificate));

            var ChainedCertificates = new List<Certificate>();
            var CurrentCertificate = Certificate;

            while (CurrentCertificate != null)
            {
                ChainedCertificates.Add(CurrentCertificate);

                if (CurrentCertificate.IsSelfSigned)
                    break;

                CurrentCertificate = GetByKeyIdentifier(CurrentCertificate.Issuer.KeyIdentifier);
            }

            return ChainedCertificates.ToArray();
        }

        /// <summary>
        /// Add a certificate.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">can not take meta-data only certificte</exception>
        public bool Add(Certificate Certificate)
        {
            if (Certificate is null)
                throw new ArgumentNullException(nameof(Certificate));

            if (GetByThumbprint(Certificate.Thumbprint) != null)
                return false;

            m_Certificates.Add(Certificate);
            return true;
        }

        /// <summary>
        /// Remove a certificate.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public bool Remove(Certificate Certificate)
        {
            if (Certificate is null)
                throw new ArgumentNullException(nameof(Certificate));

            if ((Certificate = GetByThumbprint(Certificate.Thumbprint)) != null)
            {
                m_Certificates.Remove(Certificate);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Copy this certificate store to <see cref="Pkcs12Store"/>.
        /// </summary>
        /// <param name="Pkcs"></param>
        internal Pkcs12Store ToPkcs12Store(Pkcs12Store Pkcs = null)
        {
            Pkcs = Pkcs ?? new Pkcs12Store();
            foreach (var Each in m_Certificates)
            {
                var Entry = new X509CertificateEntry(Each.X509);
                Pkcs.SetCertificateEntry(Each.Subject, Entry);

                if (Each.HasPrivateKey)
                {
                    var KeyEntry = new AsymmetricKeyEntry(Each.PrivateKey);
                    var KeyChainEntries = GetChain(Each)
                        .Select(X => X == Each ? Entry : new X509CertificateEntry(X.X509))
                        .ToArray();

                    Pkcs.SetKeyEntry(Each.Subject, KeyEntry, KeyChainEntries);
                }
            }

            return Pkcs;
        }

        /// <summary>
        /// Copy certificates from <see cref="Pkcs12Store"/>.
        /// </summary>
        /// <param name="Pkcs"></param>
        internal CertificateStore FromPkcs12Store(Pkcs12Store Pkcs)
        {
            foreach (var Each in Pkcs.Aliases)
            {
                if (Each is null)
                    continue;

                var Alias = Each.ToString();
                var Entry = Pkcs.GetCertificate(Alias);
                if (Entry is null || Entry.Certificate is null)
                    continue;

                var KeyEntry = Pkcs.GetKey(Alias);
                var Key = KeyEntry != null ? KeyEntry.Key : null;
                Add(new Certificate(Entry.Certificate, Key));
            }

            return this;
        }

        /// <summary>
        /// Import the certificate store from pkcs12 store bytes.
        /// </summary>
        /// <param name="PkcsBytes"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static CertificateStore Import(byte[] PkcsBytes, string Password = null)
        {
            var Pkcs = new Pkcs12Store();
            try
            {
                using (var Temp = new MemoryStream(PkcsBytes))
                    Pkcs.Load(Temp, (Password ?? string.Empty).ToCharArray());
            }

            catch (Exception Error)
            {
                if (string.IsNullOrWhiteSpace(Error.Message) == false && Error.Message.Contains("password"))
                    throw new InvalidOperationException($"PFX password is not matched.");

                throw new FormatException($"{nameof(PkcsBytes)} is not PKCS#12 store bytes.");
            }

            return new CertificateStore().FromPkcs12Store(Pkcs);
        }

        /// <summary>
        /// Import PEM text.
        /// </summary>
        /// <param name="PemText"></param>
        /// <returns></returns>
        public static CertificateStore ImportPem(string PemText)
        {
            var Store = new CertificateStore();

            // --> normalize sparators.
            PemText = string.Join("\r\n", PemText.Split(' ', '\n', '\r')
                .Where(X => !string.IsNullOrWhiteSpace(X)));

            using (var TempReader = new StringReader(PemText))
            {
                var Pem = new PemReader(TempReader);
                var Certs = new List<X509Certificate>();
                var KeyPairs = new List<AsymmetricCipherKeyPair>();

                while (true)
                {
                    var Obj = Pem.ReadObject();
                    if (Obj is null)
                        break;

                    if (Obj is X509Certificate Cert) Certs.Add(Cert);
                    else if (Obj is AsymmetricCipherKeyPair KeyPair)
                        KeyPairs.Add(KeyPair);
                }

                var Results = KeyPairs
                    .Select(X => (KeyPair: X, Ski: new SubjectKeyIdentifierStructure(X.Public)))
                    .Select(X => (X.KeyPair, Ski: X.Ski.GetKeyIdentifier()))
                    .Select(X => (X.KeyPair, Ski: X.Ski.Select(Y => Y.ToString("x2"))))
                    .Select(X => (X.KeyPair, Ski: string.Join("", X.Ski)))
                    .Select(X => (X.KeyPair, Cert: Certs.FirstOrDefault(Y => Y.GetKeyIdentifier() == X.Ski)))
                    .Where(X => X.Cert != null).Select(X => new Certificate(X.Cert, X.KeyPair.Private))
                    .ToArray().AsEnumerable();

                Certs.RemoveAll(X => Results.FirstOrDefault(Y => Y.X509 == X) != null);
                Results = Results.Concat(Certs.Select(X => new Certificate(X)));

                foreach (var Each in Results)
                    Store.Add(Each);
            }

            return Store;
        }

        /// <summary>
        /// Import the certificate store from pem bytes.
        /// </summary>
        /// <param name="PemBytes"></param>
        /// <returns></returns>
        public static CertificateStore ImportPem(byte[] PemBytes)
            => ImportPem(Encoding.UTF8.GetString(PemBytes));

        /// <summary>
        /// Export this certificate store to pkcs12 store bytes (pfx, p12).
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
        public byte[] Export(string Password = null)
        {
            using (var Temp = new MemoryStream())
            {
                ToPkcs12Store().Save(Temp, (Password ?? string.Empty).ToArray(), new SecureRandom());
                return Temp.ToArray();
            }
        }

    }
}