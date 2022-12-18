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

namespace NIdentity.Core.X509.Controls
{
    public partial class CertificateDocumentEditor : Form
    {
        public CertificateDocumentEditor()
        {
            InitializeComponent();
        }

        // ---
        private bool m_IsReadonly = false;
        private Document m_Document = null;

        /// <summary>
        /// Indicates whether the document is read-only or not.
        /// </summary>
        public bool IsReadonly
        {
            get => m_IsReadonly;
            set
            {
                m_IsReadonly = value;
                m_MimeType.Enabled = value == false;
                m_AccessLimit.Enabled = value == false;
                m_Data.Enabled = value == false;
                m_Cancel.Visible = value == false;
                m_Save.Text = value ? "Close" : "Save";
            }
        }

        /// <summary>
        /// Document itself.
        /// </summary>
        public Document Document
        {
            get => m_Document;
            set
            {
                if ((m_Document = value) is null)
                {
                    ClearForm();
                    return;
                }

                SetToForm();
            }
        }

        /// <summary>
        /// Clear the form.
        /// </summary>
        private void ClearForm()
        {
            m_Owner.Text = "";
            m_OwnerKeyId.Text = "";
            m_MimeType.Text = "text/plain";
            m_PathName.Text = "";
            m_Data.Text = "";
            m_AccessLimit.SelectedIndex = 0;

            m_Owner.Enabled = false;
            m_OwnerKeyId.Enabled = false;
            m_PathName.Enabled = true;
        }

        /// <summary>
        /// Set to the form.
        /// </summary>
        private void SetToForm()
        {
            m_Owner.Text = m_Document.Identity.Owner.Subject;
            m_OwnerKeyId.Text = m_Document.Identity.Owner.KeyIdentifier;
            m_MimeType.Text = m_Document.MimeType ?? "text/plain";
            m_PathName.Text = m_Document.Identity.PathName ?? string.Empty;
            m_AccessLimit.SelectedIndex = (int)m_Document.Access;
            m_Data.Text = m_Document.Data ?? string.Empty;

            m_Owner.Enabled = false;
            m_OwnerKeyId.Enabled = false;
            m_PathName.Enabled = false;
        }

        /// <summary>
        /// Cancel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSave(object sender, EventArgs e)
        {
            if (IsReadonly == false)
                SetChangesToDocument();

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Set the changes to the document.
        /// </summary>
        private void SetChangesToDocument()
        {
            if (m_Document is null)
            {
                m_Document = new Document
                {
                    Access = (DocumentAccess)m_AccessLimit.SelectedIndex,
                    Identity = new DocumentIdentity(new CertificateIdentity(
                        m_Owner.Text, m_OwnerKeyId.Text), m_PathName.Text),

                    MimeType = m_MimeType.Text,
                    Data = m_Data.Text
                };
            }

            else
            {
                m_Document.Access = (DocumentAccess)m_AccessLimit.SelectedIndex;
                m_Document.MimeType = m_MimeType.Text;
                m_Document.Data = m_Data.Text;
            }
        }
    }
}
