namespace NIdentity.Core.X509.Browser.Forms.Docs
{
    partial class FrmNewDocument
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_EditOwner = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_EditOwnerKeyId = new System.Windows.Forms.TextBox();
            this.m_EditPathName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_EditMimeType = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_LstAccess = new System.Windows.Forms.ComboBox();
            this.m_EditData = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_EditData);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(10, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox1.Size = new System.Drawing.Size(803, 365);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_LstAccess);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.m_EditMimeType);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.m_EditPathName);
            this.panel1.Controls.Add(this.m_EditOwnerKeyId);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.m_EditOwner);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 125);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Owner";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Path Name";
            // 
            // m_EditOwner
            // 
            this.m_EditOwner.BackColor = System.Drawing.Color.White;
            this.m_EditOwner.Location = new System.Drawing.Point(124, 12);
            this.m_EditOwner.Name = "m_EditOwner";
            this.m_EditOwner.ReadOnly = true;
            this.m_EditOwner.Size = new System.Drawing.Size(273, 27);
            this.m_EditOwner.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(403, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Owner Key Id";
            // 
            // m_EditOwnerKeyId
            // 
            this.m_EditOwnerKeyId.BackColor = System.Drawing.Color.White;
            this.m_EditOwnerKeyId.Location = new System.Drawing.Point(512, 12);
            this.m_EditOwnerKeyId.Name = "m_EditOwnerKeyId";
            this.m_EditOwnerKeyId.ReadOnly = true;
            this.m_EditOwnerKeyId.Size = new System.Drawing.Size(273, 27);
            this.m_EditOwnerKeyId.TabIndex = 4;
            // 
            // m_EditPathName
            // 
            this.m_EditPathName.Location = new System.Drawing.Point(124, 49);
            this.m_EditPathName.Name = "m_EditPathName";
            this.m_EditPathName.Size = new System.Drawing.Size(661, 27);
            this.m_EditPathName.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Mime Type";
            // 
            // m_EditMimeType
            // 
            this.m_EditMimeType.Location = new System.Drawing.Point(124, 85);
            this.m_EditMimeType.Name = "m_EditMimeType";
            this.m_EditMimeType.Size = new System.Drawing.Size(155, 27);
            this.m_EditMimeType.TabIndex = 7;
            this.m_EditMimeType.Text = "text/plain";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(403, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Access Limit";
            // 
            // m_LstAccess
            // 
            this.m_LstAccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_LstAccess.FormattingEnabled = true;
            this.m_LstAccess.Items.AddRange(new object[] {
            "Public",
            "Private",
            "Authority",
            "Super"});
            this.m_LstAccess.Location = new System.Drawing.Point(512, 84);
            this.m_LstAccess.Name = "m_LstAccess";
            this.m_LstAccess.Size = new System.Drawing.Size(155, 28);
            this.m_LstAccess.TabIndex = 9;
            // 
            // m_EditData
            // 
            this.m_EditData.AcceptsReturn = true;
            this.m_EditData.AcceptsTab = true;
            this.m_EditData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_EditData.Location = new System.Drawing.Point(10, 30);
            this.m_EditData.MaxLength = 3276700;
            this.m_EditData.Multiline = true;
            this.m_EditData.Name = "m_EditData";
            this.m_EditData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_EditData.Size = new System.Drawing.Size(783, 325);
            this.m_EditData.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(10, 500);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(803, 51);
            this.panel2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(661, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 29);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(10, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(132, 29);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FrmNewDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 561);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.MinimumSize = new System.Drawing.Size(841, 608);
            this.Name = "FrmNewDocument";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "NIdentity X509 New Document";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private Panel panel1;
        private TextBox m_EditOwner;
        private Label label2;
        private Label label1;
        private TextBox m_EditPathName;
        private TextBox m_EditOwnerKeyId;
        private Label label3;
        private TextBox m_EditMimeType;
        private Label label4;
        private ComboBox m_LstAccess;
        private Label label5;
        private TextBox m_EditData;
        private Panel panel2;
        private Button button2;
        private Button button1;
    }
}