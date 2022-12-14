namespace NIdentity.Connector.AspNetCore.Abstractions
{
    /// <summary>
    /// Validates identity informations in <see cref="Requester"/>.
    /// </summary>
    public interface IRequesterIdentityValidator
    {
        /// <summary>
        /// Validate identities of <paramref name="Requester"/>.
        /// And the validator can remove identities from requester.
        /// </summary>
        /// <param name="Requester"></param>
        /// <param name="Input"></param>
        /// <returns></returns>
        Task<bool> ValidateAsync(Requester Requester, RequesterIdentity Input);
    }
}
