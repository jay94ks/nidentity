namespace NIdentity.Core.X509.Browser.Forms
{
    partial class FrmParameters
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
            this.m_ChkSuperMode = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_Timeout = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.m_LstMode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.m_BtnTest = new System.Windows.Forms.Button();
            this.m_LblAuthority = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_EditServerUri = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Timeout)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_ChkSuperMode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.m_Timeout);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.m_LstMode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.m_BtnTest);
            this.groupBox1.Controls.Add(this.m_LblAuthority);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.m_EditServerUri);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 299);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Settings";
            // 
            // m_ChkSuperMode
            // 
            this.m_ChkSuperMode.AutoSize = true;
            this.m_ChkSuperMode.Location = new System.Drawing.Point(340, 195);
            this.m_ChkSuperMode.Name = "m_ChkSuperMode";
            this.m_ChkSuperMode.Size = new System.Drawing.Size(86, 24);
            this.m_ChkSuperMode.TabIndex = 8;
            this.m_ChkSuperMode.Text = "Enabled";
            this.m_ChkSuperMode.UseVisualStyleBackColor = true;
            this.m_ChkSuperMode.CheckedChanged += new System.EventHandler(this.OnCheckSuperMode);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Super Access";
            // 
            // m_Timeout
            // 
            this.m_Timeout.Location = new System.Drawing.Point(340, 157);
            this.m_Timeout.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_Timeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_Timeout.Name = "m_Timeout";
            this.m_Timeout.Size = new System.Drawing.Size(168, 27);
            this.m_Timeout.TabIndex = 1;
            this.m_Timeout.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.m_Timeout.ValueChanged += new System.EventHandler(this.OnSetTimeout);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Timeout (in Seconds)";
            // 
            // m_LstMode
            // 
            this.m_LstMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_LstMode.FormattingEnabled = true;
            this.m_LstMode.Items.AddRange(new object[] {
            "Https",
            "WebSockets"});
            this.m_LstMode.Location = new System.Drawing.Point(340, 115);
            this.m_LstMode.Name = "m_LstMode";
            this.m_LstMode.Size = new System.Drawing.Size(168, 28);
            this.m_LstMode.TabIndex = 5;
            this.m_LstMode.SelectedIndexChanged += new System.EventHandler(this.OnSelectConnectionMode);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Mode";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(19, 250);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(315, 27);
            this.progressBar1.TabIndex = 1;
            // 
            // m_BtnTest
            // 
            this.m_BtnTest.Location = new System.Drawing.Point(340, 250);
            this.m_BtnTest.Name = "m_BtnTest";
            this.m_BtnTest.Size = new System.Drawing.Size(168, 27);
            this.m_BtnTest.TabIndex = 1;
            this.m_BtnTest.Text = "Test Connectivity";
            this.m_BtnTest.UseVisualStyleBackColor = true;
            this.m_BtnTest.Click += new System.EventHandler(this.OnTestConnectivity);
            // 
            // m_LblAuthority
            // 
            this.m_LblAuthority.BackColor = System.Drawing.Color.White;
            this.m_LblAuthority.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_LblAuthority.Location = new System.Drawing.Point(171, 74);
            this.m_LblAuthority.Name = "m_LblAuthority";
            this.m_LblAuthority.Size = new System.Drawing.Size(337, 27);
            this.m_LblAuthority.TabIndex = 3;
            this.m_LblAuthority.Text = "(Nothing Selected)";
            this.m_LblAuthority.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.m_LblAuthority.Click += new System.EventHandler(this.OnSelectAuthorityCertificate);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Authority Certificate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server URI";
            // 
            // m_EditServerUri
            // 
            this.m_EditServerUri.Location = new System.Drawing.Point(171, 33);
            this.m_EditServerUri.Name = "m_EditServerUri";
            this.m_EditServerUri.Size = new System.Drawing.Size(337, 27);
            this.m_EditServerUri.TabIndex = 0;
            this.m_EditServerUri.Text = "https://127.0.0.1:7001/api/infra/live";
            this.m_EditServerUri.TextChanged += new System.EventHandler(this.OnChangeServerUri);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(13, 317);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(532, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Cache Policy";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Not available yet";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(377, 414);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 27);
            this.button2.TabIndex = 7;
            this.button2.Text = "Save Settings";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnSaveSettings);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(13, 414);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(168, 27);
            this.button3.TabIndex = 8;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnCancelSettings);
            // 
            // FrmParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(559, 460);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmParameters";
            this.Text = "NIdentity X509 Parameters";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_Timeout)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private Label label1;
        private TextBox m_EditServerUri;
        private Label label2;
        private Label m_LblAuthority;
        private Button m_BtnTest;
        private ProgressBar progressBar1;
        private Label label4;
        private ComboBox m_LstMode;
        private NumericUpDown m_Timeout;
        private Label label5;
        private GroupBox groupBox2;
        private Label label6;
        private Button button2;
        private Button button3;
        private CheckBox m_ChkSuperMode;
        private Label label3;
    }
}