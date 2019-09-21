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
    public partial class CLSTuDK : Form
    {
        public CLSTuDK()
        {
            InitializeComponent();
        }

        private void ToaThuocCu_Load(object sender, EventArgs e)
        {
            string sql = @"select B.IDKHAMBENH
                ,a.ngayratoa,
		        a.idbenhnhantoathuoc
                 ,TENDICHVU=DBO.HS_TENPHONG(B.PHONGID)
                 , c.mota
                  ,d.tenbacsi 
                from benhnhantoathuoc a 
                inner join khambenh b on a.idkhambenh=b.idkhambenh
                 left join chandoanicd c on c.idicd=b.ketluan
                 left join bacsi d on B.idbacsi=d.idbacsi
                 WHERE B.IDBENHNHAN='" + Truyendulieu.idbenhnhan+@"'
                                      AND B.IDPHONGKHAMBENH=1
                ORDER BY B.IDKHAMBENH  DESC ";
            DataTable tt = DataAcess.Connect.GetTable(sql);
            for (int i=0;i<tt.Rows.Count;i++)
            {
                TreeNode Node =new TreeNode("Ngày khám: "+ DateTime.Parse(tt.Rows[i]["ngayratoa"].ToString()).ToString("dd/MM/yyyy")+"---"+ tt.Rows[i]["TENDICHVU"].ToString() +"---" + tt.Rows[i]["tenbacsi"].ToString());
                Node.Tag = tt.Rows[i]["idkhambenh"].ToString();
                Node.ForeColor = Color.Blue;
                //treeView1.Nodes.Add(Node);
                string sql2 = @"select 
                        TENTHUOC
                        ,TENDVT
                        ,SOLUONGKE,
                        A0.IDCHITIETBENHNHANTOATHUOC
                        ,B.IDKHAMBENH
                        ,ngayratoa=convert(nvarchar(20),B.NGAYKHAM,103)
                        ,TENDICHVU=DBO.HS_TENPHONG(B.PHONGID)
                        , c.mota
                        ,d.tenbacsi 
                        ,ISCHON=0
                        from 
                         CHITIETBENHNHANTOATHUOC A0
                        INNER join khambenh b on a0.idkhambenh=b.idkhambenh
                        left join chandoanicd c on c.idicd=b.ketluan
                        left join bacsi d on B.idbacsi=d.idbacsi
                        INNER join THUOC F ON A0.IDTHUOC=F.IDTHUOC
                        LEFT JOIN THUOC_DONVITINH G ON F.IDDVT=G.ID
                        WHERE A0.IDKHAMBENH =" + tt.Rows[i]["IDKHAMBENH"].ToString() + @"
                        ORDER BY B.NGAYKHAM, B.IDKHAMBENH  DESC ";
                DataTable tt2 = DataAcess.Connect.GetTable(sql2);
                for (int j=0;j<tt2.Rows.Count;j++)
                {
                    TreeNode Node2 = new TreeNode(tt2.Rows[j]["TENTHUOC"].ToString()+"---SL: "+tt2.Rows[j]["SOLUONGKE"].ToString());
                    Node2.Tag = Node.Tag;
                  //  treeView1.Nodes[i].Nodes.Add(Node2);
                }
            }
         
       }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Truyendulieu.idkhambenh_old = e.Node.Tag.ToString();
         //   textBox1.Text= e.Node.Tag.ToString();
        }

        private void treeView1_Enter(object sender, EventArgs e)
        {
           
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            this.Close();
        }
    }
}
