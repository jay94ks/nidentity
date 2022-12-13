using Microsoft.AspNetCore.Mvc;
using NIdentity.Core.X509.Helpers;
using NIdentity.Core.X509.Server.Helpers;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Ocsp;
using BcCertificateStatus = Org.BouncyCastle.Ocsp.CertificateStatus;

namespace NIdentity.Core.X509.Server.Ocsp
{
    /// <summary>
    /// Ocsp Response.
    /// </summary>
    public class OcspResponse
    {
        private static readonly DerObjectIdentifier PkixOcspExtendedRevoke
            = new DerObjectIdentifier(OcspObjectIdentifiers.PkixOcsp + ".9");

        private readonly OcspRequest m_Request;
        private readonly List<OcspCertificateStatus> m_Results = new();

        /// <summary>
        /// Initialize a new <see cref="OcspResponse"/> instance for specified request.
        /// </summary>
        /// <param name="Request"></param>
        internal OcspResponse(OcspRequest Request) => m_Request = Request;

        /// <summary>
        /// Ocsp Execution Status.
        /// </summary>
        public OcspExecutionStatus Status { get; set; } = OcspExecutionStatus.Successful;

        /// <summary>
        /// Ocsp Responder Certificate.
        /// </summary>
        public Certificate Responder { get; set; }

        /// <summary>
        /// Expiration.
        /// </summary>
        public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(2);


        /// <summary>
        /// Set the certificate entry from response.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public OcspResponse Unset(OcspCertificateIdentity Identity)
        {
            if (!Identity.Validity)
                throw new ArgumentException($"the specified certificate identity is invalid.");

            m_Results.RemoveAll(X => X.Identity == Identity);
            return this;
        }

        /// <summary>
        /// Set the certificate is not revoked.
        /// But if expiration reached, this will set it as privilege withdrawn.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <returns></returns>
        public OcspResponse Set(OcspCertificateIdentity Identity, Certificate Certificate)
        {
            if (Certificate is null)
                throw new ArgumentNullException(nameof(Certificate));

            if (Certificate.IsRevokeIdentified)
            {
                Set(Identity, Certificate.RevokeReason.Value, Certificate.RevokeTime.HasValue
                    ? Certificate.RevokeTime.Value : DateTimeOffset.UtcNow);

                return this;
            }

            if (Certificate.IsExpired)
            {
                Set(Identity, CertificateRevokeReason.PrivilegeWithdrawn, Certificate.ExpirationTime);
                return this;
            }

            var Result = m_Results.Find(X => X.Identity == Identity);
            if (Result != null)
            {
                Result.Reason = null;
                Result.Time = null;
                return this;
            }

            m_Results.Add(new OcspCertificateStatus(Identity));
            return this;
        }

        /// <summary>
        /// Set the certificate revoked by identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <param name="Reason"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        public OcspResponse Set(OcspCertificateIdentity Identity, CertificateRevokeReason Reason, DateTimeOffset Time)
        {
            if (!Identity.Validity)
                throw new ArgumentException($"the specified certificate identity is invalid.");

            var Result = m_Results.Find(X => X.Identity == Identity);
            if (Result != null)
            {
                Result.Reason = Reason;
                Result.Time = Time;
                return this;
            }

            m_Results.Add(new OcspCertificateStatus(Identity)
            {
                Reason = Reason,
                Time = Time
            });

            return this;
        }

        /// <summary>
        /// Generate <see cref="IActionResult"/> from <see cref="OcspResponse"/>.
        /// if <paramref name="Quietly"/> is false, this will throw 
        /// <see cref="InvalidOperationException"/> when the responder is not set (or has no private key).
        /// And if true, this will generate `Unauthorized` when the responder is not set.
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        internal async Task<IActionResult> GenerateAsync(ICertificateRepository Repository, bool Quietly = true, CancellationToken Token = default)
        {
            var Bytes = await EncodeAsync(Repository, Quietly, Token);
            return new FileContentResult(Bytes, "application/ocsp-response");
        }

        /// <summary>
        /// Encode the <see cref="OcspResponse"/> bytes.
        /// if <paramref name="Quietly"/> is false, this will throw 
        /// <see cref="InvalidOperationException"/> when the responder is not set.
        /// </summary>
        /// <param name="Repository"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task<byte[]> EncodeAsync(ICertificateRepository Repository, bool Quietly, CancellationToken Token = default)
        {
            Token.ThrowIfCancellationRequested();
            if (Status != OcspExecutionStatus.Successful)
                return MakeStatusResponse(Status);

            if (Responder is null)
            {
                if (Quietly)
                    return MakeStatusResponse(OcspExecutionStatus.Unauthorized);

                throw new InvalidOperationException("No responder is set.");
            }

            if (Responder.HasPrivateKey == false)
            {
                if (Quietly)
                    return MakeStatusResponse(OcspExecutionStatus.Unauthorized);

                throw new InvalidOperationException("Responder certificate should have private key.");
            }

            var Reqs = m_Request.Ocsp.GetRequestList();
            if (Reqs.Length <= 0)
                return MakeStatusResponse(OcspExecutionStatus.Successful);

            try
            {
                var ResponderChain = await Repository.LoadChainAsync(Responder, Token);
                var Generator = new BasicOcspRespGenerator(Responder.PublicKey);
                var Extensions = new X509ExtensionsGenerator();

                var ThisTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(1));
                var NextTime = DateTime.UtcNow.Add(Expiration);

                foreach (var Each in Reqs)
                {
                    Token.ThrowIfCancellationRequested();
                    var EachId = Each.GetCertID();
                    var Identity = Each.ToOcspIdentity();

                    var Result = m_Results.Find(X => X.Identity == Identity);
                    if (Result is null)
                        Generator.AddResponse(EachId, new UnknownStatus());

                    else if (Result.Reason.HasValue)
                    {
                        var Reason = Result.Reason.Value;
                        var Number = Reason.GetReasonNumber();
                        var Time = Result.Time.Value;

                        if (Reason == CertificateRevokeReason.CertificateHold)
                        {
                            Extensions.AddExtension(PkixOcspExtendedRevoke,
                                false, DerNull.Instance.GetDerEncoded());
                        }

                        Generator.AddResponse(EachId,
                            new RevokedStatus(Time.UtcDateTime, Number),
                            ThisTime, NextTime, null);
                    }

                    else
                    {
                        Generator.AddResponse(EachId,
                            BcCertificateStatus.Good,
                            ThisTime, NextTime, null);

                    }
                }


                // --> copy nonce value from request.
                var Nonce = m_Request.Ocsp.GetExtensionValue(OcspObjectIdentifiers.PkixOcspNonce);
                if (Nonce != null)
                {
                    Extensions.AddExtension(OcspObjectIdentifiers.PkixOcspNonce, false, Nonce.GetOctets());
                }

                var ResponderX509Chain = ResponderChain.Select(X => X.X509).ToArray();
                var Response = Generator.Generate(Responder.CreateSignatureFactory(), ResponderX509Chain, ThisTime);
                return new OCSPRespGenerator().Generate(Status.GetErrorNumber(), Response).GetEncoded();
            }
            catch
            {
            }

            return MakeStatusResponse(OcspExecutionStatus.InternalError);
        }

        /// <summary>
        /// Make status only response bytes.
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        private static byte[] MakeStatusResponse(OcspExecutionStatus Status)
            => new OCSPRespGenerator().Generate(Status.GetErrorNumber(), null).GetEncoded();
    }


}
