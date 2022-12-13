using Org.BouncyCastle.Crypto;

namespace NIdentity.Core.X509.Algorithms
{
    /// <summary>
    /// Certificate Algorithm.
    /// </summary>
    public abstract class Algorithm
    {
        /// <summary>
        /// Supported ECDSA Curve Names.
        /// </summary>
        public static readonly string[] EcdsaCurveNames
            = new string[] { /*"secp256r1", "secp256k1", "secp384r1"*/ };

        /// <summary>
        /// Supported RSA Key Lengths.
        /// </summary>
        public static readonly int[] RsaKeyLengths
            = new int[] { 2048, 4096 };

        /// <summary>
        /// Make RSA certificate algorithm instance.
        /// </summary>
        /// <param name="KeyLength"></param>
        /// <returns></returns>
        public static Algorithm MakeRsa(int KeyLength) => new RsaAlgorithm(KeyLength);

        /// <summary>
        /// Make ECDSA certificate algorithm instance.
        /// </summary>
        /// <param name="CurveName"></param>
        /// <returns></returns>
        public static Algorithm MakeEcdsa(string CurveName) => new EcdsaAlgorithm(CurveName);

        /// <summary>
        /// To ban inheritance from external codes.
        /// </summary>
        internal Algorithm() { }

        /// <summary>
        /// Generate a new key pair.
        /// </summary>
        /// <returns></returns>
        internal abstract AsymmetricCipherKeyPair GenerateKeyPair();
    }
}
