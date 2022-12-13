namespace NIdentity.Core.X509.Server.Mvc.Helpers
{
    internal static class CertificateHelpers
    {
        private static readonly object KEY_CERTIFICATE = new();

        /// <summary>
        /// Get the <see cref="Certificate"/> from <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <returns></returns>
        public static async Task<Certificate> GetCertificateAsync(this HttpContext HttpContext)
        {
            if (HttpContext.Items.TryGetValue(KEY_CERTIFICATE, out var Temp))
                return Temp as Certificate;

            HttpContext.Items[KEY_CERTIFICATE] = null;

            var Connection = HttpContext.Connection;
            if (Connection.ClientCertificate is null)
                return null;

            try
            {
                var Repository = HttpContext.RequestServices.GetRequiredService<ICertificateRepository>();
                var Cert = await Repository.LoadAsync(Connection.ClientCertificate, HttpContext.RequestAborted);
                if (Cert is null)
                    return null;

                HttpContext.Items[KEY_CERTIFICATE] = Cert;
                return Cert;
            }

            catch (Exception Error)
            {
                HttpContext.RequestServices.GetService<ILogger<ICertificateRepository>>()
                    ?.LogError(Error, "failed to authorize the client connection.");
            }

            return null;
        }
    }
}
