namespace NIdentity.Connector.AspNetCore.Abstractions
{
    /// <summary>
    /// Extracts identity information from <see cref="HttpContext"/>.
    /// </summary>
    public interface IRequesterIdentityRecognizer
    {
        /// <summary>
        /// Recognizes <see cref="RequesterIdentity"/> for the <see cref="Requester"/>
        /// And returns count of recognized identities.
        /// </summary>
        /// <param name="Requester"></param>
        /// <returns></returns>
        Task RecognizeAsync(Requester Requester);
    }
}
