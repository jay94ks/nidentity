using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509.Commands.Documents
{
    /// <summary>
    /// A command to request document list of certificate.
    /// </summary>
    [Command(Kind = "x509")]
    public class X509ListDocumentCommand : X509DocumentAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509ListDocumentCommand"/>
        /// </summary>
        public X509ListDocumentCommand() : base("doc_list")
        {
        }

        /// <summary>
        /// Listing result.
        /// </summary>
        public class Result : DocumentResult<Result>
        {
            /// <summary>
            /// Children documents.
            /// </summary>
            [JsonProperty("children")]
            public string[] Children { get; set; }
        }
    }
}
