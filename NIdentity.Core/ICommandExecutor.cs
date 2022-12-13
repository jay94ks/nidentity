using Newtonsoft.Json.Linq;
using NIdentity.Core.Commands;

namespace NIdentity.Core
{
    public interface ICommandExecutor
    {
        /// <summary>
        /// Execute a <see cref="Command"/> instance and returns its execution result.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        Task<CommandResult> Execute(Command Command, CancellationToken Token = default);

        /// <summary>
        /// Execute a <see cref="Command"/> instance and returns its execution result.
        /// </summary>
        /// <param name="Command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        Task<CommandResult> Execute(JObject Json, CancellationToken Token = default);
    }
}