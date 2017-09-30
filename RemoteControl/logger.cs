using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App
{
    class logger
    {
        public static void Log(string txt)
        {
            System.IO.File.AppendAllText(ModernUI.App.MainWindow.specificFolder + "\\log.txt", DateTime.Now + " " + txt + Environment.NewLine);
        }
    }
}
