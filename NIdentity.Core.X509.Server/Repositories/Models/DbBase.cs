namespace NIdentity.Core.X509.Server.Repositories.Models
{
    public abstract class DbBase
    {
        public const int LEN_SHA256 = 256 / 4;
        public const int LEN_SHA1 = 160 / 4;

        public const int LEN_SHA1_WITH_SAFE = LEN_SHA1 + 1;

        public const int LEN_SUBJECT = 255;
        public const int LEN_NAME_HASH = LEN_SHA1 + 1;
        public const int LEN_SERIAL_NUMBER = 256 / 4 + 1;
    }
}
