using Microsoft.EntityFrameworkCore;
using NIdentity.Core.X509.Server.Ocsp;
using NIdentity.Core.X509.Server.Repositories.Models;

namespace NIdentity.Core.X509.Server.Repositories
{
    /// <summary>
    /// X509 Repository.
    /// </summary>
    public class X509Repository : IMutableCertificateRepository
    {
        private readonly ICertificateCacheRepository m_CacheRepository;
        private readonly X509Context m_X509Context;

        /// <summary>
        /// Initialize a new <see cref="X509Repository"/> instance.
        /// </summary>
        /// <param name="X509Context"></param>
        public X509Repository(X509Context X509Context, ICertificateCacheRepository CacheRepository)
        {
            m_CacheRepository = CacheRepository;
            m_X509Context = X509Context;
        }

        /// <inheritdoc/>
        public async Task<Certificate> LoadAsync(CertificateReference Identity, CancellationToken Token = default)
        {
            var Cert = await m_CacheRepository.GetAsync(Identity);
            if (Cert is null)
            {
                var DbCert = m_X509Context.GetCertificate(Identity);
                if (DbCert is null) return null;

                var DbStore = m_X509Context.GetCertificateStore(Identity);
                if (DbStore is null) return null;

                var Store = DbStore.Load();
                if (Store is null) return null;

                Cert = Store.GetByKeyIdentifier(DbCert.KeyIdentifier);
                if (DbCert.IsRevoked)
                {
                    Cert.RevokeReason = DbCert.RevokeReason;
                    Cert.RevokeTime = DbCert.RevokeTime;
                }

                await m_CacheRepository.SetAsync(Cert);
            }

            return Cert;
        }

        /// <inheritdoc/>
        public async Task<Certificate> LoadAsync(CertificateIdentity Identity, CancellationToken Token = default)
        {
            var Cert = await m_CacheRepository.GetAsync(Identity);
            if (Cert is null)
            {
                var DbCert = m_X509Context.GetCertificate(Identity);
                if (DbCert is null) return null;

                var DbStore = m_X509Context.GetCertificateStore(Identity);
                if (DbStore is null) return null;

                var Store = DbStore.Load();
                if (Store is null) return null;

                Cert = Store.GetByKeyIdentifier(DbCert.KeyIdentifier);
                if (DbCert.IsRevoked)
                {
                    Cert.RevokeReason = DbCert.RevokeReason;
                    Cert.RevokeTime = DbCert.RevokeTime;
                }

                await m_CacheRepository.SetAsync(Cert);
            }

            return Cert;
        }

        /// <inheritdoc/>
        public async Task<Certificate> LoadAsync(OcspCertificateIdentity Identity, CancellationToken Token = default)
        {
            if (Identity.TryGetCertificateReference(out var Reference))
                return await LoadAsync(Reference, Token);

            if (Identity.IsWithNameHash)
            {
                var SerialNumber = Identity.SerialNumber;
                var IssuerHash = Identity.IssuerNameHash;
                var DbIdentity = m_X509Context.Certificates
                    .Where(X => X.SerialNumber == SerialNumber)
                    .Where(X => X.IssuerHash == IssuerHash)
                    .Select(X => new { X.Subject, X.KeyIdentifier, X.KeySHA1 })
                    .FirstOrDefault();

                if (DbIdentity != null)
                {
                    return await LoadAsync(new CertificateIdentity(
                        DbIdentity.Subject, DbIdentity.KeyIdentifier, DbIdentity.KeySHA1));
                }
            }

            return null;
        }

        /// <inheritdoc/>
        public async Task<bool> StoreAsync(Certificate Certificate, bool ExcludePrivateKey = false, CancellationToken Token = default)
        {
            if (Certificate is null)
                throw new ArgumentNullException(nameof(Certificate));

            try
            {
                var DbCert = DbCertificate.Make(Certificate);
                if (m_X509Context.Create(DbCert))
                {
                    try
                    {
                        var DbStore = DbCertificateStore.Make(Certificate, ExcludePrivateKey);
                        if (m_X509Context.Create(DbStore))
                        {
                            await m_CacheRepository.SetAsync(Certificate);
                            return true;
                        }
                    }

                    catch { }
                    m_X509Context.Remove(DbCert);
                }
            }

            catch { }
            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> RevokeAsync(CertificateReference Identity, CertificateRevokeReason Reason, CancellationToken Token = default)
        {
            try
            {
                var DbCert = m_X509Context.GetCertificate(Identity);
                if (DbCert != null)
                {
                    DbCert.RevokeReason = Reason;
                    DbCert.RevokeTime = DateTimeOffset.UtcNow;
                    if (m_X509Context.Update(DbCert))
                    {
                        await UpdateFromCache(Identity, X =>
                        {
                            if (Reason != CertificateRevokeReason.None)
                            {
                                X.RevokeReason = Reason;
                                X.RevokeTime = DateTimeOffset.UtcNow;
                            }

                            else
                            {
                                X.RevokeReason = null;
                                X.RevokeTime = null;
                            }
                        });
                        return true;
                    }
                }
            }

            catch { }
            return false;
        }

        /// <inheritdoc/>
        private async Task UpdateFromCache(CertificateReference Identity, Action<Certificate> Updater)
        {
            var Cache = await m_CacheRepository.GetAsync(Identity);
            if (Cache != null)
            {
                await m_CacheRepository.SetAsync(Cache, Updater);
            }
        }

        /// <inheritdoc/>
        private async Task RemoveFromCache(CertificateReference Identity)
        {
            var Cache = await m_CacheRepository.GetAsync(Identity);
            if (Cache != null)
                await m_CacheRepository.DeleteAsync(Cache);
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(CertificateReference Identity, CancellationToken Token = default)
        {
            try
            {
                var DbCert = m_X509Context.GetCertificate(Identity);
                if (DbCert != null && m_X509Context.Remove(DbCert))
                {
                    await RemoveFromCache(Identity);

                    try
                    {
                        var DbStore = m_X509Context.GetCertificateStore(Identity);
                        if (DbStore != null) m_X509Context.RemoveStore(DbStore);
                    }

                    catch { }
                    // --
                    m_X509Context.RemoveDocuments(Identity);
                    return true;
                }
            }

            catch { }
            return false;
        }

        /// <inheritdoc/>
        public Task<int> CountAsync(Certificate Authority, CancellationToken Token = default)
        {
            if (Authority is null)
                throw new ArgumentNullException(nameof(Authority));

            var Subject = Authority.Subject;
            var KeyIdentifier = Authority.KeyIdentifier;
            var KeySHA256 = Authority.KeySHA1;

            var Count = m_X509Context.Certificates
                .Where(X => X.KeySHA1 != KeySHA256)
                .Where(X => X.IssuerKeyIdentifier == KeyIdentifier)
                .Where(X => X.Issuer == Subject)
                .Count();

            return Task.FromResult(Count);
        }

        /// <inheritdoc/>
        public async Task<Certificate[]> FindAsync(Certificate Authority, int Offset, int Count, CancellationToken Token = default)
        {
            if (Authority is null)
                throw new ArgumentNullException(nameof(Authority));

            //var Issuer = await LoadAsync(Authority.Issuer, Token);

            var Subject = Authority.Subject;
            var KeyIdentifier = Authority.KeyIdentifier;
            var KeySHA1 = Authority.KeySHA1;

            var Identities = m_X509Context.Certificates
                .Where(X => X.KeySHA1 != KeySHA1)
                .Where(X => X.IssuerKeyIdentifier == KeyIdentifier)
                .Where(X => X.Issuer == Subject).Select(X => new { X.KeyIdentifier, X.Subject, X.KeySHA1 })
                .Skip(Offset).Take(Count).AsEnumerable()
                .Select(X => new CertificateIdentity(X.Subject, X.KeyIdentifier, X.KeySHA1))
                .ToArray();

            var Certs = new List<Certificate>();
            foreach (var Each in Identities)
                Certs.Add(await LoadAsync(Each, Token));

            Certs.RemoveAll(X => ReferenceEquals(X, null));
            return Certs.ToArray();
        }
    }
}
