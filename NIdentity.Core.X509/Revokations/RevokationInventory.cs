using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509.Revokations
{
    /// <summary>
    /// Certificate's Revokation Inventory.
    /// </summary>
    public class RevokationInventory
    {
        /// <summary>
        /// Authority who made this list.
        /// </summary>
        public CertificateIdentity Authority { get; set; }

        /// <summary>
        /// Creation Time.
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Last Write Time.
        /// </summary>
        public DateTimeOffset LastWriteTime { get; set; }

        /// <summary>
        /// Revision Tracker.
        /// </summary>
        public long? Revision { get; set; }

        /// <summary>
        /// Total Revokations.
        /// </summary>
        public int TotalRevokations { get; set; }
    }
}
