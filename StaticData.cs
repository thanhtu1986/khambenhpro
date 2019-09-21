using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KhamBenhPro
{
    class StaticData
    {
        public static int int_Search(DataTable dtSearch, string s_Filter)
        {
            try
            {
                DataRow[] DR = dtSearch.Select(s_Filter);
                if (DR.Length == 0) return -1;
                return dtSearch.Rows.IndexOf(DR[0]);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
