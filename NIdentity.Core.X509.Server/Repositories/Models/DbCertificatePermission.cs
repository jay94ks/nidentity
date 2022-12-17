using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIdentity.Core.X509.Server.Repositories.Models
{
    /// <summary>
    /// Authorizations.
    /// </summary>
    [Table("CertificatePermissions")]
    public class DbCertificatePermission : DbBase
    {
        public static void Configure(ModelBuilder Mb)
        {
            var Entity = Mb.Entity<DbCertificatePermission>();

            // -- duplication filter purpose unique key.
            Entity.HasIndex(new[] { nameof(KeySHA1), nameof(AccessKeySHA1) }, "UK_ANTI_DUP").IsUnique(true);

            // --
            Entity.HasIndex(X => X.KeySHA1, "BY_KEYSHA1");
            Entity.HasIndex(X => X.AccessKeySHA1, "BY_ACCSHA1");
            Entity.HasIndex(X => X.AuthorityKeySHA1, "BY_ATHSHA1");
        }

        /// <summary>
        /// (PK, AUTO_INC) Number.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long No { get; set; }

        /// <summary>
        /// (UK, ANTI_DUP) Certificate's KeySHA1 to define.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string KeySHA1 { get; set; }

        /// <summary>
        /// (UK, ANTI_DUP) Certificate's Key SHA1 who tries to access.
        /// If this value is empty string, it means that <see cref="KeySHA1"/> and its children default.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string AccessKeySHA1 { get; set; } = string.Empty;

        /// <summary>
        /// Authority of this <see cref="KeySHA1"/>.
        /// This is used to optimize accesses for query speed.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string AuthorityKeySHA1 { get; set; } = string.Empty;

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset LastWriteTime { get; set; }

        /// <summary>
        /// Indicates whether the authority of <see cref="KeySHA1"/> can interfere this intermediate CA's certificates or not.
        /// If this option set true, all accesses from the authority of <see cref="KeySHA1"/> will be denied.
        /// And this option can only be altered by exact owner.
        /// </summary>
        public bool CanAuthorityInterfere { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="AccessKeySHA1"/> can generate intermediate or leafs or not.
        /// </summary>
        /// <returns></returns>
        public bool CanGenerate { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="AccessKeySHA1"/> can list certificates or not.
        /// </summary>
        public bool CanList { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="AccessKeySHA1"/> can revoke certificates or not.
        /// </summary>
        public bool CanRevoke { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="AccessKeySHA1"/> can delete certificates or not.
        /// </summary>
        public bool CanDelete { get; set; } = true;
    }
}
