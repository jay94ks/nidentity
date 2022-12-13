using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;
using NIdentity.Core.X509.Documents;

namespace NIdentity.Core.X509.Commands.Documents
{
    [Command(Kind = "x509")]
    public class X509WriteDocumentCommand : X509DocumentAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509WriteDocumentCommand"/>
        /// </summary>
        public X509WriteDocumentCommand() : base("doc_write")
        {
        }

        public class Result : DocumentResult<Result>
        {
        }

        /// <summary>
        /// Previous Revision. (for concurrency controls)
        /// </summary>
        [JsonProperty("prev_revision")]
        public long? PreviousRevision { get; set; }

        /// <summary>
        /// Access Mode. (set non-null to alter)
        /// </summary>
        [JsonProperty("access")]
        public DocumentAccess? Access { get; set; }

        /// <summary>
        /// Mime Type. (set non-null to alter)
        /// </summary>
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        /// <summary>
        /// Document Data.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }
    }
}
