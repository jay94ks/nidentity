namespace NIdentity.Core.X509.Controls
{
    partial class CertificatePermissionEditor
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
            this.m_Apply = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.m_CanAlter = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.m_CanDelete = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.m_CanRevoke = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.m_CanList = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.m_CanGenerate = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_CanAuthorityInterfere = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.m_Description = new System.Windows.Forms.Label();
            this.m_Accessor = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_Apply);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.panel5);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 431);
            this.panel1.TabIndex = 0;
            // 
            // m_Apply
            // 
            this.m_Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Apply.Location = new System.Drawing.Point(296, 389);
            this.m_Apply.Name = "m_Apply";
            this.m_Apply.Size = new System.Drawing.Size(126, 29);
            this.m_Apply.TabIndex = 7;
            this.m_Apply.Text = "Apply";
            this.m_Apply.UseVisualStyleBackColor = true;
            this.m_Apply.Click += new System.EventHandler(this.OnApply);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.m_CanAlter);
            this.panel8.Controls.Add(this.label6);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 331);
            this.panel8.Name = "panel8";
            this.panel8.Padding = new System.Windows.Forms.Padding(10);
            this.panel8.Size = new System.Drawing.Size(432, 47);
            this.panel8.TabIndex = 6;
            // 
            // m_CanAlter
            // 
            this.m_CanAlter.AutoSize = true;
            this.m_CanAlter.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_CanAlter.Location = new System.Drawing.Point(404, 10);
            this.m_CanAlter.Name = "m_CanAlter";
            this.m_CanAlter.Size = new System.Drawing.Size(18, 27);
            this.m_CanAlter.TabIndex = 1;
            this.m_CanAlter.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(10, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(250, 27);
            this.label6.TabIndex = 0;
            this.label6.Text = "Can Alter Permissions?";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.m_CanDelete);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 284);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(10);
            this.panel7.Size = new System.Drawing.Size(432, 47);
            this.panel7.TabIndex = 5;
            // 
            // m_CanDelete
            // 
            this.m_CanDelete.AutoSize = true;
            this.m_CanDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_CanDelete.Location = new System.Drawing.Point(404, 10);
            this.m_CanDelete.Name = "m_CanDelete";
            this.m_CanDelete.Size = new System.Drawing.Size(18, 27);
            this.m_CanDelete.TabIndex = 1;
            this.m_CanDelete.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(10, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(250, 27);
            this.label5.TabIndex = 0;
            this.label5.Text = "Can Delete Child Certificates?";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.m_CanRevoke);
            this.panel6.Controls.Add(this.label4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 237);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(10);
            this.panel6.Size = new System.Drawing.Size(432, 47);
            this.panel6.TabIndex = 4;
            // 
            // m_CanRevoke
            // 
            this.m_CanRevoke.AutoSize = true;
            this.m_CanRevoke.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_CanRevoke.Location = new System.Drawing.Point(404, 10);
            this.m_CanRevoke.Name = "m_CanRevoke";
            this.m_CanRevoke.Size = new System.Drawing.Size(18, 27);
            this.m_CanRevoke.TabIndex = 1;
            this.m_CanRevoke.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(10, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(250, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "Can Revoke Child Certificates?";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.m_CanList);
            this.panel5.Controls.Add(this.label3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 190);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(10);
            this.panel5.Size = new System.Drawing.Size(432, 47);
            this.panel5.TabIndex = 3;
            // 
            // m_CanList
            // 
            this.m_CanList.AutoSize = true;
            this.m_CanList.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_CanList.Location = new System.Drawing.Point(404, 10);
            this.m_CanList.Name = "m_CanList";
            this.m_CanList.Size = new System.Drawing.Size(18, 27);
            this.m_CanList.TabIndex = 1;
            this.m_CanList.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(10, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(250, 27);
            this.label3.TabIndex = 0;
            this.label3.Text = "Can List Child Certificates?";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.m_CanGenerate);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 143);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(10);
            this.panel4.Size = new System.Drawing.Size(432, 47);
            this.panel4.TabIndex = 2;
            // 
            // m_CanGenerate
            // 
            this.m_CanGenerate.AutoSize = true;
            this.m_CanGenerate.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_CanGenerate.Location = new System.Drawing.Point(404, 10);
            this.m_CanGenerate.Name = "m_CanGenerate";
            this.m_CanGenerate.Size = new System.Drawing.Size(18, 27);
            this.m_CanGenerate.TabIndex = 1;
            this.m_CanGenerate.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(309, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Can Generate Intermedidate or Leafs?";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.m_CanAuthorityInterfere);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 96);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10);
            this.panel3.Size = new System.Drawing.Size(432, 47);
            this.panel3.TabIndex = 1;
            // 
            // m_CanAuthorityInterfere
            // 
            this.m_CanAuthorityInterfere.AutoSize = true;
            this.m_CanAuthorityInterfere.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_CanAuthorityInterfere.Location = new System.Drawing.Point(404, 10);
            this.m_CanAuthorityInterfere.Name = "m_CanAuthorityInterfere";
            this.m_CanAuthorityInterfere.Size = new System.Drawing.Size(18, 27);
            this.m_CanAuthorityInterfere.TabIndex = 1;
            this.m_CanAuthorityInterfere.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Can Authority Interfere?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.m_Description);
            this.panel2.Controls.Add(this.m_Accessor);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(432, 96);
            this.panel2.TabIndex = 0;
            // 
            // m_Description
            // 
            this.m_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Description.Location = new System.Drawing.Point(10, 10);
            this.m_Description.Name = "m_Description";
            this.m_Description.Size = new System.Drawing.Size(412, 56);
            this.m_Description.TabIndex = 0;
            this.m_Description.Text = "Specifies the authority to be applied when the specified {Accessor} accesses {Sub" +
    "ject}.";
            // 
            // m_Accessor
            // 
            this.m_Accessor.AutoSize = true;
            this.m_Accessor.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_Accessor.Location = new System.Drawing.Point(10, 66);
            this.m_Accessor.Name = "m_Accessor";
            this.m_Accessor.Size = new System.Drawing.Size(143, 20);
            this.m_Accessor.TabIndex = 1;
            this.m_Accessor.Text = "Accessor: {Accessor)";
            // 
            // CertificatePermissionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 431);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CertificatePermissionEditor";
            this.Text = "Permission Editor";
            this.panel1.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label m_Description;
        private Label m_Accessor;
        private Panel panel3;
        private Label label1;
        private CheckBox m_CanAuthorityInterfere;
        private Panel panel8;
        private CheckBox m_CanAlter;
        private Label label6;
        private Panel panel7;
        private CheckBox m_CanDelete;
        private Label label5;
        private Panel panel6;
        private CheckBox m_CanRevoke;
        private Label label4;
        private Panel panel5;
        private CheckBox m_CanList;
        private Label label3;
        private Panel panel4;
        private CheckBox m_CanGenerate;
        private Label label2;
        private Button m_Apply;
    }
}