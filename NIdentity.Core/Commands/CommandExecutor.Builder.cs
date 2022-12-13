using NIdentity.Core.Commands.Internals;
using NIdentity.Core.Helpers;
using System.Reflection;

namespace NIdentity.Core.Commands
{

    public partial class CommandExecutor
    {
        /// <summary>
        /// Command executor builder.
        /// </summary>
        public class Builder
        {
            private readonly Dictionary<string, Type> m_Types = new();
            private readonly Dictionary<Type, Func<CommandContext, Task<CommandResult>>> m_Executor = new();

            /// <summary>
            /// Map a type as its type name.
            /// </summary>
            /// <param name="Type"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            /// <exception cref="InvalidOperationException"></exception>
            public Builder MapType(Type Type, Func<CommandContext, Task<CommandResult>> Handler)
            {
                if (Type is null)
                    throw new ArgumentNullException(nameof(Type));

                if (Handler is null)
                    throw new ArgumentNullException(nameof(Handler));

                if (Type.IsAbstract || !Type.IsAssignableTo(typeof(Command)))
                    throw new ArgumentException($"the type, {Type.FullName} should be inherited from command class.");

                var Ctor = Type.GetConstructor(Type.EmptyTypes);
                if (Ctor is null)
                    throw new InvalidOperationException($"the type, {Type.FullName} has no default constructor.");

                var Attrs = Type.GetCustomAttributes<CommandAttribute>();
                if (Attrs.Count() > 0)
                {
                    foreach (var Attr in Attrs)
                    {
                        var Temp = Ctor.Invoke(EMPTY_ARGS) as Command;
                        if (m_Types.TryAdd($"{Attr.Kind}.{Temp.Type}", Type))
                        {
                            m_Executor[Type] = Handler;
                            continue;
                        }

                        m_Types.TryGetValue(Temp.Type, out var Dup);
                        throw new InvalidOperationException(
                            $"the type name, {Temp.Type} is already registered by {Dup.FullName}.");
                    }
                }
                else
                {
                    var Temp = Ctor.Invoke(EMPTY_ARGS) as Command;
                    if (m_Types.TryAdd(Temp.Type, Type))
                    {
                        m_Executor[Type] = Handler;
                        return this;
                    }

                    m_Types.TryGetValue(Temp.Type, out var Dup);
                    throw new InvalidOperationException(
                        $"the type name, {Temp.Type} is already registered by {Dup.FullName}.");
                }

                return this;
            }

            /// <summary>
            /// Map a type as its type name.
            /// </summary>
            /// <typeparam name="TCommand"></typeparam>
            /// <returns></returns>
            public Builder MapType<TCommand>(Func<CommandContext<TCommand>, Task<CommandResult>> Handler) where TCommand : Command, new()
            {
                if (Handler is null)
                    throw new ArgumentNullException(nameof(Handler));

                return MapType(typeof(TCommand), X => Handler.Invoke(X as CommandContext<TCommand>));
            }

            /// <summary>
            /// Enable "bulk" command type.
            /// </summary>
            /// <returns></returns>
            public Builder EnableBulk() => MapType<BulkCommand>(async Context =>
            {
                var Now = DateTime.Now;
                var Results = new List<CommandResult>();
                if (Context.Command.Actions != null)
                {
                    foreach (var Each in Context.Command.Actions)
                        Results.Add(await Context.CommandExecutor.Execute(Each, Context.CommandAborted));
                }

                return new BulkCommand.Result()
                {
                    Results = Results.ToArray(),
                    TotalTime = DateTime.Now - Now
                };
            });

            /// <summary>
            /// Map all commands by scanning the assembly.
            /// </summary>
            /// <param name="Assemblies"></param>
            /// <param name="Kind"></param>
            /// <returns></returns>
            public Builder MapAssemblies(string Kind, params Assembly[] Assemblies)
            {
                var Types = Assemblies.SelectMany(X => X.GetTypes());
                var CommandTypes = Types
                    .Where(X => !X.IsAbstract && X.IsAssignableTo(typeof(Command)))
                    .Where(X => X.GetCustomAttributes<CommandAttribute>().Where(Y => Y.Kind == Kind).Count() > 0)
                    .ToArray();

                var Availables = Types.Where(X => !X.IsAbstract)
                    .Where(X => X.GetCustomAttributes<CommandHandlerAttribute>().Where(Y => Y.Kind == Kind)
                    .Where(Y => CommandTypes.Contains(Y.CommandType)).Count() > 0)
                    .Select(X => (Type: X, Targets: X.GetCustomAttributes<CommandHandlerAttribute>()
                    .Where(Y => Y.Kind == Kind).Select(X => X.CommandType).ToArray()))
                    .ToArray();

                foreach (var Each in CommandTypes)
                    MapType(Each, Availables);

                return this;
            }

            /// <summary>
            /// Map a type using available handler list.
            /// </summary>
            /// <param name="Each"></param>
            /// <param name="Availables"></param>
            /// <exception cref="InvalidOperationException"></exception>
            private void MapType(Type Each, (Type Type, Type[] Targets)[] Availables)
            {
                var HandlerType = SelectHandlerType(Availables, Each);
                var HandlerCtor = HandlerType.GetConstructors().Where(X => X.IsPublic)
                    .Select(X => (Ctor: X, Params: X.GetParameters())).OrderByDescending(X => X.Params.Length)
                    .FirstOrDefault();

                if (HandlerCtor.Ctor is null)
                    throw new InvalidOperationException($"the handler type, {HandlerType} has no constructor.");

                var HandlerInfo = SelectHandlerMethod(Each, HandlerType);
                var ReturnType = HandlerInfo.Method.ReturnType.GetGenericArguments().First();

                if (ReturnType.IsAssignableTo(typeof(CommandResult)) == false)
                    throw new InvalidOperationException("the handler method's return type should be based on CommandResult.");

                // --> Map the handler here.
                MapType(Each, Context => ExecutionBody(Context, HandlerCtor, HandlerInfo, ReturnType));
            }

            /// <summary>
            /// Execution body for handler type.
            /// </summary>
            /// <param name="Context"></param>
            /// <param name="HandlerCtor"></param>
            /// <param name="HandlerInfo"></param>
            /// <param name="ReturnType"></param>
            /// <returns></returns>
            private static async Task<CommandResult> ExecutionBody(CommandContext Context, 
                (ConstructorInfo Ctor, ParameterInfo[] Params) HandlerCtor, 
                (MethodInfo Method, Type ParamType) HandlerInfo, Type ReturnType)
            {
                var Params = HandlerCtor.Params
                    .Select(X => Context.Services.GetService(X.ParameterType))
                    .ToArray();

                var Instance = HandlerCtor.Ctor.Invoke(Params);
                try
                {
                    var Return = (Task)HandlerInfo.Method
                        .Invoke(Instance, new object[] { Context });

                    return (
                        await TaskHelpers.Convert(Return, ReturnType))
                        as CommandResult;
                }

                finally
                {
                    if (Instance is IAsyncDisposable Async)
                        await Async.DisposeAsync();

                    if (Instance is IDisposable Sync)
                        Sync.Dispose();
                }
            }

            /// <summary>
            /// Select a handler.
            /// </summary>
            /// <param name="Availables"></param>
            /// <param name="Each"></param>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            /// <exception cref="AmbiguousMatchException"></exception>
            private static Type SelectHandlerType((Type Type, Type[] Targets)[] Availables, Type Each)
            {
                var EachHandlers = Availables
                    .Where(X => X.Targets.Contains(Each))
                    .Select(X => X.Type).ToArray();

                if (EachHandlers.Length <= 0)
                    throw new InvalidOperationException($"{Each.FullName} has no handler type.");

                if (EachHandlers.Length > 1)
                {
                    var Handlers = string.Join(", ", EachHandlers.Select(X => X.FullName));
                    throw new AmbiguousMatchException($"{Each.FullName} has too many handler types: {Handlers}.");
                }

                return EachHandlers.First();
            }

            /// <summary>
            /// Select a suitable handler method.
            /// </summary>
            /// <param name="Each"></param>
            /// <param name="HandlerType"></param>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            private static (MethodInfo Method, Type ParamType) SelectHandlerMethod(Type Each, Type HandlerType)
            {
                var ContextType = typeof(CommandContext<>).MakeGenericType(Each);
                var HandlerMethods = HandlerType.GetMethods()
                    .Where(X => X.Name == "ExecuteAsync").Where(X => FilterHandlerMethod(X, ContextType))
                    .Select(X => (Method: X, ParamType: X.GetParameters().Select(Y => Y.ParameterType).First()))
                    .OrderByDescending(X => X.ParamType == ContextType ? 1 : 0)
                    .ToArray();

                if (HandlerMethods.Length <= 0)
                    throw new InvalidOperationException($"{Each.FullName} has no suitable handler method, ExecuteAsync.");

                return HandlerMethods.First();
            }

            /// <summary>
            /// Filter Handler Method.
            /// </summary>
            /// <param name="Method"></param>
            /// <returns></returns>
            private static bool FilterHandlerMethod(MethodInfo Method, Type ContextType)
            {
                if (Method.IsPublic == false)
                    return false;

                var Params = Method.GetParameters();
                if (Params.Length != 1)
                    return false;

                if (Params[0].ParameterType != typeof(CommandContext) &&
                    Params[0].ParameterType != ContextType)
                    return false;

                if (!Method.ReturnType.IsConstructedGenericType)
                    return false;

                if (Method.ReturnType.GetGenericTypeDefinition() != typeof(Task<>))
                    return false;

                return true;
            }

            /// <summary>
            /// Build a <see cref="CommandExecutor"/> instance.
            /// </summary>
            /// <returns></returns>
            public CommandExecutor Build(IServiceProvider Services, CancellationToken Aborter= default)
            {
                var Registry = m_Types.Select(
                    X => (Type: X.Value, TypeName: X.Key, Handler: m_Executor[X.Value]));

                return new CommandExecutor(Services, Aborter, Registry);
            }
        }
    }

}
