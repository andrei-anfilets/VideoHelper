using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using FirstFloor.ModernUI.Presentation;
using System.Xml;
using System.Xml.Serialization;

using System.IO;

namespace FirstFloor.ModernUI.App
{
    [Serializable]
    public class UserStyle
    {

        public UserStyle()
        {

        }
        private string _src;
        public String themeSource
        {
            get { return _src; }
            set { _src = value; }
        }

        private string _theme;
        public string themeColor
        {
            get { return _theme; }
            set { _theme = value; }
        }      


        public UserStyle saveSettings()
        {
            XmlSerializer xx = new XmlSerializer(typeof(UserStyle));
            TextWriter writer = new StreamWriter(MainWindow.specificFolder + "\\Style.xml", false);
            xx.Serialize(writer, this);
            writer.Close();
            return this;
        }

        public UserStyle loadSettings()
        {
            UserStyle set = new UserStyle();
            XmlSerializer xx = new XmlSerializer(typeof(UserStyle));
            FileStream myFileStream =
new FileStream(MainWindow.specificFolder + "\\Style.xml", FileMode.Open);
            set = (UserStyle)xx.Deserialize(myFileStream);
            myFileStream.Close();
            return set;
        }

    }
}
