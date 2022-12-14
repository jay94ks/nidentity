using NIdentity.Core.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Connector.X509
{
    /// <summary>
    /// Certificate collection that is read only.
    /// </summary>
    public class X509CertificateCollection : IReadOnlyCollection<Certificate>
    {
        private readonly Certificate[] m_Certificates;

        /// <summary>
        /// Initialize a new <see cref="X509CertificateCollection"/> instance.
        /// </summary>
        /// <param name="Certificates"></param>
        /// <param name="TotalCount"></param>
        internal X509CertificateCollection(IEnumerable<Certificate> Certificates, int TotalCount )
        {
            this.TotalCount = TotalCount;
            m_Certificates = Certificates.ToArray();
        }

        /// <inheritdoc/>
        public int Count => m_Certificates.Length;

        /// <inheritdoc/>
        public int TotalCount { get; }

        /// <inheritdoc/>
        public IEnumerator<Certificate> GetEnumerator() => m_Certificates.AsEnumerable().GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => m_Certificates.GetEnumerator();
    }
}
