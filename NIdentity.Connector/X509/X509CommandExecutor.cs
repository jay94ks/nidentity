using Newtonsoft.Json.Linq;
using NIdentity.Connector.Internals;
using NIdentity.Connector.X509.Caches;
using NIdentity.Core;
using NIdentity.Core.Commands;
using NIdentity.Core.Commands.Internals;
using NIdentity.Core.Helpers;
using NIdentity.Core.X509;
using NIdentity.Core.X509.Commands;
using NIdentity.Core.X509.Commands.Certificates;
using NIdentity.Core.X509.Commands.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Connector.X509
{
    /// <summary>
    /// Wrapper to execute X509 command and convert their results to correct types.
    /// </summary>
    public class X509CommandExecutor
    {
        /// <summary>
        /// Initialize a new <see cref="X509CommandExecutor"/> instance.
        /// </summary>
        /// <param name="Executor"></param>
        /// <param name="Parameters"></param>
        internal X509CommandExecutor(RemoteCommandExecutor Executor, RemoteCommandExecutorParameters Parameters)
        {
            this.Executor = Executor;
            CacheRepository = Parameters.CacheRepository
                ?? new X509CertificateCacheRepository(100);
        }

        /// <summary>
        /// Remote Executor.
        /// </summary>
        public RemoteCommandExecutor Executor { get; }

        /// <summary>
        /// Certificate Caches.
        /// </summary>
        public ICertificateCacheRepository CacheRepository { get; }

        /// <inheritdoc/>
        public Task<CommandResult> Execute(Command Command, CancellationToken Token = default)
            => Executor.Execute(Command, Token);

        /// <inheritdoc/>
        public Task<CommandResult> Execute(JObject Json, CancellationToken Token = default) 
            => Executor.Execute(Json, Token);

        /// <summary>
        /// Execute X509 bulk requests and returns execution result.
        /// </summary>
        /// <param name="Commands"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<CommandResult> BulkAsync(IEnumerable<Command> Commands, CancellationToken Token = default)
        {
            Commands = Commands.ToArray();

            foreach (var Each in Commands)
            {
                Each.Type = $"x509.{Each.Type}";
                if (Each is X509CertificateAccessCommand)
                    continue;

                if (Each is X509GenerateCommand)
                    continue;

                if (Each is X509DocumentAccessCommand)
                    continue;

                var EachType = Each.GetType();
                throw new InvalidOperationException($"{EachType.FullName} is not X509 command type.");
            }

            var Results = new CommandResult[Commands.Count()];
            var Bulk = new BulkCommand().SetActions(Commands);
            return (await Executor.Execute(Bulk, Token))
                .ToExpectedResult((Index, Json) =>
                {
                    switch (Commands.ElementAt(Index))
                    {
                        case X509GenerateCommand:
                            return Json.ToObject<X509GenerateCommand.Result>();
                        case X509GetCertificateCommand:
                            return Json.ToObject<X509GetCertificateCommand.Result>();
                        case X509ListCertificateCommand:
                            return Json.ToObject<X509ListCertificateCommand.Result>();
                        case X509RevokeCertificateCommand:
                            return Json.ToObject<X509RevokeCertificateCommand.Result>();
                        case X509UnrevokeCertificateCommand:
                            return Json.ToObject<X509UnrevokeCertificateCommand.Result>();
                        case X509DeleteCertificateCommand:
                            return Json.ToObject<X509DeleteCertificateCommand.Result>();
                        case X509ListDocumentCommand:
                            return Json.ToObject<X509ListDocumentCommand.Result>();
                        case X509ReadDocumentCommand:
                            return Json.ToObject<X509ReadDocumentCommand.Result>();
                        case X509WriteDocumentCommand:
                            return Json.ToObject<X509WriteDocumentCommand.Result>();
                        case X509RemoveDocumentCommand:
                            return Json.ToObject<X509RemoveDocumentCommand.Result>();
                        default:
                            break;
                    }

                    return Json.ToObject<CommandResult>();
                });
        }

        /// <summary>
        /// Generate the X509 certificate asynchronously.
        /// </summary>
        public async Task<CommandResult> GenerateCertificateAsync(X509GenerateCommand Command, CancellationToken Token = default) 
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509GenerateCommand.Result>();

        /// <summary>
        /// Get the X509 certificate asynchronously.
        /// </summary>
        public async Task<CommandResult> GetCertificateAsync(X509GetCertificateCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509GetCertificateCommand.Result>();

        /// <summary>
        /// Get the X509 certificate asynchronously.
        /// </summary>
        public async Task<CommandResult> GetCertificateMetaAsync(X509GetCertificateMetaCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509GetCertificateMetaCommand.Result>();

        /// <summary>
        /// List the X509 certificate asynchronously.
        /// </summary>
        public async Task<CommandResult> ListCertificatesAsync(X509ListCertificateCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509ListCertificateCommand.Result>();

        /// <summary>
        /// Revoke the X509 certificate asynchronously.
        /// </summary>
        public async Task<CommandResult> RevokeCertificateAsync(X509RevokeCertificateCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509RevokeCertificateCommand.Result>();

        /// <summary>
        /// Unrevoke the X509 certificate asynchronously.
        /// </summary>
        public async Task<CommandResult> UnrevokeCertificateAsync(X509UnrevokeCertificateCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509UnrevokeCertificateCommand.Result>();

        /// <summary>
        /// Delete the X509 certificate asynchronously.
        /// </summary>
        public async Task<CommandResult> DeleteCertificateAsync(X509DeleteCertificateCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509DeleteCertificateCommand.Result>();

        /// <summary>
        /// List the X509 document asynchronously.
        /// </summary>
        public async Task<CommandResult> ListDocumentsAsync(X509ListDocumentCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509ListDocumentCommand.Result>();

        /// <summary>
        /// Read the X509 document asynchronously.
        /// </summary>
        public async Task<CommandResult> ReadDocumentAsync(X509ReadDocumentCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509ReadDocumentCommand.Result>();

        /// <summary>
        /// Write the X509 document asynchronously.
        /// </summary>
        public async Task<CommandResult> WriteDocumentAsync(X509WriteDocumentCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509WriteDocumentCommand.Result>();

        /// <summary>
        /// Remove the X509 document asynchronously.
        /// </summary>
        public async Task<CommandResult> RemoveDocumentAsync(X509RemoveDocumentCommand Command, CancellationToken Token = default)
            => (await Executor.Execute(Command, Token)).ToExpectedResult<X509RemoveDocumentCommand.Result>();
    }
}
