namespace NIdentity.Connector
{
    /// <summary>
    /// Protocol to use.
    /// </summary>
    public enum RemoteCommandExecutorMode
    {
        /// <summary>
        /// Through Https Request.
        /// </summary>
        Https,

        /// <summary>
        /// Through Https WebSocket.
        /// </summary>
        WebSockets
    }
}
