namespace NIdentity.Endpoints
{
    public enum EndpointCautionLevel
    {
        /// <summary>
        /// Caution level is not set or marked as normal.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Marked as attention.
        /// </summary>
        Attention,

        /// <summary>
        /// Marked as criticial.
        /// In this case, the server will executes notification callbacks.
        /// </summary>
        Critical,

        /// <summary>
        /// Marked as fatal.
        /// In this cass, the server will deny all requests from this endpoint.
        /// </summary>
        Fatal
    }
}