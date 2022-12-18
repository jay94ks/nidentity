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
    public partial class FrmPfxToPem : Form
    {
        public FrmPfxToPem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when "PFX" to "PEM" convert requested.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConvert(object sender, EventArgs e)
        {
            if (m_Selector.Certificate is null)
            {
                MessageBox.Show(
                    "Error: no certificate selected.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                var Bytes = m_Selector.Certificate.ExportPem();
                using var Sfd = new SaveFileDialog()
                {
                    Title = "Select PEM Save Location",
                    Filter = "PKCS8 Key Store (*.pem)|*.pem"
                };

                if (Sfd.ShowDialog() != DialogResult.OK)
                    return;

                File.WriteAllBytes(Sfd.FileName, Bytes);
                Close();
            }

            catch
            {
                MessageBox.Show(
                    "Error: failed to save PEM file.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
