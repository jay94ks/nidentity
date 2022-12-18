namespace NIdentity.Core.X509.Controls
{
    partial class CertificateDocumentEditor
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_OwnerKeyId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.m_Owner = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.m_AccessLimit = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.m_MimeType = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.m_PathName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.m_Cancel = new System.Windows.Forms.Button();
            this.m_Save = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.m_Data = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(687, 96);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel6);
            this.splitContainer1.Panel2.Controls.Add(this.panel5);
            this.splitContainer1.Size = new System.Drawing.Size(687, 96);
            this.splitContainer1.SplitterDistance = 336;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.m_OwnerKeyId);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 47);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10);
            this.panel3.Size = new System.Drawing.Size(336, 47);
            this.panel3.TabIndex = 1;
            // 
            // m_OwnerKeyId
            // 
            this.m_OwnerKeyId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_OwnerKeyId.Enabled = false;
            this.m_OwnerKeyId.Location = new System.Drawing.Point(113, 10);
            this.m_OwnerKeyId.Name = "m_OwnerKeyId";
            this.m_OwnerKeyId.Size = new System.Drawing.Size(213, 27);
            this.m_OwnerKeyId.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Owner Key Id";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.m_Owner);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(336, 47);
            this.panel2.TabIndex = 0;
            // 
            // m_Owner
            // 
            this.m_Owner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Owner.Enabled = false;
            this.m_Owner.Location = new System.Drawing.Point(113, 10);
            this.m_Owner.Name = "m_Owner";
            this.m_Owner.Size = new System.Drawing.Size(213, 27);
            this.m_Owner.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Owner";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.m_AccessLimit);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 47);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(10);
            this.panel6.Size = new System.Drawing.Size(347, 47);
            this.panel6.TabIndex = 3;
            // 
            // m_AccessLimit
            // 
            this.m_AccessLimit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_AccessLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_AccessLimit.FormattingEnabled = true;
            this.m_AccessLimit.Items.AddRange(new object[] {
            "Public",
            "Private",
            "Authority",
            "Super"});
            this.m_AccessLimit.Location = new System.Drawing.Point(113, 10);
            this.m_AccessLimit.Name = "m_AccessLimit";
            this.m_AccessLimit.Size = new System.Drawing.Size(224, 28);
            this.m_AccessLimit.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(10, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 27);
            this.label5.TabIndex = 0;
            this.label5.Text = "Access Limit";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.m_MimeType);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(10);
            this.panel5.Size = new System.Drawing.Size(347, 47);
            this.panel5.TabIndex = 2;
            // 
            // m_MimeType
            // 
            this.m_MimeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_MimeType.Location = new System.Drawing.Point(113, 10);
            this.m_MimeType.Name = "m_MimeType";
            this.m_MimeType.Size = new System.Drawing.Size(224, 27);
            this.m_MimeType.TabIndex = 1;
            this.m_MimeType.Text = "text/plain";
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(10, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "Mime Type";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.m_PathName);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 96);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(10);
            this.panel7.Size = new System.Drawing.Size(687, 47);
            this.panel7.TabIndex = 2;
            // 
            // m_PathName
            // 
            this.m_PathName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_PathName.Location = new System.Drawing.Point(113, 10);
            this.m_PathName.Name = "m_PathName";
            this.m_PathName.Size = new System.Drawing.Size(564, 27);
            this.m_PathName.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(10, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 27);
            this.label6.TabIndex = 0;
            this.label6.Text = "Path Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.m_Cancel);
            this.panel4.Controls.Add(this.m_Save);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 500);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(10);
            this.panel4.Size = new System.Drawing.Size(687, 51);
            this.panel4.TabIndex = 3;
            // 
            // m_Cancel
            // 
            this.m_Cancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_Cancel.Location = new System.Drawing.Point(10, 10);
            this.m_Cancel.Name = "m_Cancel";
            this.m_Cancel.Size = new System.Drawing.Size(132, 31);
            this.m_Cancel.TabIndex = 3;
            this.m_Cancel.Text = "Cancel";
            this.m_Cancel.UseVisualStyleBackColor = true;
            this.m_Cancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // m_Save
            // 
            this.m_Save.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_Save.Location = new System.Drawing.Point(545, 10);
            this.m_Save.Name = "m_Save";
            this.m_Save.Size = new System.Drawing.Size(132, 31);
            this.m_Save.TabIndex = 2;
            this.m_Save.Text = "Save";
            this.m_Save.UseVisualStyleBackColor = true;
            this.m_Save.Click += new System.EventHandler(this.OnSave);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.m_Data);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(0, 143);
            this.panel8.Name = "panel8";
            this.panel8.Padding = new System.Windows.Forms.Padding(10);
            this.panel8.Size = new System.Drawing.Size(687, 357);
            this.panel8.TabIndex = 4;
            // 
            // m_Data
            // 
            this.m_Data.AcceptsReturn = true;
            this.m_Data.AcceptsTab = true;
            this.m_Data.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Data.Location = new System.Drawing.Point(10, 10);
            this.m_Data.Multiline = true;
            this.m_Data.Name = "m_Data";
            this.m_Data.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_Data.Size = new System.Drawing.Size(667, 337);
            this.m_Data.TabIndex = 0;
            // 
            // CertificateDocumentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 551);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel1);
            this.Name = "CertificateDocumentEditor";
            this.Text = "Document Editor";
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private SplitContainer splitContainer1;
        private Panel panel2;
        private TextBox m_Owner;
        private Label label1;
        private Panel panel3;
        private TextBox m_OwnerKeyId;
        private Label label2;
        private Panel panel6;
        private Label label5;
        private Panel panel5;
        private Label label4;
        private Panel panel7;
        private TextBox m_PathName;
        private Label label6;
        private Panel panel4;
        private Panel panel8;
        private TextBox m_Data;
        private ComboBox m_AccessLimit;
        private TextBox m_MimeType;
        private Button m_Cancel;
        private Button m_Save;
    }
}