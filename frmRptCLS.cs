using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using iTextSharp.text.pdf;

namespace KhamBenhPro
{
    public partial class frmRptCLS : Form
    {
        private ReportDocument report = null;
        public frmRptCLS()
        {
            InitializeComponent();
        }

        private void frmRptCLS_Load(object sender, EventArgs e)
        {
            //rptCLS rpt = new rptCLS();
            string MaPhieuCLS = "";
            string TenNhom = "";
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
                     WHERE  ISNULL(A.dahuy,0)=0 AND  A.IDKHAMBENH='" + Truyendulieu.idkhambenh +"'";
            DataTable dtmavach = DataAcess.Connect.GetTable(sql);
            if (dtmavach == null || dtmavach.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy bệnh nhân");
                return;
            }
            else
            {
                MaPhieuCLS = dtmavach.Rows[0]["MAPHIEUCLS"].ToString();
                for (int i = 0; i < dtmavach.Rows.Count; i++)
                {
                    TenNhom = dtmavach.Rows[i]["TenNhom"].ToString();
                }
                KhamBenhPro.rptCLS crystalReport = new KhamBenhPro.rptCLS();
                #region ma vach
                Barcode128 barcode = new Barcode128();
                barcode.ChecksumText = false;
                barcode.Code = MaPhieuCLS;
                //barcode.Code = MaPhieuCLS.Replace("PT", "").Replace("-", "").Replace("CT", "") + "";
                System.Drawing.Image bmp = barcode.CreateDrawingImage(Color.Black, Color.White);
                Byte[] arrByte = (Byte[])TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(Byte[]));
                for (int k = 0; k < dtmavach.Rows.Count; k++)
                {
                    dtmavach.Rows[k]["MaVach"] = arrByte;
                }
                #endregion
                string TenBenhNhan = dtmavach.Rows[0]["tenbenhnhan"].ToString();
                string gioitinh = dtmavach.Rows[0]["gioitinh"].ToString();
                string NamSinh = dtmavach.Rows[0]["ngaysinh"].ToString();
                string DiaChi = dtmavach.Rows[0]["diachi"].ToString();
                string MaTheBHYT = dtmavach.Rows[0]["sobhyt"].ToString();
                string PhongCD = dtmavach.Rows[0]["nvk_phong"].ToString();
                string KhoaCD = dtmavach.Rows[0]["TENPHONGKHAMBENH"].ToString();
                DateTime NgayKham = DateTime.Parse(dtmavach.Rows[0]["NgayKham"].ToString());
                string Gioitinh = dtmavach.Rows[0]["Gioitinh"].ToString();
                string NgayBatdau = dtmavach.Rows[0]["NgayBatdau"].ToString();
                string NgayHethan = dtmavach.Rows[0]["NgayHethan"].ToString();
                string NoiDKbandau = dtmavach.Rows[0]["TENNOIDANGKY"].ToString();
                string ChanDoan = dtmavach.Rows[0]["ChanDoan"].ToString();
                string NoiGT = dtmavach.Rows[0]["NoiGT"].ToString();
                string BacsiCD = dtmavach.Rows[0]["HOTENBS"].ToString();
                // string isCapcuu=dtmavach.Rows[0]["IsCapCuu"].ToString();
                //string CoBHYT=dtmavach.Rows[0]["LoaiKhamID"].ToString();
                crystalReport.SetDataSource(dtmavach);
                crystalReport.SetParameterValue("MaPhieuCLS", MaPhieuCLS);
                crystalReport.SetParameterValue("TenNhom", TenNhom);
                crystalReport.SetParameterValue("TenBenhNhan", TenBenhNhan);
                crystalReport.SetParameterValue("NamSinh", NamSinh);
                crystalReport.SetParameterValue("DiaChi", DiaChi);
                crystalReport.SetParameterValue("MaTheBHYT", MaTheBHYT);
                crystalReport.SetParameterValue("PhongCD", PhongCD);
                crystalReport.SetParameterValue("KhoaCD", KhoaCD);
                if (dtmavach.Rows[0]["LoaikhamID"].ToString() == "1")
                {
                    crystalReport.SetParameterValue("CoBHYT", "a");
                }
                else crystalReport.SetParameterValue("CoBHYT", "");
                crystalReport.SetParameterValue("NgayKham", "Ngày " + NgayKham.ToString("dd") + " tháng " + NgayKham.ToString("MM") + " năm " + NgayKham.ToString("yyyy") + "");
                crystalReport.SetParameterValue("Gioitinh", Gioitinh);
                crystalReport.SetParameterValue("NgayBatdau", NgayBatdau);
                crystalReport.SetParameterValue("NgayHethan", NgayHethan);
                crystalReport.SetParameterValue("NoiDKbandau", NoiDKbandau);
                crystalReport.SetParameterValue("ChanDoan", ChanDoan);
                if (dtmavach.Rows[0]["IsDungTuyen"].ToString() == "1" || dtmavach.Rows[0]["IsDungTuyen"].ToString().ToLower() == "true" || dtmavach.Rows[0]["IsDungTuyen"].ToString().ToLower() == "y")
                {
                    crystalReport.SetParameterValue("Dungtuyen", "a");
                }
                else crystalReport.SetParameterValue("Dungtuyen", "");

                if (dtmavach.Rows[0]["iscapcuu"].ToString() == "1" || dtmavach.Rows[0]["iscapcuu"].ToString().ToLower() == "true")
                {
                    crystalReport.SetParameterValue("isCapcuu", "a");
                }
                else crystalReport.SetParameterValue("isCapcuu", "");
                crystalReport.SetParameterValue("NoiGT", NoiGT);
                crystalReport.SetParameterValue("BacsiCD", BacsiCD);
                crystalReportViewer1.ReportSource = crystalReport;
            }
        
        }

        private void btnInToa_Click(object sender, EventArgs e)
        {
            crystalReportViewer1.PrintReport();
        }
    }
}
