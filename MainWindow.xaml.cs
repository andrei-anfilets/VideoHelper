using FirstFloor.ModernUI.Windows.Controls;
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
using System.Windows.Shapes;
using System.Windows.Navigation;
using FirstFloor.ModernUI.App.objects;
using System.ComponentModel;

using System.Threading;
using System.IO;
using System.Security.AccessControl;



namespace FirstFloor.ModernUI.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : FirstFloor.ModernUI.Windows.Controls.ModernWindow
    {
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker cmd_worker = new BackgroundWorker();

        public static Content.panel1 ControlPanel;
        FirstFloor.ModernUI.App.Content.SettingsAppearanceViewModel sawm;
        public static string specificFolder;
        public MainWindow()
        {



            InitializeComponent();
           
            try
            {
               // DirectoryInfo info = new DirectoryInfo(@"c:\Program Files (x86)\VideoHelper");
          //      DirectoryInfo info = new DirectoryInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
              //  SetAccessGroup(@"c:\Program Files (x86)\VideoHelper");

            //    DirectorySecurity security = info.GetAccessControl();
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                specificFolder =  folder + "\\VideoHelper";

                // Check if folder exists and if not, create it
                if (!Directory.Exists(specificFolder))
                {

                    Directory.CreateDirectory(specificFolder);
                    
                        File.Copy("Style.xml", specificFolder + "\\Style.xml");
                        File.Copy("KPPMessage.xml", specificFolder + "\\KPPMessage.xml");
                        File.Copy("KPPDevices.xml", specificFolder + "\\KPPDevices.xml");
                        File.Copy("KPPDeny.xml", specificFolder + "\\KPPDeny.xml");
                        if (File.Exists("Settings.xml"))
                            File.Copy("Settings.xml", specificFolder + "\\Settings.xml");
                        if (File.Exists("PanelsOrder.xml"))
                            File.Copy("PanelsOrder.xml", specificFolder + "\\PanelsOrder.xml");
                        File.Copy("info_7.txt", specificFolder + "\\info_7.txt");
                }
                else
                    if (!File.Exists(specificFolder + "\\info_7.txt"))
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(specificFolder);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                   
                    File.Copy("Style.xml", specificFolder + "\\Style.xml");
                    File.Copy("KPPMessage.xml", specificFolder + "\\KPPMessage.xml");
                    File.Copy("KPPDevices.xml", specificFolder + "\\KPPDevices.xml");
                    File.Copy("KPPDeny.xml", specificFolder + "\\KPPDeny.xml");
                    if (File.Exists("Settings.xml"))
                        File.Copy("Settings.xml", specificFolder + "\\Settings.xml");
                    if (File.Exists("PanelsOrder.xml"))
                        File.Copy("PanelsOrder.xml", specificFolder + "\\PanelsOrder.xml");
                    File.Copy("info_7.txt", specificFolder + "\\info_7.txt");

                }



            }
            catch(Exception err)
            {
            
            }

           sawm = new Content.SettingsAppearanceViewModel();

            try
            {
                UserStyle loadSt = (new UserStyle()).loadSettings();
                sawm.SelectedTheme.Source = new Uri(loadSt.themeSource, UriKind.Relative);
                sawm.SelectedAccentColor = (Color)ColorConverter.ConvertFromString(loadSt.themeColor);
            }
            catch (Exception err)
            {
                sawm.SelectedTheme = sawm.Themes[3];
                sawm.SelectedAccentColor = sawm.AccentColors[3];            
            }
            
           
            
            
            worker.DoWork += worker_DoWork;
            worker.RunWorkerAsync(this);

            disp = Dispatcher;

            ////TODO включение сервера обмена сообщениями
            cmd_worker.DoWork +=cmd_worker_DoWork;
            cmd_worker.RunWorkerAsync();
          
        }

        public static bool HasWritePermissionOnDir(string path)
        {
            var writeAllow = false;
            var writeDeny = false;
            var accessControlList = Directory.GetAccessControl(path);
            if (accessControlList == null)
                return false;
            var accessRules = accessControlList.GetAccessRules(true, true,
                                        typeof(System.Security.Principal.SecurityIdentifier));
            if (accessRules == null)
                return false;

            foreach (FileSystemAccessRule rule in accessRules)
            {
                if ((FileSystemRights.Write & rule.FileSystemRights) != FileSystemRights.Write)
                    continue;

                if (rule.AccessControlType == AccessControlType.Allow)
                    writeAllow = true;
                else if (rule.AccessControlType == AccessControlType.Deny)
                    writeDeny = true;
            }

            return writeAllow && !writeDeny;
        }

        server srv;
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {           
            srv = new server();
            srv.mainWinRef = (MainWindow)e.Argument;
            srv.StartListening();
        }

        int curr_cmd = -2;
        bool exec = false;


        string srv_param1 = "";
        string srv_data = "";
        void cmd_worker_DoWork(object sender, DoWorkEventArgs e)
        { 
            while (curr_cmd != -100)
            {
                if (srv != null)
                {
                    System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(() =>
                    {
                        if (curr_cmd != srv.CurrActionIndex || !srv.IsCurrActionExecuted)
                        {
                            curr_cmd = srv.CurrActionIndex;
                            srv_param1 = srv.par1;
                            srv_data = srv.data;
                            exec = false;
                        }
                    });

                }
                //включение панели
                if (curr_cmd == 1 && !exec)
                {
                    logger.Log("Удаленное подключение произведено " + srv_data);
                    disp.Invoke(() =>
                    {
                        this.ContentSource = new System.Uri("/Content/OrderControl.xaml", UriKind.Relative);
                        this.ContentSource = new System.Uri("/Pages/ControlsStyles.xaml", UriKind.Relative);

                        //Uri uri = new Uri("/Pages/ControlsStyles.xaml", UriKind.Relative);

                        //Frame.NavigationService.Navigate(new Uri("/Content/OrderControl.xaml", UriKind.Relative));
                        //NavigationService ns = NavigationService.GetNavigationService(this);
                        //       ns.Navigate(uri);
                        // ns.Navigate(new Uri("/Content/OrderControl.xaml", UriKind.Relative));                        


                        //FirstFloor.ModernUI.App.Pages.ControlsStyles.Instance.controlTab.SelectedSource = new System.Uri("/Content/OrderControl.xaml", UriKind.Relative);

                    });
                    exec = true;
                }
                //запуск очереди
                else if (curr_cmd == 5 && !exec)
                {

                    logger.Log("Запуск очереди - " + srv_data);
                    disp.Invoke(() =>
                    {
                        string queue = srv_param1;
                        if (queue.Length != 0)
                        {
                            if (FirstFloor.ModernUI.App.Content.newTabEmp.PlayList != null)
                            {
                                FirstFloor.ModernUI.App.Content.newTabEmp.PlayList.Items.Clear();
                            }
                            else
                            {
                                FirstFloor.ModernUI.App.Content.newTabEmp.PlayList = new ListBox();
                            }
                            string[] ids = queue.Split(',');
                            for (int i = 0; i < ids.Length; i++)
                            {
                                if (ids[i] != "" && ids[i] != "-")

                                    FirstFloor.ModernUI.App.Content.newTabEmp.PlayList.Items.Add(new FirstFloor.ModernUI.App.Content.item() { ID = int.Parse(ids[i]) });
                            }
                        }

                        ControlPanel.startQueueButtonClick(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                //остановка очереди
                else if (curr_cmd == 6 && !exec)
                {
                    logger.Log("Остановка очереди - " + srv.data);
                    disp.Invoke(() =>
                    {
                        ControlPanel.stopQueue();
                    });
                    exec = true;
                }
                //запуск устройств кпп
                else if ((curr_cmd == 7 || curr_cmd == 71) && !exec)
                {
                    logger.Log("Запуск устройств кпп - " + srv.data);
                    disp.Invoke(() =>
                    {
                        string queue = srv_param1;
                        if (queue.Length != 0)
                        {
                            string[] ids = queue.Split(',').Where(q => q.Length > 0).ToArray<string>();
                            foreach (var item in (System.Collections.ObjectModel.ObservableCollection<FirstFloor.ModernUI.App.Content.dev>)FirstFloor.ModernUI.App.Content.panel1.instance.DGdev.DataContext)
                            {
                                if (ids.Where(d => d.ToString() == item.ID.ToString()).Count() > 0)
                                {
                                    item.showing = true;
                                }
                                else
                                {
                                    item.showing = false;
                                }
                            }
                        }
                        if (curr_cmd==7)
                        {
                            ControlPanel.kppDevListchBox.IsChecked = true;
                        }                        
                    });
                    exec = true;
                }
                //остановка устройств
                else if (curr_cmd == 8 && !exec)
                {
                    logger.Log("Остановка устройств кпп - " + srv.data);
                    disp.Invoke(() =>
                    {
                        ControlPanel.kppDevListchBox.IsChecked = false;
                    });
                    exec = true;
                }
                //запуск запрещенных предметов
                else if ((curr_cmd == 9 || curr_cmd==91) && !exec)
                {

                    logger.Log("Запуск запрещенных предметов кпп - " + srv.data);
                    disp.Invoke(() =>
                    {
                        string queue = srv_param1;
                        if (queue.Length != 0)
                        {
                            string[] ids = queue.Split(',').Where(q => q.Length > 0).ToArray<string>();
                            foreach (var item in (System.Collections.ObjectModel.ObservableCollection<FirstFloor.ModernUI.App.Content.KPPDeny.deny>)FirstFloor.ModernUI.App.Content.panel1.instance.DGdeny.DataContext)
                            {
                                if (ids.Where(d => d.ToString() == item.ID.ToString()).Count() > 0)
                                {
                                    item.showing = true;
                                }
                                else
                                {
                                    item.showing = false;
                                }
                            }
                        }
                        if (curr_cmd==9)
                        {
                            ControlPanel.kppdenyListchBox.IsChecked = true;
                        }
                        
                    });
                    exec = true;
                }
                //отсановка списка запрещенных предметов
                else if (curr_cmd == 10 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.kppdenyListchBox.IsChecked = false;
                    });
                    logger.Log("Остановка запрещенных предметов кпп - " + srv.data);
                    exec = true;
                }
                #region MESSAGE_CONTROL
                //получение текста сообщения
                else if (curr_cmd == 1111)
                {
                    ControlPanel.srvMessage = srv_param1;
                    //disp.Invoke(() =>
                    //{
                        
                    //});
                    exec = true;
                }
                //Включение сообщений
                else if (curr_cmd == 11 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.srvMessage = srv_param1;
                        ControlPanel.enableSrvMessage(ControlPanel, new RoutedEventArgs());
                    });
                    FirstFloor.ModernUI.App.logger.Log("Включение сервисного сообщения " + srv.data);
                    exec = true;
                }
                //выключение сообщения
                else if (curr_cmd == 12 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.disableSRVMEssage(ControlPanel, new RoutedEventArgs());
                    });
                    FirstFloor.ModernUI.App.logger.Log("Выключение сервисного сообщения " + srv.data);
                    exec = true;
                }
                //Увеличивыать шрифт
                else if (curr_cmd == 13 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.MenuItem_Click_21(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                //уменьшить шрифт
                else if (curr_cmd == 14 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.MenuItem_Click_22(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                #endregion
                //показать дату время
                else if (curr_cmd == 15 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.enableTime(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                //убрать дату время
                else if (curr_cmd == 16 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.disableTime(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }


                //Включение бегущей строки
                else if (curr_cmd == 17 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.TickerTextBox.Text = srv.par1;
                        ControlPanel.tickerBox.IsChecked = true;
                        // ControlPanel.enableRunningString(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                //отключение бегущей строки
                else if (curr_cmd == 18 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.tickerBox.IsChecked = false;
                        //ControlPanel.disableRunningString(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                ///операции с датой и временем
                else if (curr_cmd == 19 && !exec)
                {

                    disp.Invoke(() =>
                       {
                           ControlPanel.downDateBox(ControlPanel, new RoutedEventArgs());
                       });
                    exec = true;
                }
                else if (curr_cmd == 20 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.decreaseDateBox(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 21 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.increaseDateBox(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 22 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.UpDateBox(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 23 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.LeftDateBox(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 24 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.RightUpDateBox(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 25 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.minusRunnStr(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == -25 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.plusRunnStr(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }

                else if (curr_cmd == 26 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.plusRunnStr(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }


                else if (curr_cmd == 30 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.enableAdBlock(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 31 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.disableAdBlock(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }

                else if (curr_cmd == 33 && !exec)
                {

                    disp.Invoke(() =>
                    {

                        ControlPanel.enableBanner(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 34 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.disableBanner(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 35 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.LeftBanner(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 36 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.RightBanner(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 37 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.UpBanner(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 38 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.downBanner(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 39 && !exec)
                {
                    disp.Invoke(() =>
                    {
                        ControlPanel.increaseBanner(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }
                else if (curr_cmd == 40 && !exec)
                {

                    disp.Invoke(() =>
                    {
                        ControlPanel.decreaseBanner(ControlPanel, new RoutedEventArgs());
                    });
                    exec = true;
                }

                else if (curr_cmd == 100 && !exec)
                {
                    System.IO.File.WriteAllBytes("rec\\" + srv.par1, System.Convert.FromBase64String(srv.par2));
                    disp.Invoke(() =>
                    {
                        ControlPanel.videoSource = "rec\\" + srv.par1;
                    });
                    exec = true;
                }
                else if (curr_cmd == 101 && !exec)
                {
                    System.IO.File.WriteAllBytes("rec\\" + srv.par1, System.Convert.FromBase64String(srv.par2));
                    disp.Invoke(() =>
                    {
                        //  Currwin.banner1.Source = new ImageSourceConverter().ConvertFromString(bannerPath) as ImageSource;// new BitmapImage(new Uri(Path.get "\\bin\\Debug\\"+bannerPath, UriKind.Relative));

                        ControlPanel.bannerPath = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\bin\\Debug\\" + "rec\\" + srv.par1;

                    });
                    exec = true;
                }
                srv.IsCurrActionExecuted = true;
            }
           
        }

        public static List<panel> panels = new List<panel>();

        private void ModernWindow_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            try
            {
                UserStyle st = new UserStyle();
                st.themeSource = sawm.SelectedTheme.Source.ToString();
                st.themeColor = sawm.SelectedAccentColor.ToString();
                st.saveSettings();
            }
            catch (Exception err)
            { 
            
            }

            for (int i = 0; i < panels.Count; i++)
			{
                try
                {
                    panels[i].win.Close();
                }
                catch (Exception errr)
                { 
                
                }
			}    
           
           // cryptor cr = new cryptor(System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\videos\\");
           // cr.crypt();
        }

        System.Windows.Threading.Dispatcher disp;
        
        public void ChangePage()
        {
            //disp.Invoke(new Action(() =>
          //  {
                // mainWinRef.ChangePage();
                //Uri uri = new Uri("/Pages/ControlsStyles.xaml", UriKind.Relative);
         //       System.Windows.Navigation.NavigationService ns = System.Windows.Navigation.NavigationService.GetNavigationService(this);
          //      ns.Navigate(uri);

               
          //  }));      

           
        }
       
    }
    
}
