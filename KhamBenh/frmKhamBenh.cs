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
using System.Collections;

namespace KhamBenhPro.KhamBenh
{
    public partial class frmKhamBenh : Form
    {
       
        string loaikhamID = null;
        string idphieutt = null;
        string idchitietdangkykham = null;
        string user = "admin";
        DataTable dt2 = null;
        DataTable dt = null;
        string iddangkykham1 = null;
        string idkhambenh_new = null;
        int clscount = 0;
        int thuocdv_click = 0;
        int thuocbh_click = 0;
        int henCLS_click = 0;
        public frmKhamBenh()
        {
            InitializeComponent();
        }

        private void frmKhamBenh_Load(object sender, EventArgs e)
        {
            sluKhoa.Enabled = false;
            sluPK.Enabled = false;
            sluKhoa.EditValue = null;
            Load_ChanDoanSoBo();
            Load_CLS();
            Load_CSL_gridview();
           //Load_CSLhen_gridview();
            LoadCDXD();
            LoadCDPH();
            //Load_Item_thuoc();
            //Load_Item_Cachdung();
            //Load_Item_DonViDung();
            //Load_Toathuoc_Gridview();
            //Load_Item_thuoc_DV();
            //Load_Item_DonViDung_DV();
            //Load_Item_Cachdung_DV();
            //Load_ToathuocDV_Gridview();
            Load_BNchokham();
            LoadsluBacsi2();
            LoadsluBacsi();
            Load_Khoa();
           // KhoThuoc_load();
            //Load_thuoc_doituong();
            NhomCLS_load();
            sluPK.Properties.NullText = "Chọn phòng khám";
            clscount = gridView1.RowCount;
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

        #region Load Chẩn đoán Phối hợp
        public void LoadCDPH()
        {
            DataTable CDPH = DataAcess.Connect.GetTable(GetData.LoadICD10());
            sluCDPH.Properties.DataSource = CDPH;
            sluCDPH.Properties.DisplayMember = "MaICD";
            sluCDPH.Properties.ValueMember = "IDICD";
            sluCDPH.Properties.NullText = "Nhập chẩn đoán";
            sluCDPH.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            sluCDPH.Properties.ImmediatePopup = true;
            sluCDPH.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

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
                                                and idphongkhambenh !=51
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

        //#region Load kho thuốc
        //public void KhoThuoc_load()
        //{

        //    string sql = @"select idkho,tenkho from khothuoc where idkho in (72,5)";
        //    DataTable dtKhothuoc = DataAcess.Connect.GetTable(sql);
        //    sluKho.Properties.DataSource = dtKhothuoc;
        //    sluKho.Properties.NullText = "Chọn Kho";
        //    sluKho.Properties.DisplayMember = "tenkho";
        //    sluKho.Properties.ValueMember = "idkho";
        //    sluKho.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
        //    sluKho.Properties.ImmediatePopup = true;
        //    sluKho.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;


        //}
        //#endregion

        //#region Load đối tượng Thuốc,VTYT..
        //public void Load_thuoc_doituong()
        //{
        //    string sql = @"select LoaiThuocID,TenLoai from Thuoc_LoaiThuoc ";
        //    DataTable dtDoituong = DataAcess.Connect.GetTable(sql);
        //    sluDoituong.Properties.DataSource = dtDoituong;
        //    sluDoituong.Properties.NullText = "Nhập đối tượng";
        //    sluDoituong.Properties.DisplayMember = "TenLoai";
        //    sluDoituong.Properties.ValueMember = "LoaiThuocID";
        //}
        //#endregion

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

        private void sluKhoa_EditValueChanged_1(object sender, EventArgs e)
        {
            if (sluKhoa.EditValue == null)
            {
                return;
            }
            else
            {
                Load_PhongKham();
            }
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

        public static string Load_ICD()
        {
            string sql = @"select IDICD,MaICD,MoTa from ChanDoanICD";
            return sql;
        }
       
        //private void Load_Item_CDSB()
        //{
        //    #region Hàm load mã ICD lên 1 ô trên Gridview CDSB
        //    DataTable dt1 = DataAcess.Connect.GetTable(Load_ICD());
        //    repositoryItemCustomGridLookUpEdit1.NullText = @"Nhập mã ICD";
        //    repositoryItemCustomGridLookUpEdit1.DataSource = dt1;
        //    repositoryItemCustomGridLookUpEdit1.ValueMember = "IDICD";
        //    repositoryItemCustomGridLookUpEdit1.DisplayMember = "MaICD";
        //    repositoryItemCustomGridLookUpEdit1.BestFitMode = BestFitMode.BestFitResizePopup;
        //    repositoryItemCustomGridLookUpEdit1.ImmediatePopup = true;
        //    repositoryItemCustomGridLookUpEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        //    colMaIcd.ColumnEdit = repositoryItemCustomGridLookUpEdit1;
        //    #endregion
        //}
        //private void Load_Item_CDPH()
        //{
        //    #region Hàm load mã ICD lên 1 ô trên Gridview CDPH
        //    DataTable dtCDPH = DataAcess.Connect.GetTable(Load_ICD());
        //    repositoryItemCDPH.NullText = @"Nhập mã ICD";
        //    repositoryItemCDPH.DataSource = dtCDPH;
        //    repositoryItemCDPH.ValueMember = "IDICD";
        //    repositoryItemCDPH.DisplayMember = "MaICD";
        //    repositoryItemCDPH.BestFitMode = BestFitMode.BestFitResizePopup;
        //    repositoryItemCDPH.ImmediatePopup = true;
        //    repositoryItemCDPH.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        //    MaICDCDPH.ColumnEdit = repositoryItemCDPH;
        //    #endregion
        //}

        //private void Load_CDSB_Gridview()
        //{
        //    string sql2 = @"select id,idicd,idicd MaICD,MoTaCD_edit from chandoansobo where idkhambenh='" + Truyendulieu.idkhambenh + "'";
        //    DataTable dt2 = DataAcess.Connect.GetTable(sql2);
        //    grcCDSB.DataSource = dt2;

        //}
        //private void Load_CDPH_Gridview()
        //{
        //    string sql2 = @"select id,idicd,idicd MaICD,MoTaCD_edit from chandoanphoihop where idkhambenh='" + Truyendulieu.idkhambenh + "'";
        //    DataTable dtCDPH2 = DataAcess.Connect.GetTable(sql2);
        //    grcCDPH.DataSource = dtCDPH2;
        //}

        private void Load_Toathuoc_Gridview()
        {
            DataTable dtluuThuoc = DataAcess.Connect.GetTable(GetData.dt_Load_Toathuoc(Truyendulieu.idkhambenh));
            grcToathuoc.DataSource = dtluuThuoc;
        }
        private void Load_Toathuoc_Gridview_new()
        {
            DataTable dtluuThuoc = DataAcess.Connect.GetTable(GetData.dt_Load_Toathuoc(idkhambenh_new));
            grcToathuoc.DataSource = dtluuThuoc;
        }

        private void Load_ToathuocDV_Gridview()
        {
            DataTable dtThuocdv = DataAcess.Connect.GetTable(GetData.dt_Load_Toathuoc_DV(Truyendulieu.idkhambenh));
            grcToaDV.DataSource = dtThuocdv;
        }
        private void Load_ToathuocDV_Gridview_new()
        {
            DataTable dtThuocdv = DataAcess.Connect.GetTable(GetData.dt_Load_Toathuoc_DV(idkhambenh_new));
            grcToaDV.DataSource = dtThuocdv;
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

        private void Load_Item_thuoc_DV()
        {
            #region Load thuốc Dịch vụ lên ô Tên thuốc gridview
            DataTable dtThuocdv = DataAcess.Connect.GetTable(Thuoc_DV());
            repositoryItemThuocDV.NullText = @"Nhập tên thuốc";
            repositoryItemThuocDV.DataSource = dtThuocdv;
            repositoryItemThuocDV.ValueMember = "idthuoc";
            repositoryItemThuocDV.DisplayMember = "tenthuoc";
            repositoryItemThuocDV.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemThuocDV.ImmediatePopup = true;
            repositoryItemThuocDV.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            colIDthuocDV.ColumnEdit = repositoryItemThuocDV;
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
                                        where slton>0 and 
                                       dongia>0
										ORDER BY TENTHUOC";
            return sql;
            #endregion
        }

       
        private string Thuoc_DV()
        {
            #region Hàm lấy thuốc Dịch vụ
            string sql = @"select * from (SELECT B.IDTHUOC as idthuoc
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
                                                        ,SLTON = (SELECT SUM(SOLUONG) FROM LOCALSERVER.NhaThuocDB.DBO.CHITIETPHIEUNHAPKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_NHAP = 72 )-ISNULL((SELECT SUM(SOLUONG) FROM LOCALSERVER.NhaThuocDB.DBO.CHITIETPHIEUXUATKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_XUAT = 72),0)
                                                        ,DonGia = (SELECT TOP 1 NhaThuocDB.DBO.zHs_GetGiaBan(DONGIA, VAT) FROM LOCALSERVER.NhaThuocDB.DBO.CHITIETPHIEUNHAPKHO WHERE IDTHUOC = B.IDTHUOC AND IDKHO_NHAP = 72)
                                                        ,TrungThuoc = ''
                        FROM LOCALSERVER.NhaThuocDB.DBO.Thuoc B
                        left join LOCALSERVER.NhaThuocDB.DBO.thuoc_donvitinh C on C.id = B.iddvt
                        left join LOCALSERVER.NhaThuocDB.DBO.thuoc_cachdung cd on cd.idcachdung = B.idcachdung
                        LEFT JOIN LOCALSERVER.NhaThuocDB.DBO.zHS_ThuTuThuoc T4 ON(SELECT TOP  1 IdSoTT FROM LOCALSERVER.NhaThuocDB.DBO.zHS_ThuTuThuoc T5 WHERE    T5.IDTHUOC = B.IDTHUOC AND T5.IDKHO = 5 AND dMonth <= GETDATE()  ORDER BY  dMonth DESC) = T4.IdSoTT
                        where B.ISTHUOCBV = 1  AND ISNULL(T4.SoTT, 0) <> -1
						and b.LoaiThuocID=1
                        and b.tenthuoc is not null
						AND ISNULL(B.IsNgungSD,0)=0)ab
                       where slton>0 and dongia>0
						ORDER BY  isnull(isbhyt,0) desc, isnull( isthuocbv,0) desc ,tenthuoc ASC";
            return sql;
            #endregion
        }

        private void Load_Item_Cachdung_DV()
        {
            #region Load Cách dùng
            string sql = @"select idcachdung,tencachdung from Thuoc_CachDung";
            DataTable dtcachdung = DataAcess.Connect.GetTable(sql);
            repositoryItemCachdungDV.NullText = @"Nhập cách dùng";
            repositoryItemCachdungDV.DataSource = dtcachdung;
            repositoryItemCachdungDV.ValueMember = "idcachdung";
            repositoryItemCachdungDV.DisplayMember = "tencachdung";
            repositoryItemCachdungDV.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemCachdungDV.ImmediatePopup = true;
            repositoryItemCachdungDV.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            colIDCD_DV.ColumnEdit = repositoryItemCachdungDV;
            #endregion
        }

        private void Load_Item_DonViDung_DV()
        {
            #region Load Đơn vị dùng
            string sql = @"select Id,TenDVT from Thuoc_DonViTinh";
            DataTable dtDonvidung = DataAcess.Connect.GetTable(sql);
            repositoryItemDVD_Dv.NullText = @"Nhập DVD";
            repositoryItemDVD_Dv.DataSource = dtDonvidung;
            repositoryItemDVD_Dv.ValueMember = "Id";
            repositoryItemDVD_Dv.DisplayMember = "TenDVT";
            repositoryItemDVD_Dv.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemDVD_Dv.ImmediatePopup = true;
            repositoryItemDVD_Dv.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            colIDDVDung_DV.ColumnEdit = repositoryItemDVD_Dv;

            #endregion
        }


        //private void repositoryItemButtonEdit3_ButtonClick_1(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        //{
        //    #region Xóa chẩn đoán sơ bộ
        //    if (MessageBox.Show("Bạn có chắc muốn xóa Chẩn đoán sơ bộ?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        try
        //        {
        //            string id = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, gridView2.Columns["id"]).ToString();
        //            if (id != null && id != "")
        //            {
        //                string delete = "delete chandoansobo where id =" + id;
        //                bool ok = DataAcess.Connect.ExecSQL(delete);
        //                if (ok)
        //                {
        //                    MessageBox.Show("Xóa thành công!");
        //                    Load_CDSB_Gridview();
        //                }
        //            }
        //            else
        //            {
        //                gridView2.DeleteRow(gridView2.FocusedRowHandle);
        //            }
        //        }
        //        catch
        //        {
        //            MessageBox.Show("Ô bạn chọn là ô trống!");
        //        }
        //    }
        //    #endregion
        //}
        //private void gridView2_CellValueChanged_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        //{
        //    #region Kiểm tra trùng mã ICD khi nhập CDSB
        //    try
        //    {
        //        string cls = gridView2.GetRowCellValue(e.RowHandle, colIDICD).ToString();
        //        for (int i = 0; i < gridView2.RowCount - 1; i++)
        //        {
        //            if (e.RowHandle != i)
        //            {
        //                string idcanlamsan = gridView2.GetRowCellValue(i, gridView2.Columns["idicd"]).ToString();
        //                if (cls == idcanlamsan)
        //                {
        //                    MessageBox.Show("Đã có nhập mã ICD này rồi!");
        //                    gridView2.DeleteRow(gridView2.FocusedRowHandle);
        //                    return;
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //    #endregion

        //    #region Click chọn ICD vào gridview CDSB
        //    if (e.Column.FieldName == "MaICD")
        //    {
        //        var value = gridView2.GetRowCellValue(e.RowHandle, e.Column);
        //        string sql = @"select IDICD,MaICD,MoTa from ChanDoanICD where  IDICD='" + value + "'";
        //        DataTable dt = DataAcess.Connect.GetTable(sql);
        //        if (dt != null)
        //        {
        //            gridView2.SetRowCellValue(e.RowHandle, "idicd", dt.Rows[0]["IDICD"].ToString());
        //            // gridView2.SetRowCellValue(e.RowHandle, "MaICD", dt.Rows[0]["MaICD"].ToString());
        //            gridView2.SetRowCellValue(e.RowHandle, "MoTaCD_edit", dt.Rows[0]["MoTa"].ToString());
        //        }
        //    }
        //    #endregion
        //}

       
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
                                            WHERE b.loaiphong = 1 and a.IsActive=1 ";
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

        private void Load_CSL_gridview_new()
        {

            DataTable dtCLS = DataAcess.Connect.GetTable(dt_load_CLS(idkhambenh_new));
            grcCLS.DataSource = dtCLS;

        }
        private void Load_CSLhen_gridview()
        {
            DataTable dtCLShen = DataAcess.Connect.GetTable(dt_load_CLShen(Truyendulieu.idkhambenh));
            grcCLShen.DataSource = dtCLShen;
        }
        private void Load_CSLhen_gridview_new()
        {
            DataTable dtCLShen = DataAcess.Connect.GetTable(dt_load_CLShen(idkhambenh_new));
            grcCLShen.DataSource = dtCLShen;
        }
        public static string dt_load_CLS(string idkhambenh)
        {
            //string IsChuyenPhong = Request.QueryString["IsChuyenPhong"];
            //string idkhambenh = process.getData("idkhambenh");
            //if (idkhambenh != null && idkhambenh != "" && idkhambenh != "0" && IsChuyenPhong == "1")
            //{
            //    DataTable dtTT = DataAcess.Connect.GetTable("SELECT TOP 1 IDCHITIETBENHNHANTOATHUOC FROM CHITIETBENHNHANTOATHUOC WHERE IDKHAMBENH=" + idkhambenh);
            //    if (dtTT != null && dtTT.Rows.Count > 0)
            //        idkhambenh = "";
            //}
            #region Load Cận lâm sàng BS chỉ định
            string sql = @"select 
                                                                cls.idcanlamsan as idbanggiadichvu
                                                                ,cls.idcanlamsan as tendichvu
                                                                ,cls.DonGiaDV as giadichvu
                                                                ,cls.bhtra as giabh
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
                                 where  isnull(cls.dahuy,0)=0 and cls.LoaiKhamID is not null and IDBENHBHDONGTIEN is not null and cls.idkhambenh='" + idkhambenh + "'";
            return sql;
            #endregion
        }
        public static string dt_load_CLShen(string idkhambenh)
        {
            #region Load Cận lâm sàng BS hẹn
            string sql = @"select 
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
                                 where  isnull(cls.dahuy,0)=0 and cls.LoaiKhamID is not null and cls.idkhambenh='" + idkhambenh + "'";
            return sql;
            #endregion
        }

        private void gridView1_CellValueChanged_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
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
            try
            {
                if (e.Column.FieldName == "tendichvu")
                {
                    var value = gridView1.GetRowCellValue(e.RowHandle, e.Column);
                    string sql = @"select idbanggiadichvu,idbanggiadichvu as tendichvu,giadichvu,bhtra as giabh,IsSuDungChoBH,fromdate,IdnhomInBV from banggiadichvu where idbanggiadichvu='" + value + "'";
                    DataTable dtCLS2 = DataAcess.Connect.GetTable(sql);
                    if (dtCLS2 != null)
                    {
                        gridView1.SetRowCellValue(e.RowHandle, "idbanggiadichvu", dtCLS2.Rows[0]["idbanggiadichvu"].ToString());
                        gridView1.SetRowCellValue(e.RowHandle, "giadichvu", dtCLS2.Rows[0]["giadichvu"].ToString());
                        gridView1.SetRowCellValue(e.RowHandle, "soluong", "1");
                        gridView1.SetRowCellValue(e.RowHandle, "giabh", dtCLS2.Rows[0]["giabh"].ToString());
                        gridView1.SetRowCellValue(e.RowHandle, "IsSuDungChoBH", dtCLS2.Rows[0]["IsSuDungChoBH"].ToString());
                        gridView1.SetRowCellValue(e.RowHandle, "IsBHYT_Save", dtCLS2.Rows[0]["IsSuDungChoBH"].ToString());
                        gridView1.SetRowCellValue(e.RowHandle, "fromdate", dtCLS2.Rows[0]["fromdate"].ToString());
                        gridView1.SetRowCellValue(e.RowHandle, "idnhomin", dtCLS2.Rows[0]["IdnhomInBV"].ToString());
                        #region Load Chẩn đoán theo CLS
                        string sql2 = @"SELECT t.idicd,d.MaICD,MoTaCD_edit 
                                    FROM chandoantheocls t
                                    inner join ChanDoanICD d on d.IDICD=t.idicd where idbanggiadichvu='" + value + "'";
                        DataTable dtCLS = DataAcess.Connect.GetTable(sql2);
                        for (int i = 0; i < dtCLS.Rows.Count; i++)
                        {

                            string dataGridViewTextBoxColumn4 = dtCLS.Rows[0]["idicd"].ToString();
                            string dataGridViewTextBoxColumn5 = dtCLS.Rows[0]["maicd"].ToString();
                            string dataGridViewTextBoxColumn6 = dtCLS.Rows[0]["MoTaCD_edit"].ToString();
                            string idcdsb = "";
                            string[] row = { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, idcdsb };
                            dtgvCDSB.Rows.Add(row);
                        }
                        #region kiểm tra trùng chẩn đoán theo thuốc
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
                        #endregion

                        #region kiểm tra HBA1C lần trước
                        if (dtCLS2.Rows[0]["idbanggiadichvu"].ToString() == "10584" || dtCLS2.Rows[0]["idbanggiadichvu"].ToString() == "10281" || dtCLS2.Rows[0]["idbanggiadichvu"].ToString() == "5134")
                        {
                            DataTable dtLuuKB = DataAcess.Connect.GetTable(this.dt_LoadBN());
                            string sql1 = @"select MAX(cls.ngaykham) as ngaykham
                                        from khambenhcanlamsan cls
                                        inner join khambenh kb on kb.idkhambenh=cls.idkhambenh
                                        inner join dangkykham dk on dk.iddangkykham=kb.iddangkykham
                                        inner join benhnhan bn on bn.idbenhnhan=dk.idbenhnhan
                                        inner join BENHNHAN_BHYT bh on bh.IDBENHNHAN_BH=dk.IDBENHNHAN_BH
                                        inner join banggiadichvu bg on bg.idbanggiadichvu=cls.idcanlamsan
                                        where bg.tendichvu like N'%hba1c%'
                                        and bh.sobhyt='" + dtLuuKB.Rows[0]["sobhyt"].ToString() + "'";
                            DataTable ktra = DataAcess.Connect.GetTable(sql1);
                            //  textBox1.Text = ktra.Rows.Count.ToString();
                            if (ktra.Rows[0]["ngaykham"].ToString() != null && ktra.Rows[0]["ngaykham"].ToString() != "" && ktra.Rows[0]["ngaykham"].ToString() != "0" && ktra.Rows[0]["ngaykham"].ToString() != "NULL")
                            {
                                MessageBox.Show("Xét nghiệm HBA1C lần trước:" + ktra.Rows[0]["ngaykham"].ToString());
                            }
                            else return;

                        }
                        #endregion
                    }
                }
            }
            catch { return; }
            #endregion
             
        }
        private void gridView9_CellValueChanged_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
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
            try
            {
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
            }
            catch { return; }
            #endregion
        }

        private void btnXoa_Click_1(object sender, EventArgs e)
        {
            #region Xóa CLS 
            if (MessageBox.Show("Bạn có chắc muốn xóa Cận lâm sàng?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["IdKBCLS"]).ToString();
                    if (id != null && id != "" && id != "0")
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
        private void btnXoahen_Click_1(object sender, EventArgs e)
        {
            #region Xóa CLS hẹn
            if (MessageBox.Show("Bạn có chắc muốn xóa Cận lâm sàng?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView9.GetRowCellValue(gridView9.FocusedRowHandle, gridView9.Columns["IdKBCLS"]).ToString();
                    if (id != null && id != ""&&id != "0")
                    {
                        string delete = "delete khambenhcanlamsanhen where idkhambenhcanlamsanhen =" + id;
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
        //private void gridView3_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        //{
        //    #region Kiểm tra trùng mã ICD khi nhập CDPH
        //    try
        //    {
        //        string cls = gridView3.GetRowCellValue(e.RowHandle, idicdCDPH).ToString();
        //        for (int i = 0; i < gridView3.RowCount - 1; i++)
        //        {
        //            if (e.RowHandle != i)
        //            {
        //                string idcanlamsan = gridView3.GetRowCellValue(i, gridView3.Columns["idicd"]).ToString();
        //                if (cls == idcanlamsan)
        //                {
        //                    MessageBox.Show("Đã có nhập mã ICD này rồi!");
        //                    gridView3.DeleteRow(gridView3.FocusedRowHandle);
        //                    return;
        //                }
        //            }
        //        }
        //    }
        //    catch { }
        //    #endregion

        //    #region Click chọn ICD vào gridview CDPH
        //    if (e.Column.FieldName == "MaICD")
        //    {
        //        var value = gridView3.GetRowCellValue(e.RowHandle, e.Column);
        //        string sql = @"select IDICD,MaICD,MoTa from ChanDoanICD where  IDICD='" + value + "'";
        //        DataTable dt = DataAcess.Connect.GetTable(sql);
        //        if (dt != null)
        //        {
        //            gridView3.SetRowCellValue(e.RowHandle, "idicd", dt.Rows[0]["IDICD"].ToString());
        //            // gridView2.SetRowCellValue(e.RowHandle, "MaICD", dt.Rows[0]["MaICD"].ToString());
        //            gridView3.SetRowCellValue(e.RowHandle, "MoTaCD_edit", dt.Rows[0]["MoTa"].ToString());
        //        }
        //        #region Kiểm tra trùng mã ICD khi nhập CDPH và CD theo thuốc
        //        try
        //        {
        //            string cls = gridView3.GetRowCellValue(e.RowHandle, idicdCDPH).ToString();

        //            for (int j = 0; j < dtgvChanDoan.Rows.Count - 1; j++)
        //            {
        //                string idcanlamsan = dtgvChanDoan.Rows[j].Cells["idicd"].Value.ToString();
        //                if (cls == idcanlamsan)
        //                {
        //                    MessageBox.Show("Đã có nhập mã ICD này rồi!");
        //                    gridView3.DeleteRow(gridView3.FocusedRowHandle);
        //                    return;
        //                }
        //            }
        //        }

        //        catch { }
        //        #endregion
        //    }
        //    #endregion
        //}

        //private void repositoryItembtnXoaCDPH_Click(object sender, EventArgs e)
        //{
        //    #region Xóa chẩn đoán Phối hợp
        //    if (MessageBox.Show("Bạn có chắc muốn xóa Chẩn đoán phối hợp?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //    {
        //        try
        //        {
        //            string id = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, gridView3.Columns["id"]).ToString();
        //            if (id != null && id != "")
        //            {
        //                string delete = "delete chandoanphoihop where id =" + id;
        //                bool ok = DataAcess.Connect.ExecSQL(delete);
        //                if (ok)
        //                {
        //                    MessageBox.Show("Xóa thành công!");
        //                    Load_CDPH_Gridview();
        //                }
        //            }
        //            else
        //            {
        //                gridView3.DeleteRow(gridView3.FocusedRowHandle);
        //            }
        //        }
        //        catch
        //        {
        //            MessageBox.Show("Ô bạn chọn là ô trống!");
        //        }
        //    }
        //    #endregion
        //}

        private void gridView4_CellValueChanged_1(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
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
            try
            {
                if (e.Column.FieldName == "tenthuoc")
                {
                    var value = gridView4.GetRowCellValue(e.RowHandle, e.Column);
                    string sql = @"select * from (SELECT B.IDTHUOC as idthuoc
										,B.TENTHUOC as tenthuoc
										,B.LOAITHUOCID as loaithuocid
										,C.TENDVT as TenDVT
										,B.iddvt
                                        ,B.congthuc as congthuc
                                        ,cd.tencachdung as duongdung
										,cd.idcachdung as idcachdung
										,cd.tencachdung as tencachdung
                                        ,B.sudungchobh as isbhyt
                                        ,B.isthuocbv
                                        ,SLTON= ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUNHAPKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_NHAP=5),0)-ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUXUATKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_XUAT=5 ),0)
                                        , DonGia  = B.GIA_MUA
                                        ,b.ghichu
                                        ,b.LoiDan
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
                        gridView4.SetRowCellValue(e.RowHandle, "ghichu", dt.Rows[0]["ghichu"].ToString());
                        gridView4.SetRowCellValue(e.RowHandle, "IsBHYT_Save", dt.Rows[0]["isbhyt"].ToString());
                        gridView4.SetRowCellValue(e.RowHandle, "issang", 1);
                        gridView4.SetRowCellValue(e.RowHandle, "ischieu", 1);
                        gridView4.SetRowCellValue(e.RowHandle, "ngayuong", 2);
                        gridView4.SetRowCellValue(e.RowHandle, "moilanuong", 1);
                        gridView4.SetRowCellValue(e.RowHandle, "idcachdung", 1);
                        gridView4.SetRowCellValue(e.RowHandle, "iddvdung", 1);
                        DataTable dtTemp = DataAcess.Connect.GetTable(this.dt_LoadBN());
                        string idkb2 = dtTemp.Rows[0]["iddangkykham"].ToString();
                        string sql3 = @"select ct.idthuoc from chitietbenhnhantoathuoc ct
                                    inner join khambenh kb on kb.idkhambenh=ct.idkhambenh
                                    inner join dangkykham dk on dk.iddangkykham=kb.iddangkykham
                                    where dk.iddangkykham='" + idkb2 + @"'
                                    and ct.idthuoc='" + value + "'";
                        DataTable dtthuoc = DataAcess.Connect.GetTable(sql3);
                        if (dtthuoc.Rows.Count > 0)
                        {
                            MessageBox.Show("Hôm nay, thuốc này đã có rồi ở phòng khám khác!");
                            gridView4.DeleteRow(gridView4.FocusedRowHandle);
                        }
                        else return;

                        #region Chẩn đoán kèm theo thuốc
                        string sql2 = @"SELECT t.idicd,d.MaICD,MoTaCD_edit 
                                    FROM chandoantheothuoc t
                                    inner join ChanDoanICD d on d.IDICD=t.idicd where idthuoc='" + value + "'";
                        dt2 = DataAcess.Connect.GetTable(sql2);
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            string idicd = dt2.Rows[i]["idicd"].ToString();
                            string MaICD = dt2.Rows[i]["MaICD"].ToString();
                            string MoTaCD_edit = dt2.Rows[i]["MoTaCD_edit"].ToString();
                            string ID_CDPH = "";
                            string[] row = { idicd, MaICD, MoTaCD_edit, ID_CDPH };
                            dtgvChanDoan.Rows.Add(row);
                            // dtgvChanDoan.AutoResizeColumns();
                            // dtgvChanDoan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                        }
                        #endregion
                        #region kiểm tra trùng chẩn đoán theo thuốc
                        int colNumber = 0;
                        for (int i = 0; i < dtgvChanDoan.Rows.Count - 1; i++)
                        {
                            if (dtgvChanDoan.Rows[i].IsNewRow) continue;
                            string tmp = dtgvChanDoan.Rows[i].Cells[colNumber].Value.ToString();
                            for (int j = dtgvChanDoan.Rows.Count - 1; j > i; j--)
                            {
                                if (dtgvChanDoan.Rows[j].IsNewRow) continue;
                                if (tmp == dtgvChanDoan.Rows[j].Cells[colNumber].Value.ToString())
                                {
                                    dtgvChanDoan.Rows.RemoveAt(j);
                                }
                            }
                        }
                        #endregion

                    }

                }
            }
            catch { return; }
            #endregion
        }

        
        private string dt_LoadBN()
        {
            #region Load bệnh nhân để kiểm tra khám mới hoặc chuyển phòng hoặc chờ cls
            string sql = @"  select ct.idchitietdangkykham,isnull(kb.idkhambenh,0) as idkhambenh,dk.IDKHAMBENH_CHUYEN,kb.TGXuatVien,isnull(kb.IdChuyenPK,0) as IdChuyenPK,bn.mabenhnhan,bn.tenbenhnhan,kb.idkhambenhchuyenphong,bn.idbenhnhan,dk.iddangkykham,ct.idbanggiadichvu,dk.LoaiKhamID,dk.IdBenhBHDongTien,bh.sobhyt,kb.*
                                                    from dangkykham dk
                                                    inner join chitietdangkykham ct on ct.iddangkykham=dk.iddangkykham
													inner join benhnhan bn on bn.idbenhnhan=dk.idbenhnhan
                                                    left join khambenh kb on kb.IdChiTietDangKyKham=ct.IdChiTietDangKyKham
                                                    left join hs_benhnhanbhdongtien dt on dt.id=dk.IdBenhBHDongTien
                                                    left join benhnhan_bhyt bh on bh.idbenhnhan_bh=dk.idbenhnhan_bh
                                                    where ct.idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'";
            return sql;
            #endregion
        }
        public void Load_BNchokham()
        {
            
            DataTable dtLuuKB = DataAcess.Connect.GetTable(this.dt_LoadBN());
            loaikhamID = dtLuuKB.Rows[0]["LoaiKhamID"].ToString();
            idphieutt = dtLuuKB.Rows[0]["IdBenhBHDongTien"].ToString();
            iddangkykham1 = dtLuuKB.Rows[0]["iddangkykham"].ToString();
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
                    btnluu.Text = "Sửa (F1)";
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
                    if (dt.Rows[0]["TGXuatVien"].ToString() != "0" && dt.Rows[0]["TGXuatVien"].ToString() != null && dt.Rows[0]["TGXuatVien"].ToString() != "")
                    {
                        txtNgayxuatkhoa.Text = DateTime.Parse(dt.Rows[0]["TGXuatVien"].ToString()).ToString("yyyy-MM-dd");
                        txtGiorv.Text = dt.Rows[0]["gioravien"].ToString();
                        txtPhutrv.Text = dt.Rows[0]["phutravien"].ToString();
                    }
                    else
                    {
                        txtNgayxuatkhoa.Text = "";
                        txtGiorv.Text = "";
                        txtPhutrv.Text = "";
                    }
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
                    if (dt.Rows[0]["IdkhoaChuyen"].ToString() != "0")
                    {
                        sluKhoa.EditValue = dt.Rows[0]["IdkhoaChuyen"].ToString();
                    }
                    else
                        sluKhoa.EditValue = null;
                    //sluPK.Properties.NullText = "Chọn phòng khám";
                    //sluKhoa.Properties.NullText = "Chọn Khoa chuyển";
                    if (dt.Rows[0]["IdChuyenPK"].ToString() != "0")
                    {
                        sluPK.EditValue = dt.Rows[0]["IdChuyenPK"].ToString();
                    }
                    else
                        sluPK.EditValue = null;
                    txtSovaovien.Text = dt.Rows[0]["SOVAOVIEN1"].ToString();
                    if (dt.Rows[0]["SOVAOVIEN1"].ToString() != "" || dt.Rows[0]["SOVAOVIEN1"].ToString() != null || dt.Rows[0]["SOVAOVIEN1"].ToString() != "0")
                    {
                        btnTaoSo.Enabled = false;

                    }
                    else { btnTaoSo.Enabled = true; }
                    txtSongayratoa.Text = dt.Rows[0]["songayratoa"].ToString();
                    try
                    {
                        dtpkTaikham.Value = DateTime.Parse(dt.Rows[0]["ngayhentaikham"].ToString());
                    }
                    catch
                    {
                        return;
                    }
                    txtPhongKham.Text = dt.Rows[0]["TENPHONG"].ToString();
                    sluCDXD.EditValue = dt.Rows[0]["ketluan"].ToString();
                    txtLoidan.Text = dt.Rows[0]["loidan"].ToString();
                    txtGhichu.Text = dt.Rows[0]["ghichu"].ToString();
                    #endregion
                    //Load_CLS(Truyendulieu.idkhambenh);
                    //Load_CLS_hen(Truyendulieu.idkhambenh);
                    //Load_ToaThuoc(Truyendulieu.idkhambenh);
                    Load_CDSB(Truyendulieu.idkhambenh);
                    Load_CDPH(Truyendulieu.idkhambenh);
                }
            }
        }

        private void btnluu_Click_1(object sender, EventArgs e)
        {
            string huongdieutri = null;
            string isHaveCLS = "0";
            string ISHAVETHUOC = "0";
            string ISHAVETHUOCBH = "0";
            string isravien = "0";
            string mota_CDSB = "";
            string MaICD_CDSB = "";
            //if (gridView1.RowCount > 1) gridView4.RowCount == 1
            if (chkChoveKT.Checked == false && chkChuyenVien.Checked == false && chkKhongKham.Checked == false && chkTieuPhau.Checked == false)
            {
                // if (sluKhoa.EditValue.ToString() == "1")
                if (sluKhoa.Text == "Khám bệnh")
                {
                    huongdieutri = "1";
                    if (gridView1.RowCount > 1)
                    {
                        isHaveCLS = "1";
                    }
                }
                else
                    // if (sluKhoa.EditValue.ToString() == "2" || sluKhoa.EditValue.ToString() == "3" || sluKhoa.EditValue.ToString() == "4" || sluKhoa.EditValue.ToString() == "46" || sluKhoa.EditValue.ToString() == "50" || sluKhoa.EditValue.ToString() == "61" || sluKhoa.EditValue.ToString() == "22")
                    if (sluKhoa.Text != "Khám bệnh" && sluKhoa.Text != "Chọn Khoa chuyển")
                    {
                        huongdieutri = "8";
                        if (gridView1.RowCount > 1)
                        {
                            isHaveCLS = "1";
                        }

                    }
                    else
                        if (sluKhoa.Text == "Chọn Khoa chuyển")
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
                            }
                            else
                                if (gridView1.RowCount > 1 && gridView4.RowCount == 1)
                                {
                                    huongdieutri = "6";
                                    isHaveCLS = "1";
                                }
                                else
                                {
                                    huongdieutri = "5";
                                }
                        }
            }
            else
                if (chkKhongKham.Checked == true)
                {
                    if (gridView1.RowCount > 1 || gridView4.RowCount > 1)
                    {
                        MessageBox.Show("Vui lòng xóa chỉ định CLS hoặc thuốc!");
                    }
                    else
                        huongdieutri = "20";
                }
                else
                    if (chkChoveKT.Checked == true)
                    {
                        huongdieutri = "3";
                        if (gridView1.RowCount > 1)
                        {
                            isHaveCLS = "1";
                        }
                    }
                    else
                        if (chkChuyenVien.Checked == true)
                        {
                            huongdieutri = "4";
                            if (gridView1.RowCount > 1)
                            {
                                isHaveCLS = "1";
                            }
                        }
                        else
                            if (chkTieuPhau.Checked == true)
                            {
                                huongdieutri = "22";
                                if (gridView1.RowCount > 1)
                                {
                                    isHaveCLS = "1";
                                }
                            }
            if (chkRavien.Checked == true)
            {
                isravien = "1";
            }
            else
                isravien = "0";
            for (int i = 0; i < dtgvCDSB.Rows.Count - 1; i++)
            {
                if (dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() != "" && dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() != null && dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() != "0")
                {
                    mota_CDSB += dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn6"].Value.ToString() + ";";
                    MaICD_CDSB += dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() + ";";
                }
            }
            string cdsb = mota_CDSB + "(" + MaICD_CDSB + ")";
            DataTable dtLuuKB2 = DataAcess.Connect.GetTable(this.dt_LoadBN());
            if (Truyendulieu.idkhambenh == "0")
            {
                if (gridView4.RowCount > 1 && chkRavien.Checked == false)
                {
                    MessageBox.Show("Chưa chọn xuất viện");
                }
                else
                {
                    if (sluBacsi.EditValue == null)
                    {
                        MessageBox.Show("Bác sĩ khám bệnh không được để trống!");
                    }
                    else
                    {
                        #region Chuyển phòng không thu phí, nhập viện,ra toa
                        string luuKB = @"insert into khambenh (ngaykham,idbenhnhan,iddangkykham,idbacsi,trieuchung,chandoanbandau,ketluan,huongdieutri
                                                    ,phongkhamchuyenden,dando,ngayhentaikham,idphongkhambenh,idphongchuyenden
                                                    ,IdChiTietDangKyKham,isNoiTru,IdPhong,DichVuKCBID,idchuyenpk,IdKhoa,idkhoachuyen,IsChuyenPhongCoPhi
                                                    ,isxuatvien,PhongID,songayratoa,tgxuatvien ,IsHaveCLS,IsChoVeKT,IsChuyenVien,IsKhongKham,IsBSMoiKham
                                                    ,ishavethuocbh,MoTaCD_edit,IsTieuPhauRoiVe,ghichu,Sysdate)
                                                values(GETDATE()
                                                ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                ,'" + dtLuuKB2.Rows[0]["iddangkykham"].ToString() + @"'
                                                , '" + (sluBacsi.Text == "Nhập Bác sĩ" ? "0" : sluBacsi.EditValue.ToString()) + @"'
                                                ,N'" + (txttrieuchung.Text == "" ? null : txttrieuchung.Text) + @"'    
                                                ,N'" + cdsb + @"'
                                                , N'" + (sluCDXD.Text == "Nhập chẩn đoán" ? "0" : sluCDXD.EditValue.ToString()) + @"'
                                                ,'" + huongdieutri + @"'
                                                ,'" + (sluKhoa.Text == "Chọn Khoa chuyển" ? "0" : sluKhoa.EditValue.ToString()) + @"'
                                                ,N'" + (txtLoidan.Text == "" ? "Null" : txtLoidan.Text) + @"'  
                                                ,'" + dtpkTaikham.Value.ToString("yyyy-MM-dd")+ @"'
                                                ,1
                                               ,'" + (sluPK.Text == "Chọn phòng khám" ? "0" : sluPK.EditValue.ToString()) + @"'
                                                ,'" + dtLuuKB2.Rows[0]["idchitietdangkykham"].ToString() + @"'
                                                ,0
                                                ,'" + Truyendulieu.PhongKhamID + @"'
                                                ,'" + dtLuuKB2.Rows[0]["idbanggiadichvu"].ToString() + @"'
                                                ,'" + (sluPK.Text == "Chọn phòng khám" ? "0" : sluPK.EditValue.ToString()) + @"'
                                                ,1
                                                ,'" + (sluKhoa.Text == "Chọn Khoa chuyển" ? "0" : sluKhoa.EditValue.ToString()) + @"'
                                                ,0
                                                ,'" + isravien + @"'
                                                ,'" + Truyendulieu.PhongKhamID + @"'
                                                ,'" + (txtSongayratoa.Text == "" ? "0" : txtSongayratoa.Text) + @"'    
                                                ," + (txtNgayxuatkhoa.Text == "" ? "Null" : txtNgayxuatkhoa.Text) + @"   
                                                ,'" + isHaveCLS + @"',0,0,0,0
                                                ,'" + ISHAVETHUOC + @"'
                                                ,N'" + (txtCDXD.Text == "" ? "Null" : txtCDXD.Text) + @"'    
                                                ,0
                                                ,N'" + (txtGhichu.Text == "" ? "Null" : txtGhichu.Text) + @"' 
                                                ,GETDATE())";
                        bool okk = DataAcess.Connect.ExecSQL(luuKB);
                        if (okk)
                        {
                            string updateDKKham = @"update dangkykham set idbenhnhanbhdongtien='" + idphieutt + "' where iddangkykham='" + iddangkykham1 + "'";
                            DataTable updatedkk = DataAcess.Connect.GetTable(updateDKKham);
                            string updateCT = @"update chitietdangkykham set dakham=1 where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'";
                            DataTable LuuCT = DataAcess.Connect.GetTable(updateCT);
                            for (int x = 0; x < dtgvCDSB.Rows.Count - 1; x++)
                            {
                                #region Thêm chẩn đoán sơ bộ
                                string insertCDSB = @"insert into chandoansobo (id,idkhambenh,idicd,maicd,MoTaCD_edit) values 
                                                        ((select max(id) from chandoansobo)+1,(select max(idkhambenh) from khambenh where 
                                                        idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                        ,'" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn4"].Value.ToString() + @"'
                                                         ,'" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn5"].Value.ToString() + @"'
                                                         ,N'" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn6"].Value.ToString() + @"'
                                                        )";
                                DataTable luuCDSB = DataAcess.Connect.GetTable(insertCDSB);
                                #endregion
                            }
                            for (int i = 0; i < dtgvChanDoan.Rows.Count - 1; i++)
                            {
                                #region Thêm chẩn đoán phối hợp
                                string insertCDPH = @"insert into chandoanphoihop (idkhambenh,idicd,maicd,MoTaCD_edit) values 
                                                        ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                            ,'" + dtgvChanDoan.Rows[i].Cells["idicd"].Value.ToString() + @"'
                                                            ,'" + dtgvChanDoan.Rows[i].Cells["MaICD"].Value.ToString() + @"'
                                                            ,N'" + dtgvChanDoan.Rows[i].Cells["MoTaCD_edit"].Value.ToString() + @"'                                                               
                                                        )";

                                DataTable luuCDPH = DataAcess.Connect.GetTable(insertCDPH);
                                #endregion
                            }
                            #region Lưu sinh hiệu
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
                            #endregion


                            #region Có nhập cls
                            if (gridView1.RowCount > 1)
                            {
                                string maphieucls = hs_tinhtien.MaPhieuCLS_new();
                                string insertDKCLS = @"insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values
                                                   ('" + maphieucls + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                                bool ok = DataAcess.Connect.ExecSQL(insertDKCLS);
                                if (ok)
                                {
                                    for (int t = 0; t < gridView1.RowCount - 1; t++)
                                    {
                                        string luuCLS = @"insert into khambenhcanlamsan (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan
                                                                                    , maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH
                                                                                    , DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV
                                                                                    , IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                                        values 
                                                        ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                        ,'" + (gridView1.GetRowCellValue(t, gridView1.Columns["idbanggiadichvu"]).ToString() == "" ? "0" : gridView1.GetRowCellValue(t, gridView1.Columns["idbanggiadichvu"]).ToString()) + @"'
                                                        ,'" + sluBacsi.EditValue.ToString() + @"',0
                                                        ,GETDATE()
                                                        ,GETDATE()
                                                        ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                        ,'" + maphieucls + @"'
                                                        ,'" + gridView1.GetRowCellValue(t, gridView1.Columns["soluong"]).ToString() + @"'
                                                        ," + (gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() == "" ? "Null" : "N'" + gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() + @"'") + @"    
                                                        ," + (gridView1.GetRowCellValue(t, gridView1.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView1.GetRowCellValue(t, gridView1.Columns["ghichu"]).ToString() + @"'") + @"                                                 
                                                        ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + @"',0
                                                        ," + (gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() == "" ? "Null" : "N'" + gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() + @"'") + @"    
                                                        ,0
                                                        ,'" + (gridView1.GetRowCellValue(t, gridView1.Columns["IsSuDungChoBH"]).ToString() == "" ? "0" : gridView1.GetRowCellValue(t, gridView1.Columns["IsSuDungChoBH"]).ToString()) + @"'
                                                        ,0," + (gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() == "" ? "Null" : "N'" + gridView1.GetRowCellValue(t, gridView1.Columns["giabh"]).ToString() + @"'") + @"  
                                                        ,'" + gridView1.GetRowCellValue(t, gridView1.Columns["giadichvu"]).ToString() + @"'
                                                        ,(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                        ,'" + gridView1.GetRowCellValue(t, gridView1.Columns["idnhomin"]).ToString() + @"'
                                                        ,'" + (gridView1.GetRowCellValue(t, gridView1.Columns["IsBHYT_Save"]).ToString() == "" ? "0" : gridView1.GetRowCellValue(t, gridView1.Columns["IsBHYT_Save"]).ToString()) + @"'
                                                        ,'" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + "')";
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
                                            ," + (gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() == null ? "Null" : "'" + gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() + @"'") + @"
                                            ," + (gridView9.GetRowCellValue(t, gridView9.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView9.GetRowCellValue(t, gridView9.Columns["ghichu"]).ToString() + @"'") + @" 
                                            ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + @"',0
                                            ,," + (gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() == null ? "Null" : "'" + gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() + @"'") + @"                                                
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["giadichvu"]).ToString() + @"'
                                            ,'" + gridView9.GetRowCellValue(t, gridView9.Columns["IsSuDungChoBH"]).ToString() + @"'
                                            ,0,," + (gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() == null ? "Null" : "'" + gridView9.GetRowCellValue(t, gridView9.Columns["giabh"]).ToString() + @"'") + @"
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
                            if (gridView4.RowCount > 1 || gridView15.RowCount > 1)
                            {
                                string insertBNTT = @"insert into benhnhantoathuoc (idkhambenh,idbacsi,idbenhnhan,ngayratoa) values 
                                                    ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                    ,'" + sluBacsi.EditValue.ToString() + @"'
                                                    ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                    ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "')";
                                bool ook = DataAcess.Connect.ExecSQL(insertBNTT);
                                if (ook)
                                {
                                    #region Lấy thuốc từ Gridview4 insert vào chitietbenhnhantoathuoc
                                    if (gridView4.RowCount > 1)
                                    {
                                        for (int i = 0; i < gridView4.RowCount - 1; i++)
                                        {
                                            string insertCTBNTT = @"insert into chitietbenhnhantoathuoc (idbenhnhantoathuoc,idthuoc,soluongke,ngayuong,moilanuong,ghichu,idkhambenh,idkho,doituongthuocID,idcachdung,iddvdung,iddvt,ischieu,issang,istoi,istrua,ngayratoa,isbhyt_save,slton,isdaxuat,slxuat)
                                                        values 
                                                        ((select max(idbenhnhantoathuoc) from benhnhantoathuoc where idbenhnhan='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["idthuoc"]).ToString() + @"'
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["soluongke"]).ToString() + @"'
                                                        ,N'" + gridView4.GetRowCellValue(i, gridView4.Columns["ngayuong"]).ToString() + @"'
                                                        ,N'" + gridView4.GetRowCellValue(i, gridView4.Columns["moilanuong"]).ToString() + @"'
                                                        ," + (gridView4.GetRowCellValue(i, gridView4.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView4.GetRowCellValue(i, gridView4.Columns["ghichu"]).ToString() + @"'") + @"                                                 
                                                        ,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                        ,'5'
                                                        ,'1'
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
                                                        ,'" + gridView4.GetRowCellValue(i, gridView4.Columns["sldaxuat"]).ToString() + "')";
                                            DataTable luuToa = DataAcess.Connect.GetTable(insertCTBNTT);
                                        }
                                        MessageBox.Show("Đã lưu toa BH thành công");
                                    }
                                    #endregion

                                    #region Toa thuốc dịch vụ (Lấy thuốc từ Gridview15)
                                    if (gridView15.RowCount > 1)
                                    {
                                        for (int y = 0; y < gridView15.RowCount - 1; y++)
                                        {
                                            string insertThuocDV = @"insert into chitietbenhnhantoathuoc_nhathuoc (idbenhnhantoathuoc,idthuoc,soluongke
                                                        ,ngayuong,moilanuong,ghichu,idkhambenh,idkho,doituongthuocID,idcachdung,iddvdung,iddvt
                                                        ,ischieu,issang,istoi,istrua,ngayratoa,isbhyt_save,slton,isdaxuat,slxuat)
                                                        values ((select max(idbenhnhantoathuoc) from benhnhantoathuoc where idbenhnhan='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'))
                                                        ,'" + (gridView15.GetRowCellValue(y, gridView15.Columns["idthuoc"]).ToString() == "" ? "0" : gridView15.GetRowCellValue(y, gridView15.Columns["idthuoc"]).ToString()) + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["soluongke"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["ngayuong"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["moilanuong"]).ToString() + @"'
                                                        ," + (gridView15.GetRowCellValue(y, gridView15.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView15.GetRowCellValue(y, gridView15.Columns["ghichu"]).ToString() + @"'") + @"                                                 
                                                        ,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                        ,'72'
                                                        ,'1'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["idcachdung"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["iddvdung"]).ToString() + @"' 
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["iddvt"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["ischieu"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["issang"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["istoi"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["istrua"]).ToString() + @"'
                                                        ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00") + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["IsBHYT_Save"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["slton"]).ToString() + @"',0
                                                        ,'" + gridView15.GetRowCellValue(y, gridView15.Columns["sldaxuat"]).ToString() + @"')";
                                            DataTable luuThuocDV = DataAcess.Connect.GetTable(insertThuocDV);
                                        }
                                        MessageBox.Show("Lưu toa dịch vụ thành công!");
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                            MessageBox.Show("Thành công");
                            btnluu.Text = "Sửa (F1)";
                            //Load_CDPH_Gridview();
                            //DataTable dt = (DataTable)dtgvChanDoan.DataSource;
                            //if (dt != null) dt.Clear();
                            //dtgvChanDoan.DataSource = dt;
                            //dtgvChanDoan.Rows.Clear();
                        }
                        DataTable dtLuuKB = DataAcess.Connect.GetTable(this.dt_LoadBN());
                        string loaikhamID = dtLuuKB.Rows[0]["LoaiKhamID"].ToString();
                        idkhambenh_new = dtLuuKB.Rows[0]["idkhambenh"].ToString();
                        if (loaikhamID == "1")
                        {
                            bool OK = hs_tinhtien.TinhTien(idphieutt, iddangkykham1, false);
                            if (OK)
                            {
                                MessageBox.Show("tính tiền BH");
                            }
                            else MessageBox.Show("THẤT BẠI");
                        }
                        else
                        {
                            hs_tinhtien.TinhTienDV(idphieutt, iddangkykham1, false);
                            MessageBox.Show("tính tiền DV");
                        }
                        #endregion
                        Load_CDSB(idkhambenh_new);
                        Load_CDPH(idkhambenh_new);
                        Load_CSL_gridview_new();
                        Load_CSLhen_gridview_new();
                        Load_Toathuoc_Gridview_new();
                        Load_ToathuocDV_Gridview_new();
                        clscount = gridView1.RowCount;
                    }
                    if (gridView4.RowCount > 1)
                    {

                        hs_tinhtien.XuatThuoc(idkhambenh_new);

                    }
                }
            }
            else
                if (gridView4.RowCount > 1 && chkRavien.Checked == false)
                {
                    MessageBox.Show("Chưa chọn xuất viện");
                }
                else
                {
                    if (Truyendulieu.idkhambenh != "0")
                    {
                        #region gridview
                        //                    for (int i = 0; i < gridView2.RowCount - 1; i++)
                        //                    {
                        //                        #region thêm chẩn đoán sơ bộ
                        //                        if (gridView2.GetRowCellValue(i, gridView2.Columns["id"]).ToString() == "" && gridView2.GetRowCellValue(i, gridView2.Columns["id"]).ToString() == null && gridView2.GetRowCellValue(i, gridView2.Columns["id"]).ToString() == "0")
                        //                        {
                        //                            string insertCDSB = @"insert into chandoansobo (id,idkhambenh,idicd,maicd,MoTaCD_edit) 
                        //                                                    values 
                        //                                                    ((select max(id) from chandoansobo)+1,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                        //                                                    ,'" + gridView2.GetRowCellValue(i, gridView2.Columns["idicd"]).ToString() + @"'
                        //                                                    ,'" + gridView2.GetRowCellValue(i, gridView2.Columns["MaICD"]).ToString() + @"'
                        //                                                    ,N'" + gridView2.GetRowCellValue(i, gridView2.Columns["MoTaCD_edit"]).ToString() + "')";
                        //                            DataTable luuCDSB = DataAcess.Connect.GetTable(insertCDSB);
                        //                        }
                        //                        #endregion
                        //                    }
                        //                    for (int x = 0; x < gridView3.RowCount - 1; x++)
                        //                    {
                        //                        #region thêm chẩn đoán phối hợp
                        //                        if (gridView3.GetRowCellValue(x, gridView3.Columns["id"]).ToString() == "" && gridView3.GetRowCellValue(x, gridView3.Columns["id"]).ToString() == null && gridView3.GetRowCellValue(x, gridView3.Columns["id"]).ToString() == "0")
                        //                        {
                        //                            string insertCDPH = @"insert into chandoanphoihop (idkhambenh,idicd,maicd,MoTaCD_edit) values 
                        //                                                            ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                        //                                                            ,'" + gridView3.GetRowCellValue(x, gridView3.Columns["idicd"]).ToString() + @"'
                        //                                                            ,'" + gridView3.GetRowCellValue(x, gridView3.Columns["MaICD"]).ToString() + @"'
                        //                                                            ,N'" + gridView3.GetRowCellValue(x, gridView3.Columns["MoTaCD_edit"]).ToString() + "')";
                        //                            DataTable luuCDPH = DataAcess.Connect.GetTable(insertCDPH);
                        //                        }
                        //                        #endregion
                        //                    }
                        //                    for (int j = 0; j < dtgvChanDoan.Rows.Count - 1; j++)
                        //                    {
                        //                        #region thêm chẩn đoán theo thuốc
                        //                        string insertCDTT = @"insert into chandoanphoihop (idkhambenh,idicd,maicd,MoTaCD_edit) values 
                        //                                                            ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                        //                                                            ,'" + dtgvChanDoan.Rows[j].Cells["idicd"].Value.ToString() + @"'
                        //                                                            ,'" + dtgvChanDoan.Rows[j].Cells["MaICD"].Value.ToString() + @"'
                        //                                                            ,N'" + dtgvChanDoan.Rows[j].Cells["MoTaCD_edit"].Value.ToString() + "')";
                        //                        DataTable ok = DataAcess.Connect.GetTable(insertCDTT);
                        //                        #endregion
                        //                    }
                        #endregion
                        for (int x = 0; x < dtgvCDSB.Rows.Count - 1; x++)
                        {
                            #region Thêm chẩn đoán sơ bộ
                            if (dtgvCDSB.Rows[x].Cells["IDCDSB"].Value.ToString() == "" || dtgvCDSB.Rows[x].Cells["IDCDSB"].Value.ToString() == "0" || dtgvCDSB.Rows[x].Cells["IDCDSB"].Value.ToString() == null)
                            {
                                string insertCDSB = @"insert into chandoansobo (id,idkhambenh,idicd,maicd,MoTaCD_edit) values 
                                                        ((select max(id) from chandoansobo)+1,(select max(idkhambenh) from khambenh where 
                                                        idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                        ,'" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn4"].Value.ToString() + @"'
                                                         ,'" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn5"].Value.ToString() + @"'
                                                         ,N'" + dtgvCDSB.Rows[x].Cells["dataGridViewTextBoxColumn6"].Value.ToString() + @"'
                                                        )";
                                DataTable luuCDSB = DataAcess.Connect.GetTable(insertCDSB);
                            }
                            #endregion
                        }
                        for (int i = 0; i < dtgvChanDoan.Rows.Count - 1; i++)
                        {
                            #region Thêm chẩn đoán phối hợp

                            if (dtgvChanDoan.Rows[i].Cells["ID_CDPH"].Value.ToString() == "" || dtgvChanDoan.Rows[i].Cells["ID_CDPH"].Value.ToString() == "0" || dtgvChanDoan.Rows[i].Cells["ID_CDPH"].Value.ToString() == null)
                            {
                                string insertCDPH = @"insert into chandoanphoihop (idkhambenh,idicd,maicd,MoTaCD_edit) values 
                                                        ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                            ,'" + dtgvChanDoan.Rows[i].Cells["idicd"].Value.ToString() + @"'
                                                            ,'" + dtgvChanDoan.Rows[i].Cells["MaICD"].Value.ToString() + @"'
                                                            ,N'" + dtgvChanDoan.Rows[i].Cells["MoTaCD_edit"].Value.ToString() + @"'                                                               
                                                        )";
                                DataTable luuCDPH = DataAcess.Connect.GetTable(insertCDPH);
                            }
                            #endregion
                        }
                        #region update bảng khambenhcanlamsan
                        if (gridView1.RowCount > clscount)
                        {
                            string maphieucls1 = hs_tinhtien.MaPhieuCLS_new();
                            string insertDKCLS1 = "insert into hs_DangKyCLS (MaPhieuCLS,NgayDK,NguoiDK,IDBENHNHAN) values('" + maphieucls1 + "',getdate(),0,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + "')";
                            DataTable dkcls1 = DataAcess.Connect.GetTable(insertDKCLS1);
                            for (int y = 0; y < gridView1.RowCount - 1; y++)
                            {
                                if (gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() == "" || gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() == null || gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() == "0")
                                {
                                    #region Nếu có nhập thêm cận lâm sàng trên Gridview1
                                    string luuCLS = @"insert into khambenhcanlamsan (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
                                                values ((select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + @"')
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["idbanggiadichvu"]).ToString() + @"'
                                                ,'" + sluBacsi.EditValue.ToString() + @"',0
                                                ,'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                                                ,'" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + @"'
                                                ,'" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"'
                                                ,'" + maphieucls1 + @"'
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["soluong"]).ToString() + @"'
                                                ," + (gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() == null ? "Null" : "'" + gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() + @"'") + @"
                                                ," + (gridView1.GetRowCellValue(y, gridView1.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView1.GetRowCellValue(y, gridView1.Columns["ghichu"]).ToString() + @"'") + @" 
                                                ,'" + dtLuuKB2.Rows[0]["LoaiKhamID"].ToString() + @"',0
                                                ," + (gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() == null ? "Null" : "'" + gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() + @"'") + @"
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["giadichvu"]).ToString() + @"'
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["IsSuDungChoBH"]).ToString() + @"'
                                                ,0," + (gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() == null ? "Null" : "'" + gridView1.GetRowCellValue(y, gridView1.Columns["giabh"]).ToString() + @"'") + @"
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["giadichvu"]).ToString() + @"'
                                                ,(select MAX(IdDangKyCLS) from hs_DangKyCLS where IDBENHNHAN='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["idnhomin"]).ToString() + @"'
                                                ,'" + gridView1.GetRowCellValue(y, gridView1.Columns["IsBHYT_Save"]).ToString() + @"'
                                                ,'" + dtLuuKB2.Rows[0]["IDBENHBHDONGTIEN"].ToString() + @"')";
                                    DataTable LuuCLS = DataAcess.Connect.GetTable(luuCLS);
                                    #endregion
                                }
                            }
                        }
                        for (int y = 0; y < gridView1.RowCount - 1; y++)
                        {

                            if (gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() != "" && gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() != null && gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() != "0")
                            {
                                #region có sửa trên Gridview1 thì cập nhật lại
                                string updateCLS = @"update khambenhcanlamsan set 
                                                        soluong='" + gridView1.GetRowCellValue(y, gridView1.Columns["soluong"]).ToString() + @"'
                                                        ,isbhyt_save='" + gridView1.GetRowCellValue(y, gridView1.Columns["IsBHYT_Save"]).ToString() + @"'
                                                        ,ghichu= " + (gridView1.GetRowCellValue(y, gridView1.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView1.GetRowCellValue(y, gridView1.Columns["ghichu"]).ToString() + @"'") + @" 
                                                        ,idcanlamsan='" + gridView1.GetRowCellValue(y, gridView1.Columns["idbanggiadichvu"]).ToString() + @"'
                                        where idkhambenhcanlamsan='" + gridView1.GetRowCellValue(y, gridView1.Columns["IdKBCLS"]).ToString() + "'";
                                DataTable editCLS = DataAcess.Connect.GetTable(updateCLS);
                                #endregion
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

                                string updateCLSHen = @"update khambenhcanlamsanhen set 
                                                        soluong='" + gridView9.GetRowCellValue(y, gridView9.Columns["soluong"]).ToString() + @"'
                                                        ,isbhyt_save='" + gridView9.GetRowCellValue(y, gridView9.Columns["IsBHYT_Save"]).ToString() + @"'
                                                        ,ghichu=N'" + (gridView9.GetRowCellValue(y, gridView9.Columns["ghichu"]).ToString() == "" ? "Null" : gridView9.GetRowCellValue(y, gridView9.Columns["ghichu"]).ToString()) + @"'
                                                        ,idcanlamsan='" + gridView9.GetRowCellValue(y, gridView9.Columns["idbanggiadichvu"]).ToString() + @"'
                                                  where idkhambenhcanlamsanhen='" + gridView9.GetRowCellValue(y, gridView9.Columns["IdKBCLS"]).ToString() + "'";
                                DataTable editCLShen = DataAcess.Connect.GetTable(updateCLSHen);

                            }
                            else
                            {
                                string luuCLSHen = @"insert into khambenhcanlamsanhen (idkhambenh,idcanlamsan, idbacsi,dathu, ngaythu, ngaykham, idbenhnhan, maphieuCLS, soluong, BHTra, GhiChu, LoaiKhamID, BNTongPhaiTra, DonGiaBH, DonGiaDV, IsBHYT, PhuThuBH, ThanhTienBH, ThanhTienDV, IDDANGKYCLS, IdnhomInBV, IsBHYT_Save, IDBENHBHDONGTIEN) 
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
                                DataTable LuuCLSHen = DataAcess.Connect.GetTable(luuCLSHen);
                            }
                        }
                        #endregion


                        for (int z = 0; z < gridView4.RowCount - 1; z++)
                        {
                            #region update bảng chitietbenhnhantoathuoc
                            if (gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() != "" && gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() != "0" && gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() != null)
                            {
                                string updateThuoc = @"update chitietbenhnhantoathuoc set 
                                                            soluongke='" + gridView4.GetRowCellValue(z, gridView4.Columns["soluongke"]).ToString() + @"'
                                                            ,ngayuong=N'" + gridView4.GetRowCellValue(z, gridView4.Columns["ngayuong"]).ToString() + @"'
                                                            ,moilanuong=N'" + gridView4.GetRowCellValue(z, gridView4.Columns["moilanuong"]).ToString() + @"'
                                                            ,ghichu=" + (gridView4.GetRowCellValue(z, gridView4.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView4.GetRowCellValue(z, gridView4.Columns["ghichu"]).ToString() + @"'") + @"                                                 
                                                            ,idcachdung='" + gridView4.GetRowCellValue(z, gridView4.Columns["idcachdung"]).ToString() + @"'
                                                            ,iddvdung='" + gridView4.GetRowCellValue(z, gridView4.Columns["iddvdung"]).ToString() + @"'
                                                            ,iddvt='" + gridView4.GetRowCellValue(z, gridView4.Columns["iddvt"]).ToString() + @"'
                                                            ,ischieu='" + gridView4.GetRowCellValue(z, gridView4.Columns["ischieu"]).ToString() + @"'
                                                            ,issang='" + gridView4.GetRowCellValue(z, gridView4.Columns["issang"]).ToString() + @"'
                                                            ,istoi='" + gridView4.GetRowCellValue(z, gridView4.Columns["istoi"]).ToString() + @"'
                                                            ,istrua='" + gridView4.GetRowCellValue(z, gridView4.Columns["istrua"]).ToString() + @"'
                                                            ,IsBHYT_Save='" + gridView4.GetRowCellValue(z, gridView4.Columns["IsBHYT_Save"]).ToString() + @"' 
                                                        where idchitietbenhnhantoathuoc='" + gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() + "'";
                                DataTable editThuoc = DataAcess.Connect.GetTable(updateThuoc);
                            }
                            #endregion

                            #region nếu nhập thêm thuốc thì insert
                            if (gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() == "" || gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() == null || gridView4.GetRowCellValue(z, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString() == "0")
                            {
                                string insertCTBNTT = @"insert into chitietbenhnhantoathuoc (idbenhnhantoathuoc,idthuoc,soluongke,ngayuong,moilanuong,ghichu,idkhambenh,idkho,doituongthuocID,idcachdung,iddvdung,iddvt,ischieu,issang,istoi,istrua,ngayratoa,isbhyt_save,slton,isdaxuat,slxuat)
                                                            values ((select max(idbenhnhantoathuoc) from benhnhantoathuoc where idbenhnhan='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                            ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["idthuoc"]).ToString() + @"'
                                                            ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["soluongke"]).ToString() + @"'
                                                            ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["ngayuong"]).ToString() + @"'
                                                            ,'" + gridView4.GetRowCellValue(z, gridView4.Columns["moilanuong"]).ToString() + @"'
                                                            ," + (gridView4.GetRowCellValue(z, gridView4.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView4.GetRowCellValue(z, gridView4.Columns["ghichu"]).ToString() + @"'") + @" 
                                                            ,'" + Truyendulieu.idkhambenh + @"'
                                                            ,5,1
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
                        for (int j = 0; j < gridView15.RowCount - 1; j++)
                        {
                            #region Thêm thuốc Dịch vụ nếu có nhập thêm
                            if (gridView15.GetRowCellValue(j, gridView15.Columns["idchitietbenhnhantoathuoc"]).ToString() == "" || gridView15.GetRowCellValue(j, gridView15.Columns["idchitietbenhnhantoathuoc"]).ToString() == null || gridView15.GetRowCellValue(j, gridView15.Columns["idchitietbenhnhantoathuoc"]).ToString() == "0")
                            {
                                string insertThuocDV2 = @"insert into chitietbenhnhantoathuoc_nhathuoc (idbenhnhantoathuoc,idthuoc,soluongke
                                                        ,ngayuong,moilanuong,ghichu,idkhambenh,idkho,doituongthuocID,idcachdung,iddvdung,iddvt,ischieu,issang,istoi,istrua,
                                                        ngayratoa,slton,slxuat)
                                                        values ((select max(idbenhnhantoathuoc) from benhnhantoathuoc where idbenhnhan='" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"')
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["idthuoc"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["soluongke"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["ngayuong"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["moilanuong"]).ToString() + @"'
                                                        ," + (gridView15.GetRowCellValue(j, gridView15.Columns["ghichu"]).ToString() == "" ? "Null" : gridView15.GetRowCellValue(j, gridView15.Columns["ghichu"]).ToString()) + @"                                                 
                                                        ,'" + Truyendulieu.idkhambenh + @"'
                                                        ,'72'
                                                        ,'1'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["idcachdung"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["iddvdung"]).ToString() + @"' 
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["iddvt"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["ischieu"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["issang"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["istoi"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["istrua"]).ToString() + @"'
                                                        ,getdate()
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["slton"]).ToString() + @"'
                                                        ,'" + gridView15.GetRowCellValue(j, gridView15.Columns["sldaxuat"]).ToString() + @"')";
                                DataTable luuThuocDV2 = DataAcess.Connect.GetTable(insertThuocDV2);

                            }
                            else
                            {
                                string updateThuocDV = @"update chitietbenhnhantoathuoc_nhathuoc set 
                                                            soluongke='" + gridView15.GetRowCellValue(j, gridView15.Columns["soluongke"]).ToString() + @"'
                                                            ,ngayuong='" + gridView15.GetRowCellValue(j, gridView15.Columns["ngayuong"]).ToString() + @"'
                                                            ,moilanuong='" + gridView15.GetRowCellValue(j, gridView15.Columns["moilanuong"]).ToString() + @"'
                                                            ,ghichu=" + (gridView15.GetRowCellValue(j, gridView15.Columns["ghichu"]).ToString() == "" ? "Null" : "N'" + gridView15.GetRowCellValue(j, gridView15.Columns["ghichu"]).ToString() + @"'") + @"                                                 
                                                            ,idcachdung='" + gridView15.GetRowCellValue(j, gridView15.Columns["idcachdung"]).ToString() + @"'
                                                            ,iddvdung='" + gridView15.GetRowCellValue(j, gridView15.Columns["iddvdung"]).ToString() + @"'
                                                            ,ischieu='" + gridView15.GetRowCellValue(j, gridView15.Columns["ischieu"]).ToString() + @"'
                                                            ,issang='" + gridView15.GetRowCellValue(j, gridView15.Columns["issang"]).ToString() + @"'
                                                            ,istoi='" + gridView15.GetRowCellValue(j, gridView15.Columns["istoi"]).ToString() + @"'
                                                            ,istrua='" + gridView15.GetRowCellValue(j, gridView15.Columns["istrua"]).ToString() + @"'
                                                        where idchitietbenhnhantoathuoc='" + gridView15.GetRowCellValue(j, gridView15.Columns["idchitietbenhnhantoathuoc"]).ToString() + "'";
                                DataTable editThuocDV = DataAcess.Connect.GetTable(updateThuocDV);

                            }

                            #endregion
                        }

                        #region Update lại table KhamBenh
                        string updateKB = @"update khambenh set 
                                                        idbacsi='" + (sluBacsi.Text == "Nhập Bác sĩ" ? "0" : sluBacsi.EditValue) + @"'
                                                        ,chandoanbandau=N'" + cdsb + @"'
                                                        ,ketluan='" + (sluCDXD.Text == "Nhập chẩn đoán" ? "0" : sluCDXD.EditValue) + @"'
                                                        ,huongdieutri='" + huongdieutri + @"'
                                                        ,phongkhamchuyenden='" + (sluKhoa.Text == "Chọn Khoa chuyển" ? "0" : sluKhoa.EditValue) + @"'
                                                        ,dando=N'" + txtLoidan.Text + @"'
                                                        ,ngayhentaikham='" + dtpkTaikham.Value.ToString("yyyy-MM-dd") + @"'
                                                        ,idphongchuyenden='" + (sluPK.Text == "Chọn phòng khám" ? "0" : sluPK.EditValue) + @"'
                                                        ,isNoiTru='" + isravien + @"'
                                                        ,idphong='" + Truyendulieu.PhongKhamID + @"'
                                                        ,idchuyenpk='" + (sluPK.Text == "Chọn phòng khám" ? "0" : sluPK.EditValue) + @"'
                                                        ,idkhoachuyen='" + (sluKhoa.Text == "Chọn Khoa chuyển" ? "0" : sluKhoa.EditValue) + @"'
                                                        ,IsChuyenPhongCoPhi='" + chkThuphi.Checked + @"'
                                                        ,isxuatvien='" + chkRavien.Checked + @"'
                                                        ,PhongID='" + Truyendulieu.PhongKhamID + @"'
                                                        ,songayratoa='" + txtSongayratoa.Text + @"'
                                                        ,tgxuatvien='" + txtNgayxuatkhoa.Text + @"'
                                                        ,IsHaveCLS='" + isHaveCLS + @"'
                                                        ,IsChoVeKT='" + chkChoveKT.Checked + @"'
                                                        ,IsChuyenVien='" + chkChuyenVien.Checked + @"'
                                                        ,IsKhongKham='" + chkKhongKham.Checked + @"'
                                                        ,idbacsi2='" + (gluBacSi2.Text == "Nhập Bác sĩ 2" ? "0" : gluBacSi2.EditValue) + @"'
                                                        ,IsBSMoiKham='" + chkMoiKham.Checked + @"'
                                                        ,ishavethuoc='" + ISHAVETHUOC + @"'
                                                        ,ishavethuocbh='" + ISHAVETHUOCBH + @"'
                                                        ,MoTaCD_edit=N'" + txtCDXD.Text + @"'
                                                        ,IsTieuPhauRoiVe='" + chkTieuPhau.Checked + @"'
                                                        ,ghichu=N'" + txtGhichu.Text + @"'    
                                              where idkhambenh='" + Truyendulieu.idkhambenh + "'";
                        DataTable LuuKB = DataAcess.Connect.GetTable(updateKB);
                        #endregion
                        MessageBox.Show("Thành công");
                        MessageBox.Show(huongdieutri);
                        // Load_CDPH_Gridview();
                        //DataTable dt = (DataTable)dtgvChanDoan.DataSource;
                        //if (dt != null) dt.Clear();
                        //dtgvChanDoan.DataSource = dt;
                        //dtgvChanDoan.Rows.Clear();
                        #region update lại Sinh hiệu
                        string updateSinhHieu = @"update sinhhieu set mach=" + (this.txtMach.Text == "" ? "Null" : "N'" + this.txtMach.Text + @"'") + @"
                                                                ,nhietdo=" + (this.txtNhietDo.Text == "" ? "Null" : "N'" + this.txtNhietDo.Text + @"'") + @"
                                                                ,huyetap1=" + (this.txtHuyetAp.Text == "" ? "Null" : "N'" + this.txtHuyetAp.Text + @"'") + @"
                                                                ,huyetap2=" + (this.txtHuyetAp2.Text == "" ? "Null" : "N'" + this.txtHuyetAp2.Text + @"'") + @"
                                                                ,nhiptho=" + (this.txtNhipTho.Text == "" ? "Null" : "N'" + this.txtNhipTho.Text + @"'") + @"
                                                                ,chieucao=" + (this.txtChieuCao.Text == "" ? "Null" : "N'" + this.txtChieuCao.Text + @"'") + @"
                                                                ,cannang=" + (this.txtCanNang.Text == "" ? "Null" : "N'" + this.txtCanNang.Text + @"'") + @"
                                                                ,BMI=" + (this.txtBMI.Text == "" ? "Null" : "N'" + this.txtBMI.Text + @"'") + @"
                                                                    where IdKhamBenh='" + Truyendulieu.idkhambenh + "' ";
                        DataTable dtUpdateSH = DataAcess.Connect.GetTable(updateSinhHieu);

                        #endregion

                        #region Tính tiền lại
                        DataTable dtLuuKB = DataAcess.Connect.GetTable(this.dt_LoadBN());
                        string loaikhamID = dtLuuKB.Rows[0]["LoaiKhamID"].ToString();
                        if (loaikhamID == "1")
                        {
                            bool OK = hs_tinhtien.TinhTien(idphieutt, iddangkykham1, false);
                            if (OK)
                            {
                                MessageBox.Show("tính tiền BH");
                            }
                            else MessageBox.Show("THẤT BẠI");
                        }
                        else
                        {
                            hs_tinhtien.TinhTienDV(idphieutt, iddangkykham1, false);
                            MessageBox.Show("tính tiền DV");
                        }
                        #endregion
                        Load_CDSB(Truyendulieu.idkhambenh);
                        Load_CDPH(Truyendulieu.idkhambenh);
                        Load_CSL_gridview();
                        Load_CSLhen_gridview();
                        Load_Toathuoc_Gridview();
                        Load_ToathuocDV_Gridview();
                        clscount = gridView1.RowCount;
                        if (gridView4.RowCount > 1)
                        {

                            hs_tinhtien.XuatThuoc(Truyendulieu.idkhambenh);

                        }
                    }
                }
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

        private void btninthuocbh_Click_1(object sender, EventArgs e)
        {
            if (Truyendulieu.idkhambenh == "0" || Truyendulieu.idkhambenh == null || Truyendulieu.idkhambenh == "")
            {
                Truyendulieu.idkhambenh = idkhambenh_new;
            }
            frmToaThuocBH frmpTT = new frmToaThuocBH();
            frmpTT.Show();
        }

        private void btninthuocdv_Click_1(object sender, EventArgs e)
        {
            if (Truyendulieu.idkhambenh == "0" || Truyendulieu.idkhambenh == null || Truyendulieu.idkhambenh == "")
            {
                Truyendulieu.idkhambenh = idkhambenh_new;
            }
            frmToaThuocDV frmpTTDV = new frmToaThuocDV();
            frmpTTDV.Show();
        }

        private void btninclsbh_Click_1(object sender, EventArgs e)
        {
            if (Truyendulieu.idkhambenh == "0" || Truyendulieu.idkhambenh == null || Truyendulieu.idkhambenh == "")
            {
                Truyendulieu.idkhambenh = idkhambenh_new;
            }
            string sql = @"SELECT	 
		            	DOITUONG=(CASE WHEN DKK.LOAIKHAMID<>1 THEN NULL ELSE (CASE WHEN DKK.LOAIKHAMID=1 AND  ISNULL(B.ISSUDUNGCHOBH,0)=1  AND  ISNULL(A.ISBHYT_SAVE,0)=1 THEN N'BHYT' ELSE N'DỊCH VỤ' END) END)             
		                        ,TENCHIDINH= case when isnull(a.ghichu,'')<> '' then B.TENDICHVU +' ('+ a.ghichu +')' else B.TENDICHVU end
		                        ,SL=ISNULL(A.SOLUONG,0)
		                        ,CHANDOAN=ISNULL(C.CHANDOANBANDAU,DBO.[nvk_ListMoTaChanDoan_1KhamBenh](C.IDKHAMBENH))
		                        ,C.MAPHIEUKHAM
		                        ,C.NGAYKHAM
                                ,E.TENPHONGKHAMBENH
                                ,TENNHOM='(' + REPLACE( TENNHOMCD,N'DỊCH VỤ',N'DỊCH VỤ KỸ THUẬT') + (CASE WHEN DKK.LOAIKHAMID<>1 THEN  ''   ELSE ( CASE WHEN ISNULL(B.ISSUDUNGCHOBH,0)=1  AND  ISNULL(A.ISBHYT_SAVE,0)=1 THEN N'-BHYT' ELSE N'-DỊCH VỤ' END ) END )+')'
                                ,HOTENBS=G.TENBACSI
                                ,MaVach=CONVERT(IMAGE,NULL)
                                ,SH.MACH
                                ,SH.HUYETAP1
                                ,SH.HUYETAP2
                                ,nvk_phong= (CASE WHEN ISNULL(C.PHONGID,0)<>0 THEN DBO.HS_TENPHONG(C.PHONGID) ELSE    isnull(
								    (select top 1 tenphong from benhnhannoitru nn inner join kb_phong pp on pp.id=nn.idphongnoitru  where idchitietdangkykham =c.idchitietdangkykham order by ngayvaovien desc)
								    ,0
								        ) END)
                                ,c.idphongkhambenh,c.idchitietdangkykham
                                ,A.MAPHIEUCLS
								,d.tenbenhnhan
								,(CASE WHEN d.gioiTinh='0' THEN 'Nam' ELSE N'Nữ' END) as gioitinh
								,d.ngaysinh
								,d.diachi
								,bhyt.sobhyt
								,dkk.LoaiKhamID
								,CONVERT(varchar,bhyt.ngaybatdau,103) as ngaybatdau
								,CONVERT(varchar,bhyt.ngayhethan,103) as ngayhethan
								,ndk.TENNOIDANGKY 
                                ,bhyt.IsDungTuyen
								,bhyt.IsCapCuu
                                ,ngt.TENNOIDANGKY as NoiGT
                        FROM KHAMBENHCANLAMSAN A
                        LEFT JOIN BANGGIADICHVU B ON A.IDCANLAMSAN=B.IDBANGGIADICHVU
                        LEFT JOIN KHAMBENH C ON A.IDKHAMBENH=C.IDKHAMBENH
                        LEFT JOIN DANGKYKHAM DKK ON C.IDDANGKYKHAM=DKK.IDDANGKYKHAM
						left join BENHNHAN_BHYT bhyt on bhyt.IDBENHNHAN_BH=dkk.IDBENHNHAN_BH
						left join KB_NOIDANGKYKB ndk on ndk.IDNOIDANGKY=bhyt.IdNoiDangKyBH
                        left join KB_NOIDANGKYKB ngt on ngt.IDNOIDANGKY=bhyt.IdNoiGioiThieu
                        LEFT JOIN BENHNHAN D ON C.IDBENHNHAN=D.IDBENHNHAN                       
                        LEFT JOIN PHONGKHAMBENH E ON E.IDPHONGKHAMBENH=ISNULL(C.IDKHOA,C.IDPHONGKHAMBENH)
                        LEFT JOIN PHONGKHAMBENH F ON F.IDPHONGKHAMBENH=B.IDPHONGKHAMBENH 
                        LEFT JOIN BACSI G ON C.IDBACSI=G.IDBACSI
                        LEFT JOIN SINHHIEU SH ON C.IDKHAMBENH=SH.IDKHAMBENH
                     WHERE  ISNULL(A.dahuy,0)=0 AND  A.IDKHAMBENH='" + Truyendulieu.idkhambenh + "'";
            DataTable dtmavach = DataAcess.Connect.GetTable(sql);
            if (dtmavach == null || dtmavach.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy bệnh nhân");
                return;
            }
            else
            {
                frmRptCLS frmp = new frmRptCLS();
                frmp.Show();
            }
           
        }

        private void btninbv01_Click_1(object sender, EventArgs e)
        {
            Truyendulieu.idphieutt = idphieutt;
            frmBV01 frm01 = new frmBV01();
            frm01.Show();
        }

        private void btndelete_Click_1(object sender, EventArgs e)
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

        private void btninclsdv_Click_1(object sender, EventArgs e)
        {
            DataTable dtLuuKB = DataAcess.Connect.GetTable(this.dt_LoadBN());
            string loaikhamID = dtLuuKB.Rows[0]["LoaiKhamID"].ToString();
            if (loaikhamID == "1")
            {
              bool OK=hs_tinhtien.TinhTien(idphieutt, iddangkykham1, false);
              if (OK)
              {
                  MessageBox.Show("tính tiền BH");
              }
              else MessageBox.Show("THẤT BẠI");
            }
            else
            {
                hs_tinhtien.TinhTienDV(idphieutt, iddangkykham1, false);
                MessageBox.Show("tính tiền DV");
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            PhieuHen frm01 = new PhieuHen();
            frm01.Show();
        }

        private void dtgvChanDoan_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            #region Xóa chẩn đoán phối hợp trên datagridview
            int r = dtgvChanDoan.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvChanDoan.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaCd")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvChanDoan.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (oneCell.Selected)
                                {
                                    if (dtgvChanDoan.Rows[r].Cells["ID_CDPH"].Value.ToString() != "" || dtgvChanDoan.Rows[r].Cells["ID_CDPH"].Value.ToString() != null || dtgvChanDoan.Rows[r].Cells["ID_CDPH"].Value.ToString() != "0")
                                    {
                                        if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Chẩn đoán phối hợp", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                        {
                                            string sql = "delete chandoanphoihop where id='" + dtgvChanDoan.Rows[r].Cells["ID_CDPH"].Value.ToString() + "'";
                                            DataTable xoacdsb = DataAcess.Connect.GetTable(sql);
                                            dtgvChanDoan.Rows.RemoveAt(oneCell.RowIndex);
                                        }
                                    }
                                    else
                                    {
                                        dtgvChanDoan.Rows.RemoveAt(oneCell.RowIndex);
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
            #endregion
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }
        private void chkRavien_Click_1(object sender, EventArgs e)
        {
            if (chkRavien.Checked == true)
            {
                txtNgayxuatkhoa.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtGiorv.Text = DateTime.Now.ToString("hh");
                txtPhutrv.Text = DateTime.Now.ToString("mm");
            }
            else
            {
                txtNgayxuatkhoa.Text = "";
                txtGiorv.Text = "";
                txtPhutrv.Text = "";
            }
        }

        private void btnThemCDSB_Click_1(object sender, EventArgs e)
        {
            #region Thêm chẩn đoán sơ bộ
            try
            {
                
                string sql=@"select idicd,MaICD,MoTa from ChanDoanICD where IDICD='" + gluCDSobo.EditValue.ToString() + "'";
                DataTable dtICD_CDSB = DataAcess.Connect.GetTable(sql);
                string dataGridViewTextBoxColumn4 = dtICD_CDSB.Rows[0]["idicd"].ToString();
                string dataGridViewTextBoxColumn5 = dtICD_CDSB.Rows[0]["maicd"].ToString();
                string dataGridViewTextBoxColumn6 = txtCDSB.Text; //dtICD_CDSB.Rows[0]["MoTa"].ToString();
                string idcdsb = "";
                string[] row = { dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, idcdsb };
                //dtgvCDSB.Rows.Add(row);
                //for (int i = 0; i < dtICD_CDSB.Rows.Count; i++)
                //{
                //    dtICD_CDSB.Rows[i]["STT"] = i + 1;

                //}
               // string[] row = { STT,dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, idcdsb };
                dtgvCDSB.Rows.Add(row);
               // dtgvCDSB.AutoResizeColumns();
               // dtgvCDSB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
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

        private void gluCDSobo_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string maICD1 = gluCDSobo.EditValue.ToString();
                string sql = "SELECT mota FROM dbo.ChanDoanICD where idicd= '" + maICD1 + "'";
                DataTable layMota = DataAcess.Connect.GetTable(sql);
                txtCDSB.Text = layMota.Rows[0]["mota"].ToString();
            }
            catch
            {
                return;
            }
        }

        #region Load Chẩn đoán Sơ bộ lên SearchLookup
        public void Load_ChanDoanSoBo()
        {
            DataTable dtChandoanSB = DataAcess.Connect.GetTable(GetData.LoadICD10());
            gluCDSobo.Properties.DataSource = dtChandoanSB;
            gluCDSobo.Properties.DisplayMember = "MaICD";
            gluCDSobo.Properties.ValueMember = "IDICD";
            gluCDSobo.Properties.NullText = "Mã ICD";
            gluCDSobo.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            gluCDSobo.Properties.ImmediatePopup = true;
            gluCDSobo.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

        }
        #endregion

        private void dtgvCDSB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            #region Xóa chẩn đoán sơ bộ trên datagridview
            int r = dtgvCDSB.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCDSB.Columns[e.ColumnIndex].Name;
                if (command == "btnXoaCDSB1")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvCDSB.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != "" || dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != null || dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != "0")
                                {
                                    if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Chẩn đoán sơ bộ", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                    {
                                        string sql = "delete chandoansobo where id='" + dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() + "'";
                                        DataTable xoacdsb = DataAcess.Connect.GetTable(sql);
                                        dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                }
                                else
                                {
                                    dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
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
            #endregion
        }

        public void Load_CDSB(string idkhambenh)
        {
            #region Load chẩn đoán sơ bộ theo ID khám bệnh
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
           // dtgvCDSB.AutoResizeColumns();
           // dtgvCDSB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            #endregion
        }

        public void TongHop_CDSB(string idkhambenh)
        {
            #region Load chẩn đoán sơ bộ theo ID khám bệnh
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
                    string idicd = luuCDSB.Rows[x]["idicd"].ToString();
                    string MaICD = luuCDSB.Rows[x]["maicd"].ToString();
                    string MoTaCD_edit = luuCDSB.Rows[x]["MoTa"].ToString();
                    string ID_CDPH = "";
                    string[] row = { idicd, MaICD, MoTaCD_edit, ID_CDPH };
                    dtgvChanDoan.Rows.Add(row);
                    int colNumber = 0;
                    for (int i = 0; i < dtgvChanDoan.Rows.Count; i++)
                    {
                        if (dtgvChanDoan.Rows[i].IsNewRow) continue;
                        string tmp = dtgvChanDoan.Rows[i].Cells[colNumber].Value.ToString();
                        for (int j = dtgvChanDoan.Rows.Count - 1; j > i; j--)
                        {
                            if (dtgvChanDoan.Rows[j].IsNewRow) continue;
                            if (tmp == dtgvChanDoan.Rows[j].Cells[colNumber].Value.ToString())
                            {
                                dtgvChanDoan.Rows.RemoveAt(j);
                            }
                        }
                    }
                }
            }
            // dtgvCDSB.AutoResizeColumns();
            // dtgvCDSB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            #endregion
        }


        public void Load_CDPH(string idkhambenh)
        {
            DataTable luuCDPH = DataAcess.Connect.GetTable(GetData.Load_CDPH(idkhambenh));
            if (luuCDPH == null)
            {
                MessageBox.Show("Không có Chẩn đoán phối hợp!");
                return;
            }
            else
            {
                for (int t = 0; t < luuCDPH.Rows.Count; t++)
                {
                    string idicd = luuCDPH.Rows[t]["id_ph"].ToString();
                    string MaICD = luuCDPH.Rows[t]["maicd_ph"].ToString();
                    string MoTaCD_edit = luuCDPH.Rows[t]["MoTa_ph"].ToString();
                    string ID_CDPH = luuCDPH.Rows[t]["id"].ToString();
                    string[] row = { idicd, MaICD, MoTaCD_edit, ID_CDPH };
                    dtgvChanDoan.Rows.Add(row);
                    int colNumber = 0;
                    for (int i = 0; i < dtgvChanDoan.Rows.Count; i++)
                    {
                        if (dtgvChanDoan.Rows[i].IsNewRow) continue;
                        string tmp = dtgvChanDoan.Rows[i].Cells[colNumber].Value.ToString();
                        for (int j = dtgvChanDoan.Rows.Count - 1; j > i; j--)
                        {
                            if (dtgvChanDoan.Rows[j].IsNewRow) continue;
                            if (tmp == dtgvChanDoan.Rows[j].Cells[colNumber].Value.ToString())
                            {
                                dtgvChanDoan.Rows.RemoveAt(j);
                            }
                        }
                    }
                }
            }
        }

        private void btnThemCDPH_Click_1(object sender, EventArgs e)
        {
            #region Thêm chẩn đoán phối hợp
            try
            {
                string sql = @"select idicd,MaICD,MoTa from ChanDoanICD where IDICD='" + sluCDPH.EditValue.ToString() + "'";
                DataTable dtICD = DataAcess.Connect.GetTable(sql);
                //dataGridView1.DataSource = dtbenhnhan;
                //string secondColum = gluChanDoan.Text;

                string idicd = dtICD.Rows[0]["idicd"].ToString();
                string MaICD = dtICD.Rows[0]["maicd"].ToString();
                string MoTaCD_edit = txtCDPH.Text;
                string ID_CDPH = "";
                string[] row = { idicd, MaICD, MoTaCD_edit, ID_CDPH };
                //for (int i = 0; i < dtICD.Rows.Count; i++)
                //{
                //    dtICD.Rows[i]["STT"] = i + 1;

                //}
                dtgvChanDoan.Rows.Add(row);
              //  dtgvChanDoan.AutoResizeColumns();
               // dtgvChanDoan.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            }
            catch
            {
                MessageBox.Show("Chưa chọn chẩn đoán!");
            }

            int colNumber = 0;
            for (int i = 0; i < dtgvChanDoan.Rows.Count - 1; i++)
            {
                if (dtgvChanDoan.Rows[i].IsNewRow) continue;
                string tmp = dtgvChanDoan.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dtgvChanDoan.Rows.Count - 1; j > i; j--)
                {
                    if (dtgvChanDoan.Rows[j].IsNewRow) continue;
                    if (tmp == dtgvChanDoan.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dtgvChanDoan.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion
        }

        private void dtgvCDSB_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            #region Đổ màu hồng vào Button xóa Chẩn đoán sơ bộ
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
            #endregion
        }

        private void dtgvChanDoan_CellPainting_1(object sender, DataGridViewCellPaintingEventArgs e)
        {
            #region Đổ màu hồng vào Button xóa Chẩn đoán phối hợp
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
            #endregion
        }

        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            string mota_CDSB = null;
            string MaICD_CDSB = null;
            for (int i = 0; i < dtgvCDSB.Rows.Count - 1; i++)
            {
                if (dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() != "" && dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() != null && dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() != "0")
                {
                    mota_CDSB += dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn6"].Value.ToString() + ";";
                    MaICD_CDSB += dtgvCDSB.Rows[i].Cells["dataGridViewTextBoxColumn5"].Value.ToString() + ";";
                }

            }
            string cdsb = mota_CDSB + "(" + MaICD_CDSB + ")";
            MessageBox.Show(cdsb);
        }

        private void chkKhongKham_Click(object sender, EventArgs e)
        {
            if (chkKhongKham.Checked == true)
            {
                chkChoveKT.Enabled = false;
                chkChuyenVien.Enabled = false;
                chkTieuPhau.Enabled = false;
            }
            else
            {
                chkChoveKT.Enabled = true;
                chkChuyenVien.Enabled = true;
                chkTieuPhau.Enabled = true;
            }
        }

        private void chkChoveKT_Click(object sender, EventArgs e)
        {
            if (chkChoveKT.Checked == true)
            {
                chkKhongKham.Enabled = false;
                chkChuyenVien.Enabled = false;
                chkTieuPhau.Enabled = false;
            }
            else
            {
                chkKhongKham.Enabled = true;
                chkChuyenVien.Enabled = true;
                chkTieuPhau.Enabled = true;
            }
        }

        private void chkChuyenVien_Click(object sender, EventArgs e)
        {
            if (chkChuyenVien.Checked == true)
            {
                chkKhongKham.Enabled = false;
                chkChoveKT.Enabled = false;
                chkTieuPhau.Enabled = false;
            }
            else
            {
                chkKhongKham.Enabled = true;
                chkChoveKT.Enabled = true;
                chkTieuPhau.Enabled = true;
            }
        }

        private void chkTieuPhau_Click(object sender, EventArgs e)
        {
            if (chkTieuPhau.Checked == true)
            {
                chkKhongKham.Enabled = false;
                chkChoveKT.Enabled = false;
                chkChuyenVien.Enabled = false;
            }
            else
            {
                chkKhongKham.Enabled = true;
                chkChoveKT.Enabled = true;
                chkChuyenVien.Enabled = true;
            }
        }

        private void sluCDPH_EditValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                string maICD2 = sluCDPH.EditValue.ToString();
                string sql = "SELECT mota FROM dbo.ChanDoanICD where idicd= '" + maICD2 + "'";
                DataTable layMota = DataAcess.Connect.GetTable(sql);
                txtCDPH.Text = layMota.Rows[0]["mota"].ToString();
            }
            catch
            {
                return;
            }
        }

        private void btnLayCDSB_Click_1(object sender, EventArgs e)
        {
            TongHop_CDSB(Truyendulieu.idkhambenh);
        }
        private void checkBox1_Click_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                sluKhoa.Enabled = true;
                sluPK.Enabled = true;
            }
            else
            {
                sluKhoa.Enabled = false;
                sluPK.Enabled = false;
                sluPK.EditValue = null;
                sluKhoa.EditValue = null;
            }

        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                btnluu.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Alt | Keys.F4))
            {
                foreach (Form f in this.MdiChildren)
                {
                    f.Close();
                }
                return true;
            }
            else if (keyData == Keys.F2)
            {
                btnmoi.PerformClick();
                return true;
            }
            else if (keyData == Keys.F3)
            {
                btndelete.PerformClick();
                return true;
            }
            //else if (keyData == Keys.F5)
            //{
            //    if (mabn.Text != null && mabn.Text != "" && mabn.Text != "null")
            //    {
            //        resetngay();
            //        setTimKiem();
            //        loadDSDangkykham();
            //        loadlisthenkham();
            //        //btthongtuyen.PerformClick();
            //    }
            //    return true;
            //}
            else if (keyData == Keys.F6)
            {
                btninthuocbh.PerformClick();
                return true;
            }
            else if (keyData == Keys.F7)
            {
                btninthuocdv.PerformClick();
                return true;
            }
            else if (keyData == Keys.F8)
            {
                btninclsbh.PerformClick();
                return true;
            }
            else if (keyData == Keys.F9)
            {
                btninbv01.PerformClick();
                return true;
            }
            else if (keyData == Keys.F10)
            {
                simpleButton1.PerformClick();
                return true;
            }
            //else if (keyData == Keys.F11)
            //{
            //    MessageBox.Show("Chưa có chức năng!");
            //    return true;
            //}
            //else if (keyData == Keys.F12)
            //{
            //    btthongtuyen.PerformClick();
            //    return true;
            //}
            else return false;
        }

        private void btnNhomCLS_Click(object sender, EventArgs e)
        {
            string sql = @"select bg.idbanggiadichvu
                            ,bg.idbanggiadichvu as tendichvu
                            ,bg.giadichvu
                            ,soluong=1
                            ,bg.bhtra as giabh
                            ,bg.IsSuDungChoBH
                            ,bg.IsSuDungChoBH as IsBHYT_Save
                            ,bg.fromdate
                            ,bg.IdnhomInBV as idnhomin
                            ,ghichu=''
                            ,IdKBCLS=''
                             from  KB_NhomCLS n
                             inner join KB_ChiTietNhomCLS c on c.NhomID=n.NhomId
                             inner join banggiadichvu bg on bg.idbanggiadichvu=c.idbanggiadichvu
                             where n.NhomId='" + slNhomCLS.EditValue + "' and bg.IsActive=1";
            DataTable dtNhomCLS = DataAcess.Connect.GetTable(sql);
            grcCLS.DataSource = dtNhomCLS;
        }


        private void gridView15_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            #region Kiểm tra trùng mã Thuốc dịch vụ
            try
            {
                string cls = gridView15.GetRowCellValue(e.RowHandle, colIDthuocDV).ToString();
                for (int i = 0; i < gridView15.RowCount - 1; i++)
                {
                    if (e.RowHandle != i)
                    {
                        string idcanlamsan = gridView15.GetRowCellValue(i, gridView15.Columns["idthuoc"]).ToString();
                        if (cls == idcanlamsan)
                        {
                            MessageBox.Show("Đã có thuốc này rồi!");
                            gridView15.DeleteRow(gridView15.FocusedRowHandle);
                            return;
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region Click chọn thuốc vào gridview Thuốc Dịch vụ
            try
            {
                if (e.Column.FieldName == "tenthuoc")
                {
                    var value = gridView15.GetRowCellValue(e.RowHandle, e.Column);
                    string sql = @"select * from (SELECT B.IDTHUOC as idthuoc
						                                ,B.TENTHUOC as tenthuoc
                                                        ,B.LOAITHUOCID as loaithuocid
                                                        ,c.TenDVT
                                                        ,B.iddvt
                                                        ,B.congthuc as congthuc
                                                        ,cd.tencachdung as duongdung
                                                        ,cd.idcachdung as idcachdung
                                                        ,cd.tencachdung as tencachdung
                                                        ,B.sudungchobh as isbhyt
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
                        and b.idthuoc='" + value + @"'
						AND ISNULL(B.IsNgungSD,0)=0)ab
                        where slton>0 and dongia>0
						ORDER BY  isnull(isbhyt,0) desc, isnull( isthuocbv,0) desc ,tenthuoc ASC";
                    DataTable dt = DataAcess.Connect.GetTable(sql);
                    if (dt != null)
                    {
                        gridView15.SetRowCellValue(e.RowHandle, "idthuoc", dt.Rows[0]["idthuoc"].ToString());
                        // gridView2.SetRowCellValue(e.RowHandle, "MaICD", dt.Rows[0]["MaICD"].ToString());
                        gridView15.SetRowCellValue(e.RowHandle, "congthuc", dt.Rows[0]["congthuc"].ToString());
                        gridView15.SetRowCellValue(e.RowHandle, "TenDVT", dt.Rows[0]["TenDVT"].ToString());
                        gridView15.SetRowCellValue(e.RowHandle, "iddvt", dt.Rows[0]["iddvt"].ToString());
                        gridView15.SetRowCellValue(e.RowHandle, "isbhyt", dt.Rows[0]["isbhyt"].ToString());
                        gridView15.SetRowCellValue(e.RowHandle, "slton", dt.Rows[0]["SLTON"].ToString());
                        // gridView15.SetRowCellValue(e.RowHandle, "IsBHYT_Save", dt.Rows[0]["isbhyt"].ToString());
                        gridView15.SetRowCellValue(e.RowHandle, "issang", 1);
                        gridView15.SetRowCellValue(e.RowHandle, "ischieu", 1);
                        gridView15.SetRowCellValue(e.RowHandle, "ngayuong", 2);
                        gridView15.SetRowCellValue(e.RowHandle, "moilanuong", 1);
                        gridView15.SetRowCellValue(e.RowHandle, "idcachdung", 1);
                        gridView15.SetRowCellValue(e.RowHandle, "iddvdung", 1);
                    }
                }
            }
            catch { return; }
            #endregion
        }

        private void repositoryItembtnXoaThuoc_Click(object sender, EventArgs e)
        {
            #region Xóa thuốc bảo hiểm
            if (MessageBox.Show("Bạn có chắc muốn xóa thuốc Bảo hiểm?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView4.GetRowCellValue(gridView4.FocusedRowHandle, gridView4.Columns["idchitietbenhnhantoathuoc"]).ToString();
                    if (id != null && id != "")
                    {
                        string delete = "delete chitietbenhnhantoathuoc where idchitietbenhnhantoathuoc =" + id;
                        bool ok = DataAcess.Connect.ExecSQL(delete);
                        if (ok)
                        {
                            MessageBox.Show("Xóa thành công!");
                            Load_Toathuoc_Gridview();
                        }
                    }
                    else
                    {
                        gridView4.DeleteRow(gridView4.FocusedRowHandle);
                    }
                }
                catch
                {
                    MessageBox.Show("Ô bạn chọn là ô trống!");
                }
            }
            #endregion
        }

        private void repositoryItemButtonEdit4_Click(object sender, EventArgs e)
        {
            #region Xóa thuốc Dịch vụ
            if (MessageBox.Show("Bạn có chắc muốn xóa thuốc Dịch vụ?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView15.GetRowCellValue(gridView15.FocusedRowHandle, gridView15.Columns["idchitietbenhnhantoathuoc"]).ToString();
                    if (id != null && id != "")
                    {
                        string delete = "delete chitietbenhnhantoathuoc_nhathuoc where idchitietbenhnhantoathuoc =" + id;
                        bool ok = DataAcess.Connect.ExecSQL(delete);
                        if (ok)
                        {
                            MessageBox.Show("Xóa thành công!");
                            Load_ToathuocDV_Gridview();
                        }
                    }
                    else
                    {
                        gridView15.DeleteRow(gridView15.FocusedRowHandle);
                    }
                }
                catch
                {
                    MessageBox.Show("Ô bạn chọn là ô trống!");
                }
            }
            #endregion
        }

        private void grcCLS_Click(object sender, EventArgs e)
        {
         //Load_CLS();
        }

        private void txtSongayratoa_TextChanged(object sender, EventArgs e)
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

        private void expandablePanel13_Click(object sender, EventArgs e)
        {
        
        }

        private void expandablePanel13_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (expandablePanel13.Expanded==true)
            {
                thuocdv_click = thuocdv_click+1;
            }

            if (thuocdv_click == 1 && expandablePanel13.Expanded == true)
            {
                Load_Item_thuoc_DV();
                Load_Item_DonViDung_DV();
                Load_Item_Cachdung_DV();
                Load_ToathuocDV_Gridview();
            }
        }

        private void expandablePanel5_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (expandablePanel5.Expanded == true)
            {
                thuocbh_click = thuocbh_click + 1;
            }
            if (thuocbh_click == 1 && expandablePanel5.Expanded == true)
            {
                Load_Item_thuoc();
                Load_Item_Cachdung();
                Load_Item_DonViDung();
                Load_Toathuoc_Gridview();
                
            }
        }

        private void expandablePanel14_ExpandedChanged(object sender, DevComponents.DotNetBar.ExpandedChangeEventArgs e)
        {
            if (expandablePanel14.Expanded == true)
            {
                henCLS_click = henCLS_click + 1;
            }
            if (henCLS_click == 1 && expandablePanel14.Expanded == true)
            {
                Load_CSLhen_gridview();
            }
        }

        private void txtMach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtHuyetAp.Focus();
            }
        }

        private void txtHuyetAp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtHuyetAp2.Focus();
            }
        }

        private void txtHuyetAp2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtNhietDo.Focus();
            }
        }

        private void txtNhietDo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtNhipTho.Focus();
            }
        }

        private void txtNhipTho_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtCanNang.Focus();
            }
        }

        private void txtCanNang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtChieuCao.Focus();
            }
        }

        private void txtChieuCao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtBMI.Focus();
            }
        }

        private void txtBMI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txttiensu.Focus();
            }
        }

        private void txttiensu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txttrieuchung.Focus();
            }
        }

        private void txttrieuchung_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtbenhsu.Focus();
            }
        }

        private void txtbenhsu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                txtdiung.Focus();
            }
        }

        private void txtdiung_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                sluBacsi.Focus();
            }
        }

        private void gluCDSobo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnThemCDSB_Click_1(sender,e);
            }
        }

        private void slNhomCLS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnNhomCLS_Click(sender, e);
            }
        }

        private void sluCDPH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnThemCDPH_Click_1(sender, e);
            }
        }

    }
}
