using NIdentity.Connector.X509;
using NIdentity.Core.X509.Controls.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIdentity.Core.X509.Controls
{
    /// <summary>
    /// Certificate List View.
    /// </summary>
    public class CertificateListView : ListView
    {
        private InstrusiveWorker m_Worker;
        private X509CommandExecutor m_X509;

        private Certificate[] m_Certificates;

        /// <summary>
        /// Initialize a new <see cref="CertificateTreeView"/> instance.
        /// </summary>
        public CertificateListView()
        {
            FullRowSelect = true;
            HideSelection = false;
            View = View.Details;

            AddColumns();
        }

        /// <summary>
        /// Add columns.
        /// </summary>
        private void AddColumns()
        {
            Columns.Add("KeySHA1");
            Columns.Add("Subject");
            Columns.Add("Issuer");
            Columns.Add("Serial Number");
            Columns.Add("Thumbprint");
            Columns.Add("Creation Time");
            Columns.Add("Expiration Time");
            Columns.Add("Revoke Reason");
            Columns.Add("Key Identifier");
            Columns.Add("Issuer Key Identifier");
        }

        /// <summary>
        /// Triggered when the loading started.
        /// </summary>
        public event Action<CertificateListView> LoadStarted;

        /// <summary>
        /// Triggered when the loading ended.
        /// </summary>
        public event Action<CertificateListView> LoadEnded;

        /// <summary>
        /// Triggered when a certificate selected.
        /// </summary>
        public event Action<CertificateListView, Certificate> Selected;

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
                    m_Certificates = Iterate().ToArray();

                return m_Certificates;
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

            for(var i = 0; i < Items.Count; ++i)
            {
                var Item = Items[i];
                if (Item.Tag is not Certificate Each)
                    continue;

                if (Each.Self.IsExact(Certificate) == false)
                    continue;

                SelectedIndices.Clear();
                SelectedIndices.Add(i);

                TriggerSelectedEvent();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Reload the list view.
        /// </summary>
        public void Reload()
        {
            if (Authority is null)
            {
                Items.Clear();
                return;
            }

            if (m_X509 is null)
                return;

            Items.Clear();
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
                Items.Clear();
                return;
            }

            if (m_X509 is null)
                return;

            LoadMore(Certificate);
        }

        /// <summary>
        /// Set the worker who execute commands to load X509 certificates from NIdentity host.
        /// </summary>
        /// <param name="Worker"></param>
        /// <returns></returns>
        public CertificateListView SetWorker(InstrusiveWorker Worker)
        {
            if (m_Worker != null)
                m_Worker.Dispose();

            m_Worker = Worker;
            return this;
        }

        /// <summary>
        /// Set the executor that is used to execute commands.
        /// </summary>
        /// <param name="Executor"></param>
        /// <returns></returns>
        public CertificateListView SetExecutor(ICommandExecutor Executor)
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
        /// Called when selected index changed.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            if (Authority is null)
                return;

            TriggerSelectedEvent();
        }

        /// <summary>
        /// Trigger the `<see cref="Selected"/>` event.
        /// </summary>
        private void TriggerSelectedEvent()
        {
            if (SelectedIndices.Count <= 0)
                return;

            var Index = SelectedIndices[0];
            if (Items.Count <= Index || Index < 0) 
                return;

            Selected?.Invoke(this, Items[Index].Tag as Certificate);
        }

        /// <summary>
        /// Enumerates all certificates.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Certificate> Iterate()
        {
            for (var i = 0; i < Items.Count; ++i)
            {
                var Node = Items[i];
                if (Node is null || Node.Tag is not Certificate Cert)
                    continue;

                yield return Cert;
            }
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
                            for (int i = 0; i < Items.Count; ++i)
                            {
                                if (Items[i].Tag is Certificate Cert)
                                {
                                    if (Cert.Self.IsExact(Certificate) == false)
                                        continue;

                                    Items.RemoveAt(i);
                                    break;
                                }
                            }
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
                        try { PutItems(Items); }
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
        private void PutItems(X509CertificateCollection Items)
        {
            this.Items.Clear();
            foreach (var Each in Items)
            {
                if (Each.Self.IsExact(Authority))
                    continue;

                var Item = this.Items.Add(Each.KeySHA1);

                Item.SubItems.Add(Each.Subject);
                Item.SubItems.Add(Each.Issuer.Subject);
                Item.SubItems.Add(Each.SerialNumber);
                Item.SubItems.Add(Each.Thumbprint);
                Item.SubItems.Add(Each.CreationTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
                Item.SubItems.Add(Each.ExpirationTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));
                Item.SubItems.Add(Each.RevokeReason.HasValue ? Each.RevokeReason.ToString() : "");
                Item.SubItems.Add(Each.KeyIdentifier);
                Item.SubItems.Add(Each.Issuer.KeyIdentifier);

                Item.Tag = Each;
            }
        }
    }
}
