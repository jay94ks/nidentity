using Org.BouncyCastle.Asn1.X509;

namespace NIdentity.Core.X509.Helpers
{
    public static class X509RevokeReasonHelpers
    {
        private static readonly Dictionary<CertificateRevokeReason, int> REVOKE_CODES
            = new Dictionary<CertificateRevokeReason, int>()
            {
                { CertificateRevokeReason.None, CrlReason.Unspecified },
                { CertificateRevokeReason.KeyCompromised, CrlReason.KeyCompromise },
                { CertificateRevokeReason.CACompromised, CrlReason.CACompromise },
                { CertificateRevokeReason.AffiliationChanged, CrlReason.AffiliationChanged },
                { CertificateRevokeReason.Superseded, CrlReason.Superseded },
                { CertificateRevokeReason.CessationOfOperation, CrlReason.CessationOfOperation },
                { CertificateRevokeReason.CertificateHold, CrlReason.CertificateHold },
                { CertificateRevokeReason.RemoveFromCrl, CrlReason.RemoveFromCrl },
                { CertificateRevokeReason.PrivilegeWithdrawn, CrlReason.PrivilegeWithdrawn },
                { CertificateRevokeReason.AACompromise, CrlReason.AACompromise }
            };

        /// <summary>
        /// Get the reason number. (See: <see cref="CrlReason"/>.
        /// </summary>
        /// <param name="Reason"></param>
        /// <returns></returns>
        public static int GetReasonNumber(this CertificateRevokeReason Reason)
        {
            if (REVOKE_CODES.TryGetValue(Reason, out int Result))
                return Result;

            return 0;
        }
    }
}
