using System.Diagnostics;

namespace NIdentity
{
    public class AppEnv
    {
        private static int GetIntegerEnvironment(string EnvName, int Default)
        {
            var Value = Environment.GetEnvironmentVariable(EnvName);
            if (string.IsNullOrWhiteSpace(Value))
                return Default;

            if (int.TryParse(Value, out var IntValue))
                return IntValue;

            return Default;
        }

        public static readonly string APP_ROOT = Path.GetDirectoryName(typeof(App).Assembly.Location);

        /// <summary>
        /// MySQL Host.
        /// </summary>
        public static readonly string MYSQL_HOST
            = Environment.GetEnvironmentVariable(nameof(MYSQL_HOST))
            ?? "127.0.0.1";

        /// <summary>
        /// MySQL Port.
        /// </summary>
        public static readonly int MYSQL_PORT
            = GetIntegerEnvironment(nameof(MYSQL_PORT), 3510);

        /// <summary>
        /// MySQL User.
        /// </summary>
        public static readonly string MYSQL_USER
            = Environment.GetEnvironmentVariable(nameof(MYSQL_USER))
            ?? "nidentity";

        /// <summary>
        /// MySQL Password.
        /// </summary>
        public static readonly string MYSQL_PASS
            = Environment.GetEnvironmentVariable(nameof(MYSQL_PASS))
            ?? "nidentity1!";

        /// <summary>
        /// MySQL Database.
        /// </summary>
        public static readonly string MYSQL_DB
            = Environment.GetEnvironmentVariable(nameof(MYSQL_DB))
            ?? "nidentity";

        /// <summary>
        /// SSL cert file.
        /// </summary>
        public static readonly string GENESIS_CMDS
            = Environment.GetEnvironmentVariable(nameof(GENESIS_CMDS))
            ?? "generated/genesis_cmds.json";

        /// <summary>
        /// Http Base URL.
        /// </summary>
        public static readonly string HTTP_BASE_URL
            = Environment.GetEnvironmentVariable(nameof(HTTP_BASE_URL))
            ?? "http://127.0.0.1:7000/";

        /// <summary>
        /// SSL cert file.
        /// </summary>
        public static readonly string SSL_CERT_FILE
            = Environment.GetEnvironmentVariable(nameof(SSL_CERT_FILE))
            ?? "data/https_cert.pfx";

        /// <summary>
        /// CRL generation term.
        /// </summary>
        public static readonly int CRL_TERM
            = GetIntegerEnvironment(nameof(CRL_TERM), 30);

        /// <summary>
        /// CER generation term.
        /// </summary>
        public static readonly int CER_TERM
            = GetIntegerEnvironment(nameof(CER_TERM), 30);

        /// <summary>
        /// CER generation term.
        /// </summary>
        public static readonly int MAX_CACHE_KEYS
            = GetIntegerEnvironment(nameof(MAX_CACHE_KEYS), 1024);

        /// <summary>
        /// Run as super mode or not.
        /// </summary>
        public static readonly int RUN_AS_SUPER
            = GetIntegerEnvironment(nameof(RUN_AS_SUPER), 0);
    }
}
