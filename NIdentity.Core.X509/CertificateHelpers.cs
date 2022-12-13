using NIdentity.Core.X509.Helpers;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NIdentity.Core.X509
{
    public static class CertificateHelpers
    {
        /// <summary>
        /// Convert <see cref="Certificate"/> to <see cref="X509Certificate2"/> instance.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static X509Certificate2 ToDotNetCert(this Certificate Certificate)
        {
            using var X509Pub = new X509Certificate2(Certificate.X509.GetEncoded(),
                string.Empty, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);

            switch (Certificate.PrivateKey)
            {
                case RsaPrivateCrtKeyParameters Rsa:
                    return X509Pub.CopyWithPrivateKey(Rsa.ToRSA());

                case ECPrivateKeyParameters Ecdsa:
                    {
                        using var Writer = new StringWriter();
                        {
                            var Pem = new PemWriter(Writer);
                            Pem.WriteObject(Certificate.PrivateKey);
                            Pem.WriteObject(Certificate.X509);
                            Pem.Writer.Flush();
                        }

                        var PemText = string.Join("\r\n", Writer.ToString()
                            .Split('\n').Select(X => X.Trim()));

                        var Ec = ECDsa.Create();
                        Ec.ImportFromPem(PemText);

                        return X509Pub.CopyWithPrivateKey(Ec);
                    }

                default:
                    throw new InvalidOperationException("not supported algorithm");
            }
        }


        /// <summary>
        /// Export the certificate as PEM file.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public static byte[] ExportPem(this Certificate Certificate)
        {
            using var Writer = new StringWriter();
            {
                var Pem = new PemWriter(Writer);
                if (Certificate.HasPrivateKey)
                    Pem.WriteObject(Certificate.PrivateKey);

                Pem.WriteObject(Certificate.X509);
                Pem.Writer.Flush();
            }

            var PemText = string.Join("\r\n", Writer
                .ToString().Split('\n').Select(X => X.Trim()));

            return Encoding.UTF8.GetBytes(PemText);
        }
    }
}
