namespace NIdentity.Core.X509.Authority
{
    /// <summary>
    /// Issuer's Authority Access Information.
    /// </summary>
    public class AuthorityAccess
    {
        /// <summary>
        /// Initialize a new <see cref="AuthorityAccess"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="AccessUri"></param>
        public AuthorityAccess(
            AuthorityAccessType Type,
            Uri AccessUri)
        {
            this.Type = Type;
            this.AccessUri = AccessUri;
        }

        /// <summary>
        /// Access Information Type.
        /// </summary>
        public AuthorityAccessType Type { get; }

        /// <summary>
        /// Access Uri.
        /// </summary>
        public Uri AccessUri { get; }
    }
}
