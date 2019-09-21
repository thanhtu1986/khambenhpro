namespace KhamBenhPro
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.DSChoKham = new System.Windows.Forms.ToolStripMenuItem();
            this.DSDaKham = new System.Windows.Forms.ToolStripMenuItem();
            this.quảnLýToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phiếuYêuCầuLãnhToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phiếuYêuCầuTrảToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.báoCáoXNTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.báoCáoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mẫuA1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xuấtXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TabCtrl_main = new DevComponents.DotNetBar.TabControl();
            this.kếtNốiDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabCtrl_main)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DSChoKham,
            this.DSDaKham,
            this.quảnLýToolStripMenuItem,
            this.báoCáoToolStripMenuItem,
            this.xuấtXMLToolStripMenuItem,
            this.kếtNốiDBToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1350, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // DSChoKham
            // 
            this.DSChoKham.Name = "DSChoKham";
            this.DSChoKham.Size = new System.Drawing.Size(89, 20);
            this.DSChoKham.Text = "DS chờ khám";
            this.DSChoKham.Click += new System.EventHandler(this.DSChoKham_Click);
            // 
            // DSDaKham
            // 
            this.DSDaKham.Name = "DSDaKham";
            this.DSDaKham.Size = new System.Drawing.Size(82, 20);
            this.DSDaKham.Text = "DS đã khám";
            this.DSDaKham.Click += new System.EventHandler(this.DSDaKham_Click);
            // 
            // quảnLýToolStripMenuItem
            // 
            this.quảnLýToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.phiếuYêuCầuLãnhToolStripMenuItem,
            this.phiếuYêuCầuTrảToolStripMenuItem,
            this.báoCáoXNTToolStripMenuItem});
            this.quảnLýToolStripMenuItem.Name = "quảnLýToolStripMenuItem";
            this.quảnLýToolStripMenuItem.Size = new System.Drawing.Size(104, 20);
            this.quảnLýToolStripMenuItem.Text = "Quản lý tồn kho";
            // 
            // phiếuYêuCầuLãnhToolStripMenuItem
            // 
            this.phiếuYêuCầuLãnhToolStripMenuItem.Name = "phiếuYêuCầuLãnhToolStripMenuItem";
            this.phiếuYêuCầuLãnhToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.phiếuYêuCầuLãnhToolStripMenuItem.Text = "Phiếu yêu cầu lãnh";
            // 
            // phiếuYêuCầuTrảToolStripMenuItem
            // 
            this.phiếuYêuCầuTrảToolStripMenuItem.Name = "phiếuYêuCầuTrảToolStripMenuItem";
            this.phiếuYêuCầuTrảToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.phiếuYêuCầuTrảToolStripMenuItem.Text = "Phiếu yêu cầu trả";
            // 
            // báoCáoXNTToolStripMenuItem
            // 
            this.báoCáoXNTToolStripMenuItem.Name = "báoCáoXNTToolStripMenuItem";
            this.báoCáoXNTToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.báoCáoXNTToolStripMenuItem.Text = "Báo cáo XNT";
            // 
            // báoCáoToolStripMenuItem
            // 
            this.báoCáoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mẫuA1ToolStripMenuItem});
            this.báoCáoToolStripMenuItem.Name = "báoCáoToolStripMenuItem";
            this.báoCáoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.báoCáoToolStripMenuItem.Text = "Báo cáo";
            // 
            // mẫuA1ToolStripMenuItem
            // 
            this.mẫuA1ToolStripMenuItem.Name = "mẫuA1ToolStripMenuItem";
            this.mẫuA1ToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.mẫuA1ToolStripMenuItem.Text = "Mẫu A1";
            // 
            // xuấtXMLToolStripMenuItem
            // 
            this.xuấtXMLToolStripMenuItem.Name = "xuấtXMLToolStripMenuItem";
            this.xuấtXMLToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.xuấtXMLToolStripMenuItem.Text = "Xuất XML";
            this.xuấtXMLToolStripMenuItem.Click += new System.EventHandler(this.xuấtXMLToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // TabCtrl_main
            // 
            this.TabCtrl_main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.TabCtrl_main.CanReorderTabs = true;
            this.TabCtrl_main.CloseButtonOnTabsVisible = true;
            this.TabCtrl_main.ColorScheme.TabBackground = System.Drawing.Color.Gray;
            this.TabCtrl_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabCtrl_main.Location = new System.Drawing.Point(0, 24);
            this.TabCtrl_main.Name = "TabCtrl_main";
            this.TabCtrl_main.SelectedTabFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.TabCtrl_main.SelectedTabIndex = -1;
            this.TabCtrl_main.Size = new System.Drawing.Size(1350, 705);
            this.TabCtrl_main.TabIndex = 2;
            this.TabCtrl_main.TabLayoutType = DevComponents.DotNetBar.eTabLayoutType.FixedWithNavigationBox;
            this.TabCtrl_main.Text = "tabControl1";
            this.TabCtrl_main.TabItemClose += new DevComponents.DotNetBar.TabStrip.UserActionEventHandler(this.TabCtrl_main_TabItemClose);
            // 
            // kếtNốiDBToolStripMenuItem
            // 
            this.kếtNốiDBToolStripMenuItem.Name = "kếtNốiDBToolStripMenuItem";
            this.kếtNốiDBToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.kếtNốiDBToolStripMenuItem.Text = "Kết nối DB";
            this.kếtNốiDBToolStripMenuItem.Click += new System.EventHandler(this.kếtNốiDBToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.TabCtrl_main);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Phân hệ Khám bệnh";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabCtrl_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem DSChoKham;
        private System.Windows.Forms.ToolStripMenuItem DSDaKham;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public DevComponents.DotNetBar.TabControl TabCtrl_main;
        private System.Windows.Forms.ToolStripMenuItem quảnLýToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phiếuYêuCầuLãnhToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phiếuYêuCầuTrảToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem báoCáoXNTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem báoCáoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mẫuA1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xuấtXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kếtNốiDBToolStripMenuItem;
    }
}

