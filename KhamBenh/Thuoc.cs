using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;

namespace KhamBenhPro.KhamBenh
{
    public partial class Thuoc : Form
    {
        public Thuoc()
        {
            InitializeComponent();
            Load_thuoc();
            Load_Item_CDSB();
            Load_CD_Gridview();
        }
        public void Load_thuoc()
        {
         string sql = @"select * from (SELECT B.IDTHUOC as idthuoc
										,B.TENTHUOC as tenthuoc
										,C.TENDVT as donvitinh
                                        ,B.congthuc as congthuc
                                        , cd.tencachdung as duongdung
                                        ,(CASE WHEN B.sudungchobh=1 THEN 'BH' ELSE 'DV' END) as isbhyt
                                        ,B.isthuocbv
                                        , DonGia  = B.GIA_MUA
                                         FROM Thuoc B  
                                        left join thuoc_donvitinh C on C.id=B.iddvt
                                        left join thuoc_cachdung cd on cd.idcachdung=B.idcachdung
                                        where     ISNULL( B.IsNgungSD,0)=0
										AND B.LOAITHUOCID=1
										AND B.ISTHUOCBV=1
                                        and b.tenthuoc is not null)ab
                                        where --slton>0 and 
                                        dongia>0
										ORDER BY TENTHUOC";
         DataTable dtthuoc = DataAcess.Connect.GetTable(sql);
         sluThuoc.Properties.DataSource = dtthuoc;
         sluThuoc.Properties.DisplayMember = "tenthuoc";
         sluThuoc.Properties.ValueMember = "idthuoc";
         sluThuoc.Properties.NullText = "Nhập tên thuốc";
         sluThuoc.Properties.BestFitMode = BestFitMode.BestFitResizePopup;
         sluThuoc.Properties.ImmediatePopup = true;
         sluThuoc.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        }

        private void Load_CD_Gridview()
        {
            string sql = @"select id,idicd,maicd,MoTaCD_edit from chandoantheothuoc where idthuoc='"+sluThuoc.EditValue+"'";
            DataTable dtCDThuoc = DataAcess.Connect.GetTable(sql);
            grcCD_Thuoc.DataSource = dtCDThuoc;
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
            string sql = @"select idthuoc,tenthuoc,congthuc,ghichu,LoiDan from thuoc where idthuoc='" + sluThuoc.EditValue + "'";
            DataTable dtthuoc = DataAcess.Connect.GetTable(sql);
            txtTenthuoc.Text = dtthuoc.Rows[0]["tenthuoc"].ToString();
            txtHoatchat.Text=dtthuoc.Rows[0]["congthuc"].ToString();
            txtIdthuoc.Text=dtthuoc.Rows[0]["idthuoc"].ToString();
            txtGhichu.Text = dtthuoc.Rows[0]["ghichu"].ToString();
            txtSongayratoa.Text = dtthuoc.Rows[0]["LoiDan"].ToString();
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

        private void Thuoc_Load(object sender, EventArgs e)
        {

        }

        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            #region Xóa chẩn đoán theo thuốc
            if (MessageBox.Show("Bạn có chắc muốn xóa Chẩn đoán theo thuốc?", "Cảnh báo!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridView1.Columns["id"]).ToString();
                    if (id != null && id != "")
                    {
                        string delete = "delete chandoantheothuoc where id =" + id;
                        bool ok = DataAcess.Connect.ExecSQL(delete);
                        if (ok)
                        {
                            MessageBox.Show("Xóa thành công!");
                            Load_CD_Gridview();
                        }
                    }
                    else
                    {
                        gridView1.DeleteRow(gridView1.FocusedRowHandle);
                    }
                }
                catch
                {
                    MessageBox.Show("Ô bạn chọn là ô trống!");
                }
            }
            #endregion
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.RowCount-1; i++)
            {
                if (gridView1.GetRowCellValue(i, gridView1.Columns["id"]).ToString() == "" || gridView1.GetRowCellValue(i, gridView1.Columns["id"]).ToString() == null || gridView1.GetRowCellValue(i, gridView1.Columns["id"]).ToString() == "0")
                {
                    string sql = @"insert into chandoantheothuoc (idthuoc,tenthuoc,hoatchat,idicd,maicd,MoTaCD_edit,NGAYCHANDOAN)
                               values('" + txtIdthuoc.Text + "',N'" + txtTenthuoc.Text + "',N'" + txtHoatchat.Text + @"',
                                '" + gridView1.GetRowCellValue(i, gridView1.Columns["idicd"]).ToString() + @"',
                                '" + gridView1.GetRowCellValue(i, gridView1.Columns["maicd"]).ToString() + @"',
                                N'" + gridView1.GetRowCellValue(i, gridView1.Columns["MoTaCD_edit"]).ToString() + "',GETDATE())";
                    DataTable luuCD = DataAcess.Connect.GetTable(sql);
                }
            }
            string sql2 = @"update thuoc set ghichu=N'"+txtGhichu.Text+"',LoiDan='"+txtSongayratoa.Text+"' where idthuoc='"+txtIdthuoc.Text+"'";
            DataTable luuGhichu = DataAcess.Connect.GetTable(sql2);
        }

    }
}
