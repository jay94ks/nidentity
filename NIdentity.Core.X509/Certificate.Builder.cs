using NIdentity.Core.X509.Algorithms;
using NIdentity.Core.X509.Authority;
using NIdentity.Core.X509.Helpers;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Extension;
using System.Net;

namespace NIdentity.Core.X509
{
    /// <summary>
    /// X509 certificate wrapper.
    /// </summary>
    public partial class Certificate
    {
        /// <summary>
        /// Certificate Builder.
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Authority parameters.
            /// </summary>
            public class AuthorityParameters
            {
                /// <summary>
                /// Subject.
                /// </summary>
                public string Subject { get; set; }

                /// <summary>
                /// Serial Number.
                /// </summary>
                public string SerialNumber { get; set; }

                /// <summary>
                /// Key Identifier.
                /// </summary>
                public string KeyIdentifier { get; set; }

                /// <summary>
                /// Issuer Identity.
                /// </summary>
                public CertificateIdentity Issuer { get; set; }
            }

            private readonly List<Action<AuthorityBuilder, AuthorityParameters>> m_Authorities = new();
            private readonly List<Action<X509V3CertificateGenerator>> m_PostConfigures = new();

            /// <summary>
            /// Certificate creation time.
            /// </summary>
            public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.UtcNow;

            /// <summary>
            /// Certificate expiration time.
            /// </summary>
            public TimeSpan ExpirationTime { get; set; } = TimeSpan.FromDays(365);

            /// <summary>
            /// Subject builder.
            /// </summary>
            public SubjectBuilder Subject { get; } = new();

            /// <summary>
            /// Issuer, aka, Authority Builder.
            /// </summary>
            public AuthorityBuilder Issuer { get; } = new();

            /// <summary>
            /// Set the creation time.
            /// </summary>
            /// <param name="CreationTime"></param>
            /// <returns></returns>
            public Builder SetCreationTime(DateTimeOffset CreationTime)
            {
                this.CreationTime = CreationTime;
                return this;
            }

            /// <summary>
            /// Set the expiration time.
            /// </summary>
            /// <param name="ExpirationTime"></param>
            /// <returns></returns>
            public Builder SetExpirationTime(TimeSpan ExpirationTime)
            {
                this.ExpirationTime = ExpirationTime;
                return this;
            }

            /// <summary>
            /// Add post authority builder delegate.
            /// </summary>
            /// <param name="Delegate"></param>
            /// <returns></returns>
            public Builder PostAuthorityBuilder(Action<AuthorityBuilder, AuthorityParameters> Delegate)
            {
                m_Authorities.Add(Delegate);
                return this;
            }

            /// <summary>
            /// Add post configure delegates.
            /// </summary>
            /// <param name="PostConfigure"></param>
            /// <returns></returns>
            public Builder PostConfigure(Action<X509V3CertificateGenerator> PostConfigure)
            {
                m_PostConfigures.Add(PostConfigure);
                return this;
            }

            /// <summary>
            /// Build a certificate.
            /// </summary>
            /// <returns></returns>
            public Certificate Build()
            {
                if (string.IsNullOrWhiteSpace(Subject.Subject))
                    throw new InvalidOperationException("the subject can not be null or empty.");

                var FixedSerialNumber = null as BigInteger;
                if (!string.IsNullOrWhiteSpace(Subject.SerialNumber))
                {
                    try { FixedSerialNumber = new BigInteger(Subject.SerialNumber, 16); }
                    catch
                    {
                        throw new InvalidOperationException("the serial number should be byte hex codes.");
                    }
                }

                if (Subject.Algorithm is null)
                    Subject.Algorithm = Algorithm.MakeEcdsa(Algorithm.EcdsaCurveNames.Last());

                if (Subject.Type != CertificateType.Root)
                {
                    if (Issuer.Certificate is null)
                        throw new InvalidOperationException("Non-root certificate requires issuer certificate.");

                    if (Issuer.Certificate.HasPrivateKey == false)
                        throw new InvalidOperationException("To sign the generated certificate, issuer's private key is required.");
                }

                var Generator = new X509V3CertificateGenerator();
                var KeyPair = Subject.Algorithm.GenerateKeyPair();

                // --> Set certificate properties.
                var Info = SetBasicProperties(Generator, FixedSerialNumber, KeyPair);
                SetSubjectAlternativeNames(Generator);

                // --> Clone the authority builder to take dynamic authority access points.
                var Authority = Issuer.Clone();
                InvokePostAuthorityDelegates(Authority, Info.SN, Info.Ski, Info.Issuer, Info.Aki);
                SetAuthorityAccesPoints(Generator, Authority);

                // --> then, set certificate policies.
                SetCertificatePolicies(Generator);

                // --> Invoke post configure delegates.
                InvokePostConfigureDelegates(Generator);

                var IssuerKey = Issuer.Certificate is null
                    ? KeyPair.Private : Issuer.Certificate.PrivateKey;

                var Signer = IssuerKey.CreateSignatureFactory(Subject.HashAlgorithm);
                if (Signer is null)
                {
                    throw new InvalidOperationException(
                        "the specified algorithm is not supported, " +
                        "or currently disabled due to bug fixes.");
                }

                // --> finally, generate the certificate.
                var Certificate = Generator.Generate(Signer);
                return new Certificate(Certificate, KeyPair.Private);
            }

            /// <summary>
            /// Set the certificate's basic informations.
            /// </summary>
            /// <param name="Generator"></param>
            /// <param name="SerialNumber"></param>
            /// <param name="KeyPair"></param>
            private (BigInteger SN, string Ski, string Issuer, string Aki) SetBasicProperties(
                X509V3CertificateGenerator Generator, BigInteger SerialNumber, AsymmetricCipherKeyPair KeyPair)
            {
                var Issuer = this.Issuer.Certificate != null
                    ? this.Issuer.Certificate.Subject : Subject.Subject;

                Generator.SetSubjectDN(new X509Name(Subject.Subject));
                Generator.SetIssuerDN(new X509Name(Issuer));


                if (SerialNumber is null)
                    SerialNumber = BigInteger.ProbablePrime(160, new SecureRandom());

                Generator.SetSerialNumber(SerialNumber);
                Generator.SetNotAfter(CreationTime.Add(ExpirationTime).UtcDateTime);
                Generator.SetNotBefore(CreationTime.UtcDateTime);
                Generator.SetPublicKey(KeyPair.Public);

                // --> Subject Key Identifier.
                var Ski = new SubjectKeyIdentifierStructure(KeyPair.Public);
                Generator.AddExtension(X509Extensions.SubjectKeyIdentifier, false, Ski);

                // --> Key Identifiers.
                var SkiString = string.Join("", Ski.GetKeyIdentifier().Select(X => X.ToString("x2")));
                var AkiString = SkiString;
                // --> Authority Key Identifier.
                if (this.Issuer.Certificate != null)
                {
                    var Aki = new AuthorityKeyIdentifierStructure(this.Issuer.Certificate.PublicKey);
                    Generator.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, Aki);
                    AkiString = this.Issuer.Certificate.KeyIdentifier; // string.Join("", Aki.GetKeyIdentifier().Select(X => X.ToString("x2")));
                }

                // --> Self Signed Authority Key Identifier.
                else
                {
                    var Aki = new AuthorityKeyIdentifierStructure(KeyPair.Public);
                    Generator.AddExtension(X509Extensions.AuthorityKeyIdentifier, false, Aki);
                }

                // --> Set the ceritificate is for authority or not.
                Generator.AddExtension(X509Extensions.BasicConstraints, false,
                    new BasicConstraints(Subject.Type != CertificateType.Leaf));

                // --> Set the key usage for covering subject type's role.
                switch (Subject.Type)
                {
                    case CertificateType.Root:
                    case CertificateType.Immediate:
                        Generator.AddExtension(X509Extensions.KeyUsage, false, new KeyUsage(
                            KeyUsage.DigitalSignature | KeyUsage.KeyCertSign | KeyUsage.CrlSign));

                        Subject
                            .AddPurposes(CertificatePurposes.Stamping)
                            .AddPurposes(CertificatePurposes.Networking)
                            ;
                        break;

                    case CertificateType.Leaf:
                        Generator.AddExtension(X509Extensions.KeyUsage, false, new KeyUsage(
                            KeyUsage.DigitalSignature | KeyUsage.KeyEncipherment));

                        Subject.AddPurposes(CertificatePurposes.Networking);
                        break;
                }

                Generator.AddExtension(X509Extensions.ExtendedKeyUsage, false, new ExtendedKeyUsage(MakePurposeIds()));
                return (SN: SerialNumber, Ski: SkiString, Issuer: Issuer, Aki: AkiString);
            }

            /// <summary>
            /// Make purpose id array.
            /// </summary>
            /// <returns></returns>
            private KeyPurposeID[] MakePurposeIds()
            {
                var PurposeIds = new List<KeyPurposeID>();
                if (Subject.HasPurposes(CertificatePurposes.Networking))
                {
                    PurposeIds.Add(KeyPurposeID.IdKPServerAuth);
                    PurposeIds.Add(KeyPurposeID.IdKPClientAuth);
                }

                if (Subject.HasPurposes(CertificatePurposes.Protection))
                {
                    PurposeIds.Add(KeyPurposeID.IdKPEmailProtection);
                    PurposeIds.Add(KeyPurposeID.IdKPCodeSigning);
                }

                if (Subject.HasPurposes(CertificatePurposes.Stamping))
                {
                    PurposeIds.Add(KeyPurposeID.IdKPTimeStamping);
                    PurposeIds.Add(KeyPurposeID.IdKPOcspSigning);
                }

                if (Subject.HasPurposes(CertificatePurposes.IPSecEndSystem))
                    PurposeIds.Add(KeyPurposeID.IdKPIpsecEndSystem);

                if (Subject.HasPurposes(CertificatePurposes.IPSecTunnel))
                    PurposeIds.Add(KeyPurposeID.IdKPIpsecTunnel);

                if (Subject.HasPurposes(CertificatePurposes.IPSecUser))
                    PurposeIds.Add(KeyPurposeID.IdKPIpsecUser);

                return PurposeIds.ToArray();
            }

            /// <summary>
            /// Set the subject's alternative names.
            /// </summary>
            /// <param name="Generator"></param>
            private void SetSubjectAlternativeNames(X509V3CertificateGenerator Generator)
            {
                if (Subject.DnsNames.Count > 0)
                {
                    var RawSans = Subject
                        .DnsNames.OrderBy(X => X).Select(X =>
                        {
                            IPAddress.TryParse(X ?? string.Empty, out var Address);
                            return (Name: X, Address);
                        });

                    var DerSans = new DerSequence(RawSans
                        .Where(X => X.Address is null)
                        .Select(N => new GeneralName(GeneralName.DnsName, N.Name))
                        .Concat(RawSans.Where(X => X.Address != null)
                        .Select(N => new GeneralName(GeneralName.IPAddress, N.Address.ToString())))
                        .ToArray());

                    Generator.AddExtension(X509Extensions.SubjectAlternativeName, false, DerSans);
                }
            }

            /// <summary>
            /// Set the certificate policies.
            /// </summary>
            /// <param name="Generator"></param>
            private void SetCertificatePolicies(X509V3CertificateGenerator Generator)
            {
                // --> Set the certificate policies.
                if (Subject.Type == CertificateType.Root)
                {
                    Generator.AddExtension(X509Extensions.CertificatePolicies, false,
                        new CertificatePolicies(
                            new PolicyInformation(PolicyQualifierID.IdQtCps,
                                new DerSequence(new PolicyQualifierInfo("c0"))
                        )));
                }
            }

            /// <summary>
            /// Set the CA's authority access points.
            /// </summary>
            /// <param name="Generator"></param>
            /// <param name="Issuer"></param>
            private void SetAuthorityAccesPoints(X509V3CertificateGenerator Generator, AuthorityBuilder Issuer)
            {
                var OcspServers = Issuer.AccessPoints
                    .Where(X => X.Type == AuthorityAccessType.OcspServerUri)
                    .Select(X => X.AccessUri).ToArray();

                var AuthorityCerticiateUrls = Issuer.AccessPoints
                    .Where(X => X.Type == AuthorityAccessType.AuthorityCertificateUri)
                    .Select(X => X.AccessUri).ToArray();

                var AccessPoints = OcspServers.Select(X => new AccessDescription(AccessDescription.IdADOcsp,
                    new GeneralName(GeneralName.UniformResourceIdentifier, X.ToString()))).Concat(
                    AuthorityCerticiateUrls.Select(X => new AccessDescription(AccessDescription.IdADCAIssuers,
                    new GeneralName(GeneralName.UniformResourceIdentifier, X.ToString())))).ToArray();

                if (AccessPoints.Length > 0)
                {
                    Generator.AddExtension(X509Extensions.AuthorityInfoAccess, false, new AuthorityInformationAccess(AccessPoints));
                }

                var CrlDistributionPoints = Issuer.AccessPoints
                    .Where(X => X.Type == AuthorityAccessType.CrlDistributionPointUri)
                    .Select(X => new GeneralName(GeneralName.UniformResourceIdentifier, X.AccessUri.ToString()))
                    .Select(X => new DistributionPointName(DistributionPointName.FullName, X))
                    .Select(X => new DistributionPoint(X, null, null)).ToArray();

                if (CrlDistributionPoints.Length > 0)
                {
                    Generator.AddExtension(X509Extensions.CrlDistributionPoints, false, new CrlDistPoint(CrlDistributionPoints));
                }
            }

            /// <summary>
            /// Invoke all <see cref="PostConfigure(Action{X509V3CertificateGenerator})"/> delegates.
            /// </summary>
            /// <param name="Builder"></param>
            /// <param name="Issuer"></param>
            /// <param name="Aki"></param>
            /// <param name="Ski"></param>
            /// <param name="SN"></param>
            private void InvokePostAuthorityDelegates(AuthorityBuilder Builder, BigInteger SN, string Ski, string Issuer, string Aki)
            {
                var Temp = m_Authorities.ToArray();
                var Queue = new Queue<Action<AuthorityBuilder, AuthorityParameters>>(m_Authorities);
                var Params = new AuthorityParameters
                {
                    Subject = Subject.Subject,
                    SerialNumber = SN.ToString(16),
                    KeyIdentifier = Ski,
                    Issuer = new CertificateIdentity(Issuer, Aki)
                };

                m_Authorities.Clear();
                try
                {
                    while (true)
                    {
                        while (Queue.TryDequeue(out var Authority))
                            Authority?.Invoke(Builder, Params);

                        if (m_Authorities.Count <= 0)
                            break;

                        foreach (var Each in m_Authorities)
                            Queue.Enqueue(Each);

                        m_Authorities.Clear();
                    }
                }
                finally { m_Authorities.AddRange(Temp); }
            }

            /// <summary>
            /// Invoke all <see cref="PostConfigure(Action{X509V3CertificateGenerator})"/> delegates.
            /// </summary>
            /// <param name="Generator"></param>
            private void InvokePostConfigureDelegates(X509V3CertificateGenerator Generator)
            {
                var Temp = m_PostConfigures.ToArray();
                var Queue = new Queue<Action<X509V3CertificateGenerator>>(m_PostConfigures);

                m_PostConfigures.Clear();
                try
                {
                    while (true)
                    {
                        while (Queue.TryDequeue(out var PostConfigure))
                            PostConfigure?.Invoke(Generator);

                        if (m_PostConfigures.Count <= 0)
                            break;

                        foreach (var Each in m_PostConfigures)
                            Queue.Enqueue(Each);

                        m_PostConfigures.Clear();
                    }
                }
                finally { m_PostConfigures.AddRange(Temp); }
            }
        }
    }
}