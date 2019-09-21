using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KhamBenhPro
{
    public partial class frmBV01 : Form
    {
        public frmBV01()
        {
            InitializeComponent();
        }

        private void frmBV01_Load(object sender, EventArgs e)
        {
            string Ma_LK = "";
            KhamBenhPro.rptBV01 crystalReport4 = new KhamBenhPro.rptBV01();
            DataTable dt = hs_tinhtien.dtSource_BV(Truyendulieu.idphieutt);

              
                DateTime ngayravien = DateTime.Parse(dt.Rows[0]["NgayTinhBH_Thuc"].ToString());
                DateTime ngayvaovien = DateTime.Parse(dt.Rows[0]["NgayTinhBH"].ToString());

                bool IsCapCuu = (hs_tinhtien.IsCheck(dt.Rows[0]["IsCapCuu"].ToString()) ? true : false);
                bool IsDungTuyen = (dt.Rows[0]["DUNGTUYEN"].ToString().ToUpper() == "Y" || hs_tinhtien.IsCheck(dt.Rows[0]["IsDungTuyen"].ToString()));
                bool IsTraiTuyen = (!IsCapCuu && !IsDungTuyen);
           

                string IsNoiTru = (hs_tinhtien.IsCheck(dt.Rows[0]["IsNoiTru"].ToString()) ? "1" : "0");
                double TongTienBNPT_BV = double.Parse(dt.Rows[0]["TongTienBNPT"].ToString() == "" ? "0" : dt.Rows[0]["TongTienBNPT"].ToString());
                double TONGTIEN_DATRA_BV = double.Parse(dt.Rows[0]["TONGTIEN_DATRA"].ToString() == "" ? "0" : dt.Rows[0]["TONGTIEN_DATRA"].ToString());
                double TongTien_ConLai = TongTienBNPT_BV - TONGTIEN_DATRA_BV;
                string SoTienHoanTra = "";
                string SoTienThuThem = "";
            bool IsKhamBenh_bool = (IsNoiTru != "1" && (dt.Rows[0]["ID_KHOA_VIEW"].ToString() == "1" || dt.Rows[0]["ID_KHOA_VIEW"].ToString() == "3") ? true : false);
            bool IsNoiTru_bool = (IsNoiTru == "1" ? true : false);
            bool IsDieuTriNgoaiTru_bool = (!IsKhamBenh_bool && !IsNoiTru_bool);
            double THANHTIENDV_total = 0;
            double THANHTIEN_total = 0;
            double BNTRA_total = 0;
            double QUYBHYT_total = 0;
            double BN_TU_TRA_total = 0;
            double TongChiPhi_total = 0;
            double TongTienBNPT_total = 0;
           float TongChiPhi1 = 0;

                DataSet  dsDetail = hs_tinhtien.dtSourceDetail_BV(Truyendulieu.idphieutt, IsNoiTru_bool, dt.Rows[0]["ngaytrinhthe1"].ToString(), dt.Rows[0]["ngaytrinhthe2"].ToString(), dt.Rows[0]["idbenhnhan_bh1"].ToString(), dt.Rows[0]["idbenhnhan_bh2"].ToString());
                DataTable dtDetail = dsDetail.Tables[0].Copy();
                if (dtDetail == null)
                {
                    MessageBox.Show("Không có BV01");
                }
                else
                {
                    Ma_LK = dt.Rows[0]["Ma_LK"].ToString();
                    #region ma vach
                    Barcode128 barcode = new Barcode128();
                    barcode.ChecksumText = false;
                    barcode.Code = Ma_LK;
                    //barcode.Code = MaPhieuCLS.Replace("PT", "").Replace("-", "").Replace("CT", "") + "";
                    System.Drawing.Image bmp = barcode.CreateDrawingImage(Color.Black, Color.White);
                    Byte[] arrByte = (Byte[])TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(Byte[]));
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        dtDetail.Rows[k]["MaVach"] = arrByte;
                    }
                    #endregion
                    dtDetail.TableName = "dtBangKeChiPhi";
                    dsDetail.Tables.Add(dtDetail);
                crystalReport4.SetDataSource(dsDetail);
                string idbenhnhan_bh_clause = "";
                object THANHTIENDV = dtDetail.Compute("SUM(THANHTIENDV)", idbenhnhan_bh_clause); string s_THANHTIENDV = (THANHTIENDV == null ? "" : (THANHTIENDV.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(THANHTIENDV.ToString())));
                object THANHTIEN = dtDetail.Compute("SUM(THANHTIEN)", idbenhnhan_bh_clause); string s_THANHTIEN = (THANHTIEN == null ? "" : (THANHTIEN.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(THANHTIEN.ToString())));
                object BNTRA = dtDetail.Compute("SUM(BNTRA)", idbenhnhan_bh_clause); string s_BNTRA = (BNTRA == null ? "" : (BNTRA.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(BNTRA.ToString())));
                object QUYBHYT = dtDetail.Compute("SUM(QUYBHYT)", idbenhnhan_bh_clause); string s_QUYBHYT = (QUYBHYT == null ? "" : (QUYBHYT.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(QUYBHYT.ToString())));
                object BN_TU_TRA = dtDetail.Compute("SUM(NGUOIBENH)", idbenhnhan_bh_clause); string s_BN_TU_TRA = (BN_TU_TRA == null ? "" : (BN_TU_TRA.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(BN_TU_TRA.ToString())));
                double TongChiPhi = double.Parse(s_THANHTIENDV == "" ? "0" : s_THANHTIENDV);
                double TongTienBNPT = double.Parse(s_BNTRA == "" ? "0" : s_BNTRA) + double.Parse(s_BN_TU_TRA == "" ? "0" : s_BN_TU_TRA);
                if (THANHTIENDV != null && THANHTIENDV.ToString() != "") THANHTIENDV_total += double.Parse(THANHTIENDV.ToString());
                if (THANHTIEN != null && THANHTIEN.ToString() != "") THANHTIEN_total += double.Parse(THANHTIEN.ToString());
                if (BNTRA != null && BNTRA.ToString() != "") BNTRA_total += double.Parse(BNTRA.ToString());
                if (QUYBHYT != null && QUYBHYT.ToString() != "") QUYBHYT_total += double.Parse(QUYBHYT.ToString());
                if (BN_TU_TRA != null && BN_TU_TRA.ToString() != "") BN_TU_TRA_total += double.Parse(BN_TU_TRA.ToString());
                if (TongChiPhi != null && TongChiPhi.ToString() != "") TongChiPhi_total += double.Parse(TongChiPhi.ToString());
                if (TongTienBNPT != null && TongTienBNPT.ToString() != "") TongTienBNPT_total += double.Parse(TongTienBNPT.ToString());
                string s_THANHTIENDV_total = (THANHTIENDV_total == null ? "" : (THANHTIENDV_total.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(THANHTIENDV_total.ToString())));
                string s_THANHTIEN_total = (THANHTIEN_total == null ? "" : (THANHTIEN_total.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(THANHTIEN_total.ToString())));
                string s_BNTRA_total = (BNTRA_total == null ? "" : (BNTRA_total.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(BNTRA_total.ToString())));
                string s_QUYBHYT_total = (QUYBHYT_total == null ? "" : (QUYBHYT_total.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(QUYBHYT_total.ToString())));
                string s_BN_TU_TRA_total = (BN_TU_TRA_total == null ? "" : (BN_TU_TRA_total.ToString() == "0" ? "" : hs_tinhtien.FormatSNumberToPrint(BN_TU_TRA_total.ToString())));

                crystalReport4.SetParameterValue("thanhtiendvT", s_THANHTIENDV_total);
                crystalReport4.SetParameterValue("thanhtienbhT", s_THANHTIEN_total);
                crystalReport4.SetParameterValue("bntraT", s_BNTRA_total);
                crystalReport4.SetParameterValue("quybhytT", s_QUYBHYT_total);
                crystalReport4.SetParameterValue("nguoibenhT", s_BN_TU_TRA_total);
               
                crystalReport4.SetParameterValue("TenKhoa",dt.Rows[0]["TEN_KHOA_VIEW"].ToString());
                crystalReport4.SetParameterValue("MaKhoa", dt.Rows[0]["MA_KHOA_VIEW"].ToString());
                crystalReport4.SetParameterValue("MaBN", dt.Rows[0]["mabenhnhan"].ToString());
                crystalReport4.SetParameterValue("SoKB", dt.Rows[0]["id"].ToString());
                crystalReport4.SetParameterValue("HoTenBN", dt.Rows[0]["tenbenhnhan"].ToString());
                crystalReport4.SetParameterValue("NgaySinh", dt.Rows[0]["ngaysinh"].ToString());
                crystalReport4.SetParameterValue("GioiTinh", dt.Rows[0]["gioi_tinh"].ToString());
                crystalReport4.SetParameterValue("DiaChi", dt.Rows[0]["dia_chi"].ToString());
                crystalReport4.SetParameterValue("SoBHYT", dt.Rows[0]["ma_the"].ToString());
                crystalReport4.SetParameterValue("NgayBD", dt.Rows[0]["gt_the_tu"].ToString());
                crystalReport4.SetParameterValue("NgayHH", dt.Rows[0]["gt_the_den"].ToString());
                crystalReport4.SetParameterValue("NoiDKKCB", dt.Rows[0]["ten_dkbd"].ToString());
                crystalReport4.SetParameterValue("MaNoiDK", dt.Rows[0]["ma_dkbd"].ToString());
                crystalReport4.SetParameterValue("NgayKham", ngayvaovien.ToString("HH") + "  giờ, " + ngayvaovien.ToString("mm") + " phút, ngày " + ngayvaovien.ToString("dd/MM/yyyy"));
                crystalReport4.SetParameterValue("TGxuatvien", ngayravien.ToString("HH") + "   giờ,  " + ngayravien.ToString("mm") + "   phút, ngày   " + ngayravien.ToString("dd/MM/yyyy"));
                crystalReport4.SetParameterValue("SoNgayDT", dt.Rows[0]["SO_NGAY_DTRI"].ToString());
                crystalReport4.SetParameterValue("TTravien", dt.Rows[0]["tinh_trang_rv_BV"].ToString());
                if(IsCapCuu==true)
                {
                    crystalReport4.SetParameterValue("isCapcuu", "a");
                 }
                else crystalReport4.SetParameterValue("isCapcuu", "");
                if(IsDungTuyen==true)
                {
                    crystalReport4.SetParameterValue("IsDungTuyen", "a");
                }
                else crystalReport4.SetParameterValue("IsDungTuyen", "");
                crystalReport4.SetParameterValue("ChanDoanXD", dt.Rows[0]["ten_benh"].ToString());
                crystalReport4.SetParameterValue("MaCDXD", dt.Rows[0]["MA_BENH"].ToString());
                crystalReport4.SetParameterValue("ChanDoanPH", dt.Rows[0]["ChanDoanKhac"].ToString());
                crystalReport4.SetParameterValue("MaCDPH", dt.Rows[0]["ma_benhkhac"].ToString());
                if (dt.Rows[0]["ngaydu_5namlientuc"].ToString() == "" || dt.Rows[0]["ngaydu_5namlientuc"].ToString() == null || dt.Rows[0]["ngaydu_5namlientuc"].ToString() == "0")
                {
                    crystalReport4.SetParameterValue("5nam", "");
                }
                else
                {
                    crystalReport4.SetParameterValue("5nam", DateTime.Parse(dt.Rows[0]["ngaydu_5namlientuc"].ToString()).ToString("dd/MM/yyyy"));
                }
                crystalReport4.SetParameterValue("isMienCCT", dt.Rows[0]["ngaybd_miendct"].ToString());
                crystalReport4.SetParameterValue("KhuVuc", dt.Rows[0]["MA_KHUVUC"].ToString());
                crystalReport4.SetParameterValue("MucHuong", dt.Rows[0]["MUC_HUONG"].ToString());
                crystalReport4.SetParameterValue("NoiGT", dt.Rows[0]["ten_noi_chuyen"].ToString());
                crystalReport4.SetParameterValue("NoiCD", dt.Rows[0]["TenBV_ChuyenDi"].ToString());
           
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    TongChiPhi1 += (dtDetail.Rows[i]["THANHTIENDV"].ToString() != "" ? float.Parse(dtDetail.Rows[i]["THANHTIENDV"].ToString()) : 0);
                }
                crystalReport4.SetParameterValue("TongChiPhikcb", hs_tinhtien.ConvertMoneyToText(TongChiPhi1.ToString()));
                crystalReport4.SetParameterValue("TongTienBNDT", hs_tinhtien.FormatSNumberToPrint(TONGTIEN_DATRA_BV.ToString()));
                crystalReport4.SetParameterValue("TongTienBNPT", hs_tinhtien.FormatSNumberToPrint(TongTienBNPT_BV.ToString()));
                if (TongTien_ConLai > 0)
                {
                    SoTienThuThem = TongTien_ConLai.ToString();
                    crystalReport4.SetParameterValue("ConLai","Số tiền thu thêm: " + hs_tinhtien.FormatSNumberToPrint(SoTienThuThem)+ "đồng");
                    crystalReport4.SetParameterValue("ConLaiBangChu", hs_tinhtien.ConvertMoneyToText(SoTienThuThem));
                }
                else
                {
                    SoTienHoanTra = (-1 * TongTien_ConLai).ToString();
                    crystalReport4.SetParameterValue("ConLai", "Số tiền hoàn trả: " + hs_tinhtien.FormatSNumberToPrint(SoTienHoanTra)+ "đồng");
                    crystalReport4.SetParameterValue("ConLaiBangChu", hs_tinhtien.ConvertMoneyToText(SoTienHoanTra));

                }
                crystalReport4.SetParameterValue("ngaythangnam", "Ngày " + ngayravien.ToString("dd") + " tháng " + ngayravien.ToString("MM") + " năm " + ngayravien.ToString("yyyy") + "");
                crpv01.ReportSource = crystalReport4;
            }
        }
    }
}
