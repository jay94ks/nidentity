namespace NIdentity.Connector.AspNetCore.Identities.X509
{
    public class X509RequesterIdentityOptions
    {
        /// <summary>
        /// Disable X509 requester identity recognizer.
        /// </summary>
        public bool Disable { get; set; }

        /// <summary>
        /// Disable X509 forwarder passed headers.
        /// </summary>
        public bool DisableForwarderHeaders { get; set; }

        /// <summary>
        /// Disable Certificate Metadata Caches.
        /// </summary>
        public bool DisableCache { get; set; }

        /// <summary>
        /// Certificate Metadata Cache Expiration.
        /// </summary>
        public TimeSpan CacheExpiration { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Header Name which is used to pass certificate from forwsrd proxy.
        /// </summary>
        public string PemBase64Header = "SSL_CLIENT_CERT";

        /// <summary>
        /// Header Name which is used to pass validation result from forwsrd proxy.
        /// </summary>
        public string ResultHeader = "SSL_CLIENT_VERIFY";

        /// <summary>
        /// Expected value that should be present at <see cref="ResultHeader"/>.
        /// </summary>
        public string ExpectedResultValue = "SUCCESS";
    }
}
