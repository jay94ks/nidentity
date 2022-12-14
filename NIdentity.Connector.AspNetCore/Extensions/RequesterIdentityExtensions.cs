using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using NIdentity.Connector.AspNetCore.Builders;
using NIdentity.Connector.AspNetCore.Middlewares;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace NIdentity.Connector.AspNetCore.Extensions
{
    public static class RequesterIdentityExtensions
    {
        /// <summary>
        /// Setup SSL configurations for the requester recognition.
        /// This will override <see cref="HttpsConnectionAdapterOptions.ClientCertificateMode"/>,
        /// <see cref="HttpsConnectionAdapterOptions.CheckCertificateRevocation"/>,
        /// <see cref="HttpsConnectionAdapterOptions.ClientCertificateValidation"/>,
        /// <see cref="HttpsConnectionAdapterOptions.OnAuthenticate"/> properties.
        /// And if <paramref name="Mode"/> specified to <see cref="ClientCertificateMode.NoCertificate"/>,
        /// this will force it to be <see cref="ClientCertificateMode.AllowCertificate"/>,
        /// <see cref="SslServerAuthenticationOptions.CertificateRevocationCheckMode"/>
        /// to <see cref="X509RevocationMode.NoCheck"/>.<br /><br />
        /// <b>I strongly recommends that this method should be called at last of last.</b>
        /// </summary>
        /// <param name="Kestrel"></param>
        /// <returns></returns>
        public static KestrelServerOptions EnableSslRequesterRecognition(
            this KestrelServerOptions Kestrel, ClientCertificateMode Mode = ClientCertificateMode.AllowCertificate)
        {
            if (Mode == ClientCertificateMode.NoCertificate)
                Mode = ClientCertificateMode.AllowCertificate;

            Kestrel.ConfigureHttpsDefaults(Options =>
            {
                Options.CheckCertificateRevocation = false;
                Options.ClientCertificateMode = Mode;
                Options.ClientCertificateValidation = (_, _2, _3) => true;
                
                var Previous = Options.OnAuthenticate;
                Options.OnAuthenticate = (A, B) =>
                {
                    Previous?.Invoke(A, B);
                    B.CertificateRevocationCheckMode = X509RevocationMode.NoCheck;
                };
            });

            return Kestrel;
        }

        /// <summary>
        /// Add the requester identity service to service collection.
        /// </summary>
        /// <param name="Services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRequesterIdentitySystem(this IServiceCollection Services)
        {
            Services
                .AddSingleton<RequesterIdentitySystem>()
                ;

            return Services;
        }

        /// <summary>
        /// Add the requester recognition to the application.
        /// This add <see cref="RequesterRecognition"/> middleware.
        /// </summary>
        /// <param name="App"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequesterRecognition(
            this IApplicationBuilder Builder,
            Action<RequesterIdentitySystemBuilder> RecognitionBuilder = null)
        {
            var System = Builder.ApplicationServices.GetService<RequesterIdentitySystem>()
                ?? throw new InvalidOperationException(
                    "the use requester recognition, add reequester identity service to service collection.");

            RecognitionBuilder?.Invoke(new RequesterIdentitySystemBuilder(System));

            // --> configure the recognizer middleware.
            return Builder.UseMiddleware<RequesterRecognition>();
        }

        /// <summary>
        /// Add the requester validation to the application.
        /// This add <see cref="RequesterValidation"/> middleware.
        /// </summary>
        /// <param name="Builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequesterValidation(this ApplicationBuilder Builder)
        {
            var System = Builder.ApplicationServices.GetService<RequesterIdentitySystem>()
                ?? throw new InvalidOperationException(
                    "the use requester recognition, add reequester identity service to service collection.");

            // --> configure the validator middleware.
            return Builder.UseMiddleware<RequesterValidation>();
        }
    }
}
