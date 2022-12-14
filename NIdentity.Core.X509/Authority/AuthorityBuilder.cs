namespace NIdentity.Core.X509.Authority
{
    /// <summary>
    /// Builds issuer (authority) information.
    /// </summary>
    public class AuthorityBuilder
    {
        private readonly List<AuthorityAccess> m_AccessPoints = new();

        /// <summary>
        /// Access Points.
        /// </summary>
        public IReadOnlyCollection<AuthorityAccess> AccessPoints => m_AccessPoints;

        /// <summary>
        /// Authority Certificate, this should have private key.
        /// if want self-signed, leave this as null.
        /// but if, <see cref="CertificateType"/> is not <see cref="CertificateType.Root"/>, the builder will fail.
        /// </summary>
        public Certificate Certificate { get; set; }

        /// <summary>
        /// Set the certificate from <see cref="CertificateStore"/> with name.
        /// </summary>
        /// <param name="From"></param>
        /// <param name="KeyIdentifier"></param>
        /// <returns></returns>
        public AuthorityBuilder SetCertificate(CertificateStore From, string KeyIdentifier)
        {
            var Certificate = From.GetByKeyIdentifier(KeyIdentifier);
            if (Certificate is null)
                throw new KeyNotFoundException("No issuer key exists on the certificate store.");

            this.Certificate = Certificate;
            return this;
        }

        /// <summary>
        /// Add an Ocsp server uri as authority access information.
        /// </summary>
        /// <param name="Uri"></param>
        /// <returns></returns>
        public AuthorityBuilder AddOcspServerUri(Uri Uri)
        {
            var Access = m_AccessPoints
                .Where(X => X.Type == AuthorityAccessType.OcspServerUri)
                .FirstOrDefault(X => X.AccessUri == Uri);

            if (Access != null)
                throw new InvalidOperationException("No duplicated Ocsp server uri added.");

            m_AccessPoints.Add(new AuthorityAccess(
                AuthorityAccessType.OcspServerUri, Uri));

            return this;
        }

        /// <summary>
        /// Add an CRL distribution point uri as authority access information.
        /// </summary>
        /// <param name="Uri"></param>
        /// <returns></returns>
        public AuthorityBuilder AddCrlDistributionPoint(Uri Uri)
        {
            var Access = m_AccessPoints
                .Where(X => X.Type == AuthorityAccessType.CrlDistributionPointUri)
                .FirstOrDefault(X => X.AccessUri == Uri);

            if (Access != null)
                throw new InvalidOperationException("No duplicated CRL distribution point uri added.");

            m_AccessPoints.Add(new AuthorityAccess(
                AuthorityAccessType.CrlDistributionPointUri, Uri));

            return this;
        }

        /// <summary>
        /// Add an Authority certificate uri as authority access information.
        /// </summary>
        /// <param name="Uri"></param>
        /// <returns></returns>
        public AuthorityBuilder AddAuthorityCertificateUri(Uri Uri)
        {
            var Access = m_AccessPoints
                .Where(X => X.Type == AuthorityAccessType.AuthorityCertificateUri)
                .FirstOrDefault(X => X.AccessUri == Uri);

            if (Access != null)
                throw new InvalidOperationException("No duplicated Authority certificate uri added.");

            m_AccessPoints.Add(new AuthorityAccess(
                AuthorityAccessType.AuthorityCertificateUri, Uri));

            return this;
        }

        /// <summary>
        /// Remove an access point.
        /// </summary>
        /// <param name="AccessPoint"></param>
        /// <returns></returns>
        public AuthorityBuilder RemoveAccessPoint(AuthorityAccess AccessPoint)
        {
            m_AccessPoints.Remove(AccessPoint);
            return this;
        }

        /// <summary>
        /// Clone the <see cref="AuthorityBuilder"/> instance.
        /// </summary>
        /// <returns></returns>
        internal AuthorityBuilder Clone()
        {
            var New = new AuthorityBuilder();

            New.Certificate = Certificate;
            New.m_AccessPoints.AddRange(m_AccessPoints);

            return New;
        }
    }
}
