using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcess;
using System.Data;
using System.Data.SqlClient;

namespace KhamBenhPro
{
    class hs_tinhtien
    {
        private static string FormatNumber(string s)
        {
            if (s.Length <= 3)
                return s;
            string str1 = "";
            int num = 0;
            for (int index = s.Length - 1; index >= 0; --index)
            {
                str1 += (string)(object)s[index].ToString();
                ++num;
                if (num == 3)
                {
                    num = 0;
                    str1 += ".";
                }
            }
            if (str1[str1.Length - 1] == '.')
                str1 = str1.Remove(str1.Length - 1, 1);
            string str2 = "";
            for (int index = str1.Length - 1; index >= 0; --index)
                str2 += (string)(object)str1[index].ToString();
            return str2;
        }

        public static string FormatSNumberToPrint(string s)
        {
            if (s == null || s == "" || s.Length <= 3)
                return s;
            s = s.Replace(",", "").Replace(".", "");
            string str1 = "";
            string str2 = "";
            if (s[0] == '-')
                str1 = "-";
            if (s[0] == '(')
                str1 = "(";
            if (s[s.Length - 1] == ')')
                str2 = ")";
            int length = s.IndexOf(".");
            string s1;
            string s2;
            string str3;
            if (length != -1)
            {
                s1 = s.Substring(str1.Length, length);
                s2 = s.Substring(length + 1, s.Length - str2.Length - length - 1);
                str3 = ".";
            }
            else
            {
                s1 = s.Substring(str1.Length, s.Length - str2.Length - str1.Length);
                s2 = "";
                str3 = "";
            }
            s = str1 + hs_tinhtien.FormatNumber(s1) + str3 + hs_tinhtien.FormatNumber(s2) + str2;
            return s;
        }

        public static string CheckDate(string strDate)
        {
            if (strDate == null || strDate == "")
                return "";
            if (strDate.Length == "08/01/2012 00:00:00".Length)
                strDate = strDate.Substring(0, "08/01/2012".Length);
            if (strDate.Length == "11/18/2012 12:07:00 PM".Length)
                strDate = strDate.Substring(0, "11/18/2012".Length);
            string[] strArray = strDate.Split('/');
            try
            {
                int num1 = Convert.ToInt32(strArray[0]);
                int num2 = Convert.ToInt32(strArray[1]);
                int int32 = Convert.ToInt32(strArray[2]);
                if (num2 > 12)
                {
                    int num3 = num2;
                    num2 = num1;
                    num1 = num3;
                }
                string str1 = num1.ToString().Trim();
                string str2 = num2.ToString().Trim();
                string str3 = int32.ToString().Trim();
                if (str1.Length == 1)
                    str1 = "0" + str1;
                if (str2.Length == 1)
                    str2 = "0" + str2;
                return str3 + "/" + str2 + "/" + str1;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static int int_Search(DataTable dtSearch, string s_Filter)
        {
            try
            {
                DataRow[] dataRowArray = dtSearch.Select(s_Filter);
                if (dataRowArray.Length == 0)
                    return -1;
                return dtSearch.Rows.IndexOf(dataRowArray[0]);
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static bool IsCheck(string value)
        {
            return value != null && !(value == "") && (!(value == "0") && !(value.ToLower() == "false") && !(value.ToLower() == "n"));
        }

        public static bool IsCheck(object value)
        {
            if (value == null || value.ToString() == "")
                return false;
            return hs_tinhtien.IsCheck(value.ToString());
        }

        public static string fixDouble(string s)
        {
            s = s.Replace(";", "-");
            string[] strArray = s.Split('-');
            List<string> stringList = new List<string>();
            string str = "";
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (strArray[index].Trim() != "" && stringList.IndexOf(strArray[index]) == -1)
                {
                    stringList.Add(strArray[index]);
                    str = str + strArray[index] + "-";
                }
            }
            if (str != "")
                str = str.Remove(str.Length - 1, 1);
            return str;
        }
        private static string SaveOtherInfor(bool IsBHYT)
        {
            return @"DECLARE  @IDKHAMBENH_LAST AS BIGINT
                    DECLARE @MA_KHOA AS NVARCHAR(50)                     
                    DECLARE @SOVAOVIEN AS NVARCHAR(20)                   
                    DECLARE @ket_qua_dtri AS INT         
                    DECLARE @TINH_TRANG_RV AS INT                     
                    DECLARE @ISXUATVIEN AS BIT                 
                    DECLARE @MACHANDOAN AS NVARCHAR(50) 
                    DECLARE @TENCHANDOAN AS NVARCHAR(500)
                    DECLARE @IDXUATKHOA AS BIGINT
                    DECLARE @NGAY_XUATKHOA AS DATETIME          
                    DECLARE @IDKHAMBENH_XUATKHOA AS BIGINT
                    DECLARE @ma_benhkhac  AS NVARCHAR(500)  
                    DECLARE @ChanDoanKhac AS NVARCHAR(400) 
                    DECLARE @HUONGDIEUTRI AS BIGINT
                    DECLARE @IDKHOA_LAST AS BIGINT
                    DECLARE @IDKHAMBENH_FIRST BIGINT   " + (IsBHYT ? @"
                    DECLARE @ITYPE AS INT  
                    SET @ITYPE=DBO.zHS_GetIType(@MATINH,@MADANGKY_KCB_BANDAU,@DUNGTUYEN,@ISCAPCUU)  " : "") + @"SELECT TOP 1 @IDXUATKHOA= A.IdXuatKhoa                 
                    ,@IDKHAMBENH_XUATKHOA=A.IdKhamBenhXK     
                    ,@NGAY_XUATKHOA=A.NgayXuatKhoa 
                    ,@IDKHAMBENH_LAST= A.IdKhamBenhXK
                    ,@IDKHAMBENH_FIRST= A.IdKhamBenhXK          
                    ,@NGAYTINHBH_THUC=A.NgayXuatKhoa  
                    ,@ket_qua_dtri=d.mahoa
                    ,@TINH_TRANG_RV=C.mahoa   
                    ,@MA_KHOA  =E.maphongkhambenh
                    ,@MACHANDOAN=G.MAICD
                    ,@TENCHANDOAN=ISNULL(A.MoTaCD_edit,F.MoTaCD_edit)
                    ,@ISXUATVIEN =  C.ISKetThuc 
                    ,@HUONGDIEUTRI=F.HUONGDIEUTRI
                    ,@IDKHOA_LAST=E.idphongkhambenh
                    from BenhNhanXuatKhoa a                 
                    INNER JOIN CHITIETDANGKYKHAM B ON A.IdChiTietDangKyKham=B.idchitietdangkykham  
                    LEFT JOIN KB_HUONGDIEUTRI C ON C.HuongDieuTriId=A.IdHuongDieuTri 
                    LEFT join KB_TinhTrangXuatKhoa d on a.idtinhTrang=D.idTinhTrang   
                    INNER JOIN PHONGKHAMBENH E ON A.IdKhoaXuat=E.idphongkhambenh  
                    LEFT JOIN KHAMBENH F ON A.IdKhamBenhXK   =F.IDKHAMBENH
                    LEFT JOIN CHANDOANICD G ON IsNull(F.ketluan,F.idchandoan)=G.IDICD
                    WHERE B.iddangkykham=@IDDANGKYKHAM               
                    ORDER BY A.NgayXuatKhoa DESC IF (@IDXUATKHOA IS   NULL)        BEGIN    
                    DECLARE @COUNTKB AS INT
                    SELECT @COUNTKB=COUNT(1)
                    FROM KHAMBENH KB
                    INNER JOIN DANGKYKHAM DK ON KB.IDDANGKYKHAM=DK.IDDANGKYKHAM
                    WHERE DK.IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN
                    SELECT TOP 1 @IDKHAMBENH_LAST= A.IDKHAMBENH
                    ,@IDKHAMBENH_FIRST= A.IDKHAMBENH  
                    FROM khambenh A               
                    INNER JOIN dangkykham B ON A.iddangkykham=B.iddangkykham
                    WHERE B.IdBenhBHDongTien=@IDBENHBHDONGTIEN                                                
                    ORDER BY  ISNULL( A.TGXuatVien,A.ngaykham)  DESC   SELECT  @NGAYTINHBH_THUC=ISNULL( TGXuatVien,ngaykham)   ,@MA_KHOA  =KHOA.MAPHONGKHAMBENH
                    ,@MACHANDOAN=G.MAICD
                    ,@TENCHANDOAN=ISNULL(A.MoTaCD_edit,G.MOTA)
                    ,@ISXUATVIEN =  C.ISKetThuc
                    ,@HUONGDIEUTRI=A.HUONGDIEUTRI
                    ,@ket_qua_dtri=(CASE WHEN tinhtrang.mahoa IS NULL AND IsChuyenVien=1 THEN '4' ELSE  ISNULL( tinhtrang.mahoa  ,'1') END )
                    ,@TINH_TRANG_RV=C.mahoa    
                    ,@IDKHOA_LAST= A.idphongkhambenh 
                    FROM khambenh A        
                    INNER JOIN dangkykham B ON A.iddangkykham=B.iddangkykham
                    LEFT JOIN KB_HUONGDIEUTRI C ON C.HuongDieuTriId=A.huongdieutri 
                    LEFT JOIN CHANDOANICD G ON IsNull(A.ketluan,A.idchandoan)=G.IDICD
                    LEFT JOIN PHONGKHAMBENH KHOA ON  KHOA.idphongkhambenh=A.idphongkhambenh
                    LEFT join KB_TinhTrangXuatKhoa tinhtrang on A.idtinhtrangxk=tinhtrang.idTinhTrang
                    WHERE A.IDKHAMBENH=@IDKHAMBENH_LAS IF (@COUNTKB>1)  BEGIN                        SELECT  TOP 1 
                    @IDKHAMBENH_FIRST= A.IDKHAMBENH        
                    ,@MACHANDOAN=G.MAICD
                    ,@TENCHANDOAN=ISNULL(A.MoTaCD_edit,G.MOTA)
                    FROM khambenh A         
                    INNER JOIN dangkykham B ON A.iddangkykham=B.iddangkykham 
                    LEFT JOIN CHANDOANICD G ON IsNull(A.ketluan,A.idchandoan)=G.IDICD
                    WHERE B.IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN
                    AND G.MAICD IS NOT NULL
                    ORDER BY  ISNULL( A.TGXuatVien,A.ngaykham)     END
                    set @ma_benhkhac=''
                    SELECT @ma_benhkhac=STUFF(( SELECT distinct ';'+D0.MaICD  FROM  chandoanphoihop  A0                                                                                  
                    INNER JOIN khambenh B0 ON A0.IDKHAMBENH=B0.IDKHAMBENH
                    INNER JOIN DANGKYKHAM C0 ON B0.IDDANGKYKHAM=C0.IDDANGKYKHAM
                    INNER JOIN CHANDOANICD D0 ON A0.IDICD=D0.IDICD
                    WHERE C0.IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN  
                    AND D0.MAICD<>@MACHANDOAN   FOR XML PATH('') ), 1, 1, '' )
                    set @ChanDoanKhac=''
                    SELECT @ChanDoanKhac=STUFF((SELECT distinct ';'+A0.MOTACD_EDIT  FROM  chandoanphoihop  A0                                                                           
                    INNER JOIN khambenh B0 ON A0.IDKHAMBENH=B0.IDKHAMBENH
                    INNER JOIN DANGKYKHAM C0 ON B0.IDDANGKYKHAM=C0.IDDANGKYKHAM 
                    WHERE C0.IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN
                    AND A0.MOTACD_EDIT<>@TENCHANDOAN
                    FOR XML PATH('')), 1, 1, '' ) 
                    IF (@COUNTKB>1)
                    BEGIN                                                                      
                    DECLARE @ma_benhkhac_other  AS NVARCHAR(500)
                    DECLARE @ChanDoanKhac_other AS NVARCHAR(400)
                    set @ma_benhkhac_other=''
                    SELECT @ma_benhkhac_other=STUFF((SELECT distinct ';'+D0.MaICD   FROM  khambenh B0   
                    INNER JOIN DANGKYKHAM C0 ON B0.IDDANGKYKHAM=C0.IDDANGKYKHAM                                                                                           
                    INNER JOIN CHANDOANICD D0 ON B0.KETLUAN=D0.IDICD                                                                                        
                    WHERE C0.IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN 
                    AND B0.IDKHAMBENH<> @IDKHAMBENH_FIRST
                    AND D0.MaICD<>@MACHANDOAN FOR XML PATH('')), 1, 1, '' )                                                                             
                    set @ChanDoanKhac_other=''                                                                          
                    SELECT @ChanDoanKhac_other=STUFF(( SELECT distinct ';'+B0.MOTACD_EDIT  FROM khambenh B0                                          
                    INNER JOIN DANGKYKHAM C0 ON B0.IDDANGKYKHAM=C0.IDDANGKYKHAM                                                                                          
                    INNER JOIN CHANDOANICD D0 ON B0.KETLUAN=D0.IDICD                                                                                               
                    WHERE C0.IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN                                                                     
                    AND B0.IDKHAMBENH<> @IDKHAMBENH_FIRST  
                    AND B0.MOTACD_EDIT<>@TENCHANDOAN                                                                                                
                    FOR XML PATH('') ), 1, 1, '' ) 
                    SET @ma_benhkhac=IsNull(@ma_benhkhac,'') + ISNULL(';'+ @ma_benhkhac_other,'')
                    SET @ChanDoanKhac=IsNull(@ChanDoanKhac,'') + ISNULL(';'+ @ChanDoanKhac_other,'') END END  ELSE   BEGIN
                    set @ma_benhkhac=''
                    SELECT @ma_benhkhac=STUFF((SELECT distinct ';'+B.MAICD  FROM  nvk_chanDoanXuatKhoa A
                    INNER JOIN CHANDOANICD B ON A.idicd=B.idicd
                    WHERE A.idxuatkhoa=@IDXUATKHOA  FOR XML PATH('') ), 1, 1, '' )
                    set @ChanDoanKhac=''
                    SELECT @ChanDoanKhac=STUFF((SELECT distinct ';'+A.MoTaChanDoan_XK
                    FROM  nvk_chanDoanXuatKhoa A
                    WHERE A.idxuatkhoa=@IDXUATKHOA FOR XML PATH('') ), 1, 1, '' ) END";
        }

        public static void FixChanDoan(string idbenhbhdongtien)
        {
            DataTable table = Connect.GetTable("\r\n                                SELECT ID,MACHANDOAN,TENCHANDOAN,ma_benhkhac,chandoankhac from HS_BENHNHANBHDONGTIEN WHERE ID=" + idbenhbhdongtien);
            if (table.Rows[0]["ma_benhkhac"].ToString() != "")
                table.Rows[0]["ma_benhkhac"] = (object)hs_tinhtien.fixDouble(table.Rows[0]["ma_benhkhac"].ToString());
            if (table.Rows[0]["ChanDoanKhac"].ToString() != "")
                table.Rows[0]["ChanDoanKhac"] = (object)hs_tinhtien.fixDouble(table.Rows[0]["ChanDoanKhac"].ToString());
            if (!(table.Rows[0]["ma_benhkhac"].ToString() != ""))
                return;
            Connect.ExecSQL("UPDATE HS_BENHNHANBHDONGTIEN SET IsFixChanDoan=1, ma_benhkhac=N'" + table.Rows[0]["ma_benhkhac"].ToString() + "',ChanDoanKhac=N'" + table.Rows[0]["ChanDoanKhac"].ToString() + "' WHERE ID=" + idbenhbhdongtien);
        }
        public static bool TinhTien(string idbenhbhdongtien, string iddangkykham, bool IsNewDKK)
        {
            return hs_tinhtien.TinhTien(idbenhbhdongtien, iddangkykham, IsNewDKK, true, true, true, true);
        }

        public static bool TinhTien(string idbenhbhdongtien, string iddangkykham, bool IsNewDKK, bool IsPhiKham, bool IsCLS, bool IsThuoc, bool IsTienGiuong)
        {
            if ((iddangkykham == null || iddangkykham == "") && idbenhbhdongtien != null && idbenhbhdongtien != "")
                iddangkykham = Connect.GetTable("SELECT TOP 1 IDDANGKYKHAM FROM DANGKYKHAM WHERE IDBENHBHDONGTIEN=" + idbenhbhdongtien).Rows[0][0].ToString();
            DataTable table1 = Connect.GetTable("SELECT 1 FROM DANGKYKHAM WHERE IDDANGKYKHAM=" + iddangkykham + " AND LoaiKhamID=1");
            if (table1 == null)
                return false;
            if (table1.Rows.Count == 0)
                return hs_tinhtien.TinhTienDV(idbenhbhdongtien, iddangkykham, IsNewDKK, IsPhiKham, IsCLS, IsThuoc, IsTienGiuong);
            string strCommandText = "\r\n                          DECLARE @iddangkykham AS BIGINT  \r\n                          SET @iddangkykham=" + iddangkykham + "\r\n\r\n                          DECLARE @IDBENHBHDONGTIEN AS BIGINT                      \r\n                          DECLARE @LOAIKHAMID AS INT                      \r\n                          DECLARE @NGAYDANGKY AS DATETIME                      \r\n                          DECLARE @IDBENHNHAN AS BIGINT                      \r\n                          DECLARE @LOAIBN AS INT                      \r\n                          DECLARE @DUNGTUYEN AS NVARCHAR(10)                      \r\n                          DECLARE @DUNGTUYEN_SAVE AS NVARCHAR(10)                      \r\n                          DECLARE @HOTENBN AS NVARCHAR(50)                      \r\n                          DECLARE @NGAYHETHAN AS DATETIME                      \r\n                          DECLARE @NGAYHETHAN2 AS DATETIME  \r\n                          DECLARE @IDLOAIBH AS BIGINT                      \r\n                          DECLARE @SOBHYT AS NVARCHAR(50)                      \r\n                          DECLARE @IDBENHNHAN_BH AS BIGINT                      \r\n                          DECLARE @IDBENHNHAN_BH2 AS BIGINT \r\n                          DECLARE @ISCAPCUU AS BIT                      \r\n                          DECLARE @NGAYTRINHTHE AS DATETIME                      \r\n                          DECLARE @NGAYTRINHTHE2 AS DATETIME\r\n                          DECLARE @IDKHOADK AS BIGINT                     \r\n                          DECLARE @MADANGKY_KCB_BANDAU AS NVARCHAR(20)                      \r\n                          DECLARE @MATINH AS NVARCHAR(20)                      \r\n                          DECLARE @SOBH1 AS NVARCHAR(10)                      \r\n                          DECLARE @SOBH2 AS NVARCHAR(10)                      \r\n                          DECLARE @IS100 BIT          \r\n                          DECLARE @NGAYTINHBH   AS DATETIME                 \r\n                          DECLARE @NGAYTINHBH_THUC   AS DATETIME         \r\n                          DECLARE @IDPHANLOAI BIGINT         \r\n                    \r\n                           SELECT                \r\n                            @IDBENHNHAN=A.IDBENHNHAN,                      \r\n                            @NGAYDANGKY=A.NGAYDANGKY,                      \r\n                            @LOAIBN=A.LOAIKHAMID,                      \r\n                            @DUNGTUYEN =B0.DUNGTUYEN,                      \r\n                            @DUNGTUYEN_SAVE=B0.DUNGTUYEN,                      \r\n                            @HOTENBN=B.TENBENHNHAN,                      \r\n                            @MADANGKY_KCB_BANDAU=D.MADANGKY,                      \r\n                            @NGAYHETHAN=  CONVERT(DATETIME, CONVERT(VARCHAR, ISNULL( B0.NGAYHETHAN,GETDATE()), 111)+' 23:59:59'),   \r\n                            @NGAYHETHAN2=  CONVERT(DATETIME, CONVERT(VARCHAR, ISNULL( B1.NGAYHETHAN,GETDATE()), 111)+' 23:59:59'),                   \r\n                            @IDLOAIBH=C.ID,                      \r\n                            @SOBHYT=B0.SOBHYT,                      \r\n                            @IDBENHNHAN_BH=A.IDBENHNHAN_BH,                      \r\n                            @IDBENHNHAN_BH2=A.IDBENHNHAN_BH2, \r\n                            @ISCAPCUU=B0.ISCAPCUU,                      \r\n                            @NGAYTRINHTHE=  CONVERT(VARCHAR,  ISNULL(A.NGAYTRINHTHE,A.NGAYDANGKY), 111)+' 00:00:00'          \r\n                            ,@NGAYTRINHTHE2=  CONVERT(VARCHAR,  A.NGAYTRINHTHE2, 111)+' 00:00:00'\r\n                            ,@IDKHOADK=(SELECT TOP 1  IDKHOA FROM CHITIETDANGKYKHAM WHERE IDDANGKYKHAM=A.IDDANGKYKHAM )                      \r\n                            ,@MATINH=B0.SOBH3                      \r\n                            ,@SOBH1=B0.sobh1                      \r\n                            ,@SOBH2=B0.sobh2                      \r\n                            ,@IS100=A.IS100                      \r\n                            ,@IDBENHBHDONGTIEN=A.IDBENHBHDONGTIEN                \r\n                            ,@LOAIKHAMID=A.LOAIKHAMID         \r\n                            ,@NGAYTINHBH=ISNULL(HS.NGAYTINHBH,A.NGAYDANGKY)          \r\n                            ,@NGAYTINHBH_THUC=ISNULL(HS.NGAYTINHBH_THUC,A.NGAYDANGKY)         \r\n                            ,@IDPHANLOAI   =A.IDPHANLOAI        \r\n                           FROM DANGKYKHAM A                      \r\n                           INNER JOIN BENHNHAN B ON A.IDBENHNHAN=B.IDBENHNHAN                      \r\n                           left JOIN BENHNHAN_BHYT B0 ON A.IDBENHNHAN_BH=B0.IDBENHNHAN_BH                      \r\n                           left JOIN BENHNHAN_BHYT B1 ON A.IDBENHNHAN_BH2=B1.IDBENHNHAN_BH\r\n                           left JOIN KB_DOITUONGBH C ON  B0.SOBH2=C.DOITUONG                      \r\n                           left JOIN KB_NOIDANGKYKB D ON B0.IDNOIDANGKYBH=D.IDNOIDANGKY           \r\n                           LEFT JOIN HS_BENHNHANBHDONGTIEN HS ON A.IDBENHBHDONGTIEN=HS.ID                   \r\n                           WHERE IDDANGKYKHAM=@IDDANGKYKHAM  \r\n                                 AND A.LOAIKHAMID=1 \r\n                        \r\n                           IF(@IDKHOADK=15)\r\n                             BEGIN\r\n                                        IF (@ISCAPCUU<>1 OR @DUNGTUYEN<>N'Y') \r\n                                             BEGIN\r\n                                                    Update BENHNHAN_BHYT set ISCAPCUU=1,DUNGTUYEN='Y', IsDungTuyen=1 where IDBENHNHAN_BH=@IDBENHNHAN_BH\r\n                                                    Update BENHNHAN_BHYT set ISCAPCUU=1,DUNGTUYEN='Y', IsDungTuyen=1 where IDBENHNHAN_BH=@IDBENHNHAN_BH2\r\n                                             END\r\n                                        SET @ISCAPCUU=1\r\n                                        SET @DUNGTUYEN =N'Y'                     \r\n                                        SET @DUNGTUYEN_SAVE=N'Y'       \r\n                             END\r\n                            \r\n                          if (@IDBENHBHDONGTIEN is not null)                    \r\n                                BEGIN                    \r\n\t\t                        IF NOT EXISTS (SELECT 1 FROM HS_BENHNHANBHDONGTIEN WHERE ID=@IDBENHBHDONGTIEN)           \r\n\t\t\t                         SET @IDBENHBHDONGTIEN=NULL \r\n\t\t                        END                         \r\n                    \r\n                               IF (@IDBENHBHDONGTIEN IS NULL)           \r\n                                  BEGIN                    \r\n                                                     ------------------GET IDBENHBHDONGTIEN--------------------                    \r\n                                                     SELECT TOP 1 @IDBENHBHDONGTIEN=ID                    \r\n                                                     FROM HS_BENHNHANBHDONGTIEN                    \r\n                                                      WHERE IDBENHNHAN=@IDBENHNHAN                    \r\n                                                      AND DBO.HS_EQUARDAY1(@NGAYDANGKY,NGAYTINHBH)=1                    \r\n                                                       AND ISNULL(IsXuatVien,0)=0                    \r\n                                                       AND ISNULL( IDKHOA_DK,0)=@IDKHOADK                    \r\n                                                       AND ISNULL(ISBHYT,0)=1                    \r\n                                                       AND IDBENHNHAN_BH=@IDBENHNHAN_BH                    \r\n                                                        ----------------------IDBENHBHDONGTIEN NOT EXIST------------------------                    \r\n                                                 IF (@IDBENHBHDONGTIEN IS NULL)                    \r\n                                                   BEGIN                    \r\n\t\t\t                                                       INSERT HS_BENHNHANBHDONGTIEN(                    \r\n\t\t\t                                                       ISBHYT                    \r\n\t\t\t                                                       ,IDBENHNHAN                    \r\n\t\t\t                                                       ,NGAYTINHBH                    \r\n\t\t\t                                                       ,NgayTinhBH_Thuc                    \r\n\t\t\t                                                       ,IDDANGKYKHAM_DV                    \r\n\t\t\t                                                       ,HOTENBN                    \r\n\t\t\t                                                       ,IsXuatVien                    \r\n\t\t\t                                                       ,IDKHOA_DK                    \r\n\t\t\t                                                       ,IDBENHNHAN_BH\r\n                                                                   ,IDBENHNHAN_BH2   \r\n                                                                   ,DungTuyen\r\n                                                                   ,ISCAPCUU                 \r\n                                                                   )                    \r\n                                                                 VALUES (                    \r\n\t\t\t                                                       1                    \r\n\t\t\t                                                       ,@IDBENHNHAN                    \r\n\t\t\t                                                       ,@NGAYDANGKY                    \r\n\t\t\t                                                       ,@NGAYDANGKY                    \r\n\t\t\t                                                       ,@IDDANGKYKHAM                     \r\n\t\t\t                                                       ,@HOTENBN                    \r\n\t\t\t                                                       ,0                    \r\n\t\t\t                                                       ,@IDKHOADK                    \r\n\t\t\t                                                       ,@IDBENHNHAN_BH \r\n                                                                   ,@IDBENHNHAN_BH2\r\n                                                                   ,@DungTuyen  \r\n                                                                   ,@ISCAPCUU                      \r\n                                                                  )                    \r\n\t\t\t                                                    SELECT TOP 1 @IDBENHBHDONGTIEN=ID                    \r\n\t\t\t                                                      FROM HS_BenhNhanBHDongTien                    \r\n\t\t\t                                                      WHERE IDBENHNHAN=@IDBENHNHAN                    \r\n\t\t\t                                                      ORDER BY ID DESC                     \r\n                                                      END           \r\n                                     \r\n                                                  UPDATE dangkykham SET IdBenhBHDongTien=@IDBENHBHDONGTIEN WHERE iddangkykham=@IDDANGKYKHAM                   \r\n                                           END                    \r\n\t\t                                ELSE                    \r\n\t\t\t\t                          BEGIN                    \r\n\t\t\t\t\t\t\t                        IF ( @NGAYTINHBH IS NULL ) SET @NGAYTINHBH=@NGAYDANGKY                    \r\n\t\t\t\t\t\t\t                        ELSE                    \r\n\t\t\t\t\t\t\t                        IF(@NGAYDANGKY<@NGAYTINHBH) SET @NGAYTINHBH=@NGAYDANGKY                    \r\n\t\t\t\t                          END \r\n                            IF (@IDPHANLOAI IS NOT NULL)\r\n                               BEGIN\r\n                                            DECLARE \r\n\t                                            @IsMP_KB AS BIT,\r\n\t                                            @IsMP_CLS AS BIT,\r\n\t                                            @IsMP_Giuong AS BIT,\r\n\t                                            @IsMP_PhauThuat AS BIT,\r\n                                                @idchuongtrinh AS bigint\r\n                                            SELECT TOP 1 \r\n\t                                            @IsMP_KB=IsMP_KB,\r\n\t                                            @IsMP_CLS=IsMP_CLS,\r\n\t                                            @IsMP_Giuong=IsMP_Giuong,\r\n\t                                            @IsMP_PhauThuat=IsMP_PhauThuat,\r\n                                                @idchuongtrinh=idchuongtrinh\r\n                                             FROM KB_ChuongTrinh_MP_ChenhLechBHYT\r\n                                            WHERE IDPHANLOAI=@IDPHANLOAI\r\n                                            AND TUNGAY<=@NGAYDANGKY\r\n                                            ORDER BY TUNGAY DESC \r\n\r\n                                        IF (@idchuongtrinh IS NOT NULL)\r\n                                             UPDATE dangkykham SET  IsMP_KB=@IsMP_KB,\r\n\t                                            IsMP_CLS=@IsMP_CLS,\r\n\t                                            IsMP_Giuong=@IsMP_Giuong,\r\n\t                                            IsMP_PhauThuat=@IsMP_PhauThuat WHERE iddangkykham=@IDDANGKYKHAM  \r\n                                END\r\n                   \r\n                         ";
            if (!IsNewDKK)
                strCommandText = strCommandText + hs_tinhtien.SaveOtherInfor(true) + "\r\n        \r\n                              UPDATE HS_BENHNHANBHDONGTIEN SET\r\n                                                ISBHYT=1\r\n                                                ,IDBENHNHAN_BH=@IDBENHNHAN_BH\r\n                                                ,IDBENHNHAN_BH2=@IDBENHNHAN_BH2\r\n                                                ,NGAYTRINHTHE=@NGAYTRINHTHE\r\n                                                ,NGAYTRINHTHE2=@NGAYTRINHTHE2\r\n                                                ,SOBHYT=@SOBHYT\r\n                                                ,DungTuyen=@DungTuyen \r\n                                                ,ISCAPCUU=@ISCAPCUU \r\n                                                ,IDKHAMBENH_LAST=@IDKHAMBENH_LAST\r\n                                                ,@IDKHOA_LAST=@IDKHOA_LAST\r\n                                                ,TINH_TRANG_RV=@TINH_TRANG_RV\r\n                                                ,ket_qua_dtri=@ket_qua_dtri\r\n                                                ,MACHANDOAN=@MACHANDOAN\r\n                                                ,TENCHANDOAN=@TENCHANDOAN\r\n                                                ,ma_benhkhac=   @ma_benhkhac\r\n                                                ,ChanDoanKhac=@ChanDoanKhac\r\n                                                ,IsXuatVien=@ISXUATVIEN\r\n                                                ,ITYPE=@ITYPE\r\n                                                ,NGAYTINHBH_THUC=@NGAYTINHBH_THUC\r\n                                                ,IDKHAMBENH_FIRST=@IDKHAMBENH_FIRST\r\n                                                ,SongayDT=\r\n                                                             (case when isnull(ISNOITRU,0)=0 THEN 0 ELSE            \r\n                                                                   (CASE WHEN CONVERT(VARCHAR,NGAYTINHBH,111)=CONVERT(VARCHAR,@NgayTinhBH_Thuc,111) THEN 1 ELSE    CONVERT(INT, CONVERT(DATETIME, CONVERT(VARCHAR, @NgayTinhBH_Thuc,111))-CONVERT(DATETIME, CONVERT(VARCHAR, NgayTinhBH,111)))  +(CASE WHEN @HUONGDIEUTRI=4 OR @HUONGDIEUTRI=24 THEN 1 ELSE 0 END ) END)                \r\n                                                             END)        \r\n                                                                        \r\n                                    WHERE ID=@IDBENHBHDONGTIEN";
            Connect.ExecSQL(strCommandText);
            DataTable table2 = Connect.GetTable("SELECT IDBENHBHDONGTIEN FROM DANGKYKHAM WHERE IdDangKyKham=" + iddangkykham);
            if (table2 == null || table2.Rows.Count == 0)
                return false;
            idbenhbhdongtien = table2.Rows[0]["IDBENHBHDONGTIEN"].ToString();
            hs_tinhtien.FixChanDoan(idbenhbhdongtien);
            return hs_tinhtien.TinhTienBH(idbenhbhdongtien, iddangkykham, IsNewDKK, IsPhiKham, IsCLS, IsThuoc, IsTienGiuong);
        }

        private static bool TinhTienBH(string idbenhbhdongtien, string iddangkykham, bool IsNewDKK, bool IsPhiKham, bool IsCLS, bool IsThuoc, bool IsTienGiuong)
        {
            DateTime now = DateTime.Now;
            double num1 = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;
            double num8 = 0.0;
            double num9 = 0.0;
            double num10 = 0.0;
            double num11 = 0.0;
            double num12 = 0.0;
            double num13 = 0.0;
            double num14 = 0.0;
            double num15 = 0.0;
            double num16 = 0.0;
            double num17 = 0.0;
            double num18 = 0.0;
            double num19 = 0.0;
            double num20 = 0.0;
            string strSelect1 = " select \r\n                                        A.idchitietdangkykham \r\n                                        ,DonGiaDV=( CASE WHEN D.ISBHYT=1 AND (B.IsMienPhiChenhLech=1 OR B.IsMP_KB=1) AND ( ISNULL(B.NGAYTRINHTHE,B.NGAYDANGKY)<=B.NGAYDANGKY  OR ISNULL(B.NGAYTRINHTHE2,B.NGAYDANGKY)<=B.NGAYDANGKY ) THEN D.GIABH ELSE  D.GiaDV END)\r\n                                        ,ThanhTienDV=( CASE WHEN D.ISBHYT=1 AND (B.IsMienPhiChenhLech=1 OR B.IsMP_KB=1) AND ( ISNULL(B.NGAYTRINHTHE,B.NGAYDANGKY)<=B.NGAYDANGKY  OR ISNULL(B.NGAYTRINHTHE2,B.NGAYDANGKY)<=B.NGAYDANGKY ) THEN D.GIABH ELSE  D.GiaDV END)\r\n                                        ,DonGiaBH=(CASE WHEN D.IsBHYT=1 AND ( ISNULL(B.NGAYTRINHTHE,B.NGAYDANGKY)<=B.NGAYDANGKY  OR ISNULL(B.NGAYTRINHTHE2,B.NGAYDANGKY)<=B.NGAYDANGKY ) THEN D.GiaBH ELSE  0 END)\r\n                                        ,ThanhTienBH=(CASE WHEN D.IsBHYT=1 AND ( ISNULL(B.NGAYTRINHTHE,B.NGAYDANGKY)<=B.NGAYDANGKY  OR ISNULL(B.NGAYTRINHTHE2,B.NGAYDANGKY)<=B.NGAYDANGKY ) THEN D.GiaBH ELSE  0 END)\r\n                                        ,IsBHYT=(CASE WHEN D.IsBHYT=1  AND ( ISNULL(B.NGAYTRINHTHE,B.NGAYDANGKY)<=B.NGAYDANGKY  OR ISNULL(B.NGAYTRINHTHE2,B.NGAYDANGKY)<=B.NGAYDANGKY ) THEN 1 ELSE 0 END)\r\n                                        ,PhuThuBH=(CASE WHEN D.IsBHYT=1  AND ( ISNULL(B.NGAYTRINHTHE,B.NGAYDANGKY)<=B.NGAYDANGKY  OR ISNULL(B.NGAYTRINHTHE2,B.NGAYDANGKY)<=B.NGAYDANGKY ) THEN (CASE WHEN ISNULL(B.IsMienPhiChenhLech,0)=0 AND ISNULL(B.IsMP_KB,0)=0 THEN  D.GiaDV-D.GiaBH ELSE 0 END)  ELSE  0 END)\r\n                                        ,BNTra=0\r\n                                        ,BHTra=0\r\n                                        ,TONGTIENBNPT=0\r\n                                        ,TIENGIADV=(CASE WHEN D.IsBHYT=1  AND ( ISNULL(B.NGAYTRINHTHE,B.NGAYDANGKY)<=B.NGAYDANGKY  OR ISNULL(B.NGAYTRINHTHE2,B.NGAYDANGKY)<=B.NGAYDANGKY ) THEN 0 ELSE  D.GiaDV END)\r\n                                        ,B.IsMienPhiChenhLech\r\n                                        ,IsSoKB=(CASE WHEN A.IDBANGGIADICHVU=628 THEN '1' ELSE '0' END)\r\n                                        ,STGiam=D.STGiam\r\n                                        ,NGAYTRINHTHE=CONVERT(NVARCHAR,B.NGAYTRINHTHE,111)\r\n                                        ,NGAYDANGKY=CONVERT(NVARCHAR,B.NGAYDANGKY,111)\r\n                                        ,b.IsMP_KB\r\n                                        ,b.IsMP_CLS\r\n                                        ,b.IsMP_Giuong\r\n                                        ,B.IsMP_PhauThuat\r\n                                        ,idphanloai=B.idphanloai\r\n                              from chitietdangkykham a\r\n                             inner join dangkykham b on a.iddangkykham=b.iddangkykham\r\n                             inner join banggiadichvu c on a.idbanggiadichvu=c.idbanggiadichvu\r\n                             inner join hs_banggiavienphi d on d.IdGiaDichVu=(SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi D0 WHERE D0.IdDichVu=a.idbanggiadichvu AND D0.TuNgay<=B.NGAYDANGKY ORDER BY D0.TUNGAY DESC )\r\n                             where b.IdBenhBHDongTien=" + idbenhbhdongtien + "\r\n                                     AND ISNULL(A.dahuy,0)=0\r\n                                     AND ISNULL(A.isNotThuPhiCapCuu,0)=0\r\n                                ORDER BY   (CASE WHEN A.IDBANGGIADICHVU=628 THEN 1 ELSE 2 END)\r\n                             ";
            DataTable dataTable1 = (DataTable)null;
            if (IsPhiKham)
                dataTable1 = Connect.GetTable(strSelect1);
            if (dataTable1 != null && dataTable1.Rows.Count > 0)
            {
                if (dataTable1.Rows.Count >= 2)
                {
                    bool flag = true;
                    int index1 = 0;
                    if (dataTable1.Rows[0]["IsSoKB"].ToString() == "1")
                    {
                        index1 = 1;
                        if (dataTable1.Rows.Count == 2)
                            flag = false;
                    }
                    int num21 = 0;
                    if (flag)
                    {
                        double.Parse(dataTable1.Rows[index1]["DonGiaDV"].ToString());
                        double num22 = double.Parse(dataTable1.Rows[index1]["DonGiaBH"].ToString());
                        for (int index2 = index1 + 1; index2 < dataTable1.Rows.Count; ++index2)
                        {
                            if (!hs_tinhtien.IsCheck(dataTable1.Rows[index2]["IsMienPhiChenhLech"].ToString()) && !hs_tinhtien.IsCheck(dataTable1.Rows[index2]["IsMP_KB"].ToString()))
                            {
                                dataTable1.Rows[index2]["DonGiaDV"] = (object)(double.Parse(dataTable1.Rows[index2]["DonGiaDV"].ToString()) - double.Parse(dataTable1.Rows[index2]["STGiam"].ToString() == "" ? "0" : dataTable1.Rows[index2]["STGiam"].ToString()));
                                dataTable1.Rows[index2]["TIENGIADV"] = dataTable1.Rows[index2]["DonGiaDV"];
                                dataTable1.Rows[index2]["ThanhTienDV"] = dataTable1.Rows[index2]["DonGiaDV"];
                            }
                            if (hs_tinhtien.IsCheck(dataTable1.Rows[index2]["IsBHYT"].ToString()))
                            {
                                ++num21;
                                double num23 = 0.0;
                                if (num21 >= 1 && num21 <= 3)
                                    num23 = 0.3;
                                else if (num21 == 4)
                                    num23 = 0.1;
                                else if (num23 >= 5.0)
                                {
                                    num23 = 0.0;
                                    dataTable1.Rows[index2]["IsBHYT"] = (object)false;
                                }
                                dataTable1.Rows[index2]["DonGiaBH"] = (object)(num22 * num23);
                                dataTable1.Rows[index2]["ThanhTienBH"] = (object)(num22 * num23);
                                if (num23 != 0.0 && num23 != 1.0 && (hs_tinhtien.IsCheck(dataTable1.Rows[index2]["IsMienPhiChenhLech"].ToString()) || hs_tinhtien.IsCheck(dataTable1.Rows[index2]["IsMP_KB"].ToString())))
                                {
                                    dataTable1.Rows[index2]["ThanhTienDV"] = (object)(double.Parse(dataTable1.Rows[index2]["ThanhTienDV"].ToString()) * num23);
                                    dataTable1.Rows[index2]["TIENGIADV"] = (object)(double.Parse(dataTable1.Rows[index2]["TIENGIADV"].ToString()) * num23);
                                }
                                if (hs_tinhtien.IsCheck(dataTable1.Rows[index2]["IsBHYT"].ToString()) && (!hs_tinhtien.IsCheck(dataTable1.Rows[index2]["IsMienPhiChenhLech"].ToString()) && !hs_tinhtien.IsCheck(dataTable1.Rows[index2]["IsMP_KB"].ToString())))
                                    dataTable1.Rows[index2]["PhuThuBH"] = (object)(double.Parse(dataTable1.Rows[index2]["ThanhTienDV"].ToString()) - double.Parse(dataTable1.Rows[index2]["ThanhTienBH"].ToString()));
                            }
                        }
                    }
                }
                num5 = double.Parse(dataTable1.Compute("SUM(DonGiaBH)", "").ToString());
                num1 = double.Parse(dataTable1.Compute("SUM(DonGiaBH)", "").ToString());
                num20 = double.Parse(dataTable1.Compute("SUM(ThanhTienDV)", "").ToString());
                num7 = double.Parse(dataTable1.Compute("SUM(PhuThuBH)", "").ToString());
                num8 = double.Parse(dataTable1.Compute("SUM(TIENGIADV)", "").ToString());
            }
            string str1 = "(CASE WHEN A.per50=1 THEN 0.5 ELSE (CASE WHEN A.per80=1 THEN 0.8 ELSE 1.0 END) END)*";
            string strSelect2 = @"select  a.idkhambenhcanlamsan
                ,IDBENHBHDONGTIEN=c.IdBenhBHDongTien
                ,soluong=ISNULL(a.soluong,1)
                ,DonGiaDV= " + str1 + @"D.GiaDV
                ,ThanhTienDV=" + str1 + @"ISNULL(a.soluong,1)*D.GiaDV
                ,DonGiaBH_TEMP=" + str1 + @"(CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END)
                ,DonGiaBH=" + str1 + @"(CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END)
                ,ISBHYT=(CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN 1 ELSE 0 END)
                ,ThanhTienBH=" + str1 + @"ISNULL(a.soluong,1)*(CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END)\r\n\t\t                                            ,IDNHOMINBV=E.IDNHOMINBV\r\n\t\t                                            
                ,TONGTIENBH=" + str1 + @"ISNULL(a.soluong,1)*(CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END)                                           
               ,TIENCLS=" + str1 + @"ISNULL(a.soluong,1)*(CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END)                                       
               ,PhuThuBH=" + str1 + @"ISNULL(a.soluong,1)*(CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaDV- D.GiaBH ELSE 0 END)                                       
               ,TIENGIADV=" + str1 + @"(CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN 0 ELSE ISNULL(a.soluong,1)*D.GiaDV END)                                            
                ,XN= " + str1 + @"ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=3  THEN (CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END) ELSE 0 END)                                          
               ,CDHA= " + str1 + @"ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV IN( 4,5)THEN (CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END) ELSE 0 END)                                           
               ,TIEMTRUYEN= " + str1 + @"ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=8 THEN (CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END) ELSE 0 END)                                        
               ,THUTHUAT= " + str1 + @"ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=6 THEN (CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END) ELSE 0 END)                                           
               ,DVKTCAO= " + str1 + @"ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=7 THEN (CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END) ELSE 0 END)                                            
               ,VANCHUYEN= " + str1 + @"ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=9 THEN (CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END) ELSE 0 END)                                            
               ,CLSKHAC= " + str1 + @"ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=10 THEN (CASE WHEN (ISNULL(C.NGAYTRINHTHE ,C.NGAYTRINHTHE2) IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.ngaythu   OR C.NGAYTRINHTHE2<=A.ngaythu  ) AND A.IsBHYT_Save=1 AND D.IsBHYT=1 THEN D.GiaBH ELSE 0 END) ELSE 0 END)                                                    
               ,BNTra=0                                                    
               ,BHTra=0                                                  
               ,TONGTIENBNPT=0                                                   
               ,NGAYXUAT=A.NGAYTHU                                                    
            ,IsHave_IsMP_CLS=(CASE WHEN  E.IsCLS=1 AND C.IsMP_CLS=1 OR (E.IdPhongChucNang=20  AND C.IsMP_PhauThuat=1) THEN 1 ELSE 0 END)
            from khambenhcanlamsan a                                            
            inner join khambenh b on a.idkhambenh=b.idkhambenh                                        
            inner join dangkykham c on b.iddangkykham=c.iddangkykham                                            
            inner join hs_banggiavienphi d on d.IdGiaDichVu=(select top 1 IdGiaDichVu from hs_banggiavienphi D0 WHERE D0.IdDichVu=A.idcanlamsan AND D0.TuNgay<=A.ngaythu ORDER BY D0.TuNgay  DESC)                                          
            INNER JOIN BANGGIADICHVU E ON A.idcanlamsan=E.idbanggiadichvu                                       
            where  c.IdBenhBHDongTien=" + idbenhbhdongtien + @" 
            ORDER BY A.NGAYTHU ";
            DataTable dataTable2 = (DataTable)null;
            if (!IsNewDKK && IsCLS)
                dataTable2 = Connect.GetTable(strSelect2);
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable2.Rows.Count; ++index)
                {
                    if (hs_tinhtien.IsCheck(dataTable2.Rows[index]["ISBHYT"].ToString()) && hs_tinhtien.IsCheck(dataTable2.Rows[index]["IsHave_IsMP_CLS"].ToString()))
                    {
                        dataTable2.Rows[index]["PhuThuBH"] = (object)"0";
                        dataTable2.Rows[index]["DonGiaDV"] = dataTable2.Rows[index]["DonGiaBH"];
                        dataTable2.Rows[index]["ThanhTienDV"] = dataTable2.Rows[index]["ThanhTienBH"];
                    }
                }
                num6 += double.Parse(dataTable2.Compute("SUM(ThanhTienBH)", "").ToString());
                num1 += double.Parse(dataTable2.Compute("SUM(ThanhTienBH)", "").ToString());
                num20 += double.Parse(dataTable2.Compute("SUM(ThanhTienDV)", "").ToString());
                num7 += double.Parse(dataTable2.Compute("SUM(PhuThuBH)", "").ToString());
                num10 += double.Parse(dataTable2.Compute("SUM(XN)", "").ToString());
                num11 += double.Parse(dataTable2.Compute("SUM(CDHA)", "").ToString());
                num12 += double.Parse(dataTable2.Compute("SUM(TIEMTRUYEN)", "").ToString());
                num13 += double.Parse(dataTable2.Compute("SUM(THUTHUAT)", "").ToString());
                num16 += double.Parse(dataTable2.Compute("SUM(DVKTCAO)", "").ToString());
                num17 += double.Parse(dataTable2.Compute("SUM(VANCHUYEN)", "").ToString());
                num18 += double.Parse(dataTable2.Compute("SUM(CLSKHAC)", "").ToString());
            }
            DataTable dataTable3 = dataTable1;
            string strSelect3 = "select \r\n\t\t                                A.idchitietphieuxuat\r\n\t\t                                ,IDBENHBHDONGTIEN=c.IdBenhBHDongTien\r\n\t\t                                ,soluong=ISNULL(a.soluong,0)-isnull(A.sl_tra,0)\r\n\t\t                                ,DonGiaDV=ISNULL(A.DonGiaDV_TEMP,E.GIA_MUA)\r\n\t\t                                ,ThanhTienDV=(A.SOLUONG-isnull(A.sl_tra,0) )*ISNULL(A.DonGiaDV_TEMP,E.GIA_MUA)\r\n\t\t                                ,DonGiaBH_TEMP=(CASE WHEN (ISNULL(C.NGAYTRINHTHE,C.NGAYTRINHTHE2)  IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.NgayThang_Xuat OR C.NGAYTRINHTHE2<=A.NgayThang_Xuat ) AND D.IsBHYT_Save=1 AND E.sudungchobh=1  THEN ISNULL(A.DonGiaBH_TEMP,(CASE WHEN E.GIA_MUA<=E.GIA_THAU THEN E.GIA_MUA ELSE E.GIA_THAU END)) ELSE 0 END)\r\n\t\t                                ,DonGiaBH=(CASE WHEN (ISNULL(C.NGAYTRINHTHE,C.NGAYTRINHTHE2)  IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.NgayThang_Xuat OR C.NGAYTRINHTHE2<=A.NgayThang_Xuat ) AND D.IsBHYT_Save=1 AND E.sudungchobh=1 THEN ISNULL(A.DonGiaBH_TEMP,(CASE WHEN E.GIA_MUA<=E.GIA_THAU THEN E.GIA_MUA ELSE E.GIA_THAU END)) ELSE 0 END)\r\n\t\t                                ,ISBHYT=(CASE WHEN (ISNULL(C.NGAYTRINHTHE,C.NGAYTRINHTHE2)  IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.NgayThang_Xuat OR C.NGAYTRINHTHE2<=A.NgayThang_Xuat ) AND D.IsBHYT_Save=1 AND E.sudungchobh=1 THEN 1 ELSE 0 END)\r\n\t\t                                ,ThanhTienBH=(A.SOLUONG-isnull(A.sl_tra,0) )*(CASE WHEN (ISNULL(C.NGAYTRINHTHE,C.NGAYTRINHTHE2)  IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.NgayThang_Xuat OR C.NGAYTRINHTHE2<=A.NgayThang_Xuat ) AND D.IsBHYT_Save=1 AND E.sudungchobh=1 THEN ISNULL(A.DonGiaBH_TEMP,(CASE WHEN E.GIA_MUA<=E.GIA_THAU THEN E.GIA_MUA ELSE E.GIA_THAU END)) ELSE 0 END)\r\n\t\t                                ,TONGTIENBH=(A.SOLUONG-isnull(A.sl_tra,0) )*(CASE WHEN (ISNULL(C.NGAYTRINHTHE,C.NGAYTRINHTHE2)  IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.NgayThang_Xuat OR C.NGAYTRINHTHE2<=A.NgayThang_Xuat ) AND D.IsBHYT_Save=1 AND E.sudungchobh=1 THEN ISNULL(A.DonGiaBH_TEMP,(CASE WHEN E.GIA_MUA<=E.GIA_THAU THEN E.GIA_MUA ELSE E.GIA_THAU END)) ELSE 0 END)\r\n\t\t                                ,TIENCLS=0\r\n\t\t                                ,TIENPHUTHUBH=0\r\n\t\t                                ,TIENGIADV=(A.SOLUONG-isnull(A.sl_tra,0) )*(CASE WHEN (ISNULL(C.NGAYTRINHTHE,C.NGAYTRINHTHE2)  IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.NgayThang_Xuat OR C.NGAYTRINHTHE2<=A.NgayThang_Xuat ) AND D.IsBHYT_Save=1 AND E.sudungchobh=1 THEN 0 ELSE E.GIA_MUA END)\r\n\t\t                                ,VTYT  = (CASE WHEN  E.LoaiThuocID=4 THEN (A.SOLUONG-isnull(A.sl_tra,0) )*(CASE WHEN (ISNULL(C.NGAYTRINHTHE,C.NGAYTRINHTHE2)  IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.NgayThang_Xuat OR C.NGAYTRINHTHE2<=A.NgayThang_Xuat ) AND D.IsBHYT_Save=1 AND E.sudungchobh=1 THEN ISNULL(A.DonGiaBH_TEMP,(CASE WHEN E.GIA_MUA<=E.GIA_THAU THEN E.GIA_MUA ELSE E.GIA_THAU END)) ELSE 0 END)  ELSE 0 END)\r\n\t\t                                ,THUOC  = (CASE WHEN  E.LoaiThuocID=1 THEN (A.SOLUONG-isnull(A.sl_tra,0) )*(CASE WHEN (ISNULL(C.NGAYTRINHTHE,C.NGAYTRINHTHE2)  IS NULL OR ISNULL(C.NGAYTRINHTHE,C.NGAYDANGKY)<=A.NgayThang_Xuat OR C.NGAYTRINHTHE2<=A.NgayThang_Xuat ) AND D.IsBHYT_Save=1 AND E.sudungchobh=1 THEN ISNULL(A.DonGiaBH_TEMP,(CASE WHEN E.GIA_MUA<=E.GIA_THAU THEN E.GIA_MUA ELSE E.GIA_THAU END)) ELSE 0 END)  ELSE 0 END)\r\n\t\t                                ,THUOCK=0\r\n                                        ,BNTra=0\r\n                                        ,BHTra=0\r\n                                        ,TONGTIENBNPT=0\r\n                                        ,NgayXuat=A.NGAYTHANG_XUAT\r\n                                        ,PhuThuBH=0.00\r\n                                        ,IsHavePhuThuBH=(CASE WHEN  ISNULL( E.IsHavePhuThuBH,0)=1 THEN '1' ELSE '0' END)\r\n                                        ,top1_idchitietbenhnhantoathuoc=(case when a.idkho_xuat=5 then a.top1_idchitietbenhnhantoathuoc ELSE 0 END)\r\n                                        ,GOI_DVKT=ISNULL(D.GOI_DVKT,'')\r\n                                 from chitietphieuxuatkho a\r\n                                inner join khambenh b on a.IDKHAMBENH1=b.idkhambenh\r\n                                inner join dangkykham c on b.iddangkykham=c.iddangkykham\r\n                                inner join chitietbenhnhantoathuoc d on a.idchitietbenhnhantoathuoc=d.idchitietbenhnhantoathuoc\r\n                                INNER JOIN thuoc E ON a.idthuoc=e.idthuoc\r\n                                where \r\n                                     IsNull(D.IsHaoPhi,0)=0 \r\n                                     and ISNULL(a.soluong,0)-isnull(A.sl_tra,0)>0\r\n                                     and IsNull(a.IsBcTon,1)=1 AND\r\n                                     \r\n                                        C.IdBenhBHDongTien=" + idbenhbhdongtien + "\r\n                                        ORDER BY A.NGAYTHANG_XUAT ";
            DataTable dataTable4 = (DataTable)null;
            if (!IsNewDKK && IsThuoc)
                dataTable4 = Connect.GetTable(strSelect3);
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable4.Rows.Count; ++index)
                {
                    if (dataTable4.Rows[index]["IsHavePhuThuBH"].ToString() == "1" && hs_tinhtien.IsCheck(dataTable4.Rows[index]["ISBHYT"].ToString()))
                    {
                        double num21 = double.Parse(dataTable4.Rows[index]["ThanhTienDV"].ToString() == "" ? "0" : dataTable4.Rows[index]["ThanhTienDV"].ToString());
                        double num22 = double.Parse(dataTable4.Rows[index]["ThanhTienBH"].ToString() == "" ? "0" : dataTable4.Rows[index]["ThanhTienBH"].ToString());
                        if (num21 > num22)
                            dataTable4.Rows[index]["PhuThuBH"] = (object)(num21 - num22);
                    }
                }
                num4 += double.Parse(dataTable4.Compute("SUM(ThanhTienBH)", "").ToString());
                num1 += double.Parse(dataTable4.Compute("SUM(ThanhTienBH)", "").ToString());
                num20 += double.Parse(dataTable4.Compute("SUM(ThanhTienDV)", "").ToString());
                num14 += double.Parse(dataTable4.Compute("SUM(VTYT)", "").ToString());
                num15 += double.Parse(dataTable4.Compute("SUM(THUOC)", "").ToString());
                num8 += double.Parse(dataTable4.Compute("SUM(TIENGIADV)", "").ToString());
                num7 += double.Parse(dataTable4.Compute("SUM(PhuThuBH)", "").ToString());
            }
            string strSelect4 = "\r\n                       SELECT    \r\n                                 IsBHYT=(CASE WHEN ISNULL( A.DonGiaBH ,0 ) >0 THEN 1 ELSE 0 END)                \r\n                                ,PhuThuBH=(CASE WHEN ISNULL( A.DonGiaBH ,0 ) >0 THEN ( A.ThanhTienDV-A.ThanhTienBH) ELSE 0 END)                \r\n                                ,ThanhTienBH=(CASE WHEN ISNULL( A.DonGiaBH ,0 ) >0 THEN  A.ThanhTienBH ELSE 0 END)                \r\n                                ,TONGTIENBH=(CASE WHEN ISNULL( A.DonGiaBH ,0 ) >0 THEN A.ThanhTienBH ELSE 0 END)                \r\n                                ,TIENGIUONG=(CASE WHEN ISNULL( A.DonGiaBH ,0 ) >0 THEN A.ThanhTienBH ELSE 0 END)                \r\n                                ,PhuThuBH=(CASE WHEN ISNULL( A.DonGiaBH ,0 ) >0   THEN (A.ThanhTienDV-A.ThanhTienBH) ELSE 0 END)                \r\n                                ,TONGTIENDV=IsNull( A.ThanhTienDV ,0)\r\n                                ,ThanhTienDV=IsNull(A.ThanhTienDV,0)    \r\n                                ,DonGiaDV=IsNull(A.DonGiaDV,0)               \r\n                                ,TIENGIADV=  (CASE WHEN ISNULL( A.DonGiaBH ,0 ) >0 THEN 0 ELSE IsNull( A.ThanhTienDV ,0) END) \r\n                                ,BNTra=0\r\n                                ,BHTra=0\r\n                                ,TONGTIENBNPT=0\r\n                                ,A.IdChiTietGiuongBN     \r\n                                ,DonGiaBH=ISNULL( A.DonGiaBH ,0 )  \r\n                                ,IsMP_Giuong= ISNULL(C.IsMP_Giuong,0)      \r\n                     FROM  KB_CHITIETGIUONGBN A\r\n                          INNER JOIN CHITIETDANGKYKHAM B ON A.IDCHITIETDANGKYKHAM=B.IDchitietdangkykham\r\n                          INNER JOIN DANGKYKHAM C ON B.IDDANGKYKHAM=C.IDDANGKYKHAM\r\n                    WHERE C.IDBENHBHDONGTIEN=" + idbenhbhdongtien;
            DataTable dataTable5 = (DataTable)null;
            if (!IsNewDKK && IsTienGiuong)
                dataTable5 = Connect.GetTable(strSelect4);
            if (dataTable5 != null && dataTable5.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable5.Rows.Count; ++index)
                {
                    if (hs_tinhtien.IsCheck(dataTable5.Rows[index]["ISBHYT"].ToString()) && hs_tinhtien.IsCheck(dataTable5.Rows[index]["IsMP_Giuong"].ToString()))
                    {
                        dataTable5.Rows[index]["PhuThuBH"] = (object)"0";
                        dataTable5.Rows[index]["DonGiaDV"] = dataTable5.Rows[index]["DonGiaBH"];
                        dataTable5.Rows[index]["ThanhTienDV"] = dataTable5.Rows[index]["ThanhTienBH"];
                        dataTable5.Rows[index]["TONGTIENDV"] = dataTable5.Rows[index]["ThanhTienBH"];
                    }
                }
                num1 += double.Parse(dataTable5.Compute("SUM(ThanhTienBH)", "").ToString());
                num20 += double.Parse(dataTable5.Compute("SUM(ThanhTienDV)", "").ToString());
                num7 += double.Parse(dataTable5.Compute("SUM(PhuThuBH)", "").ToString());
                num19 += double.Parse(dataTable5.Compute("SUM(ThanhTienBH)", "").ToString());
            }
            DataTable table = Connect.GetTable("\r\n                                   SELECT  tilebhyt=(CASE WHEN A0.IS100=1 THEN 100 ELSE   DBO.ZHS_TILEBHYT(B.SOBH1  ,B.SOBH2  ,A.NGAYTINHBH_THUC  ,A.DUNGTUYEN ) END )\r\n                                            ,tilebhyt2=(CASE WHEN B2.IDBENHNHAN_BH IS NOT NULL THEN  (CASE WHEN A0.IS100=1 THEN 100 ELSE   DBO.ZHS_TILEBHYT(B2.SOBH1  ,B2.SOBH2  ,A.NGAYTINHBH_THUC  ,A.DUNGTUYEN ) END ) ELSE NULL END)\r\n                                            ,IsDungTuyen=B.IsDungTuyen\r\n                                            ,IsCapCuu=B.IsCapCuu\r\n                                            ,MUCTINHBH  =(\r\n                                                        SELECT TOP 1 dinhmuc\r\n                                                        FROM HS_THANGLUONGTOITHIEU\r\n                                                        WHERE TUNGAY<=A.NGAYTINHBH_THUC\r\n                                                        ORDER BY TUNGAY DESC\r\n                                                    )\r\n                                             FROM DANGKYKHAM A0 \r\n                                            INNER JOIN   hs_benhnhanbhdongtien  A ON A0.IDBENHBHDONGTIEN=A.ID\r\n\t\t\t                                INNER JOIN BENHNHAN_BHYT B ON A0.IDBENHNHAN_BH=B.IDBENHNHAN_BH\r\n                                            LEFT JOIN BENHNHAN_BHYT B2 ON A0.IDBENHNHAN_BH2=B2.IDBENHNHAN_BH\r\n                                            WHERE A0.IDBENHBHDONGTIEN=" + idbenhbhdongtien);
            if (table == null || table.Rows.Count == 0 || table.Rows[0][0].ToString() == "")
                return false;
            double num24 = double.Parse(table.Rows[0]["tilebhyt"].ToString());
            if (table.Rows[0]["tilebhyt2"].ToString() != "")
            {
                double num21 = double.Parse(table.Rows[0]["tilebhyt2"].ToString());
                if (num21 > num24)
                    num24 = num21;
            }
            double num25 = Math.Round(1.0 - num24 / 100.0, 2);
            bool flag1 = hs_tinhtien.IsCheck(table.Rows[0]["IsDungTuyen"].ToString());
            if (hs_tinhtien.IsCheck(table.Rows[0]["IsCapCuu"].ToString()))
                flag1 = true;
            double num26 = double.Parse(table.Rows[0]["MUCTINHBH"].ToString());
            string str2 = "";
            if (flag1 && num1 < num26)
                num25 = 0.0;
            double num27 = 100.0 - num25 * 100.0;
            for (int index = 0; dataTable3 != null && index < dataTable3.Rows.Count; ++index)
            {
                if (dataTable3.Rows[index]["ThanhTienBH"].ToString() == "")
                    dataTable3.Rows[index]["ThanhTienBH"] = (object)"0";
                if (dataTable3.Rows[index]["DonGiaBH"].ToString() == "")
                    dataTable3.Rows[index]["DonGiaBH"] = (object)"0";
                dataTable3.Rows[index]["BNTRA"] = (object)Math.Round(double.Parse(dataTable3.Rows[index]["ThanhTienBH"].ToString()) * num25, 0);
                dataTable3.Rows[index]["BHTRA"] = (object)(double.Parse(dataTable3.Rows[index]["ThanhTienBH"].ToString()) - double.Parse(dataTable3.Rows[index]["BNTRA"].ToString()));
                dataTable3.Rows[index]["TONGTIENBNPT"] = (object)(hs_tinhtien.IsCheck(dataTable3.Rows[index]["IsBHYT"].ToString()) ? double.Parse(dataTable3.Rows[index]["BNTRA"].ToString()) + double.Parse(dataTable3.Rows[index]["PhuThuBH"].ToString()) : double.Parse(dataTable3.Rows[index]["ThanhTienDV"].ToString()));
                str2 = str2 + " UPDATE CHITIETDANGKYKHAM SET ISBHYT=" + (hs_tinhtien.IsCheck(dataTable3.Rows[index]["IsBHYT"].ToString()) ? "1" : "0") + ",DonGiaDV=" + dataTable3.Rows[index]["DonGiaDV"].ToString() + ",ThanhTienDV=" + dataTable3.Rows[index]["ThanhTienDV"].ToString() + ",ThanhTienBH=" + dataTable3.Rows[index]["ThanhTienBH"].ToString() + ",DonGiaBH=" + dataTable3.Rows[index]["DonGiaBH"].ToString() + ",PhuThuBH=" + dataTable3.Rows[index]["PhuThuBH"].ToString() + ",BNTRA=" + dataTable3.Rows[index]["BNTRA"].ToString() + ",BHTRA=" + dataTable3.Rows[index]["BHTRA"].ToString() + ",BNTongPhaiTra=" + dataTable3.Rows[index]["TONGTIENBNPT"].ToString() + " WHERE IDCHITIETDANGKYKHAM=" + dataTable3.Rows[index]["idchitietdangkykham"].ToString() + "\r\n";
            }
            for (int index = 0; dataTable2 != null && dataTable2.Rows.Count > 0 && index < dataTable2.Rows.Count; ++index)
            {
                dataTable2.Rows[index]["BNTRA"] = (object)Math.Round(double.Parse(dataTable2.Rows[index]["ThanhTienBH"].ToString()) * num25, 0);
                dataTable2.Rows[index]["BHTRA"] = (object)(double.Parse(dataTable2.Rows[index]["ThanhTienBH"].ToString()) - double.Parse(dataTable2.Rows[index]["BNTRA"].ToString()));
                dataTable2.Rows[index]["TONGTIENBNPT"] = (object)(hs_tinhtien.IsCheck(dataTable2.Rows[index]["IsBHYT"].ToString()) ? double.Parse(dataTable2.Rows[index]["BNTRA"].ToString()) + double.Parse(dataTable2.Rows[index]["PhuThuBH"].ToString()) : double.Parse(dataTable2.Rows[index]["ThanhTienDV"].ToString()));
                str2 = str2 + @" UPDATE KHAMBENHCANLAMSAN SET 
                IsBHYT=" + (hs_tinhtien.IsCheck(dataTable2.Rows[index]["IsBHYT"].ToString()) ? "1" : "0") + @"
                ,DonGiaBH=" + dataTable2.Rows[index]["DonGiaBH"].ToString() + @"
                ,ThanhTienBH=" + dataTable2.Rows[index]["ThanhTienBH"].ToString() + @"
                ,DonGiaDV=" + dataTable2.Rows[index]["DonGiaDV"].ToString() + @"
                ,ThanhTienDV=" + dataTable2.Rows[index]["ThanhTienDV"].ToString() + @"
                ,IDBENHBHDONGTIEN=" + dataTable2.Rows[index]["IDBENHBHDONGTIEN"].ToString() + @"
                ,PhuThuBH=" + dataTable2.Rows[index]["PhuThuBH"].ToString() + @"
                ,BNTRA=" + dataTable2.Rows[index]["BNTRA"].ToString() + @"
                ,BHTRA=" + dataTable2.Rows[index]["BHTRA"].ToString() + @"
                ,BNTongPhaiTra=" + dataTable2.Rows[index]["TONGTIENBNPT"].ToString() + @"
                ,IdNhomInBV=" + (dataTable2.Rows[index]["IdNhomInBV"].ToString() == "" ? "NULL" : dataTable2.Rows[index]["IdNhomInBV"].ToString()) + @" 
                WHERE IDKHAMBENHCANLAMSAN=" + dataTable2.Rows[index]["IDKHAMBENHCANLAMSAN"].ToString() + "";
            }
            for (int index = 0; dataTable4 != null && dataTable4.Rows.Count > 0 && index < dataTable4.Rows.Count; ++index)
            {
                if (dataTable4.Rows[index]["DonGiaDV"].ToString() == "")
                    dataTable4.Rows[index]["DonGiaDV"] = (object)"0";
                if (dataTable4.Rows[index]["ThanhTienDV"].ToString() == "")
                    dataTable4.Rows[index]["ThanhTienDV"] = (object)"0";
                if (dataTable4.Rows[index]["DonGiaBH"].ToString() == "")
                    dataTable4.Rows[index]["DonGiaBH"] = (object)"0";
                if (dataTable4.Rows[index]["ThanhTienBH"].ToString() == "")
                    dataTable4.Rows[index]["ThanhTienBH"] = (object)"0";
                if (dataTable4.Rows[index]["top1_idchitietbenhnhantoathuoc"].ToString() == "")
                    dataTable4.Rows[index]["top1_idchitietbenhnhantoathuoc"] = (object)"0";
                dataTable4.Rows[index]["BNTRA"] = (object)Math.Round(double.Parse(dataTable4.Rows[index]["ThanhTienBH"].ToString()) * num25, 0);
                dataTable4.Rows[index]["BHTRA"] = (object)(double.Parse(dataTable4.Rows[index]["ThanhTienBH"].ToString()) - double.Parse(dataTable4.Rows[index]["BNTRA"].ToString()));
                dataTable4.Rows[index]["TONGTIENBNPT"] = (object)(hs_tinhtien.IsCheck(dataTable4.Rows[index]["IsBHYT"].ToString()) ? Math.Round(double.Parse(dataTable4.Rows[index]["BNTRA"].ToString()) + double.Parse(dataTable4.Rows[index]["PhuThuBH"].ToString()), 0) : double.Parse(dataTable4.Rows[index]["ThanhTienDV"].ToString()));
                str2 = str2 + " UPDATE CHITIETPHIEUXUATKHO SET\r\n                                                                               IsBHYT=" + (hs_tinhtien.IsCheck(dataTable4.Rows[index]["IsBHYT"].ToString()) ? "1" : "0") + ",DonGiaBH=" + dataTable4.Rows[index]["DonGiaBH"].ToString() + ",ThanhTienBH=" + dataTable4.Rows[index]["ThanhTienBH"].ToString() + ",DonGiaDV=" + dataTable4.Rows[index]["DonGiaDV"].ToString() + ",ThanhTienDV=" + dataTable4.Rows[index]["ThanhTienDV"].ToString() + ",BNTRA=" + dataTable4.Rows[index]["BNTRA"].ToString() + ",BHTRA=" + dataTable4.Rows[index]["BHTRA"].ToString() + ",PhuThuBH=" + dataTable4.Rows[index]["PhuThuBH"].ToString() + ",top1_idchitietbenhnhantoathuoc=" + dataTable4.Rows[index]["top1_idchitietbenhnhantoathuoc"].ToString() + ",IDBENHBHDONGTIEN=" + idbenhbhdongtien + ",GOI_DVKT=N'" + dataTable4.Rows[index]["GOI_DVKT"].ToString() + "' WHERE IDCHITIETPHIEUXUAT=" + dataTable4.Rows[index]["IDCHITIETPHIEUXUAT"].ToString() + "\r\n";
            }
            for (int index = 0; dataTable5 != null && dataTable5.Rows.Count > 0 && index < dataTable5.Rows.Count; ++index)
            {
                dataTable5.Rows[index]["BNTRA"] = (object)Math.Round(double.Parse(dataTable5.Rows[index]["ThanhTienBH"].ToString()) * num25, 0);
                dataTable5.Rows[index]["BHTRA"] = (object)(double.Parse(dataTable5.Rows[index]["ThanhTienBH"].ToString()) - double.Parse(dataTable5.Rows[index]["BNTRA"].ToString()));
                dataTable5.Rows[index]["TONGTIENBNPT"] = (object)(hs_tinhtien.IsCheck(dataTable5.Rows[index]["IsBHYT"].ToString()) ? double.Parse(dataTable5.Rows[index]["BNTRA"].ToString()) + double.Parse(dataTable5.Rows[index]["PhuthuBH"].ToString()) : double.Parse(dataTable5.Rows[index]["ThanhTienDV"].ToString()));
                str2 = str2 + " UPDATE KB_CHITIETGIUONGBN SET\r\n                                                                          IsBHYT=" + (hs_tinhtien.IsCheck(dataTable5.Rows[index]["IsBHYT"].ToString()) ? "1" : "0") + ",DonGiaBH=" + dataTable5.Rows[index]["DonGiaBH"].ToString() + ",ThanhTienBH=" + dataTable5.Rows[index]["ThanhTienBH"].ToString() + ",DonGiaDV=" + dataTable5.Rows[index]["DonGiaDV"].ToString() + ",ThanhTienDV=" + dataTable5.Rows[index]["ThanhTienDV"].ToString() + ",BNTRA=" + dataTable5.Rows[index]["BNTRA"].ToString() + ",BHTRA=" + dataTable5.Rows[index]["BHTRA"].ToString() + ",PhuthuBH=" + dataTable5.Rows[index]["PhuthuBH"].ToString() + " WHERE IdChiTietGiuongBN=" + dataTable5.Rows[index]["IdChiTietGiuongBN"].ToString() + "\r\n";
            }
            if (dataTable3 != null && dataTable3.Rows.Count > 0)
                num3 = double.Parse(dataTable3.Compute("SUM(BNTRA)", "").ToString());
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
                num3 += double.Parse(dataTable2.Compute("SUM(BNTRA)", "").ToString());
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
                num3 += double.Parse(dataTable4.Compute("SUM(BNTRA)", "").ToString());
            if (dataTable3 != null && dataTable3.Rows.Count > 0)
                num2 = double.Parse(dataTable3.Compute("SUM(BHTRA)", "").ToString());
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
                num2 += double.Parse(dataTable2.Compute("SUM(BHTRA)", "").ToString());
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
                num2 += double.Parse(dataTable4.Compute("SUM(BHTRA)", "").ToString());
            if (dataTable5 != null && dataTable5.Rows.Count > 0)
                num2 += double.Parse(dataTable5.Compute("SUM(BHTRA)", "").ToString());
            if (dataTable3 != null && dataTable3.Rows.Count > 0)
                num9 = double.Parse(dataTable3.Compute("SUM(TONGTIENBNPT)", "").ToString());
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
                num9 += double.Parse(dataTable2.Compute("SUM(TONGTIENBNPT)", "").ToString());
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
                num9 += double.Parse(dataTable4.Compute("SUM(TONGTIENBNPT)", "").ToString());
            if (dataTable5 != null && dataTable5.Rows.Count > 0)
                num9 += double.Parse(dataTable5.Compute("SUM(TONGTIENBNPT)", "").ToString());
            object[] objArray1 = new object[49]
      {
        (object) str2,
        (object) " UPDATE HS_BENHNHANBHDONGTIEN SET\r\n                                                TongTienBH=",
        (object) num1.ToString(),
        (object) ",BHTra=",
        (object) num2.ToString(),
        (object) ",BNPhaiTra=",
        (object) num3.ToString(),
        (object) ",TienThuoc=",
        (object) num4,
        (object) ",TienKham=",
        (object) num5,
        (object) ",TienCLS=",
        (object) num6,
        (object) ",TienPhuThuBH=",
        (object) num7,
        (object) ",TONGTIENDV=",
        (object) num20,
        (object) ",TongTienBNPT=",
        (object) num9,
        (object) ",CDHA=",
        (object) num11,
        (object) ",THUTHUAT=",
        (object) num13,
        (object) ",CLSKhac=",
        (object) num18,
        (object) ",DVKTCao=",
        (object) num16,
        (object) ",TIEMTRUYEN=",
        (object) num12,
        (object) ",TongTienBNPTConLai=",
        (object) num9,
        (object) "- ISNULL( TongTienBNDaTra,0),VanChuyen=",
        (object) num17,
        (object) ",VTYT=",
        (object) num14,
        (object) ",XN=",
        (object) num10,
        (object) ",THUOC=",
        (object) num15,
        (object) ",TIENGIUONG=",
        (object) num19,
        (object) ",TIENGIADV=",
        (object) num8,
        (object) "\r\n                                               -- ",
        null,
        null,
        null,
        null,
        null
      };
            object[] objArray2 = objArray1;
            int index3 = 44;
            string str3;
            if (dataTable4 == null || dataTable4.Rows.Count <= 0)
                str3 = "";
            else
                str3 = "   ,STT_KB=(CASE WHEN ISNULL(STT_KB,0)=0 THEN  DBO.HS_GET_SOKHAMBENH(" + idbenhbhdongtien + ",'" + DateTime.Parse(dataTable4.Rows[dataTable4.Rows.Count - 1]["NgayXuat"].ToString()).ToString("yyyy/MM/dd") + "') ELSE ISNULL(STT_KB,0) END)";
            objArray2[index3] = (object)str3;
            objArray1[45] = (object)"\r\n                                                ,MUC_HUONG=";
            objArray1[46] = (object)num27;
            objArray1[47] = (object)"\r\n                                           WHERE ID=";
            objArray1[48] = (object)idbenhbhdongtien;
            Connect.ExecSQL(string.Concat(objArray1));
            Connect.ExecSQL(@"DECLARE @IDBENHBHDONGTIEN  AS BIGINT
                SET @IDBENHBHDONGTIEN =" + idbenhbhdongtien + @"
                    ---------------------------------------------------------------
                    " + (IsPhiKham ? @"   DELETE chitietdangkykham_HS  
                    WHERE   IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN  
                    ---------------------------------------------------------------   
                    INSERT INTO  chitietdangkykham_HS  
                    SELECT A.*,B.IdBenhBHDongTien 
                    FROM chitietdangkykham A 
                    INNER JOIN dangkykham B ON A.iddangkykham=B.iddangkykham  
                    WHERE   B.IdBenhBHDongTien =@IDBENHBHDONGTIEN  
                    AND ISNULL(A.dahuy,0)=0  
                    AND ISNULL(A.isNotThuPhiCapCuu,0)=0  
                    AND ISNULL(A.DONGIADV,0)>0 
                    " : "") + @"
                    ----------------------------------------------------------------------------- 
                    " + (IsNewDKK || !IsCLS ? "" : @"                    
                    DELETE khambenhcanlamsan_HS WHERE ISNULL(IDBENHBHDONGTIEN_HS, IDBENHBHDONGTIEN)=@IDBENHBHDONGTIEN                
                    -----------------------------------------------------------------------------                 
                    INSERT INTO khambenhcanlamsan_HS                
                    (idkhambenhcanlamsan, 
                    idkhambenh,             
                    idcanlamsan,               
                    dongia,               
                    idbacsi,              
                    dathu,               
                    dakham,               
                    ngaythu,              
                    idnguoidung,             
                    ngaykham,               
                    idbenhnhan,                
                    tenBSChiDinh,                
                    maphieuCLS,               
                    thucthu,               
                    dahuy,               
                    soluong,               
                    chietkhau,               
                    thanhtien,                
                    IdNguoiThu,               
                    BHTra,               
                    GhiChu,              
                    LoaiKhamID,               
                    idkhoadangky,           
                    sldakham,               
                    istieuphau,              
                    isphauthuat,               
                    IsHoanTraCLS,             
                    BHYTTra,              
                    BNDaTra,                
                    BNTongPhaiTra,             
                    BNTra,            
                    DonGiaBH,               
                    DonGiaDV,             
                    IdChiTietPTT,            
                    IsBHYT,             
                    ISBNDaTra,             
                    NgayQuayLai,              
                    NGAYTINHBH_THUC,            
                    PhuThuBH,               
                    PrevBNTra,              
                    ThanhTienBH,              
                    ThanhTienDV,               
                    IDDANGKYCLS,              
                    IsDaDKK,              
                    IdnhomInBV,            
                    IsBHYT_Save,             
                    TOP1_IDKHAMBENHCANLAMSAN,              
                    ISDATRAKQ,            
                    THOIGIANTRAKQ,               
                    madangkycls,               
                    chandoansobo,               
                    DonGiaBH_Temp,              
                    IdBacSiCLS,                
                    SoID,                
                    NGAYCLS,                
                    per50,                
                    per80,                
                    IDBENHBHDONGTIEN,                
                    IDBENHBHDONGTIEN_HS                
                    )                

                    SELECT                 
                    A.idkhambenhcanlamsan,               
                    A.idkhambenh,              
                    A.idcanlamsan,              
                    A.dongia,               
                    A.idbacsi,                
                    A.dathu,               
                    A.dakham,                
                    A.ngaythu,               
                    A.idnguoidung,             
                    A.ngaykham,                
                    A.idbenhnhan,               
                    A.tenBSChiDinh,               
                    A.maphieuCLS,              
                    A.thucthu,              
                    A.dahuy,               
                    A.soluong,               
                    A.chietkhau,                
                    A.thanhtien,              
                    A.IdNguoiThu,               
                    A.BHTra,               
                    A.GhiChu,               
                    A.LoaiKhamID,                
                    A.idkhoadangky,              
                    A.sldakham,               
                    A.istieuphau,               
                    A.isphauthuat,              
                    A.IsHoanTraCLS,                
                    A.BHYTTra,               
                    A.BNDaTra,                
                    A.BNTongPhaiTra,                
                    A.BNTra,               
                    A.DonGiaBH,              
                    A.DonGiaDV,              
                    A.IdChiTietPTT,               
                    A.IsBHYT,             
                    A.ISBNDaTra,               
                    A.NgayQuayLai,                
                    A.NGAYTINHBH_THUC,              
                    A.PhuThuBH,               
                    A.PrevBNTra,                
                    A.ThanhTienBH,             
                    A.ThanhTienDV,       
                    A.IDDANGKYCLS,               
                    A.IsDaDKK,               
                    A.IdnhomInBV,               
                    A.IsBHYT_Save,                                  
                    A.TOP1_IDKHAMBENHCANLAMSAN,               
                    A.ISDATRAKQ,               
                    A.THOIGIANTRAKQ,               
                    A.madangkycls,            
                    A.chandoansobo,              
                    A.DonGiaBH_Temp,               
                    A.IdBacSiCLS,                
                    A.SoID,                
                    A.NGAYCLS,                
                    A.per50,                
                    A.per80,                
                    IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN,               
                    IDBENHBHDONGTIEN_HS=@IDBENHBHDONGTIEN              
                    FROM khambenhcanlamsan A                
                    INNER JOIN khambenh B  ON A.idkhambenh=B.idkhambenh               
                    INNER JOIN dangkykham C ON B.iddangkykham=C.iddangkykham                
                    WHERE C.IdBenhBHDongTien=@IDBENHBHDONGTIEN                
                    AND ISNULL(A.dahuy,0)=0                
                    -----------------------------------------------------------------------------  
                    ") + (IsNewDKK || !IsThuoc ? "" : @"
                    -----------------------------------------------------------------------------   
                    DELETE CHITIETPHIEUXUATKHO_HS WHERE ISNULL(IDBENHBHDONGTIEN_HS,IDBENHBHDONGTIEN)=@IDBENHBHDONGTIEN  
                    INSERT INTO CHITIETPHIEUXUATKHO_HS(idchitietphieuxuat, 
                    idphieuxuat,
                    idthuoc, 
                    soluong,
                    dongia,  
                    idchitietphieunhapkho,  
                    idtu, 
                    idngan, 
                    sluongxuat,  
                    dvt,  
                    thanhtien, 
                    thanhtientruocthue,  
                    tienthue, 
                    chietkhau, 
                    giavon,  
                    tienvon,  
                    tiensauchietkhau,  
                    tienchietkhau, 
                    IDCHITIETPHIEUYEUCAUXUATKHO,  
                    tkkho,  
                    tkco, 
                    idchitietbenhnhantoathuoc,  
                    idThuocPhauThuat,  
                    VAT,  
                    IDKHO_XUAT, 
                    NGAYTHANG_XUAT, 
                    LOSANXUAT_XUAT, 
                    NGAYHETHAN_XUAT,  
                    IDLOAIXUAT_XUAT,  
                    BHTra,  
                    BNDaTra,
                    BNTra,  
                    DonGiaBH,  
                    DonGiaDV, 
                    IsBHYT, 
                    ISBNDaTra,  
                    IsDaHuy, 
                    NGAYTINHBH_THUC,  
                    PrevBNTra,  
                    SL_Prev,  
                    ThanhTien_Prev, 
                    ThanhTienBH, 
                    ThanhTienDV,
                    OutPutDetailID,  
                    IDKHAMBENH1,  
                    IdNuocSX_X,  
                    GhiChu,  
                    soPhieuTra,  
                    ngayNhanTra,  
                    soLuong_bk,  
                    IsTinhTien,  
                    PHUTHUBH, 
                    IDBENHBHDONGTIEN, 
                    TOP1_IDCHITIETBENHNHANTOATHUOC,  
                    NHACUNGCAPID_X,  
                    SOHOADON_X, 
                    NGAYHOADON_X,  
                    IdLoaiThuoc, 
                    ISBHYT_NHAP, 
                    SODK_X,  
                    IDCHITIETPHIEUYEUCAUTRAKHO,  
                    SOLUONG_PREV,  
                    IDPHIEUXUAT_YCTRA,  
                    SAVEDATE,
                    FIRSTDATE,  
                    NotRunTrigger,  
                    ISBHYT_SAVE_X, 
                    ISHAOPHI_X,  
                    ISXUAT_HV,  
                    IDBENHBHDONGTIEN_HS, 
                    ISBHYT_SAVE_HS,  
                    IDKHOA  ,
                    GOI_DVKT
                    )  
                    SELECT  A. idchitietphieuxuat,  
                    A.idphieuxuat,  
                    A.idthuoc, 
                    soluong=A.SOLUONG-ISNULL(A.SL_TRA,0),  
                    A.dongia,  
                    A.idchitietphieunhapkho,  
                    A.idtu,  
                    A.idngan, 
                    A.sluongxuat,  
                    A.dvt,  
                    A.thanhtien, 
                    A.thanhtientruocthue, 
                    A.tienthue,  
                    A.chietkhau, 
                    A.giavon, 
                    A.tienvon,  
                    A.tiensauchietkhau, 
                    A.tienchietkhau,
                    A.IDCHITIETPHIEUYEUCAUXUATKHO,  
                    A.tkkho,  
                    A.tkco,  
                    A.idchitietbenhnhantoathuoc,  
                    A.idThuocPhauThuat,  
                    A.VAT, 
                    A.IDKHO_XUAT,  
                    A.NGAYTHANG_XUAT,  
                    A.LOSANXUAT_XUAT,  
                    A.NGAYHETHAN_XUAT, 
                    A.IDLOAIXUAT_XUAT,  
                    A.BHTra,  
                    A.BNDaTra,  
                    A.BNTra,  
                    A.DonGiaBH,  
                    A.DonGiaDV,  
                    A.IsBHYT,  
                    A.ISBNDaTra,  
                    A.IsDaHuy,  
                    A.NGAYTINHBH_THUC,  
                    A.PrevBNTra,  
                    A.SL_Prev,  
                    A.ThanhTien_Prev, 
                    A.ThanhTienBH, 
                    A.ThanhTienDV,  
                    A.OutPutDetailID, 
                    A.IDKHAMBENH1,  
                    A.IdNuocSX_X,  
                    A.GhiChu,  
                    A.soPhieuTra,  
                    A.ngayNhanTra,  
                    A.soLuong_bk,  
                    A.IsTinhTien,  
                    A.PHUTHUBH,  
                    A.IDBENHBHDONGTIEN,  
                    A.TOP1_IDCHITIETBENHNHANTOATHUOC,  
                    A.NHACUNGCAPID_X,  
                    A.SOHOADON_X,  
                    A.NGAYHOADON_X,  
                    A.IdLoaiThuoc, 
                    A.ISBHYT_NHAP,  
                    A.SODK_X,  
                    A.IDCHITIETPHIEUYEUCAUTRAKHO, 
                    A.SOLUONG_PREV, 
                    A.IDPHIEUXUAT_YCTRA,  
                    A.SAVEDATE,  
                    A.FIRSTDATE,  
                    A.NotRunTrigger,  
                    A.ISBHYT_SAVE_X,  
                    A.ISHAOPHI_X,  
                    A.ISXUAT_HV                                   
                    ,IdBenhBHDongTien_HS=C.IdBenhBHDongTien  
                    ,ISBHYT_SAVE_HS=A.ISBHYT_SAVE_X
                    ,IDKHOA=B.idphongkhambenh 
                    ,GOI_DVKT=D.GOI_DVKT
                    from chitietphieuxuatkho a
                    inner join khambenh b on a.IDKHAMBENH1=b.idkhambenh
                    inner join dangkykham c on b.iddangkykham=c.iddangkykham
                    inner join chitietbenhnhantoathuoc d on a.idchitietbenhnhantoathuoc=d.idchitietbenhnhantoathuoc
                    INNER JOIN thuoc E ON a.idthuoc=e.idthuoc
                    where  IsNull(D.IsHaoPhi,0)=0
                    and ISNULL(a.soluong,0)-isnull(A.sl_tra,0)>0
                    and IsNull(a.IsBcTon,1)=1 AND
                    C.IdBenhBHDongTien=" + idbenhbhdongtien + @"
                    ORDER BY A.NGAYTHANG_XUAT                                               
                    ----------------------------------------------------------------------------- 
                    ") + (IsNewDKK || !IsTienGiuong ? "" : @"
                    DELETE KB_CHITIETGIUONGBN_HS WHERE ISNULL(IDBENHBHDONGTIEN_HS, IDBENHBHDONGTIEN)=@IDBENHBHDONGTIEN  
                    -----------------------------------------------------------------------------                    
                    INSERT INTO KB_CHITIETGIUONGBN_HS               
                    SELECT *,IDBENHBHDONGTIEN_HS=@IDBENHBHDONGTIEN              
                    FROM KB_CHITIETGIUONGBN              
                    WHERE IDchitietdangkykham  IN              
                    ( SELECT IDchitietdangkykham                 
                    FROM chitietdangkykham A              
                    INNER JOIN dangkykham B ON A.iddangkykham=B.iddangkykham          
                    WHERE B.IdBenhBHDongTien=@IDBENHBHDONGTIEN                
                    )              
                    AND ISNULL( KB_CHITIETGIUONGBN.SL,0)>0                   
                    AND ISNULL( DonGiaDV,0)>0                 
                    ----------------------------------------------------------------------------- 
                    "));
            return true;
        }

        public static bool TinhTienDV(string idbenhbhdongtien, string iddangkykham, bool IsNewDKK)
        {
            return hs_tinhtien.TinhTienDV(idbenhbhdongtien, iddangkykham, IsNewDKK, true, true, true, true);
        }

        public static bool TinhTienDV(string idbenhbhdongtien, string iddangkykham, bool IsNewDKK, bool IsPhiKham, bool IsCLS, bool IsThuoc, bool IsTienGiuong)
        {
            if ((iddangkykham == null || iddangkykham == "") && idbenhbhdongtien != null && idbenhbhdongtien != "")
                iddangkykham = Connect.GetTable("SELECT TOP 1 IDDANGKYKHAM FROM DANGKYKHAM WHERE IDBENHBHDONGTIEN=" + idbenhbhdongtien).Rows[0][0].ToString();
            string strCommandText = "\r\n                            DECLARE @iddangkykham AS BIGINT  \r\n                            SET @iddangkykham=" + iddangkykham + "\r\n\r\n                             DECLARE @NGAYDANGKY AS DATETIME        \r\n                             DECLARE @IDBENHNHAN AS BIGINT        \r\n                             DECLARE @HOTENBN AS NVARCHAR(50)        \r\n                             DECLARE @IDBENHBHDONGTIEN AS BIGINT        \r\n                             DECLARE @NGAYTINHBH_THUC as datetime        \r\n                             DECLARE @NGAYTINHBH AS DATETIME        \r\n\t                         DECLARE @IDKHOADK AS BIGINT        \r\n\t                         DECLARE @LoaiKhamID AS BIGINT  \r\n                                     \r\n\t\t                     DECLARE @IsNew as bit        \r\n\t\t                     SET @IsNew=0        \r\n           \r\n                             SELECT @IDBENHNHAN=A.IDBENHNHAN,        \r\n                               @NGAYDANGKY=A.NGAYDANGKY,        \r\n                               @HOTENBN=B.TENBENHNHAN        \r\n                              ,@IDBENHBHDONGTIEN=hs.ID        \r\n                              ,@NGAYTINHBH_THUC=hs.NgayTinhBH_Thuc        \r\n                              ,@NGAYTINHBH=HS.NgayTinhBH        \r\n                              ,@IDKHOADK=(SELECT TOP 1  IDKHOA FROM CHITIETDANGKYKHAM WHERE IDDANGKYKHAM=A.IDDANGKYKHAM )          \r\n                              ,@LoaiKhamID=A.LOAIKHAMID        \r\n                             FROM DANGKYKHAM A        \r\n                             INNER JOIN BENHNHAN B ON A.IDBENHNHAN=B.IDBENHNHAN        \r\n                             LEFT JOIN HS_BenhNhanBHDongTien HS ON A.IdBenhBHDongTien=HS.ID        \r\n                             WHERE IDDANGKYKHAM=@IDDANGKYKHAM \r\n       \r\n        \r\n                             IF(@LoaiKhamID <>2)    RETURN        \r\n                             if (@IDBENHBHDONGTIEN is not null)        \r\n                                BEGIN        \r\n\t\t\t\t                    IF NOT EXISTS (SELECT 1 FROM HS_BENHNHANBHDONGTIEN WHERE ID=@IDBENHBHDONGTIEN)   \r\n\t\t\t\t                        SET @IDBENHBHDONGTIEN=NULL        \r\n\t\t\t                    END             \r\n        \r\n                           IF (@IDBENHBHDONGTIEN IS NULL)        \r\n                              BEGIN        \r\n                             ------------------GET IDBENHBHDONGTIEN--------------------        \r\n                             SELECT TOP 1 @IDBENHBHDONGTIEN=ID        \r\n                             FROM HS_BENHNHANBHDONGTIEN        \r\n                              WHERE IDBENHNHAN=@IDBENHNHAN        \r\n                               AND DBO.HS_EQUARDAY1(@NGAYDANGKY,NGAYTINHBH)=1        \r\n                               AND ISNULL(IsXuatVien,0)=0        \r\n                               AND ISNULL( IDKHOA_DK,0)=@IDKHOADK        \r\n                               AND ISNULL(ISBHYT,0)=0        \r\n                                ----------------------IDBENHBHDONGTIEN NOT EXIST------------------------        \r\n                             IF (@IDBENHBHDONGTIEN IS NULL)        \r\n                               BEGIN        \r\n                                  INSERT HS_BENHNHANBHDONGTIEN(        \r\n                                  ISBHYT        \r\n                                  ,IDBENHNHAN        \r\n                                  ,NGAYTINHBH        \r\n                                  ,NgayTinhBH_Thuc        \r\n                                  ,IDDANGKYKHAM_DV        \r\n                                  ,HOTENBN        \r\n                                  ,IsXuatVien        \r\n                                  ,IDKHOA_DK        \r\n                                  )        \r\n                                VALUES (        \r\n                                  0        \r\n                                  ,@IDBENHNHAN        \r\n                                  ,@NGAYDANGKY        \r\n                                  ,@NGAYDANGKY        \r\n                             ,@IDDANGKYKHAM         \r\n                                  ,@HOTENBN        \r\n                                  ,0        \r\n                                  ,@IDKHOADK        \r\n                                  )        \r\n                                    SELECT TOP 1 @IDBENHBHDONGTIEN=ID        \r\n                                         FROM HS_BenhNhanBHDongTien        \r\n                                         WHERE IDBENHNHAN=@IDBENHNHAN        \r\n                                         ORDER BY ID DESC         \r\n                                       SET @IsNew=1          \r\n                                     END        \r\n                             UPDATE dangkykham SET IdBenhBHDongTien=@IDBENHBHDONGTIEN WHERE iddangkykham=@IDDANGKYKHAM        \r\n                             SET @NGAYTINHBH=@NGAYDANGKY        \r\n                           SET @NGAYTINHBH_THUC=@NGAYDANGKY        \r\n                                  END        \r\n                                   ELSE        \r\n                          BEGIN        \r\n                                IF ( @NGAYTINHBH IS NULL ) SET @NGAYTINHBH=@NGAYDANGKY        \r\n                                ELSE        \r\n                                IF(@NGAYDANGKY<@NGAYTINHBH) SET @NGAYTINHBH=@NGAYDANGKY        \r\n                          END    ";
            if (!IsNewDKK)
                strCommandText = strCommandText + hs_tinhtien.SaveOtherInfor(false) + "\r\n        \r\n                              UPDATE HS_BENHNHANBHDONGTIEN SET\r\n                                                ISBHYT=0\r\n                                                ,IDBENHNHAN_BH=null\r\n                                                ,SOBHYT=''\r\n                                                ,DungTuyen=''\r\n                                                ,ISCAPCUU=0 \r\n                                                ,IDKHAMBENH_LAST=@IDKHAMBENH_LAST\r\n                                                ,IDKHOA_LAST=@IDKHOA_LAST\r\n                                                ,TINH_TRANG_RV=@TINH_TRANG_RV\r\n                                                ,ket_qua_dtri=@ket_qua_dtri\r\n                                                ,MACHANDOAN=@MACHANDOAN\r\n                                                ,TENCHANDOAN=@TENCHANDOAN\r\n                                                ,ma_benhkhac= @ma_benhkhac\r\n                                                ,ChanDoanKhac=@ChanDoanKhac\r\n                                                ,IsXuatVien=@ISXUATVIEN\r\n                                                ,NGAYTINHBH_THUC=@NGAYTINHBH_THUC\r\n                                                ,IDKHAMBENH_FIRST=@IDKHAMBENH_FIRST\r\n                                                ,SongayDT=\r\n                                                             (case when isnull(ISNOITRU,0)=0 THEN 0 ELSE            \r\n                                                                   (CASE WHEN CONVERT(VARCHAR,NGAYTINHBH,111)=CONVERT(VARCHAR,@NgayTinhBH_Thuc,111) THEN 1 ELSE    CONVERT(INT, CONVERT(DATETIME, CONVERT(VARCHAR, @NgayTinhBH_Thuc,111))-CONVERT(DATETIME, CONVERT(VARCHAR, NgayTinhBH,111)))  +(CASE WHEN @HUONGDIEUTRI=4 OR @HUONGDIEUTRI=24 THEN 1 ELSE 0 END ) END)                \r\n                                                             END)        \r\n                                                                        \r\n                                    WHERE ID=@IDBENHBHDONGTIEN";
            Connect.ExecSQL(strCommandText);
            DataTable table = Connect.GetTable("SELECT IDBENHBHDONGTIEN FROM DANGKYKHAM WHERE IdDangKyKham=" + iddangkykham);
            if (table == null || table.Rows.Count == 0)
                return false;
            idbenhbhdongtien = table.Rows[0]["IDBENHBHDONGTIEN"].ToString();
            hs_tinhtien.FixChanDoan(idbenhbhdongtien);
            DateTime now = DateTime.Now;
            double num1 = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;
            double num8 = 0.0;
            double num9 = 0.0;
            double num10 = 0.0;
            double num11 = 0.0;
            double num12 = 0.0;
            double num13 = 0.0;
            double num14 = 0.0;
            double num15 = 0.0;
            double num16 = 0.0;
            double num17 = 0.0;
            double num18 = 0.0;
            double num19 = 0.0;
            double num20 = 0.0;
            string strSelect1 = @" select 
                A.idchitietdangkykham
                ,DonGiaDV=D.GIADV
                ,DonGiaBH=0
                ,IsBHYT=0
                ,PhuThuBH=0
                ,BNTra=0
                ,BHTra=0
                ,TONGTIENBNPT=0
                ,ThanhTienBH=0
                ,TIENGIADV=D.GIADV
                ,ThanhTienDV=D.GIADV
                ,IsSoKB=(CASE WHEN A.IDBANGGIADICHVU=628 THEN '1' ELSE '0' END)
                ,STGiam=D.STGiam
                from chitietdangkykham a
                inner join dangkykham b on a.iddangkykham=b.iddangkykham
                inner join BANGGIADICHVU C ON A.IDBANGGIADICHVU=C.IDBANGGIADICHVU
                inner join hs_banggiavienphi d on d.IdGiaDichVu=(SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi D0 WHERE D0.IdDichVu=a.idbanggiadichvu AND D0.TuNgay<=B.NGAYDANGKY ORDER BY D0.TUNGAY DESC )
                where b.IdBenhBHDongTien=" + idbenhbhdongtien + @"
                AND ISNULL(A.dahuy,0)=0
                AND ISNULL(A.isNotThuPhiCapCuu,0)=0
                ORDER BY   (CASE WHEN A.IDBANGGIADICHVU=628 THEN 1 ELSE 2 END)
                ";
            DataTable dataTable1 = IsPhiKham ? Connect.GetTable(strSelect1) : (DataTable)null;
            if (dataTable1 != null && dataTable1.Rows.Count > 0)
            {
                if (dataTable1.Rows.Count >= 2)
                {
                    bool flag = true;
                    int num21 = 0;
                    if (dataTable1.Rows[0]["IsSoKB"].ToString() == "1")
                    {
                        num21 = 1;
                        if (dataTable1.Rows.Count == 2)
                            flag = false;
                    }
                    if (flag)
                    {
                        for (int index = num21 + 1; index < dataTable1.Rows.Count; ++index)
                        {
                            dataTable1.Rows[index]["DonGiaDV"] = (object)(double.Parse(dataTable1.Rows[index]["DonGiaDV"].ToString()) - double.Parse(dataTable1.Rows[index]["STGiam"].ToString() == "" ? "0" : dataTable1.Rows[index]["STGiam"].ToString()));
                            dataTable1.Rows[index]["TIENGIADV"] = dataTable1.Rows[index]["DonGiaDV"];
                            dataTable1.Rows[index]["ThanhTienDV"] = dataTable1.Rows[index]["DonGiaDV"];
                        }
                    }
                }
                num5 = double.Parse(dataTable1.Compute("SUM(DonGiaDV)", "").ToString());
                num1 = double.Parse(dataTable1.Compute("SUM(DonGiaBH)", "").ToString());
                num19 = double.Parse(dataTable1.Compute("SUM(DonGiaDV)", "").ToString());
                num7 = double.Parse(dataTable1.Compute("SUM(PhuThuBH)", "").ToString());
                num8 = double.Parse(dataTable1.Compute("SUM(TIENGIADV)", "").ToString());
            }
            string strSelect2 = @" select 
                a.idkhambenhcanlamsan
                ,IDBENHBHDONGTIEN=c.IdBenhBHDongTien
                ,soluong=ISNULL(a.soluong,1)
                ,DonGiaDV=D.GiaDV
                ,ThanhTienDV=ISNULL(a.soluong,1)*D.GiaDV
                ,DonGiaBH_TEMP=D.GiaDV
                ,DonGiaBH=0
                ,ISBHYT=0
                ,ThanhTienBH=0
                ,IDNHOMINBV=E.IDNHOMINBV
                ,TONGTIENBH=0
                ,TIENCLS=ISNULL(a.soluong,1)*D.GiaDV
                ,PhuThuBH=0
                ,TIENGIADV=ISNULL(a.soluong,1)*D.GiaDV
                ,XN= ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=3  THEN D.GiaDV ELSE 0 END)
                ,CDHA= ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV IN( 4,5)THEN D.GiaDV ELSE 0 END)
                ,TIEMTRUYEN= ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=8 THEN D.GiaDV ELSE 0 END)
                ,THUTHUAT= ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=6 THEN D.GiaDV ELSE 0 END)
                ,DVKTCAO= ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=7 THEN D.GiaDV ELSE 0 END)
                ,VANCHUYEN= ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=9 THEN D.GiaDV ELSE 0 END)
                ,CLSKHAC= ISNULL(a.soluong,1)*( CASE WHEN E.IDNHOMINBV=10 THEN D.GiaDV ELSE 0 END)
                ,BNTra=0
                ,BHTra=0
                ,TONGTIENBNPT=0
                ,NGAYXUAT=A.NGAYTHU
                from khambenhcanlamsan a
                inner join khambenh b on a.idkhambenh=b.idkhambenh
                inner join dangkykham c on b.iddangkykham=c.iddangkykham
                inner join hs_banggiavienphi d on d.IdGiaDichVu=(select top 1 IdGiaDichVu from hs_banggiavienphi D0 WHERE D0.IdDichVu=A.idcanlamsan AND D0.TuNgay<=A.ngaythu ORDER BY D0.TuNgay  DESC)
                INNER JOIN BANGGIADICHVU E ON A.idcanlamsan=E.idbanggiadichvu
                where 
                c.IdBenhBHDongTien=" + idbenhbhdongtien + @"
                ORDER BY A.NGAYTHU";
            DataTable dataTable2 = (DataTable)null;
            if (!IsNewDKK && IsCLS)
                dataTable2 = Connect.GetTable(strSelect2);
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
            {
                num6 += double.Parse(dataTable2.Compute("SUM(ThanhTienDV)", "").ToString());
                num1 += double.Parse(dataTable2.Compute("SUM(ThanhTienBH)", "").ToString());
                num19 += double.Parse(dataTable2.Compute("SUM(ThanhTienDV)", "").ToString());
                num7 += double.Parse(dataTable2.Compute("SUM(PhuThuBH)", "").ToString());
                num10 += double.Parse(dataTable2.Compute("SUM(XN)", "").ToString());
                num11 += double.Parse(dataTable2.Compute("SUM(CDHA)", "").ToString());
                num12 += double.Parse(dataTable2.Compute("SUM(TIEMTRUYEN)", "").ToString());
                num13 += double.Parse(dataTable2.Compute("SUM(THUTHUAT)", "").ToString());
                num16 += double.Parse(dataTable2.Compute("SUM(DVKTCAO)", "").ToString());
                num17 += double.Parse(dataTable2.Compute("SUM(VANCHUYEN)", "").ToString());
                num18 += double.Parse(dataTable2.Compute("SUM(CLSKHAC)", "").ToString());
            }
            DataTable dataTable3 = dataTable1;
            string strSelect3 = "select \r\n\t\t                                A.idchitietphieuxuat\r\n\t\t                                ,IDBENHBHDONGTIEN=c.IdBenhBHDongTien\r\n\t\t                                ,soluong=ISNULL(a.soluong,0)-isnull(A.sl_tra,0)\r\n\t\t                                ,DonGiaDV=ISNULL(A.DonGiaDV_TEMP,E.GIA_MUA)\r\n\t\t                                ,ThanhTienDV=(A.SOLUONG-isnull(A.sl_tra,0) )*ISNULL(A.DonGiaDV_TEMP,E.GIA_MUA)\r\n\t\t                                ,A.DonGiaDV_TEMP\r\n\t\t                                ,DonGiaBH=0\r\n\t\t                                ,ISBHYT=0\r\n\t\t                                ,ThanhTienBH=0\r\n\t\t                                ,TONGTIENBH=0\r\n\t\t                                ,TIENCLS=0\r\n\t\t                                ,TIENPHUTHUBH=0\r\n\t\t                                ,TIENGIADV=(A.SOLUONG-isnull(A.sl_tra,0) )*ISNULL(A.DonGiaDV_TEMP,E.GIA_MUA)\r\n\t\t                                ,VTYT  = (CASE WHEN  E.LoaiThuocID=4 THEN (A.SOLUONG-isnull(A.sl_tra,0) )*ISNULL(A.DonGiaDV_TEMP,E.GIA_MUA) ELSE 0 END)\r\n\t\t                                ,THUOC  = (CASE WHEN  E.LoaiThuocID=1 THEN (A.SOLUONG-isnull(A.sl_tra,0) )*ISNULL(A.DonGiaDV_TEMP,E.GIA_MUA)  ELSE 0 END)\r\n\t\t                                ,THUOCK=0\r\n                                        ,BNTra=0\r\n                                        ,BHTra=0\r\n                                        ,TONGTIENBNPT=0\r\n                                        ,NgayXuat=A.NGAYTHANG_XUAT\r\n                                        ,top1_idchitietbenhnhantoathuoc=(case when a.idkho_xuat=5 then a.top1_idchitietbenhnhantoathuoc ELSE 0 END)\r\n                                        ,GOI_DVKT=ISNULL(D.GOI_DVKT,'')\r\n                                 from chitietphieuxuatkho a\r\n                                inner join khambenh b on a.IDKHAMBENH1=b.idkhambenh\r\n                                inner join dangkykham c on b.iddangkykham=c.iddangkykham\r\n                                inner join chitietbenhnhantoathuoc d on a.idchitietbenhnhantoathuoc=d.idchitietbenhnhantoathuoc\r\n                                INNER JOIN thuoc E ON a.idthuoc=e.idthuoc\r\n                                where \r\n                                         ISNULL(D.IsHaoPhi,0)=0 \r\n                                        and ISNULL(a.soluong,0)-isnull(A.sl_tra,0)>0\r\n                                        and IsNull(a.IsBcTon,1)=1 AND\r\n                                        C.IdBenhBHDongTien=" + idbenhbhdongtien + "\r\n                                    ORDER BY A.NGAYTHANG_XUAT";
            DataTable dataTable4 = (DataTable)null;
            if (!IsNewDKK && IsThuoc)
                dataTable4 = Connect.GetTable(strSelect3);
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
            {
                num4 += double.Parse(dataTable4.Compute("SUM(ThanhTienDV)", "").ToString());
                num1 += double.Parse(dataTable4.Compute("SUM(ThanhTienBH)", "").ToString());
                num19 += double.Parse(dataTable4.Compute("SUM(ThanhTienDV)", "").ToString());
                num14 += double.Parse(dataTable4.Compute("SUM(VTYT)", "").ToString());
                num15 += double.Parse(dataTable4.Compute("SUM(THUOC)", "").ToString());
                num8 += double.Parse(dataTable4.Compute("SUM(TIENGIADV)", "").ToString());
            }
            string strSelect4 = "\r\n                       SELECT     IsBHYT=0\r\n                                 ,PhuThuBH=0\r\n                                 ,ThanhTienBH=0\r\n                                 ,TONGTIENBH=0\r\n                                  ,DonGiaDV=IsNull(A.DonGiaDV,0)\r\n                                 ,TIENGIUONG=IsNull(A.ThanhTienDV,0)\r\n                                 ,PhuThuBH=0\r\n                                ,ThanhTienDV=IsNull(A.ThanhTienDV,0)   \r\n                                 ,TONGTIENDV=IsNull(A.ThanhTienDV,0)         \r\n                                 ,TIENGIADV= IsNull(A.ThanhTienDV,0)\r\n                                ,BNTra=0\r\n                                ,BHTra=0\r\n                                ,TONGTIENBNPT=0\r\n                                ,A.IdChiTietGiuongBN              \r\n                     FROM  KB_CHITIETGIUONGBN A\r\n                          INNER JOIN CHITIETDANGKYKHAM B ON A.IDCHITIETDANGKYKHAM=B.IDchitietdangkykham\r\n                          INNER JOIN DANGKYKHAM C ON B.IDDANGKYKHAM=C.IDDANGKYKHAM\r\n                    WHERE C.IDBENHBHDONGTIEN=" + idbenhbhdongtien;
            DataTable dataTable5 = (DataTable)null;
            if (!IsNewDKK && IsTienGiuong)
                dataTable5 = Connect.GetTable(strSelect4);
            if (dataTable5 != null && dataTable5.Rows.Count > 0)
            {
                num1 += double.Parse(dataTable5.Compute("SUM(ThanhTienBH)", "").ToString());
                num19 += double.Parse(dataTable5.Compute("SUM(ThanhTienDV)", "").ToString());
                num7 += double.Parse(dataTable5.Compute("SUM(PhuThuBH)", "").ToString());
                num20 += double.Parse(dataTable5.Compute("SUM(ThanhTienBH)", "").ToString());
            }
            string str = "";
            for (int index = 0; dataTable3 != null && index < dataTable3.Rows.Count; ++index)
            {
                dataTable3.Rows[index]["BNTRA"] = (object)0;
                dataTable3.Rows[index]["BHTRA"] = (object)0;
                dataTable3.Rows[index]["TONGTIENBNPT"] = (object)double.Parse(dataTable3.Rows[index]["ThanhTienDV"].ToString());
                str = str + " UPDATE CHITIETDANGKYKHAM SET ISBHYT=0,DonGiaDV=" + dataTable3.Rows[index]["DonGiaDV"].ToString() + ",ThanhTienDV=" + dataTable3.Rows[index]["DonGiaDV"].ToString() + ",ThanhTienBH=0,DonGiaBH=0,PhuThuBH=0,BNTRA=0,BHTRA=0,BNTongPhaiTra=" + dataTable3.Rows[index]["TONGTIENBNPT"].ToString() + " WHERE IDCHITIETDANGKYKHAM=" + dataTable3.Rows[index]["idchitietdangkykham"].ToString() + "\r\n";
            }
            for (int index = 0; dataTable2 != null && index < dataTable2.Rows.Count; ++index)
            {
                dataTable2.Rows[index]["BNTRA"] = (object)0;
                dataTable2.Rows[index]["BHTRA"] = (object)0;
                dataTable2.Rows[index]["TONGTIENBNPT"] = (object)double.Parse(dataTable2.Rows[index]["ThanhTienDV"].ToString());
                str = str + @" UPDATE KHAMBENHCANLAMSAN SET 
                IsBHYT=0,DonGiaBH=0,ThanhTienBH=0,DonGiaDV=" + dataTable2.Rows[index]["DonGiaDV"].ToString() + @"
                ,ThanhTienDV=" + dataTable2.Rows[index]["ThanhTienDV"].ToString() + @"
                ,IDBENHBHDONGTIEN=" + dataTable2.Rows[index]["IDBENHBHDONGTIEN"].ToString() + @"
                ,PhuThuBH=0,BNTRA=0,BHTRA=0,BNTongPhaiTra=" + dataTable2.Rows[index]["TONGTIENBNPT"].ToString() + @"
                ,IdNhomInBV=" + (dataTable2.Rows[index]["IdNhomInBV"].ToString() == "" ? "NULL" : dataTable2.Rows[index]["IdNhomInBV"].ToString()) + @" 
                WHERE IDKHAMBENHCANLAMSAN=" + dataTable2.Rows[index]["IDKHAMBENHCANLAMSAN"].ToString() + "";
            }
            for (int index = 0; dataTable4 != null && index < dataTable4.Rows.Count; ++index)
            {
                dataTable4.Rows[index]["BNTRA"] = (object)0;
                dataTable4.Rows[index]["BHTRA"] = (object)0;
                dataTable4.Rows[index]["TONGTIENBNPT"] = (object)double.Parse(dataTable4.Rows[index]["ThanhTienDV"].ToString());
                if (dataTable4.Rows[index]["DonGiaDV"].ToString() == "")
                    dataTable4.Rows[index]["DonGiaDV"] = (object)"0";
                if (dataTable4.Rows[index]["ThanhTienDV"].ToString() == "")
                    dataTable4.Rows[index]["ThanhTienDV"] = (object)"0";
                if (dataTable4.Rows[index]["top1_idchitietbenhnhantoathuoc"].ToString() == "")
                    dataTable4.Rows[index]["top1_idchitietbenhnhantoathuoc"] = (object)"0";
                str = str + " UPDATE CHITIETPHIEUXUATKHO SET\r\n                                                        IsBHYT=0,DonGiaBH=0,ThanhTienBH=0,DonGiaDV=" + dataTable4.Rows[index]["DonGiaDV"].ToString() + ",ThanhTienDV=" + dataTable4.Rows[index]["ThanhTienDV"].ToString() + ",BNTRA=0,BHTRA=0,top1_idchitietbenhnhantoathuoc=" + dataTable4.Rows[index]["top1_idchitietbenhnhantoathuoc"].ToString() + ",IDBENHBHDONGTIEN=" + idbenhbhdongtien + ",GOI_DVKT=N'" + dataTable4.Rows[index]["GOI_DVKT"].ToString() + "' WHERE IDCHITIETPHIEUXUAT=" + dataTable4.Rows[index]["IDCHITIETPHIEUXUAT"].ToString() + "\r\n";
            }
            for (int index = 0; dataTable5 != null && index < dataTable5.Rows.Count; ++index)
            {
                dataTable5.Rows[index]["TONGTIENBNPT"] = (object)double.Parse(dataTable5.Rows[index]["ThanhTienDV"].ToString());
                str = str + " UPDATE KB_CHITIETGIUONGBN SET\r\n                                                                               IsBHYT=0\r\n                                                                               ,DonGiaBH=0\r\n                                                                               ,ThanhTienBH=0\r\n                                                                               ,DonGiaDV=" + dataTable5.Rows[index]["DonGiaDV"].ToString() + "\r\n                                                                               ,ThanhTienDV=" + dataTable5.Rows[index]["ThanhTienDV"].ToString() + "\r\n                                                                               ,BNTRA=0\r\n                                                                               ,BHTRA=0\r\n                                                                               WHERE IdChiTietGiuongBN=" + dataTable5.Rows[index]["IdChiTietGiuongBN"].ToString() + "\r\n";
            }
            if (dataTable3 != null && dataTable3.Rows.Count > 0)
                num3 = double.Parse(dataTable3.Compute("SUM(BNTRA)", "").ToString());
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
                num3 += double.Parse(dataTable2.Compute("SUM(BNTRA)", "").ToString());
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
                num3 += double.Parse(dataTable4.Compute("SUM(BNTRA)", "").ToString());
            if (dataTable3 != null && dataTable3.Rows.Count > 0)
                num2 = double.Parse(dataTable3.Compute("SUM(BHTRA)", "").ToString());
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
                num2 += double.Parse(dataTable2.Compute("SUM(BHTRA)", "").ToString());
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
                num2 += double.Parse(dataTable4.Compute("SUM(BHTRA)", "").ToString());
            if (dataTable3 != null && dataTable3.Rows.Count > 0)
                num9 = double.Parse(dataTable3.Compute("SUM(TONGTIENBNPT)", "").ToString());
            if (dataTable2 != null && dataTable2.Rows.Count > 0)
                num9 += double.Parse(dataTable2.Compute("SUM(TONGTIENBNPT)", "").ToString());
            if (dataTable4 != null && dataTable4.Rows.Count > 0)
                num9 += double.Parse(dataTable4.Compute("SUM(TONGTIENBNPT)", "").ToString());
            if (dataTable5 != null && dataTable5.Rows.Count > 0)
                num9 += double.Parse(dataTable5.Compute("SUM(TONGTIENBNPT)", "").ToString());
            Connect.ExecSQL(str + " \r\n                             \r\n    \r\n                            UPDATE HS_BENHNHANBHDONGTIEN SET TongTienBH=" + num1.ToString() + ",BHTra=" + num2.ToString() + ",BNPhaiTra=" + num3.ToString() + ",TienThuoc=" + (object)num4 + ",TienKham=" + (object)num5 + ",TienCLS=" + (object)num6 + ",TienPhuThuBH=" + (object)num7 + ",TONGTIENDV=" + (object)num19 + ",TongTienBNPT=" + (object)num9 + ",CDHA=" + (object)num11 + ",THUTHUAT=" + (object)num13 + ",CLSKhac=" + (object)num18 + ",DVKTCao=" + (object)num16 + ",TIEMTRUYEN=" + (object)num12 + ",TongTienBNPTConLai=" + (object)num9 + "- ISNULL( TongTienBNDaTra,0),VanChuyen=" + (object)num17 + ",VTYT=" + (object)num14 + ",XN=" + (object)num10 + ",THUOC=" + (object)num15 + ",TIENGIADV=" + (object)num8 + ",TIENGIUONG=" + (object)num20 + "\r\n                                        WHERE ID=" + idbenhbhdongtien);
            Connect.ExecSQL("\r\n                                         DECLARE @IDBENHBHDONGTIEN  AS BIGINT\r\n                                         SET @IDBENHBHDONGTIEN =" + idbenhbhdongtien + "\r\n                                          ---------------------------------------------------------------  \r\n                                        " + (IsPhiKham ? " \r\n                                         DELETE chitietdangkykham_HS  \r\n                                           WHERE   IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN  \r\n                                          ---------------------------------------------------------------   \r\n                                          INSERT INTO  chitietdangkykham_HS  \r\n                                          SELECT A.*,B.IdBenhBHDongTien  \r\n                                           FROM chitietdangkykham A  \r\n                                             INNER JOIN dangkykham B ON A.iddangkykham=B.iddangkykham  \r\n                                            WHERE   B.IdBenhBHDongTien =@IDBENHBHDONGTIEN  \r\n                                              AND ISNULL(A.dahuy,0)=0  \r\n                                              AND ISNULL(A.isNotThuPhiCapCuu,0)=0  \r\n                                              AND ISNULL(A.DONGIADV,0)>0  \r\n                                          -----------------------------------------------------------------------------  \r\n                                        " : "") + "\r\n                                    " + (IsNewDKK || !IsCLS ? "" : "                \r\n                                              DELETE khambenhcanlamsan_HS WHERE ISNULL(IDBENHBHDONGTIEN_HS, IDBENHBHDONGTIEN)=@IDBENHBHDONGTIEN                \r\n                                                 -----------------------------------------------------------------------------                   \r\n                                              INSERT INTO khambenhcanlamsan_HS                \r\n                                              (                \r\n                                              idkhambenhcanlamsan,                \r\n                                              idkhambenh,                \r\n                                              idcanlamsan,                \r\n                                              dongia,                \r\n                                              idbacsi,                \r\n                                              dathu,                \r\n                                              dakham,                \r\n                                              ngaythu,                \r\n                                              idnguoidung,                \r\n                                              ngaykham,                \r\n                                              idbenhnhan,                \r\n                                              tenBSChiDinh,                \r\n                                              maphieuCLS,                \r\n                                              thucthu,                \r\n                                              dahuy,                \r\n                                              soluong,                \r\n                                              chietkhau,                \r\n                                              thanhtien,                \r\n                                              IdNguoiThu,                \r\n                                              BHTra,                \r\n                                              GhiChu,                \r\n                                              LoaiKhamID,                \r\n                                              idkhoadangky,                \r\n                                              sldakham,                \r\n                                              istieuphau,                \r\n                                              isphauthuat,                \r\n                                              IsHoanTraCLS,                \r\n                                              BHYTTra,                \r\n                                              BNDaTra,                \r\n                                              BNTongPhaiTra,                \r\n                                              BNTra,              \r\n                                              DonGiaBH,                \r\n                                              DonGiaDV,                \r\n                                              IdChiTietPTT,                \r\n                                              IsBHYT,                \r\n                                              ISBNDaTra,                \r\n                                              NgayQuayLai,                \r\n                                              NGAYTINHBH_THUC,                \r\n                                              PhuThuBH,                \r\n                                              PrevBNTra,                \r\n                                              ThanhTienBH,                \r\n                                              ThanhTienDV,                \r\n                                              IDDANGKYCLS,                \r\n                                              IsDaDKK,                \r\n                                              IdnhomInBV,              \r\n                                              IsBHYT_Save,                \r\n                                              TOP1_IDKHAMBENHCANLAMSAN,                \r\n                                              ISDATRAKQ,                \r\n                                              THOIGIANTRAKQ,                \r\n                                              madangkycls,                \r\n                                              chandoansobo,                \r\n                                              DonGiaBH_Temp,                \r\n                                              IdBacSiCLS,                \r\n                                              SoID,                \r\n                                              NGAYCLS,                \r\n                                              per50,                \r\n                                              per80,                \r\n                                              IDBENHBHDONGTIEN,                \r\n                                              IDBENHBHDONGTIEN_HS                \r\n                                            )                \r\n                                                            \r\n                                              SELECT                 \r\n                                                A.idkhambenhcanlamsan,                \r\n                                                A.idkhambenh,                \r\n                                                A.idcanlamsan,                \r\n                                                A.dongia,                \r\n                                                A.idbacsi,                \r\n                                                A.dathu,                \r\n                                                A.dakham,                \r\n                                                A.ngaythu,                \r\n                                                A.idnguoidung,                \r\n                                                A.ngaykham,                \r\n                                                A.idbenhnhan,                \r\n                                                A.tenBSChiDinh,                \r\n                                                A.maphieuCLS,                \r\n                                                A.thucthu,                \r\n                                                A.dahuy,                \r\n                                                A.soluong,                \r\n                                                A.chietkhau,                \r\n                                                A.thanhtien,                \r\n                                                A.IdNguoiThu,                \r\n                                                A.BHTra,                \r\n                                                A.GhiChu,                \r\n                                                A.LoaiKhamID,                \r\n                                                A.idkhoadangky,                \r\n                                                A.sldakham,                \r\n                                                A.istieuphau,                \r\n                                                A.isphauthuat,                \r\n                                                A.IsHoanTraCLS,                \r\n                                                A.BHYTTra,                \r\n                                                A.BNDaTra,                \r\n                                                A.BNTongPhaiTra,                \r\n                                                A.BNTra,                \r\n                                                A.DonGiaBH,                \r\n                                                A.DonGiaDV,                \r\n                                            A.IdChiTietPTT,                \r\n                                                A.IsBHYT,                \r\n                                                A.ISBNDaTra,                \r\n                                                A.NgayQuayLai,                \r\n                                                A.NGAYTINHBH_THUC,                \r\n                                                A.PhuThuBH,                \r\n                                                A.PrevBNTra,                \r\n                                                A.ThanhTienBH,                \r\n                                                A.ThanhTienDV,         \r\n                                                A.IDDANGKYCLS,                \r\n                                                A.IsDaDKK,                \r\n                                                A.IdnhomInBV,                \r\n                                                A.IsBHYT_Save,                \r\n                                                                \r\n                                                A.TOP1_IDKHAMBENHCANLAMSAN,                \r\n                                                A.ISDATRAKQ,                \r\n                                                A.THOIGIANTRAKQ,                \r\n                                                A.madangkycls,                \r\n                                                A.chandoansobo,                \r\n                                                A.DonGiaBH_Temp,                \r\n                                                A.IdBacSiCLS,                \r\n                                                A.SoID,                \r\n                                                A.NGAYCLS,                \r\n                                                A.per50,                \r\n                                                A.per80,                \r\n                                                IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN,                \r\n                                                IDBENHBHDONGTIEN_HS=@IDBENHBHDONGTIEN                \r\n                                              FROM khambenhcanlamsan A                \r\n                                              INNER JOIN khambenh B  ON A.idkhambenh=B.idkhambenh                \r\n                                              INNER JOIN dangkykham C ON B.iddangkykham=C.iddangkykham                \r\n                                              WHERE C.IdBenhBHDongTien=@IDBENHBHDONGTIEN                \r\n                                               AND ISNULL(A.dahuy,0)=0                \r\n                                              -----------------------------------------------------------------------------  \r\n                                         ") + (IsNewDKK || !IsThuoc ? "" : "\r\n                                           -----------------------------------------------------------------------------   \r\n                                          DELETE CHITIETPHIEUXUATKHO_HS WHERE ISNULL(IDBENHBHDONGTIEN_HS,IDBENHBHDONGTIEN)=@IDBENHBHDONGTIEN  \r\n                                           INSERT INTO CHITIETPHIEUXUATKHO_HS(  \r\n                                                                idchitietphieuxuat,  \r\n                                                       idphieuxuat,  \r\n                                                       idthuoc,  \r\n                                                       soluong,  \r\n                                                       dongia,  \r\n                                                       idchitietphieunhapkho,  \r\n                                                       idtu,  \r\n                                                       idngan,  \r\n                                                       sluongxuat,  \r\n                                                       dvt,  \r\n                                                       thanhtien,  \r\n                                                       thanhtientruocthue,  \r\n                                                       tienthue,  \r\n                                                       chietkhau,  \r\n                                                       giavon,  \r\n                                                       tienvon,  \r\n                                                       tiensauchietkhau,  \r\n                                                       tienchietkhau,  \r\n                                                       IDCHITIETPHIEUYEUCAUXUATKHO,  \r\n                                                       tkkho,  \r\n                                                       tkco,  \r\n                                                       idchitietbenhnhantoathuoc,  \r\n                                                       idThuocPhauThuat,  \r\n                                                       VAT,  \r\n                                                       IDKHO_XUAT,  \r\n                                                       NGAYTHANG_XUAT,  \r\n                                                       LOSANXUAT_XUAT,  \r\n                                                       NGAYHETHAN_XUAT,  \r\n                                                       IDLOAIXUAT_XUAT,  \r\n                                                       BHTra,  \r\n                                                       BNDaTra,  \r\n                                                       BNTra,  \r\n                                                       DonGiaBH,  \r\n                                                       DonGiaDV,  \r\n                                                       IsBHYT,  \r\n                                                       ISBNDaTra,  \r\n                                                       IsDaHuy,  \r\n                                                       NGAYTINHBH_THUC,  \r\n                                                       PrevBNTra,  \r\n                                                       SL_Prev,  \r\n                                                       ThanhTien_Prev,  \r\n                                                       ThanhTienBH,  \r\n                                                       ThanhTienDV,  \r\n                                                       OutPutDetailID,  \r\n                                                       IDKHAMBENH1,  \r\n                                                       IdNuocSX_X,  \r\n                                                       GhiChu,  \r\n                                                       soPhieuTra,  \r\n                                                       ngayNhanTra,  \r\n                                                       soLuong_bk,  \r\n                                                       IsTinhTien,  \r\n                                                       PHUTHUBH,  \r\n                                                       IDBENHBHDONGTIEN,  \r\n                                                       TOP1_IDCHITIETBENHNHANTOATHUOC,  \r\n                                                       NHACUNGCAPID_X,  \r\n                                                       SOHOADON_X,  \r\n                                                       NGAYHOADON_X,  \r\n                                                       IdLoaiThuoc,  \r\n                                                       ISBHYT_NHAP,  \r\n                                                       SODK_X,  \r\n                                                       IDCHITIETPHIEUYEUCAUTRAKHO,  \r\n                                                       SOLUONG_PREV,  \r\n                                                       IDPHIEUXUAT_YCTRA,  \r\n                                                       SAVEDATE,  \r\n                                                       FIRSTDATE,  \r\n                                                       NotRunTrigger,  \r\n                                                       ISBHYT_SAVE_X,  \r\n                                                       ISHAOPHI_X,  \r\n                                                       ISXUAT_HV,  \r\n                                                       IDBENHBHDONGTIEN_HS,  \r\n                                                       ISBHYT_SAVE_HS,  \r\n                                                       IDKHOA  ,\r\n                                                        GOI_DVKT\r\n                                                       )  \r\n                                             SELECT  A. idchitietphieuxuat,  \r\n                                                     A.idphieuxuat,  \r\n                                                     A.idthuoc,  \r\n                                                     soluong=A.SOLUONG-isnull(A.sl_tra,0),  \r\n                                                     A.dongia,  \r\n                                                     A.idchitietphieunhapkho,  \r\n                                                     A.idtu,  \r\n                                                     A.idngan,  \r\n                                                     A.sluongxuat,  \r\n                                                     A.dvt,  \r\n                                                     A.thanhtien,  \r\n                                                     A.thanhtientruocthue,  \r\n                                                     A.tienthue,  \r\n                                                     A.chietkhau,  \r\n                                                     A.giavon,  \r\n                                                     A.tienvon,  \r\n                                                     A.tiensauchietkhau,  \r\n                                                     A.tienchietkhau,  \r\n                                                     A.IDCHITIETPHIEUYEUCAUXUATKHO,  \r\n                                                     A.tkkho,  \r\n                                                     A.tkco,  \r\n                                                     A.idchitietbenhnhantoathuoc,  \r\n                                                     A.idThuocPhauThuat,  \r\n                                                     A.VAT,  \r\n                                                     A.IDKHO_XUAT,  \r\n                                                     A.NGAYTHANG_XUAT,  \r\n                                                     A.LOSANXUAT_XUAT,  \r\n                                                     A.NGAYHETHAN_XUAT,  \r\n                                                     A.IDLOAIXUAT_XUAT,  \r\n                                                     A.BHTra,  \r\n                                                     A.BNDaTra,  \r\n                                                     A.BNTra,  \r\n                                                     A.DonGiaBH,  \r\n                                                     A.DonGiaDV,  \r\n                                                     A.IsBHYT,  \r\n                                                     A.ISBNDaTra,  \r\n                                                     A.IsDaHuy,  \r\n                                                     A.NGAYTINHBH_THUC,  \r\n                                                     A.PrevBNTra,  \r\n                                                     A.SL_Prev,  \r\n                                                     A.ThanhTien_Prev,  \r\n                                                     A.ThanhTienBH,  \r\n                                                     A.ThanhTienDV,  \r\n                                                     A.OutPutDetailID,  \r\n                                                     A.IDKHAMBENH1,  \r\n                                                     A.IdNuocSX_X,  \r\n                                                     A.GhiChu,  \r\n                                                     A.soPhieuTra,  \r\n                                                     A.ngayNhanTra,  \r\n                                                     A.soLuong_bk,  \r\n                                                     A.IsTinhTien,  \r\n                                                     A.PHUTHUBH,  \r\n                                                     A.IDBENHBHDONGTIEN,  \r\n                                                     A.TOP1_IDCHITIETBENHNHANTOATHUOC,  \r\n                                                     A.NHACUNGCAPID_X,  \r\n                                                     A.SOHOADON_X,  \r\n                                                     A.NGAYHOADON_X,  \r\n                                                     A.IdLoaiThuoc,  \r\n                                                     A.ISBHYT_NHAP,  \r\n                                                     A.SODK_X,  \r\n                                                     A.IDCHITIETPHIEUYEUCAUTRAKHO,  \r\n                                                     A.SOLUONG_PREV,  \r\n                                                     A.IDPHIEUXUAT_YCTRA,  \r\n                                                     A.SAVEDATE,  \r\n                                                     A.FIRSTDATE,  \r\n                                                     A.NotRunTrigger,  \r\n                                                     A.ISBHYT_SAVE_X,  \r\n                                                     A.ISHAOPHI_X,  \r\n                                                     A.ISXUAT_HV  \r\n                                                       ,IdBenhBHDongTien_HS=C.IdBenhBHDongTien  \r\n                                                       ,ISBHYT_SAVE_HS=A.ISBHYT_SAVE_X  \r\n                                                       ,IDKHOA=B.idphongkhambenh  \r\n                                                        ,GOI_DVKT=D.GOI_DVKT\r\n                                               from chitietphieuxuatkho a\r\n                                                    inner join khambenh b on a.IDKHAMBENH1=b.idkhambenh\r\n                                                    inner join dangkykham c on b.iddangkykham=c.iddangkykham\r\n                                                    inner join chitietbenhnhantoathuoc d on a.idchitietbenhnhantoathuoc=d.idchitietbenhnhantoathuoc\r\n                                                    INNER JOIN thuoc E ON a.idthuoc=e.idthuoc\r\n                                                    where \r\n                                                             ISNULL(D.IsHaoPhi,0)=0 \r\n                                                            and ISNULL(a.soluong,0)-isnull(A.sl_tra,0)>0\r\n                                                            and IsNull(a.IsBcTon,1)=1 AND\r\n                                                            C.IdBenhBHDongTien=" + idbenhbhdongtien + "\r\n                                                        ORDER BY A.NGAYTHANG_XUAT\r\n                                                    \r\n                                                \r\n                                         ----------------------------------------------------------------------------- \r\n                                        ") + (IsNewDKK || !IsTienGiuong ? "" : "\r\n                                                     \r\n                                      DELETE KB_CHITIETGIUONGBN_HS WHERE ISNULL(IDBENHBHDONGTIEN_HS, IDBENHBHDONGTIEN)=@IDBENHBHDONGTIEN                 \r\n                                      -----------------------------------------------------------------------------                    \r\n                                      INSERT INTO KB_CHITIETGIUONGBN_HS                \r\n                                      SELECT *,IDBENHBHDONGTIEN_HS=@IDBENHBHDONGTIEN                \r\n                                       FROM KB_CHITIETGIUONGBN                \r\n                                       WHERE IDchitietdangkykham  IN                \r\n                                           ( SELECT IDchitietdangkykham                 \r\n                                             FROM chitietdangkykham A                 \r\n                                               INNER JOIN dangkykham B ON A.iddangkykham=B.iddangkykham                \r\n                                              WHERE B.IdBenhBHDongTien=@IDBENHBHDONGTIEN                \r\n                                           )                \r\n                                        AND ISNULL( KB_CHITIETGIUONGBN.SL,0)>0                   \r\n                                        AND ISNULL( DonGiaDV,0)>0                   \r\n                                     ----------------------------------------------------------------------------- \r\n                                    "));
            return true;
        }

        public static DataTable dtSource_BV(string idphieutt)
        {
            return Connect.GetTable(@"SELECT A.*,B.*,C.*,MA_LK=ISNULL(A.ID_XML,A.ID),gioi_tinh=(case when b.gioitinh=1 then 1 else 0 end)+1,dia_chi=b.diachi,
                                        ma_the = C.SOBHYT, ma_dkbd = D.MADANGKY, ten_dkbd = D.TENNOIDANGKY, gt_the_tu = CONVERT(NVARCHAR(20), C.ngaybatdau, 103), gt_the_den = CONVERT(NVARCHAR(20), C.ngayhethan, 103),
                                        ten_benh = IsNull(A.TenChanDoan, F.MOTACD_EDIT), MA_BENH = IsNull(A.MACHANDOAN, H.MAICD)
                                        , ma_lydo_vvien = (CASE WHEN A.ISCAPCUU = 1 THEN 2 ELSE(CASE WHEN A.DUNGTUYEN = 'Y' THEN 1 ELSE 3 END) END)
                                        ,ma_noi_chuyen = REPLACE(E.MADANGKY, '-', '')
                                        ,ten_noi_chuyen = E.TENNOIDANGKY
                                        ,SO_NGAY_DTRI = (case when isnull(A.ISNOITRU, 0)= 0 THEN 0 ELSE ISNULL(NULL,(CASE WHEN CONVERT(VARCHAR, A.NGAYTINHBH, 111) = CONVERT(VARCHAR, A.NgayTinhBH_Thuc, 111) THEN 1 ELSE    CONVERT(INT, CONVERT(DATETIME, CONVERT(VARCHAR, A.NgayTinhBH_Thuc, 111)) - CONVERT(DATETIME, CONVERT(VARCHAR, A.NgayTinhBH, 111))) + (CASE WHEN CONVERT(INT, CONVERT(DATETIME, CONVERT(VARCHAR, A.NgayTinhBH_Thuc, 111)) - CONVERT(DATETIME, CONVERT(VARCHAR, A.NgayTinhBH, 111))) >= 2 THEN 1 ELSE 0 END )END))END),
                                         ket_qua_dtri = ISNULL(A.ket_qua_dtri, '1')
                                        ,tinh_trang_rv_BV = (CASE WHEN ISNULL(A.ket_qua_dtri, 0) IN(0, 1, 2) THEN '1' ELSE '2' END)
                                        , MUC_HUONG = A.MUC_HUONG,ma_loai_kcb = (case when ISNULL(A.IsDieuTriNgoaiTru, 0)= 1 THEN '2' ELSE(case when ISNULL(A.IsNoiTru, 0) = 1 THEN '3' ELSE '1' END) END)
                                        ,MA_KHOA_VIEW = G.MAPHONGKHAMBENH
                                        ,TEN_KHOA_VIEW = UPPER((case when G.tenphongkhambenh = N'KHOA N?I 2' THEN N'KHOA N?I' ELSE(CASE WHEN G.tenphongkhambenh = N'KHOA NGO?I 2' THEN N'KHOA NGO?I' ELSE G.tenphongkhambenh END) END)) ,ID_KHOA_VIEW = G.IDPHONGKHAMBENH
                                         , ma_khuvuc = C.MA_KHUVUC
                                         -- ,  can_nang = (CASE WHEN RIGHT(B.NGAYSINH, 4)= CONVERT(VARCHAR(4), YEAR(GETDATE())) AND ISNULL(CAN_NANG,0)= 0 THEN ISNULL((SELECT TOP 1 1 FROM sinhhieu A0 INNER JOIN khambenh B0 ON A0.IdKhamBenh = B0.idkhambenh INNER JOIN dangkykham C0 ON B0.iddangkykham = C0.iddangkykham WHERE C0.IdBenhBHDongTien = A.ID AND ISNULL(A0.cannang, 0) <> 0 ),3) ELSE A.CAN_NANG END )                              
                                         --,TUOIBN = DBO.kb_GetTuoi(b.ngaysinh)
                                         ,IsShow = (CASE WHEN ISNULL(A.ID_XML, 0)<> 0 THEN '0' ELSE '1' END )
                                         ,TONGTIEN_DATRA = ISNULL((SELECT SUM(A0.TONGTIEN) FROM HS_DSDATHU A0 WHERE A0.IDBENHNHANBHDONGTIEN = A.ID AND ISNULL(A0.ISDAHUY, 0) = 0 AND A0.LOAITHU <> 'VIENPHINOITRU'),0),TenBV_ChuyenDi = I.TenBenhVien,ngaytrinhthe1 = convert(varchar, a.ngaytrinhthe, 111),ngaytrinhthe2 = convert(varchar, a.ngaytrinhthe2, 111)
                                         ,idbenhnhan_bh1 = a.idbenhnhan_bh
                                         ,idbenhnhan_bh2 = a.idbenhnhan_bh2
                                         ,SOBH1_2 = BH2.SOBH1
                                         ,SOBH2_2 = BH2.SOBH2
                                         ,SOBH3_2 = BH2.SOBH3
                                         ,SOBH4_2 = BH2.SOBH4
                                         ,SOBH5_2 = BH2.SOBH5
                                         ,SOBH6_2 = BH2.SOBH6
                                         ,ngaybatdau2 = CONVERT(VARCHAR, BH2.ngaybatdau, 111)
                                         ,ngayhethan2 = CONVERT(VARCHAR, BH2.ngayhethan, 111)
                                       
                                          from hs_benhnhanbhdongtien a
                                        left join benhnhan b on a.idbenhnhan = B.IDBENHNHAN
                                        left JOIN BENHNHAN_BHYT C ON A.IDBENHNHAN_BH = C.IDBENHNHAN_BH
                                        left join KB_NOIDANGKYKB d on c.IdNoiDangKyBH = D.IDNOIDANGKY
                                        LEFT JOIN KB_NOIDANGKYKB E ON C.IdNoiGioiThieu = E.IDNOIDANGKY
                                        LEFT JOIN KHAMBENH F ON A.IDKHAMBENH_LAST = F.IDKHAMBENH
                                        LEFT JOIN PHONGKHAMBENH G ON F.IDPHONGKHAMBENH = G.IDPHONGKHAMBENH
                                         LEFT JOIN CHANDOANICD H ON F.KETLUAN = H.IDICD
                                        LEFT JOIN benhvien I ON F.IdBenhVienChuyen = I.idBenhVien
                                        left JOIN BENHNHAN_BHYT BH2 ON A.IDBENHNHAN_BH2 = BH2.IDBENHNHAN_BH WHERE a.id = " + idphieutt);
        }

        public static DataSet dtSourceDetail_BV(string idphieutt, bool IsNoiTru_bool, string ngaytrinhthe1, string ngaytrinhthe2, string idbenhnhan_bh1, string idbenhnhan_bh2)
        {
            string str1 = "\r\n                                DECLARE @NGAYTRINHTHE1 DATETIME,@NGAYTRINHTHE2 DATETIME, @IDBENHNHAN_BH1 BIGINT, @IDBENHNHAN_BH2 BIGINT\r\n                                SET    @NGAYTRINHTHE1=" + (ngaytrinhthe1 == null || !(ngaytrinhthe1 != "") || !(ngaytrinhthe1 != "null") ? "null" : "'" + DateTime.Parse(ngaytrinhthe1).ToString("yyyy/MM/dd") + "'") + " \r\n                                SET    @NGAYTRINHTHE2=" + (ngaytrinhthe2 == null || !(ngaytrinhthe2 != "") || !(ngaytrinhthe2 != "null") ? "null" : "'" + DateTime.Parse(ngaytrinhthe2).ToString("yyyy/MM/dd") + "'") + " \r\n                                SET    @idbenhnhan_bh1=" + (idbenhnhan_bh1 == null || !(idbenhnhan_bh1 != "") || !(idbenhnhan_bh1 != "null") ? "null" : idbenhnhan_bh1 ?? "") + "\r\n                                SET    @idbenhnhan_bh2=" + (idbenhnhan_bh2 == null || !(idbenhnhan_bh2 != "") || !(idbenhnhan_bh2 != "null") ? "null" : idbenhnhan_bh2 ?? "") + "\r\n               ";
            string str2 = "\r\n                                select A.*\r\n                                       ,SoTTView=B.SoTTView_2019                                        \r\n                                       ,TENNHOM=CONVERT(NVARCHAR, B.SoTTView_2019)+'. '+B.TENNHOM_2019                                    \r\n                                       ,TENPHANNHOM=b.tenphannhom_2019                                        \r\n                                       ,LOAIDICHVU=A.NOIDUNG                                        \r\n                                       ,DVT=A.DONVITINH\r\n                                       ,KHOA_CD=A.KHOA\r\n                                       ,BN_TU_TRA=(CASE WHEN A.ISBHYT=1 THEN A.PHUTHUBH ELSE A.THANHTIENDV END)\r\n   ,MaVach=CONVERT(IMAGE,NULL)                   ";
            string str3 = "\r\n                            INNER JOIN HS_NHOMINBV B ON ( CASE WHEN A.IDNHOMINBV<>2 THEN (CASE WHEN A.IDNHOMINBV=15 THEN 19 ELSE (CASE WHEN A.IDNHOMINBV=16 THEN 20 ELSE A.IDNHOMINBV  END) END) ELSE  " + (IsNoiTru_bool ? "18" : "17") + " END)=B.IdNhom  ";
            string strSelect;
            if (!IsNoiTru_bool)
            {
                strSelect = str1 + str2 + " from zHS_ViewBV01(" + idphieutt + ") A " + str3;
            }
            else
            {
                if (ngaytrinhthe2 != null && ngaytrinhthe2 != "" && ngaytrinhthe2 != "null")
                    str1 = str1 + str2 + " from zHS_ViewBV_2019(" + idphieutt + ",null,@NGAYTRINHTHE2) A " + str3 + "\r\n                            " + str2 + " from zHS_ViewBV_2019(" + idphieutt + ",@NGAYTRINHTHE2,null) A " + str3;
                strSelect = str1 + str2 + " from zHS_ViewBV_2019(" + idphieutt + ",null,null) A " + str3;
            }
            DataSet dataSet1 = DataAcess.Connect.GetDataSet(strSelect);
            DataSet dataSet2 = new DataSet();
            for (int index1 = 0; index1 < dataSet1.Tables.Count; ++index1)
            {
                DataTable dataTable = dataSet1.Tables[index1].Copy();
                if (dataTable != null && dataTable.Rows.Count > 0 && hs_tinhtien.int_Search(dataTable, "GOI_DVKT<>''") != -1)
                {
                    dataTable.DefaultView.Sort = "KHOA_CD,SoTTView ,TENNHOM,TENPHANNHOM,GOI_DVKT,SOTT_DV,LOAIDICHVU,THANHTIEN,THANHTIENDV  ";
                    dataTable = dataTable.DefaultView.ToTable();
                    List<string> stringList = new List<string>();
                    for (int index2 = 0; index2 < dataTable.Rows.Count; ++index2)
                    {
                        if (dataTable.Rows[index2]["GOI_DVKT"].ToString() != "" && (dataTable.Rows[index2]["IDNHOMINBV"].ToString() == "15" || dataTable.Rows[index2]["IDNHOMINBV"].ToString() == "16" || dataTable.Rows[index2]["IDNHOMINBV"].ToString() == "19" || dataTable.Rows[index2]["IDNHOMINBV"].ToString() == "20"))
                        {
                            dataTable.Rows[index2]["TENNHOM"] = (object)"10. G󩠶?t tu y t?";
                            dataTable.Rows[index2]["SoTTView"] = (object)"10";
                            int num1 = stringList.IndexOf(dataTable.Rows[index2]["GOI_DVKT"].ToString());
                            int num2;
                            if (num1 == -1)
                            {
                                stringList.Add(dataTable.Rows[index2]["GOI_DVKT"].ToString());
                                DataRow row = dataTable.Rows[index2];
                                string index3 = "TENPHANNHOM";
                                string[] strArray1 = new string[7];
                                strArray1[0] = "10.";
                                string[] strArray2 = strArray1;
                                int index4 = 1;
                                num2 = stringList.Count;
                                string str4 = num2.ToString();
                                strArray2[index4] = str4;
                                strArray1[2] = ". G󩠶?t tu y t? ";
                                string[] strArray3 = strArray1;
                                int index5 = 3;
                                num2 = stringList.Count;
                                string str5 = num2.ToString();
                                strArray3[index5] = str5;
                                strArray1[4] = " (";
                                strArray1[5] = dataTable.Rows[index2]["GOI_DVKT"].ToString();
                                strArray1[6] = ")";
                                string str6 = string.Concat(strArray1);
                                row[index3] = (object)str6;
                            }
                            else
                            {
                                DataRow row = dataTable.Rows[index2];
                                string index3 = "TENPHANNHOM";
                                string[] strArray1 = new string[7];
                                strArray1[0] = "10.";
                                string[] strArray2 = strArray1;
                                int index4 = 1;
                                num2 = num1 + 1;
                                string str4 = num2.ToString();
                                strArray2[index4] = str4;
                                strArray1[2] = ". G󩠶?t tu y t? ";
                                string[] strArray3 = strArray1;
                                int index5 = 3;
                                num2 = num1 + 1;
                                string str5 = num2.ToString();
                                strArray3[index5] = str5;
                                strArray1[4] = " (";
                                strArray1[5] = dataTable.Rows[index2]["GOI_DVKT"].ToString();
                                strArray1[6] = ")";
                                string str6 = string.Concat(strArray1);
                                row[index3] = (object)str6;
                            }
                        }
                    }
                }
                else if (dataTable != null)
                {
                    dataTable.DefaultView.Sort = "KHOA_CD,SoTTView ,TENNHOM,TENPHANNHOM,SOTT_DV ,LOAIDICHVU,THANHTIEN,THANHTIENDV  ";
                    dataTable = dataTable.DefaultView.ToTable();
                }
                dataSet2.Tables.Add(dataTable);
            }
            return dataSet2;
        }

        public static string ConvertMoneyToText(string csMoney)
        {
            string csResult = "";
            if (csMoney == "") return "";
            while (csMoney.Substring(0, 1) == "0")
            {
                if (csMoney.Length > 1)
                    csMoney = csMoney.Substring(1, csMoney.Length - 1);
                else
                    if (csMoney == "0")
                {
                    csMoney = "";
                    break;
                }
            }

            if (csMoney != "")
            {
                string csTram = "";
                string csNghin = "";
                string csTrieu = "";
                string csPart1 = "";
                string csPart2 = "";
                string delimStr = ",.";
                char[] delimiter = delimStr.ToCharArray();
                string[] split = null;
                split = csMoney.Split(delimiter, 2);
                csPart1 = split[0];
                if (split.Length > 1)
                {
                    csPart2 = split[1];
                    if (csPart2.Length > 3)
                        csPart2 = csPart2.Substring(0, 3);

                    csPart2 = SplipNumber(csPart2);

                    if (csPart2 != "")
                        csResult = "phay " + csPart2;
                }

                if (csPart1.Length > 9)
                {
                    //  MessageBox.Show("Tien thanh toan len den tien ty la dieu khong the\r\nHay xem du lieu va tinh toan lai di Oai vo van");
                    return "";
                }

                while (csPart1.Length > 3)
                {
                    if (csTram == "")
                        csTram = csPart1.Substring(csPart1.Length - 3, 3);
                    else
                        csNghin = csPart1.Substring(csPart1.Length - 3, 3);

                    csPart1 = csPart1.Substring(0, csPart1.Length - 3);

                }

                if (csTram == "")
                    csTram = csPart1;
                else
                    if (csNghin == "")
                    csNghin = csPart1;
                else
                    csTrieu = csPart1;

                if (csTram != "")
                    csTram = SplipNumber(csTram);
                if (csNghin != "")
                    csNghin = SplipNumber(csNghin);
                if (csTrieu != "")
                    csTrieu = SplipNumber(csTrieu);

                if (csTram != "")
                    csResult = csTram + csResult;
                if (csNghin != "")
                    csResult = csNghin + "nghìn " + csResult;
                if (csTrieu != "")
                    csResult = csTrieu + "triệu " + csResult;

                csResult += "đồng";
            }
            if (csResult.Length > 2)

                return csResult = char.ToUpper(csResult[0]).ToString() + csResult.Substring(1, csResult.Length - 1);
            return csResult;
        }

        public static string SplipNumber(string csNumber)
        {
            string csText = "";
            string csResult = "";

            if (csNumber == "000" || csNumber == "00" || csNumber == "0")
                return csResult;

            char[] NumberArr = null;
            int nCount = 0;
            NumberArr = csNumber.ToCharArray();
            for (int i = NumberArr.Length - 1; i >= 0; i--)
            {
                switch (NumberArr[i])
                {
                    case '0':
                        if (i == NumberArr.Length - 1)
                            csText = "";
                        else
                            if (i == NumberArr.Length - 2)
                            if (NumberArr[NumberArr.Length - 1] != '0')
                                csText = "lẻ ";
                            else
                                csText = "";
                        else
                            csText = "không ";
                        break;
                    case '1':
                        if (i == NumberArr.Length - 2)
                            csText = "mười ";
                        else
                            csText = "một ";
                        break;
                    case '2':
                        csText = "hai ";
                        break;
                    case '3':
                        csText = "ba ";
                        break;
                    case '4':
                        csText = "bốn ";
                        break;
                    case '5':
                        if ((i == NumberArr.Length - 1) && (NumberArr.Length != 1))
                            csText = "lăm ";
                        else
                            csText = "năm ";
                        break;
                    case '6':
                        csText = "sáu ";
                        break;
                    case '7':
                        csText = "bảy ";
                        break;
                    case '8':
                        csText = "tám ";
                        break;
                    case '9':
                        csText = "chín ";
                        break;
                }
                if ((nCount == 1) && (NumberArr[i] != '0') && (NumberArr[i] != '1'))
                    csText += "mươi ";
                else
                    if (nCount == 2)
                    csText += "trăm ";
                csResult = csText + csResult;
                nCount++;
            }

            csResult = csResult.Replace("mươi một", "mươi mốt");
            return csResult;
        }

        #region tạo mã phiếu cls
        public static string MaPhieuCLS_new()
        {
            string MaPhieu_tepm = null;
            string SysDate_ = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sqlSelectMaPhieu = "select max(SoTT) from Kb_MAPHIEUCLS where year(SysDate)=year('" + SysDate_ + "') and month(SysDate)=month('" + SysDate_ + "')";
            DataTable dtMaPhieu = DataAcess.Connect.GetTable(sqlSelectMaPhieu);
            if (dtMaPhieu == null) MaPhieu_tepm = "0";
            else
            {
                if (dtMaPhieu.Rows.Count == 0) MaPhieu_tepm = "0";
                else
                {
                    MaPhieu_tepm = dtMaPhieu.Rows[0][0].ToString();

                }

                if (MaPhieu_tepm == "") MaPhieu_tepm = "0";
                long d = long.Parse(MaPhieu_tepm);
                bool ok = false;
                while (ok == false)
                {

                    d++;
                    MaPhieu_tepm = "PT-" + DateTime.Parse(SysDate_).ToString("yyMM") + "0000000".Substring(0, 5 - d.ToString().Length) + d.ToString();
                    MaPhieu_tepm = MaPhieu_tepm + "CT";
                    string sqlSave = "insert into Kb_MAPHIEUCLS(SoTT,SysDate,MaBienLai) values(" + d.ToString() + ",'" + SysDate_ + "','" + MaPhieu_tepm + "')";
                    ok = DataAcess.Connect.ExecSQL(sqlSave);
                }
            }
            return MaPhieu_tepm;
        }
        #endregion

        public static string GetSoVaoVien(string idkhambenh, string idchitietdangkykham, string IsNoiTru)
        {
            if (idchitietdangkykham == "" || IsNoiTru == null || IsNoiTru == "") return "";
            string sqlSelect1 = @"SELECT IDBENHBHDONGTIEN 
                                FROM CHITIETDANGKYKHAM A
                                LEFT JOIN DANGKYKHAM B ON A.IDDANGKYKHAM=B.IDDANGKYKHAM
                                WHERE A.IDCHITIETDANGKYKHAM=" + idchitietdangkykham;
            DataTable dt1 = DataAcess.Connect.GetTable(sqlSelect1);
            if (dt1 == null || dt1.Rows.Count == 0) return "";
            string IdBenhNhanBHDongTien = dt1.Rows[0][0].ToString();
            string sqlSave1 = "EXEC ZHS_MASOVAOVIEN " + IdBenhNhanBHDongTien + "," + (hs_tinhtien.IsCheck(IsNoiTru) == true ? "1" : "0");
            bool OK = DataAcess.Connect.ExecSQL(sqlSave1);
            if (!OK) return "";
            string sqlSelect2 = "SELECT SOVAOVIEN FROM HS_BENHNHANBHDONGTIEN WHERE ID=" + IdBenhNhanBHDongTien;
            DataTable dt2 = DataAcess.Connect.GetTable(sqlSelect2);
            if (dt2 == null || dt2.Rows.Count == 0) return "";
            string SoVaoVien = dt2.Rows[0][0].ToString();
            if (idkhambenh != null && idkhambenh != "")
            {
                string sqlSave2 = "UPDATE KHAMBENH SET ISNOITRU=" + (hs_tinhtien.IsCheck(IsNoiTru) == true ? "1" : "0")
                + ", SOVAOVIEN='" + SoVaoVien + "' WHERE IDKHAMBENH=" + idkhambenh;
                bool OK2 = DataAcess.Connect.ExecSQL(sqlSave2);
            }
            return SoVaoVien;
        }

        public static void XuatThuoc(string idkhambenh)
        {
            if (!Connect.ExecSQL(@"DECLARE @Checkcode AS FLOAT
                                     SET  @Checkcode=DBO.haison_create_key_check()                                   
                                     DECLARE @IDKHAMBENH AS BIGINT                                  
                                     SET @IDKHAMBENH=" + idkhambenh + @"                        
                                     DECLARE @NGAYXUAT AS DATETIMe                                            
                                     ,@LOAIKHAMID AS BIGINT                                        
                                     ,@IDBENHBHDONGTIEN AS BIGINT
                                     ,@IsNoiTru as bit
                                     SELECT @NGAYXUAT=ISNULL(TGXuatVien,NGAYKHAM)
                                     ,@LOAIKHAMID=B.LoaiKhamID
                                     ,@IDBENHBHDONGTIEN=B.IDBENHBHDONGTIEN
                                     ,@IsNoiTru=C.IsNoiTru                   
                                     FROM khambenh  A
                                     INNER JOIN dangkykham B ON A.iddangkykham=B.iddangkykham
                                     INNER JOIN HS_BENHNHANBHDONGTIEN C ON B.IDBENHBHDONGTIEN=C.ID
                                     WHERE IDKHAMBENH=@IDKHAMBENH
                                     INSERT INTO phieuxuatkho(idkho,NGAYTHANG,loaixuat,IDKHAMBENH1,IdBenhNhan,idkho2,IsBcTon)
                                     SELECT DISTINCT idkho=(CASE WHEN ISNULL(D.IdKho_Used,0)<>0 THEN D.IdKho_Used ELSE D.IDKHO END),
                                     NGAYTHANG=ISNULL(B.TGXuatVien,B.NGAYKHAM),
                                     loaixuat=(CASE WHEN ISNULL(D.IdKho_Used,0)<>0 THEN 2 ELSE D.IDKHO END),
                                     IDKHAMBENH1=A.idkhambenh,
                                     IdBenhNhan=B.idbenhnhan,
                                     idkho2=(CASE WHEN ISNULL(D.IdKho_Used,0)<>0 THEN A.IDKHO ELSE NULL END),
                                     IsBcTon=(CASE WHEN ISNULL(D.IdKho_Used,0)<>0 OR D.IDKHO=5 THEN 0 ELSE 1 END)
                                     FROM chitietbenhnhantoathuoc A
                                     INNER JOIN khambenh B ON A.idkhambenh=B.idkhambenh
                                     LEFT JOiN KHOTHUOC D ON A.IDKHO=D.IDKHO
                                     LEFT JOIN PHIEUXUATKHO C ON C.IDKHO=(CASE WHEN ISNULL(D.IdKho_Used,0)<>0 THEN D.IdKho_Used ELSE D.IDKHO END) AND A.IDKHAMBENH=C.IDKHAMBENH1
                                      WHERE ISNULL(A.IDKHO,0)<>0
                                      AND B.idkhambenh=@IDKHAMBENH
                                      AND ISNULL(C.IDPHIEUXUAT,0)=0 
                                      INSERT INTO chitietphieuxuatkho (IDPHIEUXUAT,IDLOAIXUAT_XUAT ,IDKHO_XUAT ,NGAYTHANG_XUAT ,SOLUONG ,idchitietbenhnhantoathuoc,IDKHAMBENH1,IDTHUOC,IDBENHBHDONGTIEN
                                    ,IsBcTon
                                                
                                    ,Checkcode                                                
                                    ,haison_note                                            )
                                    SELECT    distinct IDPHIEUXUAT=C.IDPHIEUXUAT                                                
                                    ,IDLOAIXUAT_XUAT=(CASE WHEN ISNULL(KHOTHUOC.IdKho_Used,0)<>0 THEN 2 ELSE 2 END)                                                
                                    ,IDKHO_XUAT=C.idkho                                                
                                    ,NGAYTHANG_XUAT=C.NGAYTHANG                                               
                                     ,SOLUONG=A.soluongke                                                
                                     ,idchitietbenhnhantoathuoc=A.idchitietbenhnhantoathuoc                                                
                                     ,IDKHAMBENH1=A.idkhambenh                                                
                                     ,A.IDTHUOC                                                
                                     ,IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN                                                
                                     ,IsBcTon=(CASE WHEN ISNULL(KHOTHUOC.IdKho_Used,0)<>0 OR A.IDKHO=5 THEN 0 ELSE 1 END) 
                                     ,@Checkcode                                                
                                     ,haison_note='lehoanlib_xuatthuoc_line_2327'
                                     FROM chitietbenhnhantoathuoc A
                                     INNER JOIN khambenh B ON A.idkhambenh=B.idkhambenh
                                     INNER JOIN THUOC D ON A.IDTHUOC=D.idthuoc                                
                                     INNER JOIN KHOTHUOC KHOTHUOC ON A.IDKHO=KHOTHUOC.IDKHO
                                     LEFT JOIN chitietphieuxuatkho E ON A.idchitietbenhnhantoathuoc=E.idchitietbenhnhantoathuoc
                                     LEFT JOIN PHIEUXUATKHO C ON C.IDKHO=(CASE WHEN ISNULL(KHOTHUOC.IdKho_Used,0)<>0 THEN KHOTHUOC.IdKho_Used ELSE KHOTHUOC.IDKHO END) AND A.IDKHAMBENH=C.IDKHAMBENH1
                                     WHERE A.idkhambenh=@IDKHAMBENH
                                     AND ISNULL(A.IDKHO,0)<>0                                
                                     AND ISNULL(A.IDKHO,0)<>72AND ISNULL(E.IDCHITIETPHIEUXUAT,0)=0
                                    UPDATE chitietphieuxuatkho SET IDPHIEUXUAT=C.IDPHIEUXUAT,IDLOAIXUAT_XUAT=(CASE WHEN ISNULL(F.IdKho_Used,0)<>0 THEN 2 ELSE 2 END)                                    
                                    ,IDKHO_XUAT=(CASE WHEN ISNULL(F.IdKho_Used,0)<>0 THEN F.IdKho_Used ELSE F.IDKHO END),NGAYTHANG_XUAT=(CASE WHEN YC.ISDUYETPHAT=1 AND ISNULL(F.IdKho_Used,0)<>0 THEN YC.NGAYDUYET ELSE  ISNULL(B.TGXuatVien,B.NGAYKHAM) END),SOLUONG=A.soluongke                                    ,VAT=0                                    ,Dongia=D.GIA_MUA,ISBHYT_SAVE_X=(CASE WHEN @LOAIKHAMID=1 AND D.sudungchobh=1 THEN A.ISBHYT_SAVE ELSE 0 END)                                    ,ISBHYT_NHAP=D.sudungchobh,idchitietbenhnhantoathuoc=A.idchitietbenhnhantoathuoc,IDKHAMBENH1=A.idkhambenh,DonGiaBH=(CASE WHEN @LOAIKHAMID=1 AND D.sudungchobh=1 AND A.ISBHYT_SAVE=1 AND ISNULL(A.IsHaoPhi,0)=0 AND  ISNULL(D.IsHaoPhi_Thuoc,0)=0 THEN (CASE WHEN D.GIA_MUA<=D.GIA_THAU THEN D.GIA_MUA ELSE D.GIA_THAU END) ELSE 0 END),DonGiaDV=D.GIA_MUA,ISBHYT=(CASE WHEN @LOAIKHAMID=1 AND D.sudungchobh=1  AND A.ISBHYT_SAVE=1 AND ISNULL(A.IsHaoPhi,0)=0 AND  ISNULL(D.IsHaoPhi_Thuoc,0)=0 THEN 1 ELSE 0 END),ThanhTienBH=(CASE WHEN @LOAIKHAMID=1 AND D.sudungchobh=1  AND A.ISBHYT_SAVE=1 AND ISNULL(A.IsHaoPhi,0)=0 AND  ISNULL(D.IsHaoPhi_Thuoc,0)=0 THEN A.soluongke*(CASE WHEN D.GIA_MUA<=D.GIA_THAU THEN D.GIA_MUA ELSE D.GIA_THAU END) ELSE 0 END),ThanhTienDV=A.soluongke*D.GIA_MUA,IDTHUOC=A.IDTHUOC                                    ,IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN                                    ,IsTinhTien=(CASE WHEN ISNULL(A.IsHaoPhi,0)=1 OR  ISNULL(D.IsHaoPhi_Thuoc,0)=1 then 0 else 1 end)                                    
                                    ,TOP1_IDCHITIETBENHNHANTOATHUOC=(CASE WHEN A.IDKHO<>5 THEN NULL ELSE  (SELECT TOP 1 A0.idchitietbenhnhantoathuoc FROM chitietbenhnhantoathuoc A0 INNER JOIN khambenh B0 ON A0.idkhambenh=B0.idkhambenh INNER JOIN DANGKYKHAM C0 ON B0.IDDANGKYKHAM=C0.IDDANGKYKHAM WHERE C0.IDBENHBHDONGTIEN=@IDBENHBHDONGTIEN AND B0.IDPHONGKHAMBENH=B.IDPHONGKHAMBENH AND A0.IDTHUOC=A.IDTHUOC ORDER BY A0.IDCHITIETBENHNHANTOATHUOC ) END)
                                     ,IdLoaiThuoc=D.LOAITHUOCID                                    
                                     ,IsBcTon=(CASE WHEN A.ISDAXUAT=1 OR E.isbcton=1 OR YC.ISDUYETPHAT=1  THEN 1 ELSE  (CASE WHEN ISNULL(F.IdKho_Used,0)<>0 OR A.IDKHO=5 THEN 0 ELSE 1 END) END)
                                     ,DonGiaDV_TEMP=NULL                                    
                                     ,DonGiaBH_TEMP=NULL                                    
                                     ,Checkcode=@Checkcode                                    
                                     ,haison_note='lehoanlib_xuatthuoc_line_2366'
                                      FROM chitietbenhnhantoathuoc A
                                      INNER JOIN KHOTHUOC F ON A.IDKHO=F.IDKHO
                                      INNER JOIN khambenh B ON A.idkhambenh=B.idkhambenh
                                      INNER JOIN chitietphieuxuatkho E ON A.idchitietbenhnhantoathuoc=E.idchitietbenhnhantoathuoc
                                      INNER JOIN phieuxuatkho C ON  E.IDPHIEUXUAT=C.IDPHIEUXUAT
                                      INNER JOIN THUOC D ON A.IDTHUOC=D.idthuoc                                
                                      LEFT JOIN yc_phieuycxuat YC ON A.IDPHIEUYC_XUAT=YC.IDPHIEUYC
                                      WHERE A.idkhambenh=@IDKHAMBENH
                                      AND ISNULL(A.IDKHO,0)<>0                                        
                                      AND ISNULL(A.IDKHO,0)<>72                           
                                       Update  chitietbenhnhantoathuoc SET SLXuat=A.soluongke                              
                                       FROM chitietbenhnhantoathuoc A
                                       INNER JOIN KHOTHUOC F ON A.IDKHO=F.IDKHO
                                       INNER JOIN khambenh B ON A.idkhambenh=B.idkhambenh
                                       INNER JOIN chitietphieuxuatkho E ON A.idchitietbenhnhantoathuoc=E.idchitietbenhnhantoathuoc
                                       WHERE A.idkhambenh=@IDKHAMBENH
                                       AND ISNULL(A.IDKHO,0)<>0                                        
                                       AND ISNULL(A.IDKHO,0)<>72                                        
                                       AND (CASE WHEN A.ISDAXUAT=1 OR E.isbcton=1  THEN 1 ELSE  (CASE WHEN ISNULL(F.IdKho_Used,0)<>0 OR A.IDKHO=5 THEN 0 ELSE 1 END) END)=1"))
                return;
            DataTable table1 = Connect.GetTable("SELECT DISTINCT IDKHO=(CASE WHEN ISNULL(B.IDKHO_USED,0)<>0 THEN B.IDKHO_USED ELSE B.IDKHO END),A.IDTHUOC FROM CHITIETBENHNHANTOATHUOC A INNER JOIN KHOTHUOC B ON A.IDKHO=B.IDKHO WHERE IDKHAMBENH=" + idkhambenh + " and ISNULL(A.IDKHO,0) NOT IN (0,-1,72)");
            if (table1 == null || table1.Rows.Count <= 0)
                return;
            table1.DefaultView.Sort = "IDKHO";
            DataTable table2 = table1.DefaultView.ToTable();
            List<string> stringList1 = new List<string>();
            List<string> stringList2 = new List<string>();
            DataView dataView = new DataView(table2);
            for (int index1 = 0; index1 < table2.Rows.Count; ++index1)
            {
                if (stringList1.IndexOf(table2.Rows[index1]["idkho"].ToString()) == -1)
                {
                    stringList1.Add(table2.Rows[index1]["idkho"].ToString());
                    dataView.RowFilter = "idkho=" + table2.Rows[index1]["idkho"].ToString();
                    string str = "";
                    for (int index2 = 0; index2 < dataView.Count; ++index2)
                        str = str + dataView[index2]["idthuoc"].ToString() + ",";
                    if (str != null && str != "")
                        str = str.Remove(str.Length - 1, 1);
                    stringList2.Add(str);
                }
            }
            for (int index = 0; index < stringList1.Count; ++index)
                hs_tinhtien.Caculate_SLTON(stringList1[index], stringList2[index]);
        }

        public static void Caculate_SLTON(string idkho)
        {
            hs_tinhtien.Caculate_SLTON(idkho, (string)null);
        }

        public static void Caculate_SLTON_By_PhieuYcXuat(string idkho, string IdPhieuYC)
        {
            DataTable table = Connect.GetTable("\r\n                             DECLARE @S AS NVARCHAR(MAX)\r\n                             SET @S=''\r\n                             SELECT @S=                      \r\n                                           STUFF((                      \r\n                                                  SELECT distinct ','+ CONVERT(VARCHAR,A.IDTHUOC)                     \r\n                                                 FROM YC_PHIEUYCXUATCHITIET A\r\n\t\t\t\t\t\t                            WHERE A.IDPHIEUYC=" + IdPhieuYC + "                   \r\n                                                FOR XML PATH('')                      \r\n                                  ), 1, 1, '' ) \r\n                        SELECT ABC=@S\r\n                        ");
            if (table == null || table.Rows.Count <= 0)
                return;
            string arrIdThuoc = table.Rows[0][0].ToString();
            if (arrIdThuoc[0] == ',')
                arrIdThuoc = arrIdThuoc.Remove(0, 1);
            if (arrIdThuoc[arrIdThuoc.Length - 1] == ',')
                arrIdThuoc = arrIdThuoc.Remove(arrIdThuoc.Length - 1, 1);
            hs_tinhtien.Caculate_SLTON(idkho, arrIdThuoc);
        }

        public static void Caculate_SLTON_By_PhieuYcTra(string idkho, string IdPhieuYC)
        {
            DataTable table = Connect.GetTable("\r\n                             DECLARE @S AS NVARCHAR(MAX)\r\n                             SET @S=''\r\n                             SELECT @S=                      \r\n                                           STUFF((                      \r\n                                                  SELECT distinct ','+ CONVERT(VARCHAR,A.IDTHUOC)                     \r\n                                                 FROM YC_PHIEUYCTRACHITIET A\r\n\t\t\t\t\t\t                            WHERE A.IDPHIEUYC=" + IdPhieuYC + "                   \r\n                                                FOR XML PATH('')                      \r\n                                  ), 1, 1, '' ) \r\n                        SELECT ABC=@S\r\n                        ");
            if (table == null || table.Rows.Count <= 0)
                return;
            string arrIdThuoc = table.Rows[0][0].ToString();
            if (arrIdThuoc[0] == ',')
                arrIdThuoc = arrIdThuoc.Remove(0, 1);
            if (arrIdThuoc[arrIdThuoc.Length - 1] == ',')
                arrIdThuoc = arrIdThuoc.Remove(arrIdThuoc.Length - 1, 1);
            hs_tinhtien.Caculate_SLTON(idkho, arrIdThuoc);
        }

        public static void Caculate_SLTON_By_PhieuNhap(string idkho, string IdPhieuNhap)
        {
            DataTable table = Connect.GetTable("\r\n                             DECLARE @S AS NVARCHAR(MAX)\r\n                             SET @S=''\r\n                             SELECT @S=                      \r\n                                           STUFF((                      \r\n                                                  SELECT distinct ','+ CONVERT(VARCHAR,A.IDTHUOC)                     \r\n                                                 FROM CHITIETPHIEUNHAPKHO A\r\n\t\t\t\t\t\t                            WHERE A.IDPHIEUNHAP=" + IdPhieuNhap + "                   \r\n                                                FOR XML PATH('')                      \r\n                                  ), 1, 1, '' ) \r\n                        SELECT ABC=@S\r\n                        ");
            if (table == null || table.Rows.Count <= 0)
                return;
            string arrIdThuoc = table.Rows[0][0].ToString();
            if (arrIdThuoc[0] == ',')
                arrIdThuoc = arrIdThuoc.Remove(0, 1);
            if (arrIdThuoc[arrIdThuoc.Length - 1] == ',')
                arrIdThuoc = arrIdThuoc.Remove(arrIdThuoc.Length - 1, 1);
            hs_tinhtien.Caculate_SLTON(idkho, arrIdThuoc);
        }

        public static void Caculate_SLTON_By_PhieuXuat(string idkho, string IdPhieuXuat)
        {
            DataTable table = Connect.GetTable("\r\n                             DECLARE @S AS NVARCHAR(MAX)\r\n                             SET @S=''\r\n                             SELECT @S=                      \r\n                                           STUFF((                      \r\n                                                  SELECT distinct ','+ CONVERT(VARCHAR,A.IDTHUOC)                     \r\n                                                 FROM CHITIETPHIEUXUATKHO A\r\n\t\t\t\t\t\t                            WHERE A.IdPhieuXuat=" + IdPhieuXuat + "                   \r\n                                                FOR XML PATH('')                      \r\n                                  ), 1, 1, '' ) \r\n                        SELECT ABC=@S\r\n                        ");
            if (table == null || table.Rows.Count <= 0)
                return;
            string arrIdThuoc = table.Rows[0][0].ToString();
            if (arrIdThuoc[0] == ',')
                arrIdThuoc = arrIdThuoc.Remove(0, 1);
            if (arrIdThuoc[arrIdThuoc.Length - 1] == ',')
                arrIdThuoc = arrIdThuoc.Remove(arrIdThuoc.Length - 1, 1);
            hs_tinhtien.Caculate_SLTON(idkho, arrIdThuoc);
        }

        public static void Caculate_SLTON(string idkho, string arrIdThuoc)
        {
            string strCommandText = "\r\n                    DECLARE @IDKHO AS BIGINT\r\n                    SET @IDKHO=" + idkho + " \r\n                    update THUOC SET  SLTON_" + idkho + "=ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUNHAPKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_NHAP=@IDKHO  AND ISNULL(A0.IsBcTon,1)=1),0)-ISNULL((SELECT SUM(SOLUONG) FROM CHITIETPHIEUXUATKHO A0 WHERE A0.IDTHUOC=B.IDTHUOC AND A0.IDKHO_XUAT=@IDKHO  AND ISNULL(A0.IsBcTon,1)=1),0)      FROM THUOC B     WHERE IsNull(B.IsNgungSD,0)=0        AND IsNull(B.IsThuocBV,1)=1 \r\n                    ";
            if (arrIdThuoc != null && arrIdThuoc != "")
                strCommandText = strCommandText + "\r\n                        AND IDTHUOC IN(" + arrIdThuoc + ")";
            Connect.ExecSQL(strCommandText);
        }


    }
}
