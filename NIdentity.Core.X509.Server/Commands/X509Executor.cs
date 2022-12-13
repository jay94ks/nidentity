using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NIdentity.Core.Commands;

namespace NIdentity.Core.X509.Server.Commands
{
    /// <summary>
    /// X509 Executor for internal-use.
    /// </summary>
    internal class X509Executor
    {
        private readonly CommandExecutor m_Executor;

        /// <summary>
        /// Initialize a new <see cref="X509Executor"/> instance.
        /// </summary>
        public X509Executor(IServiceProvider Services, CancellationToken Aborter = default)
        {
            m_Executor = new CommandExecutor.Builder()
                .MapX509ServerCommands().EnableBulk()
                .Build(Services, Aborter);

            Authorization = Services.GetRequiredService<X509RequesterAccesor>();
        }

        /// <summary>
        /// Authorization information.
        /// </summary>
        public X509RequesterAccesor Authorization { get; set; }

        /// <summary>
        /// Execute a <see cref="Command"/> instance and returns its execution result.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<CommandResult> Execute(JObject Json, CancellationToken Token = default)
            => m_Executor.Execute(Json, Token);

        /// <summary>
        /// Execute a <see cref="Command"/> instance and returns its execution result.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Task<CommandResult> Execute(Command Command, CancellationToken Token = default)
            => m_Executor.Execute(Command, Token);
    }
}
