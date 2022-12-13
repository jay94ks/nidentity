using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509.Commands.Certificates
{
    [Command(Kind = "x509")]
    public class X509RevokeCertificateCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509RevokeCertificateCommand"/> instance.
        /// </summary>
        public X509RevokeCertificateCommand() : base("cert_revoke")
        {
        }

        /// <summary>
        /// Revoke reason.
        /// </summary>
        [JsonProperty("revoke_reason")]
        public CertificateRevokeReason RevokeReason { get; set; } = CertificateRevokeReason.PrivilegeWithdrawn;

        public class Result : CertificateResult<Result>
        {

        }
    }
}
