using System.Security.Cryptography;
using System.Text;

namespace NIdentity.Core.Helpers
{
    /// <summary>
    /// AES256 encryption helpers.
    /// </summary>
    public class Aes256Helpers
    {
        private const int KEY_LEN = 256 / 8;
        private const int IV_LEN = 256 / 2 / 8;

        private static readonly byte[] DEFAULT_KEY = MakeKey(typeof(Aes256Helpers).FullName);
        private static readonly byte[] DEFAULT_IV = MakeKey(typeof(Aes256Helpers).FullName + ", IV");

        /// <summary>
        /// Make the encryption key bytes.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        internal static byte[] MakeKey(string Key)
        {
            var Kb = Encoding.UTF8.GetBytes(Key);
            using (var Sha = SHA256.Create())
                Kb = Sha.ComputeHash(Kb);

            if (Kb.Length != KEY_LEN)
                Array.Resize(ref Kb, KEY_LEN);

            return Kb;
        }

        /// <summary>
        /// Make the encryption key bytes.
        /// </summary>
        /// <param name="Iv"></param>
        /// <returns></returns>
        internal static byte[] MakeIv(string Iv)
        {
            var Kb = Encoding.UTF8.GetBytes(Iv);
            using (var Sha = SHA256.Create())
                Kb = Sha.ComputeHash(Kb);

            if (Kb.Length != IV_LEN)
                Array.Resize(ref Kb, IV_LEN);

            return Kb;
        }

        /// <summary>
        /// Encrypt bytes using AES256/cbc algorithm.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] Data) => Transform(Data, false);

        /// <summary>
        /// Decrypt bytes using aES256/cbc algorithm.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] Data) => Transform(Data, true);

        /// <summary>
        /// Encrypt bytes using AES256/cbc algorithm.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] Data, string Key) => Transform(Data, false, Key);

        /// <summary>
        /// Decrypt bytes using aES256/cbc algorithm.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] Data, string Key) => Transform(Data, true, Key);

        /// <summary>
        /// Encrypt bytes using AES256/cbc algorithm.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static string EncryptB64(byte[] Data) => Convert.ToBase64String(Transform(Data, false), Base64FormattingOptions.None);

        /// <summary>
        /// Decrypt bytes using aES256/cbc algorithm.
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static byte[] DecryptB64(string Data) => Transform(Convert.FromBase64String(Data), true);

        /// <summary>
        /// Encrypt bytes using AES256/cbc algorithm.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string EncryptB64(byte[] Data, string Key) => Convert.ToBase64String(Transform(Data, false, Key), Base64FormattingOptions.None);

        /// <summary>
        /// Decrypt bytes using aES256/cbc algorithm.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static byte[] DecryptB64(string Data, string Key) => Transform(Convert.FromBase64String(Data), true, Key);

        /// <summary>
        /// Transform Input bytes.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Decrypt"></param>
        /// <returns></returns>
        private static byte[] Transform(byte[] Data, bool Decrypt)
        {
            using var Aes = CreateAes();
            using var Enc = Decrypt
                ? Aes.CreateDecryptor()
                : Aes.CreateEncryptor();

            using var Output = new MemoryStream();
            using (var Temp = new CryptoStream(Output, Enc, CryptoStreamMode.Write))
                Temp.Write(Data, 0, Data.Length);

            return Output.ToArray();
        }

        /// <summary>
        /// Transform Input bytes.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="Decrypt"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static byte[] Transform(byte[] Data, bool Decrypt, string Key)
        {
            using var Aes = CreateAes(
                Key != null ? MakeKey(Key) : null,
                Key != null ? MakeIv($"{Key},IV") : null);

            using var Enc = Decrypt
                ? Aes.CreateDecryptor()
                : Aes.CreateEncryptor();

            using var Output = new MemoryStream();
            using (var Temp = new CryptoStream(Output, Enc, CryptoStreamMode.Write))
                Temp.Write(Data, 0, Data.Length);

            return Output.ToArray();
        }

        /// <summary>
        /// Create <see cref="Aes"/> from settings.
        /// </summary>
        /// <returns></returns>
        private static Aes CreateAes(byte[] Key = null, byte[] IV = null)
        {
            var Crypt = Aes.Create();

            Crypt.KeySize = 256;
            Crypt.BlockSize = 128;
            Crypt.Mode = CipherMode.CBC;
            Crypt.Padding = PaddingMode.PKCS7;

            Crypt.Key = Key ?? DEFAULT_KEY;
            Crypt.IV = IV ?? DEFAULT_IV;
            return Crypt;
        }
    }
}
