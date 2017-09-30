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

using System.IO;
using System.Xml.Serialization;

namespace FirstFloor.ModernUI.App.Content
{


    public class item
    {
        public int ID { get; set;}
        public String name { get; set; }

        public item()
        { 
        
        }
        public item(int _id, String _name)
        {
            ID = _id;
            name = _name;
        }
    
    }
    /// <summary>
    /// Interaction logic for newTabEmp.xaml
    /// </summary>
    
    public partial class newTabEmp : UserControl
    {
        public static ListBox PlayList;
        public newTabEmp()
        {
            InitializeComponent();
  _empList.Add(new item(1, "Технические средства"));
  _empList.Add(new item(2, "Запрещенные предметы"));
  _empList.Add(new item(3, "Реклама (видео)"));
  _empList.Add(new item(4, "Сервисное сообщение (10 секунд)"));
  lbOne.ItemsSource = _empList;
  PlayList = lbTwo;

  try
  {
      XmlSerializer xx = new XmlSerializer(typeof(List<item>));
      FileStream myFileStream =
new FileStream(MainWindow.specificFolder + "\\PanelsOrder.xml", FileMode.Open);
      //TextWriter writer = new StreamWriter("KPPDevices.xml", false);
   //   queueList = (List<item>)xx.Deserialize(myFileStream);
    
  }
  catch (Exception err)
  {
      //queueList.Add(new item(1, "Технические средства"));
      //queueList.Add(new item(2, "Запрещенные предметы"));          
  }


  //lbTwo.ItemsSource = queueList;

 // lbOne.DataContext = _empList;

  //lbOne.DisplayMemberPath = "name";
  //lbTwo.DisplayMemberPath = "name";           
        }

        public List<item> _empList = new List<item>();
        public static List<item> queueList = new List<item>();
        ListBox dragSource = null;
        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }

        #region GetDataFromListBox(ListBox,Point)
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);
                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }
                    if (element == source)
                    {
                        return null;
                    }
                }
                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }
            return null;
        }

        #endregion

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            try
            {
                ListBox parent = (ListBox)sender;
                parent.DataContext = null;
                parent.ItemsSource = null;
                object data = e.Data.GetData(typeof(item));                
                parent.Items.Add((item)data);
                queueList.Add((item)data);
                                 
                //queueList = (List<item>)lbTwo.DataContext;
                if (ModernUI.App.Pages.DpiAwareness.RemoteControlEnabled)
                {
                    int elemId = ((item)data).ID;
                    client cl = new client(ModernUI.App.Pages.DpiAwareness.PanelIPToSave);
                   /// cl.StartClient();
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Сначала очистите стандартную очередь воспроизведения", "Внимание", MessageBoxButton.OK);
              //  ModernFrame.ErrorSample er = new ModernFrame.ErrorSample();
              //  NavigationCommands.GoToPage(er);
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
         
        }

        private void lbTwo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object sel = lbTwo.SelectedItem;
        }

        private void enableQuue(object sender, RoutedEventArgs e)
        {
           
        }

        private void disableQuue(object sender, RoutedEventArgs e)
        {

        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            lbTwo.ItemsSource = null;
            lbTwo.Items.Clear();
           
            try
            {
                queueList.Clear();
            }
            catch (Exception err)
            { 
              
            }
        }

        private void SaveOrderButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
              //  List<item> ls = (List<item>)lbTwo.Items;
                XmlSerializer xx = new XmlSerializer(typeof(List<item>));
                TextWriter writer = new StreamWriter(MainWindow.specificFolder + "\\PanelsOrder.xml", false);
                xx.Serialize(writer, queueList);
                writer.Close();
            }
            catch (Exception err)
            { 
            
            
            }
        }
    }
}
