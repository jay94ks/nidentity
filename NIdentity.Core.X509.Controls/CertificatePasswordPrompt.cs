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
    public partial class CertificatePasswordPrompt : Form
    {
        public CertificatePasswordPrompt()
        {
            InitializeComponent();
        }

        // ---
        private bool m_IsInput = false;

        /// <summary>
        /// Indicates whether the password should be written or not.
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the password prompt's purpose is input or not.
        /// </summary>
        public bool IsInput
        {
            get => m_IsInput;
            set
            {
                m_IsInput = value;

                // --
                m_PasswordRe.Visible = value == false;
                m_LblPasswordRe.Visible = value == false;

                Height = value ? 140 : 180;
            }
        }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password
        {
            get => m_Password.Text;
            set => m_Password.Text = value;
        }

        /// <summary>
        /// Called when "OK" button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOkay(object sender, EventArgs e)
        {
            var Pw = m_Password.Text;
            if (IsRequired == true && string.IsNullOrWhiteSpace(Pw))
            {
                MessageBox.Show(
                    "Error: password is empty.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (IsInput == false)
            {
                var Re = m_PasswordRe.Text;
                if (Pw != Re)
                {
                    MessageBox.Show(
                        "Error: password is not matched.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }
    }
}
