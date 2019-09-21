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
    public partial class CanLamSan : Form
    {
        public CanLamSan()
        {
            InitializeComponent();
            LoadSLCanLamSang();
            NhomCLS_load();
            Load_CLS(Truyendulieu.idkhambenh);
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
        private void btnTuDK_Click(object sender, EventArgs e)
        {
            CLSTuDK cls = new CLSTuDK();
            cls.Show();
        }

        private void btnCLShen_Click(object sender, EventArgs e)
        {
            HenCLS hen = new HenCLS();
            hen.Show();
        }

        #region Load Cận lâm sàng lên searchLookUpEdit
        public void LoadSLCanLamSang()
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
            slkCLS.Properties.DataSource = dtCLS1;
            slkCLS.Properties.NullText = "Nhập Cận lâm sàng";
            slkCLS.Properties.DisplayMember = "tendichvu";
            slkCLS.Properties.ValueMember = "idbanggiadichvu";
            slkCLS.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkCLS.Properties.ImmediatePopup = true;
            slkCLS.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }
        #endregion

        #region Load CLS theo ID
        public void LoadCLS_theoID()
        {
            #region Load Cận lâm sàng theo IDCLS
            try
            {
                string sql = @"select idbanggiadichvu,tendichvu,giadichvu,bhtra,IsSuDungChoBH,fromdate,IdnhomInBV from banggiadichvu where IsActive=1 and idbanggiadichvu='" + slkCLS.EditValue.ToString() + "'";
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
        #endregion

        private void btnThemCLS_Click(object sender, EventArgs e)
        {
            LoadCLS_theoID();
        }

        #region Button Xóa trên dtgvCLS
        private void dtgvCLS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dtgvCLS.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCLS.Columns[e.ColumnIndex].Name;
                if (command == "XoaCLS")
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
                                        string sql = "delete chitietbenhnhantoathu where idkhambenhcanlamsan='" + dtgvCLS.Rows[r].Cells["IdKBCLS"].Value.ToString() + "'";
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
        #endregion

        #region Tô màu button trên dtgvCLS
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
        #endregion

        public void NhomCLS_load()
        {
            #region Load nhóm cận lâm sàng
            string sql="select NhomId,TenNhom,GhiChu from  KB_NhomCLS";
            DataTable dtNhomCLS = DataAcess.Connect.GetTable(sql);
            slkNhomCLS.Properties.DataSource = dtNhomCLS;
            slkNhomCLS.Properties.NullText = "Nhập Nhóm CLS";
            slkNhomCLS.Properties.DisplayMember = "TenNhom";
            slkNhomCLS.Properties.ValueMember = "NhomId";
            slkNhomCLS.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkNhomCLS.Properties.ImmediatePopup = true;
            slkNhomCLS.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            #endregion
        }

        public void NhomCLS_TheoID_Load()
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
                                                   where T.NhomID ='" + slkNhomCLS.EditValue.ToString() + "'";

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

        private void btnThemNhomCLS_Click(object sender, EventArgs e)
        {
            NhomCLS_TheoID_Load();
        }
    }

}
