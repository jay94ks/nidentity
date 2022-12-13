using Microsoft.AspNetCore.Mvc;

namespace NIdentity.Core.X509.Server.Ocsp
{
    public class OcspContext
    {
        /// <summary>
        /// Hide constructor.
        /// </summary>
        private OcspContext(HttpContext HttpContext) => this.HttpContext = HttpContext;

        /// <summary>
        /// Execute an <see cref="OcspContext"/> handler. (Non-Strict mode)
        /// </summary>
        /// <returns></returns>
        public static Task<IActionResult> ExecuteAsync(HttpContext HttpContext, Func<OcspContext, Task> Executor = null)
            => ExecuteAsync(HttpContext, false, Executor);

        /// <summary>
        /// Execute an <see cref="OcspContext"/> handler.
        /// if <paramref name="Strict"/> is true, this will check the content-type and it should be "application/ocsp-request".
        /// </summary>
        /// <returns></returns>
        public static async Task<IActionResult> ExecuteAsync(HttpContext HttpContext, bool Strict, Func<OcspContext, Task> Executor = null)
        {
            var Repository = HttpContext.RequestServices.GetRequiredService<ICertificateRepository>();
            var Request = await OcspRequest.FromHttpAsync(HttpContext, Strict);
            var Context = new OcspContext(HttpContext)
            {
                Repository = Repository,
                Logger = HttpContext.RequestServices.GetService<ILogger<OcspContext>>(),
                Request = Request,
                Response = new OcspResponse(Request)
            };

            await Context.DefaultAsync();

            var StillSuccess = Context.Response.Status == OcspExecutionStatus.Successful;
            if (Executor != null && StillSuccess)
                await Executor.Invoke(Context);

            try
            {
                return await Context.Response
                    .GenerateAsync(Repository, true, HttpContext.RequestAborted);
            }

            catch { }
            return new StatusCodeResult(408);
        }

        /// <summary>
        /// Http Context.
        /// </summary>
        public HttpContext HttpContext { get; }

        /// <summary>
        /// Request Aborted.
        /// </summary>
        public CancellationToken RequestAborted => HttpContext.RequestAborted;

        /// <summary>
        /// Request Services.
        /// </summary>
        public IServiceProvider RequestServices => HttpContext.RequestServices;

        /// <summary>
        /// Http Context Items.
        /// </summary>
        public IDictionary<object, object> Items => HttpContext.Items;

        /// <summary>
        /// Repository.
        /// </summary>
        public ICertificateRepository Repository { get; private set; }

        /// <summary>
        /// Logger.
        /// </summary>
        internal ILogger<OcspContext> Logger { get; private set; }

        /// <summary>
        /// Request.
        /// </summary>
        public OcspRequest Request { get; private set; }

        /// <summary>
        /// Response.
        /// </summary>
        public OcspResponse Response { get; private set; }

        /// <summary>
        /// Execute defaults asynchronously.
        /// </summary>
        /// <param name="Repository"></param>
        /// <returns></returns>
        private async Task DefaultAsync()
        {
            var Identifiers = Request.GetTargetIdentities().ToArray();
            var Certificates = new List<Certificate>();

            foreach (var Each in Identifiers)
            {
                try
                {
                    var EachItem = await Repository.LoadAsync(Each, RequestAborted);
                    if (EachItem != null)
                    {
                        Response.Set(Each, EachItem);
                        Certificates.Add(EachItem);
                        continue;
                    }
                }

                catch (Exception Error)
                {
                    Logger?.LogError(Error, $"failed to load certificate: {Each}.");
                }

                Response.Unset(Each);
            }

            if (Certificates.Count <= 0)
            {
                if (Identifiers.Length > 0)
                {
                    Response.Status = OcspExecutionStatus.MalformedRequest;
                    return;
                }

                Response.Status = OcspExecutionStatus.Successful;
                return;
            }

            await SetResponderAsync(Certificates);
        }

        /// <summary>
        /// Set the responder asynchronously.
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Certificates"></param>
        /// <returns></returns>
        private async Task SetResponderAsync(IEnumerable<Certificate> Certificates)
        {
            var Issuers = Certificates
                .GroupBy(X => X.Issuer.ToString())
                .Count();

            if (Issuers == 1)
            {
                var IssuerId = Certificates.First().Issuer;
                var Issuer = Certificates.FirstOrDefault(X => IssuerId.IsExact(X));

                try
                {
                    if (Issuer is null)
                        Issuer = await Repository.LoadAsync(IssuerId, RequestAborted);
                }
                catch (Exception Error)
                {
                    Logger?.LogError(Error, $"failed to load issuer certificate: {IssuerId}.");
                    Response.Status = OcspExecutionStatus.InternalError;
                    return;
                }

                if (Issuer is null || Issuer.HasPrivateKey == false)
                {
                    Response.Status = OcspExecutionStatus.Unauthorized;
                    return;
                }

                Response.Responder = Issuer;
                return;
            }

            var Chains = await LoadChainsAsync(Certificates);

            Issuers = Chains
                .Select(X => X.First())
                .GroupBy(X => X.Issuer.ToString())
                .Count();

            // --> if multiple root issuers are detected,
            //   : it can not be signed and reliable.
            if (Issuers != 1)
            {
                Response.Status = OcspExecutionStatus.Unauthorized;
                return;
            }

            // --> select commonly specified certificate.
            Certificate Last = null;
            while (true)
            {
                var Selection = Chains
                    .Select(X => X.First())
                    .GroupBy(X => X.Issuer.ToString())
                    .Select(X => (Count: X.Count(), First: X.First()))
                    .OrderByDescending(X => X.Count)
                    .FirstOrDefault();

                if (Selection.First is null || Selection.First.HasPrivateKey == false)
                    break;

                var Done = false;
                foreach (var Each in Chains)
                {
                    if (!Each.TryPeek(out var EachFirst) ||
                        !EachFirst.Equals(Selection.First))
                    {
                        Done = true;
                        break;
                    }

                    Each.Dequeue();
                }

                if (Done)
                    break;

                Last = Selection.First;
            }

            // --> if no key choosen.
            if ((Response.Responder = Last) is null)
                Response.Status = OcspExecutionStatus.Unauthorized;
        }

        /// <summary>
        /// Load certificate chains asynchronously.
        /// </summary>
        /// <param name="Certificates"></param>
        /// <returns></returns>
        private async Task<List<Queue<Certificate>>> LoadChainsAsync(IEnumerable<Certificate> Certificates)
        {
            var Chains = new List<Queue<Certificate>>();
            foreach (var Each in Certificates)
            {
                try
                {
                    var Chain = await Repository.LoadChainAsync(Each, RequestAborted);
                    if (Chain is null || Chain.Length <= 0)
                    {
                        Logger?.LogError($"no certificate chains exists on repository: {new CertificateReference(Each)}.");
                        continue;
                    }

                    Chains.Add(new Queue<Certificate>(Chain.Reverse()));
                }

                catch (Exception Error)
                {
                    Logger?.LogError(Error, $"failed to load certificate chain of: {new CertificateReference(Each)}.");
                }
            }

            return Chains;
        }
    }
}
