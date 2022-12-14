using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Documents
{
    /// <summary>
    /// A command to receive document content.
    /// </summary>
    [Command(Kind = "x509")]
    public class X509ReadDocumentCommand : X509DocumentAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509ReadDocumentCommand"/>
        /// </summary>
        public X509ReadDocumentCommand() : base("doc_read")
        {
        }

        /// <summary>
        /// Document data result.
        /// </summary>
        public class Result : DocumentResult<Result>
        {
            /// <summary>
            /// Data
            /// </summary>
            [JsonProperty("data")]
            public string Data { get; set; }
        }

        /// <summary>
        /// Revision. (for concurrency controls)
        /// </summary>
        [JsonProperty("revision")]
        public long? Revision { get; set; }
    }
}
