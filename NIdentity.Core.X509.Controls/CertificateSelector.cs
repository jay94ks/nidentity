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
    public partial class CertificateSelector : UserControl
    {
        public CertificateSelector()
        {
            InitializeComponent();
        }

        private static readonly string FILTER_PRVATE_FORMAT =
            "PKCS#12 Key Store (*.pfx)|*.pfx|" +
            "PKCS8 Key Store (*.pem)|*.pem";

        private static readonly string FILTER_PUBLIC_FORMAT =
            FILTER_PRVATE_FORMAT + "|" +
            "PKCS1 Certificate (*.cer, *.crt)|*.cer;*.crt|";

        private static readonly string TITLE_SAVE = "Select a location to store the certificate";
        private static readonly string TITLE_LOAD = "Select a location to load the certificate.";

        /// <summary>
        /// Gets or sets whether the selector control is for save or not.
        /// </summary>
        public bool IsSaveMode { get; set; } = false;

        /// <summary>
        /// Gets or sets the certificate which selected by user should have private key or not.
        /// </summary>
        public bool RequiresPrivateKey { get; set; } = false;

        /// <summary>
        /// Selected certificate.
        /// </summary>
        public Certificate Certificate { get; set; }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateLabel();
        }

        /// <summary>
        /// Update the label, aka <see cref="m_EditCertificate"/>.
        /// </summary>
        private void UpdateLabel()
        {
            if (Certificate != null)
                m_EditCertificate.Text = $"{Certificate.Subject} ({Certificate.KeyIdentifier})";
        }

        /// <summary>
        /// Get the title of parent's form.
        /// </summary>
        /// <returns></returns>
        private string GetParentText()
        {
            if (ParentForm is null)
                return "Certificate Selector";

            return ParentForm.Text;
        }

        /// <summary>
        /// Called when the <see cref="m_EditCertificate"/> clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSelector(object sender, EventArgs e)
        {
            if (IsSaveMode)
            {
                if (Certificate is null)
                {
                    ShowMessage("Error: no certificate specified to save.");

                    return;
                }

                using var Sfd = new SaveFileDialog
                {
                    Title = TITLE_SAVE,
                    Filter = Certificate.HasPrivateKey
                        ? FILTER_PRVATE_FORMAT : FILTER_PUBLIC_FORMAT,

                    AddExtension = true,
                    OverwritePrompt = true,
                };

                if (Sfd.ShowDialog() != DialogResult.OK)
                    return;

                if (string.IsNullOrWhiteSpace(Sfd.FileName))
                {
                    ShowMessage("Error: specified location is not valid", MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    var Format = (Sfd.FileName.Split('.').LastOrDefault() ?? "pfx").ToLower();
                    var Bytes = null as byte[];
                    switch (Format)
                    {
                        case "pfx":
                            Bytes = Certificate.ExportPfx();
                            break;

                        case "pem":
                            Bytes = Certificate.ExportPem();
                            break;

                        case "cer":
                        case "crt":
                            Bytes = Certificate.Export();
                            break;

                        default:
                            break;
                    }

                    if (Bytes is null)
                    {
                        ShowMessage("Error: unsupported certificate specified.");
                        return;
                    }    

                    File.WriteAllBytes(Sfd.FileName, Bytes);
                }

                catch
                {
                    ShowMessage("Error: failed to save the certificate.");
                }
            }

            else LoadFromFile();

        }

        /// <summary>
        /// Load certificate from file.
        /// </summary>
        private void LoadFromFile()
        {
            using var Ofd = new OpenFileDialog
            {
                Title = TITLE_LOAD,
                Filter = RequiresPrivateKey
                    ? FILTER_PRVATE_FORMAT : FILTER_PUBLIC_FORMAT,

                AddExtension = true,
                CheckFileExists = true
            };
            if (Ofd.ShowDialog() != DialogResult.OK)
                return;

            if (string.IsNullOrWhiteSpace(Ofd.FileName))
            {
                ShowMessage("Error: specified location is not valid", MessageBoxIcon.Error);
                return;
            }

            var Format = (Ofd.FileName.Split('.').LastOrDefault() ?? string.Empty).ToLower();
            try
            {
                var Bytes = File.ReadAllBytes(Ofd.FileName);
                switch (Format)
                {
                    case "pfx":
                        Certificate = Certificate.ImportPfx(Bytes);
                        break;

                    case "pem":
                        Certificate = Certificate.ImportPem(Bytes);
                        break;

                    case "cer": case "crt":
                        Certificate = Certificate.Import(Bytes);
                        break;

                    default:
                        Certificate = null;
                        break;
                }
            }
            catch
            {
                ShowMessage("Error: failed to open the file.");
                return;
            }

            if (Certificate is null)
            {
                ShowMessage("Error: unsupported certificate specified.");
                return;
            }

            if (RequiresPrivateKey && Certificate.HasPrivateKey == false)
            {
                Certificate = null;
                ShowMessage("Error: specified certificate has no private key.");
                return;
            }

            UpdateLabel();
        }

        /// <summary>
        /// Show a message to user.
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Icon"></param>
        private void ShowMessage(string Message, MessageBoxIcon Icon = MessageBoxIcon.Error)
        {
            MessageBox.Show(Message, GetParentText(), MessageBoxButtons.OK, Icon);
        }
    }
}
