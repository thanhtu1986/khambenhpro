using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KhamBenhPro.KhamBenh
{
    public partial class Thuoc : Form
    {
        public Thuoc()
        {
            InitializeComponent();
            Load_ToaThuoc(Truyendulieu.idkhambenh);
            KhoThuoc_load();
            Load_thuoc_doituong();
            Load_GhiChu();
        }

        private void btnThemThuoc_Click(object sender, EventArgs e)
        {
            Load_thuoc_theoID();
        }

        public void Load_GhiChu()
        {
            DataTable dtGhichu = DataAcess.Connect.GetTable(GetData.dt_BNDaKham2(Truyendulieu.idkhambenh));
            txtSongayratoa.Text= dtGhichu.Rows[0]["songayratoa"].ToString();
            try
            {
                if (dtGhichu.Rows[0]["ngayhentaikham"].ToString() != "" || dtGhichu.Rows[0]["ngayhentaikham"].ToString() != null)
                {
                    dtpkTaikham.Value = DateTime.Parse(dtGhichu.Rows[0]["ngayhentaikham"].ToString());
                }
                else dtpkTaikham.Value = DateTime.Now;
            }
            catch { }
            txtLoidan.Text= dtGhichu.Rows[0]["loidan"].ToString();
            txtGhichu.Text = dtGhichu.Rows[0]["ghichu"].ToString();
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
                                        ,SLTON= ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUNHAPKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_NHAP='" + slkKho.EditValue.ToString() + @"'),0)-ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUXUATKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_XUAT='" + slkKho.EditValue.ToString() + @"' ),0)
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
                                        and b.idthuoc='" + slkThuoc.EditValue.ToString() + @"'
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
                        and b.idthuoc='" + slkThuoc.EditValue.ToString() + @"'
                        ORDER BY TENTHUOC";
            return sql;
        }
        public void Load_thuoc_theoID()
        {
            #region Load thuốc theo Id thuốc
            try
            {
                DataTable dtThuocID = null;
                if (slkKho.EditValue.ToString() == "72")
                {
                    dtThuocID = DataAcess.Connect.GetTable(Thuoc_DV_Id());
                }
                else
                    if (slkKho.EditValue.ToString() == "5")
                {
                    dtThuocID = DataAcess.Connect.GetTable(Thuoc_BH_Id());
                }
                string khoxuat = slkKho.Text.ToString();
                string doituong = slkDoiTuong.Text.ToString();
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
                if (slkKho.EditValue.ToString() == "72")
                {
                    dtthuoc = DataAcess.Connect.GetTable(Thuoc_DV());
                }
                else
                        if (slkKho.EditValue.ToString() == "5")
                {
                    dtthuoc = DataAcess.Connect.GetTable(Thuoc_BH());
                }
                slkThuoc.Properties.DataSource = dtthuoc;
                slkThuoc.Properties.NullText = "Nhập tên thuốc";
                slkThuoc.Properties.DisplayMember = "tenthuoc";
                slkThuoc.Properties.ValueMember = "idthuoc";
                slkThuoc.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
                slkThuoc.Properties.ImmediatePopup = true;
                slkThuoc.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            }
            catch { MessageBox.Show("Chưa chọn kho"); }
            #endregion
        }
        public void Load_thuoc_doituong()
        {
            #region Load đối tượng Thuốc,VTYT..
          string sql="select LoaiThuocID,TenLoai from Thuoc_LoaiThuoc ";
            DataTable dtDoituong = DataAcess.Connect.GetTable(sql);
            slkDoiTuong.Properties.DataSource = dtDoituong;
            slkDoiTuong.Properties.NullText = "Nhập đối tượng";
            slkDoiTuong.Properties.DisplayMember = "TenLoai";
            slkDoiTuong.Properties.ValueMember = "LoaiThuocID";
         
            #endregion
        }
        public void KhoThuoc_load()
        {
            #region Load kho thuốc
            string sql="select idkho,tenkho from khothuoc where idkho in (72,5)";
            DataTable dtKhothuoc = DataAcess.Connect.GetTable(sql);
            slkKho.Properties.DataSource = dtKhothuoc;
            //  sluKho.Properties.NullText = "Chọn Kho";
            slkKho.Properties.DisplayMember = "tenkho";
            slkKho.Properties.ValueMember = "idkho";
            slkKho.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkKho.Properties.ImmediatePopup = true;
            slkKho.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
       
            #endregion
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

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void slkKho_EditValueChanged(object sender, EventArgs e)
        {
            Load_Thuoc();
        }
    }
}
