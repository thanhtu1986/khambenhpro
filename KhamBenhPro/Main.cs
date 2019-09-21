
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;

namespace KhamBenhPro
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void TabCtrl_main_TabItemClose(object sender, DevComponents.DotNetBar.TabStripActionEventArgs e)
        {
            TabCtrl_main.Tabs.Remove(TabCtrl_main.SelectedTab);
        }
        #region Kiem tra checkTab
        public bool checkTab(string name)
        {
            for (int i = 0; i < TabCtrl_main.Tabs.Count; i++)
            {
                if (TabCtrl_main.Tabs[i].Text == name)
                {
                    TabCtrl_main.SelectedTabIndex = i;
                    return true;
                }
            }
            return false;
        }
        #endregion

        private void DSChoKham_Click(object sender, EventArgs e)
        {
           

            if (checkTab("Chờ khám") == false)
            {
                TabItem tab = TabCtrl_main.CreateTab("Chờ khám");
            
                KhamBenh.DSChoKham frm01 = new KhamBenh.DSChoKham();
                frm01.Dock = DockStyle.Fill;
                frm01.FormBorderStyle = FormBorderStyle.None;
                frm01.TopLevel = false;
                tab.AttachedControl.Controls.Add(frm01);
                frm01.Show();
                TabCtrl_main.SelectedTabIndex = TabCtrl_main.Tabs.Count - 1;
                
            }
        }

        private void DSDaKham_Click(object sender, EventArgs e)
        {
            if (checkTab("Đã khám") == false)
            {
                TabItem tab = TabCtrl_main.CreateTab("Đã khám");

                KhamBenh.DSDaKham frm02 = new KhamBenh.DSDaKham();
                frm02.Dock = DockStyle.Fill;
                frm02.FormBorderStyle = FormBorderStyle.None;
                frm02.TopLevel = false;
                tab.AttachedControl.Controls.Add(frm02);
                frm02.Show();
                TabCtrl_main.SelectedTabIndex = TabCtrl_main.Tabs.Count - 1;

            }
        }

        private void xuấtXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkTab("Xuất XML") == false)
            {
                TabItem tab = TabCtrl_main.CreateTab("Xuất XML");

                XML.XuatXML xml = new XML.XuatXML();
                xml.Dock = DockStyle.Fill;
                xml.FormBorderStyle = FormBorderStyle.None;
                xml.TopLevel = false;
                tab.AttachedControl.Controls.Add(xml);
                xml.Show();
                TabCtrl_main.SelectedTabIndex = TabCtrl_main.Tabs.Count - 1;

            }
        }

        private void kếtNốiDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataAcess.Connect.NewConnect();
        }
    }

}