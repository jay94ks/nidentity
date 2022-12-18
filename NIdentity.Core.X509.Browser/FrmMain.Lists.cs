using NIdentity.Connector.X509;
using NIdentity.Core.X509.Controls;
using NIdentity.Core.X509.Controls.Internals;

namespace NIdentity.Core.X509.Browser
{
    public partial class FrmMain
    {
        private int m_Loads = 0;

        /// <summary>
        /// Initialize lists.
        /// </summary>
        private void InitializeLists()
        {
            m_CertTree.LoadStarted += OnLoadStarted;
            m_CertList.LoadStarted += OnLoadStarted;

            m_CertTree.LoadEnded += OnLoadEnded;
            m_CertList.LoadEnded += OnLoadEnded;

            m_CertTree.Selected += OnSelected;

            m_CertTree.MouseDoubleClick += OnTreeDoubleClick;
            m_CertList.MouseDoubleClick += OnDoubleClick;
        }

        /// <summary>
        /// Reset the list.
        /// </summary>
        private void ResetLists()
        {
            m_CertList.Authority = null;
            m_CertTree.Authority = null;

            m_CertList.Reload();
            m_CertTree.Reload();
        }

        /// <summary>
        /// Called when the certificate load started.
        /// </summary>
        /// <param name="Control"></param>
        private void OnLoadStarted(Control Control)
        {
            m_WindowSplitter.Enabled = (++m_Loads) > 0;
        }

        /// <summary>
        /// Called when the certificate load ended.
        /// </summary>
        /// <param name="Control"></param>
        private void OnLoadEnded(Control Control)
        {
            m_WindowSplitter.Enabled = (--m_Loads) <= 0;
        }

        /// <summary>
        /// Called when the certificate selected.
        /// </summary>
        /// <param name="Control"></param>
        /// <param name="Certificate"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnSelected(Control Control, Certificate Certificate)
        {
            m_MenuGenerate.Enabled = Certificate.Type != CertificateType.Leaf;
            m_MenuRevoke.Enabled = true;
            m_MenuUnrevoke.Enabled = true;
            m_MenuDelete.Enabled = true;

            m_CertList.Authority = Certificate;
            m_CertList.Reload();

            m_PropGrid.SelectedObject = Certificate;
        }

        /// <summary>
        /// Called when user double-click the certificate on listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void OnDoubleClick(object sender, MouseEventArgs e)
        {
            if (m_CertList.SelectedItems.Count <= 0)
                return;

            if (m_CertList.SelectedItems[0].Tag is not Certificate Selected)
                return;

            var Viewer = new CertificateViewer
            {
                Certificate = Selected,
                RevokeHandler = OnHandleRevoke,
                UnrevokeHandler = OnHandleUnrevoke,
                DeleteHandler = OnHandleDelete,
                ReloadHandler = (Cert) =>
                {
                    m_CertTree.Reload();
                    m_CertList.Reload();
                }
            };

            Viewer.Show();
        }

        /// <summary>
        /// Called when user double-click the certificate on treeview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTreeDoubleClick(object sender, MouseEventArgs e)
        {
            if (m_CertTree.SelectedNode is null)
                return;

            if (m_CertTree.SelectedNode.Tag is not Certificate Selected)
                return;

            var Viewer = new CertificateViewer
            {
                Certificate = Selected,
                RevokeHandler = OnHandleRevoke,
                UnrevokeHandler = OnHandleUnrevoke,
                DeleteHandler = OnHandleDelete,
                ReloadHandler = (Cert) =>
                {
                    m_CertTree.Reload();
                    m_CertList.Reload();
                }
            };

            Viewer.Show();
        }
    }

}
