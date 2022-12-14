using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Documents;

namespace NIdentity.Core.X509.Commands
{
    /// <summary>
    /// Base class for document access commands.
    /// </summary>
    public abstract class X509DocumentAccessCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="X509CertificateAccessCommand"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        protected X509DocumentAccessCommand(string Type) : base(Type)
        {
        }

        /// <summary>
        /// Document result.
        /// </summary>
        public class DocumentResult : CommandResult
        {
            /// <summary>
            /// Make a document result.
            /// </summary>
            /// <param name="Document"></param>
            /// <returns></returns>
            public static DocumentResult Make(Document Document) => Make<DocumentResult>(Document);

            /// <summary>
            /// Make a document result.
            /// </summary>
            /// <typeparam name="TResult"></typeparam>
            /// <param name="Document"></param>
            /// <param name="More"></param>
            /// <returns></returns>
            public static TResult Make<TResult>(Document Document, Action<TResult> More = null) where TResult : DocumentResult, new()
            {
                var Result = new TResult()
                {
                    Subject = Document.Identity.Owner.Subject,
                    KeyIdentifier = Document.Identity.Owner.KeyIdentifier,
                    PathName = DocumentIdentity.NormalizePathName(Document.Identity.PathName),
                    Access = Document.Access,
                    CreationTime = Document.CreationTime,
                    LastWriteTime = Document.LastWriteTime,
                    Revision = Document.RevisionNumber,
                    MimeType = Document.MimeType
                };

                More?.Invoke(Result);
                return Result;
            }

            /// <summary>
            /// Generated Certificate's Subject.
            /// </summary>
            [JsonProperty("subject")]
            public string Subject { get; set; }

            /// <summary>
            /// Generated Certificate's Key Identifier.
            /// </summary>
            [JsonProperty("key_id")]
            public string KeyIdentifier { get; set; }

            /// <summary>
            /// Path Name.
            /// </summary>
            [JsonProperty("path")]
            public string PathName { get; set; }

            /// <summary>
            /// Access Mode.
            /// </summary>
            [JsonProperty("access")]
            public DocumentAccess Access { get; set; }

            /// <summary>
            /// Creation Time.
            /// </summary>
            [JsonProperty("creation_time")]
            public DateTimeOffset CreationTime { get; set; }

            /// <summary>
            /// Last Write Time.
            /// </summary>
            [JsonProperty("last_write_time")]
            public DateTimeOffset LastWriteTime { get; set; }

            /// <summary>
            /// Revision Number.
            /// </summary>
            [JsonProperty("revision")]
            public long Revision { get; set; }

            /// <summary>
            /// Mime Type.
            /// </summary>
            [JsonProperty("mime_type")]
            public string MimeType { get; set; }

            /// <summary>
            /// Has children or not.
            /// </summary>
            [JsonProperty("has_children")]
            public bool HasChildren { get; set; }
        }

        /// <summary>
        /// Document result (generic version)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        public class DocumentResult<TResult> : DocumentResult where TResult : DocumentResult, new()
        {
            /// <summary>
            /// Make a document result.
            /// </summary>
            /// <param name="Document"></param>
            /// <param name="More"></param>
            /// <returns></returns>
            public static TResult Make(Document Document, Action<TResult> More = null) => Make<TResult>(Document, More);
        }

        // -------------- (by key id) --

        /// <summary>
        /// Subject.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Key Identifier.
        /// </summary>
        [JsonProperty("key_id")]
        public string KeyIdentifier { get; set; }

        /// <summary>
        /// Path Name.
        /// </summary>
        [JsonProperty("path")]
        public string PathName { get; set; }

        /// <summary>
        /// Owner identity.
        /// </summary>
        [JsonIgnore]
        public CertificateIdentity Owner => new CertificateIdentity(Subject, KeyIdentifier);

        /// <summary>
        /// Document identity.
        /// </summary>
        [JsonIgnore]
        public DocumentIdentity Identity => new DocumentIdentity(Owner, PathName);

    }
}
