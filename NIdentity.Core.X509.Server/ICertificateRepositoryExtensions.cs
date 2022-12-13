namespace NIdentity.Core.X509.Server
{
    /// <summary>
    /// Certificate Repository Extensions.
    /// </summary>
    public static class ICertificateRepositoryExtensions
    {
        private static readonly Certificate[] EMPTY_CHAIN = new Certificate[0];

        /// <summary>
        /// Load chained certificates asynchronously.
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Identity"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static async Task<Certificate[]> LoadChainAsync(
            this ICertificateRepository Repository, CertificateReference Identity,
            CancellationToken Token = default)
        {
            var First = await Repository.LoadAsync(Identity, Token);
            if (First is null)
            {
                return EMPTY_CHAIN;
            }

            // --> if self-signed, stop loading here.
            var Results = new List<Certificate>();
            Results.Add(First);

            if (First.IsSelfSigned)
                return Results.ToArray();

            // --> loads all chained issuers.
            var Subject = First.Issuer;

            while (First != null)
            {
                var Issuer = await Repository.LoadAsync(Subject, Token);
                if (Issuer is null)
                    break;

                Results.Add(Issuer);
                if (Issuer.IsSelfSigned)
                    break;

                // --> set to next certificate.
                Subject = Issuer.Issuer;
            }

            return Results.ToArray();
        }

        /// <summary>
        /// Test whether the <paramref name="MaybeIssuer"/> has privilege of <paramref name="Target"/> certificate.
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="MaybeIssuer"></param>
        /// <param name="Target"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        public static async Task<bool> IsIssuerAsync(this ICertificateRepository Repository,
            Certificate MaybeIssuer, Certificate Target, CancellationToken Token = default)
        {
            if (ReferenceEquals(MaybeIssuer, null))
                throw new ArgumentNullException(nameof(MaybeIssuer));

            if (ReferenceEquals(Target, null))
                throw new ArgumentNullException(nameof(Target));

            while (Target != null)
            {
                if (MaybeIssuer.Equals(Target))
                    return true;

                if (Target.IsSelfSigned)
                    break;

                Target = await Repository.LoadAsync(Target.Issuer, Token);
            }

            return false;
        }
    }
}
