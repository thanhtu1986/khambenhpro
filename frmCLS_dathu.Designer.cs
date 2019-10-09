namespace KhamBenhPro
{
    partial class frmCLS_dathu
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
            this.rptviewdathu = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // rptviewdathu
            // 
            this.rptviewdathu.ActiveViewIndex = -1;
            this.rptviewdathu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rptviewdathu.Cursor = System.Windows.Forms.Cursors.Default;
            this.rptviewdathu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptviewdathu.Location = new System.Drawing.Point(0, 0);
            this.rptviewdathu.Name = "rptviewdathu";
            this.rptviewdathu.Size = new System.Drawing.Size(742, 469);
            this.rptviewdathu.TabIndex = 0;
            this.rptviewdathu.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // frmCLS_dathu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 469);
            this.Controls.Add(this.rptviewdathu);
            this.Name = "frmCLS_dathu";
            this.Text = "frmCLS_dathu";
            this.Load += new System.EventHandler(this.frmCLS_dathu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer rptviewdathu;
    }
}