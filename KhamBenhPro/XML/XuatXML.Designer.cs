namespace KhamBenhPro.XML
{
    partial class XuatXML
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.IsBase64 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.txtMA_LK = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTenBN = new System.Windows.Forms.TextBox();
            this.btnKetXuat = new System.Windows.Forms.Button();
            this.btnLayDS = new System.Windows.Forms.Button();
            this.dtgvXML = new System.Windows.Forms.DataGridView();
            this.txtErro = new System.Windows.Forms.TextBox();
            this.txtMaBN = new System.Windows.Forms.TextBox();
            this.txtXN = new System.Windows.Forms.TextBox();
            this.chbHaveXML45 = new System.Windows.Forms.CheckBox();
            this.btnSavetoDB = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvXML)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Đường dẫn";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(89, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(390, 20);
            this.txtPath.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(485, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Chon thư mục";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // IsBase64
            // 
            this.IsBase64.AutoSize = true;
            this.IsBase64.Location = new System.Drawing.Point(26, 44);
            this.IsBase64.Name = "IsBase64";
            this.IsBase64.Size = new System.Drawing.Size(62, 17);
            this.IsBase64.TabIndex = 3;
            this.IsBase64.Text = "Base64";
            this.IsBase64.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ngày ra viện";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd/MM/yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(167, 41);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(94, 20);
            this.dtpFromDate.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(267, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "đến";
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd/MM/yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(299, 41);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(94, 20);
            this.dtpToDate.TabIndex = 7;
            // 
            // txtMA_LK
            // 
            this.txtMA_LK.Location = new System.Drawing.Point(465, 41);
            this.txtMA_LK.Name = "txtMA_LK";
            this.txtMA_LK.Size = new System.Drawing.Size(105, 20);
            this.txtMA_LK.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(417, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Mã LK";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(578, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Tên BN";
            // 
            // txtTenBN
            // 
            this.txtTenBN.Location = new System.Drawing.Point(628, 41);
            this.txtTenBN.Name = "txtTenBN";
            this.txtTenBN.Size = new System.Drawing.Size(125, 20);
            this.txtTenBN.TabIndex = 10;
            // 
            // btnKetXuat
            // 
            this.btnKetXuat.Location = new System.Drawing.Point(764, 40);
            this.btnKetXuat.Name = "btnKetXuat";
            this.btnKetXuat.Size = new System.Drawing.Size(92, 23);
            this.btnKetXuat.TabIndex = 12;
            this.btnKetXuat.Text = "Kết Xuất";
            this.btnKetXuat.UseVisualStyleBackColor = true;
            this.btnKetXuat.Click += new System.EventHandler(this.btnKetXuat_Click);
            // 
            // btnLayDS
            // 
            this.btnLayDS.Location = new System.Drawing.Point(870, 40);
            this.btnLayDS.Name = "btnLayDS";
            this.btnLayDS.Size = new System.Drawing.Size(92, 23);
            this.btnLayDS.TabIndex = 13;
            this.btnLayDS.Text = "Lấy DS";
            this.btnLayDS.UseVisualStyleBackColor = true;
            this.btnLayDS.Click += new System.EventHandler(this.btnLayDS_Click);
            // 
            // dtgvXML
            // 
            this.dtgvXML.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvXML.Location = new System.Drawing.Point(4, 91);
            this.dtgvXML.Name = "dtgvXML";
            this.dtgvXML.Size = new System.Drawing.Size(1088, 323);
            this.dtgvXML.TabIndex = 14;
            // 
            // txtErro
            // 
            this.txtErro.Location = new System.Drawing.Point(26, 67);
            this.txtErro.Name = "txtErro";
            this.txtErro.Size = new System.Drawing.Size(367, 20);
            this.txtErro.TabIndex = 15;
            // 
            // txtMaBN
            // 
            this.txtMaBN.Location = new System.Drawing.Point(517, 67);
            this.txtMaBN.Name = "txtMaBN";
            this.txtMaBN.Size = new System.Drawing.Size(105, 20);
            this.txtMaBN.TabIndex = 16;
            // 
            // txtXN
            // 
            this.txtXN.Location = new System.Drawing.Point(638, 65);
            this.txtXN.Name = "txtXN";
            this.txtXN.Size = new System.Drawing.Size(105, 20);
            this.txtXN.TabIndex = 17;
            // 
            // chbHaveXML45
            // 
            this.chbHaveXML45.AutoSize = true;
            this.chbHaveXML45.Location = new System.Drawing.Point(794, 69);
            this.chbHaveXML45.Name = "chbHaveXML45";
            this.chbHaveXML45.Size = new System.Drawing.Size(56, 17);
            this.chbHaveXML45.TabIndex = 18;
            this.chbHaveXML45.Text = "xml4 5";
            this.chbHaveXML45.UseVisualStyleBackColor = true;
            // 
            // btnSavetoDB
            // 
            this.btnSavetoDB.Location = new System.Drawing.Point(986, 40);
            this.btnSavetoDB.Name = "btnSavetoDB";
            this.btnSavetoDB.Size = new System.Drawing.Size(75, 23);
            this.btnSavetoDB.TabIndex = 19;
            this.btnSavetoDB.Text = "To DB";
            this.btnSavetoDB.UseVisualStyleBackColor = true;
            this.btnSavetoDB.Click += new System.EventHandler(this.btnSavetoDB_Click);
            // 
            // XuatXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 426);
            this.Controls.Add(this.btnSavetoDB);
            this.Controls.Add(this.chbHaveXML45);
            this.Controls.Add(this.txtXN);
            this.Controls.Add(this.txtMaBN);
            this.Controls.Add(this.txtErro);
            this.Controls.Add(this.dtgvXML);
            this.Controls.Add(this.btnLayDS);
            this.Controls.Add(this.btnKetXuat);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtTenBN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMA_LK);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IsBase64);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label1);
            this.Name = "XuatXML";
            this.Text = "XuatXML";
            this.Load += new System.EventHandler(this.XuatXML_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvXML)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox IsBase64;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.TextBox txtMA_LK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTenBN;
        private System.Windows.Forms.Button btnKetXuat;
        private System.Windows.Forms.Button btnLayDS;
        private System.Windows.Forms.DataGridView dtgvXML;
        private System.Windows.Forms.TextBox txtErro;
        private System.Windows.Forms.TextBox txtMaBN;
        private System.Windows.Forms.TextBox txtXN;
        private System.Windows.Forms.CheckBox chbHaveXML45;
        private System.Windows.Forms.Button btnSavetoDB;
    }
}