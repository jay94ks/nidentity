namespace NIdentity.Core.Commands
{
    /// <summary>
    /// Command context.
    /// </summary>
    public class CommandContext
    {
        /// <summary>
        /// Service Provider.
        /// </summary>
        public IServiceProvider Services { get; set; }

        /// <summary>
        /// Command object to execute.
        /// </summary>
        public Command Command { get; set; }

        /// <summary>
        /// Command executor.
        /// </summary>
        public CommandExecutor CommandExecutor { get; set; }

        /// <summary>
        /// Command abort token.
        /// </summary>
        public CancellationToken CommandAborted { get; set; }
    }

    /// <summary>
    /// Command context.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public class CommandContext<TCommand> : CommandContext where TCommand: Command
    {
        /// <summary>
        /// Command object to execute.
        /// </summary>
        public new TCommand Command
        {
            get => base.Command as TCommand;
            set => base.Command = value;
        }
    }

}
