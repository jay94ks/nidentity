using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIdentity.Core.X509.Server.Repositories.Models
{
    [Table("Revokations")]
    public class DbRevokation : DbBase
    {
        /// <summary>
        /// Configure <see cref="DbCertificate"/> to the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public static void Configure(ModelBuilder Mb)
        {
            var Entity = Mb.Entity<DbRevokation>();

            // -- selection keys. (UK)
            Entity.HasIndex(new[] { nameof(AuthorityKeySHA1), nameof (RefSHA1) }, "UK_RF").IsUnique(true);

            // -- indices to cover managemental purposes.
            Entity.HasIndex(X => X.AuthorityKeySHA1, "BY_ISSUER");
            Entity.HasIndex(X => X.Revision, "BY_REV");
        }

        /// <summary>
        /// (PK) Number.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Number { get; set; }

        /// <summary>
        /// (PK) <see cref="Certificate.KeySHA1"/>.
        /// to track authority.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string AuthorityKeySHA1 { get; set; }

        /// <summary>
        /// (UK) <see cref="Certificate.RefSHA1"/>.
        /// to restore <see cref="CertificateReference"/>.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string RefSHA1 { get; set; }

        /// <summary>
        /// (IDX) Inventory's revision number when this entry created.
        /// </summary>
        public long Revision { get; set; }

        /// <summary>
        /// Serial Number to restore <see cref="CertificateReference"/>.
        /// </summary>
        [MaxLength(LEN_SERIAL_NUMBER)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Issuer's Key Identifier to restore <see cref="CertificateReference"/>.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string IssuerKeyIdentifier { get; set; }

        /// <summary>
        /// Time when this revokation created.
        /// </summary>
        public DateTimeOffset Time { get; set; }

        /// <summary>
        /// Reason why this revokation created.
        /// </summary>
        public CertificateRevokeReason Reason { get; set; }
    }
}
