using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NIdentity.Core.X509.Server.Mvc.Helpers;

namespace NIdentity.Core.X509.Server.Mvc
{
    /// <summary>
    /// Marks an action that requires the certificate.
    /// </summary>
    public class RequiresCertificateAttribute : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext Action, ActionExecutionDelegate Next)
        {
            var Cert = await Action.HttpContext.GetCertificateAsync();
            if (Cert is null || !await Authorize(Action.HttpContext, Cert))
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
        /// Test whether the <see cref="Certificate"/> has enough privileges or not.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        protected virtual Task<bool> Authorize(HttpContext HttpContext, Certificate Certificate)
            => Task.FromResult(true);
    }
}
