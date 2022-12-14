namespace NIdentity.Connector.AspNetCore.Middlewares
{
    /// <summary>
    /// Requester validation middleware.
    /// </summary>
    public class RequesterValidation
    {
        private readonly RequestDelegate m_Next;

        /// <summary>
        /// Initialize a new <see cref="RequesterRecognition"/> instance.
        /// </summary>
        /// <param name="Next"></param>
        public RequesterValidation(RequestDelegate Next)
        {
            m_Next = Next;
        }

        /// <summary>
        /// Invoke the <see cref="RequesterValidation"/> middleware.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext HttpContext)
        {

            var Requester = AspNetCore.Requester.FromHttpContext(HttpContext);
            await ValidateAsync(HttpContext, Requester);
            await OnInvokeAsync(Requester, () => m_Next(HttpContext));
        }

        /// <summary>
        /// Validate the <see cref="Requester"/>.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="Requester"></param>
        /// <returns></returns>
        private static async Task ValidateAsync(HttpContext HttpContext, Requester Requester)
        {
            var System = HttpContext.RequestServices.GetRequiredService<RequesterIdentitySystem>();
            var Logger = HttpContext.RequestServices.GetService<ILogger<RequesterIdentitySystem>>();
            var ValidatedOnce = new HashSet<RequesterIdentity>();

            while (true)
            {
                var Identities = Requester
                    .Where(X => ValidatedOnce.Contains(X) == false)
                    .Where(X => X.IsValidated == false)
                    .ToArray(); // --> snapshot to alter collections.

                // --> if no more identities are available,
                //     break the validator loop here.

                if (Identities.Length <= 0)
                    break;

                foreach (var Identity in Identities)
                {
                    if (Requester.Contains(Identity) == false || Identity.IsValidated == true)
                    {
                        ValidatedOnce.Add(Identity);
                        continue;
                    }

                    // --> recognize identities.
                    ValidatedOnce.Add(Identity);
                    foreach (var Recognizer in System.Validators)
                    {
                        try
                        {
                            var Result = await Recognizer.ValidateAsync(Requester, Identity);
                            if (Result)
                            {
                                Identity.SetValidated(true);
                                break;
                            }
                        }
                        catch (Exception Error)
                        {
                            var RecognizerType = Recognizer.GetType();
                            Logger?.LogError(Error,
                                $"failed to validate identity " +
                                $"using {RecognizerType.FullName}.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Override this method when the additional operations required.
        /// </summary>
        /// <param name="Requester"></param>
        /// <param name="Next"></param>
        /// <returns></returns>
        protected virtual Task OnInvokeAsync(Requester Requester, Func<Task> Next)
        {
            // --> execute next.
            return Next.Invoke();
        }

    }
}
