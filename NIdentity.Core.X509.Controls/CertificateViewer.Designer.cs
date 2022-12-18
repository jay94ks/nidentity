namespace NIdentity.Core.X509.Controls
{
    partial class CertificateViewer
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.m_Subject = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_KeyIdentifier = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.m_Issuer = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.m_IssuerKeyIdentifier = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.m_SerialNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.m_Thumbprint = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.m_CreationTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.m_ExpirationTime = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.m_RevokeReason = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.m_AlternativeNames = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.m_IsSelfSigned = new System.Windows.Forms.CheckBox();
            this.m_IsAuthority = new System.Windows.Forms.CheckBox();
            this.m_HasPrivateKey = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel12);
            this.panel1.Controls.Add(this.panel11);
            this.panel1.Controls.Add(this.panel10);
            this.panel1.Controls.Add(this.panel9);
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
            this.panel1.Size = new System.Drawing.Size(718, 264);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.m_Subject);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(695, 47);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Subject";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_Subject
            // 
            this.m_Subject.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_Subject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Subject.Location = new System.Drawing.Point(183, 10);
            this.m_Subject.Name = "m_Subject";
            this.m_Subject.ReadOnly = true;
            this.m_Subject.Size = new System.Drawing.Size(502, 27);
            this.m_Subject.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.m_KeyIdentifier);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 47);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10);
            this.panel3.Size = new System.Drawing.Size(695, 47);
            this.panel3.TabIndex = 1;
            // 
            // m_KeyIdentifier
            // 
            this.m_KeyIdentifier.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_KeyIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_KeyIdentifier.Location = new System.Drawing.Point(183, 10);
            this.m_KeyIdentifier.Name = "m_KeyIdentifier";
            this.m_KeyIdentifier.ReadOnly = true;
            this.m_KeyIdentifier.Size = new System.Drawing.Size(502, 27);
            this.m_KeyIdentifier.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Key Identifier";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.m_Issuer);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 94);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(10);
            this.panel4.Size = new System.Drawing.Size(695, 47);
            this.panel4.TabIndex = 2;
            // 
            // m_Issuer
            // 
            this.m_Issuer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_Issuer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Issuer.Location = new System.Drawing.Point(181, 10);
            this.m_Issuer.Name = "m_Issuer";
            this.m_Issuer.ReadOnly = true;
            this.m_Issuer.Size = new System.Drawing.Size(504, 27);
            this.m_Issuer.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(10, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 27);
            this.label3.TabIndex = 0;
            this.label3.Text = "Issuer";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.m_IssuerKeyIdentifier);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 141);
            this.panel5.Name = "panel5";
            this.panel5.Padding = new System.Windows.Forms.Padding(10);
            this.panel5.Size = new System.Drawing.Size(695, 47);
            this.panel5.TabIndex = 3;
            // 
            // m_IssuerKeyIdentifier
            // 
            this.m_IssuerKeyIdentifier.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_IssuerKeyIdentifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_IssuerKeyIdentifier.Location = new System.Drawing.Point(181, 10);
            this.m_IssuerKeyIdentifier.Name = "m_IssuerKeyIdentifier";
            this.m_IssuerKeyIdentifier.ReadOnly = true;
            this.m_IssuerKeyIdentifier.Size = new System.Drawing.Size(504, 27);
            this.m_IssuerKeyIdentifier.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(10, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "Issuer Key Identifier";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.m_SerialNumber);
            this.panel6.Controls.Add(this.label5);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 188);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(10);
            this.panel6.Size = new System.Drawing.Size(695, 47);
            this.panel6.TabIndex = 4;
            // 
            // m_SerialNumber
            // 
            this.m_SerialNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_SerialNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_SerialNumber.Location = new System.Drawing.Point(183, 10);
            this.m_SerialNumber.Name = "m_SerialNumber";
            this.m_SerialNumber.ReadOnly = true;
            this.m_SerialNumber.Size = new System.Drawing.Size(502, 27);
            this.m_SerialNumber.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(10, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 27);
            this.label5.TabIndex = 0;
            this.label5.Text = "Serial Number";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.m_Thumbprint);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 235);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(10);
            this.panel7.Size = new System.Drawing.Size(695, 47);
            this.panel7.TabIndex = 5;
            // 
            // m_Thumbprint
            // 
            this.m_Thumbprint.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_Thumbprint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_Thumbprint.Location = new System.Drawing.Point(183, 10);
            this.m_Thumbprint.Name = "m_Thumbprint";
            this.m_Thumbprint.ReadOnly = true;
            this.m_Thumbprint.Size = new System.Drawing.Size(502, 27);
            this.m_Thumbprint.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(10, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(173, 27);
            this.label6.TabIndex = 0;
            this.label6.Text = "Thumbprint";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.m_CreationTime);
            this.panel8.Controls.Add(this.label7);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 282);
            this.panel8.Name = "panel8";
            this.panel8.Padding = new System.Windows.Forms.Padding(10);
            this.panel8.Size = new System.Drawing.Size(695, 47);
            this.panel8.TabIndex = 6;
            // 
            // m_CreationTime
            // 
            this.m_CreationTime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_CreationTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_CreationTime.Location = new System.Drawing.Point(183, 10);
            this.m_CreationTime.Name = "m_CreationTime";
            this.m_CreationTime.ReadOnly = true;
            this.m_CreationTime.Size = new System.Drawing.Size(502, 27);
            this.m_CreationTime.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(10, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(173, 27);
            this.label7.TabIndex = 0;
            this.label7.Text = "Creation Time";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.m_ExpirationTime);
            this.panel9.Controls.Add(this.label8);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(0, 329);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new System.Windows.Forms.Padding(10);
            this.panel9.Size = new System.Drawing.Size(695, 47);
            this.panel9.TabIndex = 7;
            // 
            // m_ExpirationTime
            // 
            this.m_ExpirationTime.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_ExpirationTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_ExpirationTime.Location = new System.Drawing.Point(183, 10);
            this.m_ExpirationTime.Name = "m_ExpirationTime";
            this.m_ExpirationTime.ReadOnly = true;
            this.m_ExpirationTime.Size = new System.Drawing.Size(502, 27);
            this.m_ExpirationTime.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Left;
            this.label8.Location = new System.Drawing.Point(10, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(173, 27);
            this.label8.TabIndex = 0;
            this.label8.Text = "Expiration Time";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.m_RevokeReason);
            this.panel10.Controls.Add(this.label9);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(0, 376);
            this.panel10.Name = "panel10";
            this.panel10.Padding = new System.Windows.Forms.Padding(10);
            this.panel10.Size = new System.Drawing.Size(695, 47);
            this.panel10.TabIndex = 8;
            // 
            // m_RevokeReason
            // 
            this.m_RevokeReason.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_RevokeReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_RevokeReason.Location = new System.Drawing.Point(183, 10);
            this.m_RevokeReason.Name = "m_RevokeReason";
            this.m_RevokeReason.ReadOnly = true;
            this.m_RevokeReason.Size = new System.Drawing.Size(502, 27);
            this.m_RevokeReason.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.Location = new System.Drawing.Point(10, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(173, 27);
            this.label9.TabIndex = 0;
            this.label9.Text = "Revoke Reason";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.m_AlternativeNames);
            this.panel11.Controls.Add(this.label10);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 423);
            this.panel11.Name = "panel11";
            this.panel11.Padding = new System.Windows.Forms.Padding(10);
            this.panel11.Size = new System.Drawing.Size(695, 47);
            this.panel11.TabIndex = 9;
            // 
            // m_AlternativeNames
            // 
            this.m_AlternativeNames.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_AlternativeNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_AlternativeNames.Location = new System.Drawing.Point(183, 10);
            this.m_AlternativeNames.Name = "m_AlternativeNames";
            this.m_AlternativeNames.ReadOnly = true;
            this.m_AlternativeNames.Size = new System.Drawing.Size(502, 27);
            this.m_AlternativeNames.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Left;
            this.label10.Location = new System.Drawing.Point(10, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(173, 27);
            this.label10.TabIndex = 0;
            this.label10.Text = "Alternative Names";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.panel13);
            this.panel12.Controls.Add(this.label11);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel12.Location = new System.Drawing.Point(0, 470);
            this.panel12.Name = "panel12";
            this.panel12.Padding = new System.Windows.Forms.Padding(10);
            this.panel12.Size = new System.Drawing.Size(695, 47);
            this.panel12.TabIndex = 10;
            // 
            // label11
            // 
            this.label11.Dock = System.Windows.Forms.DockStyle.Left;
            this.label11.Location = new System.Drawing.Point(10, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(173, 27);
            this.label11.TabIndex = 0;
            this.label11.Text = "Flags";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.m_HasPrivateKey);
            this.panel13.Controls.Add(this.m_IsAuthority);
            this.panel13.Controls.Add(this.m_IsSelfSigned);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel13.Location = new System.Drawing.Point(183, 10);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(502, 27);
            this.panel13.TabIndex = 1;
            // 
            // m_IsSelfSigned
            // 
            this.m_IsSelfSigned.AutoSize = true;
            this.m_IsSelfSigned.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_IsSelfSigned.Enabled = false;
            this.m_IsSelfSigned.Location = new System.Drawing.Point(0, 0);
            this.m_IsSelfSigned.Name = "m_IsSelfSigned";
            this.m_IsSelfSigned.Size = new System.Drawing.Size(123, 27);
            this.m_IsSelfSigned.TabIndex = 0;
            this.m_IsSelfSigned.Text = "Is Self Signed";
            this.m_IsSelfSigned.UseVisualStyleBackColor = true;
            // 
            // m_IsAuthority
            // 
            this.m_IsAuthority.AutoSize = true;
            this.m_IsAuthority.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_IsAuthority.Enabled = false;
            this.m_IsAuthority.Location = new System.Drawing.Point(123, 0);
            this.m_IsAuthority.Name = "m_IsAuthority";
            this.m_IsAuthority.Size = new System.Drawing.Size(109, 27);
            this.m_IsAuthority.TabIndex = 1;
            this.m_IsAuthority.Text = "Is Authority";
            this.m_IsAuthority.UseVisualStyleBackColor = true;
            // 
            // m_HasPrivateKey
            // 
            this.m_HasPrivateKey.AutoSize = true;
            this.m_HasPrivateKey.Dock = System.Windows.Forms.DockStyle.Left;
            this.m_HasPrivateKey.Enabled = false;
            this.m_HasPrivateKey.Location = new System.Drawing.Point(232, 0);
            this.m_HasPrivateKey.Name = "m_HasPrivateKey";
            this.m_HasPrivateKey.Size = new System.Drawing.Size(136, 27);
            this.m_HasPrivateKey.TabIndex = 2;
            this.m_HasPrivateKey.Text = "Has Private Key";
            this.m_HasPrivateKey.UseVisualStyleBackColor = true;
            // 
            // CertificateView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "CertificateView";
            this.Size = new System.Drawing.Size(718, 264);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel12.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private TextBox m_Subject;
        private Panel panel3;
        private TextBox m_KeyIdentifier;
        private Label label2;
        private Panel panel5;
        private TextBox m_IssuerKeyIdentifier;
        private Label label4;
        private Panel panel4;
        private TextBox m_Issuer;
        private Label label3;
        private Panel panel10;
        private TextBox m_RevokeReason;
        private Label label9;
        private Panel panel9;
        private TextBox m_ExpirationTime;
        private Label label8;
        private Panel panel8;
        private TextBox m_CreationTime;
        private Label label7;
        private Panel panel7;
        private TextBox m_Thumbprint;
        private Label label6;
        private Panel panel6;
        private TextBox m_SerialNumber;
        private Label label5;
        private Panel panel11;
        private TextBox m_AlternativeNames;
        private Label label10;
        private Panel panel12;
        private Panel panel13;
        private Label label11;
        private CheckBox m_HasPrivateKey;
        private CheckBox m_IsAuthority;
        private CheckBox m_IsSelfSigned;
    }
}
