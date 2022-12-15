using Microsoft.EntityFrameworkCore;
using NIdentity.Core.Server.Helpers;
using NIdentity.Core.X509.Documents;
using NIdentity.Core.X509.Server.Helpers;
using NIdentity.Core.X509.Server.Repositories.Models;

namespace NIdentity.Core.X509.Server.Repositories
{
    /// <summary>
    /// X509 Context.
    /// </summary>
    public class X509Context
    {
        /// <summary>
        /// Initialize a new <see cref="X509Context"/> instance.
        /// </summary>
        /// <param name="DbContext"></param>
        public X509Context(DbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        /// <summary>
        /// Database Context.
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// Certificates.
        /// </summary>
        public DbSet<DbCertificate> Certificates => DbContext.Set<DbCertificate>();

        /// <summary>
        /// Documents.
        /// </summary>
        public DbSet<DbCertificateDocument> Documents => DbContext.Set<DbCertificateDocument>();

        /// <summary>
        /// Stores.
        /// </summary>
        public DbSet<DbCertificateStore> Stores => DbContext.Set<DbCertificateStore>();

        /// <summary>
        /// Revokations.
        /// </summary>
        public DbSet<DbRevokation> Revokations => DbContext.Set<DbRevokation>();

        /// <summary>
        /// Revokation Inventories.
        /// </summary>
        public DbSet<DbRevokationInventory> RevokationInventories => DbContext.Set<DbRevokationInventory>();

        /// <summary>
        /// Configure <see cref="ModelBuilder"/>.
        /// </summary>
        /// <param name="Mb"></param>
        public void Configure(ModelBuilder Mb)
        {
            Mb.ApplyNotations<X509Context>();
            DbCertificate.Configure(Mb);
            DbCertificateDocument.Configure(Mb);
            DbCertificateStore.Configure(Mb);
            DbRevokation.Configure(Mb);
            Mb.Entity<DbRevokationInventory>();
        }

        /// <summary>
        /// Get the <see cref="DbCertificate"/> by <see cref="CertificateIdentity"/>.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public DbCertificate GetCertificate(CertificateIdentity Identity)
        {
            var Key = Identity.MakeKeySHA1();
            return Certificates.FirstOrDefault(X => X.KeySHA1 == Key);
        }

        /// <summary>
        /// Get the <see cref="DbCertificate"/> by <see cref="CertificateReference"/>.
        /// </summary>
        /// <param name="Reference"></param>
        /// <returns></returns>
        public DbCertificate GetCertificate(CertificateReference Reference)
        {
            var Ref = Reference.MakeRefSHA1();
            return Certificates.FirstOrDefault(X => X.RefSHA1 == Ref);
        }

        /// <summary>
        /// Get the <see cref="DbCertificateDocument"/> by <see cref="DocumentIdentity"/>.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public DbCertificateDocument GetCertificateDocument(DocumentIdentity Identity, long? RevisionNumber = null)
        {
            var Key = Identity.Owner.MakeKeySHA1();
            var Path = DocumentIdentity.NormalizePathName(Identity.PathName);

            if (RevisionNumber.HasValue)
            {
                var RNumber = RevisionNumber.Value;
                return Documents
                    .Where(X => X.KeySHA1 == Key).Where(X => X.PathName == Path)
                    .FirstOrDefault(X => X.RevisionNumber == RNumber);
            }

            return Documents
                .Where(X => X.KeySHA1 == Key)
                .FirstOrDefault(X => X.PathName == Path);
        }

        /// <summary>
        /// Get the <see cref="DbCertificateStore"/> by <see cref="CertificateIdentity"/>.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public DbCertificateStore GetCertificateStore(CertificateIdentity Identity)
        {
            var Key = Identity.MakeKeySHA1();
            return Stores.FirstOrDefault(X => X.KeySHA1 == Key);
        }

        /// <summary>
        /// Get the <see cref="DbCertificateStore"/> by <see cref="CertificateReference"/>.
        /// </summary>
        /// <param name="Reference"></param>
        /// <returns></returns>
        public DbCertificateStore GetCertificateStore(CertificateReference Reference)
        {
            var Key = Reference.MakeRefSHA1();
            return Stores.FirstOrDefault(X => X.RefSHA1 == Key);
        }

        /// <summary>
        /// Create <see cref="DbCertificate"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public bool Create(DbCertificate Certificate) => DbContext.DbCreate(Certificate);

        /// <summary>
        /// Create <see cref="DbCertificateStore"/>.
        /// </summary>
        /// <param name="CertificateStore"></param>
        /// <returns></returns>
        public bool Create(DbCertificateStore CertificateStore) => DbContext.DbCreate(CertificateStore);

        /// <summary>
        /// Create <see cref="DbCertificateDocument"/>.
        /// </summary>
        /// <param name="CertificateDocument"></param>
        /// <returns></returns>
        public bool Create(DbCertificateDocument CertificateDocument) => DbContext.DbCreate(CertificateDocument);

        /// <summary>
        /// Update <see cref="DbCertificate"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public bool Update(DbCertificate Certificate) => DbContext.DbUpdate(Certificate);

        /// <summary>
        /// Update <see cref="DbCertificateDocument"/>.
        /// </summary>
        /// <param name="CertificateDocument"></param>
        /// <returns></returns>
        public bool Update(DbCertificateDocument CertificateDocument) => DbContext.DbUpdate(CertificateDocument);

        /// <summary>
        /// Remove <see cref="DbCertificate"/>.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public bool Remove(DbCertificate Certificate) => DbContext.DbRemove(Certificate);

        /// <summary>
        /// Remove <see cref="DbCertificateStore"/>.
        /// </summary>
        /// <param name="CertificateStore"></param>
        /// <returns></returns>
        public bool RemoveStore(DbCertificateStore CertificateStore) => DbContext.DbRemove(CertificateStore);

        /// <summary>
        /// Remove <see cref="DbCertificateDocument"/>.
        /// </summary>
        /// <param name="CertificateDocument"></param>
        /// <returns></returns>
        public bool Remove(DbCertificateDocument CertificateDocument) => DbContext.DbRemove(CertificateDocument);

        /// <summary>
        /// Remove all <see cref="DbCertificateDocument"/> by <see cref="CertificateIdentity"/>.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public int RemoveDocuments(CertificateIdentity Identity)
        {
            var Key = Identity.MakeKeySHA1();
            var Query = Documents.Where(X => X.KeySHA1 == Key);
            var Count = 0;

            while (true)
            {
                var Docs = Query.Take(100).ToArray();
                if (Docs.Length <= 0)
                    break;

                foreach (var Each in Docs)
                    Count += Remove(Each) ? 1 : 0;
            }

            return Count;
        }

        /// <summary>
        /// Remove all <see cref="DbCertificateDocument"/> by <see cref="CertificateReference"/>.
        /// </summary>
        /// <param name="Reference"></param>
        /// <returns></returns>
        public int RemoveDocuments(CertificateReference Reference)
        {
            var Key = Reference.MakeRefSHA1();
            var Query = Documents.Where(X => X.RefSHA1 == Key);
            var Count = 0;

            while (true)
            {
                var Docs = Query.Take(100).ToArray();
                if (Docs.Length <= 0)
                    break;

                foreach (var Each in Docs)
                    Count += Remove(Each) ? 1 : 0;
            }

            return Count;
        }

    }
}
