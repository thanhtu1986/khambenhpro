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
            Load_CLS();
            Load_Item_CDSB();
            Load_CD_Gridview();
        }
        public void Load_CLS()
        {
            string sql = @"SELECT A.idbanggiadichvu as idbanggiadichvu
                                               ,A.tendichvu as tendichvu
                                               ,BH.GiaDV as giadichvu
                                               ,BH.GIABH as giabh
                                               ,IsSuDungChoBH=BH.ISBHYT
											   ,bh.TuNgay as fromdate
											    ,A.TENBAOHIEM as tenbaohiem
                  				            FROM BANGGIADICHVU A
               				                LEFT JOIN PHONGKHAMBENH b on a.idphongkhambenh=b.idphongkhambenh
                                            left join hs_banggiavienphi BH ON BH.IdGiaDichVu=(SELECT TOP 1 IdGiaDichVu FROM hs_banggiavienphi BH0 WHERE BH0.IdDichVu=A.IDBANGGIADICHVU AND BH0.TuNgay<=GETDATE() ORDER BY TuNgay DESC)
                                            WHERE b.loaiphong = 1 and a.isactive=1";
            DataTable dtthuoc = DataAcess.Connect.GetTable(sql);
            sluCLS.Properties.DataSource = dtthuoc;
            sluCLS.Properties.DisplayMember = "tendichvu";
            sluCLS.Properties.ValueMember = "idbanggiadichvu";
            sluCLS.Properties.NullText = "Nhập tên CLS";
            sluCLS.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
            sluCLS.Properties.ImmediatePopup = true;
            sluCLS.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }
        private void Load_CD_Gridview()
        {
            string sql = @"select id,idicd,maicd,MoTaCD_edit from chandoantheocls where idbanggiadichvu='" + sluCLS.EditValue + "'";
            DataTable dtCDThuoc = DataAcess.Connect.GetTable(sql);
            grcCD_CLS.DataSource = dtCDThuoc;
        }

        public static string Load_ICD()
        {
            string sql = @"select IDICD,MaICD,MoTa from ChanDoanICD";
            return sql;
        }

        private void Load_Item_CDSB()
        {
            #region Hàm load mã ICD lên 1 ô trên Gridview Chẩn đoán theo thuốc
            DataTable dt1 = DataAcess.Connect.GetTable(Load_ICD());
            repositoryItemCustomGridLookUpEdit1.NullText = @"Nhập mã ICD";
            repositoryItemCustomGridLookUpEdit1.DataSource = dt1;
            repositoryItemCustomGridLookUpEdit1.ValueMember = "IDICD";
            repositoryItemCustomGridLookUpEdit1.DisplayMember = "MaICD";
            repositoryItemCustomGridLookUpEdit1.BestFitMode = BestFitMode.BestFitResizePopup;
            repositoryItemCustomGridLookUpEdit1.ImmediatePopup = true;
            repositoryItemCustomGridLookUpEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            MaICD_col.ColumnEdit = repositoryItemCustomGridLookUpEdit1;
            #endregion
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Load_CD_Gridview();
            string sql = @"select idbanggiadichvu,tendichvu from banggiadichvu where idbanggiadichvu='" + sluCLS.EditValue + "'";
            DataTable dtcls = DataAcess.Connect.GetTable(sql);
            txtTenCLS.Text = dtcls.Rows[0]["tendichvu"].ToString();
            txtIdCLS.Text = dtcls.Rows[0]["idbanggiadichvu"].ToString();
            Load_CD_Gridview();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            #region Kiểm tra trùng mã ICD khi nhập Chẩn đoán theo thuốc
            try
            {
                string cls = gridView1.GetRowCellValue(e.RowHandle, gridColumn9).ToString();
                for (int i = 0; i < gridView1.RowCount - 1; i++)
                {
                    if (e.RowHandle != i)
                    {
                        string idcanlamsan = gridView1.GetRowCellValue(i, gridView1.Columns["idicd"]).ToString();
                        if (cls == idcanlamsan)
                        {
                            MessageBox.Show("Đã có nhập mã ICD này rồi!");
                            gridView1.DeleteRow(gridView1.FocusedRowHandle);
                            return;
                        }
                    }
                }
            }
            catch { }
            #endregion

            #region Click chọn ICD vào gridview Chẩn đoán theo thuốc
            if (e.Column.FieldName == "maicd")
            {
                var value = gridView1.GetRowCellValue(e.RowHandle, e.Column);
                string sql = @"select IDICD,MaICD,MoTa from ChanDoanICD where  IDICD='" + value + "'";
                DataTable dt = DataAcess.Connect.GetTable(sql);
                if (dt != null)
                {
                    gridView1.SetRowCellValue(e.RowHandle, "idicd", dt.Rows[0]["IDICD"].ToString());
                    // gridView2.SetRowCellValue(e.RowHandle, "MaICD", dt.Rows[0]["MaICD"].ToString());
                    gridView1.SetRowCellValue(e.RowHandle, "MoTaCD_edit", dt.Rows[0]["MoTa"].ToString());
                }
            }
            #endregion
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.RowCount - 1; i++)
            {
                if (gridView1.GetRowCellValue(i, gridView1.Columns["id"]).ToString() == "" || gridView1.GetRowCellValue(i, gridView1.Columns["id"]).ToString() == null || gridView1.GetRowCellValue(i, gridView1.Columns["id"]).ToString() == "0")
                {
                    string sql = @"insert into chandoantheocls (idbanggiadichvu,tendichvu,idicd,MaICD,MoTaCD_edit,NGAYCHANDOAN)
                               values('" + txtIdCLS.Text + "',N'" + txtTenCLS.Text + @"',
                                '" + gridView1.GetRowCellValue(i, gridView1.Columns["idicd"]).ToString() + @"',
                                '" + gridView1.GetRowCellValue(i, gridView1.Columns["maicd"]).ToString() + @"',
                                N'" + gridView1.GetRowCellValue(i, gridView1.Columns["MoTaCD_edit"]).ToString() + "',GETDATE())";
                    DataTable luuCD = DataAcess.Connect.GetTable(sql);
                }
            }
        }
       
    }
}
