using NIdentity.Core.Commands.Internals;
using NIdentity.Core.X509;
using NIdentity.Core.X509.Commands.Certificates;

namespace NIdentity.Connector.X509
{
    /// <summary>
    /// Certificate related command helpers.
    /// </summary>
    public static class X509CertificateHelpers
    {
        /// <summary>
        /// Generate a certificate.
        /// </summary>
        /// <returns></returns>
        public static async Task<Certificate> GenerateCertificateAsync(
            this X509CommandExecutor Executor,
            CertificateType KeyType, TimeSpan Expiration, string Subject,
            CertificateIdentity Issuer = default, string[] DnsNames = null,
            CertificatePurposes Purposes = CertificatePurposes.Networking | CertificatePurposes.Protection,
            string Algorithm = "rsa-2048", CancellationToken Token = default)
        {
            var Cmd = new X509GenerateCommand
            {
                KeyType = Issuer.Validity == false
                    ? CertificateType.Root : KeyType,
                Algorithm = Algorithm,
                Purposes = Purposes,
                ExpirationHours = Expiration.TotalHours,
                Subject = Subject,
                DnsNames = DnsNames,
                Issuer = Issuer.Validity == false
                    ? Issuer.Subject : null,
                IssuerKeyIdentifier = Issuer.Validity == false
                    ? Issuer.KeyIdentifier : null,
                WithOcsp = true,
                WithCrlDists = true,
                WithCAIssuers = true
            };

            var Raw = await Executor.GenerateCertificateAsync(Cmd, Token);
            if (Raw.Success == false)
                throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

            if (Raw is X509GenerateCommand.Result Result && Result.Success)
            {
                var RetVal = Certificate.ImportPfx(Convert.FromBase64String(Result.PfxBase64));
                if (RetVal != null)
                    await Executor.CacheRepository.SetAsync(RetVal, Token);

                return RetVal;
            }

            return null;
        }

        /// <summary>
        /// Get the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<Certificate> GetCertificateAsync(
            this X509CommandExecutor Executor, CertificateIdentity Identity, CancellationToken Token = default)
        {
            var Cache = await Executor.CacheRepository.GetAsync(Identity, Token);
            if (Cache is null) {

                var Cmd = new X509GetCertificateCommand
                {
                    Subject = Identity.Subject,
                    KeyIdentifier = Identity.KeyIdentifier
                };

                var Raw = await Executor.GetCertificateAsync(Cmd, Token);
                if (Raw.Success == false)
                    throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

                if (Raw is X509GetCertificateCommand.Result Result && Result.Success)
                    Cache = await LoadAsync(Executor, Result, Token);
            }

            return Cache;
        }

        /// <summary>
        /// Get the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<Certificate> GetCertificateAsync(
            this X509CommandExecutor Executor, CertificateReference Reference, CancellationToken Token = default)
        {
            var Cache = await Executor.CacheRepository.GetAsync(Reference, Token);
            if (Cache is null)
            {
                var Cmd = new X509GetCertificateCommand
                {
                    SerialNumber = Reference.SerialNumber,
                    IssuerKeyIdentifier = Reference.IssuerKeyIdentifier
                };

                var Raw = await Executor.GetCertificateAsync(Cmd, Token);
                if (Raw.Success == false)
                    throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

                if (Raw is X509GetCertificateCommand.Result Result && Result.Success)
                    Cache = await LoadAsync(Executor, Result, Token);
            }

            return Cache;
        }

        /// <summary>
        /// Get the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<X509GetCertificateMetaCommand.Result> GetCertificateMetaAsync(
            this X509CommandExecutor Executor, CertificateReference Reference, CancellationToken Token = default)
        {
            var Cmd = new X509GetCertificateMetaCommand
            {
                SerialNumber = Reference.SerialNumber,
                IssuerKeyIdentifier = Reference.IssuerKeyIdentifier
            };

            var Raw = await Executor.GetCertificateMetaAsync(Cmd, Token);
            if (Raw.Success == false)
                throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

            return Raw as X509GetCertificateMetaCommand.Result;
        }

        /// <summary>
        /// Load certificate from result asynchronously.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Result"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        private static async Task<Certificate> LoadAsync(this X509CommandExecutor Executor, X509GetCertificateCommand.Result Result, CancellationToken Token)
        {
            var Cache = Certificate.Import(Convert.FromBase64String(Result.CerBase64));

            Cache.RevokeReason = null;
            Cache.RevokeTime = null;
            if (Result.IsRevoked)
            {
                Cache.RevokeReason = Result.RevokeReason;
                Cache.RevokeTime = Result.RevokeTime;
            }

            await Executor.CacheRepository.SetAsync(Cache, Token);
            return Cache;
        }

        /// <summary>
        /// List all certificates asynchronously.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Authority"></param>
        /// <param name="Offset"></param>
        /// <param name="Count"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<X509CertificateCollection> ListCertificatesAsync(
            this X509CommandExecutor Executor, CertificateIdentity Authority, int Offset, int Count = 20, CancellationToken Token = default)
        {
            var Cmd = new X509ListCertificateCommand
            {
                Subject = Authority.Subject,
                KeyIdentifier = Authority.KeyIdentifier,
                Offset = Offset,
                Count = Count
            };

            var Raw = await Executor.ListCertificatesAsync(Cmd, Token);
            if (Raw.Success == false)
                throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

            if (Raw is X509ListCertificateCommand.Result Result && Result.Success)
                return await LoadAllAsync(Executor, Result, Token);

            return new X509CertificateCollection(new Certificate[0], 0);
        }

        /// <summary>
        /// List all certificates asynchronously.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Authority"></param>
        /// <param name="Offset"></param>
        /// <param name="Count"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<X509CertificateCollection> ListCertificatesAsync(
            this X509CommandExecutor Executor, CertificateReference Authority, int Offset, int Count = 20, CancellationToken Token = default)
        {
            var Cmd = new X509ListCertificateCommand
            {
                SerialNumber = Authority.SerialNumber,
                IssuerKeyIdentifier = Authority.IssuerKeyIdentifier,
                Offset = Offset,
                Count = Count
            };

            var Raw = await Executor.ListCertificatesAsync(Cmd, Token);
            if (Raw.Success == false)
                throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

            if (Raw is X509ListCertificateCommand.Result Result && Result.Success)
                return await LoadAllAsync(Executor, Result, Token);

            return new X509CertificateCollection(new Certificate[0], 0);
        }

        /// <summary>
        /// Load all certificates from list.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Result"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static async Task<X509CertificateCollection> LoadAllAsync(this X509CommandExecutor Executor, X509ListCertificateCommand.Result Result, CancellationToken Token)
        {
            var List = new List<Certificate>();
            var Bulk = Result.Items.Select(Each => new X509GetCertificateCommand
            {
                Subject = Each.Subject,
                KeyIdentifier = Each.KeyIdentifier
            });

            var BulkRaw = await Executor.BulkAsync(Bulk, Token);
            if (BulkRaw.Success == false || BulkRaw is not BulkCommand.Result BulkResult)
                throw new Exception($"{BulkRaw.ReasonKind}: {BulkRaw.Reason}");

            var All = BulkResult.Results
                .Select(X => X as X509GetCertificateCommand.Result)
                .Where(X => X != null);

            foreach (var Each in All)
                List.Add(await Executor.LoadAsync(Each, Token));

            return new X509CertificateCollection(List, Result.TotalItems);
        }

        /// <summary>
        /// Revoke the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> RevokeCertificateAsync(
            this X509CommandExecutor Executor, CertificateIdentity Identity, CertificateRevokeReason Reason, CancellationToken Token = default)
        {
            var Cert = await Executor.GetCertificateAsync(Identity, Token);
            if (Cert != null) {

                var Cmd = new X509RevokeCertificateCommand
                {
                    Subject = Identity.Subject,
                    KeyIdentifier = Identity.KeyIdentifier,
                    RevokeReason = Reason
                };

                var Raw = await Executor.RevokeCertificateAsync(Cmd, Token);
                if (Raw.Success == false)
                    throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

                if (Raw is X509RevokeCertificateCommand.Result Result && Result.Success)
                {
                    await Executor.CacheRepository.SetAsync(Cert, X =>
                    {
                        X.RevokeReason = Result.RevokeReason;
                        X.RevokeTime = Result.RevokeTime;
                    });

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Revoke the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> RevokeCertificateAsync(
            this X509CommandExecutor Executor, CertificateReference Reference, CertificateRevokeReason Reason, CancellationToken Token = default)
        {
            var Cert = await Executor.GetCertificateAsync(Reference, Token);
            if (Cert != null)
            {
                var Cmd = new X509RevokeCertificateCommand
                {
                    SerialNumber = Reference.SerialNumber,
                    IssuerKeyIdentifier = Reference.IssuerKeyIdentifier,
                    RevokeReason = Reason
                };

                var Raw = await Executor.RevokeCertificateAsync(Cmd, Token);
                if (Raw.Success == false)
                    throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

                if (Raw is X509RevokeCertificateCommand.Result Result && Result.Success)
                {
                    await Executor.CacheRepository.SetAsync(Cert, X =>
                    {
                        X.RevokeReason = Result.RevokeReason;
                        X.RevokeTime = Result.RevokeTime;
                    });

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Unrevoke the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> UnrevokeCertificateAsync(
            this X509CommandExecutor Executor, CertificateIdentity Identity, CancellationToken Token = default)
        {
            var Cert = await Executor.GetCertificateAsync(Identity, Token);
            if (Cert != null)
            {

                var Cmd = new X509UnrevokeCertificateCommand
                {
                    Subject = Identity.Subject,
                    KeyIdentifier = Identity.KeyIdentifier
                };

                var Raw = await Executor.UnrevokeCertificateAsync(Cmd, Token);
                if (Raw.Success == false)
                    throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

                if (Raw is X509UnrevokeCertificateCommand.Result Result && Result.Success)
                {
                    await Executor.CacheRepository.SetAsync(Cert, X =>
                    {
                        X.RevokeReason = Result.RevokeReason;
                        X.RevokeTime = Result.RevokeTime;
                    });

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Unrevoke the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> UnrevokeCertificateAsync(
            this X509CommandExecutor Executor, CertificateReference Reference, CancellationToken Token = default)
        {
            var Cert = await Executor.GetCertificateAsync(Reference, Token);
            if (Cert != null)
            {
                var Cmd = new X509UnrevokeCertificateCommand
                {
                    SerialNumber = Reference.SerialNumber,
                    IssuerKeyIdentifier = Reference.IssuerKeyIdentifier
                };

                var Raw = await Executor.UnrevokeCertificateAsync(Cmd, Token);
                if (Raw.Success == false)
                    throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

                if (Raw is X509UnrevokeCertificateCommand.Result Result && Result.Success)
                {
                    await Executor.CacheRepository.SetAsync(Cert, X =>
                    {
                        X.RevokeReason = Result.RevokeReason;
                        X.RevokeTime = Result.RevokeTime;
                    });

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Unrevoke the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> DeleteCertificateAsync(
            this X509CommandExecutor Executor, CertificateIdentity Identity, CancellationToken Token = default)
        {
            var Cert = await Executor.GetCertificateAsync(Identity, Token);
            if (Cert != null)
            {

                var Cmd = new X509DeleteCertificateCommand
                {
                    Subject = Identity.Subject,
                    KeyIdentifier = Identity.KeyIdentifier
                };

                var Raw = await Executor.DeleteCertificateAsync(Cmd, Token);
                if (Raw.Success == false)
                    throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

                if (Raw is X509DeleteCertificateCommand.Result Result && Result.Success)
                {
                    await Executor.CacheRepository.DeleteAsync(Cert);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Unrevoke the certificate asynchronously.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> DeleteCertificateAsync(
            this X509CommandExecutor Executor, CertificateReference Reference, CancellationToken Token = default)
        {
            var Cert = await Executor.GetCertificateAsync(Reference, Token);
            if (Cert != null)
            {
                var Cmd = new X509DeleteCertificateCommand
                {
                    SerialNumber = Reference.SerialNumber,
                    IssuerKeyIdentifier = Reference.IssuerKeyIdentifier
                };

                var Raw = await Executor.DeleteCertificateAsync(Cmd, Token);
                if (Raw.Success == false)
                    throw new Exception($"{Raw.ReasonKind}: {Raw.Reason}");

                if (Raw is X509DeleteCertificateCommand.Result Result && Result.Success)
                {
                    await Executor.CacheRepository.DeleteAsync(Cert);
                    return true;
                }
            }

            return false;
        }
    }
}
