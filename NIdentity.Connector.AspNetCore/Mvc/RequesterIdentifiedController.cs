using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NIdentity.Connector.AspNetCore.Filters;
using NIdentity.Core.Helpers;

namespace NIdentity.Connector.AspNetCore.Mvc
{
    /// <summary>
    /// Requester Identified Controller.
    /// Controllers that inherited from this will not be executed without identity.
    /// No <see cref="RequireIdentityAttribute"/> needed for inner actions, because it's  duplicate.
    /// </summary>
    public abstract class RequesterIdentifiedController : Controller
    {
        private static readonly object KEY = RequireIdentityAttribute.KEY;
        private Requester m_Requester;

        /// <summary>
        /// Requester instance.
        /// </summary>
        public Requester Requester 
            => GetterHelpers.Cached(ref m_Requester, () => Requester.FromHttpContext(HttpContext));

        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext Action, ActionExecutionDelegate Next)
        {
            // --> block the filter behaviours duplicated.
            if (Action.HttpContext.Items.ContainsKey(KEY))
                return;

            Action.HttpContext.Items[KEY] = true;

            // -------

            var Requester = AspNetCore.Requester.FromHttpContext(Action.HttpContext);
            if (Requester.Count <= 0 || await CheckIdentityAsync(Requester) == false)
            {
                Action.Result = ReplaceToUnauthorized(Action.HttpContext);
                await Action.Result.ExecuteResultAsync(Action);
                return;
            }

            await base.OnActionExecutionAsync(Action, Next);
        }

        /// <summary>
        /// Called to replace the action to 401 unauthorized.
        /// </summary>
        /// <param name="Action"></param>
        /// <returns></returns>
        [NonAction]
        protected IActionResult ReplaceToUnauthorized(HttpContext HttpContext) => new StatusCodeResult(401);

        /// <summary>
        /// Called to implement additional authorization process.
        /// </summary>
        /// <param name="Requester"></param>
        /// <returns></returns>
        [NonAction]
        protected virtual Task<bool> CheckIdentityAsync(Requester Requester) => Task.FromResult(true);

    }
}
