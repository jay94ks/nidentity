using Microsoft.EntityFrameworkCore;
using NIdentity.Connector.AspNetCore.Extensions;
using NIdentity.Connector.AspNetCore.Identities.X509;
using NIdentity.Core.Server.Commands;
using NIdentity.Endpoints.Server.Commands;
using NIdentity.Endpoints.Server.Repositories;

namespace NIdentity.Endpoints.Server
{
    public static class EndpointServerExtensions
    {
        /// <summary>
        /// Add Endpoint identity services
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="Services"></param>
        /// <returns></returns>
        public static EndpointServerSettings AddEndpointIdentityServer<TDbContext>(this IServiceCollection Services) where TDbContext : DbContext
        {
            var Settings = new EndpointServerSettings();

            Services
                .AddSingleton(Settings)
                .AddSingleton(X => new EndpointContext(X.GetRequiredService<TDbContext>()))
                .AddScoped<EndpointRepository>()
                .AddScoped<EndpointNetworkRepository>()
                .AddScoped<EndpointInventoryRepository>()
                ;

            Services
                .AddScoped<IEndpointRepository>(X => X.GetRequiredService<EndpointRepository>())
                .AddScoped<IMutableEndpointRepository>(X => X.GetRequiredService<EndpointRepository>())
                .AddScoped<IEndpointNetworkRepository>(X => X.GetRequiredService<EndpointNetworkRepository>())
                .AddScoped<IMutableEndpointNetworkRepository>(X => X.GetRequiredService<EndpointNetworkRepository>())
                .AddScoped<IEndpointInventoryRepository>(X => X.GetRequiredService<EndpointInventoryRepository>())
                .AddScoped<IMutableEndpointInventoryRepository>(X => X.GetRequiredService<EndpointInventoryRepository>())
                ;

            return Settings;
        }

        /// <summary>
        /// Add Endpoint Terraformer.
        /// </summary>
        /// <param name="Builder"></param>
        /// <returns></returns>
        public static CommandTerraformBuilder WitnEndpoints(this CommandTerraformBuilder Builder)
        {
            Builder.With(async (Context, Next) =>
            {
                var Http = Context.HttpContext;
                var Accessor = Http.RequestServices.GetRequiredService<EndpointRequesterAccesor>();
                var Settings = Http.RequestServices.GetRequiredService<EndpointServerSettings>();
                var Identity = Http.GetRequester().Get<X509RequesterIdentity>();

                if (Identity != null && Identity.IsValidated)
                    Accessor.Requester = Identity.Recognized;

                Accessor.IsSuperAccess = Settings.IsSuperMode;// -- Debugger.IsAttached;
                await Next.Invoke();
            });

            return Builder;
        }

    }
}