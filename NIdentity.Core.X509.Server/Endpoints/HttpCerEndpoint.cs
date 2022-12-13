using NIdentity.Core.X509.Server.Repositories;

namespace NIdentity.Core.X509.Server.Endpoints
{
    public class HttpCerEndpoint
    {
        /// <summary>
        /// Serve "CER" file.
        /// </summary>
        /// <param name="Http"></param>
        /// <param name="Next"></param>
        /// <returns></returns>
        public static async Task InvokeAsync(HttpContext Http)
        {
            var Settings = Http.RequestServices.GetService<X509ServerSettings>();
            if (Settings is null || Http.Request.Path.HasValue == false)
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var HttpCAIssuers = (Settings.HttpCAIssuers ?? string.Empty).Trim('/');
            if (string.IsNullOrWhiteSpace(HttpCAIssuers))
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Path = (Http.Request.Path.Value ?? string.Empty)
                .Split('?', 2).First().Trim('/');

            if (Path.StartsWith($"{HttpCAIssuers}/") == false)
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Cer = Path.Substring(HttpCAIssuers.Length + 1);
            if (Cer.EndsWith(".cer") == false)
            {
                Http.Response.StatusCode = 404;
                return;
            }

            if ((Http.Request.Method ?? string.Empty).ToLower() != "get")
            {
                Http.Response.StatusCode = 405;
                return;
            }

            var CertificateRepository = Http.RequestServices.GetRequiredService<ICertificateRepository>();
            var X509 = Http.RequestServices.GetRequiredService<X509Context>();
            var KeySHA256 = Cer.Substring(0, Cer.Length - 4);

            if (string.IsNullOrWhiteSpace(Settings.CachePath) == false)
            {
                var WrittenBytes = false;
                try
                {
                    var PathName = System.IO.Path.Combine(Settings.CachePath, Cer);
                    if (File.Exists(PathName))
                    {
                        var CreationAt = File.GetCreationTimeUtc(PathName);
                        if (DateTime.UtcNow - CreationAt <= Settings.CrlTerm)
                        {
                            WrittenBytes = true;
                            await ServeBytes(Http, File.ReadAllBytes(PathName));
                            return;
                        }
                    }
                }
                catch
                {
                    if (WrittenBytes)
                    {
                        Http.Response.StatusCode = 500;
                        return;
                    }
                }
            }


            var Temp = X509
                .Certificates.Where(X => X.KeySHA1 == KeySHA256)
                .Select(X => new { X.KeyIdentifier, X.Subject })
                .FirstOrDefault();

            if (Temp is null)
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Identity = new CertificateIdentity(Temp.Subject, Temp.KeyIdentifier);
            var Certificate = await CertificateRepository.LoadAsync(Identity, Http.RequestAborted);
            if (Certificate is null)
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Bytes = Certificate.Export();
            if (string.IsNullOrWhiteSpace(Settings.CachePath) == false)
            {
                try { File.WriteAllBytes(System.IO.Path.Combine(Settings.CachePath, Cer), Bytes); }
                catch { }
            }

            try { await ServeBytes(Http, Bytes); }
            catch
            {
            }
        }

        private static async Task ServeBytes(HttpContext Http, byte[] Bytes)
        {
            Http.Response.StatusCode = 200;
            Http.Response.ContentType = "application/x-x509-ca-cert";
            Http.Response.ContentLength = Bytes.Length;
            await Http.Response.Body.WriteAsync(Bytes);
        }
    }
}
