using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Net.Sockets;

namespace FirstFloor.ModernUI.App
{
    public class remotePanel
    {
        IPAddress panelIP;
        int index = 0;
        String name = "";

        public remotePanel(IPAddress adress, int indx, String _name)
        {
            panelIP = adress;
            index = indx;
            name = _name;
        }
    }
}
