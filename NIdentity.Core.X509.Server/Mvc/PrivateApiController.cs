using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NIdentity.Core.X509.Server.Mvc.Helpers;

namespace NIdentity.Core.X509.Server.Mvc
{
    /// <summary>
    /// Controller base that requires the certificate.
    /// </summary>
    public abstract class CertificatedApiController : Controller
    {
        /// <summary>
        /// Requester.
        /// </summary>
        public Certificate Requester { get; set; }

        /// <summary>
        /// Certificate Repository.
        /// </summary>
        public ICertificateRepository CertificateRepository
            => HttpContext.RequestServices.GetRequiredService<ICertificateRepository>();

        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext Action, ActionExecutionDelegate Next)
        {
            if ((Requester = await Action.HttpContext.GetCertificateAsync()) is null ||
               !await Authorize(Action.HttpContext, Requester))
            {
                try
                {
                    Action.Result = new StatusCodeResult(401);
                    await Action.Result.ExecuteResultAsync(Action);
                }

                catch { }
                return;
            }

            await base.OnActionExecutionAsync(Action, Next);
        }

        /// <summary>
        /// Test whether the <see cref="Requester"/> has enough privileges or not.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        protected virtual Task<bool> Authorize(HttpContext HttpContext, Certificate Certificate) => Task.FromResult(true);
    }
}
