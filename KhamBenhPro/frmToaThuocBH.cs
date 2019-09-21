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
    public partial class frmToaThuocBH : Form
    {
        public frmToaThuocBH()
        {
            InitializeComponent();
        }
        private static string GetIdkhambenh()
        {
            string idkhambenh = Truyendulieu.idkhambenh;
            return idkhambenh;
        }

        private void frmToaThuocBH_Load(object sender, EventArgs e)
        {
            string idkhambenh = GetIdkhambenh();
            string MaBN = "";
            KhamBenhPro.rptToaThuocBH crystalReport2 = new KhamBenhPro.rptToaThuocBH();
            #region subReportCLS

           if (idkhambenh == null || idkhambenh == "" || idkhambenh == "0") return;

            DataTable dtCLS = loadDV(idkhambenh);
            if (dtCLS != null)
            {
                dtCLS.TableName = "dtSubCLS";
                crystalReport2.OpenSubreport("rptSubReportCLS.rpt").SetDataSource(dtCLS);

            }
            else return;
            #endregion

            DataTable dtsrc = dtSource(idkhambenh);
            DateTime Ngayratoa =DateTime.Now;
            if (dtsrc.Rows[0]["ngayratoa"].ToString() != "" || dtsrc.Rows[0]["ngayratoa"].ToString() != "")
            {
                Ngayratoa=DateTime.Parse(dtsrc.Rows[0]["ngayratoa"].ToString());
            }

            if (dtsrc == null)
            {
                MessageBox.Show("Không có toa thuốc bảo hiểm");
            }
            else
            {
                
                dtsrc.TableName = "dtThuoc";
                DataSet ds = new DataSet();
                ds.Tables.Add(dtsrc);
                MaBN = dtsrc.Rows[0]["mabenhnhan"].ToString();
                #region ma vach
                Barcode128 barcode = new Barcode128();
                barcode.ChecksumText = false;
                barcode.Code = MaBN;
                //barcode.Code = MaPhieuCLS.Replace("PT", "").Replace("-", "").Replace("CT", "") + "";
                System.Drawing.Image bmp = barcode.CreateDrawingImage(Color.Black, Color.White);
                Byte[] arrByte = (Byte[])TypeDescriptor.GetConverter(bmp).ConvertTo(bmp, typeof(Byte[]));
                for (int k = 0; k < dtsrc.Rows.Count; k++)
                {
                    dtsrc.Rows[k]["MaVach"] = arrByte;
                }
                #endregion
                crystalReport2.SetDataSource(ds);
                crystalReport2.SetParameterValue("TenBacsi", dtsrc.Rows[0]["tenbacsi"].ToString());
                crystalReport2.SetParameterValue("MaBenhNhan", dtsrc.Rows[0]["mabenhnhan"].ToString());
                crystalReport2.SetParameterValue("TuoiBN", dtsrc.Rows[0]["TuoiBN"].ToString());
                crystalReport2.SetParameterValue("CanNang", dtsrc.Rows[0]["cannang"].ToString());
                crystalReport2.SetParameterValue("HuyetAp1", dtsrc.Rows[0]["huyetap1"].ToString());
                crystalReport2.SetParameterValue("HuyetAp2", dtsrc.Rows[0]["huyetap2"].ToString());
                if (dtsrc.Rows[0]["NguoiGiamHo"].ToString() == "")
                {
                    crystalReport2.SetParameterValue("TenNguoiGiamHo", "");
                }
                else crystalReport2.SetParameterValue("TenNguoiGiamHo", "- Tên bố hoặc mẹ của trẻ hoặc người đưa trẻ đến khám bệnh, chữa bệnh :" + dtsrc.Rows[0]["NguoiGiamHo"].ToString());
                crystalReport2.SetParameterValue("Mach", dtsrc.Rows[0]["mach"].ToString());
                crystalReport2.SetParameterValue("GhiChu", dtsrc.Rows[0]["GhiChu"].ToString());
                crystalReport2.SetParameterValue("LoiDan", dtsrc.Rows[0]["loidan"].ToString()); 
                //for (int i = 0; i < dtsrc.Rows.Count - 1; i++)
                //{
                //    crystalReport2.SetParameterValue("CachDung", dtsrc.Rows[i]["CachDung"].ToString());
                //}
                crystalReport2.SetParameterValue("NgayKham", "Ngày " + Ngayratoa.ToString("dd") + " tháng " + Ngayratoa.ToString("MM") + " năm " + Ngayratoa.ToString("yyyy") + "");
                
                nvk_SetPara_HanhChinh(dtsrc.Rows[0]["idbenhnhan"].ToString(), dtsrc.Rows[0]["idchitietdangkykham"].ToString(), idkhambenh, crystalReport2);
                crystalReportViewer2.ReportSource = crystalReport2;
            }
        }

        private static DataTable dtSource(string idkhambenh)
        {
            string sqlSelect = @" declare @idkhambenh as bigint
                                        set @idkhambenh=" + idkhambenh + @"
                                select  ngaydo,huyetap1,huyetap2,mach,cannang
                                    from sinhhieu a
                                    where idkhambenh= @idkhambenh
                                     
                                    select idchitietdangkykham,dando,ngayratoa=ngaykham
                                    ,tenbacsi 
                                    ,mabenhnhan
                                    ,MaVach=CONVERT(IMAGE,NULL)
                                    ,tenbenhnhan
                                    ,gioitinh=dbo.GetGioiTinh(c.gioitinh)
                                    ,c.diachi
                                    ,ngaysinh=c.ngaysinh
                                    ,TuoiBN=DBO.kb_GetTuoi(c.ngaysinh)
                                    ,noidungtaikham
                                    ,convert(nvarchar, ngayhentaikham, 103) as ngaytaikham
                                    ,ghichu2=''
                                    ,ghichu=A.ghichu
                                    ,a.ngaykham
                                    ,matoathuoc=''
                                    ,A.idbenhnhan
                                    ,E.SOBH1
                                    ,E.SOBH2
                                    ,E.SOBH3
                                    ,E.SOBH4
                                    ,E.SOBH5
                                    ,E.SOBH6
                                    ,ngaybatdau= CONVERT(NVARCHAR(20), E.ngaybatdau,103)
                                    ,ngayhethan= CONVERT(NVARCHAR(20), E.ngayhethan,103)
                                    ,NoiDangKyKCB=NOIDK.TENNOIDANGKY
                                    ,NoiGioiThieu=NOIGT.TENNOIDANGKY
                                    ,icd.MAICD
                                    ,MOTA=(CASE WHEN ISNULL(A.MoTaCD_edit,'')<>'' THEN A.MoTaCD_edit ELSE ICD.MOTA END)
                                    ,D.LOAIKHAMID
                                    ,loidan=a.dando
                                    ,a.idphongkhambenh
                                    ,C.NguoiGiamHo
                                    ,C.chungminhthu
                                    ,mavach=convert(image,null)
                                    from khambenh a
                                    left join bacsi b on  a.idbacsi =B.idbacsi -- (CASE WHEN ISNULL(A.IDBACSI2,0)<>0 THEN A.IDBACSI2 ELSE  a.idbacsi END)=b.idbacsi
                                    inner join benhnhan c on a.idbenhnhan=c.idbenhnhan
                                    INNER JOIN DANGKYKHAM D ON A.IDDANGKYKHAM=D.IDDANGKYKHAM
                                    LEFT JOIN BENHNHAN_BHYT E ON D.IDBENHNHAN_BH=E.IDBENHNHAN_BH        
                                    LEFT JOIN KB_NOIDANGKYKB NOIDK ON E.IdNoiDangKyBH=NOIDK.IDNOIDANGKY
                                    LEFT JOIN KB_NOIDANGKYKB NOIGT ON E.IdNoiGioiThieu=NOIGT.IDNOIDANGKY
                                    LEFT JOIN CHANDOANICD ICD ON A.KETLUAN=ICD.IDICD
                                    where a.idkhambenh=@idkhambenh
                                       
                                    select  
                                    TenThuoc,
                                    congthuc as KetLuan,
                                    donvitinh=c.TenDVT,
                                    soluongke
                                    ,CachDung=
                                               (CASE WHEN ( ISNULL(a.moilanuong,'') ='' OR a.moilanuong='0' ) AND ( ISNULL(a.ngayuong,'') ='' OR a.ngayuong='0' ) THEN lower( CACHDUNG.TenCachDung)+IsNull('('+ a.GHICHU +')','') 
                                                    ELSE 
                                                                ( CASE WHEN ISNULL(isngay,0) =0 AND ISNULL(istuan,0)=0 THEN N'Ngày ' else  ( case when IsNgay=1 then N'Ngày ' else N'Tuần ' end )   End )  
                                                                +
                                                                    lower( CACHDUNG.TenCachDung)
                                                                +' '+ CONVERT(NVARCHAR(20), ngayuong)
                                                                +N' lần'
                                                                + CASE WHEN ISNULL(a.moilanuong,'') ='' OR ISNULL(a.moilanuong,'') ='0' then N'' else N', mỗi lần ' + isnull(a.moilanuong,'')+ isnull(' '+ dvt2.TenDVT,'')  end
                                                                + '. '
                                                + ISNULL( LOWER( DBO.KB_GetGhiChuToaThuoc2(a.idchitietbenhnhantoathuoc) ),'')
                                                END)
                                            
                                     from chitietbenhnhantoathuoc a
                                    inner join thuoc b on a.idthuoc=b.idthuoc
                                    LEFT join Thuoc_DonViTinh c on b.iddvt=c.Id
                                    left join Thuoc_DonViTinh dvt2 ON a.IDDVDUNG=DVT2.ID
                                    left join Thuoc_CACHDUNG CACHDUNG ON ISNULL(a.IDCACHDUNG,b.IDCACHDUNG)=CACHDUNG.IDCACHDUNG 
                                   where   a.idkhambenh=@idkhambenh
                                             AND B.sudungchobh=1 AND  A.ISBHYT_SAVE=1 and isnull(a.idkho,0) <>0 and  isnull(a.idkho,0)<>-1  AND ISNULL(a.idkho,0)<>72
                                            -- AND (   ISNULL( B.sudungchobh,0)=0 OR  ISNULL( A.ISBHYT_SAVE,0)=0 or isnull(a.idkho,0) =0 or  isnull(a.idkho,0) = -1   OR ISNULL(a.idkho,0)=72)
                                        ";
            DataSet ds = DataAcess.Connect.GetDataSet(sqlSelect);
            DataTable dt1 = ds.Tables[0].Copy();
            DataTable dt2 = ds.Tables[1].Copy();
            DataTable dtSRC = ds.Tables[2].Copy();
            for (int i = 0; i < dt1.Columns.Count; i++)
            {
                dtSRC.Columns.Add(dt1.Columns[i].ColumnName, dt1.Columns[i].DataType);
                if (dt1.Rows.Count > 0)
                    for (int j = 0; j < dtSRC.Rows.Count; j++)
                        dtSRC.Rows[j][dt1.Columns[i].ColumnName] = dt1.Rows[0][dt1.Columns[i].ColumnName];
            }

            for (int i = 0; i < dt2.Columns.Count; i++)
            {
                dtSRC.Columns.Add(dt2.Columns[i].ColumnName, dt2.Columns[i].DataType);
                if (dt2.Rows.Count > 0)
                    for (int j = 0; j < dtSRC.Rows.Count; j++)
                        dtSRC.Rows[j][dt2.Columns[i].ColumnName] = dt2.Rows[0][dt2.Columns[i].ColumnName];
            }
            return dtSRC;
        }

        public static DataTable nvk_thongTimBaohiemBy_idkhambenh(string idkhambenh)
        {
            string sqlSelect = @"SELECT IDBENHBHDONGTIEN  
                                        FROM KHAMBENH A0
                                        LEFT JOIN CHITIETDANGKYKHAM A ON A0.IDCHITIETDANGKYKHAM=A.IDCHITIETDANGKYKHAM
                                        LEFT JOIN DANGKYKHAM B ON A.IDDANGKYKHAM=B.IDDANGKYKHAM
                                        WHERE A0.IDKHAMBENH=" + idkhambenh;
            DataTable dt = DataAcess.Connect.GetTable(sqlSelect);
            if (dt == null || dt.Rows.Count == 0) return null;
            return nvk_thongTimBaohiemBy_idbnbhdt(dt.Rows[0][0].ToString());
        }

        public static DataTable nvk_thongTimBaohiemBy_idbnbhdt(string idbnbhdt)
        {
            string sqlInfor = @"SELECT top 1
            b.idbenhnhan,
            b.mabenhnhan,
            b.tenbenhnhan,
            diachi=(CASE WHEN ISNULL(B.noicongtac,'')='' THEN  b.diachi ELSE B.noicongtac end) ,TuoiBN=DBO.KB_Tuoi(B.NgaySinh),
            tengioitinh=dbo.getgioitinh(b.gioitinh),
            b.gioitinh,
            b.ngaysinh,
            b.chungminhthu,
            b.ngaytiepnhan,
            (CASE WHEN DONG.ISBHYT=1 THEN 1 ELSE 2 END) as loai,
            b.dienthoai,
            bnbh.sobhyt,
            convert(varchar(10),bnbh.ngaybatdau,103) as ngaybatdau,
            convert(varchar(10),bnbh.ngayhethan,103) as ngayhethan,
            bnbh.DungTuyen,
            noigioithieu=ngt.TENNOIDANGKY,
            noidangkykcb=ndk.TENNOIDANGKY,
			MaDangKy_KCB_bandau=ndk.MADANGKY,
            capcuu =dong.IsCapCuu,
    		thoihanthe = (case when  DONG.ISBHYT=1 then convert(varchar(10),bnbh.ngaybatdau,103)+N'  đến  '+ convert(varchar(10),bnbh.ngayhethan,103) else N'' end),
            chandoansobo=( SELECT  top 1 chandoanbandau from khambenh A1 LEFT JOIN DANGKYKHAM B1 ON A1.IDDANGKYKHAM=B1.IDDANGKYKHAM   WHERE B1.IDBENHBHDONGTIEN=" + idbnbhdt + @"  ORDER BY IDKHAMBENH   ),
            SoNgayDieuTri=CONVERT(INT,DONG.NgayTinhBH_Thuc-DONG.NgayTinhBH),
            idchitietdangkykham=DONG.IDCHITIETDANGKYKHAM_PREV,
            iddangkykham=DONG.IDDANGKYKHAM_DV,
            b.idbenhnhan,
            ngaynhapvien=DONG.NgayTinhBH,
            giovaovien= convert(varchar(5),DONG.NgayTinhBH,108),
            gioravien= convert(varchar(5),DONG.NgayTinhBH_Thuc,108),
            ngayxuatvien= DONG.NgayTinhBH_Thuc,
            NgayTinhBH_Thuc,
            IsNoiTru=Dong.IsNoiTru,
            DONG.ISBHYT,
            TenKhoa=(SELECT TENPHONGKHAMBENH FROM KHAMBENH A2 LEFT JOIN PHONGKHAMBENH B2 ON ISNULL(A2.IDKHOA,A2.IDPHONGKHAMBENH)=B2.IDPHONGKHAMBENH WHERE A2.IDKHAMBENH=DONG.IDKHAMBENH_LAST)
            ,NOICONGTAC=B.NOICONGTAC
             FROM    HS_BenhNhanBHDongTien dong 
                    left join benhnhan_bhyt bnbh on bnbh.idbenhnhan_bh=dong.idbenhnhan_bh
			        left join KB_NOIDANGKYKB ndk on ndk.IDNOIDANGKY= bnbh.IdNoiDangKyBH
			        left join KB_NOIDANGKYKB ngt on ngt.IDNOIDANGKY= bnbh.IdNoiGioiThieu
                    LEFT JOIN BENHNHAN B ON dong.IdBenhNhan=B.IdBenhNhan
            WHERE DONG.ID=" + idbnbhdt;
            DataTable dtBN = DataAcess.Connect.GetTable(sqlInfor);
            return dtBN;
        }

        private static void nvk_SetPara_HanhChinh(string idbenhnhan, string idctdkk, string idkhambenh,  KhamBenhPro.rptToaThuocBH crystalReport2)
        {

            DataTable dt_hanhChinh =nvk_thongTimBaohiemBy_idkhambenh(idkhambenh);
            string nvk_TenBenNhan = "";
            string nvk_MaBenhNhan = "";
            string nvk_NgaySinh = "";
            string nvk_GioiTinh = "";
            string nvk_DiaChi = "";
            string nvk_SoDienThoai = "";
            string nvk_Ngaybatdau = "";
            string nvk_Ngayhethan = "";
            string nvk_noiDkKcbBd = "";
            string nvk_noigioithieu = "";
            string nvk_chandoan = "";
            string nvk_strMaChanDoan = "";
            string nvk_bhyt = "";
           
            if (dt_hanhChinh != null && dt_hanhChinh.Rows.Count > 0)
            {
                
                nvk_TenBenNhan = dt_hanhChinh.Rows[0]["tenbenhnhan"].ToString();
                nvk_MaBenhNhan = dt_hanhChinh.Rows[0]["mabenhnhan"].ToString();
                nvk_NgaySinh = dt_hanhChinh.Rows[0]["ngaysinh"].ToString();
                nvk_GioiTinh = dt_hanhChinh.Rows[0]["tengioitinh"].ToString();
                nvk_DiaChi = dt_hanhChinh.Rows[0]["diachi"].ToString();
                nvk_SoDienThoai = dt_hanhChinh.Rows[0]["dienthoai"].ToString();
                nvk_Ngaybatdau =  dt_hanhChinh.Rows[0]["ngaybatdau"].ToString();
                nvk_Ngayhethan =  dt_hanhChinh.Rows[0]["ngayhethan"].ToString() ;
                nvk_noiDkKcbBd =  dt_hanhChinh.Rows[0]["noidangkykcb"].ToString();
                nvk_noigioithieu =  dt_hanhChinh.Rows[0]["noigioithieu"].ToString();
                nvk_setTongHopChanDoan_ByIdKhamBenh(idkhambenh, ref nvk_strMaChanDoan, ref nvk_chandoan);
                nvk_bhyt =dt_hanhChinh.Rows[0]["sobhyt"].ToString(); 
                //if (SoBhyt_Bn.Length > 10)
                //{
                //    if (IsBHYT == "1" && (string.IsNullOrEmpty(IsDV) || IsDV.Equals("0")))
                //    {
                //        nvk_bh1 = SoBhyt_Bn.Substring(0, 2);
                //        nvk_bh2 = SoBhyt_Bn.Substring(2, 1);
                //        nvk_bh3 = SoBhyt_Bn.Substring(3, 2);
                //        nvk_bh4 = SoBhyt_Bn.Substring(5, 2);
                //        nvk_bh5 = SoBhyt_Bn.Substring(7, 3);
                //        nvk_bh6 = SoBhyt_Bn.Substring(10, 5);
                //    }
                //    else
                //    {
                //        nvk_ThoiHanthe = "";
                //        nvk_noiDkKcbBd = "";
                //    }
                //}
            }
            try { crystalReport2.SetParameterValue("TenBN", nvk_TenBenNhan); }
            catch (Exception) { }
            try { crystalReport2.SetParameterValue("Ngaysinh", nvk_NgaySinh); }
            catch (Exception) { }
            try { crystalReport2.SetParameterValue("Gioitinh", nvk_GioiTinh); }
            catch (Exception) { }
            try { crystalReport2.SetParameterValue("Diachi", nvk_DiaChi); }
            catch (Exception) { }
            //try { crystalReport2.SetParameterValue("@nvk_SoDienThoai", nvk_SoDienThoai); }
            //catch (Exception) { }
            try { crystalReport2.SetParameterValue("NgayBatdau", nvk_Ngaybatdau); }
            catch (Exception) { }
            try { crystalReport2.SetParameterValue("NgayHethan", nvk_Ngayhethan); }
            catch (Exception) { }
            try { crystalReport2.SetParameterValue("NoiDKBD", nvk_noiDkKcbBd); }
            catch (Exception) { }
            try { crystalReport2.SetParameterValue("SoBHYT", nvk_bhyt); }
            catch (Exception) { }
            try { crystalReport2.SetParameterValue("NoiGT", nvk_noigioithieu); }
            catch (Exception) { }
             try { crystalReport2.SetParameterValue("ChanDoan", nvk_chandoan); }
                catch (Exception) { }
             try { crystalReport2.SetParameterValue("MaChanDoan", nvk_strMaChanDoan); }
                catch (Exception) { }

        }


        public static void nvk_setTongHopChanDoan_ByIdKhamBenh(string IdKhamBenh, ref string nvk_listMaIcd, ref string nvk_listMoTaIcd)
        {
            if (IdKhamBenh == null || IdKhamBenh == "") return;
            string nvk_sq = @"	SELECT distinct a.MOTACD_edit,b.maicd
	                FROM KHAMBENH a
		                left JOIN CHANDOANICD b ON a.KETLUAN=b.IDICD
	                WHERE a.ketluan>0 AND A.IdKhamBenh=" + IdKhamBenh + @"
                union ALL
	                SELECT distinct b.MOTACD_edit,c.maicd
	                FROM KHAMBENH A
		                inner JOIN CHANDOANPHOIHOP B ON  a.IDKHAMBENH=B.IDKHAMBENH
		                LEFT JOIN CHANDOANICD C ON  c.IDICD=b.IDICD
                       WHERE A.IdKhamBenh=" + IdKhamBenh;
            DataTable dt_cd = DataAcess.Connect.GetTable(nvk_sq);
            System.Collections.Generic.List<String> lstC = new System.Collections.Generic.List<string>();

            if (dt_cd != null && dt_cd.Rows.Count > 0)
            {
                nvk_listMoTaIcd = "";
                nvk_listMaIcd = "";
                for (int i = 0; i < dt_cd.Rows.Count; i++)
                {
                    if (lstC.IndexOf(dt_cd.Rows[i]["maicd"].ToString()) == -1)
                    {
                        nvk_listMaIcd += " " + dt_cd.Rows[i]["maicd"].ToString() + " -";
                        nvk_listMoTaIcd += " " + dt_cd.Rows[i]["MOTACD_edit"].ToString() + " -";
                        lstC.Add(nvk_listMaIcd);
                    }
                }
                nvk_listMaIcd = nvk_listMaIcd.TrimEnd('-');
                nvk_listMoTaIcd = nvk_listMoTaIcd.TrimEnd('-');
            }
        }


        private static DataTable loadDV(string idkhambenh)
        {
            
                string sW = "";
                string ColumTenDV = "tendichvu";

                string sql = @"select
                            tendichvu=bg." + ColumTenDV + @",
                            khoa.tenphongkhambenh,
                            soluong=cls.soluong
            from khambenhcanlamsan cls left join khambenh kb on kb.idkhambenh=cls.idkhambenh
            left join chitietdangkykham ctdk on ctdk.idchitietdangkykham=kb.idchitietdangkykham
            inner join banggiadichvu bg on bg.idbanggiadichvu=cls.idcanlamsan
            left join benhnhan bn on bn.idbenhnhan=kb.idbenhnhan
            left join phongkhambenh khoa on bg.idphongkhambenh= khoa.idphongkhambenh
            left join bacsi bs on bs.idbacsi=kb.idbacsi
            left join chandoanicd icd on icd.idicd=kb.ketluan
            where  isnull(cls.dahuy,0)=0 and cls.isbhyt_save=1 and cls.idkhambenh=" + idkhambenh + sW;
                //if (IsAllCLS != "1")
                //{
                //    sql += " AND ISNULL(cls.ISBNDaTra,0)=0 AND ISNULL(CLS.DATHU,0)=0";
                //}

                DataTable dt = DataAcess.Connect.GetTable(sql);
                return dt;
        }

    }
}
