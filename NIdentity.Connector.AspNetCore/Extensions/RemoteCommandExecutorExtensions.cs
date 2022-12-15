using NIdentity.Core;

namespace NIdentity.Connector.AspNetCore.Extensions
{
    /// <summary>
    /// Provides <see cref="RemoteCommandExecutor"/> configuration methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class RemoteCommandExecutorExtensions
    {

        /// <summary>
        /// Add a remote command executor.
        /// </summary>
        /// <param name="Services"></param>
        /// <param name="Configure"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddRemoteCommandExecutor(this IServiceCollection Services,
            Action<IServiceProvider, RemoteCommandExecutorParameters> Configure)
        {
            if (Configure is null)
                throw new ArgumentNullException(nameof(Configure));

            var Parameters = new RemoteCommandExecutorParameters();
            return Services.AddScoped<ICommandExecutor>(Services =>
            {
                Configure.Invoke(Services, Parameters);
                return new RemoteCommandExecutor(Parameters);
            });
        }
    }
}
