using NIdentity.Connector.AspNetCore.Abstractions;

namespace NIdentity.Connector.AspNetCore
{
    /// <summary>
    /// Requester Identity System.
    /// </summary>
    public sealed class RequesterIdentitySystem
    {
        private readonly List<IRequesterIdentityRecognizer> m_Recognizers = new();
        private readonly List<IRequesterIdentityValidator> m_Validators = new();

        /// <summary>
        /// Recognizers.
        /// </summary>
        internal IList<IRequesterIdentityRecognizer> MutableRecognizers => m_Recognizers;

        /// <summary>
        /// Validators.
        /// </summary>
        internal IList<IRequesterIdentityValidator> MutableValidators => m_Validators;

        /// <summary>
        /// Recognizers.
        /// </summary>
        public IReadOnlyList<IRequesterIdentityRecognizer> Recognizers => m_Recognizers;

        /// <summary>
        /// Validators.
        /// </summary>
        public IReadOnlyList<IRequesterIdentityValidator> Validators => m_Validators;
    }
}
