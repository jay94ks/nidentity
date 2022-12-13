namespace NIdentity.Core.X509.Browser.Forms
{
    partial class FrmGenerator
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
            this.m_EditSansName = new System.Windows.Forms.TextBox();
            this.m_EditName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.m_BtnRemove = new System.Windows.Forms.Button();
            this.m_LstSans = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_BtnAdd = new System.Windows.Forms.Button();
            this.m_NumHours = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_LstPurposes = new System.Windows.Forms.ListBox();
            this.m_LstAlgorithm = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_LstType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_EditKeyId = new System.Windows.Forms.TextBox();
            this.m_EditIssuer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_ChkCAIssuers = new System.Windows.Forms.CheckBox();
            this.m_ChkCRL = new System.Windows.Forms.CheckBox();
            this.m_ChkOcsp = new System.Windows.Forms.CheckBox();
            this.m_BtnGenerate = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.m_LblPath = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_NumHours)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_EditSansName);
            this.groupBox1.Controls.Add(this.m_EditName);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.m_BtnRemove);
            this.groupBox1.Controls.Add(this.m_LstSans);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.m_BtnAdd);
            this.groupBox1.Controls.Add(this.m_NumHours);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.m_LstPurposes);
            this.groupBox1.Controls.Add(this.m_LstAlgorithm);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.m_LstType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(534, 364);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Subject";
            // 
            // m_EditSansName
            // 
            this.m_EditSansName.Location = new System.Drawing.Point(98, 285);
            this.m_EditSansName.Name = "m_EditSansName";
            this.m_EditSansName.Size = new System.Drawing.Size(282, 27);
            this.m_EditSansName.TabIndex = 14;
            // 
            // m_EditName
            // 
            this.m_EditName.Location = new System.Drawing.Point(98, 173);
            this.m_EditName.Name = "m_EditName";
            this.m_EditName.Size = new System.Drawing.Size(414, 27);
            this.m_EditName.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 20);
            this.label8.TabIndex = 13;
            this.label8.Text = "Name";
            // 
            // m_BtnRemove
            // 
            this.m_BtnRemove.Location = new System.Drawing.Point(386, 319);
            this.m_BtnRemove.Name = "m_BtnRemove";
            this.m_BtnRemove.Size = new System.Drawing.Size(126, 29);
            this.m_BtnRemove.TabIndex = 13;
            this.m_BtnRemove.Text = "Remove Name";
            this.m_BtnRemove.UseVisualStyleBackColor = true;
            this.m_BtnRemove.Click += new System.EventHandler(this.OnRemoveName);
            // 
            // m_LstSans
            // 
            this.m_LstSans.FormattingEnabled = true;
            this.m_LstSans.ItemHeight = 20;
            this.m_LstSans.Location = new System.Drawing.Point(98, 211);
            this.m_LstSans.Name = "m_LstSans";
            this.m_LstSans.Size = new System.Drawing.Size(414, 64);
            this.m_LstSans.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 20);
            this.label6.TabIndex = 9;
            this.label6.Text = "SANS";
            // 
            // m_BtnAdd
            // 
            this.m_BtnAdd.Location = new System.Drawing.Point(386, 284);
            this.m_BtnAdd.Name = "m_BtnAdd";
            this.m_BtnAdd.Size = new System.Drawing.Size(126, 29);
            this.m_BtnAdd.TabIndex = 12;
            this.m_BtnAdd.Text = "Add Name";
            this.m_BtnAdd.UseVisualStyleBackColor = true;
            this.m_BtnAdd.Click += new System.EventHandler(this.OnAddName);
            // 
            // m_NumHours
            // 
            this.m_NumHours.Location = new System.Drawing.Point(361, 137);
            this.m_NumHours.Maximum = new decimal(new int[] {
            876000,
            0,
            0,
            0});
            this.m_NumHours.Name = "m_NumHours";
            this.m_NumHours.Size = new System.Drawing.Size(151, 27);
            this.m_NumHours.TabIndex = 7;
            this.m_NumHours.Value = new decimal(new int[] {
            8760,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Expiration (in hours)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Purpose";
            // 
            // m_LstPurposes
            // 
            this.m_LstPurposes.FormattingEnabled = true;
            this.m_LstPurposes.ItemHeight = 20;
            this.m_LstPurposes.Items.AddRange(new object[] {
            "Networking",
            "Protection",
            "Stamping",
            "IPSecEndSystem",
            "IPSecTunnel",
            "IPSecUser"});
            this.m_LstPurposes.Location = new System.Drawing.Point(98, 67);
            this.m_LstPurposes.Name = "m_LstPurposes";
            this.m_LstPurposes.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.m_LstPurposes.Size = new System.Drawing.Size(414, 64);
            this.m_LstPurposes.TabIndex = 4;
            // 
            // m_LstAlgorithm
            // 
            this.m_LstAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_LstAlgorithm.FormattingEnabled = true;
            this.m_LstAlgorithm.Items.AddRange(new object[] {
            "rsa-2048",
            "rsa-4096",
            "secp256k1",
            "secp256r1"});
            this.m_LstAlgorithm.Location = new System.Drawing.Point(361, 24);
            this.m_LstAlgorithm.Name = "m_LstAlgorithm";
            this.m_LstAlgorithm.Size = new System.Drawing.Size(151, 28);
            this.m_LstAlgorithm.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(258, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Algorithm";
            // 
            // m_LstType
            // 
            this.m_LstType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_LstType.FormattingEnabled = true;
            this.m_LstType.Items.AddRange(new object[] {
            "Root",
            "Immediate",
            "Leaf"});
            this.m_LstType.Location = new System.Drawing.Point(98, 24);
            this.m_LstType.Name = "m_LstType";
            this.m_LstType.Size = new System.Drawing.Size(151, 28);
            this.m_LstType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.m_EditKeyId);
            this.groupBox2.Controls.Add(this.m_EditIssuer);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(12, 382);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(534, 98);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Issuer";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Key ID";
            // 
            // m_EditKeyId
            // 
            this.m_EditKeyId.BackColor = System.Drawing.Color.White;
            this.m_EditKeyId.Location = new System.Drawing.Point(98, 59);
            this.m_EditKeyId.Name = "m_EditKeyId";
            this.m_EditKeyId.ReadOnly = true;
            this.m_EditKeyId.Size = new System.Drawing.Size(414, 27);
            this.m_EditKeyId.TabIndex = 16;
            // 
            // m_EditIssuer
            // 
            this.m_EditIssuer.BackColor = System.Drawing.Color.White;
            this.m_EditIssuer.Location = new System.Drawing.Point(98, 26);
            this.m_EditIssuer.Name = "m_EditIssuer";
            this.m_EditIssuer.ReadOnly = true;
            this.m_EditIssuer.Size = new System.Drawing.Size(414, 27);
            this.m_EditIssuer.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 20);
            this.label7.TabIndex = 13;
            this.label7.Text = "Issuer";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.m_ChkCAIssuers);
            this.groupBox3.Controls.Add(this.m_ChkCRL);
            this.groupBox3.Controls.Add(this.m_ChkOcsp);
            this.groupBox3.Location = new System.Drawing.Point(12, 486);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(534, 125);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Features";
            // 
            // m_ChkCAIssuers
            // 
            this.m_ChkCAIssuers.AutoSize = true;
            this.m_ChkCAIssuers.Checked = true;
            this.m_ChkCAIssuers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ChkCAIssuers.Location = new System.Drawing.Point(14, 86);
            this.m_ChkCAIssuers.Name = "m_ChkCAIssuers";
            this.m_ChkCAIssuers.Size = new System.Drawing.Size(255, 24);
            this.m_ChkCAIssuers.TabIndex = 2;
            this.m_ChkCAIssuers.Text = "CA\'s certificate distribution point";
            this.m_ChkCAIssuers.UseVisualStyleBackColor = true;
            // 
            // m_ChkCRL
            // 
            this.m_ChkCRL.AutoSize = true;
            this.m_ChkCRL.Checked = true;
            this.m_ChkCRL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ChkCRL.Location = new System.Drawing.Point(14, 56);
            this.m_ChkCRL.Name = "m_ChkCRL";
            this.m_ChkCRL.Size = new System.Drawing.Size(181, 24);
            this.m_ChkCRL.TabIndex = 1;
            this.m_ChkCRL.Text = "CRL distribution point";
            this.m_ChkCRL.UseVisualStyleBackColor = true;
            // 
            // m_ChkOcsp
            // 
            this.m_ChkOcsp.AutoSize = true;
            this.m_ChkOcsp.Checked = true;
            this.m_ChkOcsp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_ChkOcsp.Location = new System.Drawing.Point(14, 26);
            this.m_ChkOcsp.Name = "m_ChkOcsp";
            this.m_ChkOcsp.Size = new System.Drawing.Size(172, 24);
            this.m_ChkOcsp.TabIndex = 0;
            this.m_ChkOcsp.Text = "OCSP responsder uri";
            this.m_ChkOcsp.UseVisualStyleBackColor = true;
            // 
            // m_BtnGenerate
            // 
            this.m_BtnGenerate.Location = new System.Drawing.Point(420, 689);
            this.m_BtnGenerate.Name = "m_BtnGenerate";
            this.m_BtnGenerate.Size = new System.Drawing.Size(126, 29);
            this.m_BtnGenerate.TabIndex = 14;
            this.m_BtnGenerate.Text = "Generate";
            this.m_BtnGenerate.UseVisualStyleBackColor = true;
            this.m_BtnGenerate.Click += new System.EventHandler(this.OnGenerate);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.m_LblPath);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Location = new System.Drawing.Point(12, 617);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(534, 66);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output";
            // 
            // m_LblPath
            // 
            this.m_LblPath.BackColor = System.Drawing.Color.White;
            this.m_LblPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_LblPath.Location = new System.Drawing.Point(98, 23);
            this.m_LblPath.Name = "m_LblPath";
            this.m_LblPath.Size = new System.Drawing.Size(414, 24);
            this.m_LblPath.TabIndex = 16;
            this.m_LblPath.Text = "(Not Specified)";
            this.m_LblPath.Click += new System.EventHandler(this.OnSelectOutput);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 20);
            this.label9.TabIndex = 0;
            this.label9.Text = "Path";
            // 
            // FrmGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 728);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.m_BtnGenerate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmGenerator";
            this.Text = "NIdentity X509 Certificate Generator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_NumHours)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ComboBox m_LstType;
        private Label label1;
        private ComboBox m_LstAlgorithm;
        private Label label2;
        private Label label3;
        private ListBox m_LstPurposes;
        private NumericUpDown m_NumHours;
        private Label label4;
        private TextBox m_EditName;
        private Label label8;
        private Button m_BtnRemove;
        private Button m_BtnAdd;
        private ListBox m_LstSans;
        private Label label6;
        private Label label5;
        private TextBox m_EditKeyId;
        private TextBox m_EditIssuer;
        private Label label7;
        private GroupBox groupBox3;
        private CheckBox m_ChkCAIssuers;
        private CheckBox m_ChkCRL;
        private CheckBox m_ChkOcsp;
        private Button m_BtnGenerate;
        private GroupBox groupBox4;
        private Label m_LblPath;
        private Label label9;
        private TextBox m_EditSansName;
    }
}