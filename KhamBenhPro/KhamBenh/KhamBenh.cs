using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
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
using DataAcess;
using iTextSharp.text.pdf;
namespace KhamBenhPro.KhamBenh
{
    public partial class KhamBenh : DevExpress.XtraEditors.XtraForm
    {
        string strConnectionString = @"Data Source=192.168.1.162;Initial Catalog=BVMDDB ;Persist Security Info=True;User ID=sa;Password=123456@huy";
        // Đối tượng kết nối
        SqlConnection conn = null;
       // string idkhambenh = null;
        string idchitietdangkykham = null;
        string loaikhamID = null;
        string idphieutt = null;
        public KhamBenh()
        {
            InitializeComponent();
        }

        private void KhamBenh_Load(object sender, EventArgs e)
        {
           
            Load_BNchokham();
            LoadsluBacsi();
            LoadgluChandoan();
            LoadCDXD();
            LoadSLCanLamSang();
            LoadSLCanLamSang_hen();
            NhomCLS_load_hen();
            NhomCLS_load();
            Load_ChanDoanSoBo();
            LoadsluBacsi2();
            KhoThuoc_load();
           // Load_Thuoc_cachdung();
           // Load_thuoc_donvidung();
           // Load_thuoc_donvitinh();
            Load_thuoc_doituong();
            Load_Khoa();
            
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
            DataTable dt = null;
            DataTable dtLuuKB = DataAcess.Connect.GetTable(this.dt_LoadBN());
            loaikhamID = dtLuuKB.Rows[0]["LoaiKhamID"].ToString();
            idphieutt= dtLuuKB.Rows[0]["IdBenhBHDongTien"].ToString();
            Truyendulieu.idbenhnhan= dtLuuKB.Rows[0]["idbenhnhan"].ToString();
            if (Truyendulieu.TypeName== "Chờ khám" || Truyendulieu.TypeName == "Chờ khám(Có tự ĐKCLS)")
            {
                #region Load thông tin hành chính khám mới
                idchitietdangkykham = dtLuuKB.Rows[0]["idchitietdangkykham"].ToString();
                dt = DataAcess.Connect.GetTable(GetData.dt_BNChoKham(Truyendulieu.idchitietdangkykham));
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
                txtPhongKham.Text = dt.Rows[0]["tenphong"].ToString();
                if (dt.Rows[0]["isdungtuyen"].ToString() == "1")
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
                if (Truyendulieu.TypeName== "Chuyển phòng")
                {
                    dt = DataAcess.Connect.GetTable(GetData.dt_BNKhamCP(Truyendulieu.idkhambenh));
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
                    txtPhongKham.Text = dt.Rows[0]["tenphong"].ToString();
                    sluCDXD.EditValue= dt.Rows[0]["ketluan"].ToString();
                    if (dt.Rows[0]["isdungtuyen"].ToString() == "1")
                    {
                        cbDungTuyen.Checked = true;
                    }
                    else
                    {
                        cbDungTuyen.Checked = false;
                    }
                    Load_CDSB(Truyendulieu.idkhambenh);
                    Load_CDPH(Truyendulieu.idkhambenh);

                }
                else
                {
                    simpleButton5.Text = "Sửa";
                    dt = DataAcess.Connect.GetTable(GetData.dt_BNDaKham2(Truyendulieu.idkhambenh));
                    #region Load thông tin hành chính đã khám
                    txtMach.Text = dt.Rows[0]["MACH"].ToString();
                    txtNhietDo.Text = dt.Rows[0]["NHIETDO"].ToString();
                    txtHuyetAp.Text = dt.Rows[0]["HUYETAP1"].ToString();
                    txtHuyetAp2.Text = dt.Rows[0]["HUYETAP2"].ToString();
                    txtNhipTho.Text = dt.Rows[0]["NHIPTHO"].ToString();
                    txtCanNang.Text = dt.Rows[0]["CANNANG"].ToString();
                    txtChieuCao.Text = dt.Rows[0]["CHIEUCAO"].ToString();
                    txtBMI.Text = dt.Rows[0]["BMI"].ToString();
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
                    txtPhongKham.Text = dt.Rows[0]["TENPHONG"].ToString();
                    if (dt.Rows[0]["isdungtuyen"].ToString() == "1")
                    {
                        cbDungTuyen.Checked = true;
                    }
                    else
                    {
                        cbDungTuyen.Checked = false;
                    }
                    txtRavien.Text = dt.Rows[0]["TGXuatVien"].ToString() + " " + dt.Rows[0]["gioravien"].ToString() + ":" + dt.Rows[0]["phutravien"].ToString();
                    if (dt.Rows[0]["isNgoaiTru"].ToString() == "1")
                    {
                        chkNgoaitru.Checked = true;
                    }
                    else
                    {
                        chkNgoaitru.Checked = false;
                    }
                    if (dt.Rows[0]["isNoitru"].ToString() == "1")
                    {
                        chkNoitru.Checked = true;
                    }
                    else
                    {
                        chkNoitru.Checked = false;
                    }
                    if (dt.Rows[0]["isXuatvien"].ToString() == "True")
                    {
                        chkRavien.Checked = true;
                    }
                    else
                    {
                        chkRavien.Checked = false;
                    }
                    sluBacsi.EditValue = dt.Rows[0]["idbacsi"].ToString();
                    gluBacSi2.EditValue = dt.Rows[0]["idbacsi2"].ToString();
                    sluPK.EditValue = dt.Rows[0]["IdChuyenPK"].ToString();
                    sluKhoa.EditValue = dt.Rows[0]["IdkhoaChuyen"].ToString();
                    txtSovaovien.Text = dt.Rows[0]["SOVAOVIEN1"].ToString();
                    //if(dt.Rows[0]["SOVAOVIEN1"].ToString() != ""|| dt.Rows[0]["SOVAOVIEN1"].ToString() != null|| dt.Rows[0]["SOVAOVIEN1"].ToString() != "0")
                    //{
                    //    simpleButton2.Enabled = false;
                    //    
                    //}
                    //else { simpleButton2.Enabled = true; }
                    txtSongayratoa.Text = dt.Rows[0]["songayratoa"].ToString();
                    txtPhongKham.Text = dt.Rows[0]["TENPHONG"].ToString();
                    sluCDXD.EditValue = dt.Rows[0]["ketluan"].ToString();
                    #endregion
                    Load_CLS(Truyendulieu.idkhambenh);
                    Load_CLS_hen(Truyendulieu.idkhambenh);
                    Load_ToaThuoc(Truyendulieu.idkhambenh);
                    Load_CDSB(Truyendulieu.idkhambenh);
                    Load_CDPH(Truyendulieu.idkhambenh);
                }
            }
        }

        public void Load_CDSB(string idkhambenh)
        {
            #region Load chẩn đoán sơ bộ
            DataTable luuCDSB = DataAcess.Connect.GetTable(GetData.dt_Load_CDSB(idkhambenh));
            if (luuCDSB == null)
            {
                MessageBox.Show("Không có cdsb");
                return;
            }
            else
            {
                for (int x = 0; x < luuCDSB.Rows.Count; x++)
                {
                    string dataGridViewTextBoxColumn4 = luuCDSB.Rows[x]["idicd"].ToString();
                    string dataGridViewTextBoxColumn5 = luuCDSB.Rows[x]["maicd"].ToString();
                    string dataGridViewTextBoxColumn6 = luuCDSB.Rows[x]["MoTa"].ToString();
                    string IDCDSB = luuCDSB.Rows[x]["id"].ToString();
                    string[] row = { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, IDCDSB };
                    dtgvCDSB.Rows.Add(row);
                    int colNumber = 0;
                    for (int i = 0; i < dtgvCDSB.Rows.Count; i++)
                    {
                        if (dtgvCDSB.Rows[i].IsNewRow) continue;
                        string tmp = dtgvCDSB.Rows[i].Cells[colNumber].Value.ToString();
                        for (int j = dtgvCDSB.Rows.Count - 1; j > i; j--)
                        {
                            if (dtgvCDSB.Rows[j].IsNewRow) continue;
                            if (tmp == dtgvCDSB.Rows[j].Cells[colNumber].Value.ToString())
                            {
                                dtgvCDSB.Rows.RemoveAt(j);
                            }
                        }
                    }
                }
            }
            dtgvCDSB.AutoResizeColumns();
            dtgvCDSB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            #endregion
        }

        public void Load_CDPH(string idkhambenh)
        {
            DataTable luuCDPH = DataAcess.Connect.GetTable(GetData.Load_CDPH(idkhambenh));
            if (luuCDPH == null)
            {
                MessageBox.Show("Không có cdph");
                return;
            }
            else
            {
                for (int t = 0; t < luuCDPH.Rows.Count; t++)
                {
                    string firstcolum = luuCDPH.Rows[t]["id_ph"].ToString();
                    string secondcolum = luuCDPH.Rows[t]["maicd_ph"].ToString();
                    string thirdcolum = luuCDPH.Rows[t]["MoTa_ph"].ToString();
                    string IDCDPH = luuCDPH.Rows[t]["id"].ToString();
                    string[] row = { firstcolum, secondcolum, thirdcolum, IDCDPH };
                    dataGridView1.Rows.Add(row);
                    int colNumber = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].IsNewRow) continue;
                        string tmp = dataGridView1.Rows[i].Cells[colNumber].Value.ToString();
                        for (int j = dataGridView1.Rows.Count - 1; j > i; j--)
                        {
                            if (dataGridView1.Rows[j].IsNewRow) continue;
                            if (tmp == dataGridView1.Rows[j].Cells[colNumber].Value.ToString())
                            {
                                dataGridView1.Rows.RemoveAt(j);
                            }
                        }
                    }
                }
            }
            dataGridView1.AutoResizeColumns();
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      
        }

        public void Load_ToaThuoc(string idkhambenh)
        {
            #region Load toa thuốc bảo hiểm
            DataTable dtluuThuoc = DataAcess.Connect.GetTable(GetData.dt_Load_Toathuoc(idkhambenh));
            if (dtluuThuoc == null)
            {
                MessageBox.Show("Không có thuốc");
                return;
            }
            else
            {
                for (int y = 0; y < dtluuThuoc.Rows.Count; y++)
                {
                    string khoxuat = dtluuThuoc.Rows[y]["tenkho"].ToString();
                    string doituong = dtluuThuoc.Rows[y]["TenLoai"].ToString();
                    string tenthuoc = dtluuThuoc.Rows[y]["tenthuoc"].ToString();
                    string hoatchat = dtluuThuoc.Rows[y]["congthuc"].ToString();
                    string dvt = dtluuThuoc.Rows[y]["TenDVT"].ToString();
                    string sluong = dtluuThuoc.Rows[y]["soluongke"].ToString();
                    string cachdung = dtluuThuoc.Rows[y]["tencachdung"].ToString();
                    string ngaydung = dtluuThuoc.Rows[y]["ngayuong"].ToString();
                    string moilan = dtluuThuoc.Rows[y]["moilanuong"].ToString();
                    string dvdung = dtluuThuoc.Rows[y]["Tendvdung"].ToString();
                    string sang = dtluuThuoc.Rows[y]["issang"].ToString();
                    string trua = dtluuThuoc.Rows[y]["istrua"].ToString();
                    string chieu = dtluuThuoc.Rows[y]["ischieu"].ToString();
                    string toi = dtluuThuoc.Rows[y]["istoi"].ToString();
                    string gchu = dtluuThuoc.Rows[y]["ghichu"].ToString();
                    string trongdm = dtluuThuoc.Rows[y]["IsBHYT_Save"].ToString();
                    string thoadk = dtluuThuoc.Rows[y]["IsBHYT_Save"].ToString();
                    string slton = dtluuThuoc.Rows[y]["slton"].ToString();
                    string isDaxuat = dtluuThuoc.Rows[y]["isDaxuat"].ToString();
                    string idctthuoc = dtluuThuoc.Rows[y]["idchitietbenhnhantoathuoc"].ToString();
                    string idthuoc = dtluuThuoc.Rows[y]["idthuoc"].ToString();
                    string[] row = { khoxuat, doituong, tenthuoc, hoatchat, dvt, sluong, cachdung, ngaydung, moilan, dvdung, sang, trua, chieu, toi, gchu, trongdm, thoadk, slton, isDaxuat, idctthuoc, idthuoc };
                    dtgvThuoc.Rows.Add(row);
                    int colNumber = 2;
                    for (int i = 0; i < dtgvThuoc.Rows.Count; i++)
                    {
                        if (dtgvThuoc.Rows[i].IsNewRow) continue;
                        string tmp = dtgvThuoc.Rows[i].Cells[colNumber].Value.ToString();
                        for (int j = dtgvThuoc.Rows.Count - 1; j > i; j--)
                        {
                            if (dtgvThuoc.Rows[j].IsNewRow) continue;
                            if (tmp == dtgvThuoc.Rows[j].Cells[colNumber].Value.ToString())
                            {
                                dtgvThuoc.Rows.RemoveAt(j);
                            }
                        }
                    }

                }
            }
            dtgvThuoc.AutoResizeColumns();
            dtgvThuoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            #endregion
        }

        public void Load_CLS(string idkhambenh)
        {
            #region Load CLS bác sĩ chỉ định
            DataTable dtluuCLS = DataAcess.Connect.GetTable(KhamBenhPro.GetData.dt_load_CLS(idkhambenh));
            if (dtluuCLS == null)
            {
                MessageBox.Show("Không có cls");
                return;
            }
            else
            {
                for (int i = 0; i < dtluuCLS.Rows.Count; i++)
                {
                    string tendichvu = dtluuCLS.Rows[i]["tendichvu"].ToString();
                    string giadichvu = dtluuCLS.Rows[i]["giadichvu"].ToString();
                    string giabh = dtluuCLS.Rows[i]["bhtra"].ToString();
                    string issudungbh = dtluuCLS.Rows[i]["IsSuDungChoBH"].ToString();
                    string soluong = dtluuCLS.Rows[i]["soluong"].ToString(); ;
                    string isbhyt_save = dtluuCLS.Rows[i]["bhyt_save"].ToString();
                    string ghichu = dtluuCLS.Rows[i]["ghichu"].ToString();
                    string fromdate = dtluuCLS.Rows[i]["fromdate"].ToString();
                    string isdathu = dtluuCLS.Rows[i]["dathu"].ToString();
                    string IdKBCLS = dtluuCLS.Rows[i]["IdKBCLS"].ToString();
                    string idcls = dtluuCLS.Rows[i]["idbanggiadichvu"].ToString();
                    string idnhomin = dtluuCLS.Rows[i]["idnhominbv"].ToString();
                    string[] row = { tendichvu, soluong, giadichvu, giabh, ghichu, issudungbh, isbhyt_save, fromdate, isdathu, IdKBCLS, idcls, idnhomin };
                    dtgvCLS.Rows.Add(row);
                    int colNumber = 0;
                    for (int t = 0; t < dtgvCLS.Rows.Count - 1; t++)
                    {
                        if (dtgvCLS.Rows[t].IsNewRow) continue;
                        string tmp = dtgvCLS.Rows[t].Cells[colNumber].Value.ToString();
                        for (int j = dtgvCLS.Rows.Count - 1; j > t; j--)
                        {
                            if (dtgvCLS.Rows[j].IsNewRow) continue;
                            if (tmp == dtgvCLS.Rows[j].Cells[colNumber].Value.ToString())
                            {
                                dtgvCLS.Rows.RemoveAt(j);
                            }
                        }
                    }

                }
            }
            dtgvCLS.AutoResizeColumns();
            dtgvCLS.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            #endregion
        }

        public void Load_CLS_hen(string idkhambenh)
        {
            #region Load CLS hẹn bác sĩ chỉ định
            DataTable dtluuCLS = DataAcess.Connect.GetTable(KhamBenhPro.GetData.dt_load_CLS_hen(idkhambenh));
            if (dtluuCLS == null)
            {
                MessageBox.Show("Không có cls");
                return;
            }
            else
            {
                for (int i = 0; i < dtluuCLS.Rows.Count; i++)
                {
                    string tendv_clshen = dtluuCLS.Rows[i]["tendichvu"].ToString();
                    string giadichvu_clshen = dtluuCLS.Rows[i]["giadichvu"].ToString();
                    string bhtra_clshen = dtluuCLS.Rows[i]["bhtra"].ToString();
                    string isBH_clshen = dtluuCLS.Rows[i]["IsSuDungChoBH"].ToString();
                    string soluong_clshen = dtluuCLS.Rows[i]["soluong"].ToString(); ;
                    string isSDBH_clshen = dtluuCLS.Rows[i]["bhyt_save"].ToString();
                    string ghichu_clshen = dtluuCLS.Rows[i]["ghichu"].ToString();
                    string fromdate_clshen = dtluuCLS.Rows[i]["fromdate"].ToString();
                    string isdathu_clshen = dtluuCLS.Rows[i]["dathu"].ToString();
                    string IdKBCLS_clshen = dtluuCLS.Rows[i]["IdKBCLS"].ToString();
                    string idCLS_clshen = dtluuCLS.Rows[i]["idbanggiadichvu"].ToString();
                    string idnhomin_clshen = dtluuCLS.Rows[i]["idnhominbv"].ToString();
                    string[] row = { tendv_clshen, soluong_clshen, giadichvu_clshen, bhtra_clshen, ghichu_clshen, isBH_clshen, isSDBH_clshen, fromdate_clshen, isdathu_clshen, IdKBCLS_clshen,idCLS_clshen, idnhomin_clshen };
                    dtgvCLSHen.Rows.Add(row);
                    int colNumber = 0;
                    for (int t = 0; t < dtgvCLSHen.Rows.Count - 1; t++)
                    {
                        if (dtgvCLSHen.Rows[t].IsNewRow) continue;
                        string tmp = dtgvCLSHen.Rows[t].Cells[colNumber].Value.ToString();
                        for (int j = dtgvCLSHen.Rows.Count - 1; j > t; j--)
                        {
                            if (dtgvCLSHen.Rows[j].IsNewRow) continue;
                            if (tmp == dtgvCLSHen.Rows[j].Cells[colNumber].Value.ToString())
                            {
                                dtgvCLSHen.Rows.RemoveAt(j);
                            }
                        }
                    }

                }
            }
            dtgvCLSHen.AutoResizeColumns();
            dtgvCLSHen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            #endregion
        }


        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private string dt_Load_Bacsi()
        {
            string sql = "SELECT idbacsi,tenbacsi,mabacsi FROM dbo.bacsi WHERE mabacsi like '%CCHN%'";
            return sql;
        }

        #region Load bác sĩ 1
        public void LoadsluBacsi()
        {
            DataTable dtBacsi = DataAcess.Connect.GetTable(this.dt_Load_Bacsi());
            sluBacsi.Properties.DataSource = dtBacsi;
            sluBacsi.Properties.DisplayMember = "tenbacsi";
            sluBacsi.Properties.ValueMember = "idbacsi";
            sluBacsi.Properties.NullText = "Nhập Bác sĩ";
            sluBacsi.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            sluBacsi.Properties.ImmediatePopup = true;
            sluBacsi.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }
        #endregion

        #region Load Bác sĩ 2
        public void LoadsluBacsi2()
        {
            DataTable dtBacsi2 = DataAcess.Connect.GetTable(this.dt_Load_Bacsi());
            gluBacSi2.Properties.DataSource = dtBacsi2;
            gluBacSi2.Properties.DisplayMember = "tenbacsi";
            gluBacSi2.Properties.ValueMember = "idbacsi";
            gluBacSi2.Properties.NullText = "Nhập Bác sĩ 2";
            gluBacSi2.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            gluBacSi2.Properties.ImmediatePopup = true;
            gluBacSi2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }
        #endregion

        #region Load Khoa chuyển đến
        public void Load_Khoa()
        {
            string sql = @" select idphongkhambenh,tenphongkhambenh
                                                 from phongkhambenh 
                                                 where maphongkhambenh is not null 
                                                 and loaiphong=0";
            DataTable dtKhoa = DataAcess.Connect.GetTable(sql);
            sluKhoa.Properties.NullText = "Chọn Khoa chuyển";
            sluKhoa.Properties.DataSource = dtKhoa;
            sluKhoa.Properties.DisplayMember = "tenphongkhambenh";
            sluKhoa.Properties.ValueMember = "idphongkhambenh";
            sluKhoa.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            sluKhoa.Properties.ImmediatePopup = true;
            sluKhoa.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }
        #endregion

        #region Load Phòng khám chuyển đến
        public void Load_PhongKham()
        {
            string sql=@"select p.id,p.maso+'-'+p.TenPhong as 'tenphong'
                                            from KB_Phong p
                                            inner join banggiadichvu bg on bg.idbanggiadichvu = p.DichVuKCB
                                            where bg.idphongkhambenh='" + sluKhoa.EditValue.ToString() + @"'
                                            and p.isPhongNoiTru = 0
                                            and p.IsActive = 1
                                            and p.Status=1
                                            and p.id<>'"+Truyendulieu.PhongKhamID+@"'
                                            order by p.MaSo";
            DataTable dtPhong2 = DataAcess.Connect.GetTable(sql);
             sluPK.Properties.NullText = "Chọn phòng khám";
            sluPK.Properties.DataSource = dtPhong2;
            sluPK.Properties.DisplayMember = "tenphong";
            sluPK.Properties.ValueMember = "id";
            sluPK.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            sluPK.Properties.ImmediatePopup = true;
            sluPK.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
       
        }
        #endregion

        #region Load Chẩn đoán xác định
        public void LoadCDXD()
        {
            DataTable CDXD = DataAcess.Connect.GetTable(GetData.LoadICD10());
            sluCDXD.Properties.DataSource = CDXD;
            sluCDXD.Properties.DisplayMember = "MaICD";
            sluCDXD.Properties.ValueMember = "IDICD";
            sluCDXD.Properties.NullText = "Nhập chẩn đoán";
            sluCDXD.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            sluCDXD.Properties.ImmediatePopup = true;
            sluCDXD.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
           
        }
        #endregion

        #region Load Chẩn đoán phối hợp lên SearchLookup
        public void LoadgluChandoan()
        {
            DataTable dtChandoan = DataAcess.Connect.GetTable(GetData.LoadICD10());
            gluChanDoan.Properties.DataSource = dtChandoan;
            gluChanDoan.Properties.DisplayMember = "MoTa";
            gluChanDoan.Properties.ValueMember = "IDICD";
            gluChanDoan.Properties.NullText = "Nhập chẩn đoán";
            gluChanDoan.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            gluChanDoan.Properties.ImmediatePopup = true;
            gluChanDoan.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
 
        }
        #endregion

        #region Load Chẩn đoán Sơ bộ lên SearchLookup
        public void Load_ChanDoanSoBo()
        {
            DataTable dtChandoanSB = DataAcess.Connect.GetTable(GetData.LoadICD10());
            gluCDSobo.Properties.DataSource = dtChandoanSB;
            gluCDSobo.Properties.DisplayMember = "MoTa";
            gluCDSobo.Properties.ValueMember = "IDICD";
            gluCDSobo.Properties.NullText = "Nhập chẩn đoán sơ bộ";
            gluCDSobo.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            gluCDSobo.Properties.ImmediatePopup = true;
            gluCDSobo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
   
        }
        #endregion

        private void gluChanDoan_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void gluChanDoan_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        #region Load cls lên searchLookUpEdit
        public void LoadSLCanLamSang()
        {
           string sql=@"SELECT A.idbanggiadichvu as idbanggiadichvu
                                               ,A.tendichvu as tendichvu
                                               ,BH.GiaDV as giadichvu
                                               ,BH.GIABH as bhtra
                                               ,IsSuDungChoBH=BH.ISBHYT
											   ,bh.TuNgay as fromdate
											    ,A.TENBAOHIEM as tenbaohiem
                  				            FROM BANGGIADICHVU A
               				                LEFT JOIN PHONGKHAMBENH b on a.idphongkhambenh=b.idphongkhambenh
                                            left join hs_banggiavienphi BH ON BH.IdGiaDichVu=(SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu=A.IDBANGGIADICHVU AND BH0.TuNgay<=GETDATE() ORDER BY TuNgay DESC)
                                            WHERE b.loaiphong = 1 and a.IsActive=1";
            DataTable dtCLS1 = DataAcess.Connect.GetTable(sql);
            SLCANLAMSANG.Properties.DataSource = dtCLS1;
            SLCANLAMSANG.Properties.NullText = "Nhập Cận lâm sàng";
            SLCANLAMSANG.Properties.DisplayMember = "tendichvu";
            SLCANLAMSANG.Properties.ValueMember = "idbanggiadichvu";
            SLCANLAMSANG.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            SLCANLAMSANG.Properties.ImmediatePopup = true;
            SLCANLAMSANG.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }
        #endregion

        public void LoadSLCanLamSang_hen()
        {
            string sql = @"SELECT A.idbanggiadichvu as idbanggiadichvu
                                               ,A.tendichvu as tendichvu
                                               ,BH.GiaDV as giadichvu
                                               ,BH.GIABH as bhtra
                                               ,IsSuDungChoBH=BH.ISBHYT
											   ,bh.TuNgay as fromdate
											    ,A.TENBAOHIEM as tenbaohiem
                  				            FROM BANGGIADICHVU A
               				                LEFT JOIN PHONGKHAMBENH b on a.idphongkhambenh=b.idphongkhambenh
                                            left join hs_banggiavienphi BH ON BH.IdGiaDichVu=(SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu=A.IDBANGGIADICHVU AND BH0.TuNgay<=GETDATE() ORDER BY TuNgay DESC)
                                            WHERE b.loaiphong = 1 and a.IsActive=1";
            DataTable dtCLS1 = DataAcess.Connect.GetTable(sql);
            slkCLSHen.Properties.DataSource = dtCLS1;
            slkCLSHen.Properties.NullText = "Nhập Cận lâm sàng";
            slkCLSHen.Properties.DisplayMember = "tendichvu";
            slkCLSHen.Properties.ValueMember = "idbanggiadichvu";
            slkCLSHen.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkCLSHen.Properties.ImmediatePopup = true;
            slkCLSHen.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }
        private void btnThemCLS_Click(object sender, EventArgs e)
        {


        }

        private void gVCLS_InitNewRow(object sender, InitNewRowEventArgs e)
        {

        }

        public void simpleButton1_Click(object sender, EventArgs e)
        {

            LoadCLS_theoID();

        }

        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {


        }
        public void LoadCLS_theoID()
        {
            #region Load Cận lâm sàng theo IDCLS
            try
            {
                string sql= @"select idbanggiadichvu,tendichvu,giadichvu,bhtra,IsSuDungChoBH,fromdate,IdnhomInBV from banggiadichvu where IsActive=1 and idbanggiadichvu='" + SLCANLAMSANG.EditValue.ToString() + "'";
                DataTable dtCLS = DataAcess.Connect.GetTable(sql);
                string tendichvu = dtCLS.Rows[0]["tendichvu"].ToString();
                string giadichvu = dtCLS.Rows[0]["giadichvu"].ToString();
                string giabh = dtCLS.Rows[0]["bhtra"].ToString();
                string issudungbh = dtCLS.Rows[0]["IsSuDungChoBH"].ToString();
                string soluong = "1";
                string isbhyt_save = dtCLS.Rows[0]["IsSuDungChoBH"].ToString();
                string ghichu = "";
                string fromdate = dtCLS.Rows[0]["fromdate"].ToString();
                string isdathu = "0";
                string IdKBCLS = "";
                string idcls = dtCLS.Rows[0]["idbanggiadichvu"].ToString();
                string idnhomin = dtCLS.Rows[0]["IdnhomInBV"].ToString();
                string[] row = { tendichvu, soluong, giadichvu, giabh, ghichu, issudungbh, isbhyt_save, fromdate, isdathu, IdKBCLS, idcls, idnhomin };
                dtgvCLS.Rows.Add(row);
                dtgvCLS.AutoResizeColumns();
                dtgvCLS.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            }
            catch
            {
                MessageBox.Show("Chưa chọn CLS");
            }
            int colNumber = 0;
            for (int i = 0; i < dtgvCLS.Rows.Count - 1; i++)
            {
                if (dtgvCLS.Rows[i].IsNewRow) continue;
                string tmp = dtgvCLS.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dtgvCLS.Rows.Count - 1; j > i; j--)
                {
                    if (dtgvCLS.Rows[j].IsNewRow) continue;
                    if (tmp == dtgvCLS.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dtgvCLS.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion
        }

        public void LoadCLS_theoID_hen()
        {
            #region Load Cận lâm sàng theo IDCLS
            try
            {
                string sql = @"select idbanggiadichvu,tendichvu,giadichvu,bhtra,IsSuDungChoBH,fromdate,IdnhomInBV from banggiadichvu where IsActive=1 and idbanggiadichvu='" + slkCLSHen.EditValue.ToString() + "'";
                DataTable dtCLS = DataAcess.Connect.GetTable(sql);
                string tendichvu = dtCLS.Rows[0]["tendichvu"].ToString();
                string giadichvu = dtCLS.Rows[0]["giadichvu"].ToString();
                string giabh = dtCLS.Rows[0]["bhtra"].ToString();
                string issudungbh = dtCLS.Rows[0]["IsSuDungChoBH"].ToString();
                string soluong = "1";
                string isbhyt_save = dtCLS.Rows[0]["IsSuDungChoBH"].ToString();
                string ghichu = "";
                string fromdate = dtCLS.Rows[0]["fromdate"].ToString();
                string isdathu = "0";
                string IdKBCLS = "";
                string idcls = dtCLS.Rows[0]["idbanggiadichvu"].ToString();
                string idnhomin = dtCLS.Rows[0]["IdnhomInBV"].ToString();
                string[] row = { tendichvu, soluong, giadichvu, giabh, ghichu, issudungbh, isbhyt_save, fromdate, isdathu, IdKBCLS, idcls, idnhomin };
                dtgvCLSHen.Rows.Add(row);
                dtgvCLSHen.AutoResizeColumns();
                dtgvCLSHen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            }
            catch
            {
                MessageBox.Show("Chưa chọn CLS");
            }
            int colNumber = 0;
            for (int i = 0; i < dtgvCLSHen.Rows.Count - 1; i++)
            {
                if (dtgvCLSHen.Rows[i].IsNewRow) continue;
                string tmp = dtgvCLSHen.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dtgvCLSHen.Rows.Count - 1; j > i; j--)
                {
                    if (dtgvCLSHen.Rows[j].IsNewRow) continue;
                    if (tmp == dtgvCLSHen.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dtgvCLSHen.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion
        }


        private void btnXoaCLS_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell oneCell in dtgvCLS.SelectedCells)
                {
                    if (oneCell.Selected)
                        dtgvCLS.Rows.RemoveAt(oneCell.RowIndex);
                }
            }
            catch
            {
                MessageBox.Show("Bạn chọn ô trống rồi");
            }
        }

        private void btnXoaHen_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell oneCell in dtgvCLSHen.SelectedCells)
                {
                    if (oneCell.Selected)
                        dtgvCLSHen.Rows.RemoveAt(oneCell.RowIndex);
                }
            }
            catch
            {
                MessageBox.Show("Bạn chọn ô trống rồi");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void btnNhomCLS_Click(object sender, EventArgs e)
        {

        }

        private void SLCANLAMSANG_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        public void NhomCLS_load()
        {
            #region Load nhóm cận lâm sàng
           string sql=@"select NhomId,TenNhom,GhiChu from  KB_NhomCLS";
            DataTable dtNhomCLS = DataAcess.Connect.GetTable(sql);
            slNhomCLS.Properties.DataSource = dtNhomCLS;
            slNhomCLS.Properties.NullText = "Nhập Nhóm CLS";
            slNhomCLS.Properties.DisplayMember = "TenNhom";
            slNhomCLS.Properties.ValueMember = "NhomId";
            slNhomCLS.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slNhomCLS.Properties.ImmediatePopup = true;
            slNhomCLS.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            #endregion
        }

        public void NhomCLS_load_hen()
        {
            #region Load nhóm cận lâm sàng
            string sql = @"select NhomId,TenNhom,GhiChu from  KB_NhomCLS";
            DataTable dtNhomCLS = DataAcess.Connect.GetTable(sql);
            slkNhomCLSHen.Properties.DataSource = dtNhomCLS;
            slkNhomCLSHen.Properties.NullText = "Nhập Nhóm CLS";
            slkNhomCLSHen.Properties.DisplayMember = "TenNhom";
            slkNhomCLSHen.Properties.ValueMember = "NhomId";
            slkNhomCLSHen.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkNhomCLSHen.Properties.ImmediatePopup = true;
            slkNhomCLSHen.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            #endregion
        }
        public void NhomCLS_TheoID_Load()
        {
            #region Load Nhóm CLS theo ID nhóm
            try
            {
                string sql=@" select b.idbanggiadichvu as idbanggiadichvu
                                                   ,B.tendichvu as tendichvu
                                                   ,BH.GiaDV as giadichvu
                                                   , BH.GiaBH as bhtra
                                                   ,isnull(BH.ISBHYT,0) as IsSuDungChoBH
                                                   ,soluong =1
                                                   ,bh.tungay as fromdate
                                                   ,b.IdnhomInBV as IdnhomInBV
                                                   from KB_ChiTietNhomCLS T
                                                   left join KB_NhomCLS A on T.NhomID = A.NhomId
                                                   left join banggiadichvu B on T.idbanggiadichvu = B.idbanggiadichvu
                                                   left join phongkhambenh pkb on b.idphongkhambenh = pkb.idphongkhambenh
                                                   left join hs_banggiavienphi BH ON BH.IdGiaDichVu = (SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu = B.IDBANGGIADICHVU AND BH0.TuNgay <= GETDATE() ORDER BY TuNgay DESC)
                                                   where T.NhomID ='" + slNhomCLS.EditValue.ToString() + "'";

                DataTable dtNhomCLS1 = DataAcess.Connect.GetTable(sql);
                for (int i = 0; i < dtNhomCLS1.Rows.Count; i++)
                {
                    //dtgvCLS.DataSource = dtNhomCLS1;
                    string tendichvu = dtNhomCLS1.Rows[i]["tendichvu"].ToString();
                    string giadichvu = dtNhomCLS1.Rows[i]["giadichvu"].ToString();
                    string giabh = dtNhomCLS1.Rows[i]["bhtra"].ToString();
                    string issudungbh = dtNhomCLS1.Rows[i]["IsSuDungChoBH"].ToString();
                    string soluong = "1";
                    string isbhyt_save = dtNhomCLS1.Rows[i]["IsSuDungChoBH"].ToString();
                    string ghichu = "";
                    string fromdate = dtNhomCLS1.Rows[i]["fromdate"].ToString();
                    string isdathu = "0";
                    string IdKBCLS = "";
                    string idcls = dtNhomCLS1.Rows[i]["idbanggiadichvu"].ToString();
                    string idnhomin = dtNhomCLS1.Rows[i]["IdnhomInBV"].ToString();
                    string[] row = { tendichvu, soluong, giadichvu, giabh, ghichu, issudungbh, isbhyt_save, fromdate, isdathu, IdKBCLS, idcls, idnhomin };
                    dtgvCLS.Rows.Add(row);
                }

                dtgvCLS.AutoResizeColumns();
                dtgvCLS.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
              
            }
            catch
            { MessageBox.Show("Chưa chọn nhóm CLS"); }
            int colNumber = 0;
            for (int i = 0; i < dtgvCLS.Rows.Count - 1; i++)
            {
                if (dtgvCLS.Rows[i].IsNewRow) continue;
                string tmp = dtgvCLS.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dtgvCLS.Rows.Count - 1; j > i; j--)
                {
                    if (dtgvCLS.Rows[j].IsNewRow) continue;
                    if (tmp == dtgvCLS.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dtgvCLS.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion
        }

        public void NhomCLS_TheoID_Load_hen()
        {
            #region Load Nhóm CLS theo ID nhóm
            try
            {
                string sql = @" select b.idbanggiadichvu as idbanggiadichvu
                                                   ,B.tendichvu as tendichvu
                                                   ,BH.GiaDV as giadichvu
                                                   , BH.GiaBH as bhtra
                                                   ,isnull(BH.ISBHYT,0) as IsSuDungChoBH
                                                   ,soluong =1
                                                   ,bh.tungay as fromdate
                                                   ,b.IdnhomInBV as IdnhomInBV
                                                   from KB_ChiTietNhomCLS T
                                                   left join KB_NhomCLS A on T.NhomID = A.NhomId
                                                   left join banggiadichvu B on T.idbanggiadichvu = B.idbanggiadichvu
                                                   left join phongkhambenh pkb on b.idphongkhambenh = pkb.idphongkhambenh
                                                   left join hs_banggiavienphi BH ON BH.IdGiaDichVu = (SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu = B.IDBANGGIADICHVU AND BH0.TuNgay <= GETDATE() ORDER BY TuNgay DESC)
                                                   where T.NhomID ='" + slkNhomCLSHen.EditValue.ToString() + "'";

                DataTable dtNhomCLS1 = DataAcess.Connect.GetTable(sql);
                for (int i = 0; i < dtNhomCLS1.Rows.Count; i++)
                {
                    //dtgvCLS.DataSource = dtNhomCLS1;
                    string tendichvu = dtNhomCLS1.Rows[i]["tendichvu"].ToString();
                    string giadichvu = dtNhomCLS1.Rows[i]["giadichvu"].ToString();
                    string giabh = dtNhomCLS1.Rows[i]["bhtra"].ToString();
                    string issudungbh = dtNhomCLS1.Rows[i]["IsSuDungChoBH"].ToString();
                    string soluong = "1";
                    string isbhyt_save = dtNhomCLS1.Rows[i]["IsSuDungChoBH"].ToString();
                    string ghichu = "";
                    string fromdate = dtNhomCLS1.Rows[i]["fromdate"].ToString();
                    string isdathu = "0";
                    string IdKBCLS = "";
                    string idcls = dtNhomCLS1.Rows[i]["idbanggiadichvu"].ToString();
                    string idnhomin = dtNhomCLS1.Rows[i]["IdnhomInBV"].ToString();
                    string[] row = { tendichvu, soluong, giadichvu, giabh, ghichu, issudungbh, isbhyt_save, fromdate, isdathu, IdKBCLS, idcls, idnhomin };
                    dtgvCLSHen.Rows.Add(row);
                }

                dtgvCLSHen.AutoResizeColumns();
                dtgvCLSHen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            }
            catch
            { MessageBox.Show("Chưa chọn nhóm CLS"); }
            int colNumber = 0;
            for (int i = 0; i < dtgvCLSHen.Rows.Count - 1; i++)
            {
                if (dtgvCLSHen.Rows[i].IsNewRow) continue;
                string tmp = dtgvCLSHen.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dtgvCLSHen.Rows.Count - 1; j > i; j--)
                {
                    if (dtgvCLSHen.Rows[j].IsNewRow) continue;
                    if (tmp == dtgvCLSHen.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dtgvCLSHen.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion
        }

        private void SLCANLAMSANG_EditValueChanged(object sender, EventArgs e)
        {
            // LoadCLS_theoID();
        }

        private void slNhomCLS_EditValueChanged(object sender, EventArgs e)
        {
            //  NhomCLS_TheoID_Load();
        }

        private void SLCANLAMSANG_KeyDown(object sender, KeyEventArgs e)
        {
            // LoadCLS_theoID();
        }

        private void btnNhomCLS_Click_1(object sender, EventArgs e)
        {
            NhomCLS_TheoID_Load();
        }

        private void groupControl2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            
        }

        private void btnXoaCD_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell oneCell in dataGridView1.SelectedCells)
                {
                    if (oneCell.Selected)
                        dataGridView1.Rows.RemoveAt(oneCell.RowIndex);
                }
            }
            catch
            {
                MessageBox.Show("Bạn chọn ô trống rồi");
            }
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewCell oneCell in dtgvCDSB.SelectedCells)
                {
                    if (oneCell.Selected)
                        dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
                }
            }
            catch
            {
                MessageBox.Show("Bạn chọn ô trống rồi");
            }
        }

        private void btnThemCDSB_Click(object sender, EventArgs e)
        {
            
        }

        public void KhoThuoc_load()
        {
            #region Load kho thuốc
           string sql=@"select idkho,tenkho from khothuoc where idkho in (72,5)";
            DataTable dtKhothuoc = DataAcess.Connect.GetTable(sql);
            sluKho.Properties.DataSource = dtKhothuoc;
            sluKho.Properties.NullText = "Chọn Kho";
            sluKho.Properties.DisplayMember = "tenkho";
            sluKho.Properties.ValueMember = "idkho";
            sluKho.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            sluKho.Properties.ImmediatePopup = true;
            sluKho.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
 
            #endregion
        }
        public void Load_Thuoc_cachdung()
        {
            #region Load cách dùng lên ComboBox trong dataGridview
            string sql=@"select  idcachdung,tencachdung from Thuoc_CachDung where tencachdung is not null order by idcachdung";
            DataTable dtCachdung = DataAcess.Connect.GetTable(sql);
            DataGridViewComboBoxCell box = dtgvThuoc.Rows[0].Cells["cachdung"] as DataGridViewComboBoxCell;
            //(dtgvThuoc.Columns["cachdung"] as DataGridViewComboBoxColumn).DisplayMember = "tencachdung";
            //(dtgvThuoc.Columns["cachdung"] as DataGridViewComboBoxColumn).ValueMember = "idcachdung";
            //(dtgvThuoc.Columns["cachdung"] as DataGridViewComboBoxColumn).DataSource = dtCachdung;
            box.DataSource = dtCachdung;
            box.DisplayMember = "tencachdung";
            box.ValueMember = "idcachdung";

            #endregion
        }

        public void Load_thuoc_donvidung()
        {
            #region Load Đơn vị dùng lên ComboBox trong dataGrigview
           string sql=@"select id,TenDVT from Thuoc_DonViTinh order by id ";
            DataTable dtDonvidung = DataAcess.Connect.GetTable(sql);
            // dtgvThuoc.AutoGenerateColumns = false;
            //dtgvThuoc.DataSource = dtCachdung;
            //(dtgvThuoc.Columns["dvdung"] as DataGridViewComboBoxColumn).DisplayMember = "TenDVT";
            //(dtgvThuoc.Columns["dvdung"] as DataGridViewComboBoxColumn).ValueMember = "id";
            //(dtgvThuoc.Columns["dvdung"] as DataGridViewComboBoxColumn).DataSource = dtDonvidung;
            DataGridViewComboBoxCell box1 = dtgvThuoc.Rows[0].Cells["dvdung"] as DataGridViewComboBoxCell;
            box1.DataSource = dtDonvidung;
            box1.DisplayMember = "TenDVT";
            box1.ValueMember = "id";
           
            #endregion
        }

        public void Load_thuoc_donvitinh()
        {
            #region Load Đơn vị tính lên ComboBox trong dataGrigview
            string sql=@"select id as iddvt,TenDVT from Thuoc_DonViTinh order by id ";
            DataTable dtDonvitinh = DataAcess.Connect.GetTable(sql);
            // dtgvThuoc.AutoGenerateColumns = false;
            //dtgvThuoc.DataSource = dtCachdung;
            //(dtgvThuoc.Columns["dvt"] as DataGridViewComboBoxColumn).DisplayMember = "TenDVT";
            //(dtgvThuoc.Columns["dvt"] as DataGridViewComboBoxColumn).ValueMember = "id";
            //(dtgvThuoc.Columns["dvt"] as DataGridViewComboBoxColumn).DataSource = dtDonvitinh;
            DataGridViewComboBoxCell box2 = dtgvThuoc.Rows[0].Cells["dvt"] as DataGridViewComboBoxCell;
            box2.DataSource = dtDonvitinh;
            box2.DisplayMember = "TenDVT";
            box2.ValueMember = "iddvt";
            
            #endregion
        }
        public void Load_thuoc_doituong()
        {
            #region Load đối tượng Thuốc,VTYT..
           string sql=@"select LoaiThuocID,TenLoai from Thuoc_LoaiThuoc ";
            DataTable dtDoituong = DataAcess.Connect.GetTable(sql);
            sluDoituong.Properties.DataSource = dtDoituong;
            sluDoituong.Properties.NullText = "Nhập đối tượng";
            sluDoituong.Properties.DisplayMember = "TenLoai";
            sluDoituong.Properties.ValueMember = "LoaiThuocID";

            #endregion
        }

        private string Thuoc_BH()
        {
            string sql = @"select * from (SELECT B.IDTHUOC as idthuoc
										,B.TENTHUOC as tenthuoc
										,B.LOAITHUOCID as loaithuocid
										,C.TENDVT as donvitinh
										,B.iddvt
                                        ,B.congthuc as congthuc
                                        , cd.tencachdung as duongdung
										,cd.idcachdung as idcachdung
										,cd.tencachdung as tencachdung
                                        ,(CASE WHEN B.sudungchobh=1 THEN 'BH' ELSE 'DV' END) as isbhyt
                                        ,B.isthuocbv
                                        ,SLTON= ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUNHAPKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_NHAP=5),0)-ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUXUATKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_XUAT=5 ),0)
                                        , DonGia  = B.GIA_MUA
                                         ,TrungThuoc=''
                                         FROM Thuoc B  
                                        left join thuoc_donvitinh C on C.id=B.iddvt
                                        left join thuoc_cachdung cd on cd.idcachdung=B.idcachdung
                                        where     ISNULL( B.IsNgungSD,0)=0
										AND B.LOAITHUOCID=1
										AND B.ISTHUOCBV=1
                                        and b.tenthuoc is not null)ab
                                        where slton>0 and dongia>0
										ORDER BY TENTHUOC";
            return sql;
        }

        private string Thuoc_DV()
        {
            string sql = @" select * from (SELECT B.IDTHUOC as idthuoc
						                                ,B.TENTHUOC as tenthuoc
                                                        ,B.LOAITHUOCID as loaithuocid
                                                        ,C.TENDVT as donvitinh
                                                        ,B.iddvt
                                                        ,B.congthuc as congthuc
                                                        ,cd.tencachdung as duongdung
                                                        ,cd.idcachdung as idcachdung
                                                        ,cd.tencachdung as tencachdung
                                                        ,(CASE WHEN B.sudungchobh=1 THEN 'BH' ELSE 'DV' END) as isbhyt
                                                        ,B.isthuocbv
                                                        ,SLTON = (SELECT SUM(SOLUONG) FROM NhaThuocDB.DBO.CHITIETPHIEUNHAPKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_NHAP = 72 )-ISNULL((SELECT SUM(SOLUONG) FROM NhaThuocDB.DBO.CHITIETPHIEUXUATKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_XUAT = 72),0)
                                                        ,DonGia = (SELECT TOP 1 NhaThuocDB.DBO.zHs_GetGiaBan(DONGIA, VAT) FROM NhaThuocDB.DBO.CHITIETPHIEUNHAPKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_NHAP = 72)
                                                        ,TrungThuoc = ''
                        FROM NhaThuocDB.DBO.Thuoc B
                        left join NhaThuocDB.DBO.thuoc_donvitinh C on C.id = B.iddvt
                        left join NhaThuocDB.DBO.thuoc_cachdung cd on cd.idcachdung = B.idcachdung
                        LEFT JOIN NhaThuocDB.DBO.zHS_ThuTuThuoc T4 ON(SELECT TOP  1 IdSoTT FROM NhaThuocDB.DBO.zHS_ThuTuThuoc T5 WHERE    T5.IDTHUOC = B.IDTHUOC AND T5.IDKHO = 5 AND dMonth <= GETDATE()  ORDER BY  dMonth DESC) = T4.IdSoTT
                        where B.ISTHUOCBV = 1  AND ISNULL(T4.SoTT, 0) <> -1
						and b.LoaiThuocID=1
                        and b.tenthuoc is not null
						AND ISNULL(B.IsNgungSD,0)=0)ab
                        where slton>0 and dongia>0
						ORDER BY  isnull(isbhyt,0) desc, isnull( isthuocbv,0) desc ,tenthuoc ASC";
            return sql;
        }
        public void Load_Thuoc()
        {
            DataTable dtthuoc = null;
            #region Load thuốc lên SearchLookUpEdit
            try
            {
                if (sluKho.EditValue.ToString() == "72")
                {
                    dtthuoc = DataAcess.Connect.GetTable(Thuoc_DV());
                }
                else
                        if (sluKho.EditValue.ToString() == "5")
                {
                    dtthuoc = DataAcess.Connect.GetTable(Thuoc_BH());
                }
                sluThuoc.Properties.DataSource = dtthuoc;
                sluThuoc.Properties.NullText = "Nhập tên thuốc";
                sluThuoc.Properties.DisplayMember = "tenthuoc";
                sluThuoc.Properties.ValueMember = "idthuoc";
                sluThuoc.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
                sluThuoc.Properties.ImmediatePopup = true;
                sluThuoc.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            }
            catch { MessageBox.Show("Chưa chọn kho"); }
                   #endregion
        }

        private void sluThuoc_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void sluThuoc_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void sluThuoc_Click(object sender, EventArgs e)
        {

        }
        private string Thuoc_BH_Id()
        {
            string sql = @"SELECT B.IDTHUOC as idthuoc
										,B.TENTHUOC as tenthuoc
										,B.LOAITHUOCID as loaithuocid
										,C.TENDVT as donvitinh
										,B.iddvt as iddvt
                                        ,B.congthuc as congthuc
                                     	,cd.idcachdung as idcachdung
										,cd.tencachdung as tencachdung
                                        , B.sudungchobh as isbhyt
                                        ,B.isthuocbv
                                        ,SLTON= ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUNHAPKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_NHAP='" + sluKho.EditValue.ToString() + @"'),0)-ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUXUATKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_XUAT='" + sluKho.EditValue.ToString() + @"' ),0)
                                        , DonGia  = B.GIA_MUA
                                        ,b.idthuoc as idthuoc
                                        ,TrungThuoc=''
                                      FROM Thuoc B  
                                        left join thuoc_donvitinh C on C.id=B.iddvt
                                        left join thuoc_cachdung cd on cd.idcachdung=B.idcachdung
                                        where     ISNULL( B.IsNgungSD,0)=0
										AND B.LOAITHUOCID=1
										AND B.ISTHUOCBV=1
                                        and b.tenthuoc is not null
                                        and b.idthuoc='" + sluThuoc.EditValue.ToString() + @"'
										ORDER BY TENTHUOC";
            return sql;
        }
        private string Thuoc_DV_Id()
        {
            string sql = @"SELECT B.IDTHUOC as idthuoc
						                                ,B.TENTHUOC as tenthuoc
                                                        ,B.LOAITHUOCID as loaithuocid
                                                        ,C.TENDVT as donvitinh
                                                        ,B.iddvt
                                                        ,B.congthuc as congthuc
                                                        ,cd.tencachdung as duongdung
                                                        ,cd.idcachdung as idcachdung
                                                        ,cd.tencachdung as tencachdung
                                                        ,(CASE WHEN B.sudungchobh=1 THEN 'BH' ELSE 'DV' END) as isbhyt
                                                        ,B.isthuocbv
                                                        ,SLTON = (SELECT SUM(SOLUONG) FROM NhaThuocDB.DBO.CHITIETPHIEUNHAPKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_NHAP = 72 )-ISNULL((SELECT SUM(SOLUONG) FROM NhaThuocDB.DBO.CHITIETPHIEUXUATKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_XUAT = 72),0)
                                                        ,DonGia = (SELECT TOP 1 NhaThuocDB.DBO.zHs_GetGiaBan(DONGIA, VAT) FROM NhaThuocDB.DBO.CHITIETPHIEUNHAPKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_NHAP = 72)
                                                        ,TrungThuoc = ''
                        FROM NhaThuocDB.DBO.Thuoc B
                        left join NhaThuocDB.DBO.thuoc_donvitinh C on C.id = B.iddvt
                        left join NhaThuocDB.DBO.thuoc_cachdung cd on cd.idcachdung = B.idcachdung
                        LEFT JOIN NhaThuocDB.DBO.zHS_ThuTuThuoc T4 ON(SELECT TOP  1 IdSoTT FROM NhaThuocDB.DBO.zHS_ThuTuThuoc T5 WHERE    T5.IDTHUOC = B.IDTHUOC AND T5.IDKHO = 5 AND dMonth <= GETDATE()  ORDER BY  dMonth DESC) = T4.IdSoTT
                        where B.ISTHUOCBV = 1  AND ISNULL(T4.SoTT, 0) <> -1
                        and b.tenthuoc is not null
                        and b.idthuoc='" + sluThuoc.EditValue.ToString() + @"'
                        ORDER BY TENTHUOC";
            return sql;
        }
        public void Load_thuoc_theoID()
        {
            #region Load thuốc theo Id thuốc
            try
            {
                DataTable dtThuocID = null;
                if(sluKho.EditValue.ToString()=="72")
                {
                    dtThuocID = DataAcess.Connect.GetTable(Thuoc_DV_Id());
                }
                else
                    if(sluKho.EditValue.ToString()=="5")
                {
                    dtThuocID = DataAcess.Connect.GetTable(Thuoc_BH_Id());
                }
                string khoxuat = sluKho.Text.ToString();
                string doituong = sluDoituong.Text.ToString();
                string tenthuoc = dtThuocID.Rows[0]["tenthuoc"].ToString();
                string hoatchat = dtThuocID.Rows[0]["congthuc"].ToString();
                string dvt = dtThuocID.Rows[0]["donvitinh"].ToString();
                string sluong = "";
                string cachdung = dtThuocID.Rows[0]["tencachdung"].ToString();
                string ngaydung = "2";
                string moilan = "1";
                string dvdung = dtThuocID.Rows[0]["donvitinh"].ToString();
                string sang = "1";
                string trua = "0";
                string chieu = "1";
                string toi = "0";
                string gchu = "";
                string trongdm = dtThuocID.Rows[0]["isbhyt"].ToString();
                string thoadk = dtThuocID.Rows[0]["isbhyt"].ToString();
                string slton = dtThuocID.Rows[0]["SLTON"].ToString();
                string isDaxuat = "0";
                string idctthuoc = "";
                string idthuoc = dtThuocID.Rows[0]["idthuoc"].ToString();
                string[] row = { khoxuat, doituong, tenthuoc, hoatchat, dvt, sluong, cachdung, ngaydung, moilan, dvdung, sang, trua, chieu, toi, gchu, trongdm, thoadk, slton, isDaxuat, idctthuoc, idthuoc };
                dtgvThuoc.Rows.Add(row);
                dtgvThuoc.AutoResizeColumns();
                dtgvThuoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            }
            catch
            {
                MessageBox.Show("Chưa chọn thuốc");
            }
            int colNumber = 2;
            for (int i = 0; i < dtgvThuoc.Rows.Count - 1; i++)
            {
                if (dtgvThuoc.Rows[i].IsNewRow) continue;
                string tmp = dtgvThuoc.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dtgvThuoc.Rows.Count - 1; j > i; j--)
                {
                    if (dtgvThuoc.Rows[j].IsNewRow) continue;
                    if (tmp == dtgvThuoc.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dtgvThuoc.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion

        }

        private void btnThemThuoc_Click(object sender, EventArgs e)
        {
            
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void dtgvThuoc_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            #region Load cách dùng và đơn vị dùng qua Event
            string sql=@"select  idcachdung,tencachdung from Thuoc_CachDung where tencachdung is not null order by idcachdung";
            DataTable dtCachdung = DataAcess.Connect.GetTable(sql);
            DataGridViewComboBoxCell box = dtgvThuoc.Rows[e.RowIndex].Cells["cachdung"] as DataGridViewComboBoxCell;
            box.ValueMember = "idcachdung";
            box.DisplayMember = "tencachdung";
            box.DataSource = dtCachdung;
            string sql1=@"select id,TenDVT from Thuoc_DonViTinh order by id ";
            DataTable dtDonvidung = DataAcess.Connect.GetTable(sql1);
            DataGridViewComboBoxCell box1 = dtgvThuoc.Rows[e.RowIndex].Cells["dvdung"] as DataGridViewComboBoxCell;
            box1.DataSource = dtDonvidung;
            box1.ValueMember = "id";
            box1.DisplayMember = "TenDVT";
            string sql2=@"select id,TenDVT from Thuoc_DonViTinh order by id ";
            DataTable dtDonvitinh = DataAcess.Connect.GetTable(sql2);
            DataGridViewComboBoxCell box2 = dtgvThuoc.Rows[e.RowIndex].Cells["dvt"] as DataGridViewComboBoxCell;
            box2.DataSource = dtDonvitinh;
            box2.ValueMember = "id";
            box2.DisplayMember = "TenDVT";
            #endregion
        }

        private void dtgvThuoc_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //if (e.Exception.Message == "DataGridViewComboBoxCell value is not valid.")
            //{
            //    object value = dtgvThuoc.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            //    if (!((DataGridViewComboBoxColumn)dtgvThuoc.Columns[e.ColumnIndex]).Items.Contains(value))
            //    {
            //        ((DataGridViewComboBoxColumn)dtgvThuoc.Columns[e.ColumnIndex]).Items.Add(value);
            //        e.ThrowException = false;
            //    }
            //}
        }



        private void simpleButton5_Click(object sender, EventArgs e)
        {
            string huongdieutri = null;
            string isHaveCLS = null;
            string ISHAVETHUOC = null;
            string ISHAVETHUOCBH = null;
            string mota_CDSB = "";
            string MaICD_CDSB = "";
            if (sluKhoa.EditValue != null)
            {
                if (sluKhoa.Text == "Khám bệnh")
                {
                    huongdieutri = "1";
                    if (dtgvCLS.Rows.Count > 1)
                    {
                        isHaveCLS = "1";
                    }
                    else isHaveCLS = "0";
                }

                else
                if (sluKhoa.Text != "Khám bệnh")
                {
                    huongdieutri = "8";
                    if (dtgvCLS.Rows.Count > 1)
                    {
                        isHaveCLS = "1";
                    }
                    else isHaveCLS = "0";
                }
            }
            else if (sluKhoa.EditValue == null)
            {

                if (dtgvThuoc.Rows.Count > 1)
                {
                    huongdieutri = "2";
                    ISHAVETHUOC = "1";
                    ISHAVETHUOCBH = "1";
                    if (dtgvCLS.Rows.Count > 1)
                    {
                        isHaveCLS = "1";
                    }
                    else isHaveCLS = "0";
                }
                else
                     if (dtgvCLS.Rows.Count > 1 && dtgvThuoc.Rows.Count == 1)
                {
                    huongdieutri = "6";
                    isHaveCLS = "1";
                }
            }
            for (int i = 0; i < dtgvCDSB.Rows.Count; i++)
            {
                mota_CDSB += dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn6"].Value + ";";
                MaICD_CDSB += dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value + ";";
            }
            string cdsb = mota_CDSB + "(" + MaICD_CDSB + ")";
            DataTable dtLuuKB2 = DataAcess.Connect.GetTable(this.dt_LoadBN());
            if (simpleButton5.Text == "Lưu")
            {
                #region Chuyển phòng không thu phí, nhập viện,ra toa

                string luuKB = @"insert into khambenh (ngaykham,idbenhnhan,iddangkykham,idbacsi,chandoanbandau,ketluan,huongdieutri,phongkhamchuyenden,idphongkhambenh,idphongchuyenden,
                                     IdChiTietDangKyKham,isNoiTru,IdPhong,DichVuKCBID,idchuyenpk,IdKhoa,idkhoachuyen,IsChuyenPhongCoPhi,isxuatvien,PhongID,songayratoa,tgxuatvien,IsHaveCLS,IsChoVeKT,IsChuyenVien,IsKhongKham,IsBSMoiKham,ishavethuocbh,MoTaCD_edit,IsTieuPhauRoiVe,ishavethuoc,Sysdate)
                                     values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "','" + dtLuuKB2.Rows[0]["iddangkykham"].ToString() + "','" + sluBacsi.EditValue.ToString() + @"'
                                     ,N'" + cdsb + "','" + sluCDXD.EditValue.ToString() +"','" + huongdieutri + "','" + sluKhoa.EditValue + "',1,'" + sluPK.EditValue + "','" + dtLuuKB2.Rows[0]["idchitietdangkykham"].ToString() + "',0,'" + Truyendulieu.PhongKhamID + @"'
                                     ,'" + dtLuuKB2.Rows[0]["idbanggiadichvu"].ToString() + "','" + sluPK.EditValue + "', 1,'" + sluKhoa.EditValue + "', 0,'" + chkRavien.Checked + "','" + Truyendulieu.PhongKhamID + "','" + txtSongayratoa.Text + "','" + txtRavien.Text + "','" + isHaveCLS + "',0,0,0,0,'" + ISHAVETHUOCBH + "','"+txtCDXD.Text+"',0,'" + ISHAVETHUOC + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                // DataTable luukb2 = DataAcess.Connect.GetTable(luuKB);
                bool okk = DataAcess.Connect.ExecSQL(luuKB);
                if (okk)
                {
                    string updateCT = "update chitietdangkykham set dakham=1 where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'";
                    DataTable LuuCT = DataAcess.Connect.GetTable(updateCT);
                    for (int x = 0; x < dtgvCDSB.Rows.Count - 1; x++)
                    {
                        string insertCDSB = "insert into chandoansobo (id,idkhambenh,idicd,maicd,MoTaCD_edit) values ((select max(id) from chandoansobo)+1,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn4"].Value.ToString() + "','" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn5"].Value.ToString() + "',N'" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn6"].Value.ToString() + "')";
                        DataTable luuCDSB = DataAcess.Connect.GetTable(insertCDSB);
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                    {
                        string insertCDPH = @"insert into chandoanphoihop (idkhambenh,idicd,maicd,MoTaCD_edit) values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dataGridView1.Rows[i].Cells["firstcolum"].Value.ToString() + @"',
                                           '" + dataGridView1.Rows[i].Cells["secondcolum"].Value.ToString() + "',N'" + dataGridView1.Rows[i].Cells["thirdcolum"].Value.ToString() + "')";
                        DataTable luuCDPH = DataAcess.Connect.GetTable(insertCDPH);
                    }
                    string insertSH = @"insert into sinhhieu (idbenhnhan,ngaydo,mach,nhietdo,huyetap1,huyetap2,nhiptho,chieucao,cannang,BMI,Iddangkykham,idchitietdangkykham,idkhoasinhhieu,IdKhamBenh) values ('"+dtLuuKB2.Rows[0]["idbenhnhan"].ToString()+@"',
                                                              '"+DateTime.Now.ToString("yyyy-MM-dd hh:mm")+"','"+txtMach.Text+"','"+txtNhietDo.Text+"','"+txtHuyetAp.Text+"','"+txtHuyetAp2.Text+@"',
                                                                '"+txtNhipTho.Text+"','"+txtChieuCao.Text+"','"+txtCanNang.Text+"','"+txtBMI.Text+"','"+dtLuuKB2.Rows[0]["iddangkykham"].ToString()+"','"+ dtLuuKB2.Rows[0]["idchitietdangkykham"].ToString() + "',1,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'))";
                    DataTable luuSinhhieu = DataAcess.Connect.GetTable(insertSH);
                }

                #region Có nhập cls
                if (dtgvCLS.Rows.Count > 1)
                {
                    string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                    string insertDKCLS = "insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                    bool ok = DataAcess.Connect.ExecSQL(insertDKCLS);
                    if (ok)
                    {
                        for (int t = 0; t < dtgvCLS.Rows.Count - 1; t++)
                        {
                            string luuCLS = @"insert into khambenhcanlamsan (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                                        values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dtgvCLS.Rows[t].Cells["idbanggia"].Value.ToString() + @"'
                                                                        ,'" + sluBacsi.EditValue.ToString() + "',0,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                                        ,'" + maphieucls + "','" + dtgvCLS.Rows[t].Cells["soluong"].Value.ToString() + "','" + dtgvCLS.Rows[t].Cells["giabh"].Value.ToString() + "','" + dtgvCLS.Rows[t].Cells["ghichu"].Value.ToString() + @"'
                                                                        ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + "',0,'" + dtgvCLS.Rows[t].Cells["giabh"].Value.ToString() + "','" + dtgvCLS.Rows[t].Cells["giadichvu"].Value.ToString() + "','" + dtgvCLS.Rows[t].Cells["issudungbh"].Value.ToString() + @"'
                                                                        ,0,'" + dtgvCLS.Rows[t].Cells["giabh"].Value.ToString() + "','" + dtgvCLS.Rows[t].Cells["giadichvu"].Value.ToString() + "',(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                                        ,'" + dtgvCLS.Rows[t].Cells["idnhomin"].Value.ToString() + "','" + dtgvCLS.Rows[t].Cells["IsBHYT_Save"].Value.ToString() + "','" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + "')";
                            DataTable Luu = DataAcess.Connect.GetTable(luuCLS);
                        }

                    }
                    MessageBox.Show("Thành công");
                }
                #endregion

                #region Có nhập hẹn cls
                if (dtgvCLSHen.Rows.Count > 1)
                {
                    string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                    string insertDKCLS = "insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                    bool ok = DataAcess.Connect.ExecSQL(insertDKCLS);
                    if (ok)
                    {
                        for (int t = 0; t < dtgvCLS.Rows.Count - 1; t++)
                        {
                            string luuCLS = @"insert into khambenhcanlamsanhen (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                                        values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dtgvCLSHen.Rows[t].Cells["idCLS_clshen"].Value.ToString() + @"'
                                                                        ,'" + sluBacsi.EditValue.ToString() + "',0,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                                        ,'" + maphieucls + "','" + dtgvCLSHen.Rows[t].Cells["soluong_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[t].Cells["bhtra_clshen"].Value.ToString() + "','" + dtgvCLS.Rows[t].Cells["ghichu_clshen"].Value.ToString() + @"'
                                                                        ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + "',0,'" + dtgvCLSHen.Rows[t].Cells["bhtra_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[t].Cells["giadichvu_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[t].Cells["isBH_clshen"].Value.ToString() + @"'
                                                                        ,0,'" + dtgvCLSHen.Rows[t].Cells["bhtra_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[t].Cells["giadichvu_clshen"].Value.ToString() + "',(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                                        ,'" + dtgvCLSHen.Rows[t].Cells["idnhomin_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[t].Cells["isSDBH_clshen"].Value.ToString() + "','" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + "')";
                            DataTable Luu = DataAcess.Connect.GetTable(luuCLS);
                        }

                    }
                    MessageBox.Show("Thành công");
                }
                #endregion

                #region có nhập toa thuốc
                if (dtgvThuoc.Rows.Count > 1)
                {
                    string insertBNTT = "insert into benhnhantoathuoc (idkhambenh,idbacsi,idbenhnhan,ngayratoa) values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + sluBacsi.EditValue.ToString() + @"'
                                                   ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "','" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "')";
                    bool ook = DataAcess.Connect.ExecSQL(insertBNTT);
                    if (ook)
                    {
                        for (int i = 0; i < dtgvThuoc.Rows.Count - 1; i++)
                        {
                            string insertCTBNTT = @"insert into chitietbenhnhantoathuoc (idbenhnhantoathuoc,idthuoc,soluongke,ngayuong,moilanuong,ghichu,idkhambenh,idkho,doituongthuocID,idcachdung,iddvdung,iddvt,ischieu,issang,istoi,istrua,ngayratoa,isbhyt_save,slton,isdaxuat,slxuat)
                                                                    values ((select max(idbenhnhantoathuoc) from benhnhantoathuoc where idbenhnhan='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                                     ,'" + dtgvThuoc.Rows[i].Cells["idthuoc"].Value.ToString() + "','" + dtgvThuoc.Rows[i].Cells["sluong"].Value.ToString() + "','" + dtgvThuoc.Rows[i].Cells["ngaydung"].Value.ToString() + "','" + dtgvThuoc.Rows[i].Cells["moilan"].Value.ToString() + "',N'" + dtgvThuoc.Rows[i].Cells["gchu"].Value.ToString() + @"'
                                                                    ,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + sluKho.EditValue + "','" + sluDoituong.EditValue + "',(select top 1 idcachdung from Thuoc_CachDung where tencachdung like N'%" + dtgvThuoc.Rows[i].Cells["cachdung"].Value + "%'),(select top 1 Id from Thuoc_DonViTinh where TenDVT like N'%" + dtgvThuoc.Rows[i].Cells["dvdung"].Value + @"%') 
                                                                    ,(select top 1 Id from Thuoc_DonViTinh where TenDVT like N'%" + dtgvThuoc.Rows[i].Cells["dvt"].Value + "%'),'" + dtgvThuoc.Rows[i].Cells["chieu"].Value + "','" + dtgvThuoc.Rows[i].Cells["sang"].Value + "','" + dtgvThuoc.Rows[i].Cells["toi"].Value + "','" + dtgvThuoc.Rows[i].Cells["trua"].Value + "','" + DateTime.Now.ToString("yyyy-MM-dd 00:00") + @"'
                                                                     ,'" + dtgvThuoc.Rows[i].Cells["thoadk"].Value + "','" + dtgvThuoc.Rows[i].Cells["slton"].Value.ToString() + "',0,'" + dtgvThuoc.Rows[i].Cells["sluong"].Value.ToString() + "')";//+ dtgvThuoc.Rows[i].Cells["dvdung"].Value + 
                            DataTable luuToa = DataAcess.Connect.GetTable(insertCTBNTT);
                        }
                        MessageBox.Show("Đã lưu toa thành công");
                    }
                }
                #endregion
                MessageBox.Show("Thành công");
                #endregion
            }
            else
            if (simpleButton5.Text == "Sửa")
            {

                for (int i = 0; i < dtgvCDSB.Rows.Count - 1; i++)
                {
                    #region thêm chẩn đoán sơ bộ
                    if (dtgvCDSB.Rows[i].Cells["IDCDSB"].Value.ToString() == "" || dtgvCDSB.Rows[i].Cells["IDCDSB"].Value == null)
                    {
                        string insertCDSB = "insert into chandoansobo (id,idkhambenh,idicd,maicd,MoTaCD_edit) values ((select max(id) from chandoansobo)+1,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn4"].Value.ToString() + "','" + dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() + "',N'" + dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn6"].Value.ToString() + "')";
                        DataTable luuCDSB = DataAcess.Connect.GetTable(insertCDSB);
                    }
                    #endregion
                }
                for (int x = 0; x < dataGridView1.Rows.Count - 1; x++)
                {
                    #region thêm chẩn đoán phối hợp
                    if (dataGridView1.Rows[x].Cells["IDCDPH"].Value.ToString() == "" || dataGridView1.Rows[x].Cells["IDCDPH"].Value == null)
                    {
                        string insertCDPH = @"insert into chandoanphoihop (idkhambenh,idicd,maicd,MoTaCD_edit) values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dataGridView1.Rows[x].Cells["firstcolum"].Value.ToString() + @"',
                                           '" + dataGridView1.Rows[x].Cells["secondcolum"].Value.ToString() + "',N'" + dataGridView1.Rows[x].Cells["thirdcolum"].Value.ToString() + "')";
                        DataTable luuCDPH = DataAcess.Connect.GetTable(insertCDPH);
                    }
                    #endregion
                }
                for (int y = 0; y < dtgvCLS.Rows.Count - 1; y++)
                {
                    string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                    string insertDKCLS = "insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                    DataTable dkcls = DataAcess.Connect.GetTable(insertDKCLS);
                    #region update bảng khambenhcanlamsan
                    if (dtgvCLS.Rows[y].Cells["IdKBCLS"].Value.ToString() != "" || dtgvCLS.Rows[y].Cells["IdKBCLS"].Value != null)
                    {
                        string updateCLS = @"update khambenhcanlamsan set soluong='" + dtgvCLS.Rows[y].Cells["soluong"].Value + "',isbhyt_save='" + dtgvCLS.Rows[y].Cells["isbhyt_save"].Value + @"'
                                        ,ghichu=N'" + dtgvCLS.Rows[y].Cells["ghichu"].Value.ToString() + "' where idkhambenhcanlamsan='" + dtgvCLS.Rows[y].Cells["IdKBCLS"].Value + "'";
                        DataTable editCLS = DataAcess.Connect.GetTable(updateCLS);
                    }
                    if (dtgvCLS.Rows[y].Cells["IdKBCLS"].Value.ToString() == "" || dtgvCLS.Rows[y].Cells["IdKBCLS"].Value == null)
                    {
                        string luuCLS = @"insert into khambenhcanlamsan (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                                            values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dtgvCLS.Rows[y].Cells["idbanggia"].Value.ToString() + @"'
                                                                            ,'" + sluBacsi.EditValue.ToString() + "',0,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                                            ,'" + maphieucls + "','" + dtgvCLS.Rows[y].Cells["soluong"].Value.ToString() + "','" + dtgvCLS.Rows[y].Cells["giabh"].Value.ToString() + "','" + dtgvCLS.Rows[y].Cells["ghichu"].Value.ToString() + @"'
                                                                            ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + "',0,'" + dtgvCLS.Rows[y].Cells["giabh"].Value.ToString() + "','" + dtgvCLS.Rows[y].Cells["giadichvu"].Value.ToString() + "','" + dtgvCLS.Rows[y].Cells["issudungbh"].Value.ToString() + @"'
                                                                            ,0,'" + dtgvCLS.Rows[y].Cells["giabh"].Value.ToString() + "','" + dtgvCLS.Rows[y].Cells["giadichvu"].Value.ToString() + "',(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                                            ,'" + dtgvCLS.Rows[y].Cells["idnhomin"].Value.ToString() + "','" + dtgvCLS.Rows[y].Cells["IsBHYT_Save"].Value.ToString() + "','" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + "')";
                        DataTable Luu = DataAcess.Connect.GetTable(luuCLS);
                    }
                }

                #endregion
                #region update bảng khambenhcanlamsanhen
                for (int y = 0; y < dtgvCLSHen.Rows.Count - 1; y++)
                {
                    string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                    string insertDKCLS = "insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                    DataTable dkcls = DataAcess.Connect.GetTable(insertDKCLS);
                  
                    if (dtgvCLSHen.Rows[y].Cells["IdKBCLS_clshen"].Value.ToString() != "" || dtgvCLSHen.Rows[y].Cells["IdKBCLS_clshen"].Value != null)
                    {
                       
                        string updateCLS = @"update khambenhcanlamsanhen set soluong='" + dtgvCLSHen.Rows[y].Cells["soluong_clshen"].Value + "',isbhyt_save='" + dtgvCLSHen.Rows[y].Cells["isSDBH_clshen"].Value + @"'
                                        ,ghichu=N'" + dtgvCLSHen.Rows[y].Cells["ghichu_clshen"].Value.ToString() + "' where idkhambenhcanlamsanhen='" + dtgvCLSHen.Rows[y].Cells["IdKBCLS_clshen"].Value + "'";
                        DataTable editCLS = DataAcess.Connect.GetTable(updateCLS);
                    }
                    if (dtgvCLSHen.Rows[y].Cells["IdKBCLS_clshen"].Value.ToString() == "" || dtgvCLSHen.Rows[y].Cells["IdKBCLS_clshen"].Value == null)
                    {
                        string luuCLS = @"insert into khambenhcanlamsanhen (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                                        values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dtgvCLSHen.Rows[y].Cells["idCLS_clshen"].Value.ToString() + @"'
                                                                        ,'" + sluBacsi.EditValue.ToString() + "',0,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                                        ,'" + maphieucls + "','" + dtgvCLSHen.Rows[y].Cells["soluong_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[y].Cells["bhtra_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[y].Cells["ghichu_clshen"].Value.ToString() + @"'
                                                                        ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + "',0,'" + dtgvCLSHen.Rows[y].Cells["bhtra_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[y].Cells["giadichvu_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[y].Cells["isBH_clshen"].Value.ToString() + @"'
                                                                        ,0,'" + dtgvCLSHen.Rows[y].Cells["bhtra_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[y].Cells["giadichvu_clshen"].Value.ToString() + "',(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                                        ,'" + dtgvCLSHen.Rows[y].Cells["idnhomin_clshen"].Value.ToString() + "','" + dtgvCLSHen.Rows[y].Cells["isSDBH_clshen"].Value.ToString() + "','" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + "')";
                        DataTable Luu = DataAcess.Connect.GetTable(luuCLS);
                    }
                }
                #endregion



            }
            for (int z = 0; z < dtgvThuoc.Rows.Count - 1; z++)
            {
                #region update bảng chitietbenhnhantoathuoc
                if (dtgvThuoc.Rows[z].Cells["idctthuoc"].Value.ToString() != "" || dtgvThuoc.Rows[z].Cells["idctthuoc"].Value != null)
                {
                    string updateThuoc = @"update chitietbenhnhantoathuoc set soluongke='" + dtgvThuoc.Rows[z].Cells["sluong"].Value + "',ngayuong='" + dtgvThuoc.Rows[z].Cells["ngaydung"].Value + ",moilanuong='" + dtgvThuoc.Rows[z].Cells["moilan"].Value + @"'
                                                                                            ,ghichu='" + dtgvThuoc.Rows[z].Cells["gchu"].Value + "',idcachdung='" + dtgvThuoc.Rows[z].Cells["cachdung"].Value + "',iddvdung='" + dtgvThuoc.Rows[z].Cells["dvdung"].Value + @"'
                                                                                            ,iddvt='" + dtgvThuoc.Rows[z].Cells["dvt"].Value + "',ischieu='" + dtgvThuoc.Rows[z].Cells["chieu"].Value + "',issang='" + dtgvThuoc.Rows[z].Cells["sang"].Value + @"'
                                                                                            ,istoi='" + dtgvThuoc.Rows[z].Cells["toi"].Value + "',istrua='" + dtgvThuoc.Rows[z].Cells["trua"].Value + "',IsBHYT_Save='" + dtgvThuoc.Rows[z].Cells["thoadk"].Value + "' where idkhambenh='" + Truyendulieu.idkhambenh + "'";
                    DataTable editThuoc = DataAcess.Connect.GetTable(updateThuoc);
                }
                #endregion

                #region nếu nhập thêm thuốc thì insert
                if (dtgvThuoc.Rows[z].Cells["idctthuoc"].Value.ToString() == "" || dtgvThuoc.Rows[z].Cells["idctthuoc"].Value == null)
                {
                    string insertCTBNTT = @"insert into chitietbenhnhantoathuoc (idbenhnhantoathuoc,idthuoc,soluongke,ngayuong,moilanuong,ghichu,idkhambenh,idkho,doituongthuocID,idcachdung,iddvdung,iddvt,ischieu,issang,istoi,istrua,ngayratoa,isbhyt_save,slton,isdaxuat,slxuat)
                                                                    values ((select max(idbenhnhantoathuoc) from benhnhantoathuoc where idbenhnhan='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                                     ,'" + dtgvThuoc.Rows[z].Cells["idthuoc"].Value.ToString() + "','" + dtgvThuoc.Rows[z].Cells["sluong"].Value.ToString() + "','" + dtgvThuoc.Rows[z].Cells["ngaydung"].Value.ToString() + "','" + dtgvThuoc.Rows[z].Cells["moilan"].Value.ToString() + "',N'" + dtgvThuoc.Rows[z].Cells["gchu"].Value.ToString() + @"'
                                                                    ,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + sluKho.EditValue + "','" + sluDoituong.EditValue + "',(select top 1 idcachdung from Thuoc_CachDung where tencachdung like N'%" + dtgvThuoc.Rows[z].Cells["cachdung"].Value + "%'),(select top 1 Id from Thuoc_DonViTinh where TenDVT like N'%" + dtgvThuoc.Rows[z].Cells["dvdung"].Value + @"%') 
                                                                    ,(select top 1 Id from Thuoc_DonViTinh where TenDVT like N'%" + dtgvThuoc.Rows[z].Cells["dvt"].Value + "%'),'" + dtgvThuoc.Rows[z].Cells["chieu"].Value + "','" + dtgvThuoc.Rows[z].Cells["sang"].Value + "','" + dtgvThuoc.Rows[z].Cells["toi"].Value + "','" + dtgvThuoc.Rows[z].Cells["trua"].Value + "','" + DateTime.Now.ToString("yyyy-MM-dd 00:00") + @"'
                                                                     ,'" + dtgvThuoc.Rows[z].Cells["thoadk"].Value + "','" + dtgvThuoc.Rows[z].Cells["slton"].Value.ToString() + "',0,'" + dtgvThuoc.Rows[z].Cells["sluong"].Value.ToString() + "')";//+ dtgvThuoc.Rows[i].Cells["dvdung"].Value + 
                    DataTable luuToa = DataAcess.Connect.GetTable(insertCTBNTT);
                }
                #endregion
            }

            #region Update lại table KhamBenh
            string updateKB = @"update khambenh set idbacsi='"+sluBacsi.EditValue.ToString()+"',chandoanbandau=N'"+cdsb+"',ketluan='"+sluCDXD.EditValue.ToString()+"',huongdieutri='"+huongdieutri+"',phongkhamchuyenden='"+sluKhoa.EditValue.ToString()+"',idphongchuyenden='"+sluPK.EditValue.ToString()+@"'
                                                    ,isNoiTru='"+chkNoitru.Checked+"',idphong='"+Truyendulieu.PhongKhamID+"',idchuyenpk='"+sluPK.EditValue.ToString()+"',idkhoachuyen='"+sluKhoa.EditValue.ToString()+"',IsChuyenPhongCoPhi='"+ chkThuPhi.Checked+ "',isxuatvien='"+chkRavien.Checked+"',PhongID='"+Truyendulieu.PhongKhamID+"',songayratoa='"+txtSongayratoa.Text+@"'
                                                    ,tgxuatvien='"+txtRavien.Text+"',IsHaveCLS='"+isHaveCLS+"',IsChoVeKT='"+chkChovekt.Checked+"',IsChuyenVien='"+chkChuyenVien.Checked+"',IsKhongKham='"+chkKhongKham.Checked+@"'
                                                    ,idbacsi2='"+ gluBacSi2.EditValue.ToString()+ "',IsBSMoiKham='"+chkMoiKham.Checked+"',ishavethuocbh='"+ISHAVETHUOCBH+"',MoTaCD_edit='"+txtCDXD.Text+"',IsTieuPhauRoiVe='"+ chkTieuPhau.Checked+ "',ishavethuoc='"+ISHAVETHUOC+"' where idkhambenh='"+Truyendulieu.idkhambenh+"' ";
            DataTable LuuKB = DataAcess.Connect.GetTable(updateKB);
            #endregion
            MessageBox.Show("Thành công");
        }
        private void simpleButton6_Click(object sender, EventArgs e)
        {

        }

        private void chkRavien_CheckedChanged(object sender, EventArgs e)
        {
            txtRavien.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm");

        }

        private void txtSongayratoa_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void sluKhoa_EditValueChanged(object sender, EventArgs e)
        {
            Load_PhongKham();

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void sluPK_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void searchLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void dtgvThuoc_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dtgvThuoc.CurrentCell.ColumnIndex == 6)
            {
                if (e.Control is DataGridViewComboBoxEditingControl)
                {
                    //check box column 
                    ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                    ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                    ((ComboBox)e.Control).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                }
            }
            if (dtgvThuoc.CurrentCell.ColumnIndex == 9)
            {
                if (e.Control is DataGridViewComboBoxEditingControl)
                {
                    //check box column 
                    ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                    ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                    ((ComboBox)e.Control).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                }
            }
            if (dtgvThuoc.CurrentCell.ColumnIndex == 4)
            {
                if (e.Control is DataGridViewComboBoxEditingControl)
                {
                    //check box column 
                    ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                    ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                    ((ComboBox)e.Control).AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                }
            }
            return;
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int r = dataGridView1.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dataGridView1.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaCDPH")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dataGridView1.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Chẩn đoán phối hợp", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dataGridView1.Rows[r].Cells["IDCDPH"].Value.ToString() != "" || dataGridView1.Rows[r].Cells["IDCDPH"].Value.ToString() != null)
                                    {
                                        string sql = "delete chandoanphoihop where id='" + dataGridView1.Rows[r].Cells["IDCDPH"].Value.ToString() + "'";
                                        DataTable xoacdph = DataAcess.Connect.GetTable(sql);
                                        dataGridView1.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        dataGridView1.Rows.RemoveAt(oneCell.RowIndex);
                                    }

                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bạn chọn ô trống rồi");
                    }
                }
            }

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 4) // Also you can check for specific row by e.RowIndex
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All
                    & ~(DataGridViewPaintParts.ContentForeground));
                var r = e.CellBounds;
                r.Inflate(-4, -4);
                e.Graphics.FillRectangle(Brushes.PaleVioletRed, r);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Handled = true;
            }
        }

        private void dtgvCDSB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dtgvCDSB.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCDSB.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaCDSB")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvCDSB.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Chẩn đoán sơ bộ", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != "" || dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != null)
                                    {
                                        string sql = "delete chandoansobo where id='" + dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() + "'";
                                        DataTable xoacdsb = DataAcess.Connect.GetTable(sql);
                                        dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
                                    }

                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bạn chọn ô trống rồi");
                    }
                }
            }


        }

        private void sluCDXD_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void dtgvCDSB_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 4) // Also you can check for specific row by e.RowIndex
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All
                    & ~(DataGridViewPaintParts.ContentForeground));
                var r = e.CellBounds;
                r.Inflate(-4, -4);
                e.Graphics.FillRectangle(Brushes.PaleVioletRed, r);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Handled = true;
            }
        }

        private void dtgvCLS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dtgvCLS.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCLS.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaCLS")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvCLS.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Cận lâm sàng", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dtgvCLS.Rows[r].Cells["isdathu"].Value.ToString() != "1")
                                    {
                                        string sql = "delete khambenhcanlamsan where idkhambenhcanlamsan='" + dtgvCLS.Rows[r].Cells["IdKBCLS"].Value.ToString() + "'";
                                        DataTable xoaCLS = DataAcess.Connect.GetTable(sql);
                                        dtgvCLS.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        // dtgvCLS.Rows.RemoveAt(oneCell.RowIndex);
                                        MessageBox.Show("Cận lâm sàng này đã thu, hủy phiếu thu mới xóa được!");
                                    }

                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bạn chọn ô trống rồi");
                    }
                }
            }
        }

        private void dtgvCLS_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 12) // Also you can check for specific row by e.RowIndex
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All
                    & ~(DataGridViewPaintParts.ContentForeground));
                var r = e.CellBounds;
                r.Inflate(-4, -4);
                e.Graphics.FillRectangle(Brushes.PaleVioletRed, r);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Handled = true;
            }
        }

        private void dtgvThuoc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dtgvThuoc.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvThuoc.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaThuoc")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvThuoc.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa thuốc", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dtgvThuoc.Rows[r].Cells["isDaxuat"].Value.ToString() == "True")
                                    {
                                        MessageBox.Show("Thuốc này đã xuất, hủy xuất thuốc mới xóa được!");
                                    }
                                    else
                                    {
                                        string sql = "delete chitietbenhnhantoathuoc where idchitietbenhnhantoathuoc='" + dtgvThuoc.Rows[r].Cells["idctthuoc"].Value.ToString() + "'";
                                        DataTable xoaThuoc = DataAcess.Connect.GetTable(sql);
                                        dtgvThuoc.Rows.RemoveAt(oneCell.RowIndex);
                                    }

                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bạn chọn ô trống rồi");
                    }
                }
            }
        }

        private void dtgvThuoc_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 21) // Also you can check for specific row by e.RowIndex
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All
                    & ~(DataGridViewPaintParts.ContentForeground));
                var r = e.CellBounds;
                r.Inflate(-4, -4);
                e.Graphics.FillRectangle(Brushes.PaleVioletRed, r);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Handled = true;
            }
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            frmRptCLS frmp = new frmRptCLS();
            frmp.Show();
            
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {  
            frmToaThuocBH frmpTT = new frmToaThuocBH();
            frmpTT.Show();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            frmToaThuocDV frmpTTDV = new frmToaThuocDV();
            frmpTTDV.Show();
        }

        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            if(loaikhamID=="1")
            {
                hs_tinhtien.TinhTien(null,Truyendulieu.idkhambenh,true);

            }
            else
            {
                hs_tinhtien.TinhTienDV(null, Truyendulieu.idkhambenh, true);
            }
        }

        private void sluDoituong_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void sluKho_EditValueChanged(object sender, EventArgs e)
        {

            Load_Thuoc();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            Truyendulieu.idphieutt = idphieutt;
            frmBV01 frm01 = new frmBV01();
            frm01.Show();
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            

        }

        private void chkNoitru_CheckedChanged(object sender, EventArgs e)
        {
            chkNgoaitru.Checked = false;
        }

        private void chkNgoaitru_CheckedChanged(object sender, EventArgs e)
        {
            chkNoitru.Checked = false;
        }

        private void btnToaCu_Click(object sender, EventArgs e)
        {
            
           
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            CLSTuDK ttcu = new CLSTuDK();
            ttcu.Show();
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (dtgvThuoc.Rows.Count > 1)
                {
                    MessageBox.Show("Đã có toa thuốc rồi!");
                }
                else
                {
                    Load_ToaThuoc(Truyendulieu.idkhambenh_old);
                }
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Truyendulieu.idkhambenh_old = e.Node.Tag.ToString();
        }

        private void btnThemThuoc_Click_1(object sender, EventArgs e)
        {
            Load_thuoc_theoID();
        }

        private void tabPage3_Click_1(object sender, EventArgs e)
        {

        }

        private void groupControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThem_Click_2(object sender, EventArgs e)
        {
            #region Thêm chẩn đoán xác định
            try
            {
                conn = new SqlConnection(strConnectionString);
                conn.Open();
                // Vận chuyển dữ liệu lên DataTable dtKhachHang
                SqlDataAdapter daICD = null;
                DataTable dtICD = null;
                daICD = new SqlDataAdapter(@"select idicd,MaICD,MoTa from ChanDoanICD where IDICD='" + gluChanDoan.EditValue.ToString() + "'", conn);
                dtICD = new DataTable();
                dtICD.Clear();
                daICD.Fill(dtICD);
                //dataGridView1.DataSource = dtbenhnhan;
                //string secondColum = gluChanDoan.Text;

                string firstColum = dtICD.Rows[0]["idicd"].ToString();
                string secondColum = dtICD.Rows[0]["maicd"].ToString();
                string thirdcolum = dtICD.Rows[0]["MoTa"].ToString();
                string IDCDPH = "";
                string[] row = { firstColum, secondColum, thirdcolum, IDCDPH };
                //for (int i = 0; i < dtICD.Rows.Count; i++)
                //{
                //    dtICD.Rows[i]["STT"] = i + 1;

                //}
                dataGridView1.Rows.Add(row);
                dataGridView1.AutoResizeColumns();
                dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            }
            catch
            {
                MessageBox.Show("Chưa chọn chẩn đoán!");
            }

            int colNumber = 0;
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].IsNewRow) continue;
                string tmp = dataGridView1.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dataGridView1.Rows.Count - 1; j > i; j--)
                {
                    if (dataGridView1.Rows[j].IsNewRow) continue;
                    if (tmp == dataGridView1.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dataGridView1.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion
        }

        private void sluCDXD_EditValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                string maICD1 = sluCDXD.EditValue.ToString();
                string sql = "SELECT mota FROM dbo.ChanDoanICD where idicd= '" + maICD1 + "'";
                DataTable layMota = DataAcess.Connect.GetTable(sql);
                txtCDXD.Text = layMota.Rows[0]["mota"].ToString();
            }
            catch
            {
                return;
            }
        }

        private void btnThemCDSB_Click_1(object sender, EventArgs e)
        {
            #region Thêm chẩn đoán sơ bộ
            try
            {
                conn = new SqlConnection(strConnectionString);
                conn.Open();
                // Vận chuyển dữ liệu lên DataTable dtKhachHang
                SqlDataAdapter daICD_CDSB = null;
                DataTable dtICD_CDSB = null;
                daICD_CDSB = new SqlDataAdapter(@"select idicd,MaICD,MoTa from ChanDoanICD where IDICD='" + gluCDSobo.EditValue.ToString() + "'", conn);
                dtICD_CDSB = new DataTable();
                dtICD_CDSB.Clear();
                daICD_CDSB.Fill(dtICD_CDSB);
                //dataGridView1.DataSource = dtbenhnhan;
                //string secondColum = gluChanDoan.Text;
                string dataGridViewTextBoxColumn4 = dtICD_CDSB.Rows[0]["idicd"].ToString();
                string dataGridViewTextBoxColumn5 = dtICD_CDSB.Rows[0]["maicd"].ToString();
                string dataGridViewTextBoxColumn6 = dtICD_CDSB.Rows[0]["MoTa"].ToString();
                string idcdsb = "";
                string[] row = { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, idcdsb };
                //for (int i = 0; i < dtICD.Rows.Count; i++)
                //{
                //    dtICD.Rows[i]["STT"] = i + 1;

                //}
                dtgvCDSB.Rows.Add(row);
                dtgvCDSB.AutoResizeColumns();
                dtgvCDSB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            }
            catch
            {
                MessageBox.Show("Chưa chọn chẩn đoán!");
            }

            int colNumber = 0;
            for (int i = 0; i < dtgvCDSB.Rows.Count - 1; i++)
            {
                if (dtgvCDSB.Rows[i].IsNewRow) continue;
                string tmp = dtgvCDSB.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dtgvCDSB.Rows.Count - 1; j > i; j--)
                {
                    if (dtgvCDSB.Rows[j].IsNewRow) continue;
                    if (tmp == dtgvCDSB.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dtgvCDSB.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion
        }

        private void txtSongayratoa_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                // dtpkTaikham.Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                if (txtSongayratoa.Text == "" || txtSongayratoa.Text == "0" || txtSongayratoa.Text == null)
                {
                    this.dtpkTaikham.Value = DateTime.Now;
                    //MessageBox.Show("Nhập số ngày ra toa > 0");
                }
                else
                {
                    this.dtpkTaikham.Value = dtpkTaikham.Value.AddDays(double.Parse(txtSongayratoa.Text));
                }
            }
            catch
            {
                MessageBox.Show("Vui lòng nhập số tự nhiên");
            }
        }

        private void btnToaCu_Click_1(object sender, EventArgs e)
        {
            string sql = @"select B.IDKHAMBENH
                ,a.ngayratoa,
		        a.idbenhnhantoathuoc
                 ,TENDICHVU=DBO.HS_TENPHONG(B.PHONGID)
                 , c.mota
                  ,d.tenbacsi 
                from benhnhantoathuoc a 
                inner join khambenh b on a.idkhambenh=b.idkhambenh
                 left join chandoanicd c on c.idicd=b.ketluan
                 left join bacsi d on B.idbacsi=d.idbacsi
                 WHERE B.IDBENHNHAN='" + Truyendulieu.idbenhnhan + @"'
                                      AND B.IDPHONGKHAMBENH=1
                ORDER BY B.IDKHAMBENH  DESC ";
            DataTable tt = DataAcess.Connect.GetTable(sql);
            for (int i = 0; i < tt.Rows.Count; i++)
            {
                TreeNode Node = new TreeNode("Ngày khám: " + DateTime.Parse(tt.Rows[i]["ngayratoa"].ToString()).ToString("dd/MM/yyyy") + "---" + tt.Rows[i]["TENDICHVU"].ToString() + "---" + tt.Rows[i]["tenbacsi"].ToString());
                Node.Tag = tt.Rows[i]["idkhambenh"].ToString();
                Node.ForeColor = Color.Blue;
                treeView1.Nodes.Add(Node);
                string sql2 = @"select 
                        TENTHUOC
                        ,TENDVT
                        ,SOLUONGKE,
                        A0.IDCHITIETBENHNHANTOATHUOC
                        ,B.IDKHAMBENH
                        ,ngayratoa=convert(nvarchar(20),B.NGAYKHAM,103)
                        ,TENDICHVU=DBO.HS_TENPHONG(B.PHONGID)
                        , c.mota
                        ,d.tenbacsi 
                        ,ISCHON=0
                        from 
                         CHITIETBENHNHANTOATHUOC A0
                        INNER join khambenh b on a0.idkhambenh=b.idkhambenh
                        left join chandoanicd c on c.idicd=b.ketluan
                        left join bacsi d on B.idbacsi=d.idbacsi
                        INNER join THUOC F ON A0.IDTHUOC=F.IDTHUOC
                        LEFT JOIN THUOC_DONVITINH G ON F.IDDVT=G.ID
                        WHERE A0.IDKHAMBENH =" + tt.Rows[i]["IDKHAMBENH"].ToString() + @"
                        ORDER BY B.NGAYKHAM, B.IDKHAMBENH  DESC ";
                DataTable tt2 = DataAcess.Connect.GetTable(sql2);
                for (int j = 0; j < tt2.Rows.Count; j++)
                {
                    TreeNode Node2 = new TreeNode(tt2.Rows[j]["TENTHUOC"].ToString() + "---SL: " + tt2.Rows[j]["SOLUONGKE"].ToString());
                    Node2.Tag = Node.Tag;
                    treeView1.Nodes[i].Nodes.Add(Node2);
                }
            }
        }

        private void simpleButton2_Click_2(object sender, EventArgs e)
        {
            string SoVaoVien = "";
            string IsNoitru = "0";
            if (chkNoitru.Checked == true)
            {
                IsNoitru = "1";

            }
            else
                if (chkNgoaitru.Checked == true)
            {
                IsNoitru = "0";

            }
            SoVaoVien = hs_tinhtien.GetSoVaoVien(Truyendulieu.idkhambenh, Truyendulieu.idchitietdangkykham, IsNoitru);
            txtSovaovien.Text = SoVaoVien;
            simpleButton2.Enabled = false;
        }

        private void dataGridView1_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {
            int r = dataGridView1.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dataGridView1.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaCDPH")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dataGridView1.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Chẩn đoán phối hợp", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dataGridView1.Rows[r].Cells["IDCDPH"].Value.ToString() != "" || dataGridView1.Rows[r].Cells["IDCDPH"].Value.ToString() != null)
                                    {
                                        string sql = "delete chandoanphoihop where id='" + dataGridView1.Rows[r].Cells["IDCDPH"].Value.ToString() + "'";
                                        DataTable xoacdph = DataAcess.Connect.GetTable(sql);
                                        dataGridView1.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        dataGridView1.Rows.RemoveAt(oneCell.RowIndex);
                                    }

                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bạn chọn ô trống rồi");
                    }
                }
            }
        }

        private void dtgvCDSB_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int r = dtgvCDSB.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCDSB.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaCDSB")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvCDSB.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Chẩn đoán sơ bộ", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != "" || dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != null)
                                    {
                                        string sql = "delete chandoansobo where id='" + dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() + "'";
                                        DataTable xoacdsb = DataAcess.Connect.GetTable(sql);
                                        dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
                                    }

                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bạn chọn ô trống rồi");
                    }
                }
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            LoadCLS_theoID();
        }

        private void dtgvCLS_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

            int r = dtgvCLS.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCLS.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaCLS")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvCLS.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Cận lâm sàng", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dtgvCLS.Rows[r].Cells["isdathu"].Value.ToString() != "1")
                                    {
                                        string sql = "delete khambenhcanlamsan where idkhambenhcanlamsan='" + dtgvCLS.Rows[r].Cells["IdKBCLS"].Value.ToString() + "'";
                                        DataTable xoaCLS = DataAcess.Connect.GetTable(sql);
                                        dtgvCLS.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        // dtgvCLS.Rows.RemoveAt(oneCell.RowIndex);
                                        MessageBox.Show("Cận lâm sàng này đã thu, hủy phiếu thu mới xóa được!");
                                    }

                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bạn chọn ô trống rồi");
                    }
                }
            }
        }

        private void btnNhomCLS_Click_2(object sender, EventArgs e)
        {
            NhomCLS_TheoID_Load();
        }

        private void sluKho_EditValueChanged_1(object sender, EventArgs e)
        {
            Load_Thuoc();
        }

        private void sluDoituong_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void grcKB_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCLSHen_Click(object sender, EventArgs e)
        {
            LoadCLS_theoID_hen();
        }

        private void btnNhomCLSHen_Click(object sender, EventArgs e)
        {
            NhomCLS_TheoID_Load_hen();
        }

        private void dtgvCLS_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 12) // Also you can check for specific row by e.RowIndex
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All
                    & ~(DataGridViewPaintParts.ContentForeground));
                var r = e.CellBounds;
                r.Inflate(-4, -4);
                e.Graphics.FillRectangle(Brushes.PaleVioletRed, r);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Handled = true;
            }
        }

        private void dtgvCLSHen_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 12) // Also you can check for specific row by e.RowIndex
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All
                    & ~(DataGridViewPaintParts.ContentForeground));
                var r = e.CellBounds;
                r.Inflate(-4, -4);
                e.Graphics.FillRectangle(Brushes.PaleVioletRed, r);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Handled = true;
            }
        }

        private void dtgvCLSHen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dtgvCLSHen.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCLSHen.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaHen")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvCLSHen.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Cận lâm sàng", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dtgvCLSHen.Rows[r].Cells["isdathu_clshen"].Value.ToString() != "1")
                                    {
                                        string sql = "delete khambenhcanlamsanhen where idkhambenhcanlamsanhen='" + dtgvCLSHen.Rows[r].Cells["IdKBCLS_clshen"].Value.ToString() + "'";
                                        DataTable xoaCLS = DataAcess.Connect.GetTable(sql);
                                        dtgvCLSHen.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        // dtgvCLS.Rows.RemoveAt(oneCell.RowIndex);
                                        MessageBox.Show("Cận lâm sàng này đã thu, hủy phiếu thu mới xóa được!");
                                    }

                                }
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bạn chọn ô trống rồi");
                    }
                }
            }
        }

        private void dataGridView1_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 4) // Also you can check for specific row by e.RowIndex
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All
                    & ~(DataGridViewPaintParts.ContentForeground));
                var r = e.CellBounds;
                r.Inflate(-4, -4);
                e.Graphics.FillRectangle(Brushes.PaleVioletRed, r);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Handled = true;
            }
        }

        private void dtgvCDSB_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (e.ColumnIndex == 4) // Also you can check for specific row by e.RowIndex
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All
                    & ~(DataGridViewPaintParts.ContentForeground));
                var r = e.CellBounds;
                r.Inflate(-4, -4);
                e.Graphics.FillRectangle(Brushes.PaleVioletRed, r);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);
                e.Handled = true;
            }
        }

        private void btnPhieuHenCLS_Click(object sender, EventArgs e)
        {
            PhieuHen frm01 = new PhieuHen();
            frm01.Show();
        }
    }
}

