namespace NIdentity.Connector.AspNetCore.Middlewares
{
    /// <summary>
    /// Request recognition middleware.
    /// </summary>
    public class RequesterRecognition
    {
        private readonly RequestDelegate m_Next;

        /// <summary>
        /// Initialize a new <see cref="RequesterRecognition"/> instance.
        /// </summary>
        /// <param name="Next"></param>
        /// <param name="System"></param>
        public RequesterRecognition(RequestDelegate Next)
        {
            m_Next = Next;
        }

        /// <summary>
        /// Invoke the <see cref="RequesterRecognition"/> middleware.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext HttpContext)
        {
            var System = HttpContext.RequestServices.GetRequiredService<RequesterIdentitySystem>();
            var Logger = HttpContext.RequestServices.GetService<ILogger<RequesterIdentitySystem>>();
            var Requester = AspNetCore.Requester.FromHttpContext(HttpContext);

            // --> recognize identities.
            foreach (var Recognizer in System.Recognizers)
            {
                try { await Recognizer.RecognizeAsync(Requester); }
                catch (Exception Error)
                {
                    var RecognizerType = Recognizer.GetType();
                    Logger?.LogError(Error,
                        $"failed to recognize identity " +
                        $"using {RecognizerType.FullName}.");
                }
            }

            await OnInvokeAsync(Requester , () => m_Next(HttpContext));
        }

        /// <summary>
        /// Override this method when the additional operations required.
        /// </summary>
        /// <param name="Next"></param>
        /// <returns></returns>
        protected virtual Task OnInvokeAsync(Requester Requester, Func<Task> Next)
        {
            // --> execute next.
            return Next.Invoke();
        }
    }
}
