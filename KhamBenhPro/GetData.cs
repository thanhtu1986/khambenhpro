using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KhamBenhPro
{
   public class GetData
    {
        #region Load Cận lâm sàng BS chỉ định
        public static string dt_load_CLS(string idkhambenh)
        {
            string sql = @" select 
                                                                cls.idcanlamsan as idbanggiadichvu
                                                                ,Bg.tendichvu as tendichvu
                                                                ,cls.DonGiaDV as giadichvu
                                                                ,cls.dongiabh as BHTra
                                                                ,cls.IsBHYT as IsSuDungChoBH
																,cls.IsBHYT_Save as bhyt_save
                                                                ,soluong = cls.soluong
                                                                ,cls.GhiChu as ghichu
                                                                ,vp.tungay as fromdate
                                                                ,cls.idkhambenhcanlamsan as IdKBCLS
                                                                ,cls.dathu
                                                                ,cls.idnhominbv
                                ,StatusKQ=DBO.zHS_Status_KetQuaCLS(cls.IDKHAMBENHCANLAMSAN)
                               from khambenhcanlamsan cls
                                left join banggiadichvu bg on cls.idcanlamsan=bg.idbanggiadichvu
                                left join phongkhambenh pkb on bg.idphongkhambenh=pkb.idphongkhambenh
								left join hs_banggiavienphi vp ON vp.IdGiaDichVu = (SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu = Bg.IDBANGGIADICHVU AND BH0.TuNgay <= GETDATE() ORDER BY TuNgay DESC)
                                 where  isnull(cls.dahuy,0)=0 and cls.idkhambenh='" + idkhambenh + "'";
            return sql;
        }
        #endregion

        #region Load CLS hẹn
        public static string dt_load_CLS_hen(string idkhambenh)
        {
            string sql = @" select 
                                                                cls.idcanlamsan as idbanggiadichvu
                                                                ,Bg.tendichvu as tendichvu
                                                                ,cls.DonGiaDV as giadichvu
                                                                ,cls.dongiabh as BHTra
                                                                ,cls.IsBHYT as IsSuDungChoBH
																,cls.IsBHYT_Save as bhyt_save
                                                                ,soluong = cls.soluong
                                                                ,cls.GhiChu as ghichu
                                                                ,vp.tungay as fromdate
                                                                ,cls.idkhambenhcanlamsanhen as IdKBCLS
                                                                ,cls.dathu
                                                                ,cls.idnhominbv
                                ,StatusKQ=DBO.zHS_Status_KetQuaCLS(cls.IDKHAMBENHCANLAMSANhen)
                               from khambenhcanlamsanhen cls
                                left join banggiadichvu bg on cls.idcanlamsan=bg.idbanggiadichvu
                                left join phongkhambenh pkb on bg.idphongkhambenh=pkb.idphongkhambenh
								left join hs_banggiavienphi vp ON vp.IdGiaDichVu = (SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu = Bg.IDBANGGIADICHVU AND BH0.TuNgay <= GETDATE() ORDER BY TuNgay DESC)
                                 where  isnull(cls.dahuy,0)=0 and cls.idkhambenh='" + idkhambenh + "'";
            return sql;
        }

        #endregion


        #region Load toa thuốc bảo hiểm
        public static string dt_Load_Toathuoc(string idkhambenh)
        {
            string sql = @"select STT=row_number() over (order by T.idchitietbenhnhantoathuoc) 
                                       , b.TenLoai
                                        ,t.idthuoc as tenthuoc
                                        ,a.congthuc
                                        ,d.TenDVT
                                        ,t.soluongke
                                        ,c.tencachdung
                                        ,t.ngayuong
                                        ,t.moilanuong
                                        ,e.TenDVT as tendvdung
                                        ,t.issang
                                        ,t.istrua
                                        ,t.ischieu
                                        ,t.istoi
                                        ,t.ghichu
                                        ,a.sudungchobh as isbhyt
                                        ,t.IsBHYT_Save
                                        ,t.idthuoc
                                        ,t.iddvt
                                        ,t.idcachdung
                                        ,t.iddvdung
                                        ,t.IsDaXuat
                                        ,t.idchitietbenhnhantoathuoc
                              , tenkho=(case when t.idkho=-1 then N'Ngoài BV' else  k.tenkho end )
                              ,t.slton
                              ,sldaxuat= ISNULL(T.SLXUAT,0)
                              ,idctYc= NULL
                            from chitietbenhnhantoathuoc T
                                left join thuoc  A on T.idthuoc=A.idthuoc
                                left join Thuoc_LoaiThuoc  B on isnull(a.loaithuocid,1)=B.LoaiThuocID
                                left join Thuoc_CachDung  C on T.idcachdung=C.idcachdung
                                left join Thuoc_DonViTinh  D on T.iddvt=D.Id
                                left join Thuoc_DonViTinh  E on T.iddvdung=E.Id
                                left join category  F on a.cateid=F.cateid
                         left join khothuoc k on k.idkho = T.idkho 
                    where T.idkhambenh='" + idkhambenh + @"' AND T.IDKHO<>72
           UNION ALL   
            select STT=row_number() over (order by T.idchitietbenhnhantoathuoc)
                                        ,b.TenLoai
                                        ,a.tenthuoc
                                        ,a.congthuc
                                        ,d.TenDVT
                                        ,t.soluongke
                                        ,c.tencachdung
                                        ,t.ngayuong
                                        ,t.moilanuong
                                        ,e.TenDVT as tendvdung
                                        ,t.issang
                                        ,t.istrua
                                        ,t.ischieu
                                        ,t.istoi
                                        ,t.ghichu
                                         ,isnull(a.sudungchobh,0) as isbhyt
                                        ,t.IsBHYT_Save
                                        ,t.idthuoc
                                        ,t.iddvt
                                        ,t.idcachdung
                                        ,t.iddvdung
                                        ,isnull(t.IsDaXuat,0)
                                        ,t.idchitietbenhnhantoathuoc
                               ,tenkho=(case when t.idkho=-1 then N'Ngoài BV' else  k.tenkho end )
                              ,t.slton
                              ,sldaxuat= ISNULL(T.SLXUAT,0)
                              ,idctYc= NULL
                            from chitietbenhnhantoathuoc_nhathuoc T
                                left join NHATHUOCDB.DBO.thuoc  A on T.idthuoc=A.idthuoc
                                left join NHATHUOCDB.DBO.Thuoc_LoaiThuoc  B on isnull(a.loaithuocid,1)=B.LoaiThuocID
                                left join BvmdDB.DBO.Thuoc_CachDung  C on T.idcachdung=C.idcachdung
                                left join BvmdDB.DBO.Thuoc_DonViTinh  D on T.iddvt=D.Id
                                left join BvmdDB.DBO.Thuoc_DonViTinh  E on T.iddvdung=E.Id
                                left join NHATHUOCDB.DBO.category  F on a.cateid=F.cateid
                                left join NHATHUOCDB.DBO.khothuoc k on k.idkho = T.idkho 
                    where T.idkhambenh='" + idkhambenh + "' AND T.IDKHO=72";

            return sql;
        }
        #endregion

        #region Load Chẩn đoán sơ bộ
        public static string dt_Load_CDSB(string idkhambenh)
        {
            string sql = @"select
																cdsb.idicd as idicd
																,icd.maicd as maicd
																,cdsb.MoTaCD_edit as MoTa
                                                                ,cdsb.id
                                                                    from chandoansobo cdsb
                                                               left join khambenh kb on kb.idkhambenh = cdsb.idkhambenh
															   left join ChanDoanICD icd on icd.IDICD=cdsb.idicd
                                                               where kb.idkhambenh='" + idkhambenh + "'";
            return sql;
        }
        #endregion

        #region Load Chẩn đoán phối hợp
        public static string Load_CDPH(string idkhambenh)
        {
            string sql = @"
                                select cd.IDICD AS id_ph,cd.MaICD AS maicd_ph,cd.MoTa as MoTa_ph,ph.id
                                from chandoanphoihop ph
                                inner join khambenh kb on kb.idkhambenh=ph.idkhambenh
                                inner join ChanDoanICD cd on cd.idicd=ph.idicd
                                where kb.idkhambenh='" + idkhambenh + "'";
            return sql;
        }
        #endregion

        #region Bệnh nhân chờ khám (đăng ký mới)
        public static string dt_BNChoKham(string idchitietdangkykham)
        {

            string sql = @"SELECT top 1 
			IdChiTietDangKyKham = ct.idchitietdangkykham,
            iddangkykham=ct.iddangkykham,
            PhongID = ct.phongid,IdKhoa = b.idphongkhambenh,
            DichVuKCBID = a.DichVuKCB,
            BN.IDBENHNHAN AS idbenhnhan,
            BN.TENBENHNHAN AS tenbenhnhan,
            BN.mabenhnhan AS mabenhnhan,
            bn.diachi,
            BN.NGAYSINH AS NGAYSINH,
            loaidk = dk.LoaiKhamID,
            GIOITINH=CASE WHEN BN.gioitinh='0' THEN N'NAM' ELSE N'NỮ' END,
            SH.MACH ,
            SH.NHIETDO ,
            SH.HUYETAP1 ,
            SH.HUYETAP2 ,
            SH.NHIPTHO ,
            SH.CANNANG ,
            SH.CHIEUCAO,
            SH.BMI,
            TENKHOA=c.tenphongkhambenh,
            TENPHONG=ISNULL(REPLACE( REPLACE( REPLACE(a.MaSo,'-BH',''),'-DV',''),'DV','') + '-','')+ REPLACE(a.TenPhong,N'PHÒNG ',''),
            sobhyt=upper(BHYT.SOBHYT),
            ngaybatdau= CONVERT(NVARCHAR(20), BHYT.NGAYBATDAU,103),
            ngayhethan_hiden=CONVERT(NVARCHAR(20),BHYT.NGAYHETHAN,103),
            ngayhethan=CONVERT(NVARCHAR(20), BHYT.NGAYHETHAN,103),
            TENNOIDANGKY=KCB.TENNOIDANGKY,
            IsDungTuyen=BHYT.IsDungTuyen
            ,NgayDangKy=convert(varchar, dk.ngaydangky, 108) +' , '+ convert(varchar, dk.ngaydangky, 103)
            --,idbacsi2=0
			--,idPhongChuyenDen=0
			-- ,IdkhoaChuyen=0
          --   ,idkhambenh=0
          --   ,idbacsi=0
               ,songayratoa=0
                ,ngayhentaikham=0
            ,IDBENHNHANBHDONGTIEN=DK.IDBENHBHDONGTIEN
			,TuoiBenhNhan=dbo.kb_GetTuoi(bn.ngaysinh)
            FROM 
			chitietdangkykham ct
			left join dangkykham dk on ct.iddangkykham = dk.iddangkykham 
			left join BENHNHAN BN  on dk.idbenhnhan = bn.idbenhnhan
			left join kb_phong a on a.id = ct.phongid
			left join banggiadichvu b on a.dichvukcb = b.idbanggiadichvu
			left join phongkhambenh c on c.idphongkhambenh = b.idphongkhambenh
            left join SINHHIEU SH on SH.IDDANGKYKHAM=dk.iddangkykham
             LEFT JOIN BENHNHAN_BHYT BHYT ON DK.IDBENHNHAN_BH=BHYT.IDBENHNHAN_BH
            LEFT JOIN KB_NOIDANGKYKB KCB ON BHYT.IDNOIDANGKYBH=KCB.IDNOIDANGKY
               where ct.idchitietdangkykham ='" + idchitietdangkykham + "'";
            return sql;

        }
        #endregion

        #region Bệnh nhân chờ khám (chuyển phòng)
        public static string dt_BNKhamCP(string idkhambenh)
        {
            string sql = @"SELECT top 1 
            IdChiTietDangKyKham =ISNULL(G.IDCHITIETDANGKYKHAM,CT.IDCHITIETDANGKYKHAM),
            iddangkykham=dk.iddangkykham,
            PhongID = a.id,
            IdKhoa = K2.idphongkhambenh,
            DichVuKCBID = a.DichVuKCB,
            BN.IDBENHNHAN AS idbenhnhan,
            BN.TENBENHNHAN AS tenbenhnhan,
            BN.mabenhnhan AS mabenhnhan,
            bn.diachi,
            BN.NGAYSINH AS NGAYSINH,
            loaidk = dk.LoaiKhamID,
            GIOITINH=CASE WHEN BN.gioitinh='0' THEN N'NAM' ELSE N'NỮ' END,
            SH.MACH ,
            SH.NHIETDO ,
            SH.HUYETAP1 ,
            SH.HUYETAP2 ,
            SH.NHIPTHO ,
            SH.CANNANG ,
            SH.CHIEUCAO,
            SH.BMI,
            TENKHOA=K2.TENPHONGKHAMBENH,
            kb.IdChuyenPK,
            TENPHONG=ISNULL(REPLACE( REPLACE( REPLACE(P2.MASO,'-BH',''),'-DV',''),'DV','') + '-','')+ REPLACE(P2.TENPHONG,N'PHÒNG ',''),
            KB.IDKHAMBENH,
            ischovekt=0,
            ischuyenvien=0,
            idbenhvienchuyen=NULL,
            iskhongkham=0,
            KB.idbacsi,
            mkv_idbenhvienchuyen = NULL,
            mkv_idbacsi =bs.tenbacsi,
            kb.ketluan,
            idchandoan=kb.ketluan,
            mkv_idchandoan=cd.maicd,
            mkv_mota =isnull(kb.MoTaCD_edit,cd.mota),
            kb.chandoanbandau,    
            kb.idchandoantuyenduoi,
            mkv_idchandoantuyenduoi=cdtd.maicd,
            mkv_mota_idchandoantuyenduoi=isnull(kb.chandoantuyenduoi,cdtd.mota),
            sobhyt=upper(BHYT.SOBHYT),
            ngaybatdau= CONVERT(NVARCHAR(20), BHYT.NGAYBATDAU,103),
            ngayhethan_hiden= CONVERT(NVARCHAR(20), BHYT.NGAYHETHAN,103),
            ngayhethan= CONVERT(NVARCHAR(20),  BHYT.NGAYHETHAN,103),
            TENNOIDANGKY=KCB.TENNOIDANGKY,
            IsDungTuyen=BHYT.IsDungTuyen,
            SOVAOVIEN1=DBO.zHS_GetSoVaoVienFromKB(KB.IDKHAMBENH)
            ,NgayDangKy=convert(varchar, dk.ngaydangky, 108) +' , '+ convert(varchar, dk.ngaydangky, 103)
           ,KB.isNoiTru
           ,isNgoaiTru=( CASE WHEN ISNULL( KB.isNoiTru,0)=0 THEN 1 ELSE 0 END)
            ,IDBENHNHANBHDONGTIEN=DK.IDBENHBHDONGTIEN
			,TuoiBenhNhan=dbo.kb_GetTuoi(bn.ngaysinh)
             ,IsXuatVien=ISNULL(KB.ISXUATVIEN,0)
            ,kb.idbacsi2
            ,kb.idPhongChuyenDen
            ,kb.IdkhoaChuyen
            ,TGXuatVien=convert(nvarchar(20),TGXUATVIEN,103)
            ,RIGHT( CONVERT(VARCHAR(13),tgxuatvien,120),2) as gioravien,RIGHT( CONVERT(VARCHAR(16),tgxuatvien,120),2) as phutravien
            ,songayratoa=0
            ,ngayhentaikham=0
            FROM KHAMBENH KB
			LEFT JOIN chitietdangkykham ct ON KB.idchitietdangkykham=ct.idchitietdangkykham
			left join dangkykham dk on ct.iddangkykham = dk.iddangkykham
			left join BENHNHAN BN  on KB.idbenhnhan=bn.idbenhnhan
			left join kb_phong a on a.id = kb.idchuyenpk
			left join banggiadichvu b on ct.idbanggiadichvu = b.idbanggiadichvu
			left join phongkhambenh c on c.idphongkhambenh = b.idphongkhambenh
            left join SINHHIEU SH on SH.IDDANGKYKHAM=dk.iddangkykham
            LEFT JOIN KB_PHONG P2 ON KB.IdChuyenPK=P2.ID
            LEFT JOIN PHONGKHAMBENH K2 ON KB.IdkhoaChuyen=K2.IDPHONGKHAMBENH
            LEFT JOIN DANGKYKHAM F ON KB.IDKHAMBENH=F.IDKHAMBENH_CHUYEN
            LEFT JOIN CHITIETDANGKYKHAM G ON F.IDDANGKYKHAM=G.IDDANGKYKHAM
            LEFT JOIN BENHNHAN_BHYT BHYT ON DK.IDBENHNHAN_BH=BHYT.IDBENHNHAN_BH
            LEFT JOIN KB_NOIDANGKYKB KCB ON BHYT.IDNOIDANGKYBH=KCB.IDNOIDANGKY
            left join bacsi bs on kb.idbacsi=bs.idbacsi
            left join chandoanicd cd on kb.ketluan=cd.idicd
            left join chandoanicd cdtd on kb.idchandoantuyenduoi=cdtd.idicd
            WHERE kb.idkhambenh ='" + idkhambenh + "'";
            return sql;
        }
        #endregion

        #region Bệnh nhân chờ cls hoặc đã khám
        public static string dt_BNDaKham2(string idkhambenh)
        {
            string sql = @"SELECT  
            IdKhoa = isnull(kb.idkhoa,3),IdChiTietDangKyKham = kb.idchitietdangkykham,
            iddangkykham=kb.iddangkykham,
            PhongID = kb.phongid,TGXuatVien=convert(nvarchar(20),TGXUATVIEN,103),idchandoan=kb.ketluan,
            DichVuKCBID = kb.DichVuKCBID,
            kb.IdChuyenPK,
			kb.idbacsi,kb.trieuchung,kb.benhsu,
            kb.tiensu,
            BN.IDBENHNHAN AS idbenhnhan,
            RIGHT( CONVERT(VARCHAR(13),tgxuatvien,120),2) as gioravien,RIGHT( CONVERT(VARCHAR(16),tgxuatvien,120),2) as phutravien,
            BN.TENBENHNHAN AS tenbenhnhan,
            BN.mabenhnhan AS mabenhnhan,
            bn.diachi,
            BN.NGAYSINH AS NGAYSINH,
            ngaykham = isnull(kb.ngaykham,getdate()),
            mkv_idchandoan=cd.maicd,
            mkv_mota =isnull(kb.MoTaCD_edit,cd.mota),
            mkv_IdChuyenPK =ISNULL(REPLACE( REPLACE( REPLACE(phong_chuyen.MaSo,'-BH',''),'-DV',''),'DV','') + '-','')+ REPLACE(phong_chuyen.TenPhong,N'PHÒNG ',''),
            mkv_idDVPhongChuyenDen =chuyenkhoa_Chuyen.TenDichVu,
            mkv_IdkhoaChuyen =Khoa_Chuyen.TenPhongKhamBenh
            ,IdkhoaChuyen
            ,idDVPhongChuyenDen
            ,IsXuatVien=ISNULL(KB.ISXUATVIEN,HS.ISXUATVIEN)
            ,GIOITINH=CASE WHEN BN.gioitinh='0' THEN N'NAM' ELSE N'NỮ' END,
            bs.tenbacsi as mkv_idbacsi,
            SH.MACH ,
            SH.NHIETDO ,
            SH.HUYETAP1 ,
            SH.HUYETAP2 ,
            SH.NHIPTHO ,
            SH.CANNANG ,
            SH.CHIEUCAO,
            SH.BMI,
            IsChuyenPhongCoPhi,
            TENKHOA=K2.TENPHONGKHAMBENH,
            TENPHONG=ISNULL(REPLACE( REPLACE( REPLACE(P2.MaSo,'-BH',''),'-DV',''),'DV','') + '-','')+ REPLACE(P2.TenPhong,N'PHÒNG ',''),
            songayratoa=KB.songayratoa,KB.ngayhentaikham,
            loidan=KB.dando,
            KB.ischovekt,
            KB.ischuyenvien,
            KB.idbenhvienchuyen,
            KB.iskhongkham,
            KB.idbacsi2,
            mkv_idbenhvienchuyen =bv.tenbenhvien,
            mkv_idbacsi2 =bs2.tenbacsi,
            kb.IsBSMoiKham,
            kb.chandoanbandau,
            kb.idchandoantuyenduoi,
            mkv_idchandoantuyenduoi=cdtd.maicd,
            mkv_mota_idchandoantuyenduoi=isnull(kb.chandoantuyenduoi,cdtd.mota),
            loaidk=DK.LoaiKhamID,
            sobhyt=upper(BHYT.SOBHYT),
            ngaybatdau=convert(varchar(10),BHYT.NGAYBATDAU,103)+' '+convert(varchar(5),BHYT.NGAYBATDAU,108),
            ngayhethan_hiden=convert(varchar(10),BHYT.NGAYHETHAN,103)+' '+convert(varchar(5),BHYT.NGAYHETHAN,108),
            ngayhethan=convert(varchar(10),BHYT.NGAYHETHAN,103)+' '+convert(varchar(5),BHYT.NGAYHETHAN,108),
            TENNOIDANGKY=KCB.TENNOIDANGKY,
            IsDungTuyen=BHYT.IsDungTuyen,
            SOVAOVIEN1=HS.SOVAOVIEN
           ,NgayDangKy=convert(varchar, dk.ngaydangky, 108) +' , '+ convert(varchar, dk.ngaydangky, 103)
            ,ngaykham_gio=LEFT( convert(varchar, KB.NGAYKHAM, 108),2)
            ,ngaykham_phut=RIGHT( LEFT(convert(varchar, KB.NGAYKHAM, 108),5),2)
           ,ghichu=kb.ghichu
           ,HS.isNoiTru
           ,isNgoaiTru=( CASE WHEN ISNULL( HS.isNoiTru,0)=0 THEN 1 ELSE 0 END)
           ,SoTTChuyenP=KB.SoTTChuyenP
            ,IDBENHNHANBHDONGTIEN=DK.IDBENHBHDONGTIEN
            ,KeyOff=(CASE WHEN ISNULL( HS.KeyOff,0)=1 THEN '1' ELSE '0' END)
			,TuoiBenhNhan=dbo.kb_GetTuoi(bn.ngaysinh)
             ,kb.idbacsi2
			,kb.idPhongChuyenDen
			 ,kb.IdkhoaChuyen
             ,KB.idkhambenh
              ,KB.ketluan
            FROM KHAMBENH KB
            LEFT JOIN BENHNHAN BN  ON KB.IDBENHNHAN=BN.IDBENHNHAN
			left join chitietdangkykham ct on ct.idchitietdangkykham = kb.idchitietdangkykham
			left join dangkykham dk on dk.iddangkykham = ct.iddangkykham
            left join bacsi bs on bs.idbacsi = kb.idbacsi
            left join kb_phong phong_chuyen on phong_chuyen.id = kb.idchuyenpk
            left join BANGGIADICHVU ChuyenKhoa_chuyen on ChuyenKhoa_chuyen.idbanggiadichvu = phong_chuyen.DichVuKCB
            left join PHONGKHAMBENH Khoa_chuyen on Khoa_chuyen.idphongkhambenh = kb.idkhoachuyen
            left join SINHHIEU SH on SH.IDDANGKYKHAM=dk.iddangkykham
            LEFT JOIN PHONGKHAMBENH K2 ON KB.IDKHOA=K2.IDPHONGKHAMBENH
            LEFT JOIN KB_PHONG P2 ON KB.PHONGID=P2.ID
            LEFT JOIN BENHNHAN_BHYT BHYT ON DK.IDBENHNHAN_BH=BHYT.IDBENHNHAN_BH
            LEFT JOIN KB_NOIDANGKYKB KCB ON BHYT.IDNOIDANGKYBH=KCB.IDNOIDANGKY
            left join benhvien bv on bv.idbenhvien = kb.idbenhvienchuyen
            left join chandoanicd cd on cd.idicd=kb.ketluan
            left join chandoanicd cdtd on cdtd.idicd=kb.idchandoantuyenduoi
            left join bacsi bs2 on kb.idbacsi2=bs2.idbacsi
            LEFT JOIN HS_BENHNHANBHDONGTIEN HS ON dk.IDBENHBHDONGTIEN=HS.ID
            WHERE kb.idkhambenh='" + idkhambenh + "'";
            return sql;
        }
        #endregion

        #region Load Chẩn đoán ICD10
        public static string LoadICD10()
        {
            string sql = "select IDICD,MaICD,MoTa from ChanDoanICD";
            return sql;
        }

        #endregion
    }
}
