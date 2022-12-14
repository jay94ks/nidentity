using System.Collections;

namespace NIdentity.Connector.AspNetCore
{
    /// <summary>
    /// Describes the requester itself.
    /// </summary>
    public class Requester : IReadOnlyCollection<RequesterIdentity>
    {
        private static readonly object KEY = new();
        private static readonly RequesterIdentity[] EMPTY = new RequesterIdentity[0];

        private readonly HashSet<RequesterIdentity> m_Identities = new();
        private readonly Dictionary<RequesterIdentityKind, List<RequesterIdentity>> m_Kinds = new();

        /// <summary>
        /// Hides the constructor.
        /// </summary>
        private Requester(HttpContext HttpContext)
            => this.HttpContext = HttpContext;

        /// <summary>
        /// Get the <see cref="Requester"/> instance from <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <returns></returns>
        public static Requester FromHttpContext(HttpContext HttpContext)
        {
            HttpContext.Items.TryGetValue(KEY, out var Temp);
            if (Temp is not Requester Requester)
                HttpContext.Items[KEY] = Requester = new(HttpContext);

            return Requester;
        }

        /// <summary>
        /// Http Context who owned this requester instance.
        /// </summary>
        public HttpContext HttpContext { get; }

        /// <summary>
        /// Http Request from <see cref="HttpContext"/>.
        /// </summary>
        public HttpRequest HttpRequest => HttpContext.Request;

        /// <inheritdoc/>
        public int Count => m_Identities.Count;

        /// <inheritdoc/>
        public IEnumerator<RequesterIdentity> GetEnumerator() => m_Identities.OrderByDescending(X => X.Kind).GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => m_Identities.OrderByDescending(X => X.Kind).GetEnumerator();

        /// <summary>
        /// Identity Kinds that included in this requester.
        /// </summary>
        public IReadOnlyCollection<RequesterIdentityKind> Kinds => m_Kinds.Keys;

        /// <summary>
        /// Get <see cref="RequesterIdentity"/> instances by their kind.
        /// </summary>
        /// <param name="Kind"></param>
        /// <param name="Snapshot"></param>
        /// <returns></returns>
        public IEnumerable<RequesterIdentity> ByKind(RequesterIdentityKind Kind, bool Snapshot = true)
        {
            if (m_Kinds.TryGetValue(Kind, out var Identities) == true)
                return Snapshot ? Identities.ToArray() : Identities;

            return EMPTY;
        }

        /// <summary>
        /// Test whether the requester has given identity or not.
        /// </summary>
        /// <typeparam name="TIdentity"></typeparam>
        /// <returns></returns>
        public bool Has<TIdentity>() where TIdentity : RequesterIdentity
            => m_Identities.FirstOrDefault(X => X is TIdentity) != null;

        /// <summary>
        /// Test whether the requester has given identity or not.
        /// </summary>
        /// <returns></returns>
        public bool Has(Type IdentityType)
        {
            if (IdentityType is null)
                throw new ArgumentNullException(nameof(IdentityType));

            if (IdentityType.IsAssignableTo(typeof(RequesterIdentity)) == false)
                throw new ArgumentException($"{IdentityType.FullName} is not identity type.");

            return m_Identities.FirstOrDefault(X => IdentityType.IsAssignableFrom(X.GetType())) != null;
        }

        /// <summary>
        /// Get the requester's identity.
        /// </summary>
        /// <typeparam name="TIdentity"></typeparam>
        /// <returns></returns>
        public TIdentity Get<TIdentity>() where TIdentity : RequesterIdentity
             => m_Identities.FirstOrDefault(X => X is TIdentity) as TIdentity;

        /// <summary>
        /// Test whether the requester has given identity or not.
        /// </summary>
        /// <returns></returns>
        public RequesterIdentity Get(Type IdentityType)
        {
            if (IdentityType is null)
                throw new ArgumentNullException(nameof(IdentityType));

            if (IdentityType.IsAssignableTo(typeof(RequesterIdentity)) == false)
                throw new ArgumentException($"{IdentityType.FullName} is not identity type.");

            return m_Identities.FirstOrDefault(X => IdentityType.IsAssignableFrom(X.GetType()));
        }

        /// <summary>
        /// Add a requester identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public bool Add(RequesterIdentity Identity)
        {
            if (Identity is null)
                throw new ArgumentNullException(nameof(Identity));

            if (Has(Identity.GetType()) == false)
            {
                if (!m_Identities.Add(Identity))
                    return false;

                m_Kinds.TryGetValue(Identity.Kind, out var Kinds);
                if (Kinds is null)
                    m_Kinds[Identity.Kind] = Kinds = new();

                Kinds.Add(Identity);
                return true;
            }
            //m_Kinds
            // --> duplicated.
            return false;
        }

        /// <summary>
        /// Remove a requester identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public bool Remove(RequesterIdentity Identity)
        {
            if (Identity is null)
                throw new ArgumentNullException(nameof(Identity));

            if (m_Identities.Remove(Identity))
            {
                m_Kinds.TryGetValue(Identity.Kind, out var Kinds);
                if (Kinds is null)
                    return true;
                
                if (Kinds.Remove(Identity) && Kinds.Count <= 0)
                    m_Kinds.Remove(Identity.Kind);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Replace specified identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Replace(RequesterIdentity Identity)
        {
            if (Identity is null)
                throw new ArgumentNullException(nameof(Identity));

            var Old = Get(Identity.GetType());
            if (Old != null) Remove(Old);

            return Add(Identity);
        }

    }
}
