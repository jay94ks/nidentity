using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NIdentity.Core.X509.Server.Repositories.Models
{
    [Table("RevokationInventories")]
    public class DbRevokationInventory : DbBase
    {
        /// <summary>
        /// (PK) <see cref="Certificate.KeySHA1"/>.
        ///  to restore <see cref="CertificateIdentity"/>.
        /// </summary>
        [Key, MaxLength(LEN_SHA1_WITH_SAFE)]
        public string KeySHA1 { get; set; }

        /// <summary>
        /// Subject to restore <see cref="CertificateIdentity"/>.
        /// </summary>
        [MaxLength(LEN_SUBJECT)]
        public string Subject { get; set; }

        /// <summary>
        /// Key Identifier to restore <see cref="CertificateIdentity"/>.
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string KeyIdentifier { get; set; }

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Last Write Time.
        /// </summary>
        public DateTimeOffset LastWriteTime { get; set; }

        /// <summary>
        /// Revision.
        /// </summary>
        [ConcurrencyCheck]
        public long Revision { get; set; }
    }
}
