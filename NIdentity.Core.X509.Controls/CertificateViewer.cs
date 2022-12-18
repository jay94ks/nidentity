using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIdentity.Core.X509.Controls
{
    public partial class CertificateViewer : Form
    {
        public CertificateViewer()
        {
            InitializeComponent();
        }

        // ---
        private Certificate m_Certificate;
        private Action<Certificate, CertificateRevokeReason, Action<Certificate>> m_RevokeHandler;
        private Action<Certificate, Action<Certificate>> m_UnrevokeHandler;
        private Action<Certificate, Action<bool>> m_DeleteHandler;

        /// <summary>
        /// Certificate.
        /// </summary>
        public Certificate Certificate
        {
            get => m_Certificate;
            set
            {
                certificateView1.Certificate = value;
                if ((m_Certificate = value) != null)
                {
                    Text = value != null
                        ? $"{value.Subject} ({value.KeyIdentifier})"
                        : "Certificate Viewer"
                        ;
                }

                else
                {
                    Text = "Certificate Viewer";
                }

                SetHandler(null);
            }
        }

        /// <summary>
        /// Revoke Handler.
        /// </summary>
        public Action<Certificate, CertificateRevokeReason, Action<Certificate>> RevokeHandler
        {
            get => m_RevokeHandler;
            set => SetHandler(() => m_RevokeHandler = value);
        }

        /// <summary>
        /// Unrevoke Handler.
        /// </summary>
        public Action<Certificate, Action<Certificate>> UnrevokeHandler
        {
            get => m_UnrevokeHandler;
            set => SetHandler(() => m_UnrevokeHandler = value);
        }

        /// <summary>
        /// Delete Handler.
        /// </summary>
        public Action<Certificate, Action<bool>> DeleteHandler
        {
            get => m_DeleteHandler;
            set => SetHandler(() => m_DeleteHandler = value);
        }

        /// <summary>
        /// Reload Handler.
        /// </summary>
        public Action<Certificate> ReloadHandler { get; set; }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetHandler(null);
        }

        /// <summary>
        /// Set handler and reset button visible status.
        /// </summary>
        /// <param name="Handler"></param>
        private void SetHandler(Action Action)
        {
            Action?.Invoke();

            var Revokable = m_RevokeHandler != null;
            var Unrevokable = m_UnrevokeHandler != null;
            var Deletable = m_DeleteHandler != null;

            if (m_Certificate != null)
            {
                Revokable = Revokable && m_Certificate.IsRevokeIdentified == false;
                Unrevokable = Unrevokable && m_Certificate.IsRevokeIdentified == true;
            }

            m_RevokeReason.Visible = Revokable;
            m_Revoke.Visible = Revokable;

            m_Unrevoke.Visible = Unrevokable;
            m_Delete.Visible = Deletable;

            panel1.Visible = 
                Revokable == true ||
                Unrevokable == true ||
                Deletable == true;
        }

        /// <summary>
        /// Called when "Revoke" button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRevoke(object sender, EventArgs e)
        {
            if (m_RevokeReason.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Error: select the revoke reason before clicking this button.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            var Reason = (CertificateRevokeReason)m_RevokeReason.SelectedIndex;
            m_RevokeHandler?.Invoke(Certificate, Reason, X =>
            {
                Certificate = X; 
                ReloadHandler?.Invoke(m_Certificate);
            });
        }

        /// <summary>
        /// Called when "Unrevoke" button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnrevoke(object sender, EventArgs e)
        {
            m_UnrevokeHandler?.Invoke(m_Certificate, X =>
            {
                Certificate = X;
                ReloadHandler?.Invoke(m_Certificate);
            });
        }

        /// <summary>
        /// Called when "Delete" button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDelete(object sender, EventArgs e)
        {
            var Confirm = MessageBox.Show(
                "This behavior is irreversible. Would you like to proceed?",
                Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (Confirm != DialogResult.OK)
                return;

            m_DeleteHandler?.Invoke(m_Certificate, X =>
            {
                ReloadHandler?.Invoke(m_Certificate);
                Close();
            });
        }
    }
}
