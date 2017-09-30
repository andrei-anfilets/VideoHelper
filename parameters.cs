using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.HSSF.Model; // InternalWorkbook
using NPOI.HSSF.UserModel; // HSSFWorkbook, HSSFSheet
using System.IO;


namespace FirstFloor.ModernUI.App
{
    class parameters
    {

        public HSSFWorkbook wb;
        public HSSFSheet sh;
        private String path = "";

        public parameters()
        { 
        
        }

        public parameters (String dataPath)
        {
            var fs = new FileStream(dataPath, FileMode.Open, FileAccess.Read);
            wb = new HSSFWorkbook(fs);
            path = dataPath;
        }

        //
        public void SavePanelParameters(string name, String source)
        {
            sh = (HSSFSheet)wb.GetSheet("panels");
            int i = 1;
            while (sh.GetRow(i) != null)
            {
                if (sh.GetRow(i).GetCell(0).StringCellValue == name)
                {                  
                    sh.GetRow(i).GetCell(5).SetCellValue(source.ToString());      
                }
                i++;
            }        
        }
        public void save()
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                wb.Write(fs);
            }
        }

        //1- width, 2-height, 3-source
        public String LoadPanelSrc(string name)
        {
            String rez = "-";
            sh = (HSSFSheet)wb.GetSheet("panels");
            int i = 1;
            while (sh.GetRow(i) != null)
            {
                if (sh.GetRow(i).GetCell(0).StringCellValue == name)
                {
                   return sh.GetRow(i).GetCell(5).StringCellValue;
                }
                i++;
            }
            return rez;

        }



    }
}
