namespace NIdentity.Core
{
    /// <summary>
    /// Abstracted command cache repository.
    /// </summary>
    public interface ICommandCacheRepository
    {
        /// <summary>
        /// Get the command cache by its kind asyncrhonously.
        /// </summary>
        /// <param name="Kind"></param>
        /// <param name="Key"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<string> GetAsync(Type Kind, string Key, CancellationToken Token = default);
    }
}