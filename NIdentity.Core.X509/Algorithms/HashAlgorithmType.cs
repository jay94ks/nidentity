using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509.Algorithms
{
    /// <summary>
    /// Certificate Hash Type.
    /// </summary>
    public enum HashAlgorithmType
    {
        /// <summary>
        /// Default (SHA 256)
        /// </summary>
        Default,

        /// <summary>
        /// SHA 224.
        /// </summary>
        Sha224 = 224,

        /// <summary>
        /// SHA 256.
        /// </summary>
        Sha256 = 256,

        /// <summary>
        /// SHA 384
        /// </summary>
        Sha384 = 384,

        /// <summary>
        /// SHA512
        /// </summary>
        Sha512 = 512
    }
}
