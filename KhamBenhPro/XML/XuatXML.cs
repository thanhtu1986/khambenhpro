using Suport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KhamBenhPro.XML
{
    public partial class XuatXML : Form
    {
        public XuatXML()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.ShowDialog();
            this.txtPath.Text = dlg.SelectedPath;
            Properties.Settings.Default.XMLPath = this.txtPath.Text;
            Properties.Settings.Default.Save();
        }

        private void XuatXML_Load(object sender, EventArgs e)
        {
            this.txtPath.Text = Properties.Settings.Default.XMLPath;
        }

        private void btnKetXuat_Click(object sender, EventArgs e)
        {
            if (this.txtPath.Text.Trim() == "")
                return;
            this.txtErro.Text = "";
            this.txtXN.Text = "";
            this.SaveToFile();
        }
        void SaveToFile()
        {
            string path = this.txtPath.Text.Trim();
            string NumberRow = "";
            this.SaveToPath(path, ref NumberRow, true);
            int num = (int)MessageBox.Show(NumberRow.ToString() + " dòng thành công");
        }

        private void SaveToPath(string path, ref string NumberRow, bool IsSaveToDataBase)
        {
            bool flag1 = this.IsBase64.Checked;
            string str1 = DateTime.Now.ToString("yyMMddHHmmss");
            DataView dataView1 = new DataView(this.dtSource_B1());
            dataView1.RowFilter = "IsShow='1'";
            DataTable table1 = dataView1.ToTable();
            dataView1.RowFilter = "IsShow='0'";
            DataTable table2 = dataView1.ToTable();
            DateTime now;
            for (int Pos = 0; Pos < table2.Rows.Count; ++Pos)
            {
                int index = hs_tinhtien.int_Search(table1, "MA_LK=" + table2.Rows[Pos]["MA_LK"].ToString());
                if (index == -1)
                {
                    hsTool.dt_ImportRow(table2, Pos, table1);
                }
                else
                {
                    table1.Rows[index]["MA_THE"] = (object)(table2.Rows[Pos]["MA_THE"].ToString() + ";" + table1.Rows[index]["MA_THE"].ToString());
                    table1.Rows[index]["ma_dkbd"] = (object)(table2.Rows[Pos]["ma_dkbd"].ToString() + ";" + table1.Rows[index]["ma_dkbd"].ToString());
                    table1.Rows[index]["NGAY_VAO"] = table2.Rows[Pos]["NGAY_VAO"];
                    table1.Rows[index]["GT_THE_TU"] = (object)(table2.Rows[Pos]["GT_THE_TU"].ToString() + ";" + table1.Rows[index]["GT_THE_TU"].ToString());
                    table1.Rows[index]["GT_THE_DEN"] = (object)(table2.Rows[Pos]["GT_THE_DEN"].ToString() + ";" + table1.Rows[index]["GT_THE_DEN"].ToString());
                    now = DateTime.Parse(table2.Rows[Pos]["NGAYTINHBH"].ToString());
                    now.ToString("yyyy.MM/dd HH:mm:ss");
                    now = DateTime.Parse(table1.Rows[index]["NGAYTINHBH_THUC"].ToString());
                    now.ToString("yyyy.MM/dd HH:mm:ss");
                }
            }
            DataTable dataTable1 = table1;
            DataTable table3 = this.dtSource_B2();
            DataTable table4 = this.dtSource_B3();
            DataTable dtDes = this.chbHaveXML45.Checked ? this.dtSource_B4() : (DataTable)null;
            DataTable dataTable2 = (DataTable)null;
            if (this.chbHaveXML45.Checked && hsTool.s_ReadFile("NoKQXN.txt") != "1")
                dataTable2 = this.dtXetNghiem();
            if (dataTable2 != null && dataTable2.Rows.Count > 0 && this.chbHaveXML45.Checked)
            {
                this.txtXN.Text = "Số dòng KQXQ:" + dataTable2.Rows.Count.ToString() + "\r\n";
                hsTool.dt_Copy(dataTable2, ref dtDes);
            }
            DataTable table5 = this.chbHaveXML45.Checked ? this.dtSource_B5() : (DataTable)null;
            string str2 = "";
            DataAcess.Connect.ExecSQL("DELETE TEMP_XML1\r\n                                                    DELETE TEMP_XML2\r\n                                                    DELETE TEMP_XML3\r\n                                                ");
            for (int index1 = 0; index1 < dataTable1.Rows.Count; ++index1)
            {
                str2 = str2 + dataTable1.Rows[index1]["MA_LK"].ToString() + ",";
                DataTable table6 = new DataView(table3)
                {
                    RowFilter = ("MA_LK='" + dataTable1.Rows[index1]["MA_LK"].ToString() + "' AND THANH_TIEN>0 AND DON_GIA>0")
                }.ToTable();
                DataTable table7 = new DataView(table4)
                {
                    RowFilter = ("MA_LK='" + dataTable1.Rows[index1]["MA_LK"].ToString() + "' AND THANH_TIEN>0 AND DON_GIA>0")
                }.ToTable();
                table7.DefaultView.Sort = "loaicp  ,NGAY_YL ";
                DataTable table8 = table7.DefaultView.ToTable();
                table8.Columns.Remove("loaicp");
                DataView dataView2 = this.chbHaveXML45.Checked ? new DataView(dtDes) : (DataView)null;
                if (dataView2 != null)
                    dataView2.RowFilter = "MA_LK='" + dataTable1.Rows[index1]["MA_LK"].ToString() + "'";
                DataView dataView3 = this.chbHaveXML45.Checked ? new DataView(table5) : (DataView)null;
                if (dataView3 != null)
                    dataView3.RowFilter = "MA_LK='" + dataTable1.Rows[index1]["MA_LK"].ToString() + "'";
                DataTable dataTable3 = this.chbHaveXML45.Checked ? dataView2.ToTable() : (DataTable)null;
                DataTable table9 = dataView3.ToTable();
                object obj1 = table6.Compute("SUM(T_BHTT)", "");
                object obj2 = obj1 != null && !(obj1.ToString() == "") ? (object)Math.Round(double.Parse(obj1.ToString()), 2) : (object)"0";
                object obj3 = table6.Compute("SUM(T_BNCCT)", "");
                object obj4 = obj3 != null && !(obj3.ToString() == "") ? (object)Math.Round(double.Parse(obj3.ToString()), 2) : (object)"0";
                object obj5 = table6.Compute("SUM(T_BNTT)", "");
                object obj6 = obj5 != null && !(obj5.ToString() == "") ? (object)Math.Round(double.Parse(obj5.ToString()), 2) : (object)"0";
                object obj7 = (object)Math.Round(double.Parse(obj2.ToString()) + double.Parse(obj4.ToString()) + double.Parse(obj6.ToString()), 2);
                object obj8 = table8.Compute("SUM(T_BHTT)", "");
                object obj9 = obj8 != null && !(obj8.ToString() == "") ? (object)Math.Round(double.Parse(obj8.ToString()), 2) : (object)"0";
                object obj10 = table8.Compute("SUM(T_BNCCT)", "");
                object obj11 = obj10 != null && !(obj10.ToString() == "") ? (object)Math.Round(double.Parse(obj10.ToString()), 2) : (object)"0";
                object obj12 = table8.Compute("SUM(T_BNTT)", "");
                object obj13 = obj12 != null && !(obj12.ToString() == "") ? (object)Math.Round(double.Parse(obj12.ToString()), 2) : (object)"0";
                object obj14 = (object)Math.Round(double.Parse(obj9.ToString()) + double.Parse(obj11.ToString()), 2);
                object obj15 = table8.Compute("SUM(THANH_TIEN)", "TEN_VAT_TU<>''");
                if (obj15 == null || obj15.ToString() == "")
                    obj15 = (object)"0";
                dataTable1.Rows[index1]["T_THUOC"] = (object)double.Parse(obj7.ToString());
                dataTable1.Rows[index1]["T_VTYT"] = (object)double.Parse(obj15.ToString());
                dataTable1.Rows[index1]["T_TONGCHI"] = (object)(double.Parse(obj7.ToString()) + double.Parse(obj14.ToString()));
                dataTable1.Rows[index1]["T_BHTT"] = (object)Math.Round(double.Parse(obj2.ToString()) + double.Parse(obj9.ToString()), 2);
                dataTable1.Rows[index1]["T_BNCCT"] = (object)Math.Round(double.Parse(obj4.ToString()) + double.Parse(obj11.ToString()), 2);
                dataTable1.Rows[index1]["T_BNTT"] = (object)Math.Round(double.Parse(obj6.ToString()) + double.Parse(obj13.ToString()), 2);
                string str3 = "";
                List<string> stringList = new List<string>();
                for (int index2 = 0; index2 < table6.Rows.Count; ++index2)
                {
                    if (stringList.IndexOf(table6.Rows[index2]["MA_KHOA"].ToString()) == -1)
                    {
                        stringList.Add(table6.Rows[index2]["MA_KHOA"].ToString());
                        str3 = str3 + table6.Rows[index2]["MA_KHOA"].ToString() + ";";
                    }
                }
                for (int index2 = 0; index2 < table8.Rows.Count; ++index2)
                {
                    if (stringList.IndexOf(table8.Rows[index2]["MA_KHOA"].ToString()) == -1)
                    {
                        stringList.Add(table8.Rows[index2]["MA_KHOA"].ToString());
                        str3 = str3 + table8.Rows[index2]["MA_KHOA"].ToString() + ";";
                    }
                }
                if (str3 != "")
                    str3.Remove(str3.Length - 1, 1);
                if (dataTable1.Rows[index1]["MA_LOAI_KCB"].ToString() == "1")
                    dataTable1.Rows[index1]["SO_NGAY_DTRI"] = (object)"0";
                this.txtErro.Text = "đang kết xuất dòng thứ:" + (index1 + 1).ToString() + "/" + dataTable1.Rows.Count.ToString();
                if (hsTool.int_Search(dataTable2, "MA_LK='" + dataTable1.Rows[index1]["MA_LK"].ToString() + "'") != -1)
                {
                    TextBox txtXn = this.txtXN;
                    txtXn.Text = txtXn.Text + dataTable1.Rows[index1]["MA_LK"].ToString() + ";";
                }
                hsTool.Alert(this.txtErro.Text, "Thông báo", true);
                string str4 = dataTable1.Rows[index1]["MA_LK"].ToString() + ".XML";
                string str5 = "<?xml version=\"1.0\" encoding=\"utf-8\"?><GIAMDINHHS xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">  <THONGTINDONVI>    <MACSKCB>83041</MACSKCB>  </THONGTINDONVI>  <THONGTINHOSO>    <NGAYLAP>";
                now = DateTime.Now;
                string str6 = now.ToString("yyyyMMdd");
                string str7 = "</NGAYLAP>    <SOLUONGHOSO>1</SOLUONGHOSO>    <DANHSACHHOSO>\t\t<HOSO>";
                string str8 = str5 + str6 + str7 + "\t\t\t<FILEHOSO>\t\t\t\t<LOAIHOSO>XML1</LOAIHOSO>\t\t\t\t<NOIDUNGFILE>";
                string plainText1 = "" + "<TONG_HOP>\r\n                                    <MA_LK>" + dataTable1.Rows[index1]["MA_LK"].ToString() + "</MA_LK>\r\n                                    <STT>" + dataTable1.Rows[index1]["STT"].ToString() + "</STT>\r\n                                    <MA_BN>" + dataTable1.Rows[index1]["MA_BN"].ToString() + "</MA_BN>\r\n                                    <HO_TEN><![CDATA[" + dataTable1.Rows[index1]["HO_TEN"].ToString() + "]]></HO_TEN>\r\n                                    <NGAY_SINH>" + dataTable1.Rows[index1]["NGAY_SINH"].ToString() + "</NGAY_SINH>\r\n                                    <GIOI_TINH>" + dataTable1.Rows[index1]["GIOI_TINH"].ToString() + "</GIOI_TINH>\r\n                                    <DIA_CHI><![CDATA[" + dataTable1.Rows[index1]["DIA_CHI"].ToString() + "]]></DIA_CHI>\r\n                                    <MA_THE>" + dataTable1.Rows[index1]["MA_THE"].ToString() + "</MA_THE>\r\n                                    <MA_DKBD>" + dataTable1.Rows[index1]["MA_DKBD"].ToString() + "</MA_DKBD>\r\n                                    <GT_THE_TU>" + dataTable1.Rows[index1]["GT_THE_TU"].ToString() + "</GT_THE_TU>\r\n                                    <GT_THE_DEN>" + dataTable1.Rows[index1]["GT_THE_DEN"].ToString() + "</GT_THE_DEN>\r\n                                    <MIEN_CUNG_CT>" + dataTable1.Rows[index1]["MIEN_CUNG_CT"].ToString() + "</MIEN_CUNG_CT>\r\n                                    <TEN_BENH><![CDATA[" + dataTable1.Rows[index1]["TEN_BENH"].ToString() + "]]></TEN_BENH>\r\n                                    <MA_BENH>" + dataTable1.Rows[index1]["MA_BENH"].ToString() + "</MA_BENH>\r\n                                    <MA_BENHKHAC><![CDATA[" + dataTable1.Rows[index1]["MA_BENHKHAC"].ToString() + "]]></MA_BENHKHAC>\r\n                                    <MA_LYDO_VVIEN>" + dataTable1.Rows[index1]["MA_LYDO_VVIEN"].ToString() + "</MA_LYDO_VVIEN>\r\n                                    <MA_NOI_CHUYEN>" + dataTable1.Rows[index1]["MA_NOI_CHUYEN"].ToString() + "</MA_NOI_CHUYEN>\r\n                                    <MA_TAI_NAN>" + dataTable1.Rows[index1]["MA_TAI_NAN"].ToString() + "</MA_TAI_NAN>\r\n                                    <NGAY_VAO>" + dataTable1.Rows[index1]["NGAY_VAO"].ToString() + "</NGAY_VAO>\r\n                                    <NGAY_RA>" + dataTable1.Rows[index1]["NGAY_RA"].ToString() + "</NGAY_RA>\r\n                                    <SO_NGAY_DTRI>" + dataTable1.Rows[index1]["SO_NGAY_DTRI"].ToString() + "</SO_NGAY_DTRI>\r\n                                    <KET_QUA_DTRI>" + dataTable1.Rows[index1]["KET_QUA_DTRI"].ToString() + "</KET_QUA_DTRI>\r\n                                    <TINH_TRANG_RV>" + dataTable1.Rows[index1]["TINH_TRANG_RV"].ToString() + "</TINH_TRANG_RV>\r\n                                    <NGAY_TTOAN>" + dataTable1.Rows[index1]["NGAY_TTOAN"].ToString() + "</NGAY_TTOAN>\r\n                                    <T_THUOC>" + dataTable1.Rows[index1]["T_THUOC"].ToString() + "</T_THUOC>\r\n                                    <T_VTYT>" + dataTable1.Rows[index1]["T_VTYT"].ToString() + "</T_VTYT>\r\n                                    <T_TONGCHI>" + dataTable1.Rows[index1]["T_TONGCHI"].ToString() + "</T_TONGCHI>\r\n                                    <T_BNTT>" + dataTable1.Rows[index1]["T_BNTT"].ToString() + "</T_BNTT>\r\n                                    <T_BNCCT>" + dataTable1.Rows[index1]["T_BNCCT"].ToString() + "</T_BNCCT>\r\n                                    <T_BHTT>" + dataTable1.Rows[index1]["T_BHTT"].ToString() + "</T_BHTT>\r\n                                    <T_NGUONKHAC>" + dataTable1.Rows[index1]["T_NGUONKHAC"].ToString() + "</T_NGUONKHAC>\r\n                                    <T_NGOAIDS>" + dataTable1.Rows[index1]["T_NGOAIDS"].ToString() + "</T_NGOAIDS>\r\n                                    <NAM_QT>" + dataTable1.Rows[index1]["NAM_QT"].ToString() + "</NAM_QT>\r\n                                    <THANG_QT>" + dataTable1.Rows[index1]["THANG_QT"].ToString() + "</THANG_QT>\r\n                                    <MA_LOAI_KCB>" + dataTable1.Rows[index1]["MA_LOAI_KCB"].ToString() + "</MA_LOAI_KCB>\r\n                                    <MA_KHOA>" + dataTable1.Rows[index1]["MA_KHOA"].ToString() + "</MA_KHOA>\r\n                                    <MA_CSKCB>" + dataTable1.Rows[index1]["MA_CSKCB"].ToString() + "</MA_CSKCB>\r\n                                    <MA_KHUVUC>" + dataTable1.Rows[index1]["MA_KHUVUC"].ToString() + "</MA_KHUVUC>\r\n                                    <MA_PTTT_QT>" + dataTable1.Rows[index1]["MA_PTTT_QT"].ToString() + "</MA_PTTT_QT>\r\n                                    <CAN_NANG>" + dataTable1.Rows[index1]["CAN_NANG"].ToString() + "</CAN_NANG>\r\n                        </TONG_HOP>";
                string str9 = (!flag1 ? str8 + plainText1 : str8 + XuatXML.Base64Encode(plainText1)) + "\r\n\t\t\t\t</NOIDUNGFILE>\r\n             \t\t\t</FILEHOSO>\r\n                    ";
                if (IsSaveToDataBase)
                {
                    string[] strArray = new string[40]
                    {
            "MA_LK",
            "STT",
            "MA_BN",
            "HO_TEN",
            "NGAY_SINH",
            "GIOI_TINH",
            "DIA_CHI",
            "MA_THE",
            "MA_DKBD",
            "GT_THE_TU",
            "GT_THE_DEN",
            "MIEN_CUNG_CT",
            "TEN_BENH",
            "MA_BENH",
            "MA_BENHKHAC",
            "MA_LYDO_VVIEN",
            "MA_NOI_CHUYEN",
            "MA_TAI_NAN",
            "NGAY_VAO",
            "NGAY_RA",
            "SO_NGAY_DTRI",
            "KET_QUA_DTRI",
            "TINH_TRANG_RV",
            "NGAY_TTOAN",
            "T_THUOC",
            "T_VTYT",
            "T_TONGCHI",
            "T_BNTT",
            "T_BNCCT",
            "T_BHTT",
            "T_NGUONKHAC",
            "T_NGOAIDS",
            "NAM_QT",
            "THANG_QT",
            "MA_LOAI_KCB",
            "MA_KHOA",
            "MA_CSKCB",
            "MA_KHUVUC",
            "MA_PTTT_QT",
            "CAN_NANG"
                    };
                    string str10 = "";
                    string str11 = "";
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                    {
                        str10 = str10 + strArray[index2] + ",";
                        str11 = str11 + strArray[index2] + "=N'" + dataTable1.Rows[index1][strArray[index2]].ToString() + "',";
                    }
                    string str12 = str11.Remove(str11.Length - 1, 1);
                    if (!DataAcess.Connect.ExecSQL("\r\n                                       INSERT INTO TEMP_XML1 (" + str10.Remove(str10.Length - 1, 1) + ",SAVEDATE,OUTPUTCODE) select\r\n                                                " + str12 + ",SAVEDATE=GETDATE(),OUTPUTCODE='" + str1 + "'"))
                    {
                        int num = (int)MessageBox.Show("Lưu vào TEMP_XML1 thất bại");
                        return;
                    }
                }
                else
                    DataAcess.Connect.ExecSQL("\r\n                                       UPDATE hs_benhnhanbhdongtien SET IsOutPutXML=1 WHERE ID=" + dataTable1.Rows[index1]["MA_LK"].ToString() + "     \r\n                                        ");
                if (table6.Rows.Count > 0)
                {
                    string str10 = str9 + "\r\n                    <FILEHOSO>\r\n             \t\t\t<LOAIHOSO>XML2</LOAIHOSO>\r\n             \t\t\t\t<NOIDUNGFILE>";
                    string str11 = "<DSACH_CHI_TIET_THUOC>";
                    for (int index2 = 0; index2 < table6.Rows.Count; ++index2)
                    {
                        string str12 = str11 + "\r\n                          <CHI_TIET_THUOC>";
                        for (int index3 = 0; index3 < table6.Columns.Count; ++index3)
                        {
                            string str13;
                            if (table6.Columns[index3].ColumnName == "TEN_THUOC" || table6.Columns[index3].ColumnName == "HAM_LUONG" || table6.Columns[index3].ColumnName == "LIEU_DUNG")
                                str13 = "\r\n                                <" + table6.Columns[index3].ColumnName + "><![CDATA[" + table6.Rows[index2][table6.Columns[index3].ColumnName].ToString() + "]]></" + table6.Columns[index3].ColumnName + ">";
                            else if (table6.Rows[index2][table6.Columns[index3].ColumnName].ToString() == "")
                                str13 = "\r\n                                        <" + table6.Columns[index3].ColumnName + " />";
                            else
                                str13 = "\r\n                                        <" + table6.Columns[index3].ColumnName + ">" + table6.Rows[index2][table6.Columns[index3].ColumnName].ToString() + "</" + table6.Columns[index3].ColumnName + ">";
                            str12 += str13;
                        }
                        str11 = str12 + "\r\n                        </CHI_TIET_THUOC>\r\n                        ";
                        if (IsSaveToDataBase)
                        {
                            string str13 = "";
                            for (int index3 = 0; index3 < table6.Columns.Count; ++index3)
                                str13 = str13 + table6.Columns[index3].ColumnName + "=N'" + table6.Rows[index2][index3].ToString().Replace("'", "''") + "',";
                            if (!DataAcess.Connect.ExecSQL("\r\n                                        INSERT INTO TEMP_XML2 \r\n                                                SELECT " + str13.Remove(str13.Length - 1, 1) + ",SAVEDATE=GETDATE(),OUTPUTCODE='" + str1 + "'\r\n                                        "))
                            {
                                int num = (int)MessageBox.Show("Lưu vào TEMP_XML2 thất bại");
                                return;
                            }
                        }
                    }
                    string plainText2 = str11 + "</DSACH_CHI_TIET_THUOC>";
                    str9 = (!flag1 ? str10 + plainText2 : str10 + XuatXML.Base64Encode(plainText2)) + "\r\n\t\t\t\t</NOIDUNGFILE>\r\n             \t\t\t</FILEHOSO>\r\n                    ";
                }
                if (table8.Rows.Count > 0)
                {
                    string str10 = str9 + "\r\n                <FILEHOSO>\r\n                    <LOAIHOSO>XML3</LOAIHOSO>\r\n             \t\t\t\t<NOIDUNGFILE>";
                    string str11 = "<DSACH_CHI_TIET_DVKT>";
                    for (int index2 = 0; index2 < table8.Rows.Count; ++index2)
                    {
                        string str12 = str11 + "\r\n                          <CHI_TIET_DVKT>";
                        for (int index3 = 0; index3 < table8.Columns.Count; ++index3)
                        {
                            string str13;
                            if (table8.Columns[index3].ColumnName == "TEN_VAT_TU" || table8.Columns[index3].ColumnName == "TEN_DICH_VU" || table8.Columns[index3].ColumnName == "LIEU_DUNG")
                                str13 = "\r\n                                <" + table8.Columns[index3].ColumnName + "><![CDATA[" + table8.Rows[index2][table8.Columns[index3].ColumnName].ToString() + "]]></" + table8.Columns[index3].ColumnName + ">";
                            else if (table8.Rows[index2][table8.Columns[index3].ColumnName].ToString() == "")
                                str13 = "\r\n                                        <" + table8.Columns[index3].ColumnName + " />";
                            else
                                str13 = "\r\n                                            <" + table8.Columns[index3].ColumnName + ">" + table8.Rows[index2][table8.Columns[index3].ColumnName].ToString() + "</" + table8.Columns[index3].ColumnName + ">";
                            str12 += str13;
                        }
                        str11 = str12 + "\r\n                          </CHI_TIET_DVKT>\r\n                        ";
                        if (IsSaveToDataBase)
                        {
                            string str13 = "";
                            for (int index3 = 0; index3 < table8.Columns.Count; ++index3)
                                str13 = str13 + table8.Columns[index3].ColumnName + "=N'" + table8.Rows[index2][index3].ToString() + "',";
                            if (!DataAcess.Connect.ExecSQL("\r\n                                         INSERT INTO TEMP_XML3 \r\n                                                SELECT " + str13.Remove(str13.Length - 1, 1) + ",SAVEDATE=GETDATE(),OUTPUTCODE='" + str1 + "'\r\n                                        "))
                            {
                                int num = (int)MessageBox.Show("Lưu vào TEMP_XML3 thất bại");
                                return;
                            }
                        }
                    }
                    string plainText2 = str11 + "</DSACH_CHI_TIET_DVKT>";
                    str9 = (!flag1 ? str10 + plainText2 : str10 + XuatXML.Base64Encode(plainText2)) + "\r\n                </NOIDUNGFILE>\r\n \t\t\t</FILEHOSO>\r\n            ";
                }
                bool flag2;
                if (dataTable3 != null && dataTable3.Rows.Count > 0)
                {
                    string str10 = str9 + "\r\n                <FILEHOSO>\r\n                    <LOAIHOSO>XML4</LOAIHOSO>\r\n             \t\t\t\t<NOIDUNGFILE>";
                    string str11 = "<DSACH_CHI_TIET_CLS>";
                    for (int index2 = 0; index2 < dataTable3.Rows.Count; ++index2)
                    {
                        string str12 = str11 + "\r\n                          <CHI_TIET_CLS>";
                        for (int index3 = 0; index3 < dataTable3.Columns.Count; ++index3)
                        {
                            string str13;
                            if (dataTable3.Columns[index3].ColumnName == "TEN_CHI_SO" || dataTable3.Columns[index3].ColumnName == "GIA_TRI" || dataTable3.Columns[index3].ColumnName == "MO_TA" || dataTable3.Columns[index3].ColumnName == "KET_LUAN")
                                str13 = "\r\n                                <" + dataTable3.Columns[index3].ColumnName + "><![CDATA[" + dataTable3.Rows[index2][dataTable3.Columns[index3].ColumnName].ToString() + "]]></" + dataTable3.Columns[index3].ColumnName + ">";
                            else if (dataTable3.Rows[index2][dataTable3.Columns[index3].ColumnName].ToString() == "")
                                str13 = "\r\n                                        <" + dataTable3.Columns[index3].ColumnName + " />";
                            else
                                str13 = "\r\n                                            <" + dataTable3.Columns[index3].ColumnName + ">" + dataTable3.Rows[index2][dataTable3.Columns[index3].ColumnName].ToString() + "</" + dataTable3.Columns[index3].ColumnName + ">";
                            str12 += str13;
                        }
                        str11 = str12 + "\r\n                          </CHI_TIET_CLS>\r\n                        ";
                        flag2 = true;
                    }
                    string plainText2 = str11 + "</DSACH_CHI_TIET_CLS>";
                    str9 = (!flag1 ? str10 + plainText2 : str10 + XuatXML.Base64Encode(plainText2)) + "\r\n                </NOIDUNGFILE>\r\n \t\t\t</FILEHOSO>\r\n            ";
                }
                if (table9 != null && table9.Rows.Count > 0)
                {
                    string str10 = str9 + "\r\n                <FILEHOSO>\r\n                    <LOAIHOSO>XML5</LOAIHOSO>\r\n             \t\t\t\t<NOIDUNGFILE>";
                    string str11 = "<DSACH_CHI_TIET_DIEN_BIEN_BENH>";
                    for (int index2 = 0; index2 < table9.Rows.Count; ++index2)
                    {
                        string str12 = str11 + "\r\n                          <CHI_TIET_DIEN_BIEN_BENH>";
                        for (int index3 = 0; index3 < table9.Columns.Count; ++index3)
                        {
                            string str13;
                            if (table9.Columns[index3].ColumnName == "DIEN_BIEN" || table9.Columns[index3].ColumnName == "HOI_CHAN" || table9.Columns[index3].ColumnName == "PHAU_THUAT")
                                str13 = "\r\n                                <" + table9.Columns[index3].ColumnName + "><![CDATA[" + table9.Rows[index2][table9.Columns[index3].ColumnName].ToString() + "]]></" + table9.Columns[index3].ColumnName + ">";
                            else if (table9.Rows[index2][table9.Columns[index3].ColumnName].ToString() == "")
                                str13 = "\r\n                                        <" + table9.Columns[index3].ColumnName + " />";
                            else
                                str13 = "\r\n                                            <" + table9.Columns[index3].ColumnName + ">" + table9.Rows[index2][table9.Columns[index3].ColumnName].ToString() + "</" + table9.Columns[index3].ColumnName + ">";
                            str12 += str13;
                        }
                        str11 = str12 + "\r\n                          </CHI_TIET_DIEN_BIEN_BENH>\r\n                        ";
                        flag2 = true;
                    }
                    string plainText2 = str11 + "</DSACH_CHI_TIET_DIEN_BIEN_BENH>";
                    str9 = (!flag1 ? str10 + plainText2 : str10 + XuatXML.Base64Encode(plainText2)) + "\r\n                </NOIDUNGFILE>\r\n \t\t\t</FILEHOSO>\r\n            ";
                }
                byte[] bytes = Encoding.ASCII.GetBytes(str9 + "\r\n        </HOSO>\r\n     </DANHSACHHOSO>\r\n  </THONGTINHOSO>\r\n   <CHUKYDONVI />\r\n </GIAMDINHHS>");
                hsTool.b_WriteFile(path + "\\" + str4, bytes);
            }
            NumberRow = dataTable1.Rows.Count.ToString();
        }

        public static string Base64Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        private void btnLayDS_Click(object sender, EventArgs e)
        {
           
        }

        private DataTable dtSource_B1()
        {
          //  int selectedIndex1 = this.cbIsNoiTru.SelectedIndex;
          //  int selectedIndex2 = this.cbIsDaKetXuat.SelectedIndex;
            string str1 = this.txtMA_LK.Text.Trim();
            string str2 = this.txtTenBN.Text.Trim();
            string str3 = this.txtMaBN.Text.Trim();
            string strCommandText1 = @"UPDATE hs_benhnhanbhdongtien SET MA_TAI_NAN=ISNULL(B.loaitainanid,0)
                                        FROM hs_benhnhanbhdongtien A
                                        INNER JOIN DANGKYKHAM B ON A.ID=B.IDBENHBHDONGTIEN
                                        INNER JOIN BENHNHAN BN ON A.IDBENHNHAN=BN.IDBENHNHAN 
                                        WHERE ISNULL(B.loaitainanid,0)<>0
                                        AND A.NGAYTINHBH_THUC>='" + this.dtpFromDate.Value.ToString("yyyy/MM/dd") + @"'
                                        AND A.NGAYTINHBH_THUC<='" + this.dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59'";
            if (str1 != null && str1 != "")
                strCommandText1 = strCommandText1 + " AND ISNULL(A.ID_XML,A.ID)=" + str1;
            if (str3 != "")
                strCommandText1 = strCommandText1 + " AND BN.MABENHNHAN=N'" + str3 + "'";
            if (str2 != "")
                strCommandText1 = strCommandText1 + " AND BN.tenbenhnhan LIKE N'%" + str2 + "%'";
            DataAcess.Connect.ExecSQL(strCommandText1);
            string[] strArray1 = new string[5]
                        {
                    @"UPDATE HS_BENHNHANBHDONGTIEN
                    SET
                    MUC_HUONG=ISNULL((CASE WHEN ( B.DUNGTUYEN='Y' OR B.ISCAPCUU=1 ) AND  A.TONGTIENBH<=dbo.GetMucTinhBH(A.NGAYTINHBH_THUC)  THEN 100 ELSE C.BHYTTRA*(CASE WHEN ( B.DUNGTUYEN='Y' OR B.IsCapCuu=1 ) THEN 1 ELSE ROUND( D.BHYTTRA*1.0/100,0) END) END ) ,0)
                    FROM HS_BENHNHANBHDONGTIEN A
                    INNER JOIN BENHNHAN_BHYT B ON A.IDBENHNHAN_BH=B.IDBENHNHAN_BH
                    INNER JOIN HS_TILEBHYT C ON C.ID= (SELECT TOP 1 C0.ID FROM  HS_TILEBHYT C0 WHERE  C0.DOITUONG=B.SOBH1 AND C0.MUCHUONG=B.SOBH2 ORDER BY C0.TUNGAY DESC)
                    INNER JOIN HS_MUCHUONGTRAITUYEN D ON D.ID=(SELECT TOP 1 D0.ID FROM HS_MUCHUONGTRAITUYEN D0 WHERE TUNGAY<=A.NGAYTINHBH_THUC ORDER BY TUNGAY DESC)
                    INNER JOIN BENHNHAN BN ON A.IDBENHNHAN=BN.IDBENHNHAN 
                    WHERE A.ISBHYT=1
                    AND A.ISNOITRU=0
                    AND ISNULL(A.XML_DATE,A.NGAYTINHBH_THUC)>='",
                     this.dtpFromDate.Value.ToString("yyyy/MM/dd"),
                     "' AND ISNULL(A.XML_DATE,A.NGAYTINHBH_THUC)<='",
                    null,
                    null
            };
            string[] strArray2 = strArray1;
            int index1 = 3;
            DateTime dateTime = this.dtpToDate.Value;
            string str4 = dateTime.ToString("yyyy/MM/dd");
            strArray2[index1] = str4;
            strArray1[4] = " 23:59:59'\r\n                                        ";
            string strCommandText2 = string.Concat(strArray1);
            if (str1 != null && str1 != "")
                strCommandText2 = strCommandText2 + " AND ISNULL(A.ID_XML,A.ID)=" + str1;
            if (str3 != "")
                strCommandText2 = strCommandText2 + " AND BN.MABENHNHAN=N'" + str3 + "'";
            if (str2 != "")
                strCommandText2 = strCommandText2 + " AND BN.tenbenhnhan LIKE N'%" + str2 + "%'";
            DataAcess.Connect.ExecSQL(strCommandText2);
            string[] strArray3 = new string[5]
            {
                @"UPDATE HS_BENHNHANBHDONGTIEN
                SET MA_KHOA=C.MAPHONGKHAMBENH 
                FROM HS_BENHNHANBHDONGTIEN A
                INNER JOIN KHAMBENH B ON A.IDKHAMBENH_LAST=B.IDKHAMBENH
                INNER JOIN PHONGKHAMBENH C ON B.IDPHONGKHAMBENH=C.IDPHONGKHAMBENH
                INNER JOIN BENHNHAN BN ON A.IDBENHNHAN=BN.IDBENHNHAN
                WHERE A.ISBHYT=1
                AND ISNULL(A.MA_KHOA,'')=''
                AND A.NGAYTINHBH_THUC>='",
                null,
                null,
                null,
                null
            };
            string[] strArray4 = strArray3;
            int index2 = 1;
            dateTime = this.dtpFromDate.Value;
            string str5 = dateTime.ToString("yyyy/MM/dd");
            strArray4[index2] = str5;
            strArray3[2] = "'AND A.NGAYTINHBH_THUC<='";
            string[] strArray5 = strArray3;
            int index3 = 3;
            dateTime = this.dtpToDate.Value;
            string str6 = dateTime.ToString("yyyy/MM/dd");
            strArray5[index3] = str6;
            strArray3[4] = " 23:59:59'";
            string strCommandText3 = string.Concat(strArray3);
            if (str1 != null && str1 != "")
                strCommandText3 = strCommandText3 + " AND ISNULL(A.ID_XML,A.ID)=" + str1;
            if (str3 != "")
                strCommandText3 = strCommandText3 + " AND BN.MABENHNHAN=N'" + str3 + "'";
            if (str2 != "")
                strCommandText3 = strCommandText3 + " AND BN.tenbenhnhan LIKE N'%" + str2 + "%'";
            DataAcess.Connect.ExecSQL(strCommandText3);
            string[] strArray6 = new string[7];
            strArray6[0] = "declare @fromdate as datetime set @fromdate='";
            string[] strArray7 = strArray6;
            int index4 = 1;
            dateTime = this.dtpFromDate.Value;
            string str7 = dateTime.ToString("yyyy/MM/dd");
            strArray7[index4] = str7;
            strArray6[2] = "'declare @todate as datetime set @todate='";
            string[] strArray8 = strArray6;
            int index5 = 3;
            dateTime = this.dtpToDate.Value;
            string str8 = dateTime.ToString("yyyy/MM/dd");
            strArray8[index5] = str8;
            strArray6[4] = @" 23:59:59' select   MA_LK=ISNULL(A.ID_XML,A.ID),
                                                           STT=row_number() over(ORDER BY ISNULL(A.ID_XML,A.ID)) ,
						                                    ma_bn=b.mabenhnhan,
							                                ho_ten=b.tenbenhnhan,
							                                Ngay_sinh= ( CASE WHEN LEN(NGAYSINH)>=4 THEN RIGHT(NGAYSINH,4) ELSE '' END )+( CASE WHEN LEN(NGAYSINH)>=4 THEN (CASE WHEN CHARINDEX('/',ngaysinh,2)<>0 THEN REPLACE( SUBSTRING(NGAYSINH,CHARINDEX('/',ngaysinh,2)+1,2),'/','') ELSE (CASE WHEN CHARINDEX('-',ngaysinh,2)<>0 THEN REPLACE( SUBSTRING(NGAYSINH,CHARINDEX('-',ngaysinh,2)+1,2),'-','') ELSE '' END  ) END  )ELSE '' END) +( CASE WHEN LEN(NGAYSINH)>=4 THEN (CASE WHEN CHARINDEX('/',ngaysinh)<>0 THEN SUBSTRING(NGAYSINH,0,CHARINDEX('/',ngaysinh)) ELSE (CASE WHEN CHARINDEX('-',ngaysinh)<>0 THEN SUBSTRING(NGAYSINH,0,CHARINDEX('-',ngaysinh)) ELSE '' END  ) END  ) ELSE '' END)
							                                 ,
							                                gioi_tinh=(case when b.gioitinh=1 then 1 else 0 end)+1,
							                                dia_chi=b.diachi,
							                                 ma_the=C.SOBHYT+ISNULL(';'+C2.SOBHYT,''),
							                                  ma_dkbd=REPLACE( D.MADANGKY,'-','')+ISNULL(';'+REPLACE( D2.MADANGKY,'-',''),''),
							                                 gt_the_tu=REPLACE( CONVERT(NVARCHAR(20), C.ngaybatdau,111),'/','')+ISNULL(';'+REPLACE( CONVERT(NVARCHAR(20), C2.ngaybatdau,111),'/',''),''),
							                                 gt_the_den=REPLACE( CONVERT(NVARCHAR(20), C.ngayhethan,111),'/','')+ISNULL(';'+REPLACE( CONVERT(NVARCHAR(20), C2.ngayhethan,111),'/',''),''),
							                                 ten_benh=REPLACE( REPLACE( A.TenChanDoan_All,'-',','),';',','),
							                                 MA_BENH= A.MACHANDOAN, -- REPLACE( REPLACE( A.MAChanDoan_All,'-',','),';',','),
							                                 MA_BENHkhac= A.ma_benhkhac,
							                                 ma_lydo_vvien=(CASE WHEN A.ISCAPCUU=1 THEN 2 ELSE (CASE WHEN A.DUNGTUYEN='Y' THEN (CASE WHEN D.MADANGKY='83-041' THEN 1 ELSE 4 END) ELSE 3 END) END),
							                                 ma_noi_chuyen=REPLACE(I.MABENHVIEN,'-',''),
							                                 ma_tai_nan=NULL,
							                                 ngay_vao=REPLACE( CONVERT(NVARCHAR(20), A.NGAYTINHBH,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),A.NGAYTINHBH,108),5),':',''),
							                                  ngay_ra=REPLACE( CONVERT(NVARCHAR(20), A.NGAYTINHBH_THUC,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),A.NGAYTINHBH_THUC,108),5),':',''),
							                                  SO_NGAY_DTRI= (CASE WHEN ISNULL(A.ISNOITRU,0)=0 THEN 0 ELSE CONVERT(INT,( CONVERT(DATETIME, CONVERT(NVARCHAR(20), A.NGAYTINHBH_THUC,111)+' 00:00:00' ) -CONVERT(DATETIME, CONVERT(NVARCHAR(20), A.NGAYTINHBH,111)+' 00:00:00' )))+1  END),
							                                  ket_qua_dtri= (CASE WHEN I.MABENHVIEN IS NOT NULL THEN '4' ELSE   ISNULL( A.ket_qua_dtri,'1') END),
							                                  tinh_trang_rv=(CASE WHEN I.MABENHVIEN IS NOT NULL THEN '2' ELSE  ISNULL( a.TINH_TRANG_RV,'1') END),
							                                  ngay_ttoan=REPLACE( CONVERT(NVARCHAR(20), A.NGAYTINHBH_THUC,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),A.NGAYTINHBH_THUC,108),5),':',''),
							                                  MUC_HUONG=A.MUC_HUONG,
							                                  t_thuoc=0.00,
							                                  t_vtyt=0.00,
							                                  t_tongchi=0.00,
							                                  t_bntt=0.00,
							                                  t_bhtt=0.00,
							                                    T_BNCCT=0.00,
								                                 t_nguonkhac=0,
								                                 t_ngoaids=0,
								                                  nam_qt=YEAR(A.NGAYTINHBH_THUC),
								                                   thang_qt=MONTH(A.NGAYTINHBH_THUC),
								                                   ma_loai_kcb=(case when ISNULL(A.IsDieuTriNgoaiTru,0)=1 THEN '2' ELSE (case when ISNULL(A.IsNoiTru,0)=1 THEN '3' ELSE '1' END) END),
								                                   MA_KHOA=G.MAPHONGKHAMBENH,
								                                   ma_cskcb='83041',
								                                   ma_khuvuc=C.MA_KHUVUC,
								                                   MA_PTTT_qt=NULL,
								                                   can_nang=(CASE WHEN RIGHT(B.NGAYSINH,4)=CONVERT(VARCHAR(4),YEAR(GETDATE())) AND ISNULL(CAN_NANG,0)=0 THEN ISNULL((SELECT TOP 1 1 FROM sinhhieu A0 INNER JOIN khambenh B0 ON A0.IdKhamBenh=B0.idkhambenh INNER JOIN dangkykham C0 ON B0.iddangkykham=C0.iddangkykham WHERE C0.IdBenhBHDongTien=A.ID AND ISNULL( A0.cannang,0)<>0 ),3) ELSE A.CAN_NANG END ),
								                                    a.idbenhnhan,
									                                  TUOIBN=DBO.kb_GetTuoi(b.ngaysinh),
									                                  MIEN_CUNG_CT=REPLACE( CONVERT(NVARCHAR(20), C.ngaybd_miendct,111),'/',''),
									                                 MA_TAI_NAN=A.MA_TAI_NAN
									                                 ,IsShow=(CASE WHEN ISNULL(A.ID_XML,0)<>0 THEN '0' ELSE '1' END )
									                                 ,A.NGAYTINHBH
									                                 ,A.NGAYTINHBH_THUC
									                                 ,A.IsNoiTru 
									                                 ,A.IsFixChanDoan
									                                      from hs_benhnhanbhdongtien a
									                                 inner join benhnhan b on a.idbenhnhan=B.IDBENHNHAN
					                                 INNER JOIN BENHNHAN_BHYT C ON A.IDBENHNHAN_BH=C.IDBENHNHAN_BH
					                                  left join KB_NOIDANGKYKB d on c.IdNoiDangKyBH=D.IDNOIDANGKY
					                                  LEFT JOIN KB_NOIDANGKYKB E ON C.IdNoiGioiThieu=E.IDNOIDANGKY
					                                  LEFT JOIN KHAMBENH F ON A.IDKHAMBENH_LAST=F.IDKHAMBENH
					                                  LEFT JOIN PHONGKHAMBENH G ON F.IDPHONGKHAMBENH=G.IDPHONGKHAMBENH
					                                  LEFT JOIN benhvien I ON F.IdBenhVienChuyen=I.idBenhVien
						                              LEFT JOIN BENHNHAN_BHYT C2 ON A.IDBENHNHAN_BH2=C2.IDBENHNHAN_BH
						                              LEFT JOIN KB_NOIDANGKYKB D2 on C2.IdNoiDangKyBH=D2.IDNOIDANGKY
						                              LEFT JOIN KB_NOIDANGKYKB E2 ON C2.IdNoiGioiThieu=E2.IDNOIDANGKY
                                                where a.isbhyt=1
                                                AND ISNULL(A.IdKhamBenh_Last,0)<>0
                                                AND ISNULL(A.ISCHECK_ALL,0)=1 ";
            strArray6[5] = str1 == null || str1 == "" ? " AND ISNULL(A.XML_DATE,A.NGAYTINHBH_THUC)>=@fromdate AND ISNULL(A.XML_DATE,A.NGAYTINHBH_THUC)<=@todate" : "";
            strArray6[6] = "";
            string strSelect = string.Concat(strArray6);
            if (str2 != null && str2 != "")
                strSelect = strSelect + " AND B.TENBENHNHAN LIKE N'%" + str2 + "%'";
            if (str3 != null && str3 != "")
                strSelect = strSelect + " AND B.mabenhnhan LIKE N'" + str3 + "'";
            if (str1 != null && str1 != "")
                strSelect = strSelect + " AND ISNULL(A.ID_XML,A.ID) =" + str1;
            //if (selectedIndex1 != 0 && selectedIndex1 != -1)
            //    strSelect = selectedIndex1 != 1 ? strSelect + " AND ISNULL(A.ISNOITRU,0)=0" : strSelect + " AND A.ISNOITRU=1";
            //if (selectedIndex2 != 0 && selectedIndex2 != -1)
            //    strSelect = selectedIndex2 != 1 ? strSelect + " AND ISNULL(A.IsOutPutXML,0)=0" : strSelect + " AND A.IsOutPutXML=1";
            DataTable table = DataAcess.Connect.GetTable(strSelect);
            for (int index6 = 0; index6 < table.Rows.Count; ++index6)
                table.Rows[index6]["ma_benhkhac"] = (object)XuatXML.fixDouble(table.Rows[index6]["ma_benhkhac"].ToString());
            return table;
        }

        private DataTable dtSource_B2()
        {
            string str1 = this.txtMA_LK.Text.Trim();
            string str2 = this.txtTenBN.Text.Trim();
            string str3 = this.txtMaBN.Text.Trim();
            string sql=@"declare @fromdate as datetime
                                                            set @fromdate='" + this.dtpFromDate.Value.ToString("yyyy/MM/dd") + @"'
                                                            declare @todate as datetime
                                                            set @todate='" + this.dtpToDate.Value.ToString("yyyy/MM/dd") + @" 23:59:59'
                                                            SELECT    DISTINCT       MA_LK=ISNULL(B.ID_XML,B.ID),
                                                            STT=1,
                                                            MA_THUOC=ISNULL(C.mahoatchat,'000'),
                                                            MA_NHOM=(CASE WHEN ISNULL(C.TiLeTT,'100.00')='100' OR ISNULL(C.TiLeTT,'100.00')='100.00' THEN '4' ELSE '6' END),
                                                            TEN_THUOC=ISNULL(C.TENGOC,C.TENTHUOC),
                                                            DON_VI_TINH=D.TenDVT,
                                                            HAM_LUONG= C.HamLuong,
                                                            DUONG_DUNG=ISNULL(E.MADUONGDUNG,'1.01'),
	                                                        LIEU_DUNG=ISNULL(BNTT.moilanuong+'x'+CONVERT(NVARCHAR,BNTT.ngayuong) +' '+D.TENDVT,N'Không có'), 
                                                            SO_DANG_KY=  C.SODK_THUOC,
                                                            TT_THAU=(CASE WHEN ISNULL(C.TT_THAU,'')<>'' THEN C.TT_THAU ELSE 'null' end),
                                                            PHAM_VI= 1,
	                                                        SO_LUONG=SUM(A.SOLUONG),
	                                                        DON_GIA=(CASE WHEN CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))*1.00=100 THEN  A.DONGIABH ELSE C.GIA_MUA END)  , 
                                                            TYLE_TT=CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))*1.00,
                                                             THANH_TIEN= SUM(A.SOLUONG)* (CASE WHEN CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))*1.00=100 THEN  A.DONGIABH ELSE C.GIA_MUA END) , 
                                                             MUC_HUONG= B.MUC_HUONG, 
                                                              T_NGUONKHAC=0.0, 
                                                               T_BNTT=(CASE WHEN CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))*1.00=100 THEN  0 ELSE ROUND( SUM(A.SOLUONG)*C.GIA_MUA*CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))/100,2) END)     ,
                                                               T_BHTT=ROUND((B.MUC_HUONG/100)*SUM(A.SOLUONG)* (CASE WHEN CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))*1.00=100 THEN  A.DONGIABH ELSE CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))*C.GIA_MUA/100 END),2), 
                                                                T_BNCCT=ROUND(((100-B.MUC_HUONG)/100)*SUM(A.SOLUONG)* (CASE WHEN CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))*1.00=100 THEN  A.DONGIABH ELSE CONVERT(FLOAT,ISNULL(C.TiLeTT,'100.00'))*C.GIA_MUA/100 END),2),
	                                                             T_NGOAIDS=0, 
	                                                               MA_KHOA=B.MA_KHOA, 
	                                                               MA_BAC_SI=IsNULL(H.mabacsi,'000'),
	                                                               MA_BENH=ISNULL(J.MAICD,B.MACHANDOAN),
	                                                                NGAY_YL=REPLACE( CONVERT(NVARCHAR(20),ISNULL( G.NGAYKHAM,B.NGAYTINHBH_THUC),111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),ISNULL( G.NGAYKHAM,B.NGAYTINHBH_THUC),108),5),':',''),
		                                                            MA_PTTT='1' 
		                                                            FROM HS_BENHNHANBHDONGTIEN B
                                                            inner join benhnhan BN on B.idbenhnhan=BN.IDBENHNHAN
                                                             INNER JOIN CHITIETPHIEUXUATKHO_HS A ON A.IDBENHBHDONGTIEN_HS=B.ID 
                                                            LEFT JOIN CHITIETBENHNHANTOATHUOC BNTT ON ISNULL(A.IDCHITIETBENHNHANTOATHUOC,A.TOP1_IDCHITIETBENHNHANTOATHUOC)=BNTT.IDCHITIETBENHNHANTOATHUOC
                                                            LEFT JOIN THUOC C ON A.IDTHUOC=C.IDTHUOC  
                                                             LEFT JOIN Thuoc_DonViTinh D ON C.iddvt=D.Id
                                                               LEFT JOIN Thuoc_CachDung E ON C.IdCachDung=E.idcachdung 
                                                                LEFT JOIN khambenh G ON A.IDKHAMBENH1=G.idkhambenh 
	                                                             LEFT JOIN BACSI H ON H.IDBACSI=(CASE WHEN ISNULL(G.idbacsi,0)=0 THEN G.idbacsi ELSE G.idbacsi END) 
                                                              LEFT JOIN PHONGKHAMBENH I ON G.IDPHONGKHAMBENH=I.IDPHONGKHAMBENH 
                                                               LEFT JOIN CHANDOANICD J ON CONVERT(VARCHAR, J.IDICD)=G.KETLUAN
                                                            WHERE B.IsBHYT=1 
                                                            AND A.IsBHYT=1
                                                            AND ISNULL(B.IDKHAMBENH_LAST,0)<>0
                                                            AND ISNULL(B.ISCHECK_ALL,0)=1
                                                            " + (str1 == null || str1 == "" ? @"
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)>=@fromdate
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)<=@ToDate
                                                            " : "") + @"
                                                            AND C.LOAITHUOCID=1
                                                            AND A.DonGiaBH>0 AND A.SOLUONG>0
                                                            " + (str1 == null || !(str1 != "") ? "" : " AND ISNULL(B.ID_XML,B.ID)='" + str1 + "'") + @"   
                                                            " + (str2 == null || !(str2 != "") ? "" : " AND BN.TENBENHNHAN LIKE N'%" + str2 + "%'") + @"    
                                                            " + (str3 == null || !(str3 != "") ? "" : " AND BN.MABENHNHAN=N'" + str3 + "'") + @"
                                                           GROUP BY   ISNULL(B.ID_XML,B.ID), 
     C.mahoatchat, 
	 ISNULL(C.TENGOC,C.TENTHUOC),  
	 D.TenDVT,
     C.HamLuong,   
     E.MADUONGDUNG,
				 BNTT.moilanuong+'x'+CONVERT(NVARCHAR,BNTT.ngayuong) +' '+D.TENDVT,
				   C.SODK_THUOC, 
				     C.TT_THAU,
					  A.ISBHYT,
					  B.MA_KHOA,
					   H.mabacsi,
					     ISNULL(J.MAICD,B.MACHANDOAN),  
						  B.MUC_HUONG, 
						   REPLACE( CONVERT(NVARCHAR(20), ISNULL( G.NGAYKHAM,B.NGAYTINHBH_THUC),111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),ISNULL( G.NGAYKHAM,B.NGAYTINHBH_THUC),108),5),':','')    
						    ,ISNULL(C.TiLeTT,'100.00') 
							 ,A.DONGIABH
							   ,C.GIA_MUA 
														                                                          
                                                UNION ALL
                                                           SELECT    MA_LK=ISNULL(B.ID_XML,B.ID),
   STT=1, 
    MA_THUOC=C.madv, 
	MA_NHOM=NHOM_XML.MA_NHOM,
	 TEN_THUOC=C.tendichvu,
	 DON_VI_TINH=D.TenDVT,
	  HAM_LUONG= '', 
	  DUONG_DUNG=N'2.15',
	  LIEU_DUNG=N'Ngày dùng 1 lần',  
	  SO_DANG_KY=  '', 
	   TT_THAU='null',
	   PHAM_VI= 1,
	   SO_LUONG=(A.SOLUONG),
	    DON_GIA= ROUND( ROUND( (A.THANHTIENBH+ISNULL(0,0))/A.SOLUONG,2),2),
		TYLE_TT=100.00,
		THANH_TIEN= (A.THANHTIENBH+ISNULL(0,0)),
		 MUC_HUONG= B.MUC_HUONG,
		  T_NGUONKHAC=0,
		   T_BNTT=ISNULL(0,0), 
		     T_BHTT=ROUND((A.THANHTIENBH*B.MUC_HUONG/100),2), 
			  T_BNCCT= ROUND( (A.THANHTIENBH)-ROUND((A.THANHTIENBH*B.MUC_HUONG/100),2),2),
  T_NGOAIDS=0, 
   MA_KHOA=B.MA_KHOA,  
   MA_BAC_SI=IsNULL(H.mabacsi,'000'),
    MA_BENH=ISNULL(J.MAICD,B.MACHANDOAN),
	NGAY_YL=REPLACE( CONVERT(NVARCHAR(20),ISNULL( G.NGAYKHAM,B.NGAYTINHBH_THUC),111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),ISNULL( G.NGAYKHAM,B.NGAYTINHBH_THUC),108),5),':',''), 
 MA_PTTT='1'
   FROM HS_BENHNHANBHDONGTIEN B 
    inner join benhnhan BN on B.idbenhnhan=BN.IDBENHNHAN  
	 INNER JOIN khambenhcanlamsan_HS  A ON A.IDBENHBHDONGTIEN_HS=B.ID 
	  LEFT JOIN BANGGIADICHVU C ON A.IDCANLAMSAN=C.IDBANGGIADICHVU
	   LEFT JOIN KB_DONVITINH_DV D ON ISNULL(C.IDDVT,1)=D.IDDVT
	   LEFT JOIN khambenh G ON A.IDKHAMBENH=G.idkhambenh
	   LEFT JOIN BACSI H ON H.IDBACSI=(CASE WHEN ISNULL(G.idbacsi,0)=0 THEN G.idbacsi ELSE G.idbacsi END)
LEFT JOIN PHONGKHAMBENH I ON G.IDPHONGKHAMBENH=I.IDPHONGKHAMBENH
 LEFT JOIN CHANDOANICD J ON CONVERT(VARCHAR, J.IDICD)=G.KETLUAN  
  LEFT JOIN PHONGKHAMBENH NHOM ON C.IDPHONGKHAMBENH=NHOM.IDPHONGKHAMBENH  
   LEFT JOIN HS_NhomINBV ON C.IDNHOMINBV=HS_NhomINBV.IdNhom 
   LEFT JOIN NHOM_XML ON HS_NhomINBV.IDNHOMXML=NHOM_XML.IDNHOM 
                                                            WHERE B.IsBHYT=1
                                                            AND A.IsBHYT=1
                                                            AND ISNULL( A.SOLUONG,0)>0
                                                            AND ISNULL(B.IDKHAMBENH_LAST,0)<>0
                                                            AND ISNULL(B.ISCHECK_ALL,0)=1
                                                            AND C.idnhominbv=8
                                                            AND A.DonGiaBH>0 AND A.SOLUONG>0
                                                            " + (str1 == null || str1 == "" ? @"
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)>=@fromdate 
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)<=@ToDate
                                                            " : "") + @"
                                                            " + (str1 == null || !(str1 != "") ? "" : " AND ISNULL(B.ID_XML,B.ID)='" + str1 + "'") + @"
                                                            " + (str2 == null || !(str2 != "") ? "" : " AND BN.TENBENHNHAN LIKE N'%" + str2 + "%'") + @"  
                                                            " + (str3 == null || !(str3 != "") ? "" : " AND BN.MABENHNHAN=N'" + str3 + "'") + "  ";
            DataTable table = DataAcess.Connect.GetTable(sql);
            for (int index = 0; index < table.Rows.Count; ++index)
              table.Rows[index]["STT"] = (object)(index + 1);
            return table;
        }

        private DataTable dtSource_B3()
        {
            string str1 = this.txtMA_LK.Text.Trim();
            string str2 = this.txtTenBN.Text.Trim();
            string str3 = this.txtMaBN.Text.Trim();
            DataTable table1 = DataAcess.Connect.GetTable(@"declare @fromdate as datetime
                                                            set @fromdate='" + this.dtpFromDate.Value.ToString("yyyy/MM/dd") + @"'
                                                            declare @todate as datetime
                                                            set @todate='" + this.dtpToDate.Value.ToString("yyyy/MM/dd") + @" 23:59:59'
                                                            SELECT
                                                            MA_LK=ISNULL(B.ID_XML,B.ID),
                                                            STT=1,
                                                            MA_DICH_VU=ISNULL(C.madv,'0000'),
                                                            MA_VAT_TU=NULL,
                                                            MA_NHOM=NHOM_XML.MA_NHOM,
                                                            GOI_VTYT='',
                                                            TEN_VAT_TU='',
                                                            TEN_DICH_VU=c.tendichvu,
                                                            DON_VI_TINH=D.TenDVT,
                                                            PHAM_VI=1,
                                                            SO_LUONG=A.SOLUONG,
                                                            DON_GIA=ROUND( A.THANHTIENBH*100/(A.SOLUONG*(CASE WHEN A.per50=1 THEN 50.00 ELSE (CASE WHEN A.per80=1 THEN 80.00 ELSE 100.00 END) END)),0),
                                                            TT_THAU='null',
                                                            TYLE_TT= (CASE WHEN A.per50=1 THEN 50.00 ELSE (CASE WHEN A.per80=1 THEN 80.00 ELSE 100.00 END) END),
                                                            THANH_TIEN= A.THANHTIENBH,
                                                            T_TRANTT=0,
                                                            MUC_HUONG=B.MUC_HUONG,
                                                            T_NGUONKHAC=0,
                                                            T_BNTT=0,
                                                            T_BHTT=ROUND(A.SOLUONG*A.DonGiaBH*B.MUC_HUONG/100,2),
                                                            T_BNCCT=ROUND( A.SOLUONG*A.DonGiaBH-ROUND(A.SOLUONG*A.DonGiaBH*B.MUC_HUONG/100,2),2),
                                                            T_NGOAIDS=0,
                                                            MA_KHOA=B.MA_KHOA,
                                                            MA_GIUONG='',
                                                            MA_BAC_SI=ISNULL(H.mabacsi,'000'),
                                                            MA_BENH=ISNULL(J.MAICD,B.MACHANDOAN),
                                                            NGAY_YL =REPLACE( CONVERT(NVARCHAR(20), G.ngaykham,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),G.ngaykham,108),5),':',''),
                                                            NGAY_KQ= REPLACE( CONVERT(NVARCHAR(20), A.ngaythu,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),A.ngaythu,108),5),':',''),
                                                            MA_PTTT='1', loaicp=2
                                                            FROM HS_BENHNHANBHDONGTIEN B
                                                            inner join benhnhan BN on B.idbenhnhan=BN.IDBENHNHAN
                                                            INNER JOIN khambenhcanlamsan_HS  A ON A.IDBENHBHDONGTIEN_HS=B.ID
                                                            LEFT JOIN BANGGIADICHVU C ON A.IDCANLAMSAN=C.IDBANGGIADICHVU
                                                            LEFT JOIN KB_DONVITINH_DV D ON ISNULL(C.IDDVT,1)=D.IDDVT
                                                            LEFT JOIN khambenh G ON A.IDKHAMBENH=G.idkhambenh
                                                            LEFT JOIN BACSI H ON H.IDBACSI=(CASE WHEN ISNULL(G.idbacsi,0)=0 THEN G.idbacsi ELSE G.idbacsi END)
                                                            LEFT JOIN PHONGKHAMBENH I ON G.IDPHONGKHAMBENH=I.IDPHONGKHAMBENH
                                                            LEFT JOIN CHANDOANICD J ON CONVERT(VARCHAR, J.IDICD)=G.KETLUAN
                                                            LEFT JOIN PHONGKHAMBENH NHOM ON C.IDPHONGKHAMBENH=NHOM.IDPHONGKHAMBENH 
                                                            LEFT JOIN HS_NhomINBV ON C.IDNHOMINBV=HS_NhomINBV.IdNhom
                                                            LEFT JOIN NHOM_XML ON HS_NhomINBV.IDNHOMXML=NHOM_XML.IDNHOM 
                                                            WHERE B.IsBHYT=1
                                                            AND A.IsBHYT=1
                                                            AND ISNULL( A.SOLUONG,0)>0
                                                            AND ISNULL(B.IDKHAMBENH_LAST,0)<>0
                                                            AND ISNULL(B.ISCHECK_ALL,0)=1
                                                            AND C.idnhominbv<>8
                                                            " + (str1 == null || str1 == "" ? @"
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)>=@fromdate
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)<=@ToDate
                                                            " : "") + @"
                                                            " + (str1 == null || !(str1 != "") ? "" : " AND ISNULL(B.ID_XML,B.ID)='" + str1 + "'") + @"  
                                                            " + (str2 == null || !(str2 != "") ? "" : " AND BN.TENBENHNHAN LIKE N'%" + str2 + "%'") + @"   
                                                            " + (str3 == null || !(str3 != "") ? "" : " AND BN.MABENHNHAN=N'" + str3 + "'") + @"
                                                   UNION ALL
                                                            SELECT 
                                                            DISTINCT  MA_LK=ISNULL(B.ID_XML,B.ID),
                                                            STT=1,
                                                            MA_DICH_VU='',
                                                            MA_VAT_TU=(CASE WHEN ISNULL(C.MA_NHOM_VTYT,'')<>'' THEN ISNULL(C.MA_NHOM_VTYT,'') ELSE '000' END), 
                                                            MA_NHOM=( CASE WHEN ISNULL(C.tengoc,C.TENTHUOC) like N'%thủy tinh%' THEN '11' ELSE  '10' END),
                                                            GOI_VTYT=C.GOI_VTYT,
                                                            TEN_VAT_TU=ISNULL(C.tengoc,C.TENTHUOC),
                                                            TEN_DICH_VU='',
                                                            DON_VI_TINH=D.TenDVT,
                                                            PHAM_VI=1,
                                                            SO_LUONG=SUM(A.SOLUONG),
                                                            DON_GIA=A.DONGIABH,
                                                            TT_THAU= (CASE WHEN ISNULL(C.TT_THAU,'')<>'' THEN C.TT_THAU ELSE '2017.01.1' end), 
                                                            TYLE_TT=100,
                                                            THANH_TIEN=SUM( A.SOLUONG*A.DONGIABH),
                                                            T_TRANTT=0,
                                                            MUC_HUONG=B.MUC_HUONG,
                                                            T_NGUONKHAC=0,
                                                            T_BNTT=SUM(0),
                                                            T_BHTT=ROUND( SUM(A.SOLUONG*A.DonGiaBH*B.MUC_HUONG/100),2),
                                                            T_BNCCT=ROUND( SUM( A.SOLUONG*A.DonGiaBH)-ROUND( SUM(A.SOLUONG*A.DonGiaBH*B.MUC_HUONG/100),2),2),
                                                            T_NGOAIDS=0,
                                                            MA_KHOA=B.MA_KHOA,
                                                            MA_GIUONG='',
                                                            MA_BAC_SI=ISNULL(H.mabacsi,'000'),
                                                            MA_BENH=ISNULL(J.MAICD,B.MACHANDOAN),
                                                            NGAY_YL =REPLACE( CONVERT(NVARCHAR(20), G.ngaykham,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),G.ngaykham,108),5),':',''),
                                                            NGAY_KQ= REPLACE( CONVERT(NVARCHAR(20), G.ngaykham,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),G.ngaykham,108),5),':',''),
                                                            MA_PTTT='1', loaicp=3
                                                            FROM HS_BENHNHANBHDONGTIEN B
                                                            inner join benhnhan BN on B.idbenhnhan=BN.IDBENHNHAN
                                                            INNER JOIN CHITIETPHIEUXUATKHO_HS A ON A.IDBENHBHDONGTIEN_HS=B.ID
                                                            LEFT JOIN THUOC C ON A.IDTHUOC=C.IDTHUOC
                                                            LEFT JOIN Thuoc_DonViTinh D ON C.iddvt=D.Id
                                                            LEFT JOIN khambenh G ON A.IDKHAMBENH1=G.idkhambenh
                                                            LEFT JOIN BACSI H ON H.IDBACSI=(CASE WHEN ISNULL(G.idbacsi,0)=0 THEN G.idbacsi ELSE G.idbacsi END)
                                                            LEFT JOIN PHONGKHAMBENH I ON G.IDPHONGKHAMBENH=I.IDPHONGKHAMBENH
                                                            LEFT JOIN CHANDOANICD J ON CONVERT(VARCHAR, J.IDICD)=G.KETLUAN
                                                            WHERE B.IsBHYT=1 
                                                            AND A.IsBHYT=1 
                                                            AND ISNULL(B.IDKHAMBENH_LAST,0)<>0
                                                            AND ISNULL(B.ISCHECK_ALL,0)=1
                                                            " + (str1 == null || str1 == "" ? @"
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)>=@fromdate
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)<=@ToDate  
                                                            " : "") + @"
                                                            AND C.LOAITHUOCID <>1
                                                            " + (str1 == null || !(str1 != "") ? "" : " AND ISNULL(B.ID_XML,B.ID)='" + str1 + "'") + @"  
                                                            " + (str2 == null || !(str2 != "") ? "" : " AND BN.TENBENHNHAN LIKE N'%" + str2 + "%'") + @"   
                                                            " + (str3 == null || !(str3 != "") ? "" : " AND BN.MABENHNHAN=N'" + str3 + "'") + @"
                                                            GROUP BY
                                                            ISNULL(B.ID_XML,B.ID),
                                                            C.MA_NHOM_VTYT,
                                                            C.GOI_VTYT,
                                                            ISNULL(C.tengoc,C.TENTHUOC),
                                                            D.TenDVT,
                                                            A.ISBHYT,
                                                            A.DONGIABH,
                                                            A.DONGIABH,
                                                            C.TT_THAU,
                                                            C.T_TRANTT,
                                                            B.MA_KHOA,
                                                            H.MABACSI,
                                                            ISNULL(J.MAICD,B.MACHANDOAN),
                                                            REPLACE( CONVERT(NVARCHAR(20), G.ngaykham,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),G.ngaykham,108),5),':',''),
                                                            B.MUC_HUONG
                                                   UNION ALL
                                                            SELECT 
                                                            MA_LK=ISNULL(B.ID_XML,B.ID),
                                                            STT=1,
                                                            MA_DICH_VU=BG.MADV  ,
                                                            MA_VAT_TU='',
                                                            MA_NHOM='13',
                                                            GOI_VTYT='',
                                                            TEN_VAT_TU='',
                                                            TEN_DICH_VU=ISNULL(BG. TENBAOHIEM,BG.TENDICHVU ),
                                                            DON_VI_TINH=N'Lần',
                                                            PHAM_VI=1,
                                                            SO_LUONG=CTDKK.SOLUONG,
                                                            DON_GIA=(CASE WHEN CTDKK.ISBHYT=1 THEN  (CASE WHEN CTDKK.DONGIABH IN (8700.0,9300.0,7890.0) THEN ROUND(CTDKK.DONGIABH/0.3,0) ELSE CTDKK.DONGIABH END) ELSE 0 END)  ,
                                                            TT_THAU='null',
                                                            TYLE_TT=(CASE WHEN CTDKK.ISBHYT=1 THEN (CASE WHEN CTDKK.DONGIABH IN (8700.0,9300.0,7890.0) THEN 30 ELSE 100 END) ELSE 0 END)  , 
                                                            THANH_TIEN= CTDKK.THANHTIENBH ,
                                                            T_TRANTT=NULL,
                                                            MUC_HUONG=B.MUC_HUONG,
                                                            T_NGUONKHAC=0,
                                                            T_BNTT=0,
                                                            T_BHTT=ROUND(CTDKK.THANHTIENBH*B.MUC_HUONG/100,2),
                                                            T_BNCCT=ROUND( CTDKK.THANHTIENBH - ROUND(CTDKK.THANHTIENBH*B.MUC_HUONG/100,2),2),
                                                            T_NGOAIDS=0,
                                                            MA_KHOA=B.MA_KHOA,
                                                            MA_GIUONG='',
                                                            MA_BAC_SI= ISNULL(BS.MABACSI,'000'),
                                                            MA_BENH=  ISNULL(CD.MAICD,''),
                                                            NGAY_YL= REPLACE( CONVERT(NVARCHAR(20), KB.ngaykham,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),KB.ngaykham,108),5),':',''),
                                                            NGAY_KQ=REPLACE( CONVERT(NVARCHAR(20), KB.ngaykham,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),KB.ngaykham,108),5),':',''),
                                                            MA_PTTT='1', loaicp=1
                                                            FROM chitietdangkykham_HS CTDKK                                                   
                                                            LEFT JOIN DANGKYKHAM DKK ON CTDKK.IDDANGKYKHAM=DKK.IDDANGKYKHAM                                                    
                                                            LEFT JOIN BANGGIADICHVU BG ON BG.IDBANGGIADICHVU= CTDKK.IDBANGGIADICHVU                                                  
                                                            LEFT JOIN PHONGKHAMBENH PKB ON ISNULL( BG.IDPHONGKHAMBENH,CTDKK.IDKHOA)=PKB.IDPHONGKHAMBENH  
                                                            INNER JOIN HS_BENHNHANBHDONGTIEN B ON  CTDKK.IDBENHBHDONGTIEN=B.ID
                                                            INNER JOIN BENHNHAN BN ON B.IDBENHNHAN=BN.IDBENHNHAN
                                                            LEFT JOIN KHAMBENH KB ON KB.IDKHAMBENH= CTDKK.IdKhamBenhCD
                                                            LEFT JOIN PHONGKHAMBENH KHOA ON KHOA.IDPHONGKHAMBENH=KB.IDPHONGKHAMBENH
                                                            LEFT JOIN BACSI BS ON KB.IDBACSI=BS.IDBACSI
                                                            LEFT JOIN CHANDOANICD CD ON CONVERT(VARCHAR, CD.IDICD)=KB.KETLUAN
                                                            WHERE  1=1
                                                            AND B.ISBHYT=1
                                                            AND CTDKK.ISBHYT=1
                                                            AND CTDKK.DONGIABH>0
                                                            AND ISNULL(B.ISCHECK_ALL,0)=1
                                                            " + (str1 == null || str1 == "" ? @"
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)>=@fromdate
                                                            AND ISNULL(B.XML_DATE,B.NGAYTINHBH_THUC)<=@ToDate
                                                            " : "") + @"
                                                            " + (str1 == null || !(str1 != "") ? "" : " AND ISNULL(B.ID_XML,B.ID)='" + str1 + "'") + @"  
                                                            " + (str2 == null || !(str2 != "") ? "" : " AND BN.TENBENHNHAN LIKE N'%" + str2 + "%'") + @"   
                                                            " + (str3 == null || !(str3 != "") ? "" : " AND BN.MABENHNHAN=N'" + str3 + "'") + @"
                                                  UNION ALL
                                                            SELECT 
                                                            MA_LK=ISNULL(HS.ID_XML,HS.ID),
                                                            STT=1,
                                                            MA_DICH_VU=TG.MAGIUONG,
                                                            MA_VAT_TU='',
                                                            MA_NHOM=(CASE WHEN HS.ISNOITRU=1 THEN '15' ELSE '14' END),
                                                            GOI_VTYT='',
                                                            TEN_VAT_TU='',
                                                            TEN_DICH_VU=LG.TENLOAIGIUONG,
                                                            DON_VI_TINH=N'Ngày',
                                                            PHAM_VI=1,
                                                            SO_LUONG=G.SL,
                                                            DON_GIA=G.DONGIABH,
                                                            TT_THAU='null',
                                                            TYLE_TT=100.00,
                                                            THANH_TIEN=G.THANHTIENBH,
                                                            T_TRANTT=NULL,
                                                            MUC_HUONG=HS.MUC_HUONG,
                                                            T_NGUONKHAC=0,
                                                            T_BNTT=0,
                                                            T_BHTT=ROUND( G.THANHTIENBH*HS.MUC_HUONG/100,2),
                                                            T_BNCCT=ROUND( G.THANHTIENBH-ROUND( G.THANHTIENBH*HS.MUC_HUONG/100,2),2),
                                                            T_NGOAIDS=0,
                                                            MA_KHOA=PKB.MAPHONGKHAMBENH,
                                                            MA_GIUONG=G0.MA_GIUONG,
                                                            MA_BAC_SI=ISNULL(BS.MABACSI,'000'),
                                                            MA_BENH=hs.MACHANDOAN,
                                                            NGAY_YL =REPLACE( CONVERT(NVARCHAR(20), G.TuNgay,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),G.TuNgay,108),5),':',''),
                                                            NGAY_KQ= REPLACE( CONVERT(NVARCHAR(20), hs.NgayTinhBH_Thuc,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),hs.NgayTinhBH_Thuc,108),5),':',''),
                                                            MA_PTTT='1', loaicp=4
                                                            FROM KB_CHITIETGIUONGBN_HS G
                                                            LEFT JOIN KB_CHITIETGIUONGBN G0 ON G.IdChiTietGiuongBN=G0.IdChiTietGiuongBN
                                                            LEFT JOIN KB_GIUONG TG ON TG.GIUONGID = G0.IDGIUONG
                                                            LEFT JOIN KB_PHONG P ON TG.IDPHONG= P.ID
                                                            LEFT JOIN HS_LOAIGIUONG LG ON TG.IDLOAIGIUONG=LG.IDLOAIGIUONG
                                                            LEFT JOIN PHONGKHAMBENH F0 ON G.IDKHOA=F0.idphongkhambenh
                                                            LEFT JOIN PHONGKHAMBENH PKB ON ISNULL(F0.Parrent,F0.IDPHONGKHAMBENH)=PKB.IDPHONGKHAMBENH
                                                            INNER JOIN HS_BenhNhanBHDongTien HS ON G.IDBENHBHDONGTIEN_HS=HS.ID
                                                            inner join benhnhan BN on HS.idbenhnhan=BN.IDBENHNHAN
                                                            LEFT JOIN BACSI BS ON G0.IDBACSI=BS.IDBACSI   
                                                            WHERE hs.IsBHYT=1 
                                                            AND G.IsBHYT=1 
                                                            AND ISNULL(HS.ISCHECK_ALL,0)=1
                                                            " + (str1 == null || str1 == "" ? @"
                                                            AND ISNULL(HS.XML_DATE,HS.NGAYTINHBH_THUC)>=@fromdate
                                                            AND ISNULL(HS.XML_DATE,HS.NGAYTINHBH_THUC)<=@todate
                                                            " : "") + @"
                                                            " + (str1 == null || !(str1 != "") ? "" : " AND ISNULL(HS.ID_XML,HS.ID)='" + str1 + "'") + @"   
                                                            " + (str2 == null || !(str2 != "") ? "" : " AND BN.TENBENHNHAN LIKE N'%" + str2 + "%'") + @"   
                                                            " + (str3 == null || !(str3 != "") ? "" : " AND BN.MABENHNHAN=N'" + str3 + "'") + "");
            table1.DefaultView.Sort = "MA_NHOM,TEN_DICH_VU";
            DataTable table2 = table1.DefaultView.ToTable();
            for (int index = 0; index < table2.Rows.Count; ++index)
                table2.Rows[index]["STT"] = (object)(index + 1);
            return table2;
        }

        private DataTable dtXetNghiem()
        {
            string str1 = this.txtMA_LK.Text.Trim();
            string str2 = this.txtTenBN.Text.Trim();
            string str3 = this.txtMaBN.Text.Trim();
            string str4 = "";
            DateTime dateTime;
            if (str1 != "" || str2 != "" || str3 != "")
            {
                string[] strArray1 = new string[9];
                strArray1[0] = @"declare @fromdate as datetime
                                set @fromdate='";
                strArray1[1] = this.dtpFromDate.Value.ToString("yyyy/MM/dd");
                strArray1[2] = @"'declare @todate as datetime
                                    set @todate='";
                string[] strArray2 = strArray1;
                int index1 = 3;
                dateTime = this.dtpToDate.Value;
                string str5 = dateTime.ToString("yyyy/MM/dd");
                strArray2[index1] = str5;
                strArray1[4] = @" 23:59:59'  SELECT DISTINCT A.MAPHIEUCLS
                                            FROM KHAMBENHCANLAMSAN A
                                            INNER JOIN KHAMBENH B ON A.IDKHAMBENH=B.IDKHAMBENH
                                            INNER JOIN DANGKYKHAM C ON B.IDDANGKYKHAM=C.IDDANGKYKHAM
                                            INNER JOIN BENHNHAN D ON B.IDBENHNHAN=D.IDBENHNHAN
                                            INNER JOIN HS_BENHNHANBHDONGTIEN E  ON E.ID=C.IDBENHBHDONGTIEN
                                            WHERE
                                            C.LOAIKHAMID=1
                                            AND ISNULL(A.DAHUY,0)=0
                                            AND E.ISBHYT=1
                                            AND E.ISCHECK_ALL=1
                                            AND E.NGAYTINHBH_THUC>=@fromdate
                                            AND E.NGAYTINHBH_THUC<=@todate";
                strArray1[5] = str1 != "" ? " AND C.IDBENHBHDONGTIEN=" + str1 : "";
                strArray1[6] = str2 != "" ? "AND D.TENBENHNHAN LIKE N'" + str2 + "'" : "";
                strArray1[7] = str3 != "" ? " AND D.MABENHNHAN LIKE N'" + str3 + "'" : "";
                strArray1[8] = "\r\n                                ";
                DataTable table = DataAcess.Connect.GetTable(string.Concat(strArray1));
                if (table == null || table.Rows.Count <= 0)
                    return (DataTable)null;
                for (int index2 = 0; index2 < table.Rows.Count; ++index2)
                    str4 = str4 + "'" + table.Rows[0][0].ToString() + "',";
                str4 = str4.Remove(str4.Length - 1, 1);
            }
            string[] strArray3 = new string[9];
            strArray3[0] = "\r\n                                    declare @fromdate as datetime\r\n                                    set @fromdate='";
            string[] strArray4 = strArray3;
            int index3 = 1;
            dateTime = this.dtpFromDate.Value;
            string str6 = dateTime.ToString("yyyy/MM/dd");
            strArray4[index3] = str6;
            strArray3[2] = "'\r\n                                    declare @todate as datetime\r\n                                    set @todate='";
            string[] strArray5 = strArray3;
            int index4 = 3;
            dateTime = this.dtpToDate.Value;
            string str7 = dateTime.ToString("yyyy/MM/dd");
            strArray5[index4] = str7;
            strArray3[4] = " 23:59:59'\r\n\r\n\r\n\r\n\r\n                                        select   \r\n                                        Datein as ngayxn,  \r\n                                        SID,  \r\n                                        OrderID as sophieu,  \r\n                                        PID as mabenhnhan,  \r\n                                        PatientName as tenbenhnhan,  \r\n                                        age as namsinh,  \r\n                                        sex as gioitinh,  \r\n                                        address as diachi,  \r\n                                        objectname as doituong,  \r\n                                        doctorname as bacsi,  \r\n                                        locationname as khoahong,  \r\n                                        Printtime as thoigianin,  \r\n                                        TestcodeHIS as maxnhis,  \r\n                                        testcode as maxetnghiem,  \r\n                                        TestName as tenxetnghiem,  \r\n                                        Result as ketqua,  \r\n                                        NormalRange as chisobinhthuong,  \r\n                                        unit as donvi,  \r\n                                        batthuong,  \r\n                                        Bold as indam,  \r\n                                        Testhead as xnchinh,  \r\n                                        category as manhomxn,  \r\n                                        CategoryName as tennhomxn,  \r\n                                        PrintOrder as thutuinnhom  \r\n                                        from(  \r\n                                        select p.Datein, p.SID,p.OrderID,p.PID,p.PatientName,p.Age,p.Sex,p.Address,o.Objectname,isnull(p.Pdoctorname,d.doctorname) as DoctorName,l.locationname,  \r\n                                        p.Printtime,r.TestCode,r.TestcodeHIS,r.TestName,r.Result,r.NormalRange,r.unit,  \r\n                                        case when r.color>0 then 1 else 0 end as batthuong,r.Bold,r.Testhead,  \r\n                                        r.category,c.CategoryName,r.PrintOrder  \r\n                                        from tbl_patient p inner join tbl_Result r on p.sid=r.sid  \r\n                                        inner join tbl_object o on p.objectid=o.objectid  \r\n                                        inner join tbl_category c on r.category=c.categoryid  \r\n                                        left join tbl_doctor d on p.doctorid=d.doctorid  \r\n                                        left join tbl_location l on p.locationid=l.locationid  \r\n                                        where 1=1  \r\n                                           AND p.datein>=@FROMDATE\r\n                                           AND p.datein <=@TODATE  \r\n                                        ";
            strArray3[5] = str4 == null || !(str4 != "") ? "" : " and p.OrderID IN (" + str4 + ")";
            strArray3[6] = "\r\n                                        union all  \r\n                                        select p.Datein, p.SID,p.OrderID,p.PID,p.PatientName,p.Age,p.Sex,p.Address,o.Objectname,isnull(p.Pdoctorname,d.doctorname) as DoctorName,l.locationname,  \r\n                                        p.Printtime,r.TestCode,r.TestcodeHIS,r.TestName,r.Result,r.NormalRange,r.unit,  \r\n                                        case when r.color>0 then 1 else 0 end as batthuong,r.Bold,r.Testhead,  \r\n                                        r.category,c.CategoryName,r.PrintOrder  \r\n                                        from tbl_patient_his p inner join tbl_Result_his r on p.sid=r.sid  \r\n                                        inner join tbl_object o on p.objectid=o.objectid  \r\n                                        inner join tbl_category c on r.category=c.categoryid  \r\n                                        left join tbl_doctor d on p.doctorid=d.doctorid  \r\n                                        left join tbl_location l on p.locationid=l.locationid  \r\n                                        where 1=1  \r\n                                           AND p.datein>=@FROMDATE\r\n                                           AND p.datein <=@TODATE\r\n                                        ";
            strArray3[7] = str4 == null || !(str4 != "") ? "" : " and p.OrderID IN (" + str4 + ")";
            strArray3[8] = "\r\n                                        ) as T  \r\n                    ";
            DataTable table1 = DataAcess.Connect.GetTable(string.Concat(strArray3));
            if (table1 == null || table1.Rows.Count == 0)
                return (DataTable)null;
            string str8 = "";
            for (int index1 = 0; index1 < table1.Rows.Count; ++index1)
                str8 = str8 + "'" + table1.Rows[index1]["SoPhieu"].ToString() + "',";
            DataTable table2 = DataAcess.Connect.GetTable("\r\n                        SELECT DISTINCT SoPhieu=A.MAPHIEUCLS,MA_LK=E.ID\r\n                         FROM KHAMBENHCANLAMSAN A\r\n                        INNER JOIN KHAMBENH B ON A.IDKHAMBENH=B.IDKHAMBENH\r\n                        INNER JOIN DANGKYKHAM C ON B.IDDANGKYKHAM=C.IDDANGKYKHAM\r\n                        INNER JOIN BENHNHAN D ON B.IDBENHNHAN=D.IDBENHNHAN\r\n                        INNER JOIN HS_BENHNHANBHDONGTIEN E  ON E.ID=C.IDBENHBHDONGTIEN\r\n                        WHERE \r\n                         A.MAPHIEUCLS IN (" + str8.Remove(str8.Length - 1, 1) + ")");
            if (table2 == null || table2.Rows.Count == 0)
                return (DataTable)null;
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ma_lk");
            dataTable.Columns.Add("stt");
            dataTable.Columns.Add("ma_dich_vu");
            dataTable.Columns.Add("ma_chi_so");
            dataTable.Columns.Add("ten_chi_so");
            dataTable.Columns.Add("gia_tri");
            dataTable.Columns.Add("ma_may");
            dataTable.Columns.Add("mo_ta");
            dataTable.Columns.Add("ket_luan");
            dataTable.Columns.Add("ngay_kq");
            for (int index1 = 0; index1 < table1.Rows.Count; ++index1)
            {
                int index2 = hs_tinhtien.int_Search(table2, "SoPhieu='" + table1.Rows[index1]["SoPhieu"].ToString() + "'");
                if (index2 != -1)
                {
                    DataRow row = dataTable.NewRow();
                    row["ma_lk"] = table2.Rows[index2]["ma_lk"];
                    row["stt"] = (object)(dataTable.Rows.Count + 1).ToString();
                    row["ma_dich_vu"] = (object)table1.Rows[index1]["tenxetnghiem"].ToString();
                    row["ma_chi_so"] = (object)table1.Rows[index1]["tenxetnghiem"].ToString();
                    row["ten_chi_so"] = (object)table1.Rows[index1]["tenxetnghiem"].ToString();
                    row["gia_tri"] = (object)table1.Rows[index1]["ketqua"].ToString();
                    DataRow dataRow = row;
                    string index5 = "ngay_kq";
                    dateTime = DateTime.Parse(table1.Rows[index1]["ngayxn"].ToString());
                    string str5 = dateTime.ToString("yyyyMMddHHmm");
                    dataRow[index5] = (object)str5;
                    dataTable.Rows.Add(row);
                }
            }
            return dataTable;
        }

        private DataTable dtSource_B4()
        {
            string str1 = this.txtMA_LK.Text.Trim();
            string str2 = this.txtTenBN.Text.Trim();
            string str3 = this.txtMaBN.Text.Trim();
            return DataAcess.Connect.GetTable("\r\n\r\n                                    declare @fromdate as datetime\r\n                                    set @fromdate='" + this.dtpFromDate.Value.ToString("yyyy/MM/dd") + "'\r\n                                    declare @todate as datetime\r\n                                    set @todate='" + this.dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59'\r\n\r\n\t\t\t\t\t\t \r\n\r\n                                    SELECT  \r\n                                     ma_lk=ISNULL(F.ID_XML,F.ID)\r\n                                     ,stt=CONVERT(NVARCHAR(500),NULL)\r\n                                     ,ma_dich_vu=b.tendichvu\r\n                                     ,ma_chi_so=CONVERT(NVARCHAR(500),NULL)\r\n                                     ,ten_chi_so=CONVERT(NVARCHAR(500),NULL)\r\n                                     ,gia_tri=CONVERT(NVARCHAR(500),NULL)\r\n                                     ,ma_may=CONVERT(NVARCHAR(500),NULL)\r\n                                     ,mo_ta=A.Mota_Ketqua\r\n                                     ,ket_luan=A.ketqua\r\n                                    ,ngay_kq= REPLACE( CONVERT(NVARCHAR(20), isnull(A.NgayChupSieuAm, f.ngaytinhbh_thuc),111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),isnull(A.NgayChupSieuAm, f.ngaytinhbh_thuc),108),5),':','')\r\n                                    FROM HS_KETQUASIEUAM A\r\n                                    INNER JOIN HS_KETQUASIEUAMCHITIET B ON A.KETQUASIEUAMID=B.KETQUASIEUAMID\r\n                                    INNER JOIN KHAMBENHCANLAMSAN C ON A.IDKHAMBENHCANLAMSAN=C.IDKHAMBENHCANLAMSAN\r\n                                    INNER JOIN KHAMBENH D ON D.IDKHAMBENH=C.IDKHAMBENH\r\n                                    INNER JOIN DANGKYKHAM E ON D.IDDANGKYKHAM=E.IDDANGKYKHAM\r\n                                    INNER JOIN HS_BENHNHANBHDONGTIEN F ON E.IDBENHBHDONGTIEN=F.ID\r\n                                    INNER JOIN BENHNHAN BN ON C.IDBENHNHAN=BN.IDBENHNHAN\r\n                                  WHERE F.IsBHYT=1 \r\n                                                    AND ISNULL(F.ISCHECK_ALL,0)=1\r\n                            " + (str1 == null || str1 == "" ? "\r\n                                            AND ISNULL(F.XML_DATE,F.NGAYTINHBH_THUC)>=@fromdate\r\n                                            AND ISNULL(F.XML_DATE,F.NGAYTINHBH_THUC)<=@ToDate\r\n                             " : "") + "\r\n                " + (str1 == null || !(str1 != "") ? "" : " AND ISNULL(F.ID_XML,F.ID)='" + str1 + "'") + "    \r\n                " + (str2 == null || !(str2 != "") ? "" : " AND BN.TENBENHNHAN LIKE N'%" + str2 + "%'") + "    \r\n                " + (str3 == null || !(str3 != "") ? "" : " AND BN.MABENHNHAN=N'" + str3 + "'") + "\r\n\r\n                                    ");
        }

        private DataTable dtSource_B5()
        {
            this.txtMA_LK.Text.Trim();
            this.txtTenBN.Text.Trim();
            this.txtMaBN.Text.Trim();
            return DataAcess.Connect.GetTable("\r\n\r\n                                    declare @fromdate as datetime\r\n                                    set @fromdate='" + this.dtpFromDate.Value.ToString("yyyy/MM/dd") + "'\r\n                                    declare @todate as datetime\r\n                                    set @todate='" + this.dtpToDate.Value.ToString("yyyy/MM/dd") + " 23:59:59'\r\n\t\t\t\t\t\t\r\n                                    SELECT MA_LK=ISNULL(C.ID_XML,C.ID)\r\n\t                                       ,STT=1\r\n\t                                       ,DIEN_BIEN=A.TRIEUCHUNG\r\n\t                                       ,HOI_CHAN=NULL\r\n\t                                       ,PHAU_THUAT=A.phuongphapphauthuat\r\n\t                                       ,NGAY_YL=REPLACE( CONVERT(NVARCHAR(20), A.ngaykham,111),'/','') +REPLACE( LEFT( CONVERT(VARCHAR(20),A.ngaykham,108),5),':','')\r\n                                    FROM KHAMBENH A\r\n                                    INNER JOIN DANGKYKHAM B ON A.IDDANGKYKHAM=B.IDDANGKYKHAM\r\n                                    INNER JOIN HS_BENHNHANBHDONGTIEN C ON B.IDBENHBHDONGTIEN=C.ID\r\n                                     WHERE C.IsBHYT=1 \r\n                                                    AND C.ISBHYT=1\r\n                                                    and B.loaikhamid=1\r\n                                                    AND C.NGAYTINHBH>=@fromdate\r\n                                                    AND C.NGAYTINHBH_THUC<=@todate\r\n                                                    AND  A.TRIEUCHUNG IS NOT NULL\r\n                                    ");
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
                    str = str + strArray[index] + ";";
                }
            }
            if (str != "")
                str = str.Remove(str.Length - 1, 1);
            return str;
        }

        private void btnSavetoDB_Click(object sender, EventArgs e)
        {
            string path = this.txtPath.Text.Trim();
            string NumberRow = "";
            this.SaveToPath(path, ref NumberRow, true);
            int num = (int)MessageBox.Show(NumberRow.ToString() + " dòng thành công");
        }
    }
}
