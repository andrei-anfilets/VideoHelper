using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using System.Collections;
using System.Collections.ObjectModel;

using NPOI.HSSF.Model; // InternalWorkbook
using NPOI.HSSF.UserModel; // HSSFWorkbook, HSSFSheet
using System.IO;

using System.Xml;
using System.Xml.Serialization;

namespace FirstFloor.ModernUI.App.Content
{
    /// <summary>
    /// Interaction logic for KPPDeny.xaml
    /// </summary>
    public partial class KPPDeny : UserControl
    {
        public KPPDeny()
        {
            InitializeComponent();

            if (File.Exists(MainWindow.specificFolder + "\\KPPDeny.xml"))
            {
                ObservableCollection<deny> custdata = GetData();
                //Bind the DataGrid to the KppDevices data
                DG1.DataContext = custdata;
            }
            else
            {
                kkppdensource = new ObservableCollection<deny>();
                dev = new kppDeny();
            }
        }

        public ObservableCollection<deny> kkppdensource;
        public kppDeny dev;

        private void ModernButton_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.addKppDevice add = new Windows.addKppDevice();
            if (add.ShowDialog() == true)
            {
                deny nd = new deny();

                int maxID = 0;
                
                 if (kkppdensource != null && kkppdensource.Count >0)        
                 {
                 maxID = kkppdensource[kkppdensource.Count - 1].ID + 1;
                 }
                 nd.ID = maxID;
                nd.name = add.name1.Text;
                nd.path = add.image1.Source.ToString();
                nd.showing = false;
                kkppdensource.Add(nd);
                dev.Add(nd);
                savedata();
                ObservableCollection<deny> custdata = GetData();
                //Bind the DataGrid to the KppDevices data
                DG1.DataContext = null;
                DG1.DataContext = custdata;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (
            MessageBox.Show("Сохранить изменения?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes
            )
            {
                try
                {
                    savedata();
                }
                catch (Exception err)
                {
                    MessageBox.Show("Ошибка при сохранении информации...", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        public ObservableCollection<deny> GetData()
        {
            kkppdensource = new ObservableCollection<deny>();

            kkppdensource = new ObservableCollection<deny>();
            dev = new kppDeny();
            XmlSerializer xx = new XmlSerializer(typeof(kppDeny));
            FileStream myFileStream =
new FileStream(MainWindow.specificFolder + "\\KPPDeny.xml", FileMode.Open);
            //TextWriter writer = new StreamWriter("KPPDevices.xml", false);
            dev = (kppDeny)xx.Deserialize(myFileStream);
            myFileStream.Close();
            for (int i = 0; i < dev.Count; i++)
            {
                kkppdensource.Add(dev[i]);
            }
            return kkppdensource;
        }


        private ObservableCollection<deny> savedata()
        {
            for (int i = 0; i < dev.Count; i++)
            {
                dev[i].showing = kkppdensource[i].showing;
            }
           
            XmlSerializer xx = new XmlSerializer(typeof(kppDeny));
            TextWriter writer = new StreamWriter(MainWindow.specificFolder + "\\KPPDeny.xml", false);
            xx.Serialize(writer, dev);
            writer.Close();
            return kkppdensource;
        }

        public class deny
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            private bool _showing = false;
            public bool showing
            {
                get
                {
                    return _showing;
                }
                set
                {
                    _showing = value;
                    if (FirstFloor.ModernUI.App.Pages.DpiAwareness.RemoteControlEnabled)
                    {
                        FirstFloor.ModernUI.App.Pages.DpiAwareness.Instance.sendDenyList();
                    }
                }
            }
        }

        public class kppDeny : ICollection
        {

            private ArrayList devList = new ArrayList();

            public deny this[int index]
            {
                get { return (deny)devList[index]; }
            }

            public void CopyTo(Array a, int index)
            {
                devList.CopyTo(a, index);
            }
            public int Count
            {
                get { return devList.Count; }
            }
            public object SyncRoot
            {
                get { return this; }
            }
            public bool IsSynchronized
            {
                get { return false; }
            }
            public IEnumerator GetEnumerator()
            {
                return devList.GetEnumerator();
            }

            public void Add(deny dev)
            {
                devList.Add(dev);
            }

            public void DisableShowing(int id)
            {
                deny tmp = new deny();
                for (int i = 0; i < devList.Count; i++)
                {
                    tmp = (deny)devList[i];
                    if (tmp.ID == id)
                    {
                        tmp.showing = !tmp.showing;
                    }
                }
            }


        }
    }


}
