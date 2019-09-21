using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.Drawing;

namespace KhamBenhPro.KhamBenh
{
    public partial class DSChoKham : Form
    {
      
        
        public Main frm1;
        public DevComponents.DotNetBar.TabControl tabCtrl_main;
        public DSChoKham()
        {
            InitializeComponent();
        }

        private void DSChoKham_Load(object sender, EventArgs e)
        {
            LoadLookupEdit1();
        }
        public void LoadLookupEdit1()
        {
            string sql = @"select p.id,p.maso+'-'+p.TenPhong as 'tenphong'
                                            from KB_Phong p
                                            inner join banggiadichvu bg on bg.idbanggiadichvu = p.DichVuKCB
                                            where bg.idphongkhambenh =1
                                            and p.isPhongNoiTru = 0
                                            and p.IsActive = 1
                                            and p.Status=1
                                            order by p.MaSo";
            DataTable dtPhong = DataAcess.Connect.GetTable(sql);
            searchLookUpEdit1.Properties.NullText = "Chọn phòng khám";
            searchLookUpEdit1.Properties.DataSource = dtPhong;
            searchLookUpEdit1.Properties.DisplayMember = "tenphong";
            searchLookUpEdit1.Properties.ValueMember = "id";
            
        }

        private void btnDScho_Click(object sender, EventArgs e)
        {
            if (searchLookUpEdit1.EditValue==null)
            {
                MessageBox.Show("Chưa chọn phòng khám?");
            }
            else
            {
                try
                {
                    string sql = @" select	bn.idbenhnhan						
													,bn.mabenhnhan
					                               ,bn.tenbenhnhan
                                                    ,bn.ngaysinh
                                                    ,(CASE WHEN bn.gioiTinh='0' THEN 'Nam' ELSE N'Nữ' END) as gioitinh
                                                    ,dk.ngaydangky
                                                    ,ut.TenLoai		
                                                    ,ct.sott
                                                    ,TypeName=N'Chờ khám'+( CASE WHEN ct.IsCoDKCLS=1 THEN ( CASE WHEN dk.ISDACLS=1 THEN N'(Đủ KQ)' ELSE   N'(Có tự ĐKCLS)' END )  ELSE '' END)
                                                    ,(CASE WHEN dk.LoaiKhamID='1' THEN N'Bảo hiểm' ELSE N'Dịch vụ' END) as loaikham
                                                    ,IDDANGKYKHAM=dk.iddangkykham
                                                    ,idchitietdangkykham=ct.IDCHITIETDANGKYKHAM
                                                    ,idkhambenh=0
                                                    ,dk.LoaiKhamID
													FROM CHITIETDANGKYKHAM ct
		                                                LEFT JOIN DANGKYKHAM dk ON ct.IDDANGKYKHAM=dk.IDDANGKYKHAM
		                                                LEFT JOIN BENHNHAN bn ON bn.IDBENHNHAN=dk.IDBENHNHAN 
                                                        LEFT JOIN KB_LOAIUUTIEN ut ON bn.idloaiuutien=ut.ID
                                                        LEFT JOIN BANGGIADICHVU e ON e.IDBANGGIADICHVU=ct.IDBANGGIADICHVU
                                                    where ct.IDKHOA=1
                                                    and dk.ngaydangky>='" + dtpkDScho1.Value.ToString("yyyy-MM-dd 00:00:00.000") + @"'  AND dk.ngaydangky<='" + dtpkDScho2.Value.ToString("yyyy-MM-dd 23:59:00.000") + @"'
                                                    and ct.PhongID='" + this.searchLookUpEdit1.EditValue.ToString() + @"'
                                                    and ct.dakham=0
                                                    AND  ISNULL( ct.DAHUY,0)=0
                                                    and ct.idbanggiadichvu<>628
                                                    and (ct.isDaThu=1 or dk.loaikhamid=1 OR E.IDPHONGKHAMBENH<>1  OR ct.isNotThuPhiCapCuu=1) AND ISNULL(IDKHAMBENH_CHUYEN,0)=0
                                                    AND NOT EXISTS (SELECT IDKHAMBENH FROM KHAMBENH WHERE IDCHITIETDANGKYKHAM=ct.IDCHITIETDANGKYKHAM)
                                                    " + (this.txtMabn.Text.Trim() != "" ? " AND REPLACE( REPLACE( BN.MABENHNHAN,'BN',''),'-','') ='" + this.txtMabn.Text.Trim().Replace("BN", "").Replace("-", "") + "'" : "") + @" 
                                                    " + (this.txtTenbn.Text.Trim() != "" ? " AND (dbo.untiengviet(bn.tenbenhnhan) LIKE N'%" + this.txtTenbn.Text.Trim() + "' or bn.tenbenhnhan like N'%" + txtTenbn.Text.Trim() + "%')" : "") + @"  
                                           union all
                                                    select	bn.idbenhnhan						
													,bn.mabenhnhan
					                               ,bn.tenbenhnhan
                                                    ,bn.ngaysinh
                                                    ,(CASE WHEN bn.gioiTinh='0' THEN 'Nam' ELSE N'Nữ' END) as gioitinh
                                                    ,dk.ngaydangky
                                                    ,ut.TenLoai		
                                                    ,ct.sott
                                                    ,TypeName=N'Chuyển phòng'
                                                    ,(CASE WHEN dk.LoaiKhamID='1' THEN N'Bảo hiểm' ELSE N'Dịch vụ' END) as loaikham
                                                    ,IDDANGKYKHAM=ISNULL(F.IDDANGKYKHAM, dk.IDDANGKYKHAM)
                                                    ,idchitietdangkykham=ISNULL(G.IDCHITIETDANGKYKHAM,ct.IDCHITIETDANGKYKHAM)
													,kb.idkhambenh
                                                    ,dk.LoaiKhamID        
                                                	 FROM KHAMBENH kb
                                                    LEFT JOIN CHITIETDANGKYKHAM ct ON kb.IDCHITIETDANGKYKHAM=ct.IDCHITIETDANGKYKHAM
	                                                LEFT JOIN DANGKYKHAM dk ON dk.IDDANGKYKHAM=ct.IDDANGKYKHAM
	                                                LEFT JOIN BENHNHAN bn ON bn.IDBENHNHAN=dk.IDBENHNHAN
                                                    LEFT JOIN PHONGKHAMBENH K2 ON kb.IDKHOA=K2.IDPHONGKHAMBENH
                                                    LEFT JOIN BACSI bs ON kb.IDBACSI=bs.IDBACSI
                                                    LEFT JOIN DANGKYKHAM F ON kb.IDKHAMBENH=F.IDKHAMBENH_CHUYEN
                                                    LEFT JOIN CHITIETDANGKYKHAM G ON F.IDDANGKYKHAM=G.IDDANGKYKHAM
                                                    LEFT JOIN KB_LOAIUUTIEN ut ON bn.idloaiuutien=ut.ID
                                                    where kb.IdkhoaChuyen=1
                                                    and ct.idbanggiadichvu<>628
                                                    and KB.TGXuatVien>='" + dtpkDScho1.Value.ToString("yyyy-MM-dd 00:00:00.000") + @"'  AND KB.TGXuatVien<='" + dtpkDScho2.Value.ToString("yyyy-MM-dd 23:59:00.000") + @"'
                                                    and kb.IdChuyenPK='" + this.searchLookUpEdit1.EditValue.ToString() + @"'
                                                    --and kb.huongdieutri=1
                                                    " + (this.txtMabn.Text.Trim() != "" ? " AND REPLACE( REPLACE( BN.MABENHNHAN,'BN',''),'-','') ='" + this.txtMabn.Text.Trim().Replace("BN", "").Replace("-", "") + "'" : "") + @"        
                                                    " + (this.txtTenbn.Text.Trim() != "" ? " AND (dbo.untiengviet(bn.tenbenhnhan) LIKE N'%" + this.txtTenbn.Text.Trim() + "' or bn.tenbenhnhan like N'%" + txtTenbn.Text.Trim() + "%')" : "") + @"  
                                                    AND  ISNULL( dk.DAHUY,0)=0
                                                    AND  ISNULL( ct.DAHUY,0)=0
                                                    AND (ISNULL(kb.IsChuyenPhongCoPhi,0)=0   OR  ct.IsDathu=1 )
                                                    AND  ISNULL(kb.idkhambenhchuyenphong,0)=0
                                                union all
                                                     select	bn.idbenhnhan						
													,bn.mabenhnhan
					                               ,bn.tenbenhnhan
                                                    ,bn.ngaysinh
                                                    ,(CASE WHEN bn.gioiTinh='0' THEN 'Nam' ELSE N'Nữ' END) as gioitinh
                                                    ,dk.ngaydangky
                                                    ,ut.TenLoai		
                                                    ,ct.sott
                                                    ,TypeName=(CASE WHEN kb.ISDACLS=1 THEN N'ĐỦ KQ' ELSE  N'Chờ CLS'  END)
                                                    ,(CASE WHEN dk.LoaiKhamID='1' THEN N'Bảo hiểm' ELSE N'Dịch vụ' END) as loaikham
                                                    ,IDDANGKYKHAM=dk.iddangkykham
                                                    ,idchitietdangkykham=ct.IDCHITIETDANGKYKHAM
													,kb.idkhambenh
                                                    ,dk.LoaiKhamID
													 FROM KHAMBENH kb
												LEFT JOIN CHITIETDANGKYKHAM ct ON kb.IDCHITIETDANGKYKHAM=ct.IDCHITIETDANGKYKHAM
												LEFT JOIN DANGKYKHAM dk ON dk.IDDANGKYKHAM=ct.IDDANGKYKHAM
												LEFT JOIN BENHNHAN bn ON bn.IDBENHNHAN=dk.IDBENHNHAN
												LEFT JOIN KB_LOAIUUTIEN ut ON bn.idloaiuutien=ut.ID
                                                    where ct.IDKHOA=1
                                                    AND  ISNULL( ct.DAHUY,0)=0
                                                    and KB.NGAYKHAM>='" + dtpkDScho1.Value.ToString("yyyy-MM-dd 00:00:00.000") + @"'  AND KB.NGAYKHAM<='" + dtpkDScho2.Value.ToString("yyyy-MM-dd 23:59:00.000") + @"'
                                                    and ct.PhongID='" + this.searchLookUpEdit1.EditValue.ToString() + @"'
                                                     " + (this.txtMabn.Text.Trim() != "" ? " AND REPLACE( REPLACE( BN.MABENHNHAN,'BN',''),'-','') ='" + this.txtMabn.Text.Trim().Replace("BN", "").Replace("-", "") + "'" : "") + @" 
                                                    " + (this.txtTenbn.Text.Trim() != "" ? " AND (dbo.untiengviet(bn.tenbenhnhan) LIKE N'%" + this.txtTenbn.Text.Trim() + "' or bn.tenbenhnhan like N'%" + txtTenbn.Text.Trim() + "%')" : "") + @"    
                                                    and ISNULL(kb.ISKHONGKHAM,0)=0
                                                    and kb.IsHaveCLS=1
                                                    and ct.idbanggiadichvu<>628
                                                    and kb.huongdieutri IN (6,10)
                                                    order by dk.ngaydangky";
                    DataTable dtbenhnhan = DataAcess.Connect.GetTable(sql);
                    gridControl1.DataSource = dtbenhnhan;
                    if(dtbenhnhan.Rows.Count==0)
                    {
                        MessageBox.Show("Hết bệnh nhân chờ");
                    }
                  
                }
                catch (SqlException)
                {
                    MessageBox.Show("Không lấy được nội dung. Lỗi rồi!!!");
                }
               
            }
        }

        
        private void gridControl1_Click(object sender, EventArgs e)
        {
         
        }

        private void gridControl1_Click_1(object sender, EventArgs e)
        {
            Truyendulieu.idchitietdangkykham = gridView1.GetFocusedRowCellDisplayText(idchitietdangkykham);
            Truyendulieu.idkhambenh = gridView1.GetFocusedRowCellDisplayText(IDKhamBenh);
            Truyendulieu.PhongKhamID = searchLookUpEdit1.EditValue.ToString();
            Truyendulieu.TypeName= gridView1.GetFocusedRowCellDisplayText(TypeName);
            Main f = (Main)this.ParentForm;
            if (f.checkTab(gridView1.GetFocusedRowCellDisplayText(tenbenhnhan)) == false)
            {
                DevComponents.DotNetBar.TabItem t = f.TabCtrl_main.CreateTab(gridView1.GetFocusedRowCellDisplayText(IDKhamBenh) + " - " + gridView1.GetFocusedRowCellDisplayText(tenbenhnhan));
                KhamBenhPro.KhamBenh.frmKhamBenh frmkb1 = new KhamBenhPro.KhamBenh.frmKhamBenh();
                frmkb1.FormBorderStyle = FormBorderStyle.None;
                frmkb1.TopLevel = false;
                frmkb1.Dock = DockStyle.Fill;
                t.AttachedControl.Controls.Add(frmkb1);
                frmkb1.Show();
                f.TabCtrl_main.SelectedTabIndex = f.TabCtrl_main.Tabs.Count - 1;
            }
         
        }

        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            GridView gridview = ((GridView)sender);
            if (!gridview.GridControl.IsHandleCreated) return;
            Graphics gr = Graphics.FromHwnd(gridview.GridControl.Handle);
            SizeF size = gr.MeasureString(gridview.RowCount.ToString(), gridview.PaintAppearance.Row.GetFont());
            gridview.IndicatorWidth = Convert.ToInt32(size.Width + 0.999f) + DevExpress.XtraGrid.Views.Grid.Drawing.GridPainter.Indicator.ImageSize.Width + 10;
        }
        bool indicatorIcon = true;
        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                if (e.Info.IsRowIndicator && e.RowHandle >= 0)
                {
                    string sText = (e.RowHandle + 1).ToString();
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString(sText, e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = sText;
                }
                if (!indicatorIcon)
                    e.Info.ImageIndex = -1;

                if (e.RowHandle == GridControl.InvalidRowHandle)
                {
                    Graphics gr = e.Info.Graphics;
                    gr.PageUnit = GraphicsUnit.Pixel;
                    GridView gridView = ((GridView)sender);
                    SizeF size = gr.MeasureString("STT", e.Info.Appearance.Font);
                    int nNewSize = Convert.ToInt32(size.Width) + GridPainter.Indicator.ImageSize.Width + 10;
                    if (gridView.IndicatorWidth < nNewSize)
                    {
                        gridView.IndicatorWidth = nNewSize;
                    }

                    e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    e.Info.DisplayText = "STT";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //if (e.RowHandle >= 0)
            //{
            //    if (e.RowHandle % 2 == 0)
            //    {
            //        e.Appearance.BackColor = Color.Red;
            //    }
            //}

            //if (e.Column.FieldName == "TypeName")
            //{
                
                string category = gridView1.GetRowCellDisplayText(e.RowHandle, gridView1.Columns["TypeName"]);
                if (category == "Chờ khám")
                    e.Appearance.BackColor = Color.WhiteSmoke;
                else if (category == "Chờ CLS")
                    e.Appearance.BackColor = Color.DeepSkyBlue;
                else if (category == "Chuyển phòng")
                    e.Appearance.BackColor = Color.GreenYellow;
                else if (category == "ĐỦ KQ")
                    e.Appearance.BackColor = Color.Violet;
            //}
          
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            ////try
            ////{
            ////    int dong = e.RowHandle;
            ////    gridView1.row[dong].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
            ////}
            ////catch { }
            //this.gridView1.Appearance.OddRow.BackColor = System.Drawing.Color.Red;
            //this.gridView1.Appearance.OddRow.Options.UseBackColor = true;
            //gridView1.OptionsView.EnableAppearanceOddRow = true;
        }

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //if (e.Column.FieldName == "TypeName")
            //{

            //    string category = gridView1.GetRowCellDisplayText(e.RowHandle, gridView1.Columns["TypeName"]);
            //    if (category == "Chờ khám")
            //        e.Appearance.BackColor = Color.DeepSkyBlue;
            //    else if (category == "Chờ CLS")
            //        e.Appearance.BackColor = Color.Pink;
            //    else if (category == "Chuyển phòng")
            //        e.Appearance.BackColor = Color.SeaGreen;
            //    else if (category == "ĐỦ KQ")
            //        e.Appearance.BackColor = Color.Violet;
            //}
        }

        private void searchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            txtMabn.Focus();
        }

        private void btnChoKham_Click(object sender, EventArgs e)
        {
            Truyendulieu.idchitietdangkykham = gridView1.GetFocusedRowCellDisplayText(idchitietdangkykham);
            Truyendulieu.idkhambenh = gridView1.GetFocusedRowCellDisplayText(IDKhamBenh);
            Truyendulieu.PhongKhamID = searchLookUpEdit1.EditValue.ToString();
            Truyendulieu.TypeName = gridView1.GetFocusedRowCellDisplayText(TypeName);
            Main f = (Main)this.ParentForm;
            if (f.checkTab(gridView1.GetFocusedRowCellDisplayText(tenbenhnhan)) == false)
            {
                DevComponents.DotNetBar.TabItem t = f.TabCtrl_main.CreateTab(gridView1.GetFocusedRowCellDisplayText(IDKhamBenh) + " - " + gridView1.GetFocusedRowCellDisplayText(tenbenhnhan));
                KhamBenhPro.KhamBenh.frmKhamBenh frmkb1 = new KhamBenhPro.KhamBenh.frmKhamBenh();
                frmkb1.FormBorderStyle = FormBorderStyle.None;
                frmkb1.TopLevel = false;
                frmkb1.Dock = DockStyle.Fill;
                t.AttachedControl.Controls.Add(frmkb1);
                frmkb1.Show();
                f.TabCtrl_main.SelectedTabIndex = f.TabCtrl_main.Tabs.Count - 1;
            }

        }

        private void txtMabn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnDScho_Click(sender,e);
            }
        }
    }
}
