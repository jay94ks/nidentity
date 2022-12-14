using NIdentity.Core.X509;
using NIdentity.Core.X509.Commands.Certificates;

namespace NIdentity.Connector.AspNetCore.Identities.X509
{
    /// <summary>
    /// X509 requester identity.
    /// </summary>
    public class X509RequesterIdentity : RequesterIdentity
    {
        /// <summary>
        /// Initialize a new <see cref="X509RequesterIdentity"/> instance.
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="IssuerKeyIdentifier"></param>
        public X509RequesterIdentity(Certificate Recognized)
        {
            if (Recognized is null)
                throw new ArgumentNullException(nameof(Recognized));

            this.Recognized = Recognized;
        }

        /// <summary>
        /// Identity type.
        /// </summary>
        public sealed override RequesterIdentityKind Kind => RequesterIdentityKind.Certificate;

        /// <summary>
        /// Recognized Certificate.
        /// </summary>
        public Certificate Recognized { get; }

        /// <summary>
        /// Metadata of certificate that received from X509 endpoint.
        /// </summary>
        public X509GetCertificateMetaCommand.Result Metadata { get; internal set; }

        /// <summary>
        /// Serial Number.
        /// </summary>
        public string SerialNumber => Recognized.SerialNumber;

        /// <summary>
        /// Issuer's Key Identifier.
        /// </summary>
        public string IssuerKeyIdentifier => Recognized.Issuer.KeyIdentifier;

        /// <inheritdoc/>
        protected override string Serialize()
        {
            return $"{SerialNumber}@{IssuerKeyIdentifier}";
        }
    }
}
