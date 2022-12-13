namespace NIdentity.Core.X509
{
    public enum CertificateType
    {
        /// <summary>
        /// DigitalSignature, KeyCertSign, CrlSign
        /// </summary>
        Root = 0,

        /// <summary>
        /// DigitalSignature, KeyCertSign, CrlSign
        /// </summary>
        Immediate,

        /// <summary>
        /// DigitalSignature, KeyEncipherment
        /// </summary>
        Leaf
    }
}
