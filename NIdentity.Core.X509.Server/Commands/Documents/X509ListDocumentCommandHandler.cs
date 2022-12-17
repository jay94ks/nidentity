using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands.Documents;
using NIdentity.Core.X509.Server.Commands.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509.Server.Commands.Documents
{
    [CommandHandler(typeof(X509ListDocumentCommand), Kind = "x509")]
    public class X509ListDocumentCommandHandler : X509DocumentCommandHandler<X509ListDocumentCommand>
    {
        /// <summary>
        /// Initialize a new <see cref="X509ListDocumentCommandHandler"/> instance.
        /// </summary>
        /// <param name="Requester"></param>
        public X509ListDocumentCommandHandler(X509RequesterAccesor Requester) : base(Requester)
        {
        }

        /// <inheritdoc/>
        public override async Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Aborter = Context.CommandAborted;

            var List = await Context.Repository.ListAsync(Context.Owner, Request.PathName, Aborter);
            if (List is null)
            {
                return new X509ListDocumentCommand.Result
                {
                    Children = new string[0]
                };
            }

            return X509ListDocumentCommand.Result.Make(Context.Document,
                X => X.Children = List.Documents);
        }
    }
}
