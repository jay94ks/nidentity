namespace NIdentity.Core.Commands
{
    /// <summary>
    /// Command Handler Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class CommandHandlerAttribute : Attribute
    {
        /// <summary>
        /// Initialize a new <see cref="CommandHandlerAttribute"/> instance.
        /// </summary>
        /// <param name="CommandType"></param>
        public CommandHandlerAttribute(Type CommandType)
        {
            this.CommandType = CommandType;
        }

        /// <summary>
        /// Command Type.
        /// </summary>
        public Type CommandType { get; }

        /// <summary>
        /// Command Kind.
        /// This will be used when the scanner finds command types.
        /// </summary>
        public string Kind { get; set; }
    }
}
