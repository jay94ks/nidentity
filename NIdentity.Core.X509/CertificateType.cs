namespace NIdentity.Core.X509
{
    /// <summary>
    /// Certificate type.
    /// </summary>
    public enum CertificateType
    {
        /// <summary>
        /// DigitalSignature, KeyCertSign, CrlSign
        /// </summary>
        Root = 0,

        /// <summary>
        /// DigitalSignature, KeyCertSign, CrlSign
        /// </summary>
        Intermedidate,

        /// <summary>
        /// DigitalSignature, KeyEncipherment
        /// </summary>
        Leaf
    }
}
