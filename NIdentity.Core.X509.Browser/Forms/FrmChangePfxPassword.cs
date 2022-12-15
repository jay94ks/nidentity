using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIdentity.Core.X509.Browser.Forms
{
    public partial class FrmChangePfxPassword : Form
    {
        public FrmChangePfxPassword()
        {
            InitializeComponent();
        }

        private CertificateStore m_Source;
        private string m_Destination;

        private void m_LblAuthority_Click(object sender, EventArgs e)
        {
            using var Ofd = new OpenFileDialog
            {
                Title = "Select PFX Source Location",
                Filter =
                    "PKCS#12 Key Store (*.pfx)|*.pfx|" +
                    "PKCS8 Key Store (*.pem)|*.pem"
            };

            if (Ofd.ShowDialog() != DialogResult.OK)
                return;

            if (!File.Exists(Ofd.FileName))
            {
                MessageBox.Show(
                    $"Error: no such file exits: {Ofd.FileName}.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                var PfxPass = textBox1.Text;
                var PfxBytes = File.ReadAllBytes(Ofd.FileName);

                m_Source = CertificateStore.Import(PfxBytes, PfxPass);
                m_LblSource.Text = Ofd.FileName;
            }

            catch
            {
                MessageBox.Show(
                    $"Error: failed to load source pfx file: {Ofd.FileName}.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            using var Sfd = new SaveFileDialog
            {
                Title = "Select PFX Destination Location",
                Filter =
                    "PKCS#12 Key Store (*.pfx)|*.pfx|" +
                    "PKCS8 Key Store (*.pem)|*.pem"
            };

            if (Sfd.ShowDialog() != DialogResult.OK)
                return;

            m_Destination = Sfd.FileName;
            m_LblDestination.Text = Sfd.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var DestPass = textBox2.Text;
            if (m_Source is null)
            {
                MessageBox.Show(
                    $"Error: no source pfx file loaded.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (string.IsNullOrWhiteSpace(m_Destination))
            {
                MessageBox.Show(
                    $"Error: no destination pfx specified.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                var PfxBytes = m_Source.Export(DestPass);
                File.WriteAllBytes(m_Destination, PfxBytes);
                DialogResult = DialogResult.OK;
            }

            catch
            {
                MessageBox.Show(
                    $"Error: failed to generate destination pfx file: {m_Destination}.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
