namespace NIdentity.Core.X509.Documents
{
    /// <summary>
    /// Document Access.
    /// </summary>
    public enum DocumentAccess
    {
        /// <summary>
        /// Public Read
        /// </summary>
        Public = 0,

        /// <summary>
        /// Private Read/Write
        /// </summary>
        Private,

        /// <summary>
        /// Authority Read/Write
        /// </summary>
        Authority,

        /// <summary>
        /// Super Read/Write.
        /// </summary>
        Super
    }
}
