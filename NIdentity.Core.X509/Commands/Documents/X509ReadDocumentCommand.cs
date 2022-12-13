using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Documents
{
    [Command(Kind = "x509")]
    public class X509ReadDocumentCommand : X509DocumentAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509ReadDocumentCommand"/>
        /// </summary>
        public X509ReadDocumentCommand() : base("doc_read")
        {
        }

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
