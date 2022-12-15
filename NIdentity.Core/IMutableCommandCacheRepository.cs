namespace NIdentity.Core
{
    /// <summary>
    /// Mutable command cache repository.
    /// </summary>
    public interface IMutableCommandCacheRepository
    {
        /// <summary>
        /// Set the command cache by its kind asyncrhonously.
        /// And the cache will be expired after specified timespan.
        /// </summary>
        /// <param name="Kind"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="ExpiresAfter"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> SetAsync(Type Kind, string Key, string Value, TimeSpan? ExpiresAfter = null, CancellationToken Token = default);

        /// <summary>
        /// Unset the command cache immediately.
        /// </summary>
        /// <param name="Kind"></param>
        /// <param name="Key"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> UnsetAsync(Type Kind, string Key, CancellationToken Token = default);
    }
}