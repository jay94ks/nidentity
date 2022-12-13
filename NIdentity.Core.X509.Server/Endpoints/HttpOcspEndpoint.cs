using Microsoft.AspNetCore.Mvc;
using NIdentity.Core.X509.Server.Ocsp;

namespace NIdentity.Core.X509.Server.Endpoints
{
    public class HttpOcspEndpoint
    {
        /// <summary>
        /// Serve "Ocsp" endpoint.
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

            var HttpOcsp = (Settings.HttpOcsp ?? string.Empty).Trim('/');
            if (string.IsNullOrWhiteSpace(HttpOcsp))
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Path = (Http.Request.Path.Value ?? string.Empty)
                .Split('?', 2).First().Trim('/');

            if (Path != HttpOcsp)
            {
                Http.Response.StatusCode = 404;
                return;
            }

            var Method = (Http.Request.Method ?? string.Empty).ToLower();
            if (Method != "get" && Method != "post")
            {
                Http.Response.StatusCode = 405;
                return;
            }

            var Result = await OcspContext.ExecuteAsync(Http);
            await Result.ExecuteResultAsync(new ActionContext(Http, new(), new()));
        }
    }
}
