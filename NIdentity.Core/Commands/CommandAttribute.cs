namespace NIdentity.Core.Commands
{
    /// <summary>
    /// Command Attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Command Kind.
        /// This will be used when the scanner finds command types.
        /// </summary>
        public string Kind { get; set; }

        /// <summary>
        /// Result Type.
        /// If nothing specified, remote command executor will just return remote command as-is.
        /// </summary>
        public Type ResultType { get; set; }
    }
}
