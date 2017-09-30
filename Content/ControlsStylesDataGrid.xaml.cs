using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FirstFloor.ModernUI.App.Content
{
    // taken from MSDN (http://msdn.microsoft.com/en-us/library/system.windows.controls.datagrid.aspx)
    public enum OrderStatus { None, New, Processing, Shipped, Received };
    [Serializable]
    public class Monitor
    {
        public int number { get; set; }
        public string name { get; set; }
        public string resolution { get; set; }        
        public bool showing { get; set; }        
    }


    /// <summary>
    /// Interaction logic for ControlsStylesDataGrid.xaml
    /// </summary>
    public partial class ControlsStylesDataGrid : UserControl
    {
        public ControlsStylesDataGrid()
        {
            InitializeComponent();

            ObservableCollection<Monitor> custdata = GetData();

            //Bind the DataGrid to the customer data
            DG1.DataContext = custdata;
        }
        public ObservableCollection<Monitor> customers;

        public ObservableCollection<Monitor> GetData()
        {            
             customers = new ObservableCollection<Monitor>();
            System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;

            for (int i = 0; i < scr.Length; i++)
            {
                customers.Add(new Monitor { number=i+1, name = scr[i].DeviceName, resolution = scr[i].Bounds.Width.ToString() + "/" + scr[i].Bounds.Height.ToString(), showing = false });
          
            }
          
            return customers;
        }

        private void DG1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
           
        }

        private void DG1_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
           
        }

        private void DG1_CurrentCellChanged(object sender, EventArgs e)
        {
           
        }

        private void DG1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
         
        }

        private void DG1_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (DG1.SelectedIndex != -1 && DG1.SelectedIndex < System.Windows.Forms.Screen.AllScreens.Length)
            {
                FirstFloor.ModernUI.App.Content.Monitor MON = (FirstFloor.ModernUI.App.Content.Monitor)DG1.SelectedItem;
                if (MON.showing == true)
                {
                    //close
                    for (int i = 0; i < MainWindow.panels.Count; i++)
                    {
                        if (MainWindow.panels[i].screen_number == MON.number)
                        {
                            customers[DG1.SelectedIndex].showing = false;
                            DG1.DataContext = null;
                            DG1.DataContext = customers;
                            DG1.UpdateLayout();
                            MainWindow.panels[i].win.Close();
                        }
                    }
                }
                else
                {
                    win_options_window opt = new win_options_window();
                    if (opt.ShowDialog() == true)
                    {
                        if (opt.temp1.IsChecked == true)
                        {
                            FirstFloor.ModernUI.App.Windows.temp1 t1 = new Windows.temp1();
                            objects.panel pan = new objects.panel(MainWindow.panels.Count + 1, 1, t1);
                            FirstFloor.ModernUI.App.MainWindow.panels.Add(pan);

                            System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;
                            System.Drawing.Point p = new System.Drawing.Point(scr[DG1.SelectedIndex].Bounds.Location.X, scr[DG1.SelectedIndex].Bounds.Location.Y);

                            t1.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

                            t1.Left = p.X;
                            t1.Top = p.Y;

                            customers[DG1.SelectedIndex].showing = true;
                            DG1.DataContext = null;
                            DG1.DataContext = customers;
                            DG1.UpdateLayout();

                            t1.Show();
                            t1.WindowState = System.Windows.WindowState.Maximized;
                        }

                        else if (opt.temp4.IsChecked == true)
                        {

                            Windows.video t1 = new Windows.video();


                            // t1.srvMessage.Text = ((FirstFloor.ModernUI.App.Content.KPPMessages.msg)msgsel.DG1.SelectedItem).name;

                            objects.panel pan = new objects.panel(MainWindow.panels.Count + 1, 1, t1);
                            FirstFloor.ModernUI.App.MainWindow.panels.Add(pan);
                            System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;
                            System.Drawing.Point p = new System.Drawing.Point(scr[DG1.SelectedIndex].Bounds.Location.X, scr[DG1.SelectedIndex].Bounds.Location.Y);
                            t1.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

                            t1.Left = p.X;
                            t1.Top = p.Y;

                            customers[DG1.SelectedIndex].showing = true;
                            DG1.DataContext = null;
                            DG1.DataContext = customers;
                            DG1.UpdateLayout();

                            t1.Show();
                            t1.WindowState = System.Windows.WindowState.Maximized;


                        }

                        else if (opt.temp5.IsChecked == true)
                        {

                            Windows.message t1 = new Windows.message();

                            Windows.messageSelector msgsel = new Windows.messageSelector();

                            if (msgsel.ShowDialog() == true)
                            {
                                t1.srvMessage.Text = ((FirstFloor.ModernUI.App.Content.KPPMessages.msg)msgsel.DG1.SelectedItem).name;

                                objects.panel pan = new objects.panel(MainWindow.panels.Count + 1, 1, t1);
                                FirstFloor.ModernUI.App.MainWindow.panels.Add(pan);
                                System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;
                                System.Drawing.Point p = new System.Drawing.Point(scr[DG1.SelectedIndex].Bounds.Location.X, scr[DG1.SelectedIndex].Bounds.Location.Y);
                                t1.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

                                t1.Left = p.X;
                                t1.Top = p.Y;

                                customers[DG1.SelectedIndex].showing = true;
                                DG1.DataContext = null;
                                DG1.DataContext = customers;
                                DG1.UpdateLayout();

                                t1.Show();
                                t1.WindowState = System.Windows.WindowState.Maximized;
                            }
                            else MessageBox.Show("Не выбрано сообщение!");


                        }

                        else if (opt.temp6.IsChecked == true)
                        {
                            Windows.SlideShow t1 = new Windows.SlideShow();
                            objects.panel pan = new objects.panel(MainWindow.panels.Count + 1, 1, t1);
                            FirstFloor.ModernUI.App.MainWindow.panels.Add(pan);
                            System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;
                            System.Drawing.Point p = new System.Drawing.Point(scr[DG1.SelectedIndex].Bounds.Location.X, scr[DG1.SelectedIndex].Bounds.Location.Y);
                            t1.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

                            t1.Left = p.X;
                            t1.Top = p.Y;

                            customers[DG1.SelectedIndex].showing = true;
                            DG1.DataContext = null;
                            DG1.DataContext = customers;
                            DG1.UpdateLayout();

                            t1.Show();
                            t1.WindowState = System.Windows.WindowState.Maximized;
                        }

                        else if (opt.temp7.IsChecked == true)
                        {
                            Windows.SlideShow t1 = new Windows.SlideShow(1);

                            objects.panel pan = new objects.panel(MainWindow.panels.Count + 1, 1, t1);
                            FirstFloor.ModernUI.App.MainWindow.panels.Add(pan);
                            System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;
                            System.Drawing.Point p = new System.Drawing.Point(scr[DG1.SelectedIndex].Bounds.Location.X, scr[DG1.SelectedIndex].Bounds.Location.Y);
                            t1.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

                            t1.myImage.Width = scr[DG1.SelectedIndex].WorkingArea.Width;
                            t1.myImage2.Width = scr[DG1.SelectedIndex].WorkingArea.Width;

                            t1.myImage.Height = scr[DG1.SelectedIndex].WorkingArea.Height;
                            t1.myImage2.Height = scr[DG1.SelectedIndex].WorkingArea.Height;

                            t1.Left = p.X;
                            t1.Top = p.Y;
                            customers[DG1.SelectedIndex].showing = true;
                            DG1.DataContext = null;
                            DG1.DataContext = customers;
                            DG1.UpdateLayout();

                            t1.Show();
                            t1.WindowState = System.Windows.WindowState.Maximized;
                        }
                    }

                }
            }
          //  FirstFloor.ModernUI.App.Content.Monitor M = (FirstFloor.ModernUI.App.Content.Monitor)DG1.CurrentItem;
        }

        private void DG1_MouseDown(object sender, MouseButtonEventArgs e)
        {
         
        }

        private void DG1_MouseUp(object sender, MouseButtonEventArgs e)
        {

         
        }

        private void openWindow(object sender, RoutedEventArgs e)
        {
        
        }

        private void openWindow(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < FirstFloor.ModernUI.App.MainWindow.panels.Count; i++)
            {
                FirstFloor.ModernUI.App.MainWindow.panels[i].win.Close();
            }
        }

        private void openWindow(object sender, EventArgs e)
        {

        }

        
    }
}
