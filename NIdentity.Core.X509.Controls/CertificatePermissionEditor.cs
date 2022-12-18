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
    public partial class CertificatePermissionEditor : Form
    {
        public CertificatePermissionEditor()
        {
            InitializeComponent();
        }

        private static readonly string DESC_READ_FORMAT =
            "Shows the authority to be applied when the specified {Accessor} accesses {Subject}.";

        private static readonly string DESC_EDIT_FORMAT =
            "Specifies the authority to be applied when the specified {Accessor} accesses {Subject}.";

        private static readonly string LABEL_ACCESSOR_FORMAT =
            "Accessor: {Accessor}";

        private CertificatePermission m_Permission;
        private bool m_IsReadOnlyMode;

        /// <summary>
        /// Gets or sets readonly mode or not.
        /// </summary>
        public bool IsReadOnlyMode
        {
            get => m_IsReadOnlyMode;
            set
            {
                RefreshPermissionView();
                m_IsReadOnlyMode = value;
                m_Apply.Text = value ? "Close" : "Apply";
            }
        }

        /// <summary>
        /// Permission to edit or view.
        /// </summary>
        public CertificatePermission Permission
        {
            get => m_Permission;
            set
            {
                if ((m_Permission = value) != null)
                {
                    Text =
                        $"Permission Editor - " +
                        $"{m_Permission.Owner.Subject} ({m_Permission.Owner.KeyIdentifier})";

                    m_Description.Text = (IsReadOnlyMode ? DESC_READ_FORMAT : DESC_EDIT_FORMAT)
                        .Replace("{Subject}", $"{m_Permission.Owner.Subject} ({m_Permission.Owner.KeyIdentifier})")
                        .Replace("{Accessor}", $"{m_Permission.Accessor.Subject} ({m_Permission.Accessor.KeyIdentifier})")
                        ;
                    if (m_Permission.Accessor.Validity == true)
                    {
                        m_Accessor.Text = LABEL_ACCESSOR_FORMAT
                            .Replace("{Accessor}", $"{m_Permission.Accessor.Subject} ({m_Permission.Accessor.KeyIdentifier})")
                            ;
                    }

                    else
                    {
                        m_Accessor.Text = LABEL_ACCESSOR_FORMAT
                            .Replace("{Accessor}", "(children default)")
                            ;
                    }

                    var Enabled = m_IsReadOnlyMode == false;
                    m_CanAuthorityInterfere.Enabled = Enabled;
                    m_CanGenerate.Enabled = Enabled;
                    m_CanList.Enabled = Enabled;
                    m_CanRevoke.Enabled = Enabled;
                    m_CanDelete.Enabled = Enabled;
                    m_CanAlter.Enabled = Enabled;

                    m_CanAuthorityInterfere.Checked = m_Permission.CanAuthorityInterfere;
                    m_CanGenerate.Checked = m_Permission.CanGenerate;
                    m_CanList.Checked = m_Permission.CanList;
                    m_CanRevoke.Checked = m_Permission.CanRevoke;
                    m_CanDelete.Checked = m_Permission.CanDelete;
                    m_CanAlter.Checked = m_Permission.CanAlter;
                }
                else
                {
                    Text = "Permission Editor";
                    m_Description.Text = "No permission selected.";
                    m_Accessor.Text = "Accessor: (none)";

                    m_CanAuthorityInterfere.Enabled = false;
                    m_CanGenerate.Enabled = false;
                    m_CanList.Enabled = false;
                    m_CanRevoke.Enabled = false;
                    m_CanDelete.Enabled = false;
                    m_CanAlter.Enabled = false;
                }
            }
        }


        /// <summary>
        /// Refresh the permission view.
        /// </summary>
        private void RefreshPermissionView()
        {
            var Temp = m_Permission;
            m_Permission = null;

            Permission = Temp;
        }

        /// <inheritdoc/>
        private void OnApply(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
