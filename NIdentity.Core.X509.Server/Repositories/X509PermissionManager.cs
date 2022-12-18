using NIdentity.Core.Server.Helpers;
using NIdentity.Core.X509.Server.Repositories.Models;

namespace NIdentity.Core.X509.Server.Repositories
{
    /// <summary>
    /// X509 Permission Manager.
    /// </summary>
    public class X509PermissionManager : IMutableCertificatePermissionManager
    {
        private readonly X509Context m_X509Context;

        /// <summary>
        /// Initialize a new <see cref="X509PermissionManager"/> instance.
        /// </summary>
        /// <param name="X509Context"></param>
        public X509PermissionManager(X509Context X509Context)
        {
            m_X509Context = X509Context;
        }

        /// <inheritdoc/>
        public Task<CertificatePermission> QueryAsync(CertificateIdentity Accessor, CertificateIdentity Target, CancellationToken Token = default)
        {
            /**
             * 1. exact { acc, target} pair.
             * 2. default of target.
             * 3. exect of target's authority.
             * 4. default of target's authority.
             * .....
             */

            if (Accessor == Target)
            {
                // --> if exact permission loaded, 
                return Task.FromResult(new CertificatePermission
                {
                    Owner = Target,
                    Accessor = Accessor,
                    CreationTime = DateTimeOffset.UtcNow,
                    LastWriteTime = DateTimeOffset.UtcNow,
                    CanGenerate = true,
                    CanList = true,
                    CanRevoke = true,
                    CanDelete = true
                });
            }

            var Owner = Target;
            while(Target.Validity == true)
            {
                Token.ThrowIfCancellationRequested();

                var KeySHA1 = Target.MakeKeySHA1();
                var AccSHA1 = Accessor.Validity == true ? Accessor.MakeKeySHA1() : string.Empty;

                var Exact = m_X509Context.Permissions
                    .Where(X => X.KeySHA1 == KeySHA1 && X.AccessKeySHA1 == AccSHA1)
                    .FirstOrDefault();

                // --> no actual permission exists, try to load default if available.
                if (Exact is null && string.IsNullOrWhiteSpace(AccSHA1) == false)
                {
                    AccSHA1 = string.Empty;
                    Exact = m_X509Context.Permissions
                        .Where(X => X.KeySHA1 == KeySHA1 && X.AccessKeySHA1 == AccSHA1)
                        .FirstOrDefault();
                }

                // --> no exact permission defined, try to check authority's definitions.
                if (Exact is null)
                {
                    // --> no load if target is self signed.
                    var Upper = m_X509Context.Certificates
                        .Where(X => X.KeySHA1 == KeySHA1).Where(X => X.Subject != X.Issuer)
                        .Where(X => X.KeyIdentifier != X.IssuerKeyIdentifier)
                        .Select(X => new { X.Issuer, X.IssuerKeyIdentifier })
                        .FirstOrDefault();

                    if (Upper is null)
                        break;

                    Target = new CertificateIdentity(Upper.Issuer, Upper.IssuerKeyIdentifier);
                    continue;
                }

                // --> if exact permission loaded, 
                return Task.FromResult(new CertificatePermission
                {
                    Owner = Owner,
                    Accessor = Accessor,
                    CreationTime = Exact.CreationTime,
                    LastWriteTime = Exact.LastWriteTime,
                    CanAuthorityInterfere = Owner == Target && Exact.CanAuthorityInterfere,
                    CanGenerate = Exact.CanGenerate,
                    CanList = Exact.CanList,
                    CanRevoke = Exact.CanRevoke,
                    CanDelete = Exact.CanDelete
                });
            }

            return Task.FromResult<CertificatePermission>(null);
        }

        /// <inheritdoc/>
        public Task<CertificatePermission> GetAsync(CertificateIdentity Accessor, CertificateIdentity Owner, CancellationToken Token = default)
        {
            if (Owner.Validity == false)
                throw new ArgumentException("the owner identity should be valid.");

            var KeySHA1 = Owner.MakeKeySHA1();
            var AccSHA1 = Accessor.Validity == true ? Accessor.MakeKeySHA1() : string.Empty;

            var Exact = m_X509Context.Permissions
                .Where(X => X.KeySHA1 == KeySHA1 && X.AccessKeySHA1 == AccSHA1)
                .FirstOrDefault();

            if (Exact is null)
                return Task.FromResult<CertificatePermission>(null);

            var RetVal = new CertificatePermission
            {
                Owner = Owner,
                Accessor = Accessor,
                CreationTime = Exact.CreationTime,
                LastWriteTime = Exact.LastWriteTime,
                CanAuthorityInterfere = Exact.CanAuthorityInterfere,
                CanGenerate = Exact.CanGenerate,
                CanDelete = Exact.CanDelete,
                CanList = Exact.CanList,
                CanRevoke = Exact.CanRevoke
            };

            return Task.FromResult(RetVal);
        }

        /// <inheritdoc/>
        public Task<CertificatePermission[]> ListAsync(CertificateIdentity Owner, int Offset, int Count = 20, CancellationToken Token = default)
        {
            if (Owner.Validity == false)
                throw new ArgumentException("the owner identity should be valid.");

            var KeySHA1 = Owner.MakeKeySHA1();

            // --> select permission and accessor information from database.
            var Query = (
                from Perm in m_X509Context.Permissions
                join Cert in m_X509Context.Certificates on Perm.AccessKeySHA1 equals Cert.KeySHA1
                where Perm.KeySHA1 == KeySHA1
                select new { Perm, Cert.Subject, Cert.KeyIdentifier, Cert.KeySHA1 })
                .AsEnumerable();

            var Perms = Query.Select(X => new CertificatePermission
            {
                Owner = Owner,
                Accessor = new CertificateIdentity(X.Subject, X.KeyIdentifier, X.KeySHA1),
                CreationTime = X.Perm.CreationTime,
                LastWriteTime = X.Perm.LastWriteTime,
                CanAuthorityInterfere = X.Perm.CanAuthorityInterfere,
                CanGenerate = X.Perm.CanGenerate,
                CanDelete = X.Perm.CanDelete,
                CanList = X.Perm.CanList,
                CanRevoke = X.Perm.CanRevoke
            });

            return Task.FromResult(Perms.ToArray());
        }

        /// <inheritdoc/>
        public Task<bool> SetAsync(CertificatePermission Permissions, CancellationToken Token = default)
        {
            if (Permissions is null)
                throw new ArgumentNullException(nameof(Permissions));

            if (Permissions.Owner.Validity == false)
                throw new ArgumentException("the owner identity should be valid.");

            var KeySHA1 = Permissions.Owner.MakeKeySHA1();
            var AccSHA1 = string.Empty;

            // --> to alter default.
            if (Permissions.Accessor.Validity == true)
                AccSHA1 = Permissions.Accessor.MakeKeySHA1();

            var Exact = m_X509Context.Permissions
                .Where(X => X.KeySHA1 == KeySHA1 && X.AccessKeySHA1 == AccSHA1)
                .FirstOrDefault();

            if (Exact is null)
            {
                // --> no load if owner is self-signed.
                var ExactOwner = m_X509Context.Certificates
                    .Where(X => X.KeySHA1 == KeySHA1).Where(X => X.Subject != X.Issuer)
                    .Where(X => X.KeyIdentifier != X.IssuerKeyIdentifier)
                    .Select(X => new { X.Issuer,   X.IssuerKeyIdentifier })
                    .FirstOrDefault();

                var AthSHA1 = KeySHA1;
                if (ExactOwner != null)
                {
                    AthSHA1 = new CertificateIdentity(
                        ExactOwner.Issuer, ExactOwner.IssuerKeyIdentifier)
                        .MakeKeySHA1();
                }

                Exact = new DbCertificatePermission
                {
                    KeySHA1 = KeySHA1,
                    AccessKeySHA1 = AccSHA1,
                    AuthorityKeySHA1 = AthSHA1,
                    CreationTime = DateTimeOffset.UtcNow,
                    LastWriteTime = DateTimeOffset.UtcNow,
                    CanAuthorityInterfere = Permissions.CanAuthorityInterfere,
                    CanGenerate = Permissions.CanGenerate,
                    CanList = Permissions.CanList,
                    CanRevoke = Permissions.CanRevoke,
                    CanDelete = Permissions.CanDelete
                };

                if (m_X509Context.DbContext.DbCreate(Exact))
                    return Task.FromResult(true);

                return Task.FromResult(false);
            }

            Exact.LastWriteTime = DateTimeOffset.UtcNow;
            Exact.CanAuthorityInterfere = Permissions.CanAuthorityInterfere;
            Exact.CanGenerate = Permissions.CanGenerate;
            Exact.CanList = Permissions.CanList;
            Exact.CanRevoke = Permissions.CanRevoke;
            Exact.CanDelete = Permissions.CanDelete;

            if (m_X509Context.DbContext.DbUpdate(Exact))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        /// <inheritdoc/>
        public Task<bool> UnsetAsync(CertificateIdentity Accessor, CertificateIdentity Target, CancellationToken Token = default)
        {
            var KeySHA1 = Target.MakeKeySHA1();
            var AccSHA1 = Accessor.Validity == true ? Accessor.MakeKeySHA1() : string.Empty;

            var Exact = m_X509Context.Permissions
                .Where(X => X.KeySHA1 == KeySHA1 && X.AccessKeySHA1 == AccSHA1)
                .FirstOrDefault();

            if (Exact is null)
                return Task.FromResult(false);

            var Result = m_X509Context.DbContext.DbRemove(Exact);
            return Task.FromResult(Result);
        }
    }
}
