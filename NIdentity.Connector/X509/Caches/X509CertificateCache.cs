using NIdentity.Core.X509;

namespace NIdentity.Connector.X509.Caches
{
    /// <summary>
    /// Cached Certificate.
    /// </summary>
    internal class X509CertificateCache
    {
        /// <summary>
        /// Certificate.
        /// </summary>
        public Certificate Certificate { get; set; }

        /// <summary>
        /// Last Access Time.
        /// </summary>
        public DateTime LastAccessTime { get; set; } = DateTime.Now;
    }
}
