using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Endpoints
{
    /// <summary>
    /// Endpoint's network information.
    /// </summary>
    public class EndpointNetwork
    {
        /// <summary>
        /// Type of network.
        /// </summary>
        public EndpointNetworkType Type { get; set; } = EndpointNetworkType.Unknown;

        /// <summary>
        /// Network address.
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// Subnet mask.
        /// </summary>
        public int SubnetMask { get; set; }

        /// <summary>
        /// Name of this endpoint.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of this endpoint.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Time when this network is marked as specified caution level.
        /// </summary>
        public DateTimeOffset? CautionTime { get; set; }

        /// <summary>
        /// Caution level.
        /// </summary>
        public EndpointCautionLevel CautionLevel { get; set; } = EndpointCautionLevel.Unspecified;

        /// <summary>
        /// Indicates whether this network is makred as caution or not.
        /// </summary>
        public bool IsCautionMarked => CautionLevel != EndpointCautionLevel.Unspecified;

        /// <summary>
        /// Indicates whether the network is authority or not.
        /// </summary>
        public bool IsAuthority => Type == EndpointNetworkType.Authority;

        /// <summary>
        /// Indicates whether the network is authority or not.
        /// </summary>
        public bool IsThirdParty => Type == EndpointNetworkType.ThirdParty;

        /// <summary>
        /// Indicates whether the network is for internal servers or not.
        /// </summary>
        public bool IsInternalServers => Type == EndpointNetworkType.InternalServers;
    }
}
