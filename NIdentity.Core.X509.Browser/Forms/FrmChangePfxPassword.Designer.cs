namespace NIdentity.Core.X509.Browser.Forms
{
    partial class FrmChangePfxPassword
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
            this.m_LblSource = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_LblDestination = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_LblSource
            // 
            this.m_LblSource.BackColor = System.Drawing.Color.White;
            this.m_LblSource.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_LblSource.Location = new System.Drawing.Point(162, 28);
            this.m_LblSource.Name = "m_LblSource";
            this.m_LblSource.Size = new System.Drawing.Size(337, 27);
            this.m_LblSource.TabIndex = 5;
            this.m_LblSource.Text = "(Nothing Selected)";
            this.m_LblSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.m_LblSource.Click += new System.EventHandler(this.m_LblAuthority_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Source PFX";
            // 
            // m_LblDestination
            // 
            this.m_LblDestination.BackColor = System.Drawing.Color.White;
            this.m_LblDestination.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.m_LblDestination.Location = new System.Drawing.Point(162, 114);
            this.m_LblDestination.Name = "m_LblDestination";
            this.m_LblDestination.Size = new System.Drawing.Size(337, 27);
            this.m_LblDestination.TabIndex = 7;
            this.m_LblDestination.Text = "(Nothing Selected)";
            this.m_LblDestination.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.m_LblDestination.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Destination PFX";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(162, 70);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(337, 27);
            this.textBox1.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Source Pass";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 159);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Destination Pass";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(162, 156);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(337, 27);
            this.textBox2.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(365, 200);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 29);
            this.button1.TabIndex = 12;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmChangePfxPassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 253);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.m_LblDestination);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_LblSource);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChangePfxPassword";
            this.Text = "NIdentity X509 PFX Password changer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label m_LblSource;
        private Label label2;
        private Label m_LblDestination;
        private Label label3;
        private TextBox textBox1;
        private Label label4;
        private Label label5;
        private TextBox textBox2;
        private Button button1;
    }
}