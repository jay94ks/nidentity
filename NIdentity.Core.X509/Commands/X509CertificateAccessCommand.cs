using Newtonsoft.Json;
using NIdentity.Core.Commands;

namespace NIdentity.Core.X509.Commands
{
    /// <summary>
    /// Base class for certificate access commands.
    /// </summary>
    public abstract class X509CertificateAccessCommand : Command
    {
        /// <summary>
        /// Initialize a new <see cref="X509CertificateAccessCommand"/> instance.
        /// </summary>
        /// <param name="Type"></param>
        protected X509CertificateAccessCommand(string Type) : base(Type)
        {
        }

        // -------------- (by key id) --

        /// <summary>
        /// Subject.
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; }

        /// <summary>
        /// Key Identifier.
        /// </summary>
        [JsonProperty("key_id")]
        public string KeyIdentifier { get; set; }

        [JsonIgnore]
        public CertificateIdentity ByIdentity => new CertificateIdentity(Subject, KeyIdentifier);

        // ------------- (by cert ref) --

        /// <summary>
        /// Serial Number.
        /// </summary>
        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Issuer's Key Identifier.
        /// </summary>
        [JsonProperty("issuer_key_id")]
        public string IssuerKeyIdentifier { get; set; }

        [JsonIgnore]
        public CertificateReference ByReference => new CertificateReference(SerialNumber, IssuerKeyIdentifier);

        /// <summary>
        /// Certificate Result.
        /// </summary>
        public class CertificateResult : CommandResult
        {
            /// <summary>
            /// Make a <typeparamref name="TResult"/> result.
            /// </summary>
            /// <typeparam name="TResult"></typeparam>
            /// <param name="Certificate"></param>
            /// <param name="More"></param>
            /// <returns></returns>
            public static TResult Make<TResult>(Certificate Certificate, Action<TResult> More = null)
                where TResult : CertificateResult, new()
            {
                var Result = new TResult
                {
                    SerialNumber = Certificate.SerialNumber,
                    KeyIdentifier = Certificate.KeyIdentifier,
                    IssuerKeyIdentifier = Certificate.Issuer.KeyIdentifier,
                    Subject = Certificate.Subject,
                    Issuer = Certificate.Issuer.Subject,
                    Thumbprint = Certificate.Thumbprint,
                    CreationTime = Certificate.CreationTime,
                    ExpirationTime = Certificate.ExpirationTime,
                    IsExpired = Certificate.IsExpired,
                    IsRevoked = Certificate.IsRevokeIdentified,
                    RevokeReason = Certificate.RevokeReason,
                    RevokeTime = Certificate.RevokeTime,
                    Certificate = Certificate
                };

                More?.Invoke(Result);
                return Result;
            }

            /// <summary>
            /// Make a <typeparamref name="CertificateResult"/> result.
            /// </summary>
            /// <param name="Certificate"></param>
            /// <returns></returns>
            public static CertificateResult Make(Certificate Certificate)
            {
                var Result = new CertificateResult
                {
                    SerialNumber = Certificate.SerialNumber,
                    KeyIdentifier = Certificate.KeyIdentifier,
                    IssuerKeyIdentifier = Certificate.Issuer.KeyIdentifier,
                    Subject = Certificate.Subject,
                    Issuer = Certificate.Issuer.Subject,
                    Thumbprint = Certificate.Thumbprint,
                    CreationTime = Certificate.CreationTime,
                    ExpirationTime = Certificate.ExpirationTime,
                    IsExpired = Certificate.IsExpired,
                    IsRevoked = Certificate.IsRevokeIdentified,
                    RevokeReason = Certificate.RevokeReason,
                    RevokeTime = Certificate.RevokeTime,
                    Certificate = Certificate
                };

                return Result;
            }

            /// <summary>
            /// Generated Certificate's Serial Number.
            /// </summary>
            [JsonProperty("serial_number")]
            public string SerialNumber { get; set; }

            /// <summary>
            /// Generated Certificate's Key Identifier.
            /// </summary>
            [JsonProperty("key_id")]
            public string KeyIdentifier { get; set; }

            /// <summary>
            /// Generated Certificate's Issuer's Key Identifier.
            /// </summary>
            [JsonProperty("issuer_key_id")]
            public string IssuerKeyIdentifier { get; set; }

            /// <summary>
            /// Generated Certificate's Subject.
            /// </summary>
            [JsonProperty("subject")]
            public string Subject { get; set; }

            /// <summary>
            /// Generated Certificate's Issuer.
            /// </summary>
            [JsonProperty("issuer")]
            public string Issuer { get; set; }

            /// <summary>
            /// Generated Certificate's Thumbprint.
            /// </summary>
            [JsonProperty("thumbprint")]
            public string Thumbprint { get; set; }

            /// <summary>
            /// Generated Certificate's Revokation Status.
            /// </summary>
            [JsonProperty("is_expired")]
            public bool IsExpired { get; set; }

            /// <summary>
            /// Generated Certificate's Revokation Status.
            /// </summary>
            [JsonProperty("is_revoked")]
            public bool IsRevoked { get; set; }

            /// <summary>
            /// Generated Certificate's Revokation Time.
            /// </summary>
            [JsonProperty("creation_time")]
            public DateTimeOffset? CreationTime { get; set; }

            /// <summary>
            /// Generated Certificate's Revokation Time.
            /// </summary>
            [JsonProperty("expiration_time")]
            public DateTimeOffset? ExpirationTime { get; set; }

            /// <summary>
            /// Generated Certificate's Revokation Reason.
            /// </summary>
            [JsonProperty("revoke_reason")]
            public CertificateRevokeReason? RevokeReason { get; set; }

            /// <summary>
            /// Generated Certificate's Revokation Time.
            /// </summary>
            [JsonProperty("revoke_time")]
            public DateTimeOffset? RevokeTime { get; set; }

            /// <summary>
            /// Certificate. (Not serializable)
            /// </summary>
            [JsonIgnore]
            public Certificate Certificate { get; set; }
        }

        public class CertificateResult<TResult> : CertificateResult where TResult : CertificateResult, new()
        {
            /// <summary>
            /// Make a <typeparamref name="TResult"/> result.
            /// </summary>
            /// <typeparam name="TResult"></typeparam>
            /// <param name="Certificate"></param>
            /// <param name="More"></param>
            /// <returns></returns>
            public static TResult Make(Certificate Certificate, Action<TResult> More = null)
            {
                return Make<TResult>(Certificate, More);
            }
        }
    }
}
