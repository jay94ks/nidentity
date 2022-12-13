namespace NIdentity.Core.X509.Authority
{
    /// <summary>
    /// Issuer's Authority Access Information Type.
    /// </summary>
    public enum AuthorityAccessType
    {
        /// <summary>
        /// Ocsp Server Uri.
        /// </summary>
        OcspServerUri,

        /// <summary>
        /// Crl Distribution Point Uri.
        /// </summary>
        CrlDistributionPointUri,

        /// <summary>
        /// Authority Certificate Uri.
        /// </summary>
        AuthorityCertificateUri
    }
}
