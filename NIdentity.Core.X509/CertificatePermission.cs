using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509
{
    /// <summary>
    /// Permission of certificate.
    /// </summary>
    public class CertificatePermission
    {
        /// <summary>
        /// Owner Identity.
        /// </summary>
        public CertificateIdentity Owner { get; set; }

        /// <summary>
        /// Accessor Identity.
        /// </summary>
        public CertificateIdentity Accessor { get; set; }

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset LastWriteTime { get; set; }

        /// <summary>
        /// Indicates whether this permission entity is about children default or not.
        /// </summary>
        public bool IsChildrenDefault => Accessor.Validity == false;

        /// <summary>
        /// Indicates whether the authority of <see cref="Owner"/> can interfere this intermediate CA's certificates or not.
        /// If this option set true, all accesses from the authority of <see cref="Owner"/> will be denied.
        /// And this option can only be altered by exact owner.
        /// </summary>
        public bool CanAuthorityInterfere { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can generate intermediate or leafs or not.
        /// (Leafs can never generate root or intermediates)
        /// </summary>
        /// <returns></returns>
        public bool CanGenerate { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can list certificates or not.
        /// </summary>
        public bool CanList { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can revoke certificates or not.
        /// </summary>
        public bool CanRevoke { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can delete certificates or not.
        /// </summary>
        public bool CanDelete { get; set; } = true;

        /// <summary>
        /// Indicates whether the certificate of <see cref="Accessor"/> can alter permissions or not.
        /// </summary>
        public bool CanAlter { get; set; } = false;
    }
}
