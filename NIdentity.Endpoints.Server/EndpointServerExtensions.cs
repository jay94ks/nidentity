using Microsoft.EntityFrameworkCore;
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
                ;

            return Settings;
        }
    }
}