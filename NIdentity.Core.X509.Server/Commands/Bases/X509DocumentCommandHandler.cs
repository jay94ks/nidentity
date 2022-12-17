using Microsoft.Extensions.DependencyInjection;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;
using NIdentity.Core.X509.Commands.Documents;
using NIdentity.Core.X509.Documents;
using NIdentity.Core.X509.Server.Documents;

namespace NIdentity.Core.X509.Server.Commands.Bases
{
    /// <summary>
    /// Base class of X509 commands.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public abstract class X509DocumentCommandHandler<TCommand> where TCommand : X509DocumentAccessCommand
    {
        private readonly X509RequesterAccesor m_Requester;

        /// <summary>
        /// Initialize a new <see cref="X509DocumentCommandHandler{TCommand}"/> instance.
        /// </summary>
        protected X509DocumentCommandHandler(X509RequesterAccesor Requester) => m_Requester = Requester;

        /// <summary>
        /// Requester's certificate.
        /// </summary>
        public Certificate Requester => m_Requester.Requester;

        /// <summary>
        /// Indicates whether the requester has super-access or not.
        /// </summary>
        public bool IsSuperAccess => m_Requester.IsSuperAccess;

        /// <summary>
        /// Command Context.
        /// </summary>
        public class X509CommandContext : CommandContext<TCommand>
        {
            private ICertificateRepository m_CertificateRepository;

            private IDocumentRepository m_Repository;
            private IMutableDocumentRepository m_MutableRepository;
            private ICertificatePermissionManager m_Permissions;
            private IMutableCertificatePermissionManager m_MutablePermissions;

            /// <summary>
            /// Get the required service with cache.
            /// </summary>
            /// <typeparam name="TReturn"></typeparam>
            /// <param name="Store"></param>
            /// <returns></returns>
            private TReturn Cached<TReturn>(ref TReturn Store) where TReturn : class
            {
                if (Store is null)
                    Store = Services.GetRequiredService<TReturn>();

                return Store;
            }

            /// <summary>
            /// Certificate Repository.
            /// </summary>
            public ICertificateRepository CertificateRepository => Cached(ref m_CertificateRepository);

            /// <summary>
            /// Repository.
            /// </summary>
            public IDocumentRepository Repository => Cached(ref m_Repository);

            /// <summary>
            /// Mutable Repository.
            /// </summary>
            public IMutableDocumentRepository MutableRepository => Cached(ref m_MutableRepository);

            /// <summary>
            /// Permissions.
            /// </summary>
            public ICertificatePermissionManager Permissions => Cached(ref m_Permissions);

            /// <summary>
            /// Mutable Permissions.
            /// </summary>
            public IMutableCertificatePermissionManager MutablePermissions => Cached(ref m_MutablePermissions);

            /// <summary>
            /// Owner.
            /// </summary>
            public Certificate Owner { get; set; }

            /// <summary>
            /// Document.
            /// </summary>
            public Document Document { get; set; }
        }

        /// <summary>
        /// Execute the X509 command.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public async Task<CommandResult> ExecuteAsync(CommandContext<TCommand> Context)
        {
            var NewContext = new X509CommandContext
            {
                Command = Context.Command,
                CommandAborted = Context.CommandAborted,
                Services = Context.Services
            };

            var Request = Context.Command;
            var Aborter = Context.CommandAborted;

            await LoadTargets(NewContext, Request, Aborter);
            await CheckAccessMode(NewContext, Aborter);
            return await ExecuteAsync(NewContext);
        }

        /// <summary>
        /// Load owner's certificate and document.
        /// </summary>
        /// <param name="NewContext"></param>
        /// <param name="Request"></param>
        /// <param name="Aborter"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        private static async Task LoadTargets(X509CommandContext NewContext, TCommand Request, CancellationToken Aborter)
        {
            NewContext.Owner = await NewContext.CertificateRepository
                .LoadAsync(Request.Identity.Owner, NewContext.CommandAborted);

            if (NewContext.Owner is null)
                throw new KeyNotFoundException("no target document's certificate exists.");

            NewContext.Document = await NewContext.Repository.ReadAsync(Request.Identity, null, Aborter);

            if (Request is X509WriteDocumentCommand)
                return;

            if (NewContext.Document is null)
                throw new KeyNotFoundException("no such document exists.");
        }

        /// <summary>
        /// Check the access mode and verify permissions.
        /// </summary>
        /// <param name="NewContext"></param>
        /// <param name="Aborter"></param>
        /// <returns></returns>
        /// <exception cref="AccessViolationException"></exception>
        private async Task CheckAccessMode(X509CommandContext NewContext, CancellationToken Aborter)
        {
            bool IsWriteAccess = NewContext.Command is X509WriteDocumentCommand;
            if (IsSuperAccess == false)
            {
                var IsOwnerAccess = NewContext.Owner.Self.IsExact(Requester);
                if (IsWriteAccess == true)
                {
                    // --> to make a new document, writer should be authority, owner of document or super access.
                    var IsAuthority = await NewContext.CertificateRepository.IsIssuerAsync(Requester, NewContext.Owner, Aborter);
                    if (IsAuthority == false && IsOwnerAccess == false)
                        throw new AccessViolationException("access denied.");

                    return;
                }

                switch (NewContext.Document.Access)
                {
                    case DocumentAccess.Public:
                        break;

                    case DocumentAccess.Private:
                        {
                            if (Requester is null || IsOwnerAccess == false)
                                throw new AccessViolationException("access denied.");
                        }
                        break;

                    case DocumentAccess.Authority:
                        {
                            if (Requester is null)
                                throw new AccessViolationException("access denied.");


                            // --> If self-signed certificate, it can access to.
                            if (Requester.IsSelfSigned == false)
                            {
                                if (IsOwnerAccess == true)
                                    throw new AccessViolationException("access denied.");

                                if (await NewContext.CertificateRepository.IsIssuerAsync(Requester, NewContext.Owner, Aborter) == false)
                                    throw new AccessViolationException("access denied.");
                            }
                        }
                        break;

                    case DocumentAccess.Super: // --> can be accessed by IsSuperAccess == true.
                        throw new AccessViolationException("access denied.");
                }
            }
        }

        /// <summary>
        /// Execute the X509 command.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public abstract Task<CommandResult> ExecuteAsync(X509CommandContext Context);
    }
}
