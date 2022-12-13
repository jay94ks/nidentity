using NIdentity.Core.X509.Documents;

namespace NIdentity.Core.X509.Server.Commands
{
    public class X509RequesterAccesor
    {
        private readonly AsyncLocal<Authorization> m_Local = new();
        private class Authorization
        {
            /// <summary>
            /// Requester's certificate.
            /// </summary>
            public Certificate Requester { get; set; }

            /// <summary>
            /// Indicates whether the requester is super-access or not.
            /// </summary>
            public bool IsSuperAccess { get; set; }
        }

        /// <summary>
        /// Execute the getter.
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="Getter"></param>
        /// <returns></returns>
        private TReturn Exec<TReturn>(Func<Authorization, TReturn> Getter)
        {
            if (m_Local.Value is null)
                m_Local.Value = new Authorization();

            return Getter(m_Local.Value);
        }

        /// <summary>
        /// Requester's certificate.
        /// If this set null and <see cref="IsSuperAccess"/> set true,
        /// that will strip requester checking branches.
        /// </summary>
        public Certificate Requester
        {
            get => Exec(X => X.Requester);
            set => Exec(X => X.Requester = value);
        }

        /// <summary>
        /// Indicates whether the requester is super-access or not.
        /// </summary>
        public bool IsSuperAccess
        {
            get => Exec(X => X.IsSuperAccess);
            set => Exec(X => X.IsSuperAccess = value);
        }
    }
}
