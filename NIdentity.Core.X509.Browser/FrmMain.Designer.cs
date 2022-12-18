namespace NIdentity.Core.X509.Browser
{
    partial class FrmMain
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.browserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_Settings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_Connect = new System.Windows.Forms.ToolStripMenuItem();
            this.m_Disconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_WindowSplitter = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.m_CertTree = new NIdentity.Core.X509.Controls.CertificateTreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.m_MenuGenerate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.m_MenuRevoke = new System.Windows.Forms.ToolStripMenuItem();
            this.m_MenuUnrevoke = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.m_MenuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.m_PropGrid = new System.Windows.Forms.PropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.m_CertList = new NIdentity.Core.X509.Controls.CertificateListView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_WindowSplitter)).BeginInit();
            this.m_WindowSplitter.Panel1.SuspendLayout();
            this.m_WindowSplitter.Panel2.SuspendLayout();
            this.m_WindowSplitter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browserToolStripMenuItem,
            this.keysToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1070, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // browserToolStripMenuItem
            // 
            this.browserToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_Settings,
            this.toolStripMenuItem1,
            this.m_Connect,
            this.m_Disconnect,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.browserToolStripMenuItem.Name = "browserToolStripMenuItem";
            this.browserToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.browserToolStripMenuItem.Text = "Browser";
            // 
            // m_Settings
            // 
            this.m_Settings.Name = "m_Settings";
            this.m_Settings.Size = new System.Drawing.Size(167, 26);
            this.m_Settings.Text = "Settings";
            this.m_Settings.Click += new System.EventHandler(this.OnSettings);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(164, 6);
            // 
            // m_Connect
            // 
            this.m_Connect.Name = "m_Connect";
            this.m_Connect.Size = new System.Drawing.Size(167, 26);
            this.m_Connect.Text = "Connect";
            this.m_Connect.Click += new System.EventHandler(this.OnConnect);
            // 
            // m_Disconnect
            // 
            this.m_Disconnect.Enabled = false;
            this.m_Disconnect.Name = "m_Disconnect";
            this.m_Disconnect.Size = new System.Drawing.Size(167, 26);
            this.m_Disconnect.Text = "Disconnect";
            this.m_Disconnect.Click += new System.EventHandler(this.OnDisconnect);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(164, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(167, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExit);
            // 
            // keysToolStripMenuItem
            // 
            this.keysToolStripMenuItem.Name = "keysToolStripMenuItem";
            this.keysToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.keysToolStripMenuItem.Text = "Keys";
            // 
            // m_WindowSplitter
            // 
            this.m_WindowSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_WindowSplitter.Location = new System.Drawing.Point(0, 28);
            this.m_WindowSplitter.Name = "m_WindowSplitter";
            // 
            // m_WindowSplitter.Panel1
            // 
            this.m_WindowSplitter.Panel1.Controls.Add(this.splitContainer2);
            // 
            // m_WindowSplitter.Panel2
            // 
            this.m_WindowSplitter.Panel2.Controls.Add(this.tabControl1);
            this.m_WindowSplitter.Size = new System.Drawing.Size(1070, 560);
            this.m_WindowSplitter.SplitterDistance = 207;
            this.m_WindowSplitter.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.m_CertTree);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.m_PropGrid);
            this.splitContainer2.Size = new System.Drawing.Size(207, 560);
            this.splitContainer2.SplitterDistance = 274;
            this.splitContainer2.TabIndex = 0;
            // 
            // m_CertTree
            // 
            this.m_CertTree.Authority = null;
            this.m_CertTree.ContextMenuStrip = this.contextMenuStrip1;
            this.m_CertTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_CertTree.FullRowSelect = true;
            this.m_CertTree.HideSelection = false;
            this.m_CertTree.Location = new System.Drawing.Point(0, 0);
            this.m_CertTree.Name = "m_CertTree";
            this.m_CertTree.Size = new System.Drawing.Size(207, 274);
            this.m_CertTree.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openInfoToolStripMenuItem,
            this.toolStripMenuItem5,
            this.m_MenuGenerate,
            this.toolStripMenuItem3,
            this.m_MenuRevoke,
            this.m_MenuUnrevoke,
            this.toolStripMenuItem4,
            this.m_MenuDelete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 142);
            // 
            // openInfoToolStripMenuItem
            // 
            this.openInfoToolStripMenuItem.Name = "openInfoToolStripMenuItem";
            this.openInfoToolStripMenuItem.Size = new System.Drawing.Size(148, 24);
            this.openInfoToolStripMenuItem.Text = "Open Info";
            this.openInfoToolStripMenuItem.Click += new System.EventHandler(this.OnOpenInfo);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(145, 6);
            // 
            // m_MenuGenerate
            // 
            this.m_MenuGenerate.Name = "m_MenuGenerate";
            this.m_MenuGenerate.Size = new System.Drawing.Size(148, 24);
            this.m_MenuGenerate.Text = "Generate";
            this.m_MenuGenerate.Click += new System.EventHandler(this.OnMenuGenerate);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(145, 6);
            // 
            // m_MenuRevoke
            // 
            this.m_MenuRevoke.Name = "m_MenuRevoke";
            this.m_MenuRevoke.Size = new System.Drawing.Size(148, 24);
            this.m_MenuRevoke.Text = "Revoke";
            // 
            // m_MenuUnrevoke
            // 
            this.m_MenuUnrevoke.Name = "m_MenuUnrevoke";
            this.m_MenuUnrevoke.Size = new System.Drawing.Size(148, 24);
            this.m_MenuUnrevoke.Text = "Unrevoke";
            this.m_MenuUnrevoke.Click += new System.EventHandler(this.OnUnrevokeFromNode);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(145, 6);
            // 
            // m_MenuDelete
            // 
            this.m_MenuDelete.ForeColor = System.Drawing.Color.Red;
            this.m_MenuDelete.Name = "m_MenuDelete";
            this.m_MenuDelete.Size = new System.Drawing.Size(148, 24);
            this.m_MenuDelete.Text = "Delete";
            this.m_MenuDelete.Click += new System.EventHandler(this.OnDeleteFromNode);
            // 
            // m_PropGrid
            // 
            this.m_PropGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_PropGrid.Location = new System.Drawing.Point(0, 0);
            this.m_PropGrid.Name = "m_PropGrid";
            this.m_PropGrid.Size = new System.Drawing.Size(207, 282);
            this.m_PropGrid.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(859, 560);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.m_CertList);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(851, 527);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Certificates";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // m_CertList
            // 
            this.m_CertList.Authority = null;
            this.m_CertList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_CertList.FullRowSelect = true;
            this.m_CertList.HideSelection = true;
            this.m_CertList.Location = new System.Drawing.Point(3, 3);
            this.m_CertList.Name = "m_CertList";
            this.m_CertList.Size = new System.Drawing.Size(845, 521);
            this.m_CertList.TabIndex = 0;
            this.m_CertList.UseCompatibleStateImageBehavior = false;
            this.m_CertList.View = System.Windows.Forms.View.Details;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1070, 588);
            this.Controls.Add(this.m_WindowSplitter);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.Text = "NIdentity X509 Browser";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.m_WindowSplitter.Panel1.ResumeLayout(false);
            this.m_WindowSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_WindowSplitter)).EndInit();
            this.m_WindowSplitter.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem browserToolStripMenuItem;
        private ToolStripMenuItem m_Settings;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem m_Connect;
        private ToolStripMenuItem m_Disconnect;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem keysToolStripMenuItem;
        private SplitContainer m_WindowSplitter;
        private SplitContainer splitContainer2;
        private Controls.CertificateTreeView m_CertTree;
        private PropertyGrid m_PropGrid;
        private Controls.CertificateListView m_CertList;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem m_MenuGenerate;
        private ToolStripSeparator toolStripMenuItem3;
        private ToolStripMenuItem m_MenuRevoke;
        private ToolStripMenuItem m_MenuUnrevoke;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem m_MenuDelete;
        private ToolStripMenuItem openInfoToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem5;
    }
}