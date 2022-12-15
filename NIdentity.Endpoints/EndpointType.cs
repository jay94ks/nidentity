namespace NIdentity.Endpoints
{
    /// <summary>
    /// Endpoint type.
    /// </summary>
    public enum EndpointType
    {
        /// <summary>
        /// Unknown endpoint.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Reverse Proxy Endpoint.
        /// </summary>
        ReverseProxy,

        /// <summary>
        /// Authority Endpoint, aka, management traffics.
        /// </summary>
        Authority,

        /// <summary>
        /// Server Endpoint.
        /// </summary>
        Server
    }
}