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
    public partial class FrmPfxToCrt : Form
    {
        public FrmPfxToCrt()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when "PFX" to "CRT" convert requested.
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
                var Bytes = m_Selector.Certificate.Export();
                using var Sfd = new SaveFileDialog()
                {
                    Title = "Select CRT Save Location",
                    Filter = "PKCS1 Certificate (*.cer, *.crt)|*.cer;*.crt"
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
