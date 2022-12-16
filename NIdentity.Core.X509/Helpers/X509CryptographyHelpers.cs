using NIdentity.Core.X509.Algorithms;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System.Runtime.Versioning;
using System.Security.Cryptography;

namespace NIdentity.Core.X509.Helpers
{
    /// <summary>
    /// Cryptography helpers for X509 certificates
    /// </summary>
    public static class X509CryptographyHelpers
    {
        /// <summary>
        /// Create an <see cref="ISignatureFactory"/> instance.
        /// </summary>
        /// <param name="PrivateKey"></param>
        /// <param name="HashAlgorithm"></param>
        /// <returns></returns>
        public static ISignatureFactory CreateSignatureFactory(this AsymmetricKeyParameter PrivateKey, HashAlgorithmType HashAlgorithm = HashAlgorithmType.Default)
        {
            if (PrivateKey is null)
                throw new ArgumentNullException(nameof(PrivateKey));

            if (PrivateKey.IsPrivate == false)
                throw new ArgumentException("the signature factory can only be created with private key.");

            var Identifier = HashAlgorithm.ToDer(PrivateKey);
            if (Identifier != null)
                return new Asn1SignatureFactory(Identifier.ToString(), PrivateKey);

            return null;
        }

        /// <summary>
        /// Get the <see cref="DerObjectIdentifier"/> for hash algorithm and specified private key.
        /// </summary>
        /// <param name="Algorithm"></param>
        /// <param name="Private"></param>
        /// <returns></returns>
        private static DerObjectIdentifier ToDer(this HashAlgorithmType Algorithm, AsymmetricKeyParameter Private)
        {
            switch(Private)
            {
                case ECPrivateKeyParameters:
                    {
                        switch(Algorithm)
                        {
                            case HashAlgorithmType.Sha224:
                                return X9ObjectIdentifiers.ECDsaWithSha224;

                            case HashAlgorithmType.Default:
                            case HashAlgorithmType.Sha256:
                                return X9ObjectIdentifiers.ECDsaWithSha256;

                            case HashAlgorithmType.Sha384:
                                return X9ObjectIdentifiers.ECDsaWithSha384;

                            case HashAlgorithmType.Sha512:
                                return X9ObjectIdentifiers.ECDsaWithSha512;

                            default:
                                break;
                        }
                        break;
                    }
                case RsaPrivateCrtKeyParameters:
                    {
                        switch (Algorithm)
                        {
                            case HashAlgorithmType.Sha224:
                                return PkcsObjectIdentifiers.Sha224WithRsaEncryption;

                            case HashAlgorithmType.Default:
                            case HashAlgorithmType.Sha256:
                                return PkcsObjectIdentifiers.Sha256WithRsaEncryption;

                            case HashAlgorithmType.Sha384:
                                return PkcsObjectIdentifiers.Sha384WithRsaEncryption;

                            case HashAlgorithmType.Sha512:
                                return PkcsObjectIdentifiers.Sha512WithRsaEncryption;

                            default:
                                break;
                        }
                        break;
                    }
                default:
                    break;
            }

            return null;
        }

        /// <summary>
        /// Convert <see cref="RsaPrivateCrtKeyParameters"/> to <see cref="RSA"/> object.
        /// </summary>
        /// <param name="rsaKey"></param>
        /// <returns></returns>
        public static RSA ToRSA(this RsaPrivateCrtKeyParameters rsaKey)
        {
            var RsaParams = MakeRsaParams(rsaKey);
            if (OperatingSystem.IsWindows())
                return ToRsa(RsaParams);

            return RSA.Create(RsaParams);
        }

        /// <summary>
        /// Make <see cref="RSAParameters"/> from <see cref="RsaPrivateCrtKeyParameters"/>.
        /// </summary>
        /// <param name="privKey"></param>
        /// <returns></returns>
        private static RSAParameters MakeRsaParams(RsaPrivateCrtKeyParameters privKey)
        {
            var Rsa = new RSAParameters();
            Rsa.Modulus = privKey.Modulus.ToByteArrayUnsigned();
            Rsa.Exponent = privKey.PublicExponent.ToByteArrayUnsigned();
            Rsa.P = privKey.P.ToByteArrayUnsigned();
            Rsa.Q = privKey.Q.ToByteArrayUnsigned();
            Rsa.D = BigIntegers.AsUnsignedByteArray(Rsa.Modulus.Length, privKey.Exponent);
            Rsa.DP = BigIntegers.AsUnsignedByteArray(Rsa.P.Length, privKey.DP);
            Rsa.DQ = BigIntegers.AsUnsignedByteArray(Rsa.Q.Length, privKey.DQ);
            Rsa.InverseQ = BigIntegers.AsUnsignedByteArray(Rsa.Q.Length, privKey.QInv);
            return Rsa;
        }

        /// <summary>
        /// Make <see cref="RSA"/> from <see cref="RSAParameters"/> object.
        /// </summary>
        /// <param name="Rsa"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        private static RSA ToRsa(RSAParameters Rsa)
        {
            var Csp = new CspParameters();
            Csp.KeyContainerName = string.Format("BouncyCastle-{0}", Guid.NewGuid());
            return ToRsa(Rsa, Csp);
        }

        /// <summary>
        /// Make <see cref="RSA"/> from <see cref="RSAParameters"/> and <see cref="CspParameters"/> objects.
        /// </summary>
        /// <param name="Rsa"></param>
        /// <param name="Csp"></param>
        /// <returns></returns>
        [SupportedOSPlatform("windows")]
        private static RSA ToRsa(RSAParameters Rsa, CspParameters Csp)
        {
            var Rsp = new RSACryptoServiceProvider(Csp);
            Rsp.ImportParameters(Rsa);
            return Rsp;
        }

    }
}
