using System.Security.Cryptography;

namespace NIdentity.Core.Server.Helpers
{
    public class GuidHelpers
    {
        /// <summary>
        /// Generate <see cref="Guid"/> using RNG.
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid()
        {
            using (var Rng = RandomNumberGenerator.Create())
            {
                Span<byte> Bytes = stackalloc byte[16];
                Rng.GetNonZeroBytes(Bytes);
                return new Guid(Bytes);
            }
        }
    }
}
