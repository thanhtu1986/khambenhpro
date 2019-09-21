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
using DevExpress.XtraGrid;
using System.Collections;
using DevExpress.XtraEditors.Controls;
using DataAcess;
using iTextSharp.text.pdf;

namespace KhamBenhPro.KhamBenh
{
    public partial class DaKham : Form
    {
        string idkhambenh = null;
        string idchitietdangkykham = null;
        string loaikhamID = null;
        string idphieutt = null;
        public DaKham()
        {
            InitializeComponent();
        }
        #region Load bệnh nhân để kiểm tra khám mới hoặc chuyển phòng hoặc chờ cls
        private string dt_LoadBN()
        {
            string sql = @"  select ct.idchitietdangkykham,isnull(kb.idkhambenh,0) as idkhambenh,dk.IDKHAMBENH_CHUYEN,kb.TGXuatVien,isnull(kb.IdChuyenPK,0) as IdChuyenPK,bn.mabenhnhan,bn.tenbenhnhan,kb.idkhambenhchuyenphong,bn.idbenhnhan,dk.iddangkykham,ct.idbanggiadichvu,dk.LoaiKhamID,dk.IdBenhBHDongTien
                                                    from dangkykham dk
                                                    inner join chitietdangkykham ct on ct.iddangkykham=dk.iddangkykham
													inner join benhnhan bn on bn.idbenhnhan=dk.idbenhnhan
                                                    left join khambenh kb on kb.IdChiTietDangKyKham=ct.IdChiTietDangKyKham
                                                    left join hs_benhnhanbhdongtien dt on dt.id=dk.IdBenhBHDongTien
                                                    where ct.idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'";
            return sql;
        }
        #endregion

        public void Load_BNchokham()
        {
         //   DataTable dt = null;
            DataTable dtLuuKB = DataAcess.Connect.GetTable(this.dt_LoadBN());
            loaikhamID = dtLuuKB.Rows[0]["LoaiKhamID"].ToString();
            idphieutt = dtLuuKB.Rows[0]["IdBenhBHDongTien"].ToString();
            Truyendulieu.idbenhnhan = dtLuuKB.Rows[0]["idbenhnhan"].ToString();
            if (Truyendulieu.TypeName == "Chờ khám" || Truyendulieu.TypeName == "Chờ khám(Có tự ĐKCLS)")
            {
                #region Load thông tin hành chính khám mới
                idchitietdangkykham = dtLuuKB.Rows[0]["idchitietdangkykham"].ToString();
                DataTable dt = DataAcess.Connect.GetTable(GetData.dt_BNChoKham(Truyendulieu.idchitietdangkykham));
                txtMaBN.Text = dt.Rows[0]["mabenhnhan"].ToString();
                txtTenBN.Text = dt.Rows[0]["tenbenhnhan"].ToString();
                txtNgaySinh.Text = dt.Rows[0]["ngaysinh"].ToString();
                txtGioiTinh.Text = dt.Rows[0]["gioiTinh"].ToString();
                txtDiachi.Text = dt.Rows[0]["diachi"].ToString();
                txtBHYT.Text = dt.Rows[0]["sobhyt"].ToString();
                txtNoiDK.Text = dt.Rows[0]["TENNOIDANGKY"].ToString();
                txtNgayBD.Text = dt.Rows[0]["ngaybatdau"].ToString();
                txtNgayHH.Text = dt.Rows[0]["NgayHetHan"].ToString();
                txtVaovien.Text = dt.Rows[0]["ngaydangky"].ToString();
             //   txtPhongKham.Text = dt.Rows[0]["tenphong"].ToString();
                if (dt.Rows[0]["isdungtuyen"].ToString() == "True")
                {
                    cbDungTuyen.Checked = true;
                }
                else
                {
                    cbDungTuyen.Checked = false;
                }
                #endregion
            }
            else
            {
                if (Truyendulieu.TypeName == "Chuyển phòng")
                {
                    DataTable dt = DataAcess.Connect.GetTable(GetData.dt_BNKhamCP(Truyendulieu.idkhambenh));
                    txtMaBN.Text = dt.Rows[0]["mabenhnhan"].ToString();
                    txtTenBN.Text = dt.Rows[0]["tenbenhnhan"].ToString();
                    txtNgaySinh.Text = dt.Rows[0]["ngaysinh"].ToString();
                    txtGioiTinh.Text = dt.Rows[0]["gioiTinh"].ToString();
                    txtDiachi.Text = dt.Rows[0]["diachi"].ToString();
                    txtBHYT.Text = dt.Rows[0]["sobhyt"].ToString();
                    txtNoiDK.Text = dt.Rows[0]["TENNOIDANGKY"].ToString();
                    txtNgayBD.Text = dt.Rows[0]["ngaybatdau"].ToString();
                    txtNgayHH.Text = dt.Rows[0]["NgayHetHan"].ToString();
                    txtVaovien.Text = dt.Rows[0]["ngaydangky"].ToString();
                    //txtPhongKham.Text = dt.Rows[0]["tenphong"].ToString();
                    //sluCDXD.EditValue = dt.Rows[0]["ketluan"].ToString();
                    if (dt.Rows[0]["isdungtuyen"].ToString() == "True")
                    {
                        cbDungTuyen.Checked = true;
                    }
                    else
                    {
                        cbDungTuyen.Checked = false;
                    }
                    //Load_CDSB(Truyendulieu.idkhambenh);
                    //Load_CDPH(Truyendulieu.idkhambenh);

                }
                else
                {
                    simpleButton1.Text = "Sửa";
                    DataTable dt = DataAcess.Connect.GetTable(GetData.dt_BNDaKham2(Truyendulieu.idkhambenh));
                    #region Load thông tin hành chính đã khám
                    //txtMach.Text = dt.Rows[0]["MACH"].ToString();
                    //txtNhietDo.Text = dt.Rows[0]["NHIETDO"].ToString();
                    //txtHuyetAp.Text = dt.Rows[0]["HUYETAP1"].ToString();
                    //txtHuyetAp2.Text = dt.Rows[0]["HUYETAP2"].ToString();
                    //txtNhipTho.Text = dt.Rows[0]["NHIPTHO"].ToString();
                    //txtCanNang.Text = dt.Rows[0]["CANNANG"].ToString();
                    //txtChieuCao.Text = dt.Rows[0]["CHIEUCAO"].ToString();
                    //txtBMI.Text = dt.Rows[0]["BMI"].ToString();
                    txtMaBN.Text = dt.Rows[0]["mabenhnhan"].ToString();
                    txtTenBN.Text = dt.Rows[0]["tenbenhnhan"].ToString();
                    txtNgaySinh.Text = dt.Rows[0]["ngaysinh"].ToString();
                    txtGioiTinh.Text = dt.Rows[0]["gioiTinh"].ToString();
                    txtDiachi.Text = dt.Rows[0]["diachi"].ToString();
                    txtBHYT.Text = dt.Rows[0]["sobhyt"].ToString();
                    txtNoiDK.Text = dt.Rows[0]["TENNOIDANGKY"].ToString();
                    txtNgayBD.Text = dt.Rows[0]["ngaybatdau"].ToString();
                    txtNgayHH.Text = dt.Rows[0]["NgayHetHan"].ToString();
                    txtVaovien.Text = dt.Rows[0]["ngaydangky"].ToString();
                    slkBacsi.EditValue=dt.Rows[0]["idbacsi"].ToString();
                    slkPhongKham.EditValue= dt.Rows[0]["phongid"].ToString();
                    //txtPhongKham.Text = dt.Rows[0]["TENPHONG"].ToString();
                   // textBox1.Text = dt.Rows[0]["isdungtuyen"].ToString();
                    if (dt.Rows[0]["isdungtuyen"].ToString() == "True")
                    {
                        cbDungTuyen.Checked = true;
                    }
                    else
                    {
                        cbDungTuyen.Checked = false;
                    }
                    //txtRavien.Text = dt.Rows[0]["TGXuatVien"].ToString() + " " + dt.Rows[0]["gioravien"].ToString() + ":" + dt.Rows[0]["phutravien"].ToString();
                    //if (dt.Rows[0]["isNgoaiTru"].ToString() == "1")
                    //{
                    //    chkNgoaitru.Checked = true;
                    //}
                    //else
                    //{
                    //    chkNgoaitru.Checked = false;
                    //}
                    //if (dt.Rows[0]["isNoitru"].ToString() == "1")
                    //{
                    //    chkNoitru.Checked = true;
                    //}
                    //else
                    //{
                    //    chkNoitru.Checked = false;
                    //}
                    //if (dt.Rows[0]["isXuatvien"].ToString() == "True")
                    //{
                    //    chkRavien.Checked = true;
                    //}
                    //else
                    //{
                    //    chkRavien.Checked = false;
                    //}
                    //sluBacsi.EditValue = dt.Rows[0]["idbacsi"].ToString();
                    //gluBacSi2.EditValue = dt.Rows[0]["idbacsi2"].ToString();
                    //sluPK.EditValue = dt.Rows[0]["IdChuyenPK"].ToString();
                    //sluKhoa.EditValue = dt.Rows[0]["IdkhoaChuyen"].ToString();
                    //txtSovaovien.Text = dt.Rows[0]["SOVAOVIEN1"].ToString();
                    //if(dt.Rows[0]["SOVAOVIEN1"].ToString() != ""|| dt.Rows[0]["SOVAOVIEN1"].ToString() != null|| dt.Rows[0]["SOVAOVIEN1"].ToString() != "0")
                    //{
                    //    simpleButton2.Enabled = false;
                    //    
                    //}
                    //else { simpleButton2.Enabled = true; }
                    //txtSongayratoa.Text = dt.Rows[0]["songayratoa"].ToString();
                    //txtPhongKham.Text = dt.Rows[0]["TENPHONG"].ToString();
                    //sluCDXD.EditValue = dt.Rows[0]["ketluan"].ToString();
                    #endregion
                    //Load_CLS(Truyendulieu.idkhambenh);
                    //Load_ToaThuoc(Truyendulieu.idkhambenh);
                    //Load_CDSB(Truyendulieu.idkhambenh);
                    //Load_CDPH(Truyendulieu.idkhambenh);
                }
            }
        }
        private void btnThemThuoc_Click(object sender, EventArgs e)
        {

        }
        private void DaKham_Load(object sender, EventArgs e)
        {
            Load_BNchokham();
            Load_Bacsi();
            Load_PhongKham();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaCD_Click(object sender, EventArgs e)
        {

        }

        private void btnThemCDSB_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {


        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }

        private void btnXoaCLS_Click(object sender, EventArgs e)
        {

        }

        private void btnNhomCLS_Click(object sender, EventArgs e)
        {

        }

        private void dtgvThuoc_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void dtgvThuoc_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }
        private void formInPanel(object formSinhHieu)
        {
            if(this.panel11.Controls.Count>0)
                this.panel11.Controls.RemoveAt(0);
            Form fh = formSinhHieu as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.panel11.Controls.Add(fh);
            this.panel11.Tag = fh;
            fh.Show();

        }
        private void btnSinhHieu_Click(object sender, EventArgs e)
        {
            formInPanel(new SinhHieu());
            
        }

        private void btnCLS_Click(object sender, EventArgs e)
        {
            formInPanel(new CanLamSan());
        }

        private void btnThuoc_Click(object sender, EventArgs e)
        {
            formInPanel(new Thuoc());
        }

        private void btnHuongDT_Click(object sender, EventArgs e)
        {
            formInPanel(new HuongDieuTri());
        }
        public void Load_Bacsi()
        {
            DataTable dtBacsi = DataAcess.Connect.GetTable(this.dt_Load_Bacsi());
            slkBacsi.Properties.DataSource = dtBacsi;
            slkBacsi.Properties.DisplayMember = "tenbacsi";
            slkBacsi.Properties.ValueMember = "idbacsi";
            slkBacsi.Properties.NullText = "Nhập Bác sĩ";
            slkBacsi.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkBacsi.Properties.ImmediatePopup = true;
            slkBacsi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }
        private string dt_Load_Bacsi()
        {
            string sql = "SELECT idbacsi,tenbacsi,mabacsi FROM dbo.bacsi WHERE mabacsi like '%CCHN%'";
            return sql;
        }
        public void Load_PhongKham()
        {
         
           string sql=@"select p.id,p.maso+'-'+p.TenPhong as 'tenphong'
                                            from KB_Phong p
                                            inner join banggiadichvu bg on bg.idbanggiadichvu = p.DichVuKCB
                                            where bg.idphongkhambenh =1
                                            and p.isPhongNoiTru = 0
                                            and p.IsActive = 1
                                            and p.Status=1
                                            order by p.MaSo";
            DataTable dtPhong = DataAcess.Connect.GetTable(sql);
            slkPhongKham.Properties.NullText = "Nhập phòng khám";
            slkPhongKham.Properties.DataSource = dtPhong;
            slkPhongKham.Properties.DisplayMember = "tenphong";
            slkPhongKham.Properties.ValueMember = "id";
           
        }
        public void Insert_khambenh()
        {
            DataTable dtLuuKB2 = DataAcess.Connect.GetTable(this.dt_LoadBN());
            string sql = @"insert into khambenh (ngaykham,idbenhnhan,iddangkykham,idbacsi,idphongkhambenh,IdChiTietDangKyKham,IdPhong,DichVuKCBID,IdKhoa,PhongID,Sysdate)
                                     values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "','" + dtLuuKB2.Rows[0]["iddangkykham"].ToString() + "','" + slkBacsi.EditValue.ToString() + @"'
                                     ,1,'" + dtLuuKB2.Rows[0]["idchitietdangkykham"].ToString() + "','" + Truyendulieu.PhongKhamID + @"'
                                     ,'" + dtLuuKB2.Rows[0]["idbanggiadichvu"].ToString() + "', 1,'" + Truyendulieu.PhongKhamID + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
            // DataTable luukb2 = DataAcess.Connect.GetTable(luuKB);
     
            bool okk = DataAcess.Connect.ExecSQL(sql);
            if (okk)
            {
                string updateCT = "update chitietdangkykham set dakham=1 where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'";
                DataTable LuuCT = DataAcess.Connect.GetTable(updateCT);
            }
            }
        public void Update_khambenh()
        {
            string updateKB = @"update khambenh set idbacsi='" + slkBacsi.EditValue.ToString() + @"' where idkhambenh='" + Truyendulieu.idkhambenh + "' ";
            DataTable LuuKB = DataAcess.Connect.GetTable(updateKB);
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            SinhHieu SH = new SinhHieu();
          if(simpleButton1.Text== "Lưu")
            {
               // Insert_khambenh();
               // SH.Insert_Sinhhieu();
                
            }
            else
            {
                Update_khambenh();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Truyendulieu.idphieutt = idphieutt;
            frmBV01 frm01 = new frmBV01();
            frm01.Show();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmToaThuocBH frmpTT = new frmToaThuocBH();
            frmpTT.Show();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {

            frmToaThuocDV frmpTTDV = new frmToaThuocDV();
            frmpTTDV.Show();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            frmRptCLS frmp = new frmRptCLS();
            frmp.Show();
        }
    }
    }

