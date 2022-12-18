using NIdentity.Core.X509.Controls;
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
    public partial class FrmPfxToPfx : Form
    {
        public FrmPfxToPfx()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when "PFX" to "PFX" convert requested.
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
                using var Sfd = new SaveFileDialog()
                {
                    Title = "Select CRT Save Location",
                    Filter = "PKCS#12 Key Store (*.pfx)|*.pfx"
                };

                if (Sfd.ShowDialog() != DialogResult.OK)
                    return;

                while (true)
                {
                    var Password = null as string;
                    var Dr = MessageBox.Show(
                        "Do you want to set a password for the PFX file to be created?",
                        Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    if (Dr == DialogResult.Cancel)
                        return;

                    else if (Dr == DialogResult.Yes)
                    {
                        using var Prompt = new CertificatePasswordPrompt
                        {
                            IsRequired = true,
                            IsInput = false
                        };

                        if (Prompt.ShowDialog() != DialogResult.OK)
                        {
                            Dr = MessageBox.Show(
                                "Are you sure to proceed without password?",
                                Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (Dr == DialogResult.No)
                                return;

                            continue;
                        }

                        Password = Prompt.Password;
                    }

                    var Bytes = m_Selector.Certificate.ExportPfx(Password);
                    File.WriteAllBytes(Sfd.FileName, Bytes);
                    break;
                }

                Close();
            }

            catch
            {
                MessageBox.Show(
                    "Error: failed to save PFX file.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
