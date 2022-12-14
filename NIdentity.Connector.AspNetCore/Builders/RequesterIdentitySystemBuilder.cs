using NIdentity.Connector.AspNetCore.Abstractions;

namespace NIdentity.Connector.AspNetCore.Builders
{
    /// <summary>
    /// Builds <see cref="RequesterIdentitySystem"/>
    /// </summary>
    public class RequesterIdentitySystemBuilder
    {
        private readonly RequesterIdentitySystem m_System;

        /// <summary>
        /// Hide constructor.
        /// </summary>
        internal RequesterIdentitySystemBuilder(IServiceProvider Services, RequesterIdentitySystem System)
        {
            ApplicationServices = Services;
            m_System = System;
        }

        /// <summary>
        /// Application Services.
        /// </summary>
        public IServiceProvider ApplicationServices { get; }

        /// <summary>
        /// Recognizers.
        /// </summary>
        public IList<IRequesterIdentityRecognizer> Recognizers => m_System.MutableRecognizers;

        /// <summary>
        /// Validators.
        /// </summary>
        public IList<IRequesterIdentityValidator> Validators => m_System.MutableValidators;
    }
}
