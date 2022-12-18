namespace NIdentity.Core.X509.Controls
{
    partial class CertificateSelector
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
            this.m_EditCertificate = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.m_EditCertificate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(214, 28);
            this.panel1.TabIndex = 0;
            // 
            // m_EditCertificate
            // 
            this.m_EditCertificate.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_EditCertificate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_EditCertificate.Location = new System.Drawing.Point(0, 0);
            this.m_EditCertificate.Name = "m_EditCertificate";
            this.m_EditCertificate.ReadOnly = true;
            this.m_EditCertificate.Size = new System.Drawing.Size(214, 27);
            this.m_EditCertificate.TabIndex = 0;
            this.m_EditCertificate.Click += new System.EventHandler(this.OpenSelector);
            // 
            // CertificateSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.panel1);
            this.Name = "CertificateSelector";
            this.Size = new System.Drawing.Size(214, 28);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panel1;
        private TextBox m_EditCertificate;
    }
}
