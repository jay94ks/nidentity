namespace NIdentity.Core.X509.Server.Ocsp
{
    /// <summary>
    /// Ocsp Certificate Status.
    /// </summary>
    internal class OcspCertificateStatus
    {
        /// <summary>
        /// Initialize a new <see cref="OcspCertificateStatus"/> instance.
        /// </summary>
        /// <param name="Identity"></param>
        public OcspCertificateStatus(OcspCertificateIdentity Identity)
        {
            this.Identity = Identity;
        }

        /// <summary>
        /// Certificate Identity.
        /// </summary>
        public OcspCertificateIdentity Identity { get; }

        /// <summary>
        /// Reason why the certificate revoked.
        /// </summary>
        public CertificateRevokeReason? Reason { get; set; }

        /// <summary>
        /// Time when the reason set.
        /// </summary>
        public DateTimeOffset? Time { get; set; }
    }
}
