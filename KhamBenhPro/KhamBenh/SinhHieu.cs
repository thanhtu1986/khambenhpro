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
    public partial class SinhHieu : Form
    {
        public SinhHieu()
        {
            InitializeComponent();
            Load_ChanDoanSoBo();
            Load_CDSB(Truyendulieu.idkhambenh);
            Load_SinhHieu(Truyendulieu.idkhambenh);
        }

        public void Load_SinhHieu(string idkhambenh)
        {
            DataTable dtsinhhieu= DataAcess.Connect.GetTable(GetData.dt_BNDaKham2(idkhambenh));
            try
            {
                txtMach.Text = dtsinhhieu.Rows[0]["MACH"].ToString();
                txtNhietDo.Text = dtsinhhieu.Rows[0]["NHIETDO"].ToString();
                txtHuyetAp.Text = dtsinhhieu.Rows[0]["HUYETAP1"].ToString();
                txtHuyetAp2.Text = dtsinhhieu.Rows[0]["HUYETAP2"].ToString();
                txtNhipTho.Text = dtsinhhieu.Rows[0]["NHIPTHO"].ToString();
                txtCanNang.Text = dtsinhhieu.Rows[0]["CANNANG"].ToString();
                txtChieuCao.Text = dtsinhhieu.Rows[0]["CHIEUCAO"].ToString();
                txtBMI.Text = dtsinhhieu.Rows[0]["BMI"].ToString();
                txtTiensu.Text = dtsinhhieu.Rows[0]["tiensu"].ToString();
                txtTrieuchung.Text = dtsinhhieu.Rows[0]["trieuchung"].ToString();
                txtBenhsu.Text = dtsinhhieu.Rows[0]["benhsu"].ToString();
            }
            catch { }
        }

        public void Load_CDSB(string idkhambenh)
        {
            #region Load chẩn đoán sơ bộ
            DataTable luuCDSB = DataAcess.Connect.GetTable(GetData.dt_Load_CDSB(idkhambenh));
            if (luuCDSB == null)
            {
                MessageBox.Show("Không có Chẩn đoán sơ bộ");
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
            dtgvCDSB.AutoResizeColumns();
           // dtgvCDSB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            #endregion
        }
        public void Load_ChanDoanSoBo()
        {
            DataTable dtChandoanSB = DataAcess.Connect.GetTable(GetData.LoadICD10());
            slkCDSB.Properties.DataSource = dtChandoanSB;
            slkCDSB.Properties.DisplayMember = "MaICD";
            slkCDSB.Properties.ValueMember = "IDICD";
            slkCDSB.Properties.NullText = "Nhập chẩn đoán sơ bộ";
            slkCDSB.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            slkCDSB.Properties.ImmediatePopup = true;
            slkCDSB.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

        }

        private void slkCDSB_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string maICD1 = slkCDSB.EditValue.ToString();
                string sql = "SELECT mota FROM dbo.ChanDoanICD where idicd= '" + maICD1 + "'";
                DataTable layMota = DataAcess.Connect.GetTable(sql);
                txtCDSB.Text = layMota.Rows[0]["mota"].ToString();
            }
            catch
            {
                return;
            }
        }

        private void btnThemCDSB_Click(object sender, EventArgs e)
        {
            #region Thêm chẩn đoán sơ bộ
            try
            {
                string sql = "select idicd,MaICD,MoTa from ChanDoanICD where IDICD='" + slkCDSB.EditValue.ToString() + "'";
                DataTable dtICD = DataAcess.Connect.GetTable(sql);
                string IDICD = dtICD.Rows[0]["idicd"].ToString();
                string MaICD = dtICD.Rows[0]["maicd"].ToString();
                string MoTa = txtCDSB.Text.ToString();
                string IDCDSB = "";
                string[] row = { IDICD, MaICD, MoTa, IDCDSB };
                //for (int i = 0; i < dtICD.Rows.Count; i++)
                //{
                //    dtICD.Rows[i]["STT"] = i + 1;

                //}
                dtgvCDSB.Rows.Add(row);
                dtgvCDSB.AutoResizeColumns();
                dtgvCDSB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
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

        private void dtgvCDSB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int r = dtgvCDSB.CurrentCell.RowIndex;
            if (e.RowIndex > -1)
            {
                string command = dtgvCDSB.Columns[e.ColumnIndex].Name;
                if (command == "XoaCDSB")
                {
                    try
                    {
                        foreach (DataGridViewCell oneCell in dtgvCDSB.SelectedCells)
                        {
                            if (oneCell.Selected)
                            {
                                if (MessageBox.Show("Bạn có chắc muốn xóa?", "Xóa Chẩn đoán sơ bộ", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                {

                                    if (dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != "" || dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() != null)
                                    {
                                        string sql = "delete chandoansobo where id='" + dtgvCDSB.Rows[r].Cells["IDCDSB"].Value.ToString() + "'";
                                        DataTable xoacdsb = DataAcess.Connect.GetTable(sql);
                                        dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
                                    }
                                    else
                                    {
                                        dtgvCDSB.Rows.RemoveAt(oneCell.RowIndex);
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

        private void dtgvCDSB_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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

        private string dt_LoadBN()
        {
            string sql = @"  select ct.idchitietdangkykham,isnull(kb.idkhambenh,0) as idkhambenh,dk.IDKHAMBENH_CHUYEN,kb.TGXuatVien,isnull(kb.IdChuyenPK,0) as IdChuyenPK,bn.mabenhnhan,bn.tenbenhnhan,kb.idkhambenhchuyenphong,bn.idbenhnhan,dk.iddangkykham,ct.idbanggiadichvu,dk.LoaiKhamID,dk.IdBenhBHDongTien
                                                    from dangkykham dk
                                                    inner join chitietdangkykham ct on ct.iddangkykham=dk.iddangkykham
													inner join benhnhan bn on bn.idbenhnhan=dk.idbenhnhan
                                                    left join khambenh kb on kb.IdChiTietDangKyKham=ct.IdChiTietDangKyKham
                                                    left join hs_benhnhanbhdongtien dt on dt.id=dk.IdBenhBHDongTien
                                                    where ct.idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'";
            return sql;
        }
        public void Insert_Sinhhieu()
        {
            DataTable dtLuuKB2 = DataAcess.Connect.GetTable(this.dt_LoadBN());
            string insertSH = @"insert into sinhhieu (idbenhnhan,ngaydo,mach,nhietdo,huyetap1,huyetap2,nhiptho,chieucao,cannang,BMI,Iddangkykham,idchitietdangkykham,idkhoasinhhieu,IdKhamBenh) values ('" + dtLuuKB2.Rows[0]["idbenhnhan"].ToString() + @"',
                                                              '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "','" + txtMach.Text + "','" + txtNhietDo.Text + "','" + txtHuyetAp.Text + "','" + txtHuyetAp2.Text + @"',
                                                                '" + txtNhipTho.Text + "','" + txtChieuCao.Text + "','" + txtCanNang.Text + "','" + txtBMI.Text + "','" + dtLuuKB2.Rows[0]["iddangkykham"].ToString() + "','" + dtLuuKB2.Rows[0]["idchitietdangkykham"].ToString() + "',1,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'))";
            DataTable luuSinhhieu = DataAcess.Connect.GetTable(insertSH);
            for (int x = 0; x < dtgvCDSB.Rows.Count - 1; x++)
            {
                string insertCDSB = @"insert into chandoansobo (id,idkhambenh,idicd,maicd,MoTaCD_edit) values ((select max(id) from chandoansobo)+1,(select max(idkhambenh) from khambenh where idchitietdangkykham='" + Truyendulieu.idchitietdangkykham + "'),'" + dtgvCDSB.Rows[x].Cells["IDICD"].Value.ToString() + "','" + dtgvCDSB.Rows[x].Cells["MAICD"].Value.ToString() + "',N'" + dtgvCDSB.Rows[x].Cells["MOTA"].Value.ToString() + "')";
                DataTable luuCDSB = DataAcess.Connect.GetTable(insertCDSB);
            }
        }
    }
}
