using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NIdentity.Core.Commands;
using NIdentity.Core.Commands.Internals;
using NIdentity.Core.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace NIdentity.Connector.Internals
{
    internal static class CertificateHelpers
    {
        /// <summary>
        /// Convert the result to the expected result type.
        /// </summary>
        /// <typeparam name="TExpectedResult"></typeparam>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static CommandResult ToExpectedResult<TExpectedResult>(this CommandResult Result) where TExpectedResult : CommandResult
        {
            if (Result is RemoteCommandResult Remote)
            {
                if (Remote.Success && Remote.Response != null)
                    return Remote.Response.ToObject<TExpectedResult>();
            }

            return Result;
        }

        private class RawBulkResult
        {
            /// <summary>
            /// Bulk results.
            /// </summary>
            [JsonProperty("results")]
            public JObject[] Results { get; set; }

            /// <summary>
            /// Total Time.
            /// </summary>
            [JsonProperty("total_time")]
            public TimeSpan TotalTime { get; set; }
        }

        /// <summary>
        /// Convert the result to the expected result type.
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static CommandResult ToExpectedResult(this CommandResult Result, Func<int, JObject, CommandResult> Bulk) 
        {
            if (Result is RemoteCommandResult Remote)
            {
                if (Remote.Success && Remote.Response != null)
                {
                    var RawBulk = Remote.Response.ToObject<RawBulkResult>();
                    if (RawBulk.Results is null || RawBulk.Results.Length <= 0)
                    {
                        return new BulkCommand.Result
                        {
                            Success = Result.Success,
                            Reason = Result.Reason,
                            ReasonKind = Result.ReasonKind,
                            Results = new CommandResult[0],
                            TotalTime = RawBulk.TotalTime
                        };
                    }

                    var Results = new List<CommandResult>();
                    foreach (var Each in RawBulk.Results.Select((X, i) => (X, i)))
                        Results.Add(Bulk.Invoke(Each.i, Each.X));

                    return new BulkCommand.Result
                    {
                        Success = Result.Success,
                        Reason = Result.Reason,
                        ReasonKind = Result.ReasonKind,
                        Results = Results.ToArray(),
                        TotalTime = RawBulk.TotalTime
                    };
                }
            }

            return Result;
        }

    }
}
