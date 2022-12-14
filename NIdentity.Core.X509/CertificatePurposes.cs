namespace NIdentity.Core.X509
{
    /// <summary>
    /// Purposes of certificate.
    /// </summary>
    public enum CertificatePurposes
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Server Auth, Client Auth.
        /// </summary>
        Networking = 1,

        /// <summary>
        /// Email Protection, Code Signing
        /// </summary>
        Protection = 2,

        /// <summary>
        /// Time Stamping, Ocsp Signing.
        /// </summary>
        Stamping = 4,

        /// <summary>
        /// IPSec, End-System.
        /// </summary>
        IPSecEndSystem = 8,

        /// <summary>
        /// IPSec, Tunnel.
        /// </summary>
        IPSecTunnel = 16,

        /// <summary>
        /// IPSec, User.
        /// </summary>
        IPSecUser = 32,
    }
}
