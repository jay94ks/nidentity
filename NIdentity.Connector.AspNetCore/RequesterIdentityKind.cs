namespace NIdentity.Connector.AspNetCore
{
    /// <summary>
    /// Kinds of requester identities.
    /// </summary>
    public enum RequesterIdentityKind
    {
        /// <summary>
        /// Basic identity like ID and Password.
        /// </summary>
        Basic = 0,

        /// <summary>
        /// Token based identity like bearer authorization.
        /// </summary>
        Tokenized,

        /// <summary>
        /// Digital signature based identity like bitcoin something else.
        /// </summary>
        Signature,

        /// <summary>
        /// Certificate digital signature based identity like X509.
        /// </summary>
        Certificate
    }
}
