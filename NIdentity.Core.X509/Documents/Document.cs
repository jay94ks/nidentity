namespace NIdentity.Core.X509.Documents
{
    /// <summary>
    /// Document.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Identity.
        /// </summary>
        public DocumentIdentity Identity { get; set; }

        /// <summary>
        /// Access Mode.
        /// </summary>
        public DocumentAccess Access { get; set; }

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Last Write Time.
        /// </summary>
        public DateTimeOffset LastWriteTime { get; set; }

        /// <summary>
        /// Revision Number.
        /// </summary>
        public long RevisionNumber { get; set; }

        /// <summary>
        /// Mime Type.
        /// </summary>
        public string MimeType { get; set; } = "text/plain";

        /// <summary>
        /// Has children or not.
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Data bytes.
        /// </summary>
        public string Data { get; set; }
    }
}
