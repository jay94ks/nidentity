using NIdentity.Core.X509.Documents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIdentity.Core.X509.Browser.Forms.Docs
{
    public partial class FrmNewDocument : Form
    {
        public FrmNewDocument()
        {
            InitializeComponent();
        }

        public Document Document { get; set; } = new();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            m_EditOwner.Text = Document.Identity.Owner.Subject ?? string.Empty;
            m_EditOwnerKeyId.Text = Document.Identity.Owner.KeyIdentifier ?? string.Empty;
            m_EditPathName.Text = Document.Identity.PathName ?? string.Empty;
            m_LstAccess.SelectedIndex = (int) Document.Access;
            m_EditMimeType.Text = Document.MimeType ?? "text/plain";
            m_EditData.Text = Document.Data ?? string.Empty;
        }

        private void button2_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
        private void button1_Click(object sender, EventArgs e)
        {
            Document.Identity = new DocumentIdentity(
                Document.Identity.Owner, m_EditPathName.Text);

            Document.MimeType = m_EditMimeType.Text;
            Document.Access = (DocumentAccess)m_LstAccess.SelectedIndex;
            Document.Data = m_EditData.Text;

            DialogResult = DialogResult.OK;
        }
    }
}
