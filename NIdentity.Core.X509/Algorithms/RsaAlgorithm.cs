using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

namespace NIdentity.Core.X509.Algorithms
{
    /// <summary>
    /// RSA algorithm,
    /// </summary>
    public class RsaAlgorithm : Algorithm
    {
        /// <summary>
        /// Initialize a new <see cref="RsaAlgorithm"/> instance.
        /// </summary>
        /// <param name="KeyLength"></param>
        public RsaAlgorithm(int KeyLength)
            => this.KeyLength = KeyLength;

        /// <summary>
        /// Key Length.
        /// </summary>
        public int KeyLength { get; }

        /// <summary>
        /// Throw <see cref="NotSupportedException"/> if unsupported key length.
        /// </summary>
        /// <param name="KeyLength"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private static int Throw(int KeyLength)
        {
            if (RsaKeyLengths.Contains(KeyLength))
                return KeyLength;

            throw new NotSupportedException($"the key length, {KeyLength} is not supported.");
        }

        /// <inheritdoc/>
        internal override AsymmetricCipherKeyPair GenerateKeyPair()
        {
            var KeyGen = new RsaKeyPairGenerator();

            KeyGen.Init(new KeyGenerationParameters(new SecureRandom(), KeyLength));
            return KeyGen.GenerateKeyPair();
        }

    }
}
