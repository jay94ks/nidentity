namespace NIdentity.Core.X509.Controls
{
    partial class CertificateViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.certificateView1 = new NIdentity.Core.X509.Controls.CertificateView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_RevokeReason = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.m_Unrevoke = new System.Windows.Forms.Button();
            this.m_Delete = new System.Windows.Forms.Button();
            this.m_Revoke = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // certificateView1
            // 
            this.certificateView1.Certificate = null;
            this.certificateView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.certificateView1.Location = new System.Drawing.Point(0, 0);
            this.certificateView1.Name = "certificateView1";
            this.certificateView1.Size = new System.Drawing.Size(589, 520);
            this.certificateView1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_RevokeReason);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.m_Unrevoke);
            this.panel1.Controls.Add(this.m_Delete);
            this.panel1.Controls.Add(this.m_Revoke);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 520);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(589, 47);
            this.panel1.TabIndex = 1;
            // 
            // m_RevokeReason
            // 
            this.m_RevokeReason.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_RevokeReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_RevokeReason.FormattingEnabled = true;
            this.m_RevokeReason.Items.AddRange(new object[] {
            "Not revoked",
            "Revoked, Key compromised",
            "Revoked, CA compromised",
            "Revoked, Affiliation changed",
            "Revoked, Superseded",
            "Revoked, Cassation of operation",
            "Revoked, Certificate hold",
            "Revoked, Removed from CRL",
            "Revoked, Privilege withdrawn",
            "Revoked, AA compromised"});
            this.m_RevokeReason.Location = new System.Drawing.Point(208, 10);
            this.m_RevokeReason.Name = "m_RevokeReason";
            this.m_RevokeReason.Size = new System.Drawing.Size(215, 28);
            this.m_RevokeReason.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(198, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 27);
            this.panel2.TabIndex = 3;
            // 
            // m_Unrevoke
            // 
            this.m_Unrevoke.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_Unrevoke.Location = new System.Drawing.Point(104, 10);
            this.m_Unrevoke.Name = "m_Unrevoke";
            this.m_Unrevoke.Size = new System.Drawing.Size(94, 27);
            this.m_Unrevoke.TabIndex = 4;
            this.m_Unrevoke.Text = "Unrevoke";
            this.m_Unrevoke.UseVisualStyleBackColor = true;
            this.m_Unrevoke.Click += new System.EventHandler(this.OnUnrevoke);
            // 
            // m_Delete
            // 
            this.m_Delete.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_Delete.ForeColor = System.Drawing.Color.Red;
            this.m_Delete.Location = new System.Drawing.Point(485, 10);
            this.m_Delete.Name = "m_Delete";
            this.m_Delete.Size = new System.Drawing.Size(94, 27);
            this.m_Delete.TabIndex = 1;
            this.m_Delete.Text = "Delete";
            this.m_Delete.UseVisualStyleBackColor = true;
            this.m_Delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // m_Revoke
            // 
            this.m_Revoke.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_Revoke.Location = new System.Drawing.Point(10, 10);
            this.m_Revoke.Name = "m_Revoke";
            this.m_Revoke.Size = new System.Drawing.Size(94, 27);
            this.m_Revoke.TabIndex = 0;
            this.m_Revoke.Text = "Revoke";
            this.m_Revoke.UseVisualStyleBackColor = true;
            this.m_Revoke.Click += new System.EventHandler(this.OnRevoke);
            // 
            // CertificateViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 567);
            this.Controls.Add(this.certificateView1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CertificateViewer";
            this.Text = "CertificateViewer";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CertificateView certificateView1;
        private Panel panel1;
        private ComboBox m_RevokeReason;
        private Button m_Delete;
        private Button m_Revoke;
        private Panel panel2;
        private Button m_Unrevoke;
    }
}