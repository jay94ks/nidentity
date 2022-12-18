using NIdentity.Connector.X509;
using NIdentity.Core.X509.Controls.Internals;

namespace NIdentity.Core.X509.Controls
{
    /// <summary>
    /// Certificate tree viewer.
    /// </summary>
    public class CertificateTreeView : TreeView
    {
        private InstrusiveWorker m_Worker;
        private X509CommandExecutor m_X509;

        private Certificate[] m_Certificates;
        private Dictionary<CertificateIdentity, TreeNode> m_Nodes = new();
       
        /// <summary>
        /// Initialize a new <see cref="CertificateTreeView"/> instance.
        /// </summary>
        public CertificateTreeView()
        {
            FullRowSelect = true;
            HideSelection = false;
        }

        /// <summary>
        /// Triggered when the loading started.
        /// </summary>
        public event Action<CertificateTreeView> LoadStarted;

        /// <summary>
        /// Triggered when the loading ended.
        /// </summary>
        public event Action<CertificateTreeView> LoadEnded;

        /// <summary>
        /// Triggered when a certificate selected.
        /// </summary>
        public event Action<CertificateTreeView, Certificate> Selected;

        /// <summary>
        /// Authority certificate.
        /// </summary>
        public Certificate Authority { get; set; }

        /// <summary>
        /// All certificates.
        /// </summary>
        public Certificate[] Certificates
        {
            get
            {
                if (m_Certificates is null)
                    m_Certificates = Iterate(Nodes).ToArray();

                return m_Certificates;
            }
        }

        /// <summary>
        /// Set the worker who execute commands to load X509 certificates from NIdentity host.
        /// </summary>
        /// <param name="Worker"></param>
        /// <returns></returns>
        public CertificateTreeView SetWorker(InstrusiveWorker Worker)
        {
            if (m_Worker != null)
                m_Worker?.Dispose();

            m_Worker = Worker;
            return this;
        }

        /// <summary>
        /// Set the executor that is used to execute commands.
        /// </summary>
        /// <param name="Executor"></param>
        /// <returns></returns>
        public CertificateTreeView SetExecutor(ICommandExecutor Executor)
        {
            if (Executor is null)
            {
                m_X509 = null;
                return this;
            }

            m_X509 = new X509CommandExecutor(Executor);
            return this;
        }

        /// <summary>
        /// Enumerates all certificates.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Certificate> Iterate(TreeNodeCollection Nodes)
        {
            for (var i = 0; i < Nodes.Count; ++i)
            {
                var Node = Nodes[i];
                if (Node is null || Node.Tag is not Certificate Cert)
                    continue;

                yield return Cert;
                foreach (var Each in Iterate(Node.Nodes))
                    yield return Each;
            }
        }

        /// <summary>
        /// Select a node by certificate.
        /// </summary>
        /// <param name="Certificate"></param>
        public bool Select(Certificate Certificate)
        {
            if (Certificate is null)
                return false;

            m_Nodes.TryGetValue(Certificate.Self, out var Node);
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
            if (Authority is null)
            {
                Nodes.Clear();
                m_Nodes.Clear();
                return;
            }

            if (m_X509 is null)
                return;

            Nodes.Clear();
            m_Nodes.Clear();
            LoadMore(Authority);
        }

        /// <summary>
        /// Reload about the certificate.
        /// </summary>
        /// <param name="Certificate"></param>
        public void Reload(Certificate Certificate)
        {
            if (Authority is null)
            {
                Nodes.Clear();
                m_Nodes.Clear();
                return;
            }

            if (m_X509 is null)
                return;

            LoadMore(Certificate);
        }

        /// <inheritdoc/>
        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            if (Authority is null)
                return;

            if (e.Node is null || e.Node.Tag is not Certificate Certificate)
            {
                base.OnAfterSelect(e);
                return;
            }

            LoadMore(Certificate);
            base.OnAfterSelect(e);

            Selected?.Invoke(this, Certificate);
        }

        /// <summary>
        /// Load more for the tree.
        /// </summary>
        /// <param name="Cert"></param>
        private void LoadMore(Certificate Cert)
        {
            try { Invoke(() => LoadStarted?.Invoke(this)); }
            catch
            {
            }

            if (m_Nodes.TryGetValue(Cert.Self, out var AthNode) == false)
            {
                AthNode = Nodes.Add(Cert.Subject);
                AthNode.Text = Cert.Subject;
                AthNode.Tag = Cert;
            }

            else
            {
                AthNode.Text = Cert.Subject;
                AthNode.Tag = Cert;
            }

            m_Nodes[Cert.Self] = AthNode;

            if (m_Worker is null)
                m_Worker = new InstrusiveWorker();

            m_Worker.Enqueue(Token => LoadMoreAsync(Cert, Token));
        }

        /// <summary>
        /// Load more for the tree.
        /// </summary>
        /// <param name="Certificate"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        private async Task LoadMoreAsync(Certificate Certificate, CancellationToken Token)
        {
            var Offset = 0;
            while (true)
            {
                if (Token.IsCancellationRequested)
                    break;

                var RemoveFromList = false;
                try
                {
                    var Info = await m_X509.GetCertificateMetaAsync(Certificate, Token);
                    if (Info is null)
                        RemoveFromList = true;
                }
                catch { RemoveFromList = true; }

                if (RemoveFromList)
                {
                    Invoke(() =>
                    {
                        try
                        {
                            m_Nodes.TryGetValue(Certificate.Self, out var Target);
                            Target?.Remove();
                        }
                        catch { }
                    });
                    break;
                }

                try
                {
                    var Items = await m_X509.ListCertificatesAsync(Certificate, Offset, 20, Token);
                    if (Items is null || Items.Count <= 0)
                        break;

                    Offset += Items.Count;
                    Invoke(() =>
                    {
                        try { PutNodes(Items); }
                        catch
                        {
                        }
                    });
                }

                catch
                {
                    break;
                }
            }

            try
            {
                Invoke(() =>
                {
                    m_Certificates = null;
                    LoadEnded?.Invoke(this);
                });
            }
            catch
            {
            }
        }

        /// <summary>
        /// Put more certificates to the tree.
        /// </summary>
        /// <param name="Cert"></param>
        /// <param name="Items"></param>
        private void PutNodes(X509CertificateCollection Items)
        {
            foreach (var Each in Items)
            {
                if (m_Nodes.TryGetValue(Each.Issuer, out var Parent) == false)
                    continue;

                if (m_Nodes.TryGetValue(Each.Self, out var Node))
                {
                    Node.Text = Each.Subject;
                    Node.Tag = Each;
                    continue;
                }

                Node = Parent.Nodes.Add(Each.Subject);
                Node.Text = Each.Subject;
                Node.Tag = Each;

                m_Nodes[Each.Self] = Node;
            }
        }
    }
}
