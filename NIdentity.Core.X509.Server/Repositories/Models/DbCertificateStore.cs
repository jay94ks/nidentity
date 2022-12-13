using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NIdentity.Core.Helpers;

namespace NIdentity.Core.X509.Server.Repositories.Models
{
    [Table("CertificateStores")]
    public class DbCertificateStore : DbBase
    {
        /// <summary>
        /// Configure <see cref="DbCertificateStore"/> to the <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public static void Configure(ModelBuilder Mb)
        {
            // -- selection key. (PK)
            Mb.Entity<DbCertificateStore>().HasKey(X => X.KeySHA1).HasName("PK_ID");
            Mb.Entity<DbCertificateStore>().HasIndex(X => X.RefSHA1, "UK_RF");
        }

        /// <summary>
        /// Make a <see cref="DbCertificateStore"/> instance.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public static DbCertificateStore Make(Certificate Certificate, bool ExcludePrivateKey = false)
        {
            var Type = Certificate.HasPrivateKey == true && !ExcludePrivateKey
                ? DbCertificateStoreType.PfxBytes
                : DbCertificateStoreType.CerBytes;

            var Data = Certificate.HasPrivateKey == true && !ExcludePrivateKey
                ? Aes256Helpers.Encrypt(Certificate.ExportPfx(), Certificate.KeySHA1)
                : Aes256Helpers.Encrypt(Certificate.Export(), Certificate.KeySHA1);

            return new DbCertificateStore
            {
                KeySHA1 = Certificate.KeySHA1,
                RefSHA1 = Certificate.RefSHA1,
                Type = Type,
                Base64 = Convert.ToBase64String(Data)
            };
        }

        /// <summary>
        /// (PK_ID) Key Selector: Identity.KeySHA1().
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string KeySHA1 { get; set; }

        /// <summary>
        /// (UK_RF) Key Selector: Identity.RefSHA1().
        /// </summary>
        [MaxLength(LEN_SHA1_WITH_SAFE)]
        public string RefSHA1 { get; set; }

        /// <summary>
        /// Store Type.
        /// </summary>
        public DbCertificateStoreType Type { get; set; } = DbCertificateStoreType.PfxBytes;

        /// <summary>
        /// Indicates whether the key store has private key or not.
        /// </summary>
        [NotMapped]
        public bool HasPrivateKey => Type == DbCertificateStoreType.PfxBytes;

        /// <summary>
        /// Data in base64.
        /// </summary>
        [Column(TypeName = "LONGTEXT")]
        public string Base64 { get; set; }

        /// <summary>
        /// Load from <see cref="Base64"/>.
        /// </summary>
        /// <returns></returns>
        public CertificateStore Load()
        {
            if (string.IsNullOrWhiteSpace(Base64))
                return null;

            try
            {
                var Data = Aes256Helpers.Decrypt(Convert.FromBase64String(Base64), KeySHA1);
                if (Type == DbCertificateStoreType.PfxBytes)
                    return CertificateStore.Import(Data);

                var Store = new CertificateStore();
                Store.Add(Certificate.Import(Data));
                return Store;
            }

            catch { }
            return null;
        }
    }
}
