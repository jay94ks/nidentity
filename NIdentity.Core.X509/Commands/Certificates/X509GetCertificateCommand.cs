using Newtonsoft.Json;
using NIdentity.Core.Commands;
using NIdentity.Core.X509.Commands;

namespace NIdentity.Core.X509.Commands.Certificates
{
    [Command(Kind = "x509")]
    public class X509GetCertificateCommand : X509CertificateAccessCommand
    {
        /// <summary>
        /// Initialize a new <see cref="X509GetCertificateCommand"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        public X509GetCertificateCommand() : base("cert_get")
        {
        }

        /// <summary>
        /// Result.
        /// </summary>
        public class Result : CertificateResult<Result>
        {
            /// <summary>
            /// Certificate CER file. 
            /// </summary>
            [JsonProperty("cer_base64")]
            public string CerBase64 { get; set; }
        }
    }
}
