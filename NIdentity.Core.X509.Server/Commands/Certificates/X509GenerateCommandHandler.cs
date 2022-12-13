using NIdentity.Core.Commands;
using NIdentity.Core.X509.Algorithms;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Server.Commands.Bases;

namespace NIdentity.Core.X509.Server.Commands.Certificates
{
    [CommandHandler(typeof(X509GenerateCommand), Kind = "x509")]
    public partial class X509GenerateCommandHandler : X509CertificateCommandHandler<X509GenerateCommand>
    {
        private readonly X509ExecutorSettings m_Settings;

        /// <summary>
        /// Initialize a new <see cref="X509GenerateCommandHandler"/> instance.
        /// </summary>
        /// <param name="Settings"></param>
        public X509GenerateCommandHandler(
            X509RequesterAccesor Requester,
            X509ExecutorSettings Settings) : base(Requester)
            => m_Settings = Settings;

        /// <summary>
        /// Execute the generation command.
        /// </summary>
        /// <param name="Context"></param>
        /// <returns></returns>
        public override async Task<CommandResult> ExecuteAsync(X509CommandContext Context)
        {
            var Request = Context.Command;
            var Builder = new Certificate.Builder();

            if (!IsSuperAccess)
            {
                if (Requester is null)
                    throw new AccessViolationException("no `generate` permission granted for unauthorized accesses.");
            }

            await AssertDuplication(Context, Request);

            SetSubjectBuilder(Request, Builder);
            await SetAuthorityBuilder(Request, Builder, Context.Repository, Context.CommandAborted);
            SetExpiration(Request, Builder);

            var Certificate = Builder.Build();

            var ExcludePrivateKey
                = Certificate.Type == CertificateType.Leaf
                && m_Settings.ExcludeLeafPrivateKeys;

            await AssertDuplication(Context, Request);
            if (!await Context.MutableRepository.StoreAsync(Certificate, ExcludePrivateKey, Context.CommandAborted))
                throw new InvalidOperationException("failed to store the certificate to repository.");

            // --> reload certificate to get auto-generated metadatas.
            Certificate = await Context.Repository.LoadAsync(Certificate, Context.CommandAborted);
            
            var PfxBytes = null as byte[];
            var Chain = await Context.Repository.LoadChainAsync(Certificate, Context.CommandAborted);
            if (Chain != null && Chain.Length > 0)
            {
                var Store = new CertificateStore();

                Store.Add(Certificate);
                foreach (var Each in Chain)
                {
                    if (Each.Self.IsExact(Certificate))
                    {
                        Store.Add(Each);
                        continue;
                    }

                    
                    Store.Add(Certificate.ImportPfx(Each.ExportPfx(true)));
                }

                PfxBytes = Store.Export();
            }

            if (PfxBytes is null)
                PfxBytes = Certificate.ExportPfx();

            return X509GenerateCommand.Result.Make(Certificate,
                X => X.PfxBase64 = Convert.ToBase64String(PfxBytes));
        }

        /// <summary>
        /// Assert duplication.
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static async Task AssertDuplication(X509CommandContext Context, X509GenerateCommand Request)
        {
            if (Request.SubjectReference.Validity)
            {
                var Dup = await Context.Repository.LoadAsync(Request.SubjectReference, Context.CommandAborted);
                if (Dup != null)
                {
                    throw new InvalidOperationException(
                        "the specified `Serial Number` and `Issuer Key Identifier` pair is already exist on repository");
                }
            }
        }

        /// <summary>
        /// Set the subject builder from request.
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Builder"></param>
        /// <exception cref="ArgumentException"></exception>
        private void SetSubjectBuilder(X509GenerateCommand Request, Certificate.Builder Builder)
        {
            var Subject = Builder.Subject;

            // --> basic informations.
            Subject.Type = Request.KeyType;
            Subject.Subject = Request.Subject;
            Subject.Purposes = Request.Purposes;
            Subject.SerialNumber = Request.SerialNumber;


            if (Request.DnsNames != null)
            {
                foreach (var Each in Request.DnsNames)
                    Subject.DnsNames.Add(Each);
            }

            // --> set the key algorithm.
            if (string.IsNullOrWhiteSpace(Request.Algorithm))
                Request.Algorithm = m_Settings.DefaultKeyAlgorithm;

            if (Algorithm.EcdsaCurveNames.Contains(Request.Algorithm))
                Subject.Algorithm = Algorithm.MakeEcdsa(Request.Algorithm);

            else if (Request.Algorithm.StartsWith("rsa"))
            {
                var Back = Request.Algorithm.Substring(3).Trim('-', '_', ' ');
                if (!int.TryParse(Back, out var KeyLength))
                    KeyLength = Algorithm.RsaKeyLengths.First();

                Subject.Algorithm = Algorithm.MakeRsa(KeyLength);
            }

            if (Subject.Algorithm is null)
                throw new ArgumentException($"Key algorithm, {Request.Algorithm} is not supported.");
        }

        /// <summary>
        /// Set the authority builder from request.
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Builder"></param>
        /// <param name="Repository"></param>
        /// <param name="HttpBaseUri"></param>
        /// <param name="Aborter"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task SetAuthorityBuilder(
            X509GenerateCommand Request, Certificate.Builder Builder,
            ICertificateRepository Repository, CancellationToken Aborter)
        {
            var Issuer = Builder.Issuer;

            // -- Builder.Issuer
            if (Request.KeyType != CertificateType.Root)
            {
                var Cert = await Repository.LoadAsync(Request.IssuerIdentity, Aborter);
                if (Cert is null || Cert.HasPrivateKey == false)
                    throw new ArgumentException("the specified issuer is missing or no private key exists.");

                if (Cert.Type == CertificateType.Leaf)
                    throw new InvalidOperationException("leaf certificate can not be issuer.");

                Issuer.Certificate = Cert;
            }

            await CheckPermissionsAsync(Request, Builder, Repository, Aborter);

            if (m_Settings.HttpBaseUri != null && (Request.WithOcsp || Request.WithCrlDists || Request.WithCAIssuers))
            {
                if (Request.WithOcsp && !string.IsNullOrWhiteSpace(m_Settings.HttpOcsp))
                {
                    if (m_Settings.HttpOcsp.StartsWith("http://") ||
                        m_Settings.HttpOcsp.StartsWith("https://"))
                    {
                        Issuer.AddOcspServerUri(
                            new Uri(m_Settings.HttpOcsp.TrimEnd('/')));
                    }

                    else
                    {
                        Issuer.AddOcspServerUri(
                            new Uri(m_Settings.HttpBaseUri.ToString().TrimEnd('/')
                            + $"/{m_Settings.HttpOcsp.Trim('/')}"));
                    }
                }

                if (Request.KeyType != CertificateType.Root)
                {
                    var IssuerKeySHA256 = Issuer.Certificate.Self.MakeKeySHA1();
                    if (Request.WithCrlDists && !string.IsNullOrWhiteSpace(m_Settings.HttpCRL))
                    {
                        if (m_Settings.HttpCRL.StartsWith("http://") ||
                            m_Settings.HttpCRL.StartsWith("https://"))
                        {
                            Issuer.AddCrlDistributionPoint(
                                new Uri($"{m_Settings.HttpCRL.TrimEnd('/')}/{IssuerKeySHA256}.crl"));
                        }
                        else
                        {
                            Issuer.AddCrlDistributionPoint(
                                new Uri(m_Settings.HttpBaseUri.ToString().TrimEnd('/')
                                + $"/{m_Settings.HttpCRL.Trim('/')}/{IssuerKeySHA256}.crl"));
                        }
                    }

                    if (Request.WithCAIssuers && !string.IsNullOrWhiteSpace(m_Settings.HttpCAIssuers))
                    {
                        if (m_Settings.HttpCAIssuers.StartsWith("http://") ||
                            m_Settings.HttpCAIssuers.StartsWith("https://"))
                        {
                            Issuer.AddAuthorityCertificateUri(
                                new Uri($"{m_Settings.HttpCAIssuers.TrimEnd('/')}/{IssuerKeySHA256}.cer"));
                        }
                        else
                        {
                            Issuer.AddAuthorityCertificateUri(
                                new Uri(m_Settings.HttpBaseUri.ToString().TrimEnd('/')
                                + $"/{m_Settings.HttpCAIssuers.Trim('/')}/{IssuerKeySHA256}.cer"));
                        }
                    }
                }
            }

            if (Request.AdditionalOcspServers != null)
            {
                foreach (var Each in Request.AdditionalOcspServers)
                    Issuer.AddOcspServerUri(new Uri(Each));
            }

            if (Request.AdditionalCrlDists != null)
            {
                foreach (var Each in Request.AdditionalCrlDists)
                    Issuer.AddCrlDistributionPoint(new Uri(Each));
            }

            if (Request.AdditionalCAIssuers != null)
            {
                foreach (var Each in Request.AdditionalCAIssuers)
                    Issuer.AddAuthorityCertificateUri(new Uri(Each));
            }
        }

        /// <summary>
        /// Check the permission and throw <see cref="AccessViolationException"/> if no permission.
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Repository"></param>
        /// <param name="Issuer"></param>
        /// <param name="Aborter"></param>
        /// <returns></returns>
        /// <exception cref="AccessViolationException"></exception>
        private async Task CheckPermissionsAsync(X509GenerateCommand Request, Certificate.Builder Builder, ICertificateRepository Repository, CancellationToken Aborter)
        {
            var Issuer = Builder.Issuer;
            if (Request.KeyType != CertificateType.Root)
            {
                if (!IsSuperAccess && !await Repository.IsIssuerAsync(Requester, Issuer.Certificate, Aborter))
                    throw new AccessViolationException("no permission to sign using the issuer certificate.");
            }

            else
            {
                if (m_Settings.DisallowGenerateRootCA)
                    throw new AccessViolationException("Root CA certificate generation is disallowed by X509 command settings.");

                else if (!IsSuperAccess)
                {
                    if (!Requester.IsSelfSigned)
                    {
                        if (m_Settings.SystemCertificate.HasValue == true)
                            throw new AccessViolationException("Root CA certificate generation is disallowed by X509 command settings.");

                        throw new AccessViolationException("no permission to generate Root CA certificate.");
                    }

                    else if (m_Settings.SystemCertificate.HasValue == true
                          && m_Settings.SystemCertificate.Value != Requester.Self)
                    {
                        throw new AccessViolationException("no permission to generate Root CA certificate.");
                    }
                }
            }
        }

        /// <summary>
        /// Set the expiration.
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="Builder"></param>
        /// <exception cref="ArgumentException"></exception>
        private void SetExpiration(X509GenerateCommand Request, Certificate.Builder Builder)
        {
            if (Request.ExpirationHours <= 0)
                throw new ArgumentException("expiration of certificate can not be zero or negative number.");

            var ExpirationHours = Math.Min(Request.ExpirationHours, m_Settings.MaximumExpirationInHours);

            Builder.SetCreationTime(DateTimeOffset.UtcNow);
            Builder.SetExpirationTime(TimeSpan.FromHours(ExpirationHours));
        }
    }
}
