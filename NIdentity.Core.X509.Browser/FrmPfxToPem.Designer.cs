namespace NIdentity.Core.X509.Browser
{
    partial class FrmPfxToPem
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
            this.m_Selector = new NIdentity.Core.X509.Controls.CertificateSelector();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_Selector
            // 
            this.m_Selector.AutoSize = true;
            this.m_Selector.Certificate = null;
            this.m_Selector.IsSaveMode = false;
            this.m_Selector.Location = new System.Drawing.Point(94, 17);
            this.m_Selector.Name = "m_Selector";
            this.m_Selector.RequiresPrivateKey = false;
            this.m_Selector.Size = new System.Drawing.Size(315, 28);
            this.m_Selector.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "PFX file";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(315, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "Convert";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnConvert);
            // 
            // FrmPfxToPem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 106);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_Selector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPfxToPem";
            this.Text = "PFX to PEM Converter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.CertificateSelector m_Selector;
        private Label label1;
        private Button button1;
    }
}