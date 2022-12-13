using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NIdentity.Core.X509.Documents;

namespace NIdentity.Core.X509.Server.Repositories.Models
{
    [Table("CertificateDocuments")]
    public class DbCertificateDocument : DbBase
    {
        /// <summary>
        /// Configure <see cref="DbCertificateDocument"/> to the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public static void Configure(ModelBuilder Mb)
        {
            var Entity = Mb.Entity<DbCertificateDocument>();

            // -- selection key. (PK)
            Entity.HasKey(nameof(KeySHA1), nameof(PathName)).HasName("PK_ID");
            Entity.HasIndex(new[] { nameof(RefSHA1), nameof(PathName) }, "UK_RF").IsUnique(true);
            Entity.HasIndex(X => X.ParentPathName, "BY_PARENT");

            // -- indices to cover managemental purposes.
            Entity.HasIndex(X => X.Access, "BY_ACCESS");
            Entity.HasIndex(X => X.CreationTime, "BY_CTIME");
            Entity.HasIndex(X => X.LastWriteTime, "BY_WCODE");
            Entity.HasIndex(X => X.MimeType, "BY_MIME_TYPE");
        }

        /// <summary>
        /// (PK_ID) Key Selector: Identity.KeySHA1().
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string KeySHA1 { get; set; }

        /// <summary>
        /// (UK_RF) Key Selector: Reference.MakeRefSHA1()
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string RefSHA1 { get; set; }

        /// <summary>
        /// Path Name.
        /// </summary>
        [MaxLength(128)]
        public string PathName { get; set; }

        /// <summary>
        /// Parent Path Name.
        /// </summary>
        [MaxLength(128)]
        public string ParentPathName { get; set; }

        /// <summary>
        /// Indicates whether the document has children or not.
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Document Access Mode.
        /// </summary>
        public DocumentAccess Access { get; set; }

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Last Write Time.
        /// </summary>
        public DateTimeOffset LastWriteTime { get; set; }

        /// <summary>
        /// Revision Number.
        /// </summary>
        [ConcurrencyCheck]
        public long RevisionNumber { get; set; }

        /// <summary>
        /// Mime Type.
        /// </summary>
        [MaxLength(48)]
        public string MimeType { get; set; } = "text/plain";

        /// <summary>
        /// Data in base64.
        /// </summary>
        [Column(TypeName = "LONGTEXT")]
        public string Data { get; set; }
    }
}
