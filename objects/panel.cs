using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFloor.ModernUI.App.objects
{
   public class panel
    {
        public int screen_number = 0;
        public bool is_showing = false;
        public int template_number = 1;
        public System.Windows.Window win = null;

        public panel(int _screen_num, int _template_num, System.Windows.Window _win)
        {
            screen_number = _screen_num;
            template_number = _template_num;
            is_showing = true;
            win = _win;
        }

     


    }
}
