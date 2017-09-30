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
    /// Interaction logic for KPPMessages.xaml
    /// </summary>
    public partial class KPPMessages : UserControl
    {
        public KPPMessages()
        {
            InitializeComponent();

            if (File.Exists(MainWindow.specificFolder + "\\KPPMessage.xml"))
            {
                ObservableCollection<msg> custdata = GetData();
                //Bind the DataGrid to the KppDevices data
                DG1.DataContext = custdata;
                kkppdensource = custdata;
            }
            else
            {
                kkppdensource = new ObservableCollection<msg>();
                dev = new kppMessage();
            }
        }


        public ObservableCollection<msg> kkppdensource;
        public kppMessage dev;

        private void ModernButton_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.addKppDevice add = new Windows.addKppDevice();
            if (add.ShowDialog() == true)
            {
                msg nd = new msg();

                int maxID = 0;

                if (kkppdensource != null && kkppdensource.Count > 0)
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
                ObservableCollection<msg> custdata = GetData();
                //Bind the DataGrid to the KppDevices data
                DG1.DataContext = null;
                DG1.DataContext = custdata;
            }
        }
      

        public ObservableCollection<msg> GetData()
        {
            kkppdensource = new ObservableCollection<msg>();

            kkppdensource = new ObservableCollection<msg>();
            dev = new kppMessage();
            XmlSerializer xx = new XmlSerializer(typeof(kppMessage));
            FileStream myFileStream =
new FileStream(MainWindow.specificFolder + "\\KPPMessage.xml", FileMode.Open);
            //TextWriter writer = new StreamWriter("KPPDevices.xml", false);
            dev = (kppMessage)xx.Deserialize(myFileStream);
            myFileStream.Close();
            for (int i = 0; i < dev.Count; i++)
            {
                kkppdensource.Add(dev[i]);
            }
            return kkppdensource;
        }


        private ObservableCollection<msg> savedata()
        {
            for (int i = 0; i < kkppdensource.Count; i++)
            {
                dev[i].showing = kkppdensource[i].showing;
            }

            XmlSerializer xx = new XmlSerializer(typeof(kppMessage));
            TextWriter writer = new StreamWriter(MainWindow.specificFolder + "\\KPPMessage.xml", false);
            xx.Serialize(writer, dev);
            writer.Close();
            return kkppdensource;
        }

        public class msg
        {
            public int ID { get; set; }
            public string name { get; set; }
            public string path { get; set; }
            public bool showing { get; set; }
        }

        public class kppMessage : ICollection
        {

            public ArrayList devList = new ArrayList();

            public msg this[int index]
            {
                get { return (msg)devList[index]; }
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

            public void Add(msg dev)
            {
                devList.Add(dev);
            }

            public void DisableShowing(int id)
            {
                msg tmp = new msg();
                for (int i = 0; i < devList.Count; i++)
                {
                    tmp = (msg)devList[i];
                    if (tmp.ID == id)
                    {
                        tmp.showing = !tmp.showing;
                    }
                }
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

                    if (panel1.msg != null)
                    {
                        panel1.instance.kppMsgList = new KPPMessages().kkppdensource;
                        panel1.instance.DGmsg.DataContext = null;
                        panel1.instance.DGmsg.DataContext = panel1.instance.kppMsgList;
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("Ошибка при сохранении информации...", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ModernButton_Click_2(object sender, RoutedEventArgs e)
        {
            if (
                       MessageBox.Show("Удалить сообщение?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes
                       )
            {
                try
                {
                   

                    if (panel1.msg != null)
                    {
                        panel1.instance.kppMsgList = new KPPMessages().kkppdensource;
                        panel1.instance.DGmsg.DataContext = null;
                        panel1.instance.DGmsg.DataContext = panel1.instance.kppMsgList;
                    }

                    if (((msg)DG1.SelectedCells[0].Item) != null)
                    {
                       dev.devList.Remove((msg)DG1.SelectedCells[0].Item);
                        kkppdensource.Remove((msg)DG1.SelectedCells[0].Item);
                        // DG1.ItemsSource
                    }
                    savedata();
                }

                catch (Exception err)
                {

                }

            }
        }
    }
}
