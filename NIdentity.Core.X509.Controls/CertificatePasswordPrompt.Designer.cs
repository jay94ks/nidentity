namespace NIdentity.Core.X509.Controls
{
    partial class CertificatePasswordPrompt
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
            this.m_Password = new System.Windows.Forms.TextBox();
            this.m_PasswordRe = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_LblPasswordRe = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_Password
            // 
            this.m_Password.Location = new System.Drawing.Point(122, 13);
            this.m_Password.Name = "m_Password";
            this.m_Password.PasswordChar = '*';
            this.m_Password.Size = new System.Drawing.Size(181, 27);
            this.m_Password.TabIndex = 0;
            // 
            // m_PasswordRe
            // 
            this.m_PasswordRe.Location = new System.Drawing.Point(122, 52);
            this.m_PasswordRe.Name = "m_PasswordRe";
            this.m_PasswordRe.PasswordChar = '*';
            this.m_PasswordRe.Size = new System.Drawing.Size(181, 27);
            this.m_PasswordRe.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Password";
            // 
            // m_LblPasswordRe
            // 
            this.m_LblPasswordRe.AutoSize = true;
            this.m_LblPasswordRe.Location = new System.Drawing.Point(12, 55);
            this.m_LblPasswordRe.Name = "m_LblPasswordRe";
            this.m_LblPasswordRe.Size = new System.Drawing.Size(104, 20);
            this.m_LblPasswordRe.TabIndex = 3;
            this.m_LblPasswordRe.Text = "Password (RE)";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(209, 90);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnOkay);
            // 
            // CertificatePasswordPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 133);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.m_LblPasswordRe);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_PasswordRe);
            this.Controls.Add(this.m_Password);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CertificatePasswordPrompt";
            this.Text = "Password Required";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox m_Password;
        private TextBox m_PasswordRe;
        private Label label1;
        private Label m_LblPasswordRe;
        private Button button1;
    }
}