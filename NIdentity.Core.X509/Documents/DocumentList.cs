namespace NIdentity.Core.X509.Documents
{
    /// <summary>
    /// Document List.
    /// </summary>
    public class DocumentList
    {
        /// <summary>
        /// Path Name.
        /// </summary>
        public string PathName { get; set; }

        /// <summary>
        /// Document Names.
        /// </summary>
        public string[] Documents { get; set; }
    }
}
