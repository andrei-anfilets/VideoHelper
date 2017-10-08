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
    /// Interaction logic for LoremIpsum.xaml
    /// </summary>
    public partial class LoremIpsum : UserControl
    {
        public LoremIpsum()
        {
            InitializeComponent();      

            ObservableCollection<dev> custdata = GetData();
            //Bind the DataGrid to the KppDevices data
            DG1.DataContext = custdata;
        }
        public ObservableCollection<dev> kppdevices;
        public kppDevices dev;

        


        public ObservableCollection<dev> GetData()
        {
            kppdevices = new ObservableCollection<dev>();

            kppdevices = new ObservableCollection<ModernUI.App.Content.dev>();
            dev = new kppDevices();
            XmlSerializer xx = new XmlSerializer(typeof(kppDevices));
            FileStream myFileStream =
new FileStream(MainWindow.specificFolder + "\\KPPDevices.xml", FileMode.Open);
            //TextWriter writer = new StreamWriter("KPPDevices.xml", false);
            dev = (kppDevices)xx.Deserialize(myFileStream);
            myFileStream.Close();
            for (int i = 0; i < dev.Count; i++)
            {
                kppdevices.Add(dev[i]);
            }


            return kppdevices;
        }

        private ObservableCollection<dev> savedata()
        {
            for (int i = 0; i < dev.Count; i++)
            {
                dev[i].showing = kppdevices[i].showing;
            }
            XmlSerializer xx = new XmlSerializer(typeof(kppDevices));
            TextWriter writer = new StreamWriter(MainWindow.specificFolder + "\\KPPDevices.xml", false);
            xx.Serialize(writer, dev);
            writer.Close();
            return kppdevices;
        }


        private void UserControl_LostFocus_1(object sender, RoutedEventArgs e)
        {



        }

        private void UserControl_FocusableChanged_1(object sender, DependencyPropertyChangedEventArgs e)
        {

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

        private void ModernButton_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.addKppDevice add = new Windows.addKppDevice();

            if (add.ShowDialog() == true)
            {
                dev nd = new dev();
                int maxID = kppdevices[kppdevices.Count-1].ID + 1;
                nd.ID = maxID;
                nd.name = add.name1.Text;
                nd.path = add.image1.Source.ToString();
                nd.showing = true;
                kppdevices.Add(nd);
                dev.Add(nd);
                savedata();

                ObservableCollection<dev> custdata = GetData();
                //Bind the DataGrid to the KppDevices data
                DG1.DataContext = null;
                DG1.DataContext = custdata;
            }
        }

        private void ModernButton_Click_2(object sender, RoutedEventArgs e)
        {
            Windows.addKppDevice add = new Windows.addKppDevice();
            add.Title = "Редактирование";
            try
            {
                add.image1.Source = new ImageSourceConverter().ConvertFromString(System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + ((dev)DG1.SelectedItem).path) as ImageSource;
            }
            catch (Exception eee)
            {

                try
                {
                    var ur = new Uri(((dev)DG1.SelectedItem).path).LocalPath;
                    if (System.IO.File.Exists(ur))
                    {
                        add.image1.Source = new ImageSourceConverter().ConvertFromString(ur) as ImageSource;
                    }
                }
                catch (Exception er)
                { 
                }

            }
            finally
            {
               
            }
            add.name1.Text = ((dev)DG1.SelectedItem).name;

            if (add.ShowDialog() == true)
            {
                dev nd = ((dev)DG1.SelectedItem);
              //  int maxID = kppdevices[kppdevices.Count - 1].ID + 1;
              //  nd.ID = maxID;
                nd.name = add.name1.Text;
                nd.path = add.image1.Source.ToString();
                nd.showing = false;
              
              //  dev.Add(nd);
                savedata();

                ObservableCollection<dev> custdata = GetData();
                //Bind the DataGrid to the KppDevices data
                DG1.DataContext = null;
                DG1.DataContext = custdata;
            }
        }
    }

    public class dev
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        private bool _showing = false;
        public bool showing { get {
                return _showing;
            } set {
                _showing = value;
                if (FirstFloor.ModernUI.App.Pages.DpiAwareness.RemoteControlEnabled)
                {
                    FirstFloor.ModernUI.App.Pages.DpiAwareness.Instance.sendKppDevQueue();
                }
            } }
    }

    public class kppDevices : ICollection
    {

        public ArrayList devList = new ArrayList();

        public dev this[int index]
        {
            get { return (dev)devList[index]; }
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

        public void Add(dev dev)
        {
            devList.Add(dev);
        }

        public void DisableShowing(int id)
        {
            dev tmp = new dev();
            for (int i = 0; i < devList.Count; i++)
            {
                tmp = (dev)devList[i];
                if (tmp.ID == id)
                {
                    tmp.showing = !tmp.showing;
                }
            }        
        }


    }


}
