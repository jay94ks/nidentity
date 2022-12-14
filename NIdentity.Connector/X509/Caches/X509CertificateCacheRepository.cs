﻿using NIdentity.Core.X509;

namespace NIdentity.Connector.X509.Caches
{
    /// <summary>
    /// Memory cache repository to optimize certificate loading.
    /// </summary>
    public class X509CertificateCacheRepository : IDisposable, ICertificateCacheRepository
    {
        private readonly SemaphoreSlim m_Semaphore = new(1);
        private readonly X509CertificateCache[] m_Caches;

        /// <summary>
        /// Initialize a new <see cref="X509CertificateCacheRepository"/> instance.
        /// </summary>
        /// <param name="MaxCachedCerts"></param>
        public X509CertificateCacheRepository(int MaxCachedCerts)
        {
            m_Caches = new X509CertificateCache[MaxCachedCerts];
            for (var i = 0; i < m_Caches.Length; ++i)
                m_Caches[i] = new X509CertificateCache();
        }

        /// <inheritdoc/>
        public void Dispose() => m_Semaphore.Dispose();

        /// <inheritdoc/>
        public async Task DeleteAsync(Certificate Certificate, CancellationToken Token = default)
        {
            await m_Semaphore.WaitAsync(Token);
            try
            {
                var Item = m_Caches.Where(X => X.Certificate != null).Select((X, i) => (X, i))
                    .FirstOrDefault(X => X.X.Certificate.KeySHA1 == Certificate.KeySHA1);

                if (Item.X != null)
                {
                    Item.X.Certificate = null;
                    Item.X.LastAccessTime = DateTime.Now;
                }
            }

            finally
            {
                m_Semaphore.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetAsync(Certificate Certificate, Action<Certificate> Action, CancellationToken Token = default)
        {
            await m_Semaphore.WaitAsync(Token);
            try
            {
                var Item = m_Caches.Where(X => X.Certificate != null).Select((X, i) => (X, i))
                    .FirstOrDefault(X => X.X.Certificate.KeySHA1 == Certificate.KeySHA1);

                if (Item.X != null)
                {
                    Action?.Invoke(Item.X.Certificate);
                    Item.X.LastAccessTime = DateTime.Now;
                    return;
                }

                var Slot = m_Caches
                    .Select((X, i) => (X, i))
                    .OrderBy(X => X.X.LastAccessTime)
                    .Select(X => X.i).FirstOrDefault();

                m_Caches[Slot].Certificate = Certificate;
                m_Caches[Slot].LastAccessTime = DateTime.Now;
            }

            finally
            {
                m_Semaphore.Release();
            }
        }

        /// <inheritdoc/>
        public async Task SetAsync(Certificate Certificate, CancellationToken Token = default)
        {
            await SetAsync(Certificate, Certificate =>
            {
                Certificate.RevokeReason = Certificate.RevokeReason;
                Certificate.RevokeTime = Certificate.RevokeTime;
            }, Token);
        }

        /// <inheritdoc/>
        public async Task<Certificate> GetAsync(CertificateIdentity Identity, CancellationToken Token = default)
        {
            await m_Semaphore.WaitAsync(Token);
            try
            {
                var Item = m_Caches.Where(X => X.Certificate != null).Select((X, i) => (X, i))
                    .FirstOrDefault(X => X.X.Certificate.KeySHA1 == Identity.MakeKeySHA1());

                if (Item.X != null && (DateTime.Now - Item.X.LastAccessTime).TotalMinutes <= 1)
                {
                    Item.X.LastAccessTime = DateTime.Now;
                    return Item.X.Certificate;
                }

                return null;
            }

            finally
            {
                m_Semaphore.Release();
            }
        }

        /// <inheritdoc/>
        public async Task<Certificate> GetAsync(CertificateReference Reference, CancellationToken Token = default)
        {
            await m_Semaphore.WaitAsync(Token);
            try
            {
                var Item = m_Caches.Where(X => X.Certificate != null).Select((X, i) => (X, i))
                    .FirstOrDefault(X => X.X.Certificate.RefSHA1 == Reference.MakeRefSHA1());

                if (Item.X != null && (DateTime.Now - Item.X.LastAccessTime).TotalMinutes <= 1)
                {
                    Item.X.LastAccessTime = DateTime.Now;
                    return Item.X.Certificate;
                }

                return null;
            }

            finally
            {
                m_Semaphore.Release();
            }
        }
    }
}
