using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;


namespace FirstFloor.ModernUI.App
{
    [Serializable]
    class Settings
    {

        public bool displayMon = false;
        public bool DisplayControlMonitor
        {
            get { return displayMon; }

            set { displayMon = value; }
        }

        public Settings()
        {

        }

        public Settings saveSettings()
        {
            XmlSerializer xx = new XmlSerializer(typeof(Settings));
            TextWriter writer = new StreamWriter(MainWindow.specificFolder + "\\Settings.xml", false);
            xx.Serialize(writer, this);
            writer.Close();
            return this;
        }

        public Settings loadSettings()
        {
           Settings set = new Settings();
            XmlSerializer xx = new XmlSerializer(typeof(Settings));
            FileStream myFileStream =
new FileStream(MainWindow.specificFolder + "\\Settings.xml", FileMode.Open);
            //TextWriter writer = new StreamWriter("KPPDevices.xml", false);
            set = (Settings)xx.Deserialize(myFileStream);

            return set;

        }



    }
}
