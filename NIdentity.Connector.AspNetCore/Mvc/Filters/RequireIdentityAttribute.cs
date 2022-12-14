using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NIdentity.Connector.AspNetCore.Filters
{
    /// <summary>
    /// Marks an action that requires any <see cref="RequesterIdentity"/>.
    /// </summary>
    public class RequireIdentityAttribute : ActionFilterAttribute
    {
        internal static readonly object KEY = new();

        /// <summary>
        /// Initialize a new <see cref="RequireIdentityAttribute"/> instance.
        /// </summary>
        public RequireIdentityAttribute() { }

        /// <summary>
        /// Redirect Uri.
        /// If this not set, the response will be replaced to 401 unauthorized.
        /// </summary>
        public string RedirectUri { get; set; }

        /// <inheritdoc/>
        public sealed override async Task OnActionExecutionAsync(ActionExecutingContext Action, ActionExecutionDelegate Next)
        {
            // --> block the filter behaviours duplicated.
            if (Action.HttpContext.Items.ContainsKey(KEY))
                return;

            Action.HttpContext.Items[KEY] = true;

            // -------

            var Requester = AspNetCore.Requester.FromHttpContext(Action.HttpContext);
            if (Requester.Count <= 0 || await CheckIdentityAsync(Requester) == false)
            {
                await ReplaceToUnauthorized(Action);
                return;
            }

            await base.OnActionExecutionAsync(Action, Next);
        }

        /// <inheritdoc/>
        public sealed override async Task OnResultExecutionAsync(ResultExecutingContext Result, ResultExecutionDelegate Next)
        {
            // --> block the filter behaviours duplicated.
            if (Result.HttpContext.Items.ContainsKey(KEY))
                return;

            Result.HttpContext.Items[KEY] = true;

            // -------
            
            var Requester = AspNetCore.Requester.FromHttpContext(Result.HttpContext);
            if (Requester.Count <= 0 || await CheckIdentityAsync(Requester) == false)
            {
                await ReplaceToUnauthorized(Result);
                return;
            }

            await base.OnResultExecutionAsync(Result, Next);
        }

        /// <summary>
        /// Called to implement additional authorization process.
        /// </summary>
        /// <param name="Requester"></param>
        /// <returns></returns>
        protected virtual Task<bool> CheckIdentityAsync(Requester Requester) => Task.FromResult(true);

        /// <summary>
        /// Replace the behaviour to 401 unauthorized.
        /// </summary>
        /// <param name="Action"></param>
        /// <returns></returns>
        private async Task ReplaceToUnauthorized(ActionContext Action)
        {
            IActionResult Result = null;

            if (string.IsNullOrWhiteSpace(RedirectUri))
            {
                if (Action is ActionExecutingContext ActionContext)
                    ActionContext.Result = Result = new StatusCodeResult(401);

                else if (Action is ResultExecutingContext ResultContext)
                    ResultContext.Result = Result = new StatusCodeResult(401);
            }
            else
            {
                if (Action is ActionExecutingContext ActionContext)
                    ActionContext.Result = Result = new RedirectResult(RedirectUri);

                else if (Action is ResultExecutingContext ResultContext)
                    ResultContext.Result = Result = new RedirectResult(RedirectUri);
            }


            if (Result != null)
                await Result.ExecuteResultAsync(Action);
        }
    }
}
