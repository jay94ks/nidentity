namespace NIdentity.Core.X509.Revokations
{
    /// <summary>
    /// Revokation.
    /// </summary>
    public class Revokation
    {
        /// <summary>
        /// Reference of certificate.
        /// </summary>
        public CertificateReference Reference { get; set; }

        /// <summary>
        /// Reason why revoked.
        /// </summary>
        public CertificateRevokeReason Reason { get; set; } 

        /// <summary>
        /// Time.
        /// </summary>
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Revision Number when this revokation entry wrote.
        /// </summary>
        public long Revision { get; set; }
    }
}
