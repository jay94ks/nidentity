namespace NIdentity.Connector.AspNetCore.Filters
{
    /// <summary>
    /// Marks an action that requires specific <see cref="RequesterIdentityKind"/>s.
    /// </summary>
    public class RequireIdentityKindAttribute : RequireIdentityAttribute
    {
        /// <summary>
        /// Initialize a new <see cref="RequireIdentityAttribute"/> with kinds.
        /// </summary>
        /// <param name="Kinds"></param>
        public RequireIdentityKindAttribute(RequesterIdentityKind[] Kinds)
        {
            this.Kinds = Kinds
                .GroupBy(X => X).Select(X => X.Key)
                .OrderByDescending(X => (int)X).ToArray();
        }

        /// <summary>
        /// Required Identity Kinds.
        /// The requester must have one of kinds specified.
        /// </summary>
        public RequesterIdentityKind[] Kinds { get; }

        /// <inheritdoc/>
        protected override Task<bool> CheckIdentityAsync(Requester Requester)
        {
            if (Kinds.Count(Requester.Kinds.Contains) <= 0)
                return Task.FromResult(false);

            return base.CheckIdentityAsync(Requester);
        }
    }
}
