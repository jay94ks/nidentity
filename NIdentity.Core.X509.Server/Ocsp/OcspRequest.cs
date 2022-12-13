using NIdentity.Core.X509.Server.Helpers;
using Org.BouncyCastle.Ocsp;
using System.Web;

namespace NIdentity.Core.X509.Server.Ocsp
{
    /// <summary>
    /// Ocsp Request.
    /// </summary>
    public class OcspRequest
    {
        /// <summary>
        /// Hide the constructor.
        /// </summary>
        private OcspRequest() { }

        /// <summary>
        /// Request Method.
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// Request Body.
        /// </summary>
        public byte[] Body { get; private set; }

        /// <summary>
        /// Bouncy Castle, Ocsp Req.
        /// </summary>
        internal OcspReq Ocsp { get; private set; }

        /// <summary>
        /// Get all target identities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OcspCertificateIdentity> GetTargetIdentities()
            => Ocsp.GetRequestList().Select(X => X.ToOcspIdentity());

        /// <summary>
        /// Create <see cref="OcspRequest"/> from <see cref="HttpContext"/>.
        /// </summary>
        /// <returns></returns>
        internal static async Task<OcspRequest> FromHttpAsync(HttpContext Http, bool Strict = false)
        {
            var Method = Http.Request.Method.ToUpper();
            if (Method == "GET")
                return FromHttpGetAsync(Http);

            if (Method == "POST")
                return await FromHttpPostAsync(Http, Strict);

            return null;
        }

        /// <summary>
        /// Create a new <see cref="Models.OcspRequest"/> from Http GET request.
        /// </summary>
        /// <param name="Http"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        private static OcspRequest FromHttpGetAsync(HttpContext Http)
        {
            var Uri = Http.Request.MakeUri();
            var Segment = Uri.Segments.LastOrDefault();

            if (string.IsNullOrWhiteSpace(Segment = HttpUtility.UrlDecode(Segment)))
                throw new ArgumentException("No segment set or corrupted.");

            var Body = Convert.FromBase64String(Segment);
            if (Body is null || Body.Length <= 0)
                throw new FormatException("Request content is empty.");

            return new OcspRequest
            {
                Method = "GET",
                Body = Body,
                Ocsp = new OcspReq(Body)
            };
        }

        /// <summary>
        /// Create a new <see cref="Models.OcspRequest"/> from Http POST request.
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Strict"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        private static async Task<OcspRequest> FromHttpPostAsync(HttpContext Http, bool Strict)
        {
            if (Strict)
            {
                var MediaType = (Http.Request.ContentType ?? string.Empty).ToLower();
                if (MediaType != "application/ocsp-request")
                    throw new ArgumentException("Not supported media type.");
            }

            var Body = await Http.Request.GetBodyAsync();
            if (Body is null || Body.Length <= 0)
                throw new FormatException("Request content is empty.");

            return new OcspRequest
            {
                Method = "POST",
                Body = Body,
                Ocsp = new OcspReq(Body)
            };
        }
    }
}
