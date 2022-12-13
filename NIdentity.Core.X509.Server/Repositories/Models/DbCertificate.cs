using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NIdentity.Core.X509.Server.Repositories.Models
{

    /// <summary>
    /// Certificate.
    /// </summary>
    [Table("Certificates")]
    public class DbCertificate : DbBase
    {
        /// <summary>
        /// Configure <see cref="DbCertificate"/> to the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public static void Configure(ModelBuilder Mb)
        {
            var Entity = Mb.Entity<DbCertificate>();

            // -- selection keys. (PK, UK)
            Entity.HasKey(X => X.KeySHA1).HasName("PK_ID");
            Entity.HasIndex(X => X.RefSHA1, "UK_RF").IsUnique(true);

            // --> additional selection keys. (UK_*)
            Entity.HasIndex(new[] { nameof(Subject), nameof(KeyIdentifier) }, "UK_SNK").IsUnique(true);
            Entity.HasIndex(new[] { nameof(SubjectHash), nameof(KeyIdentifier) }, "UK_SHK").IsUnique(true);
            Entity.HasIndex(new[] { nameof(SerialNumber), nameof(IssuerHash) }, "UK_SIH").IsUnique(true);
            Entity.HasIndex(new[] { nameof(SerialNumber), nameof(IssuerKeyIdentifier) }, "UK_SKI").IsUnique(true);

            // -- indices to cover managemental purposes.
            Entity.HasIndex(X => X.RevokeReason, "BY_RCODE");
            Entity.HasIndex(X => X.CreationTime, "BY_CTIME");
            Entity.HasIndex(X => X.ExpirationTime, "BY_XTIME");
            Entity.HasIndex(X => X.RevokeTime, "BY_RTIME");

            Entity.HasIndex(X => X.Subject, "BY_SUBJECT");
            Entity.HasIndex(X => X.SubjectHash, "BY_SUBJECT_HASH");

            Entity.HasIndex(X => X.Issuer, "BY_ISSUER");
            Entity.HasIndex(X => X.IssuerHash, "BY_ISSUER_HASH");

            Entity.HasIndex(X => X.KeyIdentifier, "BY_KEY");
            Entity.HasIndex(X => X.SerialNumber, "BY_SERIAL_NUMBER");
            Entity.HasIndex(X => X.IssuerKeyIdentifier, "BY_ISSUER_KEY");
        }

        /// <summary>
        /// Make a <see cref="DbCertificate"/> instance.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public static DbCertificate Make(Certificate Certificate)
        {
            var Identity = Certificate.Self;
            var Reference = new CertificateReference(Certificate);
            return new DbCertificate
            {
                KeySHA1 = Certificate.KeySHA1,
                RefSHA1 = Certificate.RefSHA1,
                KeyIdentifier = Identity.KeyIdentifier,
                Subject = Identity.Subject,
                SubjectHash = Certificate.SubjectHash,
                IssuerKeyIdentifier = Reference.IssuerKeyIdentifier,
                Issuer = Certificate.Issuer.Subject,
                IssuerHash = Certificate.IssuerHash,
                SerialNumber = Reference.SerialNumber,
                CreationTime = Certificate.CreationTime,
                ExpirationTime = Certificate.ExpirationTime,
                RevokeReason = Certificate.IsRevokeIdentified
                    ? Certificate.RevokeReason.Value
                    : CertificateRevokeReason.None,
                RevokeTime = Certificate.IsRevokeIdentified
                    ? Certificate.RevokeTime.Value
                    : DateTimeOffset.UnixEpoch,
                Type = Certificate.Type,
                Thumbprint = Certificate.Thumbprint
            };
        }

        /// <summary>
        /// (PK_ID) Key Selector: Identity.MakeKeySHA1().
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string KeySHA1 { get; set; }

        /// <summary>
        /// (UK_RF) Key Selector: Reference.MakeRefSHA1()
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string RefSHA1 { get; set; }

        /// <summary>
        /// Key Identifier.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string KeyIdentifier { get; set; }

        /// <summary>
        /// Subject.
        /// </summary>
        [MaxLength(LEN_SUBJECT)]
        public string Subject { get; set; }

        /// <summary>
        /// Subject Hash.
        /// </summary>
        [MaxLength(LEN_NAME_HASH)]
        public string SubjectHash { get; set; }

        /// <summary>
        /// Issuer's Key Identifier.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string IssuerKeyIdentifier { get; set; }

        /// <summary>
        /// Issuer Name.
        /// </summary>
        [MaxLength(LEN_SUBJECT)]
        public string Issuer { get; set; }

        /// <summary>
        /// Issuer Hash.
        /// </summary>
        [MaxLength(LEN_NAME_HASH)]
        public string IssuerHash { get; set; }

        /// <summary>
        /// Serial Number.
        /// </summary>
        [MaxLength(LEN_SERIAL_NUMBER)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Expiration Time.
        /// </summary>
        public DateTimeOffset ExpirationTime { get; set; }

        /// <summary>
        /// Indicates whether this certificate is CA or not.
        /// </summary>
        [NotMapped]
        public bool IsAuthority => Type != CertificateType.Leaf;

        /// <summary>
        /// Indicate whether this certificate is revoked or not.
        /// </summary>
        [NotMapped]
        public bool IsRevoked => RevokeReason != CertificateRevokeReason.None;

        /// <summary>
        /// Revoke Reason
        /// </summary>
        public CertificateRevokeReason RevokeReason { get; set; }

        /// <summary>
        /// Revoke Time.
        /// </summary>
        public DateTimeOffset RevokeTime { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Certificate Type. (Root, Immediate, Leaf)
        /// </summary>
        public CertificateType Type { get; set; } = CertificateType.Leaf;

        /// <summary>
        /// Pre-calculated Thumbprint.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string Thumbprint { get; set; }
    }
}
