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
    public partial class CertificateViewer : UserControl
    {
        public CertificateViewer()
        {
            InitializeComponent();
        }

        // ---
        private Certificate m_Certificate;

        /// <summary>
        /// Certificate to view.
        /// </summary>
        public Certificate Certificate
        {
            get => m_Certificate;
            set
            {
                if ((m_Certificate = value) is null)
                    SetCertificateNotSelected();

                else
                    SetCertificateSelected(value);
            }
        }

        /// <summary>
        /// Set the certificate as selected.
        /// </summary>
        /// <param name="value"></param>
        private void SetCertificateSelected(Certificate value)
        {
            m_Subject.Text = value.Subject;
            m_KeyIdentifier.Text = value.KeyIdentifier;
            m_Issuer.Text = value.Issuer.Subject;
            m_IssuerKeyIdentifier.Text = value.Issuer.KeyIdentifier;
            m_SerialNumber.Text = value.SerialNumber;
            m_Thumbprint.Text = value.Thumbprint;
            m_CreationTime.Text = value.CreationTime.ToLocalTime().ToString();
            m_ExpirationTime.Text = value.ExpirationTime.ToLocalTime().ToString();

            if (value.IsRevokeIdentified == false)
            {
                m_RevokeReason.Text = "(not revoked)";
                m_Issuer.Visible = m_IssuerKeyIdentifier.Visible = false;
            }

            else
            {
                var RevokeTime = value.RevokeTime.Value.ToLocalTime().ToString();
                m_RevokeReason.Text = value.RevokeReason.ToString() + $" ({RevokeTime})";
                m_Issuer.Visible = m_IssuerKeyIdentifier.Visible = true;
            }

            m_AlternativeNames.Text = string.Join(", ", value.Sans);

            m_IsSelfSigned.Checked = value.IsSelfSigned;
            m_IsAuthority.Checked = value.IsAuthority;
            m_HasPrivateKey.Checked = value.HasPrivateKey;
        }

        /// <summary>
        /// Set the certificate as not selected.
        /// </summary>
        private void SetCertificateNotSelected()
        {
            m_Subject.Text = string.Empty;
            m_KeyIdentifier.Text = string.Empty;
            m_Issuer.Text = string.Empty;
            m_Issuer.Visible = true;
            m_IssuerKeyIdentifier.Text = string.Empty;
            m_IssuerKeyIdentifier.Visible = true;

            m_SerialNumber.Text = string.Empty;
            m_Thumbprint.Text = string.Empty;
            m_CreationTime.Text = string.Empty;
            m_ExpirationTime.Text = string.Empty;

            m_RevokeReason.Text = string.Empty;
            m_AlternativeNames.Text = string.Empty;

            m_IsSelfSigned.Checked = false;
            m_IsAuthority.Checked = false;
            m_HasPrivateKey.Checked = false;
        }
    }
}
