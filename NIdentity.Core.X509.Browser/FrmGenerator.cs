using NIdentity.Core.X509.Commands.Certificates;
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
    public partial class FrmGenerator : Form
    {
        public FrmGenerator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Command.
        /// </summary>
        public X509GenerateCommand Command { get; set; }

        /// <summary>
        /// Pfx Save Path.
        /// </summary>
        public string PfxSavePath { get; set; }

        /// <summary>
        /// Issuer.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Issuer Key Identifier.
        /// </summary>
        public string IssuerKeyIdentifier { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // ---
            Command = null;

            m_EditIssuer.Text = Issuer ?? string.Empty;
            m_EditKeyId.Text = IssuerKeyIdentifier ?? string.Empty;

            m_LstType.SelectedIndex = 2;
            m_LstAlgorithm.SelectedIndex = 0;
        }

        private void OnAddName(object sender, EventArgs e)
        {
            var Text = m_EditSansName.Text.Trim();
            if (string.IsNullOrWhiteSpace(Text))
                return;

            m_LstSans.Items.Add(Text);
        }

        private void OnRemoveName(object sender, EventArgs e)
        {
            if (m_LstSans.SelectedIndex < 0 &&
                m_LstSans.Items.Count <= m_LstSans.SelectedIndex)
                return;

            m_LstSans.Items.RemoveAt(m_LstSans.SelectedIndex);
        }

        private void OnSelectOutput(object sender, EventArgs e)
        {
            using var Sfd = new SaveFileDialog
            {
                Title = "Select PFX Save Location",
                Filter = 
                    "PKCS#12 Key Store (*.pfx)|*.pfx|" +
                    "PKCS8 Key Store (*.pem)|*.pem"
            };

            if (Sfd.ShowDialog() != DialogResult.OK)
                return;

            var PathName = Sfd.FileName;
            var IsPfx = PathName.EndsWith(".pfx");
            var IsPem = PathName.EndsWith(".pem");

            if (IsPem == false && IsPfx == false)
                PathName += ".pfx";

            m_LblPath.Text = PathName;
            PfxSavePath = PathName;
        }

        private void OnGenerate(object sender, EventArgs e)
        {
            if (m_LstType.SelectedIndex < 0)
            {
                Error("Error: please select type.");
                return;
            }

            if (m_LstAlgorithm.SelectedIndex < 0)
            {
                Error("Error: please select algorithm.");
                return;
            }

            if (m_LstPurposes.SelectedIndices.Count <= 0)
            {
                Error("Error: please select key purposes.");
                return;
            }

            var Name = m_EditName.Text;
            if (Name.Contains(','))
                Name = Name.Split(',').FirstOrDefault() ?? string.Empty;

            if (Name.Contains('='))
                Name = Name.Split('=').Skip(1).FirstOrDefault() ?? string.Empty;

            if ((Name = Name.Trim()).Length <= 0)
            {
                Error("Error: please write name correctly.");
                return;
            }

            Command = new X509GenerateCommand();
            Command.KeyType = (CertificateType)m_LstType.SelectedIndex;
            Command.Algorithm = m_LstType.SelectedText;

            foreach (var Each in m_LstPurposes.SelectedIndices)
                Command.Purposes |= (CertificatePurposes)Each;

            Command.Subject = $"CN={Name}";

            var Sans = new List<string>();
            foreach (var Each in m_LstSans.Items)
                Sans.Add(Each.ToString());

            Command.ExpirationHours = Math.Max((int)m_NumHours.Value, 1);
            Command.DnsNames = Sans.ToArray();
            Command.Issuer = m_EditIssuer.Text;
            Command.IssuerKeyIdentifier = m_EditKeyId.Text;
            Command.WithOcsp = m_ChkOcsp.Checked;
            Command.WithCrlDists = m_ChkCRL.Checked;
            Command.WithCAIssuers = m_ChkCAIssuers.Checked;

            DialogResult = DialogResult.OK;
        }

        private void Error(string Message) => MessageBox.Show(
            Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
