namespace NIdentity.Connector.AspNetCore.Abstractions
{
    /// <summary>
    /// Cache Repository to store/restore metadatas for identities.
    /// </summary>
    public interface IRequesterIdentityCacheRepository
    {
        /// <summary>
        /// Load an identity cached data.
        /// If no cache exists, this should return null not throws exception.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Key"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<string> LoadAsync(RequesterIdentity Identity, string Key, CancellationToken Token = default);

        /// <summary>
        /// Save an identity cache data.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        Task<bool> SaveAsync(RequesterIdentity Identity, string Key, string Value, CancellationToken Token = default);
    }
}
