using Microsoft.EntityFrameworkCore;
using NIdentity.Core.X509.Documents;
using NIdentity.Core.X509.Server.Documents;
using NIdentity.Core.X509.Server.Repositories.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace NIdentity.Core.X509.Server.Repositories
{
    /// <summary>
    /// X509 Document Repository.
    /// </summary>
    public class X509DocumentRepository : IMutableDocumentRepository
    {
        private X509Context m_X509Context;

        /// <summary>
        /// Initialize a new <see cref="X509Repository"/> instance.
        /// </summary>
        /// <param name="X509Context"></param>
        public X509DocumentRepository(X509Context X509Context) => m_X509Context = X509Context;

        /// <inheritdoc/>
        public Task<DocumentList> ListAsync(CertificateReference Owner, string PathName = "/", CancellationToken Token = default)
        {
            PathName = DocumentIdentity.NormalizePathName(PathName);

            var RefSHA1 = Owner.MakeRefSHA1();
            var Children = m_X509Context.Documents
                .Where(X => X.RefSHA1 == RefSHA1)
                .Where(X => X.PathName != PathName)
                .Where(X => X.ParentPathName == PathName);

            var Docs = Children.Select(X => X.PathName).ToArray();
            var DocList = new DocumentList()
            {
                PathName = PathName,
                Documents = Docs
            };

            return Task.FromResult(DocList);
        }

        /// <inheritdoc/>
        public Task<Document> ReadAsync(DocumentIdentity Identity, long? RevisionNumber = null, CancellationToken Token = default)
        {
            var DbDoc = m_X509Context.GetCertificateDocument(Identity, RevisionNumber);
            if (DbDoc != null)
            {
                var Doc = new Document
                {
                    Identity = Identity,
                    Access = DbDoc.Access,
                    CreationTime = DbDoc.CreationTime,
                    LastWriteTime = DbDoc.LastWriteTime,
                    RevisionNumber = DbDoc.RevisionNumber,
                    MimeType = DbDoc.MimeType,
                    HasChildren = DbDoc.HasChildren,
                    Data = DbDoc.Data
                };

                return Task.FromResult(Doc);
            }

            return Task.FromResult<Document>(null);
        }

        /// <inheritdoc/>
        public async Task<bool> WriteAsync(Document Document, CancellationToken Token = default)
        {
            if (Document is null)
                throw new ArgumentNullException(nameof(Document));

            var DbCert = m_X509Context.GetCertificate(Document.Identity.Owner);
            if (DbCert is null)
                return false;

            return await WriteAsync(Document, DbCert);
        }

        /// <summary>
        /// Real implementation of WriteAsync.
        /// </summary>
        /// <param name="Document"></param>
        /// <param name="DbCert"></param>
        /// <returns></returns>
        private async Task<bool> WriteAsync(Document Document, DbCertificate DbCert)
        {
            var DbDoc = m_X509Context.GetCertificateDocument(Document.Identity, Document.RevisionNumber);
            if (DbDoc is null)
            {
                var PathName = DocumentIdentity.NormalizePathName(Document.Identity.PathName);
                var Parent = string.Join("/", PathName.Split('/').SkipLast(1));

                DbDoc = new DbCertificateDocument
                {
                    KeySHA1 = DbCert.KeySHA1,
                    RefSHA1 = DbCert.RefSHA1,
                    PathName = PathName,
                    ParentPathName = Parent,
                    Access = Document.Access,
                    CreationTime = DateTimeOffset.UtcNow,
                    LastWriteTime = DateTimeOffset.UtcNow,
                    RevisionNumber = 0,
                    MimeType = Document.MimeType,
                    Data = Document.Data ?? string.Empty
                };

                if (m_X509Context.Create(DbDoc))
                {
                    await EnsureParentsChildren(Document, DbCert, PathName);
                    return true;
                }

                return false;
            }

            DbDoc.Access = Document.Access;
            DbDoc.LastWriteTime = DateTimeOffset.UtcNow;
            DbDoc.RevisionNumber = DbDoc.RevisionNumber + 1;
            DbDoc.MimeType = Document.MimeType;
            DbDoc.Data = Document.Data ?? string.Empty;

            await EnsureParentsChildren(Document, DbCert, Document.Identity.PathName);
            return m_X509Context.Update(DbDoc);
        }

        /// <summary>
        /// Ensure parents.
        /// </summary>
        /// <param name="Document"></param>
        /// <param name="DbCert"></param>
        /// <param name="PathName"></param>
        private async Task EnsureParentsChildren(Document Document, DbCertificate DbCert, string PathName)
        {
            PathName = DocumentIdentity.NormalizePathName(Document.Identity.PathName);
            var Parent = string.Join("/", PathName.Split('/').SkipLast(1));

            while (PathName.Length > 0)
            {
                var Grandparent = string.Join("/", Parent.Split('/').SkipLast(1));
                var ParentId = new DocumentIdentity(Document.Identity.Owner, Parent);

                var ParentDocument = m_X509Context.GetCertificateDocument(ParentId);
                if (ParentDocument is null)
                {
                    var Success = await WriteAsync(new Document
                    {
                        Identity = ParentId,
                        Access = Document.Access,
                        MimeType = "text/plain",
                        Data = string.Empty
                    }, DbCert);

                    if (!Success)
                        continue;
                }

                else if (!ParentDocument.HasChildren)
                {
                    ParentDocument.HasChildren = true;
                    m_X509Context.Update(ParentDocument);
                }

                if ((Parent = Grandparent).Length <= 0)
                    break;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> RemoveAsync(DocumentIdentity Identity, long? RevisionNumber = null, CancellationToken Token = default)
        {
            var DbDoc = m_X509Context.GetCertificateDocument(Identity, RevisionNumber);
            if (DbDoc != null)
            {
                var KeySHA1 = Identity.Owner.MakeKeySHA1();
                var PathName = DocumentIdentity.NormalizePathName(Identity.PathName);
                var ParentPathName = string.Join("/", PathName.Split('/').SkipLast(1));

                // --> delete children.
                var Children = m_X509Context.Documents
                    .Where(X => X.KeySHA1 == KeySHA1)
                    .Where(X => X.PathName != PathName)
                    .Where(X => X.ParentPathName == PathName)
                    .Select(X => X.PathName).ToArray();

                foreach (var Each in Children)
                {
                    if (!await RemoveAsync(new DocumentIdentity(Identity.Owner, Each), null, Token))
                        return false;
                }

                // --> update parents.
                if (!UpdateParentDocument(new DocumentIdentity(Identity.Owner, ParentPathName)))
                    return false;

                return m_X509Context.Remove(DbDoc);
            }

            return false;
        }

        /// <summary>
        /// Update parent's <see cref="DbCertificateDocument.HasChildren"/>.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        private bool UpdateParentDocument(DocumentIdentity Identity)
        {
            var Document = m_X509Context.GetCertificateDocument(Identity, null);
            if (Document != null)
            {
                var PathName = DocumentIdentity.NormalizePathName(Identity.PathName);
                var KeySHA1 = Identity.Owner.MakeKeySHA1();

                var HasChildren = m_X509Context.Documents
                    .Where(X => X.KeySHA1 == KeySHA1)
                    .Where(X => X.PathName != PathName)
                    .Where(X => X.ParentPathName == PathName)
                    .Count() - 1 > 0;

                if (Document.HasChildren != HasChildren)
                {
                    Document.HasChildren = HasChildren;
                    return m_X509Context.Update(Document);
                }

                return true;
            }

            return false;
        }
    }
}
