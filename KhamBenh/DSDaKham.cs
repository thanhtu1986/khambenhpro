using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid.Drawing;
namespace KhamBenhPro.KhamBenh
{
    public partial class DSDaKham : Form
    {
       
        public DSDaKham()
        {
            InitializeComponent();
        }
        

        private void DSDaKham_Load(object sender, EventArgs e)
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
            sluPhongKham.Properties.NullText ="Nhập phòng khám";
            sluPhongKham.Properties.DataSource = dtPhong;
            sluPhongKham.Properties.DisplayMember = "tenphong";
            sluPhongKham.Properties.ValueMember = "id";
           
        }

        public void Load_Danhsach_DaKham()
        {
            if (sluPhongKham.EditValue==null)
            {
                MessageBox.Show("Chưa chọn phòng khám?");
            }
            else
            {
                try
                {
                    string sql=@"SELECT STT=ROW_NUMBER() OVER(ORDER BY IDKHAMBENH),
                                                  A.IDCHITIETDANGKYKHAM,
	                                              B.IDDANGKYKHAM,
	                                              B.NGAYDANGKY,
	                                              C.MABENHNHAN,
	                                              C.TENBENHNHAN,
	                                              C.NGAYSINH,
	                                              GIOITINH=DBO.GetGioiTinh( C.GIOITINH),
	                                              B.IDBENHNHAN	,
                                                  D.IDKHAMBENH,
                                                  E.TENBACSI,
                                                  D.NGAYKHAM,
                                                  TenHuongDieuTri=( CASE WHEN D.IDKHOACHUYEN=15 THEN N'Chuyển cấp cứu' else (CASE WHEN D.IDKHOACHUYEN=25 THEN N'Chuyển tán sỏi'  ELSE  (CASE WHEN D.IDKHOACHUYEN=46 THEN N'Chuyển hóa trị' else F.TenHuongDieuTri end) END) end )
                                                  ,HaveCLS=( CASE WHEN D.ISHAVECLS =1  THEN N'In phiếu CĐCLS' ELSE NULL END) 
                                                  ,HaveThuocBH= ( CASE WHEN D.ISHAVETHUOCBH =1 then  N'Toa thuốc BH' else null end)   
                                                  ,HaveThuocDV= ( CASE WHEN (D.ISHAVETHUOC =1 AND D.ISHAVETHUOCBH=0)  then  N'Toa thuốc DV' else null end)   
                                                  ,CreateUser=(SELECT TENNGUOIDUNG FROM NGUOIDUNG WHERE IDNGUOIDUNG=D.IDDIEUDUONG)
                                                  ,EditUser=(SELECT TENNGUOIDUNG FROM NGUOIDUNG WHERE IDNGUOIDUNG=D.IDDIEUDUONG2)
                                                  ,LoaiDK=(CASE WHEN B.LOAIKHAMID=1 THEN N'BH' ELSE 'DV' END)
	                                            FROM KHAMBENH D
                                                LEFT JOIN CHITIETDANGKYKHAM A ON A.IDCHITIETDANGKYKHAM=D.IDCHITIETDANGKYKHAM
		                                        LEFT JOIN DANGKYKHAM B ON A.IDDANGKYKHAM=B.IDDANGKYKHAM
		                                        LEFT JOIN BENHNHAN C ON C.IDBENHNHAN=B.IDBENHNHAN
                                                LEFT JOIN BACSI E ON ( CASE WHEN ISNULL(D.IDBACSI2,0)<>0 THEN D.IDBACSI2 ELSE  D.IDBACSI END )=E.IDBACSI
                                                LEFT JOIN KB_HUONGDIEUTRI F ON D.HUONGDIEUTRI=F.HUONGDIEUTRIID
                                                where a.IDKHOA=1
                                                      and d.NGAYKHAM>='" + dtpkDakham1.Value.ToString("yyyy-MM-dd 00:00:00.000") + @"'  AND d.NGAYKHAM<='" + dtpkDakham2.Value.ToString("yyyy-MM-dd 23:59:00.000") + @"'
                                                      and d.PhongID='" + this.sluPhongKham.EditValue.ToString() + @"'
                                                     " + (this.txtMaBN.Text.Trim() != "" ? " AND REPLACE( REPLACE( c.MABENHNHAN,'BN',''),'-','') ='" + this.txtMaBN.Text.Trim().Replace("BN", "").Replace("-", "") + "'" : "") + @" 
                                                     " + (this.txtHoTen.Text.Trim() != "" ? " AND (dbo.untiengviet(c.tenbenhnhan) LIKE N'%" + this.txtHoTen.Text.Trim() + "' or c.tenbenhnhan like N'%" + txtHoTen.Text.Trim() + "%') " : "") + "";
                    DataTable dtbenhnhan = DataAcess.Connect.GetTable(sql);
                    gridControl1.DataSource = dtbenhnhan;
                    if (dtbenhnhan.Rows.Count == 0)
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
        private void sluPhongKham_EditValueChanged(object sender, EventArgs e)
        {
            txtMaBN.Focus();
        }

        private void btnLayDSDaKham_Click(object sender, EventArgs e)
        {
            Load_Danhsach_DaKham();
        }

        private void grcDaKham_Click(object sender, EventArgs e)
        {
            Truyendulieu.idchitietdangkykham = gridView1.GetFocusedRowCellDisplayText(IDCHITIETDANGKYKHAM);
            Truyendulieu.PhongKhamID = sluPhongKham.EditValue.ToString();
            Truyendulieu.idkhambenh = gridView1.GetFocusedRowCellDisplayText(idkhambenh);
            Main f1 = (Main)this.ParentForm;
            if (f1.checkTab(gridView1.GetFocusedRowCellDisplayText(tenbenhnhan)) == false)
            {
                DevComponents.DotNetBar.TabItem t1 = f1.TabCtrl_main.CreateTab(gridView1.GetFocusedRowCellDisplayText(idkhambenh) + " - " + gridView1.GetFocusedRowCellDisplayText(tenbenhnhan));
                KhamBenhPro.KhamBenh.frmKhamBenh frmkb1 = new KhamBenhPro.KhamBenh.frmKhamBenh();
                frmkb1.FormBorderStyle = FormBorderStyle.None;
                frmkb1.TopLevel = false;
                frmkb1.Dock = DockStyle.Fill;
                t1.AttachedControl.Controls.Add(frmkb1);
                frmkb1.Show();
                f1.TabCtrl_main.SelectedTabIndex = f1.TabCtrl_main.Tabs.Count - 1;
            }
        }

        private void btnKham_Click(object sender, EventArgs e)
        {
            Truyendulieu.idchitietdangkykham = gridView1.GetFocusedRowCellDisplayText(IDCHITIETDANGKYKHAM);
            Truyendulieu.PhongKhamID = sluPhongKham.EditValue.ToString();
            Truyendulieu.idkhambenh = gridView1.GetFocusedRowCellDisplayText(idkhambenh);
            Main f1 = (Main)this.ParentForm;
            if (f1.checkTab(gridView1.GetFocusedRowCellDisplayText(tenbenhnhan)) == false)
            {
                DevComponents.DotNetBar.TabItem t1 = f1.TabCtrl_main.CreateTab(gridView1.GetFocusedRowCellDisplayText(idkhambenh) + " - " + gridView1.GetFocusedRowCellDisplayText(tenbenhnhan));
                KhamBenhPro.KhamBenh.frmKhamBenh frmkb1 = new KhamBenhPro.KhamBenh.frmKhamBenh();
                frmkb1.FormBorderStyle = FormBorderStyle.None;
                frmkb1.TopLevel = false;
                frmkb1.Dock = DockStyle.Fill;
                t1.AttachedControl.Controls.Add(frmkb1);
                frmkb1.Show();
                f1.TabCtrl_main.SelectedTabIndex = f1.TabCtrl_main.Tabs.Count - 1;
            }
        }

        private void btnInCLS_Click(object sender, EventArgs e)
        {
           
            Truyendulieu.idkhambenh = gridView1.GetFocusedRowCellDisplayText(idkhambenh);
            frmCLS_dathu frmp = new frmCLS_dathu();
            frmp.Show();
        }

        private void btnInToaBH_Click(object sender, EventArgs e)
        {
           Truyendulieu.idkhambenh = gridView1.GetFocusedRowCellDisplayText(idkhambenh);
            frmToaThuocBH frmpTT = new frmToaThuocBH();
            frmpTT.Show();
        }

        private void btnInToaDV_Click(object sender, EventArgs e)
        {
            Truyendulieu.idkhambenh = gridView1.GetFocusedRowCellDisplayText(idkhambenh);
            frmToaThuocDV frmpTTDV = new frmToaThuocDV();
            frmpTTDV.Show();
        }

        private void txtMaBN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Load_Danhsach_DaKham();
            }
        }
    }
}
