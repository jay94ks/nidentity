namespace NIdentity.Core.X509.Server.Helpers
{
    internal static class HttpContextHelpers
    {

        /// <summary>
        /// Make Uri from <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Uri MakeUri(this HttpRequest Request)
        {
            if (Request is null)
                throw new ArgumentNullException(nameof(Request));

            if (string.IsNullOrWhiteSpace(Request.Scheme))
                throw new ArgumentException("Unsupported scheme.");

            var Raw = string.Join("",
                Request.Scheme, "://",
                Request.Host.HasValue
                    ? Request.Host.Value : "UNKNOWN-HOST", "/",

                Request.Path.HasValue
                    ? Request.Path.Value.TrimStart('/') : "",

                Request.QueryString.HasValue
                    ? "?" : "",

                Request.QueryString.HasValue
                    ? Request.QueryString.Value.TrimStart('?') : "");

            return new Uri(Raw);
        }

        /// <summary>
        /// Get body bytes.
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetBodyAsync(this HttpRequest Request)
        {
            try
            {
                using var Mem = new MemoryStream(2048);
                await Request.Body.CopyToAsync(Mem);
                return Mem.ToArray();
            }
            catch { }
            return null;
        }
    }
}
