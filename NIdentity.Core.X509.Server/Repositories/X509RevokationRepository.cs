using NIdentity.Core.Server.Helpers;
using NIdentity.Core.X509.Revokations;
using NIdentity.Core.X509.Server.Repositories.Models;
using NIdentity.Core.X509.Server.Revokations;

namespace NIdentity.Core.X509.Server.Repositories
{
    public class X509RevokationRepository : IMutableRevocationRepository, IRevocationRepository
    {
        private readonly X509Context m_X509Context;

        /// <summary>
        /// Initialize a new <see cref="X509Repository"/> instance.
        /// </summary>
        /// <param name="X509Context"></param>
        public X509RevokationRepository(X509Context X509Context) => m_X509Context = X509Context;

        /// <inheritdoc/>
        public Task<RevokationInventory> GetInventoryAsync(Certificate Authority, CancellationToken Token = default)
        {
            if (Authority is null)
                throw new ArgumentNullException(nameof(Authority));

            var KeySHA1 = Authority.KeySHA1;
            var Inventory = m_X509Context.RevokationInventories
                .Where(X => X.KeySHA1 == KeySHA1)
                .FirstOrDefault();

            var Revokations = m_X509Context.Revokations
                .Where(X => X.AuthorityKeySHA1 == KeySHA1)
                .Count();

            if (Inventory is null)
                return Task.FromResult<RevokationInventory>(null);

            return Task.FromResult(new RevokationInventory
            {
                Authority = Authority.Self,
                CreationTime = Inventory.CreationTime,
                LastWriteTime = Inventory.LastWriteTime,
                Revision = Inventory.Revision,
                TotalRevokations = Revokations
            });
        }

        /// <inheritdoc/>
        public Task<Revokation[]> ListRevokationsAsync(RevokationInventory Inventory, int Offset, int Count = 20, CancellationToken Token = default)
        {
            if (Inventory is null)
                throw new ArgumentNullException(nameof(Inventory));

            var KeySHA1 = Inventory.Authority.MakeKeySHA1();
            var CurrentInventory = m_X509Context.RevokationInventories
                .Where(X => X.KeySHA1 == KeySHA1).FirstOrDefault();

            if (CurrentInventory is null)
                return Task.FromResult<Revokation[]>(null);

            // --> if revision number specified.
            var Query = m_X509Context.Revokations
                .Where(X => X.AuthorityKeySHA1 == KeySHA1);

            if (Inventory.Revision.HasValue)
            {
                var TargetRevision = Inventory.Revision.Value;
                Query = Query.Where(X => X.Revision <= TargetRevision);
            }

            var Result = Query.OrderBy(X => X.Number)
                .Skip(Offset).Take(Count).AsEnumerable()
                .Select(X => new Revokation
                {
                    Reference = new CertificateReference(X.SerialNumber, X.IssuerKeyIdentifier, X.RefSHA1),
                    Revision = CurrentInventory.Revision, Reason = X.Reason, Time = X.Time,
                })
                .ToArray();

            return Task.FromResult(Result);
        }

        /// <summary>
        /// Reset the revision number.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <returns></returns>
        private async Task ResetRevisionAsync(DbRevokationInventory Inventory)
        {
            while (true)
            {
                Inventory.Revision = 0; // --> integer rotated.
                Inventory.LastWriteTime = DateTimeOffset.UtcNow;

                if (!m_X509Context.DbContext.DbUpdate(Inventory))
                {
                    await m_X509Context.DbContext.Entry(Inventory).ReloadAsync();
                    continue;
                }

                break;
            }
        }

        /// <summary>
        /// Merge revision number to zero.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="KeySHA1"></param>
        private async Task MergeRevisionAsync(DbRevokationInventory Inventory, string KeySHA1)
        {
            var Offset = 0;
            // --> merge revisions.

            while (true)
            {
                var Revokations = m_X509Context.Revokations
                    .Where(X => X.AuthorityKeySHA1 == KeySHA1)
                    .Where(X => X.Revision != 0)
                    .Skip(Offset).Take(100).ToArray();

                if (Revokations.Length <= 0)
                    break;

                foreach (var Each in Revokations)
                {
                    Each.Revision = 0;

                    if (!m_X509Context.DbContext.DbUpdate(Inventory))
                        await m_X509Context.DbContext.Entry(Inventory).ReloadAsync();
                }

                Offset += Revokations.Length;
            }
        }

        /// <summary>
        /// Get the revision number where the revokation is placed.
        /// </summary>
        /// <param name="Inventory"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        private async Task<long> IncrementRevisionAsync(DbRevokationInventory Inventory)
        {
            var KeySHA1 = Inventory.KeySHA1;
            while (true)
            {
                var IntegerRotated = false;
                if (Inventory.Revision + 1 >= long.MaxValue - 1)
                {
                    IntegerRotated = true;
                    await ResetRevisionAsync(Inventory);
                }

                if (IntegerRotated)
                    await MergeRevisionAsync(Inventory, KeySHA1);

                Inventory.Revision = Inventory.Revision + 1;
                Inventory.LastWriteTime = DateTimeOffset.UtcNow;

                if (m_X509Context.DbContext.DbUpdate(Inventory))
                    return Inventory.Revision;

                await m_X509Context.DbContext.Entry(Inventory).ReloadAsync();
            }
        }

        /// <inheritdoc/>
        public async Task<bool> AddRevokationAsync(Certificate Authority, CertificateReference Target, CertificateRevokeReason Reason, CancellationToken Token = default)
        {
            if (Authority is null)
                throw new ArgumentNullException(nameof(Authority));

            if (Target.Validity == false)
                return false;

            if (Reason == CertificateRevokeReason.None)
                return false;

            var KeySHA1 = Authority.KeySHA1;
            var Inventory = m_X509Context.RevokationInventories
                .Where(X => X.KeySHA1 == KeySHA1).FirstOrDefault();

            while (Inventory is null)
            {
                Inventory = new DbRevokationInventory
                {
                    KeySHA1 = KeySHA1,
                    Subject = Authority.Subject,
                    KeyIdentifier = Authority.KeyIdentifier,
                    CreationTime = DateTimeOffset.UtcNow,
                    LastWriteTime = DateTimeOffset.UtcNow,
                    Revision = 0
                };

                if (m_X509Context.DbContext.DbCreate(Inventory))
                    break;

                Inventory = m_X509Context.RevokationInventories
                    .Where(X => X.KeySHA1 == KeySHA1).FirstOrDefault();
            }

            var Revokation = new DbRevokation
            {
                AuthorityKeySHA1 = KeySHA1,
                RefSHA1 = Target.MakeRefSHA1(),
                Revision = Inventory.Revision,
                SerialNumber = Authority.SerialNumber,
                IssuerKeyIdentifier = Authority.KeyIdentifier,
                Reason = Reason,
                Time = DateTimeOffset.UtcNow
            };

            if (m_X509Context.DbContext.DbCreate(Revokation))
            {
                await IncrementRevisionAsync(Inventory);
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveRevokationAsync(Certificate Authority, CertificateReference Target, CancellationToken Token = default)
        {
            if (Authority is null)
                throw new ArgumentNullException(nameof(Authority));

            if (Target.Validity == false)
                return false;

            var KeySHA1 = Authority.KeySHA1;
            var Inventory = m_X509Context.RevokationInventories
                .Where(X => X.KeySHA1 == KeySHA1)
                .FirstOrDefault();

            if (Inventory is null)
                return false;

            var RefSHA1 = Target.MakeRefSHA1();
            var Revokation = m_X509Context.Revokations
                .Where(X => X.AuthorityKeySHA1 == KeySHA1)
                .Where(X => X.RefSHA1 == RefSHA1)
                .FirstOrDefault();

            if (m_X509Context.DbContext.DbRemove(Revokation))
            {
                await IncrementRevisionAsync(Inventory);
                return true;
            }

            return false;
        }
    }
}
