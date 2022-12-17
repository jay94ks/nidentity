using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NIdentity.Connector.AspNetCore.Extensions;
using NIdentity.Connector.AspNetCore.Identities.X509;
using NIdentity.Core.Commands;
using NIdentity.Core.Server.Commands;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Server.Commands;
using NIdentity.Core.X509.Server.Documents;
using NIdentity.Core.X509.Server.Endpoints;
using NIdentity.Core.X509.Server.Repositories;
using NIdentity.Core.X509.Server.Revokations;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NIdentity.Core.X509.Server
{
    public static class X509ServerExtensions
    {
        /// <summary>
        /// Add X509 identity services.
        /// </summary>
        /// <param name="Services"></param>
        /// <returns></returns>
        public static X509ServerSettings AddX509IdentityServer<TDbContext>(this IServiceCollection Services) where TDbContext : DbContext
        {
            var Settings = new X509ServerSettings();

            Services
                .AddSingleton(Settings)
                .AddSingleton<X509RequesterAccesor>()
                .AddScoped(X => new X509Context(X.GetRequiredService<TDbContext>()))
                .AddScoped<X509Repository>()
                .AddScoped<X509DocumentRepository>()
                .AddScoped<X509RevokationRepository>()
                .AddScoped<X509PermissionManager>()
                ;

            Services
                .AddSingleton<ICertificateCacheRepository, X509CacheRepository>()
                .AddTransient(X => X.GetRequiredService<X509ServerSettings>().ExecutorSettings)
                ;

            Services
                .AddScoped<ICertificateRepository>(X => X.GetRequiredService<X509Repository>())
                .AddScoped<IDocumentRepository>(X => X.GetRequiredService<X509DocumentRepository>())
                .AddScoped<IRevocationRepository>(X => X.GetRequiredService<X509RevokationRepository>())
                .AddScoped<ICertificatePermissionManager>(X => X.GetRequiredService<X509PermissionManager>())

                .AddScoped<IMutableCertificateRepository>(X => X.GetRequiredService<X509Repository>())
                .AddScoped<IMutableDocumentRepository>(X => X.GetRequiredService<X509DocumentRepository>())
                .AddScoped<IMutableRevocationRepository>(X => X.GetRequiredService<X509RevokationRepository>())
                .AddScoped<IMutableCertificatePermissionManager>(X => X.GetRequiredService<X509PermissionManager>())
                ;

            return Settings;
        }

        /// <summary>
        /// Add middleware that enables X509 command endpoints, aka, websocket JSON command protocol.
        /// </summary>
        /// <param name="App"></param>
        /// <returns></returns>
        public static IApplicationBuilder MigrateX509(this IApplicationBuilder App)
        {
            var Settings = App.ApplicationServices.GetService<X509ServerSettings>()
                ?? throw new InvalidOperationException("to use X509 endpoints, call the `AddX509ServerService` for extending service collection.");

            using (var Scope = App.ApplicationServices.CreateScope())
            {
                var X509 = Scope.ServiceProvider.GetRequiredService<X509Context>();
                ExecutePrerequisiteCommands(Scope, X509, Settings.ExecutorSettings.Prerequisites);
            }

            return App;
        }

        /// <summary>
        /// Map X509 Endpoints to router.
        /// </summary>
        /// <param name="Router"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IEndpointRouteBuilder MapX509Endpoint(this IEndpointRouteBuilder Router)
        {
            var Settings = Router.ServiceProvider.GetService<X509ServerSettings>()
                ?? throw new InvalidOperationException("to use X509 endpoints, call the `AddX509ServerService` for extending service collection.");

            var HttpOcsp = (Settings.HttpOcsp ?? string.Empty).Trim('/');
            var HttpCRL = (Settings.HttpCRL ?? string.Empty).Trim('/');
            var HttpCAIssuers = (Settings.HttpCAIssuers ?? string.Empty).Trim('/');

            if (!string.IsNullOrWhiteSpace(HttpCAIssuers))
                Router.MapGet(HttpCAIssuers + "/{crl}.cer", HttpCerEndpoint.InvokeAsync);

            if (!string.IsNullOrWhiteSpace(HttpCRL))
                Router.MapGet(HttpCRL + "/{crl}.crl", HttpCrlEndpoint.InvokeAsync);

            if (!string.IsNullOrWhiteSpace(HttpOcsp))
                Router.Map(HttpOcsp, HttpOcspEndpoint.InvokeAsync);

            return Router;
        }

        /// <summary>
        /// Map X509 Server Commands for the command executor builder.
        /// </summary>
        /// <param name="Builder"></param>
        /// <returns></returns>
        public static CommandExecutor.Builder MapX509ServerCommands(this CommandExecutor.Builder Builder)
        {
            Builder.MapAssemblies("x509", typeof(X509Executor).Assembly, typeof(Certificate).Assembly);
            return Builder;
        }

        /// <summary>
        /// Add X509 Terraformer.
        /// </summary>
        /// <param name="Builder"></param>
        /// <returns></returns>
        public static CommandTerraformBuilder WithX509(this CommandTerraformBuilder Builder)
        {
            Builder.With(async (Context, Next) =>
            {
                var Http = Context.HttpContext;
                var Accessor = Http.RequestServices.GetRequiredService<X509RequesterAccesor>();
                var Settings = Http.RequestServices.GetRequiredService<X509ServerSettings>();
                var Identity = Http.GetRequester().Get<X509RequesterIdentity>();

                if (Identity != null && Identity.IsValidated)
                    Accessor.Requester = Identity.Recognized;

                Accessor.IsSuperAccess = Settings.IsSuperMode;// -- Debugger.IsAttached;
                await Next.Invoke();
            });

            return Builder;
        }

        /// <summary>
        /// Execute Prerequisite Commands.
        /// </summary>
        /// <param name="Scope"></param>
        /// <param name="X509"></param>
        /// <param name="Prerequisites"></param>
        private static void ExecutePrerequisiteCommands(IServiceScope Scope, X509Context X509, X509ExecutorPrerequisites Prerequisites)
        {
            var Executor = new X509Executor(Scope.ServiceProvider);
            var Logger = Scope.ServiceProvider.GetService<ILogger<X509ServerSettings>>();

            if (Prerequisites.RequiredKeys.Count > 0)
            {
                Executor.Authorization.IsSuperAccess = true;
                try
                {
                    foreach (var Each in Prerequisites.RequiredKeys)
                    {
                        var Subject = Each.Subject;
                        var IssuerKeyId = Each.IssuerKeyIdentifier;

                        if (string.IsNullOrWhiteSpace(IssuerKeyId))
                        {
                            var Temp = X509.Certificates
                                .Where(X => X.KeyIdentifier == X.IssuerKeyIdentifier)
                                .Where(X => X.Subject == Subject && X.Issuer == Subject)
                                .FirstOrDefault();

                            if (Temp != null)
                                continue;
                        }
                        else
                        {
                            var Temp = X509.Certificates
                                .Where(X => X.IssuerKeyIdentifier == X.IssuerKeyIdentifier)
                                .Where(X => X.Subject == Subject)
                                .FirstOrDefault();

                            if (Temp != null)
                                continue;
                        }

                        var Result = Executor.Execute(Each)
                            .ConfigureAwait(false)
                            .GetAwaiter().GetResult();

                        if (Result.Success)
                        {
                            if (Result is not X509GenerateCommand.Result GenResult)
                                continue;

                            var ThisPath = Path.GetDirectoryName(typeof(X509ServerExtensions).Assembly.Location);
                            var FilePath = Path.Combine(ThisPath, "data", GenResult.Thumbprint + ".pfx");

                            var BasePath = Path.GetDirectoryName(FilePath);
                            if (!Directory.Exists(BasePath))
                                 Directory.CreateDirectory(BasePath);

                            File.WriteAllBytes(FilePath, GenResult.Certificate.ExportPfx());
                            continue;
                        }

                        var Json = JsonConvert.SerializeObject(Result);
                        Logger?.LogError($"{Each.Subject} couldnt be created: {Json}.");
                    }
                }

                finally
                {
                    Executor.Authorization.IsSuperAccess = false;
                }
            }
        }
    }
}
