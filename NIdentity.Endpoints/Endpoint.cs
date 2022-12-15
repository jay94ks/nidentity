using System.Net;

namespace NIdentity.Endpoints
{
    /// <summary>
    /// Endpoint information
    /// </summary>
    public class Endpoint
    {
        /// <summary>
        /// Type of endpoint.
        /// </summary>
        public EndpointType Type { get; set; } = EndpointType.Unknown;

        /// <summary>
        /// IP Address.
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// Name of this endpoint.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of this endpoint.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Time when this endpoint is marked as specified caution level.
        /// </summary>
        public DateTimeOffset? CautionTime { get; set; }

        /// <summary>
        /// Caution level.
        /// </summary>
        public EndpointCautionLevel CautionLevel { get; set; }

        /// <summary>
        /// Indicates whether this endpoint is makred as caution or not.
        /// </summary>
        public bool IsCautionMarked => CautionLevel != EndpointCautionLevel.Unspecified;

        /// <summary>
        /// Indicates whether the address is authority or not.
        /// </summary>
        public bool IsAuthority => Type == EndpointType.Authority;

        /// <summary>
        /// Indicates whether the address is reverse proxy or not.
        /// </summary>
        public bool IsReverseProxy => Type == EndpointType.ReverseProxy;

        /// <summary>
        /// Indicates whether the address is server or not.
        /// </summary>
        public bool IsServer => Type == EndpointType.Server;
    }
}