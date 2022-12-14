namespace NIdentity.Connector.AspNetCore
{
    /// <summary>
    /// Requester's Identity.
    /// </summary>
    public abstract class RequesterIdentity : IEquatable<RequesterIdentity>
    {
        private string m_CachedString = null;

        /// <summary>
        /// Identity Kind.
        /// </summary>
        public abstract RequesterIdentityKind Kind { get; }

        /// <summary>
        /// Indicates whether the requester identity is validated or not.
        /// </summary>
        public bool IsValidated { get; private set; }

        /// <summary>
        /// Set the <see cref="IsValidated"/> to the specified value.
        /// </summary>
        /// <param name="Value"></param>
        internal void SetValidated(bool Value) => IsValidated = Value;

        /// <inheritdoc/>
        public bool Equals(RequesterIdentity Other)
        {
            if (ReferenceEquals(Other, null))
                return false;

            if (ReferenceEquals(this, Other))
                return true;

            var SelfType = GetType();
            var OtherType = Other.GetType();

            if (SelfType.IsAssignableFrom(OtherType))
                return false;

            return ToString().Equals(Other.ToString());
        }

        /// <inheritdoc/>
        public override bool Equals(object Input)
        {
            if (ReferenceEquals(Input, null))
                return false;

            if (Input is RequesterIdentity Other)
                return Equals(Other);

            return base.Equals(Input);
        }

        /// <inheritdoc/>
        public override int GetHashCode() => ToString().GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
        {
            if (m_CachedString is null)
                m_CachedString = Serialize() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(m_CachedString))
                m_CachedString = "(null)";

            return m_CachedString;
        }

        /// <summary>
        /// Called to serialize the <see cref="RequesterIdentity"/> to <see cref="string"/>.
        /// </summary>
        /// <returns></returns>
        protected abstract string Serialize();
    }
}
