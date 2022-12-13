namespace NIdentity.Core.X509.Server.Ocsp
{
    public enum OcspExecutionStatus
    {
        /// <summary>
        /// Successful.
        /// </summary>
        Successful = 0,

        /// <summary>
        /// Malformed Request.
        /// </summary>
        MalformedRequest,

        /// <summary>
        /// Internal Error.
        /// </summary>
        InternalError,

        /// <summary>
        /// Try Later.
        /// </summary>
        TryLater,

        /// <summary>
        /// Signature Required.
        /// </summary>
        SignatureRequired,

        /// <summary>
        /// Unauthorized.
        /// </summary>
        Unauthorized
    }
}
