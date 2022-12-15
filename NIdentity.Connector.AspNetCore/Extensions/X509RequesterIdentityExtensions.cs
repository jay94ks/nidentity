using NIdentity.Connector.AspNetCore.Builders;
using NIdentity.Connector.AspNetCore.Identities.X509;
using NIdentity.Connector.X509;
using NIdentity.Core;
using NIdentity.Core.X509;

namespace NIdentity.Connector.AspNetCore.Extensions
{
    /// <summary>
    /// Enables <see cref="X509RequesterIdentity"/>
    /// on <see cref="RequesterIdentitySystem"/>.
    /// </summary>
    public static class X509RequesterIdentityExtensions
    {
        /// <summary>
        /// Add <see cref="X509RequesterIdentityRecognizer"/>,
        /// <see cref="X509RequesterIdentityValidator"/>,
        /// <see cref="X509RequesterIdentityOptions"/>
        /// to the service collection.
        /// </summary>
        /// <param name="Services"></param>
        /// <param name="Factory"></param>
        /// <returns></returns>
        public static IServiceCollection AddX509RequesterIdentityService(
            this IServiceCollection Services, Func<IServiceProvider, X509CommandExecutor> Factory = null)
        {
            Services
                .AddSingleton<X509RequesterIdentityRecognizer>()
                .AddSingleton<X509RequesterIdentityValidator>()
                .AddSingleton<X509RequesterIdentityOptions>()
                ;

            if (Factory is null)
            {
                Services.AddScoped(Services =>
                {
                    var Executor = Services.GetRequiredService<ICommandExecutor>();
                    if (Executor is RemoteCommandExecutor Rce)
                        return Rce.X509;

                    var Caches = Services.GetService<ICertificateCacheRepository>();
                    return new X509CommandExecutor(Executor, Caches);
                });
            }

            else
            {
                /**
                 * A default implementation that goes beyond the Factory method's natural role can confuse
                 * the Factory method's implementers, so we decided to remove the default behavior that 
                 * was previously defined.
                 */
                Services.AddScoped(Services => Factory.Invoke(Services));
            }

            return Services;
        }

        /// <summary>
        /// Add <see cref="X509RequesterIdentityRecognizer"/>,
        /// <see cref="X509RequesterIdentityValidator"/>,
        /// <see cref="X509RequesterIdentityOptions"/>
        /// to the service collection.
        /// </summary>
        /// <param name="Services"></param>
        /// <param name="Resolver"></param>
        /// <returns></returns>
        public static IServiceCollection AddX509RequesterIdentityService<TCommandExecutor>(
            this IServiceCollection Services, Func<IServiceProvider, TCommandExecutor, X509CommandExecutor> Resolver = null)
            where TCommandExecutor : ICommandExecutor
        {
            Services
                .AddSingleton<X509RequesterIdentityRecognizer>()
                .AddSingleton<X509RequesterIdentityValidator>()
                .AddSingleton<X509RequesterIdentityOptions>()
                ;

            if (Resolver is null)
            {
                Services.AddScoped(Services =>
                {
                    var Executor = Services.GetRequiredService<TCommandExecutor>();
                    if (Executor is RemoteCommandExecutor Rce) return Rce.X509;

                    var Caches = Services.GetService<ICertificateCacheRepository>();
                    return new X509CommandExecutor(Executor, Caches);
                });
            }

            else
            {
                Services.AddScoped(Services =>
                {
                    var Executor = Services.GetService<TCommandExecutor>();
                    if (Executor is RemoteCommandExecutor Rce) return Rce.X509;

                    return Resolver.Invoke(Services, Executor);
                });
            }

            return Services;
        }

        /// <summary>
        /// Enable X509 identity for <see cref="RequesterIdentitySystem"/>.
        /// </summary>
        /// <param name="Builder"></param>
        /// <param name="Configure"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static RequesterIdentitySystemBuilder EnableX509Identity(
            this RequesterIdentitySystemBuilder Builder, Action<X509RequesterIdentityOptions> Configure = null)
        {
            var Services = Builder.ApplicationServices;
            var Recognizer = Services.GetService<X509RequesterIdentityRecognizer>();
            var Validator = Services.GetService<X509RequesterIdentityValidator>();
            var Options = Services.GetService<X509RequesterIdentityOptions>();

            if (Recognizer is null || Validator is null || Options is null)
                throw new InvalidOperationException("To enable X509 identity, please add x509 requester identity service.");

            Configure?.Invoke(Options);
            Builder.Recognizers.Add(Recognizer);
            Builder.Validators.Add(Validator);

            return Builder;
        }
    }
}
