using Newtonsoft.Json.Linq;
using NIdentity.Connector.Internals;
using NIdentity.Connector.X509;
using NIdentity.Core;
using NIdentity.Core.Commands;
using NIdentity.Core.Helpers;
using NIdentity.Core.X509;

namespace NIdentity.Connector
{
    /// <summary>
    /// Executes command on the remote server.
    /// </summary>
    public class RemoteCommandExecutor : ICommandExecutor, IDisposable
    {
        private readonly RemoteCommandExecutorParameters m_Parameters;
        private ICommandExecutor m_Remoter;

        /// <summary>
        /// Initialize a new <see cref="RemoteCommandExecutor"/> instance.
        /// </summary>
        /// <param name="Parameters"></param>
        public RemoteCommandExecutor(RemoteCommandExecutorParameters Parameters)
        {
            Parameters.ThrowExceptionIfInvalid();
            switch(Parameters.Mode)
            {
                case RemoteCommandExecutorMode.Https:
                    SetRemoter(new HttpRequestExecutor(Parameters));
                    break;

                case RemoteCommandExecutorMode.WebSockets:
                    SetRemoter(m_Remoter = new WebSocketExecutor(Parameters));
                    break;

                default:
                    throw new InvalidOperationException("not supported mode");
            }

            m_Parameters = Parameters;
            X509 = new X509CommandExecutor(this, Parameters);
        }

        /// <summary>
        /// Initialize a new <see cref="RemoteCommandExecutor"/> instance.
        /// </summary>
        public RemoteCommandExecutor(Uri ServerUri, Certificate Certificate, TimeSpan? Timeout = null)
            : this(new RemoteCommandExecutorParameters
            {
                ServerUri = ServerUri,
                Certificate = Certificate,
                Timeout = Timeout.HasValue
                    ? Timeout.Value : TimeSpan.FromSeconds(15),
                Mode = RemoteCommandExecutorMode.Https
            })
        {

        }

        /// <summary>
        /// Executor's shared objects.
        /// </summary>
        public IDictionary<object, object> Items { get; } = new Dictionary<object, object>();

        /// <summary>
        /// X509 Command Executors.
        /// </summary>
        public X509CommandExecutor X509 { get; }

        /// <summary>
        /// Executing command report.
        /// </summary>
        public event Action<JObject> Executing;

        /// <summary>
        /// Executed command report.
        /// </summary>
        public event Action<CommandResult> Executed;

        /// <inheritdoc/>
        public void Dispose()
        {
            ICommandExecutor Remoter;
            lock(this)
            {
                if ((Remoter = m_Remoter) is null)
                    return;

                m_Remoter = null;
            }

            if (Remoter is IDisposable D)
                D.Dispose();
        }

        /// <inheritdoc/>
        public Task<CommandResult> Execute(Command Command, CancellationToken Token = default)
            => ExecuteUsingRemoter(async X =>
            {
                Executing?.Invoke(Command.ToJson());
                var Result = await X.Execute(Command, Token);
                Executed?.Invoke(Result);
                return Result;
            });

        /// <inheritdoc/>
        public Task<CommandResult> Execute(JObject Json, CancellationToken Token = default)
            => ExecuteUsingRemoter(async X =>
            {
                Executing?.Invoke(Json);
                var Result = await X.Execute(Json, Token);
                Executed?.Invoke(Result);
                return Result;
            });

        /// <summary>
        /// Get the remoter instance.
        /// </summary>
        /// <returns></returns>
        private ICommandExecutor GetRemoter() { lock (this) return m_Remoter; }

        /// <summary>
        /// Set the remoter instance.
        /// </summary>
        /// <param name="New"></param>
        private void SetRemoter(ICommandExecutor New)
        {
            ICommandExecutor Old;
            lock (this)
            {
                Old = m_Remoter;
                m_Remoter = New;
            }

            if (Old != New)
            {
                if (Old is IDisposable D)
                    D.Dispose();
            }
        }

        /// <summary>
        /// Execute the command by.
        /// </summary>
        /// <param name="Executor"></param>
        /// <returns></returns>
        private Task<CommandResult> ExecuteUsingRemoter(Func<ICommandExecutor, Task<CommandResult>> Executor)
        {
            var Remoter = GetRemoter();

            if (Remoter is WebSocketExecutor Ws && !Ws.IsConnected)
                SetRemoter(Remoter = null);

            if (Remoter is null)
            {
                if (m_Parameters.Mode == RemoteCommandExecutorMode.WebSockets)
                    SetRemoter(Remoter = new WebSocketExecutor(m_Parameters));

                if (m_Parameters.Mode == RemoteCommandExecutorMode.Https)
                    SetRemoter(Remoter = new HttpRequestExecutor(m_Parameters));
            }

            return Executor.Invoke(Remoter);
        }
    }
}
