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
    public partial class HuongDieuTri : Form
    {
        public HuongDieuTri()
        {
            InitializeComponent();
            Load_CDPH(Truyendulieu.idkhambenh);
            Load_CDXD();
            Load_CDPH();
            Load_Bacsi();
            Load_PhongKham();
            Load_KhoaChuyen();
            Load_Benhvien();
            Load_Ravien();
            
        }

        public void Load_Benhvien()
        {
            string sql = "select idBenhVien,TenBenhVien from BenhVien";
            DataTable dtbenhvien = DataAcess.Connect.GetTable(sql);
            slkChuyenvien.Properties.NullText = "Nhập Khoa chuyển";
            slkChuyenvien.Properties.DataSource = dtbenhvien;
            slkChuyenvien.Properties.DisplayMember = "TenBenhVien";
            slkChuyenvien.Properties.ValueMember = "idBenhVien";
        }
        public void Load_KhoaChuyen()
        {
            string sql = "SELECT idphongkhambenh,tenphongkhambenh FROM dbo.phongkhambenh where loaiphong=0 and maphongkhambenh is not null";
            DataTable dtKhoa = DataAcess.Connect.GetTable(sql);
            slkKhoachuyen.Properties.NullText = "Nhập Khoa chuyển";
            slkKhoachuyen.Properties.DataSource = dtKhoa;
            slkKhoachuyen.Properties.DisplayMember = "tenphongkhambenh";
            slkKhoachuyen.Properties.ValueMember = "idphongkhambenh";
        }
        public void Load_Ravien()
        {

            DataTable dtravien = DataAcess.Connect.GetTable(GetData.dt_BNDaKham2(Truyendulieu.idkhambenh));
            txtSovaovien.Text= dtravien.Rows[0]["SOVAOVIEN1"].ToString();
            slkCDXD.EditValue= dtravien.Rows[0]["ketluan"].ToString();
            txtCDXD.Text= dtravien.Rows[0]["mkv_mota"].ToString();
            slkBacsi2.EditValue= dtravien.Rows[0]["idbacsi2"].ToString();
            if(dtravien.Rows[0]["IsBSMoiKham"].ToString()== "True")
            {
                chkMoiKham.Checked = true;
            }
            else { chkMoiKham.Checked = false; }
 
            slkKhoachuyen.EditValue= dtravien.Rows[0]["IdkhoaChuyen"].ToString();
            slkPhongchuyen.EditValue= dtravien.Rows[0]["idPhongChuyenDen"].ToString();
            slkChuyenvien.EditValue = dtravien.Rows[0]["idbenhvienchuyen"].ToString();
            if (dtravien.Rows[0]["iskhongkham"].ToString()== "True")
            {
                rdKhongkham.Checked = true;
            }
            else { rdKhongkham.Checked = false; }
            if (dtravien.Rows[0]["ischuyenvien"].ToString() == "True")
            {
                rdChuyenvien.Checked = true;
            }
            else { rdChuyenvien.Checked = false; }
            if (dtravien.Rows[0]["ischovekt"].ToString() == "True")
            {
                rdKhongthuoc.Checked = true;
            }
            else { rdKhongthuoc.Checked = false; }
         //  slkChuyenvien.EditValue= dtravien.Rows[0]["mkv_idbenhvienchuyen"].ToString();
            if (dtravien.Rows[0]["isNoiTru"].ToString() == "True")
            {
                rdNoitru.Checked = true;
            }
            else { rdNoitru.Checked = false; }
            if (dtravien.Rows[0]["isNgoaiTru"].ToString() == "True")
            {
                rdNgoaitru.Checked = true;
            }
            else { rdNgoaitru.Checked = false; }
            txtNgayxuatkhoa.Text = dtravien.Rows[0]["TGXuatVien"].ToString();
            txtGiorv.Text = dtravien.Rows[0]["gioravien"].ToString();
            txtPhutrv.Text= dtravien.Rows[0]["phutravien"].ToString();
            if(dtravien.Rows[0]["IsXuatVien"].ToString()=="True")
            {
                chkRavien.Checked = true;

            }
            else
            { chkRavien.Checked = false; }
        }
        public void Load_PhongKham()
        {

            string sql = @"select p.id,p.maso+'-'+p.TenPhong as 'tenphong'
                                            from KB_Phong p
                                            inner join banggiadichvu bg on bg.idbanggiadichvu = p.DichVuKCB
                                            where bg.idphongkhambenh =1
                                            and p.isPhongNoiTru = 0
                                            and p.IsActive = 1
                                            and p.Status=1
                                            order by p.MaSo";
            DataTable dtPhong = DataAcess.Connect.GetTable(sql);
            slkPhongchuyen.Properties.NullText = "Nhập phòng khám";
            slkPhongchuyen.Properties.DataSource = dtPhong;
            slkPhongchuyen.Properties.DisplayMember = "tenphong";
            slkPhongchuyen.Properties.ValueMember = "id";

        }
        public void Load_Bacsi()
        {
            DataTable dtBacsi = DataAcess.Connect.GetTable(this.dt_Load_Bacsi());
            slkBacsi2.Properties.DataSource = dtBacsi;
            slkBacsi2.Properties.DisplayMember = "tenbacsi";
            slkBacsi2.Properties.ValueMember = "idbacsi";
            slkBacsi2.Properties.NullText = "Nhập Bác sĩ";
            slkBacsi2.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkBacsi2.Properties.ImmediatePopup = true;
            slkBacsi2.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }

        private string dt_Load_Bacsi()
        {
            string sql = "SELECT idbacsi,tenbacsi,mabacsi FROM dbo.bacsi WHERE mabacsi like '%CCHN%'";
            return sql;
        }
        public void Load_CDXD()
        {
            DataTable dtChandoan = DataAcess.Connect.GetTable(GetData.LoadICD10());
            slkCDXD.Properties.DataSource = dtChandoan;
            slkCDXD.Properties.DisplayMember = "MaICD";
            slkCDXD.Properties.ValueMember = "IDICD";
            slkCDXD.Properties.NullText = "Nhập chẩn đoán";
            slkCDXD.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkCDXD.Properties.ImmediatePopup = true;
            slkCDXD.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
          
        }
        public void Load_CDPH()
        {
            DataTable dtChandoan = DataAcess.Connect.GetTable(GetData.LoadICD10());
            slkCDPH.Properties.DataSource = dtChandoan;
            slkCDPH.Properties.DisplayMember = "MaICD";
            slkCDPH.Properties.ValueMember = "IDICD";
            slkCDPH.Properties.NullText = "Nhập chẩn đoán";
            slkCDPH.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkCDPH.Properties.ImmediatePopup = true;
            slkCDPH.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
           
        }
        private void searchLookUpEdit2_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string maICD1 = slkCDPH.EditValue.ToString();
                string sql = "SELECT mota FROM dbo.ChanDoanICD where idicd= '" + maICD1 + "'";
                DataTable layMota = DataAcess.Connect.GetTable(sql);
                txtCDPH.Text = layMota.Rows[0]["mota"].ToString();
            }
            catch
            {
                return;
            }
        }
        public void Load_CDPH(string idkhambenh)
        {
            DataTable luuCDPH = DataAcess.Connect.GetTable(GetData.Load_CDPH(idkhambenh));
            if (luuCDPH == null)
            {
                MessageBox.Show("Không có cdph");
                return;
            }
            else
            {
                for (int t = 0; t < luuCDPH.Rows.Count; t++)
                {
                    string id_ph = luuCDPH.Rows[t]["id_ph"].ToString();
                    string maicd_ph = luuCDPH.Rows[t]["maicd_ph"].ToString();
                    string MoTa_ph = luuCDPH.Rows[t]["MoTa_ph"].ToString();
                    string id_cdph = luuCDPH.Rows[t]["id"].ToString();
                    string[] row = { id_ph, maicd_ph, MoTa_ph, id_cdph };
                    dtgvCDPH.Rows.Add(row);
                    int colNumber = 0;
                    for (int i = 0; i < dtgvCDPH.Rows.Count; i++)
                    {
                        if (dtgvCDPH.Rows[i].IsNewRow) continue;
                        string tmp = dtgvCDPH.Rows[i].Cells[colNumber].Value.ToString();
                        for (int j = dtgvCDPH.Rows.Count - 1; j > i; j--)
                        {
                            if (dtgvCDPH.Rows[j].IsNewRow) continue;
                            if (tmp == dtgvCDPH.Rows[j].Cells[colNumber].Value.ToString())
                            {
                                dtgvCDPH.Rows.RemoveAt(j);
                            }
                        }
                    }
                }
            }
            dtgvCDPH.AutoResizeColumns();
            dtgvCDPH.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

        }

        private void slkCDXD_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string maICD1 = slkCDXD.EditValue.ToString();
                string sql = "SELECT mota FROM dbo.ChanDoanICD where idicd= '" + maICD1 + "'";
                DataTable layMota = DataAcess.Connect.GetTable(sql);
                txtCDXD.Text = layMota.Rows[0]["mota"].ToString();
            }
            catch
            {
                return;
            }
        }

        private void dtgvCDPH_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

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
        }

        private void dtgvCDPH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dtgvCDPH.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCDPH.Columns[e.ColumnIndex].Name;
                if (command == "CDPHDel")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvCDPH.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Chẩn đoán phối hợp", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dtgvCDPH.Rows[r].Cells["ID_CDPH"].Value.ToString() != "" || dtgvCDPH.Rows[r].Cells["ID_CDPH"].Value.ToString() != null)
                                    {
                                        string sql = "delete chandoanphoihop where id='" + dtgvCDPH.Rows[r].Cells["ID_CDPH"].Value.ToString() + "'";
                                        DataTable xoaCDPH = DataAcess.Connect.GetTable(sql);
                                        dtgvCDPH.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        dtgvCDPH.Rows.RemoveAt(oneCell.RowIndex);
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

        private void btnThemCDPH_Click(object sender, EventArgs e)
        {
            #region Thêm chẩn đoán xác định
            try
            {
                string sql = @"select idicd,MaICD,MoTa from ChanDoanICD where IDICD='" + slkCDPH.EditValue.ToString() + "'";
                DataTable dtICD = DataAcess.Connect.GetTable(sql);
                string id_ph = dtICD.Rows[0]["idicd"].ToString();
                string maicd_ph = dtICD.Rows[0]["maicd"].ToString();
                string mota_ph = txtCDPH.Text.ToString();
                string IDCDPH = "";
                string[] row = { id_ph, maicd_ph, mota_ph, IDCDPH };
                //for (int i = 0; i < dtICD.Rows.Count; i++)
                //{
                //    dtICD.Rows[i]["STT"] = i + 1;

                //}
                dtgvCDPH.Rows.Add(row);
                dtgvCDPH.AutoResizeColumns();
                dtgvCDPH.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            }
            catch
            {
                MessageBox.Show("Chưa chọn chẩn đoán!");
            }

            int colNumber = 0;
            for (int i = 0; i < dtgvCDPH.Rows.Count - 1; i++)
            {
                if (dtgvCDPH.Rows[i].IsNewRow) continue;
                string tmp = dtgvCDPH.Rows[i].Cells[colNumber].Value.ToString();
                for (int j = dtgvCDPH.Rows.Count - 1; j > i; j--)
                {
                    if (dtgvCDPH.Rows[j].IsNewRow) continue;
                    if (tmp == dtgvCDPH.Rows[j].Cells[colNumber].Value.ToString())
                    {
                        dtgvCDPH.Rows.RemoveAt(j);
                    }
                }
            }
            #endregion
        }
    }
}
