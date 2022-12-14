using NIdentity.Connector.AspNetCore.Abstractions;
using NIdentity.Core.X509;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.X509;
using X509ContentType = System.Security.Cryptography.X509Certificates.X509ContentType;

namespace NIdentity.Connector.AspNetCore.Identities.X509
{
    /// <summary>
    /// X509 requester recognizer.
    /// </summary>
    public sealed class X509RequesterIdentityRecognizer : IRequesterIdentityRecognizer
    {
        /// <summary>
        /// Get the options for X509 identity.
        /// </summary>
        /// <param name="Requester"></param>
        /// <returns></returns>
        private X509RequesterIdentityOptions GetOptions(Requester Requester)
            => Requester.HttpContext.RequestServices.GetService<X509RequesterIdentityOptions>()
            ?? throw new InvalidOperationException("to use X509 identity recognizer, add X509 identity services.");

        /// <summary>
        /// Recognize X509 requester's identity.
        /// </summary>
        /// <param name="Requester"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task RecognizeAsync(Requester Requester)
        {
            X509RequesterIdentity Identity = null;

            var Options = GetOptions(Requester);
            if (Options.Disable == true)
                return Task.CompletedTask;

            if (Requester.HttpRequest.IsHttps)
                Identity = RecognizeFromHttps(Requester.HttpContext);

            else if (Options.DisableForwarderHeaders == false)
                Identity = RecognizeFromForwarder(Requester.HttpContext, Options);

            if (Identity != null)
            {
                Requester.Replace(Identity);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Recognize X509 identity from <see cref="HttpContext"/> if HTTPS based.
        /// </summary>
        /// <returns></returns>
        private X509RequesterIdentity RecognizeFromHttps(HttpContext HttpContext)
        {
            var X509 = HttpContext.Connection.ClientCertificate;
            if (X509 is null)
                return null;

            try
            {
                var Recognition = Certificate.Import(X509.Export(X509ContentType.Cert));
                if (Recognition != null)
                    return new X509RequesterIdentity(Recognition);
            }

            catch { }
            return null;
        }

        /// <summary>
        /// Recognize X509 identity from <see cref="HttpContext"/> if HTTP with forwarder headers.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <param name="Options"></param>
        /// <returns></returns>
        private X509RequesterIdentity RecognizeFromForwarder(
            HttpContext HttpContext, X509RequesterIdentityOptions Options)
        {
            HttpContext.Request.Headers.TryGetValue(Options.ResultHeader, out var ResultText);
            if (string.IsNullOrWhiteSpace(ResultText) || ResultText != Options.ExpectedResultValue)
                return null; // --> no validation result passed.

            HttpContext.Request.Headers.TryGetValue(Options.PemBase64Header, out var PemRawText);
            var PemText = ((string)PemRawText ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(PemText))
                return null; // --> no PEM base64 passed.

            try
            {
                var Recognition = Certificate.ImportPem(PemText);
                if (Recognition != null)
                    return new X509RequesterIdentity(Recognition);
            }
            catch { }
            return null; // --> failed to restore certificate from PEM passed.
        }
    }
}
