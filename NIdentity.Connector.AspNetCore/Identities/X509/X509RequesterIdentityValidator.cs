using Newtonsoft.Json;
using NIdentity.Connector.AspNetCore.Abstractions;
using NIdentity.Connector.X509;
using NIdentity.Core.X509.Commands.Certificates;
using System.Security.Cryptography;
using System.Text;

namespace NIdentity.Connector.AspNetCore.Identities.X509
{
    /// <summary>
    /// X509 requester validator.
    /// </summary>
    public sealed class X509RequesterIdentityValidator : IRequesterIdentityValidator
    {
        /// <summary>
        /// Get the options for X509 identity.
        /// </summary>
        /// <param name="Requester"></param>
        /// <returns></returns>
        private X509RequesterIdentityOptions GetOptions(Requester Requester)
            => Requester.HttpContext.RequestServices.GetService<X509RequesterIdentityOptions>()
            ?? throw new InvalidOperationException("to use X509 identity recognizer, add X509 identity services.");

        /// <summary>
        /// Get the <see cref="RemoteCommandExecutor"/> to query certificate's metadata.
        /// </summary>
        /// <param name="Requester"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private RemoteCommandExecutor GetExecutor(Requester Requester)
            => Requester.HttpContext.RequestServices.GetService<RemoteCommandExecutor>()
            ?? throw new InvalidOperationException("to use X509 identity recognizer, add remote command executor services.");

        /// <summary>
        /// Get the <see cref="IRequesterIdentityCacheRepository"/> to load/save caches.
        /// </summary>
        /// <param name="Requester"></param>
        /// <returns></returns>
        private IRequesterIdentityCacheRepository GetCacheRepository(Requester Requester)
            => Requester.HttpContext.RequestServices.GetService<IRequesterIdentityCacheRepository>();

        /// <inheritdoc/>
        public async Task<bool> ValidateAsync(Requester Requester, RequesterIdentity Input)
        {
            if (Input is not X509RequesterIdentity Identity)
                return false;

            var Options = GetOptions(Requester);
            var CacheRepository = Options.DisableCache == false
                ? GetCacheRepository(Requester) : null;

            if (CacheRepository != null)
                return await ValidateWithCache(Requester, Identity, Options, CacheRepository);

            return await ValidateUsingExecutor(Requester, Identity, CacheRepository);
        }

        /// <summary>
        /// Validate with cache repository and pass it to <see cref="ValidateUsingExecutor"/> method if cache-miss.
        /// </summary>
        /// <param name="Requester"></param>
        /// <param name="Identity"></param>
        /// <param name="Options"></param>
        /// <param name="CacheRepository"></param>
        /// <returns></returns>
        private async Task<bool> ValidateWithCache(
            Requester Requester, X509RequesterIdentity Identity,
            X509RequesterIdentityOptions Options,
            IRequesterIdentityCacheRepository CacheRepository)
        {
            var Aborter = Requester.HttpContext.RequestAborted;
            
            // --> load cache and check its expiration.
            var CacheKey = MakeCacheKey(Identity);
            var CacheText = await CacheRepository.LoadAsync(Identity, CacheKey, Aborter);

            if (string.IsNullOrWhiteSpace(CacheText) == false)
            {
                var Cache = RestoreFromJson(CacheText);
                if (Cache != null)
                {
                    if ((DateTimeOffset.UtcNow - Cache.CacheTime) > Options.CacheExpiration)
                        return await ValidateUsingExecutor(Requester, Identity, CacheRepository);

                    Identity.Metadata = Cache;
                    return true;
                }
            }

            return await ValidateUsingExecutor(Requester, Identity, CacheRepository);
        }

        /// <summary>
        /// Make cache key from identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        private static string MakeCacheKey(X509RequesterIdentity Identity)
        {
            using var Sha256 = SHA256.Create();
            var Temp = Sha256.ComputeHash(Encoding.UTF8.GetBytes(Identity.ToString()));
            return string.Join("", Temp.Select(X => X.ToString("x2")));
        }

        /// <summary>
        /// Validate using the executor.
        /// </summary>
        /// <param name="Requester"></param>
        /// <param name="Identity"></param>
        /// <param name="CacheRepository"></param>
        /// <returns></returns>
        private async Task<bool> ValidateUsingExecutor(
            Requester Requester, X509RequesterIdentity Identity,
            IRequesterIdentityCacheRepository CacheRepository)
        {
            var Aborter = Requester.HttpContext.RequestAborted;
            var Executor = GetExecutor(Requester);

            try
            {
                var Metadata = await Executor.X509.GetCertificateMetaAsync(Identity.Recognized, Aborter);
                if (Metadata != null && Metadata.Thumbprint == Identity.Recognized.Thumbprint)
                {
                    // --> identity verified.
                    if (CacheRepository is null)
                        return true;

                    // --> generate cache here.
                    var CacheKey = MakeCacheKey(Identity);
                    var Cache = RestoreFromJson(JsonConvert.SerializeObject(Metadata));

                    Identity.Metadata = Metadata;
                    Cache.CacheTime = DateTimeOffset.UtcNow;
                    await CacheRepository.SaveAsync(Identity, CacheKey, StoreIntoJson(Cache), Aborter);
                    return true;
                }
            }

            catch
            {
            }

            // --> no data in the NIdentity server or thumbprint mismatch.
            return false;
        }

        /// <summary>
        /// Cache Item.
        /// </summary>
        private class CacheItem : X509GetCertificateMetaCommand.Result
        {
            /// <summary>
            /// Cache Time
            /// </summary>
            [JsonProperty("cache_time")]
            public DateTimeOffset CacheTime { get; set; } = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Restore the metadata from JSON.
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        private CacheItem RestoreFromJson(string Json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Json) == false)
                    return JsonConvert.DeserializeObject<CacheItem>(Json);
            }

            catch { }
            return null;
        }

        /// <summary>
        /// Store the metadata into JSON.
        /// </summary>
        /// <param name="Cache"></param>
        /// <returns></returns>
        private string StoreIntoJson(CacheItem Cache)
        {
            if (Cache is null)
                return string.Empty;

            return JsonConvert.SerializeObject(Cache);
        }

    }
}
