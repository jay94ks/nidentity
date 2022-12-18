using NIdentity.Connector;
using NIdentity.Connector.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIdentity.Core.X509.Browser
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        // -----------------
        private RemoteCommandExecutorParameters m_Parameters;
        private RemoteCommandExecutor m_Executor;
        private X509CommandExecutor m_X509;

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
         
            InitializeMenus();
            InitializeLists();
        }

        /// <summary>
        /// Called when the certificate revoke button clicked.
        /// </summary>
        /// <param name="Cert"></param>
        /// <param name="Reason"></param>
        /// <param name="Callback"></param>
        private void OnHandleRevoke(Certificate Cert, CertificateRevokeReason Reason, Action<Certificate> Callback)
            => m_Worker.Enqueue(async Token =>
            {
                try
                {
                    if (await m_X509.RevokeCertificateAsync(Cert, Reason, Token))
                    {
                        Cert.RevokeReason = Reason;
                        Cert.RevokeTime = DateTimeOffset.UtcNow;
                    }
                }

                catch(Exception Error)
                {
                    Invoke(() => ShowError(Error));
                    return;
                }

                Invoke(() => Callback?.Invoke(Cert));
            });

        /// <summary>
        /// Called when the certificate unrevoke button clicked.
        /// </summary>
        /// <param name="Cert"></param>
        /// <param name="Callback"></param>
        private void OnHandleUnrevoke(Certificate Cert, Action<Certificate> Callback)
            => m_Worker.Enqueue(async Token =>
            {
                try
                {
                    if (await m_X509.UnrevokeCertificateAsync(Cert, Token))
                    {
                        Cert.RevokeReason = null;
                        Cert.RevokeTime = null;
                    }
                }

                catch (Exception Error)
                {
                    Invoke(() => ShowError(Error));
                    return;
                }

                Invoke(() => Callback?.Invoke(Cert));
            });

        /// <summary>
        /// Called when the certificate delete button clicked.
        /// </summary>
        /// <param name="Cert"></param>
        /// <param name="Callback"></param>
        private void OnHandleDelete(Certificate Cert, Action<bool> Callback) 
            => m_Worker.Enqueue(async Token =>
            {
                var State = false;
                try { State = await m_X509.DeleteCertificateAsync(Cert, Token); }
                catch (Exception Error)
                {
                    Invoke(() => ShowError(Error));
                    return;
                }

                Invoke(() => Callback?.Invoke(State));
            });

        /// <summary>
        /// Show an error.
        /// </summary>
        /// <param name="Error"></param>
        private void ShowError(Exception Error)
        {
            MessageBox.Show($"Error: {Error.Message}.",
                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

}
