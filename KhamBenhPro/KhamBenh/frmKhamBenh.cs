using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace KhamBenhPro.KhamBenh
{
    public partial class frmKhamBenh : Form
    {
        string loaikhamID = null;
        string idphieutt = null;
        string idchitietdangkykham = null;
        string user = "admin";
        public frmKhamBenh()
        {
            InitializeComponent();
        }

        private void frmKhamBenh_Load(object sender, EventArgs e)
        {
            Load_Item_CDSB();
            Load_CDSB_Gridview();
            Load_CLS();
            Load_CSL_gridview();
            Load_CSLhen_gridview();
            Load_Item_CDPH();
            Load_CDPH_Gridview();
            LoadCDXD();
            Load_Item_thuoc();
            Load_Item_Cachdung();
            Load_Item_DonViDung();
            Load_Toathuoc_Gridview();
            Load_BNchokham();
            LoadsluBacsi2();
            LoadsluBacsi();
            Load_Khoa();           
            KhoThuoc_load();
            Load_thuoc_doituong();
        }
        private string dt_Load_Bacsi()
        {
            string sql = "SELECT idbacsi,tenbacsi,mabacsi FROM dbo.bacsi WHERE mabacsi like '%CCHN%'";
            return sql;
        }

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
            sluKhoa.Properties.PopupFormSize = new Size(300, 200);
        }
        #endregion

        #region Load Phòng khám chuyển đến
        public void Load_PhongKham()
        {
            string sql = @"select p.id,p.maso+'-'+p.TenPhong as 'tenphong'
                                            from KB_Phong p
                                            inner join banggiadichvu bg on bg.idbanggiadichvu = p.DichVuKCB
                                            where bg.idphongkhambenh='" + sluKhoa.EditValue.ToString() + @"'
                                            and p.isPhongNoiTru = 0
                                            and p.IsActive = 1
                                            and p.Status=1
                                            and p.id<>'" + Truyendulieu.PhongKhamID + @"'
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

        #region Load kho thuốc
        public void KhoThuoc_load()
        {
           
            string sql = @"select idkho,tenkho from khothuoc where idkho in (72,5)";
            DataTable dtKhothuoc = DataAcess.Connect.GetTable(sql);
            sluKho.Properties.DataSource = dtKhothuoc;
            sluKho.Properties.NullText = "Chọn Kho";
            sluKho.Properties.DisplayMember = "tenkho";
            sluKho.Properties.ValueMember = "idkho";
            sluKho.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            sluKho.Properties.ImmediatePopup = true;
            sluKho.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

           
        }
        #endregion
 
        #region Load đối tượng Thuốc,VTYT..
        public void Load_thuoc_doituong()
        {           
            string sql = @"select LoaiThuocID,TenLoai from Thuoc_LoaiThuoc ";
            DataTable dtDoituong = DataAcess.Connect.GetTable(sql);
            sluDoituong.Properties.DataSource = dtDoituong;
            sluDoituong.Properties.NullText = "Nhập đối tượng";
            sluDoituong.Properties.DisplayMember = "TenLoai";
            sluDoituong.Properties.ValueMember = "LoaiThuocID";                        
        }
        #endregion

        #region Load nhóm cận lâm sàng
        public void NhomCLS_load()
        {
            
            string sql = @"select NhomId,TenNhom,GhiChu from  KB_NhomCLS";
            DataTable dtNhomCLS = DataAcess.Connect.GetTable(sql);
            slNhomCLS.Properties.DataSource = dtNhomCLS;
            slNhomCLS.Properties.NullText = "Nhập Nhóm CLS";
            slNhomCLS.Properties.DisplayMember = "TenNhom";
            slNhomCLS.Properties.ValueMember = "NhomId";
            slNhomCLS.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slNhomCLS.Properties.ImmediatePopup = true;
            slNhomCLS.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            
        }
        #endregion

        private void sluKhoa_EditValueChanged(object sender, EventArgs e)
        {
            Load_PhongKham();
        }

        private void sluCDXD_EditValueChanged(object sender, EventArgs e)
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

        public static string Load_ICD()
        {
            string sql = @"select IDICD,MaICD,MoTa from ChanDoanICD";
            return sql;
        }
       
        private void Load_Item_CDSB()
        {
            #region Hàm load mã ICD lên 1 ô trên Gridview CDSB
            DataTable dt1 = DataAcess.Connect.GetTable(Load_ICD());
            repositoryItemCustomGridLookUpEdit1.NullText = @"Nhập mã ICD";
            repositoryItemCustomGridLookUpEdit1.DataSource = dt1;
            repositoryItemCustomGridLookUpEdit1.ValueMember = "IDICD";
            repositoryItemCustomGridLookUpEdit1.DisplayMember = "MaICD";
            repositoryItemCustomGridLookUpEdit1.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemCustomGridLookUpEdit1.ImmediatePopup = true;
            repositoryItemCustomGridLookUpEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            colMaIcd.ColumnEdit = repositoryItemCustomGridLookUpEdit1;
            #endregion
        }
        private void Load_Item_CDPH()
        {
            #region Hàm load mã ICD lên 1 ô trên Gridview CDPH
            DataTable dtCDPH = DataAcess.Connect.GetTable(Load_ICD());
            repositoryItemCDPH.NullText = @"Nhập mã ICD";
            repositoryItemCDPH.DataSource = dtCDPH;
            repositoryItemCDPH.ValueMember = "IDICD";
            repositoryItemCDPH.DisplayMember = "MaICD";
            repositoryItemCDPH.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemCDPH.ImmediatePopup = true;
            repositoryItemCDPH.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            MaICDCDPH.ColumnEdit = repositoryItemCDPH;
            #endregion
        }

        private void Load_CDSB_Gridview()
        {
            string sql2 = @"select id,idicd,idicd MaICD,MoTaCD_edit from chandoansobo where idkhambenh='" + Truyendulieu.idkhambenh + "'";
            DataTable dt2 = DataAcess.Connect.GetTable(sql2);
            grcCDSB.DataSource = dt2;

        }
        private void Load_CDPH_Gridview()
        {
            string sql2 = @"select id,idicd,idicd MaICD,MoTaCD_edit from chandoanphoihop where idkhambenh='" + Truyendulieu.idkhambenh + "'";
            DataTable dtCDPH2 = DataAcess.Connect.GetTable(sql2);
            grcCDPH.DataSource = dtCDPH2;

        }

        private void Load_Toathuoc_Gridview()
        {
            DataTable dtluuThuoc = DataAcess.Connect.GetTable(GetData.dt_Load_Toathuoc(Truyendulieu.idkhambenh));
            grcToathuoc.DataSource = dtluuThuoc;
        }

        private void Load_Item_thuoc()
        {
            #region Load thuốc lên ô Tên thuốc gridview
            DataTable dtThuoc = DataAcess.Connect.GetTable(Thuoc_BH());
            repositoryItemThuoc.NullText = @"Nhập tên thuốc";
            repositoryItemThuoc.DataSource = dtThuoc;
            repositoryItemThuoc.ValueMember = "idthuoc";
            repositoryItemThuoc.DisplayMember = "tenthuoc";
            repositoryItemThuoc.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemThuoc.ImmediatePopup = true;
            repositoryItemThuoc.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            colidthuoc.ColumnEdit = repositoryItemThuoc;
            #endregion
        }

        private void Load_Item_Cachdung()
        {
            #region Load Cách dùng
            string sql = @"select idcachdung,tencachdung from Thuoc_CachDung";
            DataTable dtcachdung = DataAcess.Connect.GetTable(sql);
            repositoryItemCachDung.NullText = @"Nhập cách dùng";
            repositoryItemCachDung.DataSource = dtcachdung;
            repositoryItemCachDung.ValueMember = "idcachdung";
            repositoryItemCachDung.DisplayMember = "tencachdung";
            repositoryItemCachDung.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemCachDung.ImmediatePopup = true;
            repositoryItemCachDung.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            colIDCD.ColumnEdit = repositoryItemCachDung;
            #endregion
        }

        private void Load_Item_DonViDung()
        {
            #region Load Đơn vị dùng
            string sql = @"select Id,TenDVT from Thuoc_DonViTinh";
            DataTable dtDonvidung= DataAcess.Connect.GetTable(sql);
            repositoryItemDVDung.NullText = @"Nhập DVD";
            repositoryItemDVDung.DataSource = dtDonvidung;
            repositoryItemDVDung.ValueMember = "Id";
            repositoryItemDVDung.DisplayMember = "TenDVT";
            repositoryItemDVDung.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemDVDung.ImmediatePopup = true;
            repositoryItemDVDung.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            colIDDVDung.ColumnEdit = repositoryItemDVDung;

            #endregion
        }

        private string Thuoc_BH()
        {
            #region Hàm lấy thuốc BH
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
                                        where --slton>0 and 
                                        dongia>0
										ORDER BY TENTHUOC";
            return sql;
            #endregion
        }
        private void repositoryItemButtonEdit3_ButtonClick_1(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            #region Xóa chẩn đoán sơ bộ
            if (MessageBox.Show("Bạn có chắc muốn xóa Chẩn đoán sơ bộ?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["id"]).ToString();
                    if (id != null && id != "")
                    {
                        string delete = "delete chandoansobo where id =" + id;
                        bool ok = DataAcess.Connect.ExecSQL(delete);
                        if (ok)
                        {
                            MessageBox.Show("Xóa thành công!");
                            Load_CDSB_Gridview();
                        }
                    }
                    else
                    {
                        gridView2.DeleteRow(gridView2.FocusedRowHandle);
                    }
                }
                catch
                {
                    MessageBox.Show("Ô bạn chọn là ô trống!");
                }
            }
            #endregion
        }
        private void gridView2_CellValueChanged_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            #region Kiểm tra trùng mã ICD khi nhập CDSB
            try
            {
                string cls = gridView2.GetRowCellValue(e.RowHandle, colIDICD).ToString();
                for (int i = 0; i < gridView2.RowCount - 1; i++)
                {
                    if (e.RowHandle != i)
                    {
                        string idcanlamsan = gridView2.GetRowCellValue(i, gridView2.Columns["idicd"]).ToString();
                        if (cls == idcanlamsan)
                        {
                            MessageBox.Show("Đã có nhập mã ICD này rồi!");
                            gridView2.DeleteRow(gridView2.FocusedRowHandle);
                            return;
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region Click chọn ICD vào gridview CDSB
            if (e.Column.FieldName == "MaICD")
            {
                var value = gridView2.GetRowCellValue(e.RowHandle, e.Column);
                string sql = @"select IDICD,MaICD,MoTa from ChanDoanICD where  IDICD='" + value + "'";
                DataTable dt = DataAcess.Connect.GetTable(sql);
                if (dt != null)
                {
                    gridView2.SetRowCellValue(e.RowHandle, "idicd", dt.Rows[0]["IDICD"].ToString());
                    // gridView2.SetRowCellValue(e.RowHandle, "MaICD", dt.Rows[0]["MaICD"].ToString());
                    gridView2.SetRowCellValue(e.RowHandle, "MoTaCD_edit", dt.Rows[0]["MoTa"].ToString());
                }
            }
            #endregion
        }

       
        private void Load_CLS()
        {
            #region Hàm load cận lâm sàng vào 1 ô trong Gridview
            string sql = @"SELECT A.idbanggiadichvu as idbanggiadichvu
                                               ,A.tendichvu as tendichvu
                                               ,BH.GiaDV as giadichvu
                                               ,BH.GIABH as giabh
                                               ,IsSuDungChoBH=BH.ISBHYT
											   ,bh.TuNgay as fromdate
											    ,A.TENBAOHIEM as tenbaohiem
                  				            FROM BANGGIADICHVU A
               				                LEFT JOIN PHONGKHAMBENH b on a.idphongkhambenh=b.idphongkhambenh
                                            left join hs_banggiavienphi BH ON BH.IdGiaDichVu=(SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu=A.IDBANGGIADICHVU AND BH0.TuNgay<=GETDATE() ORDER BY TuNgay DESC)
                                            WHERE b.loaiphong = 1 ";
            DataTable dtCLS1 = DataAcess.Connect.GetTable(sql);
            repositoryItemCLS.NullText = @"Nhập tên CLS";
            repositoryItemCLS.DataSource = dtCLS1;
            repositoryItemCLS.ValueMember = "idbanggiadichvu";
            repositoryItemCLS.DisplayMember = "tendichvu";
            repositoryItemCLS.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemCLS.ImmediatePopup = true;
            repositoryItemCLS.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            colTenDV.ColumnEdit = repositoryItemCLS;
            #endregion

            #region  load vào cận lâm sàn hẹn
            repositoryItemCLShen.NullText = @"Nhập tên CLS";
            repositoryItemCLShen.DataSource = dtCLS1;
            repositoryItemCLShen.ValueMember = "idbanggiadichvu";
            repositoryItemCLShen.DisplayMember = "tendichvu";
            repositoryItemCLShen.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemCLShen.ImmediatePopup = true;
            repositoryItemCLShen.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            coltenDVhen.ColumnEdit = repositoryItemCLShen;

            #endregion
        }

        private void Load_CSL_gridview()
        {
            DataTable dtCLS = DataAcess.Connect.GetTable(dt_load_CLS(Truyendulieu.idkhambenh));
            grcCLS.DataSource = dtCLS;

        }
        private void Load_CSLhen_gridview()
        {
            DataTable dtCLShen = DataAcess.Connect.GetTable(dt_load_CLShen(Truyendulieu.idkhambenh));
            grcCLShen.DataSource = dtCLShen;
        }
        
        public static string dt_load_CLS(string idkhambenh)
        {
            #region Load Cận lâm sàng BS chỉ định
            string sql = @" select 
                                                                cls.idcanlamsan as idbanggiadichvu
                                                                ,cls.idcanlamsan as tendichvu
                                                                ,cls.DonGiaDV as giadichvu
                                                                ,cls.dongiabh as giabh
                                                                ,cls.IsBHYT as IsSuDungChoBH
																,cls.IsBHYT_Save as IsBHYT_Save
                                                                ,soluong = cls.soluong
                                                                ,cls.GhiChu as ghichu
                                                                ,vp.tungay as fromdate
                                                                ,cls.idkhambenhcanlamsan as IdKBCLS
                                                                ,cls.dathu as isdathu
                                                                ,cls.idnhominbv as idnhomin
                                ,StatusKQ=DBO.zHS_Status_KetQuaCLS(cls.IDKHAMBENHCANLAMSAN)
                               from khambenhcanlamsan cls
                                left join banggiadichvu bg on cls.idcanlamsan=bg.idbanggiadichvu
                                left join phongkhambenh pkb on bg.idphongkhambenh=pkb.idphongkhambenh
								left join hs_banggiavienphi vp ON vp.IdGiaDichVu = (SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu = Bg.IDBANGGIADICHVU AND BH0.TuNgay <= GETDATE() ORDER BY TuNgay DESC)
                                 where  isnull(cls.dahuy,0)=0 and cls.idkhambenh='" + idkhambenh + "'";
            return sql;
            #endregion
        }
        public static string dt_load_CLShen(string idkhambenh)
        {
            #region Load Cận lâm sàng BS hẹn
            string sql = @" select 
                                                                cls.idcanlamsan as idbanggiadichvu
                                                                ,cls.idcanlamsan as tendichvu
                                                                ,cls.DonGiaDV as giadichvu
                                                                ,cls.dongiabh as giabh
                                                                ,cls.IsBHYT as IsSuDungChoBH
																,cls.IsBHYT_Save as IsBHYT_Save
                                                                ,soluong = cls.soluong
                                                                ,cls.GhiChu as ghichu
                                                                ,vp.tungay as fromdate
                                                                ,cls.idkhambenhcanlamsanhen as IdKBCLS
                                                                ,cls.dathu as isdathu
                                                                ,cls.idnhominbv as idnhomin
                                ,StatusKQ=DBO.zHS_Status_KetQuaCLS(cls.IDKHAMBENHCANLAMSANhen)
                               from khambenhcanlamsanhen cls
                                left join banggiadichvu bg on cls.idcanlamsan=bg.idbanggiadichvu
                                left join phongkhambenh pkb on bg.idphongkhambenh=pkb.idphongkhambenh
								left join hs_banggiavienphi vp ON vp.IdGiaDichVu = (SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu = Bg.IDBANGGIADICHVU AND BH0.TuNgay <= GETDATE() ORDER BY TuNgay DESC)
                                 where  isnull(cls.dahuy,0)=0 and cls.idkhambenh='" + idkhambenh + "'";
            return sql;
            #endregion
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            #region Kiểm tra trùng CLS
            try
            {
                string cls = gridView1.GetRowCellValue(e.RowHandle, colidcls).ToString();
                for (int i = 0; i < gridView1.RowCount - 1; i++)
                {
                    if (e.RowHandle != i)
                    {
                        string idcanlamsan = gridView1.GetRowCellValue(i, gridView1.Columns["idbanggiadichvu"]).ToString();
                        if (cls == idcanlamsan)
                        {
                            MessageBox.Show("Đã có CLS này rồi!");
                            gridView1.DeleteRow(gridView1.FocusedRowHandle);
                            return;
                        }
                    }
                }
            }
            catch { }

            #endregion

            #region Click chọn CLS lên gridview
            if (e.Column.FieldName == "tendichvu")
            {
                var value = gridView1.GetRowCellValue(e.RowHandle, e.Column);
                string sql = @"select idbanggiadichvu,idbanggiadichvu as tendichvu,giadichvu,giabh,IsSuDungChoBH,fromdate,IdnhomInBV from banggiadichvu where idbanggiadichvu='" + value + "'";
                DataTable dtCLS2 = DataAcess.Connect.GetTable(sql);
                if (dtCLS2 != null)
                {
                    gridView1.SetRowCellValue(e.RowHandle, "idbanggiadichvu", dtCLS2.Rows[0]["idbanggiadichvu"].ToString());
                //    gridView1.SetRowCellValue(e.RowHandle, "tendichvu", dtCLS2.Rows[0]["tendichvu"].ToString());
                    gridView1.SetRowCellValue(e.RowHandle, "giadichvu", dtCLS2.Rows[0]["giadichvu"].ToString());
                    gridView1.SetRowCellValue(e.RowHandle, "soluong", "1");
                    gridView1.SetRowCellValue(e.RowHandle, "giabh", dtCLS2.Rows[0]["giabh"].ToString());
                    gridView1.SetRowCellValue(e.RowHandle, "IsSuDungChoBH", dtCLS2.Rows[0]["IsSuDungChoBH"].ToString());
                    gridView1.SetRowCellValue(e.RowHandle, "IsBHYT_Save", dtCLS2.Rows[0]["IsSuDungChoBH"].ToString());
                    gridView1.SetRowCellValue(e.RowHandle, "fromdate", dtCLS2.Rows[0]["fromdate"].ToString());
                    gridView1.SetRowCellValue(e.RowHandle, "idnhomin", dtCLS2.Rows[0]["IdnhomInBV"].ToString());
    
                }
            }
            #endregion
        }
        private void gridView9_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            #region Kiểm tra trùng CLS
            try
            {
                string cls = gridView9.GetRowCellValue(e.RowHandle, colidclshen).ToString();
                for (int i = 0; i < gridView9.RowCount - 1; i++)
                {
                    if (e.RowHandle != i)
                    {
                        string idcanlamsan = gridView9.GetRowCellValue(i, gridView9.Columns["idbanggiadichvu"]).ToString();
                        if (cls == idcanlamsan)
                        {
                            MessageBox.Show("Đã có CLS này rồi!");
                            gridView9.DeleteRow(gridView9.FocusedRowHandle);
                            return;
                        }
                    }
                }
            }
            catch { }

            #endregion

            #region Click chọn CLS lên gridview
            if (e.Column.FieldName == "tendichvu")
            {
                var value = gridView9.GetRowCellValue(e.RowHandle, e.Column);
                string sql = @"select idbanggiadichvu,idbanggiadichvu as tendichvu,giadichvu,giabh,IsSuDungChoBH,fromdate,IdnhomInBV from banggiadichvu where idbanggiadichvu='" + value + "'";
                DataTable dtCLS2 = DataAcess.Connect.GetTable(sql);
                if (dtCLS2 != null)
                {
                    gridView9.SetRowCellValue(e.RowHandle, "idbanggiadichvu", dtCLS2.Rows[0]["idbanggiadichvu"].ToString());
                    //    gridView1.SetRowCellValue(e.RowHandle, "tendichvu", dtCLS2.Rows[0]["tendichvu"].ToString());
                    gridView9.SetRowCellValue(e.RowHandle, "giadichvu", dtCLS2.Rows[0]["giadichvu"].ToString());
                    gridView9.SetRowCellValue(e.RowHandle, "soluong", "1");
                    gridView9.SetRowCellValue(e.RowHandle, "giabh", dtCLS2.Rows[0]["giabh"].ToString());
                    gridView9.SetRowCellValue(e.RowHandle, "IsSuDungChoBH", dtCLS2.Rows[0]["IsSuDungChoBH"].ToString());
                    gridView9.SetRowCellValue(e.RowHandle, "IsBHYT_Save", dtCLS2.Rows[0]["IsSuDungChoBH"].ToString());
                    gridView9.SetRowCellValue(e.RowHandle, "fromdate", dtCLS2.Rows[0]["fromdate"].ToString());
                    gridView9.SetRowCellValue(e.RowHandle, "idnhomin", dtCLS2.Rows[0]["IdnhomInBV"].ToString());

                }
            }
            #endregion
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            #region Xóa CLS 
            if (MessageBox.Show("Bạn có chắc muốn xóa Cận lâm sàng?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["IdKBCLS"]).ToString();
                    if (id != null && id != "")
                    {
                        string delete = "delete khambenhcanlamsan where idkhambenhcanlamsan =" + id;
                        bool ok = DataAcess.Connect.ExecSQL(delete);
                        if (ok)
                        {
                            MessageBox.Show("Xóa thành công!");
                            Load_CSL_gridview();
                        }
                    }
                    else
                    {
                        gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    }
                }
                catch
                {
                    MessageBox.Show("Ô bạn chọn là ô trống!");
                }
            }
            #endregion
        }
        private void btnXoahen_Click(object sender, EventArgs e)
        {
            #region Xóa CLS hẹn
            if (MessageBox.Show("Bạn có chắc muốn xóa Cận lâm sàng?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView9.GetRowCellValue(gridView9.FocusedRowHandle, gridView9.Columns["IdKBCLS"]).ToString();
                    if (id != null && id != "")
                    {
                        string delete = "delete khambenhcanlamsan where idkhambenhcanlamsan =" + id;
                        bool ok = DataAcess.Connect.ExecSQL(delete);
                        if (ok)
                        {
                            MessageBox.Show("Xóa thành công!");
                            Load_CSLhen_gridview();
                        }
                    }
                    else
                    {
                        gridView9.DeleteRow(gridView9.FocusedRowHandle);
                    }
                }
                catch
                {
                    MessageBox.Show("Ô bạn chọn là ô trống!");
                }
            }
            #endregion
        }
        private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            #region Kiểm tra trùng mã ICD khi nhập CDPH
            try
            {
                string cls = gridView3.GetRowCellValue(e.RowHandle, idicdCDPH).ToString();
                for (int i = 0; i < gridView3.RowCount - 1; i++)
                {
                    if (e.RowHandle != i)
                    {
                        string idcanlamsan = gridView3.GetRowCellValue(i, gridView3.Columns["idicd"]).ToString();
                        if (cls == idcanlamsan)
                        {
                            MessageBox.Show("Đã có nhập mã ICD này rồi!");
                            gridView3.DeleteRow(gridView3.FocusedRowHandle);
                            return;
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region Click chọn ICD vào gridview CDPH
            if (e.Column.FieldName == "MaICD")
            {
                var value = gridView3.GetRowCellValue(e.RowHandle, e.Column);
                string sql = @"select IDICD,MaICD,MoTa from ChanDoanICD where  IDICD='" + value + "'";
                DataTable dt = DataAcess.Connect.GetTable(sql);
                if (dt != null)
                {
                    gridView3.SetRowCellValue(e.RowHandle, "idicd", dt.Rows[0]["IDICD"].ToString());
                    // gridView2.SetRowCellValue(e.RowHandle, "MaICD", dt.Rows[0]["MaICD"].ToString());
                    gridView3.SetRowCellValue(e.RowHandle, "MoTaCD_edit", dt.Rows[0]["MoTa"].ToString());
                }
            }
            #endregion
        }

        private void repositoryItembtnXoaCDPH_Click(object sender, EventArgs e)
        {
            #region Xóa chẩn đoán Phối hợp
            if (MessageBox.Show("Bạn có chắc muốn xóa Chẩn đoán phối hợp?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["id"]).ToString();
                    if (id != null && id != "")
                    {
                        string delete = "delete chandoanphoihop where id =" + id;
                        bool ok = DataAcess.Connect.ExecSQL(delete);
                        if (ok)
                        {
                            MessageBox.Show("Xóa thành công!");
                            Load_CDPH_Gridview();
                        }
                    }
                    else
                    {
                        gridView3.DeleteRow(gridView3.FocusedRowHandle);
                    }
                }
                catch
                {
                    MessageBox.Show("Ô bạn chọn là ô trống!");
                }
            }
            #endregion
        }

        private void gridView4_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            #region Kiểm tra trùng mã Thuốc
            try
            {
                string cls = gridView4.GetRowCellValue(e.RowHandle, colidthuoc).ToString();
                for (int i = 0; i < gridView4.RowCount - 1; i++)
                {
                    if (e.RowHandle != i)
                    {
                        string idcanlamsan = gridView4.GetRowCellValue(i, gridView4.Columns["idthuoc"]).ToString();
                        if (cls == idcanlamsan)
                        {
                            MessageBox.Show("Đã có thuốc này rồi!");
                            gridView4.DeleteRow(gridView4.FocusedRowHandle);
                            return;
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region Click chọn thuốc vào gridview Thuốc
            if (e.Column.FieldName == "tenthuoc")
            {
                var value = gridView4.GetRowCellValue(e.RowHandle, e.Column);
                string sql = @"select * from (SELECT B.IDTHUOC as idthuoc
										,B.TENTHUOC as tenthuoc
										,B.LOAITHUOCID as loaithuocid
										,C.TENDVT as TenDVT
										,B.iddvt
                                        ,B.congthuc as congthuc
                                        , cd.tencachdung as duongdung
										,cd.idcachdung as idcachdung
										,cd.tencachdung as tencachdung
                                        ,B.sudungchobh as isbhyt
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
                                        and b.tenthuoc is not null
                                        and b.idthuoc='" + value + @"')ab
                                        where slton>0 and dongia>0
                                        ORDER BY TENTHUOC"; 
                DataTable dt = DataAcess.Connect.GetTable(sql);
                if (dt != null)
                {
                    gridView4.SetRowCellValue(e.RowHandle, "idthuoc", dt.Rows[0]["idthuoc"].ToString());
                    // gridView2.SetRowCellValue(e.RowHandle, "MaICD", dt.Rows[0]["MaICD"].ToString());
                    gridView4.SetRowCellValue(e.RowHandle, "congthuc", dt.Rows[0]["congthuc"].ToString());
                    gridView4.SetRowCellValue(e.RowHandle, "TenDVT", dt.Rows[0]["TenDVT"].ToString());
                    gridView4.SetRowCellValue(e.RowHandle, "iddvt", dt.Rows[0]["iddvt"].ToString());
                    gridView4.SetRowCellValue(e.RowHandle, "isbhyt", dt.Rows[0]["isbhyt"].ToString());
                    gridView4.SetRowCellValue(e.RowHandle, "slton", dt.Rows[0]["SLTON"].ToString());
                    gridView4.SetRowCellValue(e.RowHandle, "slton", dt.Rows[0]["SLTON"].ToString());
                    gridView4.SetRowCellValue(e.RowHandle, "IsBHYT_Save", dt.Rows[0]["isbhyt"].ToString());
                    gridView4.SetRowCellValue(e.RowHandle, "issang",1 );
                    gridView4.SetRowCellValue(e.RowHandle, "ischieu", 1);

                }
            }
            #endregion
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
            idphieutt = dtLuuKB.Rows[0]["IdBenhBHDongTien"].ToString();
            Truyendulieu.idbenhnhan = dtLuuKB.Rows[0]["idbenhnhan"].ToString();
            if (Truyendulieu.TypeName == "Chờ khám" || Truyendulieu.TypeName == "Chờ khám(Có tự ĐKCLS)")
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
                txtkhoa.Text = dt.Rows[0]["TENKHOA"].ToString();
                if (dt.Rows[0]["IsDungTuyen"].ToString() == "True")
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
                    txtkhoa.Text = dt.Rows[0]["TENKHOA"].ToString();
                    sluCDXD.EditValue = dt.Rows[0]["ketluan"].ToString();
                    if (dt.Rows[0]["IsDungTuyen"].ToString() == "True")
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
                    btnluu.Text = "Sửa";
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
                    txtkhoa.Text = dt.Rows[0]["TENKHOA"].ToString();
                    sluCDXD.EditValue = dt.Rows[0]["ketluan"].ToString();
                    if (dt.Rows[0]["IsDungTuyen"].ToString() == "True")
                    {
                        cbDungTuyen.Checked = true;
                    }
                    else
                    {
                        cbDungTuyen.Checked = false;
                    }
                    txtNgayxuatkhoa.Text = dt.Rows[0]["TGXuatVien"].ToString();
                    txtGiorv.Text = dt.Rows[0]["gioravien"].ToString();
                    txtPhutrv.Text = dt.Rows[0]["phutravien"].ToString();
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
                    if (dt.Rows[0]["IsBSMoiKham"].ToString() == "True")
                    {
                        chkMoiKham.Checked = true;
                    }
                    else
                    {
                        chkMoiKham.Checked = false;
                    }
                    sluBacsi.EditValue = dt.Rows[0]["idbacsi"].ToString();
                    gluBacSi2.EditValue = dt.Rows[0]["idbacsi2"].ToString();
                    sluKhoa.EditValue = dt.Rows[0]["IdkhoaChuyen"].ToString();
                    sluPK.EditValue = dt.Rows[0]["IdChuyenPK"].ToString();
                    txtSovaovien.Text = dt.Rows[0]["SOVAOVIEN1"].ToString();
                    if (dt.Rows[0]["SOVAOVIEN1"].ToString() != "" || dt.Rows[0]["SOVAOVIEN1"].ToString() != null || dt.Rows[0]["SOVAOVIEN1"].ToString() != "0")
                    {
                        btnTaoSo.Enabled = false;

                    }
                    else { btnTaoSo.Enabled = true; }
                    txtSongayratoa.Text = dt.Rows[0]["songayratoa"].ToString();
                    txtPhongKham.Text = dt.Rows[0]["TENPHONG"].ToString();
                    sluCDXD.EditValue = dt.Rows[0]["ketluan"].ToString();
                    #endregion
                    //Load_CLS(Truyendulieu.idkhambenh);
                    //Load_CLS_hen(Truyendulieu.idkhambenh);
                    //Load_ToaThuoc(Truyendulieu.idkhambenh);
                    //Load_CDSB(Truyendulieu.idkhambenh);
                    //Load_CDPH(Truyendulieu.idkhambenh);
                }
            }
        }

        private void btnluu_Click(object sender, EventArgs e)
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
                    if (gridView1.RowCount > 1)
                    {
                        isHaveCLS = "1";
                    }
                    else isHaveCLS = "0";
                }

                else
                    if (sluKhoa.Text != "Khám bệnh")
                    {
                        huongdieutri = "8";
                        if (gridView1.RowCount > 1)
                        {
                            isHaveCLS = "1";
                        }
                        else isHaveCLS = "0";
                    }
            }
            else if (sluKhoa.EditValue == null)
            {

                if (gridView4.RowCount > 1)
                {
                    huongdieutri = "2";
                    ISHAVETHUOC = "1";
                    ISHAVETHUOCBH = "1";
                    if (gridView1.RowCount > 1)
                    {
                        isHaveCLS = "1";
                    }
                    else isHaveCLS = "0";
                }
                else
                    if (gridView1.RowCount > 1 && gridView4.RowCount == 1)
                    {
                        huongdieutri = "6";
                        isHaveCLS = "1";
                    }
            }
            for (int i = 0; i < gridView2.RowCount -1; i++)
            {
                if (gridView2.GetRowCellValue(i, gridView2.Columns["MaICD"]).ToString() != "" && gridView2.GetRowCellValue(i, gridView2.Columns["MaICD"]).ToString() != null && gridView2.GetRowCellValue(i, gridView2.Columns["MaICD"]).ToString() != "0")
                {
                    mota_CDSB += gridView2.GetRowCellValue(i, gridView2.Columns["MoTaCD_edit"]).ToString() + ";";
                    MaICD_CDSB += gridView2.GetRowCellValue(i, gridView2.Columns["MaICD"]).ToString() + ";";
                }
            }
            string cdsb = mota_CDSB + "(" + MaICD_CDSB + ")";
            DataTable dtLuuKB2 = DataAcess.Connect.GetTable(this.dt_LoadBN());
            if (btnluu.Text == "Lưu")
            {
                #region Chuyển phòng không thu phí, nhập viện,ra toa

                string luuKB = @"insert into khambenh (ngaykham,idbenhnhan,iddangkykham,idbacsi,chandoanbandau,ketluan,huongdieutri,phongkhamchuyenden,idphongkhambenh,idphongchuyenden,
                                     IdChiTietDangKyKham,isNoiTru,IdPhong,DichVuKCBID,idchuyenpk,IdKhoa,idkhoachuyen,IsChuyenPhongCoPhi,isxuatvien,PhongID,songayratoa,tgxuatvien,IsHaveCLS,IsChoVeKT,IsChuyenVien,IsKhongKham,IsBSMoiKham,ishavethuocbh,MoTaCD_edit,IsTieuPhauRoiVe,ishavethuoc,Sysdate)
                                     values('" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + @"'
                                                ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                ,'" + dtLuuKB2.Rows[0]["iddangkykham"].ToString() + @"'
                                                ,'" + sluBacsi.EditValue.ToString() + @"'
                                                ,N'" + cdsb + "','" + sluCDXD.EditValue.ToString() + @"'
                                                ,'" + huongdieutri + @"'
                                                ,'" + sluKhoa.EditValue + @"',1
                                                ,'" + sluPK.EditValue + @"'
                                                ,'" + dtLuuKB2.Rows[0]["idchitietdangkykham"].ToString() + @"',0
                                                ,'" + Truyendulieu.PhongKhamID + @"'
                                                ,'" + dtLuuKB2.Rows[0]["idbanggiadichvu"].ToString() + @"'
                                                ,'" + sluPK.EditValue + @"', 1
                                                ,'" + sluKhoa.EditValue + @"', 0
                                                ,'" + chkRavien.Checked + @"'
                                                ,'" + Truyendulieu.PhongKhamID + @"'
                                                ,'" + txtSongayratoa.Text + @"'
                                                ,'" + txtNgayxuatkhoa.Text + @"'
                                                ,'" + isHaveCLS + @"',0,0,0,0
                                                ,'" + ISHAVETHUOCBH + @"'
                                                ,'" + txtCDXD.Text + @"',0
                                                ,'" + ISHAVETHUOC + @"'
                                                ,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "')";
                // DataTable luukb2 = DataAcess.Connect.GetTable(luuKB);
                bool okk = DataAcess.Connect.ExecSQL(luuKB);
                if (okk)
                {
                    string updateCT = "update chitietdangkykham set dakham=1 where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'";
                    DataTable LuuCT = DataAcess.Connect.GetTable(updateCT);
                    for (int x = 0; x < gridView2.RowCount - 1; x++)
                    {
                        string insertCDSB = @"insert into chandoansobo (id,idkhambenh,idicd,maicd,MoTaCD_edit) values 
                                                        ((select max(id) from chandoansobo)+1,(select max(idkhambenh) from khambenh where 
                                                        idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                        ,'" + gridView2.GetRowCellValue(x, gridView2.Columns["idicd"]).ToString() + @"'
                                                        ,'" + gridView2.GetRowCellValue(x, gridView2.Columns["MaICD"]).ToString() + @"'
                                                        ,N'" + gridView2.GetRowCellValue(x, gridView2.Columns["MoTaCD_edit"]).ToString() + "')";
                        DataTable luuCDSB = DataAcess.Connect.GetTable(insertCDSB);
                    }
                    for (int i = 0; i < gridView3.RowCount - 1; i++)
                    {
                        string insertCDPH = @"insert into chandoanphoihop (idkhambenh,idicd,maicd,MoTaCD_edit) values 
                                                        ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                        ,'" + gridView3.GetRowCellValue(i, gridView3.Columns["idicd"]).ToString() + @"'
                                                        ,'" + gridView3.GetRowCellValue(i, gridView3.Columns["MaICD"]).ToString() + @"'
                                                        ,N'" + gridView3.GetRowCellValue(i, gridView3.Columns["MoTaCD_edit"]).ToString() + "')";
                        DataTable luuCDPH = DataAcess.Connect.GetTable(insertCDPH);
                    }
                    string insertSH = @"insert into sinhhieu (idbenhnhan,ngaydo,mach,nhietdo,huyetap1,huyetap2,nhiptho,chieucao,cannang,BMI,Iddangkykham,idchitietdangkykham,idkhoasinhhieu,IdKhamBenh) values ('" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"',
                                                    '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm") + @"'
                                                    ," + (this.txtMach.Text == "" ? "Null" : "N'" + this.txtMach.Text + @"'") + @"
                                                    ," + (this.txtNhietDo.Text == "" ? "Null" : "N'" + this.txtNhietDo.Text + @"'") + @"
                                                    ," + (this.txtHuyetAp.Text == "" ? "Null" : "N'" + this.txtHuyetAp.Text + @"'") + @"
                                                    ," + (this.txtHuyetAp2.Text == "" ? "Null" : "N'" + this.txtHuyetAp2.Text + @"'") + @"
                                                    ," + (this.txtNhipTho.Text == "" ? "Null" : "N'" + this.txtNhipTho.Text + @"'") + @"
                                                    ," + (this.txtChieuCao.Text == "" ? "Null" : "N'" + this.txtChieuCao.Text + @"'") + @"
                                                    ," + (this.txtCanNang.Text == "" ? "Null" : "N'" + this.txtCanNang.Text + @"'") + @"
                                                    ," + (this.txtBMI.Text == "" ? "Null" : "N'" + this.txtBMI.Text + @"'") + @"
                                                    ,'" + dtLuuKB2.Rows[0]["iddangkykham"].ToString() + @"'
                                                    ,'" + dtLuuKB2.Rows[0]["idchitietdangkykham"].ToString() + @"',1
                                                    ,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'))";
                    DataTable luuSinhhieu = DataAcess.Connect.GetTable(insertSH);
                }

                #region Có nhập cls
                if (gridView1.RowCount > 1)
                {
                    string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                    string insertDKCLS = "insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                    bool ok = DataAcess.Connect.ExecSQL(insertDKCLS);
                    if (ok)
                    {
                        for (int t = 0; t < gridView1.RowCount - 1; t++)
                        {
                            string luuCLS = @"insert into khambenhcanlamsan (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                                        values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + gridView1.GetRowCellValue(t, gridView1.Columns["idbanggiadichvu"]).ToString() + @"'
                                                                        ,'" + sluBacsi.EditValue.ToString() + "',0,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + "','" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                                        ,'" + maphieucls + "','" + gridView1.GetRowCellValue(t, gridView1.Columns["soluong"]).ToString() + "','" + gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() + "','" + gridView1.GetRowCellValue(t, gridView1.Columns["ghichu"]).ToString() + @"'
                                                                        ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + "',0,'" + gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() + "','" + gridView1.GetRowCellValue(t, gridView1.Columns["giadichvu"]).ToString() + "','" + gridView1.GetRowCellValue(t, gridView1.Columns["IsSuDungChoBH"]).ToString() + @"'
                                                                        ,0,'" + gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() + "','" + gridView1.GetRowCellValue(t, gridView1.Columns["giadichvu"]).ToString() + "',(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                                        ,'" + gridView1.GetRowCellValue(t, gridView1.Columns["idnhomin"]).ToString() + "','" + gridView1.GetRowCellValue(t, gridView1.Columns["IsBHYT_Save"]).ToString() + "','" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + "')";
                            DataTable Luu = DataAcess.Connect.GetTable(luuCLS);
                        }

                    }
                    MessageBox.Show("Thành công");
                }
                #endregion

                #region Có nhập hẹn cls
                if (gridView9.RowCount > 1)
                {
                    string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                    string insertDKCLS = @"insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values
                                                   ('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                    bool ok = DataAcess.Connect.ExecSQL(insertDKCLS);
                    if (ok)
                    {
                        for (int t = 0; t < gridView9.RowCount - 1; t++)
                        {
                            string luuCLS = @"insert into khambenhcanlamsanhen (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                        values 
                                            ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["idbanggiadichvu"]).ToString() + @"'
                                            ,'" + sluBacsi.EditValue.ToString() + @"',0
                                            ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + @"'
                                            ,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + @"'
                                            ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                            ,'" + maphieucls + @"'
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["soluong"]).ToString() + @"'
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() + @"'
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["ghichu"]).ToString() + @"'
                                            ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + @"',0
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() + @"'
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["giadichvu"]).ToString() + @"'
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["IsSuDungChoBH"]).ToString() + @"'
                                            ,0,'" + gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() + @"'
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["giadichvu"]).ToString() + @"'                            
                                            ,(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["idnhomin"]).ToString() + @"'
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["IsBHYT_Save"]).ToString() + @"'
                                            ,'" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + "')";
                            DataTable Luu = DataAcess.Connect.GetTable(luuCLS);
                        }

                    }
                    MessageBox.Show("Thành công");
                }
                #endregion

                #region có nhập toa thuốc
                if (gridView4.RowCount > 1)
                {
                    string insertBNTT = @"insert into benhnhantoathuoc (idkhambenh,idbacsi,idbenhnhan,ngayratoa) values 
                                                    ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                    ,'" + sluBacsi.EditValue.ToString() + @"'
                                                    ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                    ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "')";
                    bool ook = DataAcess.Connect.ExecSQL(insertBNTT);
                    if (ook)
                    {
                        for (int i = 0; i < gridView4.RowCount - 1; i++)
                        {
                            string insertCTBNTT = @"insert into chitietbenhnhantoathuoc (idbenhnhantoathuoc,idthuoc,soluongke,ngayuong,moilanuong,ghichu,idkhambenh,idkho,doituongthuocID,idcachdung,iddvdung,iddvt,ischieu,issang,istoi,istrua,ngayratoa,isbhyt_save,slton,isdaxuat,slxuat)
                                                        values 
                                                        ((select max(idbenhnhantoathuoc) from benhnhantoathuoc where idbenhnhan='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["idthuoc"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["soluongke"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["ngayuong"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["moilanuong"]).ToString() + @"'
                                                        ," + (gridView4.GetRowCellValue(i, gridView4.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView4.GetRowCellValue(i, gridView4.Columns["ghichu"]).ToString() + @"'") + @"                                                 
                                                        ,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                        ," + (this.sluKho.Text == "" ? "Null" : "'" + this.sluKho.EditValue.ToString() + "'") + @"
                                                        ," + (this.sluDoituong.Text == "" ? "Null" : "'" + this.sluDoituong.EditValue.ToString() + "'") + @"
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["idcachdung"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["iddvdung"]).ToString() + @"' 
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["iddvt"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["ischieu"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["issang"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["istoi"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["istrua"]).ToString() + @"'
                                                        ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00") + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["IsBHYT_Save"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["slton"]).ToString() + @"',0
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["sldaxuat"]).ToString() + "')";//+ dtgvThuoc.Rows[i].Cells["dvdung"].Value + 
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
                if (btnluu.Text == "Sửa")
                {
                    for (int i = 0; i < gridView2.RowCount - 1; i++)
                    {
                        #region thêm chẩn đoán sơ bộ
                        if (gridView2.GetRowCellValue(i, gridView2.Columns["id"]).ToString() == "" && gridView2.GetRowCellValue(i, gridView2.Columns["id"]).ToString() == null && gridView2.GetRowCellValue(i, gridView2.Columns["id"]).ToString() == "0")
                        {
                            string insertCDSB = @"insert into chandoansobo (id,idkhambenh,idicd,maicd,MoTaCD_edit) 
                                                    values 
                                                    ((select max(id) from chandoansobo)+1,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                    ,'" + gridView2.GetRowCellValue(i, gridView2.Columns["idicd"]).ToString() + @"'
                                                    ,'" + gridView2.GetRowCellValue(i, gridView2.Columns["MaICD"]).ToString() + @"'
                                                    ,N'" + gridView2.GetRowCellValue(i, gridView2.Columns["MoTaCD_edit"]).ToString() + "')";
                            DataTable luuCDSB = DataAcess.Connect.GetTable(insertCDSB);
                        }
                        #endregion
                    }
                    for (int x = 0; x < gridView3.RowCount - 1; x++)
                    {
                        #region thêm chẩn đoán phối hợp
                        if (gridView3.GetRowCellValue(x, gridView3.Columns["id"]).ToString() == "" && gridView3.GetRowCellValue(x, gridView3.Columns["id"]).ToString() == null && gridView3.GetRowCellValue(x, gridView3.Columns["id"]).ToString() == "0")
                        {
                            string insertCDPH = @"insert into chandoanphoihop (idkhambenh,idicd,maicd,MoTaCD_edit) values 
                                                            ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                            ,'" + gridView3.GetRowCellValue(x, gridView3.Columns["idicd"]).ToString() + @"'
                                                            ,'" + gridView3.GetRowCellValue(x, gridView3.Columns["MaICD"]).ToString() + @"'
                                                            ,N'" + gridView3.GetRowCellValue(x, gridView3.Columns["MoTaCD_edit"]).ToString() + "')";
                            DataTable luuCDPH = DataAcess.Connect.GetTable(insertCDPH);
                        }
                        #endregion
                    }
                    for (int y = 0; y < gridView1.RowCount - 1; y++)
                    {
                        string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                        string insertDKCLS = "insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                        DataTable dkcls = DataAcess.Connect.GetTable(insertDKCLS);
                    #region update bảng khambenhcanlamsan
                        if (gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() != "" && gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() != "0" && gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() != null)
                        {
                            string updateCLS = @"update khambenhcanlamsan set 
                                                        soluong='" + gridView1.GetRowCellValue(y, gridView1.Columns["soluong"]).ToString() + @"'
                                                        ,isbhyt_save='" + gridView1.GetRowCellValue(y, gridView1.Columns["IsBHYT_Save"]).ToString() + @"'
                                                        ,ghichu= " + (gridView1.GetRowCellValue(y, gridView1.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView1.GetRowCellValue(y, gridView1.Columns["ghichu"]).ToString() + @"'") + @" 
                                                        ,idcanlamsan='" + gridView1.GetRowCellValue(y, gridView1.Columns["idbanggiadichvu"]).ToString() + @"'
                                        where idkhambenhcanlamsan='" + gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() + "'";
                            //DataTable editCLS = DataAcess.Connect.GetTable(updateCLS);
                            bool okk = DataAcess.Connect.ExecSQL(updateCLS);
                            if (okk)
                            {
                                MessageBox.Show("Update CLS thành công");
                            }
                            else
                            {
                                MessageBox.Show("Update CLS không thành công");
                            }
                            
                        }
                        else
                        {
                            string luuCLS = @"insert into khambenhcanlamsan (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["idbanggiadichvu"]).ToString() + @"'
                                                ,'" + sluBacsi.EditValue.ToString() + @"',0
                                                ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                                                ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + @"'
                                                ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                ,'" + maphieucls + @"'
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["soluong"]).ToString() + @"'
                                                ," + (gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString()== null ? "Null" : "'" + gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() + @"'") + @"
                                                ," + (gridView1.GetRowCellValue(y, gridView1.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView1.GetRowCellValue(y, gridView1.Columns["ghichu"]).ToString() + @"'") + @" 
                                                ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + @"',0
                                                ," + (gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString()== null ? "Null" : "'" + gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() + @"'") + @"
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["giadichvu"]).ToString() + @"'
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["IsSuDungChoBH"]).ToString() + @"'
                                                ,0," + (gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString()== null ? "Null" : "'" + gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() + @"'") + @"
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["giadichvu"]).ToString() + @"'
                                                ,(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["idnhomin"]).ToString() + @"'
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["IsBHYT_Save"]).ToString() + @"'
                                                ,'" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + @"')";
                           // DataTable Luu = DataAcess.Connect.GetTable(luuCLS);
                            bool okk = DataAcess.Connect.ExecSQL(luuCLS);
                            if (okk)
                            {
                                MessageBox.Show("Insert CLS thành công");
                            }
                            else
                            {
                                MessageBox.Show("Insert CLS không thành công");
                            }
                        }
                    }

                        #endregion

                    #region update bảng khambenhcanlamsanhen
                    for (int y = 0; y < gridView9.RowCount - 1; y++)
                    {
                        string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                        string insertDKCLS = @"insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values
                            ('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                        DataTable dkcls = DataAcess.Connect.GetTable(insertDKCLS);

                        if (gridView9.GetRowCellValue(y, gridView9.Columns["IdKBCLS"]).ToString() != "" && gridView9.GetRowCellValue(y, gridView9.Columns["IdKBCLS"]).ToString() != "0" && gridView9.GetRowCellValue(y, gridView9.Columns["IdKBCLS"]).ToString() != null)
                        {

                            string updateCLS = @"update khambenhcanlamsanhen set 
                                                        soluong='" + gridView9.GetRowCellValue(y, gridView9.Columns["soluong"]).ToString() + @"'
                                                        ,isbhyt_save='" + gridView9.GetRowCellValue(y, gridView9.Columns["isbhyt_save"]).ToString() + @"'
                                                        ,ghichu=" + (gridView9.GetRowCellValue(y, gridView9.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView9.GetRowCellValue(y, gridView9.Columns["ghichu"]).ToString() + @"'") + @"
                                                        ,idcanlamsan=N'" + gridView9.GetRowCellValue(y, gridView9.Columns["idbanggiadichvu"]).ToString() + @"'

                                                  where idkhambenhcanlamsanhen='" + gridView9.GetRowCellValue(y, gridView9.Columns["IdKBCLS"]).ToString() + "'";
                            DataTable editCLS = DataAcess.Connect.GetTable(updateCLS);
                        }
                        else
                        {
                            string luuCLS = @"insert into khambenhcanlamsanhen (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                                        values 
                                                    ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["idbanggiadichvu"]).ToString() + @"'
                                                    ,'" + sluBacsi.EditValue.ToString() + @"',0
                                                    ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + @"'
                                                    ,'" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + @"'
                                                    ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                    ,'" + maphieucls + @"'
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["soluong"]).ToString() + @"'
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["giabh"]).ToString() + @"'
                                                    ," + (gridView9.GetRowCellValue(y, gridView9.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView9.GetRowCellValue(y, gridView9.Columns["ghichu"]).ToString() + @"'") + @"                                                 
                                                    ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + @"',0
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["giabh"]).ToString() + @"'
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["giadichvu"]).ToString() + @"'
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["IsSuDungChoBH"]).ToString() + @"'
                                                    ,0,'" + gridView9.GetRowCellValue(y, gridView9.Columns["giabh"]).ToString() + @"'
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["giadichvu"]).ToString() + @"'
                                                    ,(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["idnhomin"]).ToString() + @"'
                                                    ,'" + gridView9.GetRowCellValue(y, gridView9.Columns["IsBHYT_Save"]).ToString() + @"'
                                                    ,'" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + "')";
                            DataTable Luu = DataAcess.Connect.GetTable(luuCLS);
                        }
                    }
                    #endregion
                    
                }
            for (int z = 0; z < gridView4.RowCount - 1; z++)
            {
                #region update bảng chitietbenhnhantoathuoc
                if (gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() != "" && gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() != "0" && gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() != null)
                {
                    string updateThuoc = @"update chitietbenhnhantoathuoc set 
                                                soluongke='" + gridView4.GetRowCellValue(z, gridView4.Columns["soluongke"]).ToString() + @"'
                                                ,ngayuong='" + gridView4.GetRowCellValue(z, gridView4.Columns["ngayuong"]).ToString() + @"'
                                                ,moilanuong='" + gridView4.GetRowCellValue(z, gridView4.Columns["moilanuong"]).ToString() + @"'
                                                ,ghichu=" + (gridView4.GetRowCellValue(z, gridView4.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView4.GetRowCellValue(z, gridView4.Columns["ghichu"]).ToString() + @"'") + @"                                                 
                                                ,idcachdung='" + gridView4.GetRowCellValue(z, gridView4.Columns["idcachdung"]).ToString() + @"'
                                                ,iddvdung='" + gridView4.GetRowCellValue(z, gridView4.Columns["iddvdung"]).ToString() + @"'
                                                ,iddvt='" + gridView4.GetRowCellValue(z, gridView4.Columns["iddvt"]).ToString() + @"'
                                                ,ischieu='" + gridView4.GetRowCellValue(z, gridView4.Columns["ischieu"]).ToString() + @"'
                                                ,issang='" + gridView4.GetRowCellValue(z, gridView4.Columns["issang"]).ToString() + @"'
                                                ,istoi='" + gridView4.GetRowCellValue(z, gridView4.Columns["istoi"]).ToString() + @"'
                                                ,istrua='" + gridView4.GetRowCellValue(z, gridView4.Columns["istrua"]).ToString() + @"'
                                                ,IsBHYT_Save='" + gridView4.GetRowCellValue(z, gridView4.Columns["IsBHYT_Save"]).ToString() + @"' 
                                            where idkhambenh='" + Truyendulieu.idkhambenh + "'";
                    DataTable editThuoc = DataAcess.Connect.GetTable(updateThuoc);
                }
                #endregion

                #region nếu nhập thêm thuốc thì insert
                if (gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() == "" && gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() == null)
                {
                    string insertCTBNTT = @"insert into chitietbenhnhantoathuoc (idbenhnhantoathuoc,idthuoc,soluongke,ngayuong,moilanuong,ghichu,idkhambenh,idkho,doituongthuocID,idcachdung,iddvdung,iddvt,ischieu,issang,istoi,istrua,ngayratoa,isbhyt_save,slton,isdaxuat,slxuat)
                                                values ((select max(idbenhnhantoathuoc) from benhnhantoathuoc where idbenhnhan='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["idthuoc"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["soluongke"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["ngayuong"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["moilanuong"]).ToString() + @"'
                                                ," + (gridView4.GetRowCellValue(z, gridView4.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView4.GetRowCellValue(z, gridView4.Columns["ghichu"]).ToString() + @"'") + @" 
                                                ,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                ,'" + sluKho.EditValue + "','" + sluDoituong.EditValue + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["idcachdung"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["iddvdung"]).ToString() + @"' 
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["iddvt"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["ischieu"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["issang"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["istoi"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["istrua"]).ToString() + @"'
                                                ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00") + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["IsBHYT_Save"]).ToString() + @"'
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["slton"]).ToString() + @"',0
                                                ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["sldaxuat"]).ToString() + @"')";
                    DataTable luuToa = DataAcess.Connect.GetTable(insertCTBNTT);
                }
                #endregion
            }

            #region Update lại table KhamBenh
            string updateKB = @"update khambenh set 
                                            idbacsi='" + sluBacsi.EditValue.ToString() + @"'
                                            ,chandoanbandau=N'" + cdsb + @"'
                                            ,ketluan='" + sluCDXD.EditValue.ToString() + @"'
                                            ,huongdieutri='" + huongdieutri + @"'
                                            ,phongkhamchuyenden='" + sluKhoa.EditValue.ToString() + @"'
                                            ,idphongchuyenden='" + sluPK.EditValue.ToString() + @"'
                                            ,isNoiTru='" + chkNoitru.Checked + @"'
                                            ,idphong='" + Truyendulieu.PhongKhamID + @"'
                                            ,idchuyenpk='" + sluPK.EditValue.ToString() + @"'
                                            ,idkhoachuyen='" + sluKhoa.EditValue.ToString() + @"'
                                            ,IsChuyenPhongCoPhi='" + chkThuphi.Checked + @"'
                                            ,isxuatvien='" + chkRavien.Checked + @"'
                                            ,PhongID='" + Truyendulieu.PhongKhamID + @"'
                                            ,songayratoa='" + txtSongayratoa.Text + @"'
                                            ,tgxuatvien='" + txtNgayxuatkhoa.Text + @"'
                                            ,IsHaveCLS='" + isHaveCLS + @"'
                                            ,IsChoVeKT='" + chkChovekt.Checked + @"'
                                            ,IsChuyenVien='" + chkChuyenVien.Checked + @"'
                                            ,IsKhongKham='" + chkKhongKham.Checked + @"'
                                            ,idbacsi2='" + gluBacSi2.EditValue.ToString() + @"'
                                            ,IsBSMoiKham='" + chkMoiKham.Checked + @"'
                                            ,ishavethuocbh='" + ISHAVETHUOCBH + @"'
                                            ,MoTaCD_edit='" + txtCDXD.Text + @"'
                                            ,IsTieuPhauRoiVe='" + chkTieuPhau.Checked + @"'
                                            ,ishavethuoc='" + ISHAVETHUOC + @"' 
                                  where idkhambenh='" + Truyendulieu.idkhambenh + "'";
            DataTable LuuKB = DataAcess.Connect.GetTable(updateKB);
            #endregion
            MessageBox.Show("Thành công");
        }

        private void btnTaoSo_Click(object sender, EventArgs e)
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
            btnTaoSo.Enabled = false;
        }

        private void btnmoi_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btninthuocbh_Click(object sender, EventArgs e)
        {
            frmToaThuocBH frmpTT = new frmToaThuocBH();
            frmpTT.Show();
        }

        private void btninthuocdv_Click(object sender, EventArgs e)
        {
            frmToaThuocDV frmpTTDV = new frmToaThuocDV();
            frmpTTDV.Show();
        }

        private void btninclsbh_Click(object sender, EventArgs e)
        {
            frmRptCLS frmp = new frmRptCLS();
            frmp.Show();
        }

        private void btninbv01_Click(object sender, EventArgs e)
        {
            Truyendulieu.idphieutt = idphieutt;
            frmBV01 frm01 = new frmBV01();
            frm01.Show();
        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            string sqlCheckCLS = @"SELECT * FROM KHAMBENHCANLAMSAN WHERE IDKHAMBENH='" + Truyendulieu.idkhambenh + "'" + " AND ISNULL(DAHUY,0)=0";
            DataTable dtCheckCLS = DataAcess.Connect.GetTable(sqlCheckCLS);
            if (dtCheckCLS != null && dtCheckCLS.Rows.Count > 0)
            {
                if (user=="admin")
                {
                    MessageBox.Show("Lỗi: Bệnh nhân đã cận lâm sàn");                      
                    return;
                }
                int n_DaThu = StaticData.int_Search(dtCheckCLS, "DATHU=1");
                if (n_DaThu != -1)
                {
                    MessageBox.Show("Lỗi: Cận lâm sàn đã thu tiền rồi");
                    return;
                }
            }
            string sqlCheckThuoc = @"SELECT DAXUAT=DBO.THUOC_ISDAXUAT_TOA(IDCHITIETBENHNHANTOATHUOC) FROM CHITIETBENHNHANTOATHUOC WHERE IDKHAMBENH='" + Truyendulieu.idkhambenh + "'";
            DataTable dtCheckThuoc = DataAcess.Connect.GetTable(sqlCheckThuoc);
            if (dtCheckThuoc != null && dtCheckThuoc.Rows.Count > 0)
            {
                if (user=="admin")
                {
                    MessageBox.Show("Lỗi: Bệnh nhân đã có thuốc rồi"); 
                    return;
                }
                int n_DaXuat = StaticData.int_Search(dtCheckThuoc, "DAXUAT=1");
                if (n_DaXuat != -1)
                {
                    MessageBox.Show("Lỗi : Không thể xóa khi đã xuất thuốc"); 
                    return;
                }
            }
            string deleteKB = @"DELETE  khambenh  WHERE idkhambenh=" + Truyendulieu.idkhambenh;
            bool ok = DataAcess.Connect.ExecSQL(deleteKB);
            if (ok)
            {
                DataAcess.Connect.Exec("delete from kb_giuongphieuthanhtoan where idptt = " + Truyendulieu.idkhambenh);
                MessageBox.Show("Xóa thông tin khám bệnh thành công!");
                return;
            }
            
        }

        private void btninclsdv_Click(object sender, EventArgs e)
        {
            txttiensu.Text = gridView1.GetRowCellValue(1, gridView1.Columns["soluong"]).ToString();
            txtbenhsu.Text = gridView1.GetRowCellValue(1, gridView1.Columns["idnhomin"]).ToString();
            txtdiung.Text = gridView1.GetRowCellValue(1, gridView1.Columns["IsSuDungChoBH"]).ToString();
            txttrieuchung.Text = gridView1.GetRowCellValue(1, gridView1.Columns["IsBHYT_Save"]).ToString();
        }

        

       
       

      

      

 
    }
}
