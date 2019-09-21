using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KhamBenhPro
{
    public class SimpleDataSource
    {
        public string idicd { get; set; }
        public string MaICD { get; set; }
        public string MoTaCD_edit { get; set; }

        public SimpleDataSource(string pidicd, string pMaICD, string pMoTaCD_edit)
        {
            idicd = pidicd;
            MaICD = pMaICD;
            MoTaCD_edit = pMoTaCD_edit;
        }
    }
}
