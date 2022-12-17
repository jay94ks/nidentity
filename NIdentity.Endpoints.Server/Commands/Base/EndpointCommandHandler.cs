using NIdentity.Connector.AspNetCore;
using NIdentity.Connector.AspNetCore.Identities.X509;
using NIdentity.Core.Commands;
using NIdentity.Core.X509;
using NIdentity.Core.X509.Server;
using NIdentity.Endpoints.Commands.Bases;

namespace NIdentity.Endpoints.Server.Commands.Base
{
    /// <summary>
    /// Base class of Endpoint commands.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public abstract class EndpointCommandHandler<TCommand> where TCommand : Command
    {
        private EndpointRequesterAccesor m_Requester;

        /// <summary>
        /// Initialize a new <see cref="EndpointCommandHandler{TCommand}"/> instance.
        /// This requires <see cref="RequesterIdentityKind.Signature"/> or above.
        /// </summary>
        /// <param name="Requester"></param>
        protected EndpointCommandHandler(EndpointRequesterAccesor Requester)
        {
            m_Requester = Requester;
        }

        /// <summary>
        /// Requester Certificate.
        /// </summary>
        public Certificate Requester => m_Requester.Requester;

        /// <summary>
        /// Indicates whether the requester is super access or not.
        /// </summary>
        public bool IsSuperAccess => m_Requester.IsSuperAccess;

        /// <summary>
        /// Command Context.
        /// </summary>
        public class EndpointCommandContext : CommandContext<TCommand>
        {
            private IEndpointRepository m_Endpoints;
            private IMutableEndpointRepository m_MutableEndpoints;

            private IEndpointNetworkRepository m_Networks;
            private IMutableEndpointNetworkRepository m_MutableNetworks;

            private IEndpointInventoryRepository m_Inventories;
            private IMutableEndpointInventoryRepository m_MutableInventories;

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
            /// Endpoint Repository.
            /// </summary>
            public IEndpointRepository Endpoints => Cached(ref m_Endpoints);

            /// <summary>
            /// Network Repository.
            /// </summary>
            public IEndpointNetworkRepository Networks => Cached(ref m_Networks);

            /// <summary>
            /// Inventory Repository.
            /// </summary>
            public IEndpointInventoryRepository Inventories => Cached(ref m_Inventories);

            /// <summary>
            /// Mutable Endpoint Repository.
            /// </summary>
            public IMutableEndpointRepository MutableEndpoints => Cached(ref m_MutableEndpoints);

            /// <summary>
            /// Mutable Network Repository.
            /// </summary>
            public IMutableEndpointNetworkRepository MutableNetworks => Cached(ref m_MutableNetworks);

            /// <summary>
            /// Mutable Inventory Repository.
            /// </summary>
            public IMutableEndpointInventoryRepository MutableInventories => Cached(ref m_MutableInventories);
        }

        /// <summary>
        /// Execute the X509 command.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public async Task<CommandResult> ExecuteAsync(CommandContext<TCommand> Context)
        {
            var NewContext = new EndpointCommandContext
            {
                Command = Context.Command,
                CommandAborted = Context.CommandAborted,
                Services = Context.Services
            };

            if (await IsAuthorizedRequester(NewContext))
                return await ExecuteAsync(NewContext);

            throw new InvalidOperationException("unauthorized access.");
        }

        /// <summary>
        /// Execute the X509 command.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public abstract Task<CommandResult> ExecuteAsync(EndpointCommandContext Context);

        /// <summary>
        /// Test whether the requester is authorized or not.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        protected virtual async Task<bool> IsAuthorizedRequester(EndpointCommandContext Context)
        {
            if (Context.Command is EidSensitiveCommand)
            {
                // --> test whether the authority is authority or not.
                if (Requester is null || Requester.IsAuthority == false)
                    return false;

                // --> test whether the requester is owner (or issuer, aka, authority) of inventory or not.
                if (Context.Command is EidInventoryCommand Command)
                {
                    var Inventory = await Context.Inventories.GetAsync(Command.Identity, Context.CommandAborted);

                    var IsAuthorityAccess = false;
                    var IsOwnerAccess = Inventory.Owner.IsExact(Requester);
                    if (IsOwnerAccess == false)
                    {
                        // --> try to load owner certificate and,
                        var Certificates = Context.Services.GetRequiredService<ICertificateRepository>();
                        var Owner = await Certificates.LoadAsync(Inventory.Owner, Context.CommandAborted);
                        if (Owner is null)
                            return false;

                        // --> test whether the owner's certificate is issued by requester or not.
                        IsAuthorityAccess = await Certificates.IsIssuerAsync(Requester, Owner, Context.CommandAborted);
                    }

                    // --> finally, decide whether the requester has access permission or not.
                    if (Inventory is null || (IsOwnerAccess == false && IsAuthorityAccess == false))
                        return false;
                }
            }

            return true;
        }
    }

}
