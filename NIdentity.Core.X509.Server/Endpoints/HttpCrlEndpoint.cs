using NIdentity.Core.X509.Revokations;
using NIdentity.Core.X509.Server.Repositories;
using NIdentity.Core.X509.Server.Revokations;

namespace NIdentity.Core.X509.Server.Endpoints
{
    public class HttpCrlEndpoint
    {
        private static long GetRevision(X509ServerSettings Settings, string Crl)
        {
            var PathName = Path.Combine(Settings.CachePath, $"{Crl}.rev");
            try
            {
                if (File.Exists(PathName))
                {
                    var Rev = File.ReadAllText(PathName);
                    if (long.TryParse(Rev, out var Revision))
                        return Revision;
                }
            }

            catch { }
            return 0;
        }

        private static void SetRevision(X509ServerSettings Settings, string Crl, long Revision)
        {
            var PathName = Path.Combine(Settings.CachePath, $"{Crl}.rev");
            try { File.WriteAllText(PathName, Revision.ToString()); }
            catch
            {
            }
        }

        private static byte[] GetCrl(X509ServerSettings Settings, string Crl, long Revision)
        {
            if (GetRevision(Settings, Crl) != Revision)
                return null;

            var PathName = Path.Combine(Settings.CachePath, Crl);

            try
            {
                if (File.Exists(PathName))
                    return File.ReadAllBytes(PathName);
            }

            catch { }
            return null;
        }

        private static void SetCrl(X509ServerSettings Settings, string Crl, long Revision, byte[] Data)
        {
            var PathName = Path.Combine(Settings.CachePath, Crl);

            try
            {
                File.WriteAllBytes(PathName, Data);
                SetRevision(Settings, Crl, Revision);
            }

            catch { }
        }


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

            var HttpCRL = (Settings.HttpCRL ?? string.Empty).Trim('/');
            if (string.IsNullOrWhiteSpace(HttpCRL))
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Path = (Http.Request.Path.Value ?? string.Empty)
                .Split('?', 2).First().Trim('/');

            if (Path.StartsWith($"{HttpCRL}/") == false)
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Crl = Path.Substring(HttpCRL.Length + 1);
            if (Crl.EndsWith(".crl") == false)
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
            var RevokationRepository = Http.RequestServices.GetRequiredService<IRevocationRepository>();
            var X509 = Http.RequestServices.GetRequiredService<X509Context>();
            var KeySHA256 = Crl.Substring(0, Crl.Length - 4);

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
            if (Certificate is null ||
                Certificate.HasPrivateKey == false ||
                Certificate.Type == CertificateType.Leaf)
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Inventory = await RevokationRepository.GetInventoryAsync(Certificate, Http.RequestAborted);
            if (Inventory != null && Inventory.Revision.HasValue)
            {
                var Cache = GetCrl(Settings, Crl, Inventory.Revision.Value);
                if (Cache != null)
                {
                    await ServeBytes(Http, Cache);
                    return;
                }
            }

            var Offset = 0;
            var Builder = new CRLBuilder();
            while (Inventory != null)
            {
                if (Http.RequestAborted.IsCancellationRequested)
                {
                    Http.Response.StatusCode = 408;
                    return;
                }

                var Revokes = await RevokationRepository.ListRevokationsAsync(Inventory, Offset, 100, Http.RequestAborted);
                if (Revokes is null || Revokes.Length <= 0)
                    break;

                Offset += Revokes.Length;
                foreach (var Each in Revokes)
                    Builder.Revokations.Add(Each);
            }

            Builder.Issuer = Certificate;
            Builder.Inventory = Inventory;
            Builder.NextUpdate = DateTimeOffset.UtcNow.Add(Settings.CrlTerm);

            var Bytes = Builder.Build();
            if (Inventory != null && Inventory.Revision.HasValue)
                SetCrl(Settings, Crl, Inventory.Revision.Value, Bytes);

            else
                SetCrl(Settings, Crl, -1, Bytes);

            try { await ServeBytes(Http, Bytes); }
            catch
            {
            }
        }

        private static async Task ServeBytes(HttpContext Http, byte[] Bytes)
        {
            try
            {
                Http.Response.StatusCode = 200;
                Http.Response.ContentType = "application/pkix-crl";
                Http.Response.ContentLength = Bytes.Length;
                await Http.Response.Body.WriteAsync(Bytes);
            }
            catch
            {
                try { Http.Response.StatusCode = 500; }
                catch
                {
                }
            }
        }
    }
}
