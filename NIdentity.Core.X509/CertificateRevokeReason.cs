namespace NIdentity.Core.X509
{
    public enum CertificateRevokeReason
    {
        /// <summary>
        /// Not revoked.
        /// </summary>
        None = 0,

        /// <summary>
        /// Key Compromised, Revoked.
        /// </summary>
        KeyCompromised,

        /// <summary>
        /// CA Compromised, Revoked.
        /// </summary>
        CACompromised,

        /// <summary>
        /// Affiliation Changed, Revoked. (Organization changed
        /// </summary>
        AffiliationChanged,

        /// <summary>
        /// Superseded, Revoked. (Replaced to other certificate)
        /// </summary>
        Superseded,

        /// <summary>
        /// Cessation Of Operation, Revoked. (No more service active)
        /// </summary>
        CessationOfOperation,

        /// <summary>
        /// Certificate Hold, Revoked. (Not active yet)
        /// </summary>
        CertificateHold,

        /// <summary>
        /// Remove From Crl, Revoked. (Removal)
        /// </summary>
        RemoveFromCrl,

        /// <summary>
        /// Privilege Withdrawn, Revoked. (Contract closed)
        /// </summary>
        PrivilegeWithdrawn,

        /// <summary>
        /// AA Compromise, Revoked.
        /// </summary>
        AACompromise,
    }
}
