using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Documents
{
    [Command(Kind = "x509")]
    public class X509RemoveDocumentCommand : X509DocumentAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509RemoveDocumentCommand"/>
        /// </summary>
        public X509RemoveDocumentCommand() : base("doc_remove")
        {
        }

        public class Result : DocumentResult<Result>
        {
        }

        /// <summary>
        /// Revision. (for concurrency controls)
        /// </summary>
        [JsonProperty("revision")]
        public long? Revision { get; set; }
    }
}
