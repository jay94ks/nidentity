using Microsoft.Extensions.DependencyInjection;
using NIdentity.Connector.AspNetCore.Extensions;
using NIdentity.Connector.AspNetCore.Identities.X509;
using NIdentity.Core.Commands;

namespace NIdentity.Core.X509.Server.Commands.Bases
{
    /// <summary>
    /// Base class of X509 commands.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public abstract class X509CertificateCommandHandler<TCommand> where TCommand : Command
    {
        private readonly X509RequesterAccesor m_Requester;

        /// <summary>
        /// Initialize a new <see cref="X509Command"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        protected X509CertificateCommandHandler(X509RequesterAccesor Requester) 
            => m_Requester = Requester;

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
            private ICertificateRepository m_Repository;
            private IMutableCertificateRepository m_MutableRepository;

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
            /// Repository.
            /// </summary>
            public ICertificateRepository Repository => Cached(ref m_Repository);

            /// <summary>
            /// Mutable Repository.
            /// </summary>
            public IMutableCertificateRepository MutableRepository => Cached(ref m_MutableRepository);
        }

        /// <summary>
        /// Execute the X509 command.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public Task<CommandResult> ExecuteAsync(CommandContext<TCommand> Context)
        {
            var NewContext = new X509CommandContext
            {
                Command = Context.Command,
                CommandAborted = Context.CommandAborted,
                Services = Context.Services
            };

            return ExecuteAsync(NewContext);
        }

        /// <summary>
        /// Execute the X509 command.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public abstract Task<CommandResult> ExecuteAsync(X509CommandContext Context);
    }
}
