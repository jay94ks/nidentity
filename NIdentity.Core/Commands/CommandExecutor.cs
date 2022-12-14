using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIdentity.Core.Commands.Internals;
using NIdentity.Core.Helpers;

namespace NIdentity.Core.Commands
{
    /// <summary>
    /// Command executor.
    /// </summary>
    public partial class CommandExecutor : ICommandExecutor
    {
        private static readonly object[] EMPTY_ARGS = new object[0];

        private readonly Dictionary<string, Type> m_Types = new();
        private readonly Dictionary<Type, Func<CommandContext, Task<CommandResult>>> m_Executor = new();

        private readonly IServiceProvider m_Services;
        private readonly CancellationToken m_Aborter;

        /// <summary>
        /// Initialize a new <see cref="CommandExecutor"/> instance.
        /// </summary>
        /// <param name="Services"></param>
        /// <param name="Aborter"></param>
        /// <param name="Registry"></param>
        private CommandExecutor(
            IServiceProvider Services, CancellationToken Aborter,
            IEnumerable<(Type Type, string TypeName, Func<CommandContext, Task<CommandResult>> Handler)> Registry)
        {
            m_Services = Services;
            m_Aborter = Aborter;

            foreach (var Each in Registry)
            {
                m_Types[Each.TypeName] = Each.Type;
                m_Executor[Each.Type] = Each.Handler;
            }
        }

        /// <summary>
        /// Parse <see cref="Command"/> from JSON string.
        /// If not registered type specified, this will just return null.
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public Command Parse(string Json)
        {
            if (string.IsNullOrWhiteSpace(Json))
                return null;

            try { return Parse(JsonConvert.DeserializeObject<JObject>(Json)); }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Parse <see cref="Command"/> from <see cref="JObject"/> instance.
        /// If not registered type specified, this will just return null.
        /// </summary>
        /// <param name="Json"></param>
        /// <exception cref="ArgumentException">the input JSON is not valid command instance.</exception>
        /// <returns></returns>
        public Command Parse(JObject Json)
        {
            if (Json is null)
                return null;

            var TypeName = Json.Get<string>("type");
            if (m_Types.TryGetValue(TypeName, out var Type))
                return Json.ToObject(Type) as Command;

            return null;
        }

        /// <summary>
        /// Execute a <see cref="Command"/> instance and returns its execution result.
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<CommandResult> Execute(JObject Json, CancellationToken Token = default)
        {
            try
            {
                var Command = Parse(Json);
                if (Command is null)
                    throw new ArgumentException("the input JSON is not valid command");

                return await Execute(Command, Token);
            }

            catch (Exception Error)
            {
                return new CommandResult
                {
                    Success = false,
                    Reason = Error.Message,
                    ReasonKind = MakeReasonKind(Error)
                };
            }
        }

        /// <summary>
        /// Execute a <see cref="Command"/> instance and returns its execution result.
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<CommandResult> Execute(Command Command, CancellationToken Token = default)
        {
            try
            {
                if (Command is null)
                    throw new ArgumentNullException(nameof(Command));

                var Type = Command.GetType();
                if (!m_Executor.TryGetValue(Type, out var Handler))
                    throw new InvalidOperationException($"the type, {Command.Type} is unknown.");

                if (!Command.IsValid())
                    throw new ArgumentException("the input command object is not valid.");

                using var Cts = CancellationTokenSource.CreateLinkedTokenSource(m_Aborter, Token);
                var Context = typeof(CommandContext<>)
                    .MakeGenericType(Type).GetConstructor(Type.EmptyTypes)
                    .Invoke(EMPTY_ARGS) as CommandContext;

                Context.Command = Command;
                Context.CommandExecutor = this;
                Context.CommandAborted = Cts.Token;
                Context.Services = m_Services;

                var Result = await Handler.Invoke(Context);
                if (Result is null)
                {
                    return new CommandResult
                    {
                        Success = true,
                        Reason = null,
                        ReasonKind = null
                    };
                }

                return Result;
            }

            catch (Exception Error)
            {
                return new CommandResult
                {
                    Success = false,
                    Reason = Error.Message,
                    ReasonKind = MakeReasonKind(Error)
                };
            }
        }

        /// <summary>
        /// Make the reason kind from <see cref="Exception"/>.
        /// </summary>
        /// <param name="Error"></param>
        /// <returns></returns>
        private static string MakeReasonKind(Exception Error)
        {
            var BaseName = typeof(Exception).Name;
            var TypeName = Error.GetType().Name;

            if (TypeName.EndsWith(BaseName))
                TypeName = TypeName.Substring(0, TypeName.Length - BaseName.Length);

            if (string.IsNullOrWhiteSpace(TypeName))
                return BaseName;

            return TypeName;
        }
    }

}
