using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace NIdentity.Core.X509.Algorithms
{
    public class EcdsaAlgorithm : Algorithm
    {
        /// <summary>
        /// Initialize a new <see cref="RsaAlgorithm"/> instance.
        /// </summary>
        /// <param name="CurveName"></param>
        public EcdsaAlgorithm(string CurveName)
            => this.CurveName = Validate(CurveName);

        /// <summary>
        /// Ecdsa Curve Name.
        /// </summary>
        public string CurveName { get; }

        /// <summary>
        /// Throw <see cref="NotSupportedException"/> if unsupported ecdsa curve name.
        /// </summary>
        /// <param name="CurveName"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private static string Validate(string CurveName)
        {
            if (EcdsaCurveNames.Contains(CurveName))
                return CurveName;

            throw new NotSupportedException($"the curve, {CurveName} is not supported.");
        }

        /// <inheritdoc/>
        internal override AsymmetricCipherKeyPair GenerateKeyPair()
        {
            var Ecp = SecNamedCurves.GetByName(CurveName);
            var Ecd = new ECDomainParameters(Ecp.Curve, Ecp.G, Ecp.N);
            var KeyGen = new ECKeyPairGenerator();

            KeyGen.Init(new ECKeyGenerationParameters(Ecd, new SecureRandom()));
            return KeyGen.GenerateKeyPair();
        }

    }
}
