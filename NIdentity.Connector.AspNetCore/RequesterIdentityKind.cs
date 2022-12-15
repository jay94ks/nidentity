namespace NIdentity.Connector.AspNetCore
{
    /// <summary>
    /// Kinds of requester identities.
    /// Lower identity kinds cannot replace upper identity kinds.
    /// Easily, endpoint itself can not be alternative of signature.
    /// And signature can not be alternative of certificate.
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
        /// Endpoint based identity that is recognized by the server.
        /// </summary>
        Endpoint,

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
