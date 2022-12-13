using NIdentity.Core.X509.Server.Commands;

namespace NIdentity.Core.X509.Server
{
    public class X509ServerSettings
    {
        /// <summary>
        /// Cache path.
        /// </summary>
        public string CachePath { get; set; }

        /// <summary>
        /// CRL regeneration term.
        /// </summary>
        public TimeSpan CrlTerm { get; set; } = TimeSpan.FromDays(30);

        /// <summary>
        /// CER regeneration term.
        /// </summary>
        public TimeSpan CerTerm { get; set; } = TimeSpan.FromDays(30);

        /// <summary>
        /// Maximum Cached Keys.
        /// </summary>
        public int MaxCachedKeys { get; set; } = 1024;

        /// <summary>
        /// Set whether the server is running as super mode or not.
        /// </summary>
        public bool IsSuperMode { get; set; } = false;

        /// <summary>
        /// Http Ocsp endpoint.
        /// </summary>
        public string HttpOcsp { get; set; } = "/api/infra/ocsp";

        /// <summary>
        /// Http CRL distribution endpoint.
        /// </summary>
        public string HttpCRL { get; set; } = "/api/infra/crls";

        /// <summary>
        /// Http CA issuer's certificate endpoint.
        /// </summary>
        public string HttpCAIssuers { get; set; } = "/api/infra/cers";

        /// <summary>
        /// Executor Settings.
        /// </summary>
        public X509ExecutorSettings ExecutorSettings { get; } = new X509ExecutorSettings();
    }
}
