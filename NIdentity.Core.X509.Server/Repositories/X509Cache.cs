namespace NIdentity.Core.X509.Server.Repositories
{
    /// <summary>
    /// Cached Certificate.
    /// </summary>
    internal class X509Cache
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
