using NIdentity.Connector.X509;
using NIdentity.Core.X509.Controls.Internals;
using NIdentity.Core.X509.Documents;

namespace NIdentity.Core.X509.Controls
{
    /// <summary>
    /// Certificate document tree viewer.
    /// </summary>
    public class CertificateDocumentTreeView : TreeView
    {
        private InstrusiveWorker m_Worker;
        private X509CommandExecutor m_X509;

        private Document[] m_Documents;
        private Dictionary<DocumentIdentity, TreeNode> m_Nodes = new();

        /// <summary>
        /// Initialize a new <see cref="CertificateDocumentTreeView"/> instance.
        /// </summary>
        public CertificateDocumentTreeView()
        {
            FullRowSelect = true;
            HideSelection = true;
        }

        /// <summary>
        /// Triggered when the loading started.
        /// </summary>
        public event Action<CertificateDocumentTreeView> LoadStarted;

        /// <summary>
        /// Triggered when the loading ended.
        /// </summary>
        public event Action<CertificateDocumentTreeView> LoadEnded;

        /// <summary>
        /// Owner certificate.
        /// </summary>
        public Certificate Owner { get; set; }

        /// <summary>
        /// All certificates.
        /// </summary>
        public Document[] Documents
        {
            get
            {
                if (m_Documents is null)
                    m_Documents = Iterate(Nodes).ToArray();

                return m_Documents;
            }
        }

        /// <summary>
        /// Enumerates all certificates.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Document> Iterate(TreeNodeCollection Nodes)
        {
            for (var i = 0; i < Nodes.Count; ++i)
            {
                var Node = Nodes[i];
                if (Node is null || Node.Tag is not Document Document)
                    continue;

                yield return Document;
                foreach (var Each in Iterate(Node.Nodes))
                    yield return Each;
            }
        }

        /// <summary>
        /// Select a node by certificate.
        /// </summary>
        /// <param name="Certificate"></param>
        public bool Select(Document Document)
        {
            if (Document is null)
                return false;

            m_Nodes.TryGetValue(Document.Identity, out var Node);
            if (Node != null)
            {
                if (Node.IsSelected == true)
                    return true;

                SelectedNode = Node;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reload the tree view.
        /// </summary>
        public void Reload()
        {
            if (Owner is null)
            {
                Nodes.Clear();
                m_Nodes.Clear();
                return;
            }

            if (m_X509 is null)
                return;

            m_Worker.Enqueue(async Token =>
            {
                try
                {
                    var Document = await m_X509.ReadDocumentAsync(Owner.Self, "/", null, Token);
                    if (Document != null) Invoke(() => AddDocToTree(Document));
                }
                catch { }
            });
        }

        /// <summary>
        /// Set the worker who execute commands to load X509 certificates from NIdentity host.
        /// </summary>
        /// <param name="Worker"></param>
        /// <returns></returns>
        public CertificateDocumentTreeView SetWorker(InstrusiveWorker Worker)
        {
            if (m_Worker != Worker)
                m_Worker.Dispose();

            m_Worker = Worker;
            return this;
        }

        /// <summary>
        /// Set the executor that is used to execute commands.
        /// </summary>
        /// <param name="Executor"></param>
        /// <returns></returns>
        public CertificateDocumentTreeView SetExecutor(ICommandExecutor Executor)
        {
            if (Executor is null)
            {
                m_X509 = null;
                return this;
            }

            m_X509 = new X509CommandExecutor(Executor);
            return this;
        }

        /// <inheritdoc/>
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (e.Node is null || e.Node.Tag is not Document Document)
            {
                base.OnAfterSelect(e);
                return;
            }

            AddDocToTree(Document);
            base.OnAfterSelect(e);
        }

        /// <summary>
        /// Load more for the tree.
        /// </summary>
        /// <param name="Cert"></param>
        private void AddDocToTree(Document Document)
        {
            try { Invoke(() => LoadStarted?.Invoke(this)); }
            catch
            {
            }

            var PathName = DocumentIdentity.NormalizePathName(Document.Identity.PathName);
            if (m_Nodes.TryGetValue(Document.Identity, out var AthNode) == false)
                AthNode = Nodes.Add($"/{PathName}");

            AthNode.Text = $"/{PathName}";
            AthNode.Tag = Document;

            m_Nodes[Document.Identity] = AthNode;

            if (m_Worker is null)
                m_Worker = new InstrusiveWorker();

            m_Worker.Enqueue(Token => AddDocToTreeAsync(Document, Token));
        }

        /// <summary>
        /// Load more for the tree.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        private async Task AddDocToTreeAsync(Document Document, CancellationToken Token)
        {
            try
            {
                var Owner = Document.Identity.Owner;
                var PathName = DocumentIdentity.NormalizePathName(Document.Identity.PathName);
                var Items = await m_X509.ListDocumentsAsync(Owner, PathName, Token);
                if (Items is null || Items.Documents is null)
                    return;

                var List = new List<Document>();
                foreach(var Each in Items.Documents)
                {
                    var PathEach = DocumentIdentity.NormalizePathName(Each);
                    var DocEach = await m_X509.ReadDocumentAsync(Owner, PathEach, null, Token);
                    if (m_Nodes.TryGetValue(DocEach.Identity, out var Parent) == false)
                        continue;

                    try
                    {
                        Invoke(() =>
                        {
                            try { AddDocToTree(PathName, PathEach, DocEach, Parent); }
                            catch
                            {
                            }
                        });
                    }
                    catch
                    {
                    }
                }
            }

            catch
            {
            }

            try
            {
                Invoke(() =>
                {
                    m_Documents = null;
                    LoadEnded?.Invoke(this);
                });
            }
            catch
            {
            }
        }
        
        /// <summary>
        /// Add a document to tree.
        /// </summary>
        /// <param name="PathName"></param>
        /// <param name="PathEach"></param>
        /// <param name="DocEach"></param>
        /// <param name="Parent"></param>
        private void AddDocToTree(string PathName, string PathEach, Document DocEach, TreeNode Parent)
        {
            var DocName = PathEach.Substring(PathName.Length).Trim('/');
            if (m_Nodes.TryGetValue(DocEach.Identity, out var Node))
            {
                Node.Text = DocName;
                Node.Tag = DocEach;
                return;
            }

            Node = Parent.Nodes.Add($"/{DocName}");
            Node.Text = $"/{DocName}";
            Node.Tag = DocEach;

            m_Nodes[DocEach.Identity] = Node;
        }
    }
}
