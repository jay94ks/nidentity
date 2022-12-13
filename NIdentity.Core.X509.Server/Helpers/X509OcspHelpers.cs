using NIdentity.Core.X509.Server.Ocsp;
using Org.BouncyCastle.Ocsp;

namespace NIdentity.Core.X509.Server.Helpers
{
    internal static class X509OcspHelpers
    {
        private static readonly Dictionary<OcspExecutionStatus, int> ERROR_CODES
            = new Dictionary<OcspExecutionStatus, int>()
            {
                { OcspExecutionStatus.Successful, OcspRespStatus.Successful },
                { OcspExecutionStatus.MalformedRequest, OcspRespStatus.MalformedRequest },
                { OcspExecutionStatus.InternalError, OcspRespStatus.InternalError },
                { OcspExecutionStatus.TryLater, OcspRespStatus.TryLater },
                { OcspExecutionStatus.SignatureRequired, OcspRespStatus.SigRequired },
                { OcspExecutionStatus.Unauthorized, OcspRespStatus.Unauthorized },
            };

        /// <summary>
        /// Get the <see cref="CertificateReference"/> from <see cref="Req"/>.
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static OcspCertificateIdentity ToOcspIdentity(this Req Request) => new OcspCertificateIdentity(Request);

        /// <summary>
        /// Get the request error code if <see cref="OcspExecutionStatus"/> is error or not.
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static int GetErrorNumber(this OcspExecutionStatus Status)
        {
            if (ERROR_CODES.TryGetValue(Status, out var Value))
                return Value;

            return -1;
        }

    }
}
