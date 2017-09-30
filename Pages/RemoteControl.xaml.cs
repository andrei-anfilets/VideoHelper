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
using System.Windows;

using FirstFloor.ModernUI.App;

using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;

using FirstFloor.ModernUI.Presentation;
using System.Globalization;



using System.Collections.ObjectModel;
using System.Collections;

using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using FirstFloor.ModernUI.App.Content;

namespace FirstFloor.ModernUI.App.Pages
{
    /// <summary>
    /// Interaction logic for DpiAwareness.xaml
    /// </summary>
    public partial class DpiAwareness : UserControl
    {
        public static Windows.tmp Currwin;
        public static bool remote = false;

        bool devS = false;
        bool denyS = false;
        bool advS = false;
        bool msgS = false;

        public ObservableCollection<dev> kppDevList;
        public ObservableCollection<KPPDeny.deny> kppDenyList;
        public ObservableCollection<KPPMessages.msg> kppMsgList;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        ObservableCollection<FirstFloor.ModernUI.App.Content.Monitor> custdata;

        DependencyPropertyDescriptor dp = DependencyPropertyDescriptor.FromProperty(Label.ContentProperty, typeof(Label));

        public panelState currState;


        private int groupId = 1;
        private int linkId = 5;

        private int CurrPanel = 0;


        List<remotePanel> remotePanels = new List<remotePanel>();

        Dictionary<string, string> panel_list = new Dictionary<string, string>();
        Dictionary<string, panelState> panel_states = new Dictionary<string, panelState>();

        public static bool RemoteControlEnabled = false;


        public DpiAwareness()
        {
            InitializeComponent();
           

            panel_states = new Dictionary<string, panelState>();
            currState = new panelState();


            worker.DoWork += worker_DoWork;
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            ipAddr.Text = ipAddress.ToString();
            powerButton.Visibility = System.Windows.Visibility.Collapsed;
            string panelName = "";
            //Windows.panel_name pn = new Windows.panel_name();
            //pn.panelBoxName.Text = string.Format(CultureInfo.InvariantCulture, "Панель {0}",
            //        groupId + 1);
            //if (pn.ShowDialog().Value == true)
            //{
            //    panelName = pn.panelBoxName.Text;
            //}

            //else panelName = string.Format(CultureInfo.InvariantCulture, "Панель {0}",
            //            ++groupId);
            // add group command
            this.AddGroup.Command = new RelayCommand(o => {
                if (!panel_list.ContainsKey(panelNameBox.Text))
                {
                    panel_list.Add(panelNameBox.Text, ipAddr.Text);
                    this.Menu.LinkGroups.Add(new LinkGroup
                    {
                        DisplayName = string.Format(CultureInfo.InvariantCulture, panelNameBox.Text,
                        ++groupId)

                    });
                    remoteControlPanel.Visibility = Visibility.Visible;
                    RemoteControlEnabled = true;

                }
                else
                {
                    MessageBox.Show("Нельзя добавить 2 панели с одинаковым именем!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                });

            // add link to selected group command

            this.Menu.SelectedLink.PropertyChanged += SelectedGroup_PropertyChanged;
            

            // remove selected group command
            this.RemoveGroup.Command = new RelayCommand(o => { 
                this.Menu.LinkGroups.Remove(this.Menu.SelectedLinkGroup);
                if (Menu.LinkGroups.Count==1)
                {
                    RemoteControlEnabled = false;
                    remoteControlPanel.Visibility = Visibility.Collapsed;
                }
            }, o => this.Menu.SelectedLinkGroup != null);

            LoremIpsum dev = new LoremIpsum();
            kppDevList = dev.kppdevices;
            DGdev.DataContext = null;
            DGdev.DataContext = kppDevList;
            DGdev.Visibility = Visibility.Collapsed;


            KPPDeny den = new KPPDeny();
            kppDenyList = den.kkppdensource;
            DGdeny.DataContext = null;
            DGdeny.DataContext = kppDenyList;
            DGdeny.Visibility = Visibility.Collapsed;

            KPPMessages msg = new KPPMessages();
            kppMsgList = msg.kkppdensource;
            DGmsg.DataContext = null;
            DGmsg.DataContext = kppMsgList;
            DGmsg.Visibility = Visibility.Collapsed;

            QueueShowPanel.Visibility = Visibility.Collapsed;
        }

        private void SelectedGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
          
        }



        /// <summary>
        ////Объект для передачи команды удаленному компьютеру
        /// </summary>
        client cl;
        String ipAdr = "127.0.0.1";
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                cl = new client(ipAdr);
                if (cl.StartClient(1, "-", "-") == "ok")
                {
                    this.Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        // hidePanel(connectPanel);
                        powerButton.Visibility = System.Windows.Visibility.Visible;
                        powerButton.IsEnabled = true;
                    }));

                    this.Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        cl.StartClient(2, "-", "-");
                        //   DG1.DataContext = cl.custdata;
                    }));
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Адрес  не доступен!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }            
        }

        void hidePanel(StackPanel pan)
        {           
            pan.Visibility = System.Windows.Visibility.Collapsed;
           // panels.Visibility = System.Windows.Visibility.Visible;
        }
        void showPanel(StackPanel pan)
        {
            pan.Visibility = System.Windows.Visibility.Visible;            
            //panels.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ipAdr = ipAddr.Text;
            worker.RunWorkerAsync();
            powerButton.Visibility = System.Windows.Visibility.Visible;

        }

        private void MenuItemChanged(object sender, RoutedEventArgs e)
        {
          
            
        }

        /// <summary>
        ////Вырубает удаленный компьютер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModernButton_Click_1(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(3, "-", "-") == "ok")
            {
              //  showPanel(connectPanel);
            }
        }

        void Button_Click_2(object sender, RoutedEventArgs e)
        {
          //  NavigationService.Navigate(new Uri("panel1.xaml", UriKind.Relative));
          //   NavigationService.Navigate();
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void QueueShow(object sender, RoutedEventArgs e)
        {
            if (QueueShowPanel.Visibility == Visibility.Visible)
            { QueueShowPanel.Visibility = Visibility.Collapsed; }
            else QueueShowPanel.Visibility = Visibility.Visible;
        }

        private void DenyDevicesClick(object sender, RoutedEventArgs e)
        {
            if (DGdeny.Visibility == Visibility.Visible)
            { DGdeny.Visibility = Visibility.Collapsed; }
            else DGdeny.Visibility = Visibility.Visible;
        }


        private void kppDevClick(object sender, RoutedEventArgs e)
        {
            if (DGdev.Visibility == Visibility.Visible)
            { DGdev.Visibility = Visibility.Collapsed; }
            else DGdev.Visibility = Visibility.Visible;
        }

        private void SrvMsgClick(object sender, RoutedEventArgs e)
        {
            if (DGmsg.Visibility == Visibility.Visible)
            { DGmsg.Visibility = Visibility.Collapsed; }
            else DGmsg.Visibility = Visibility.Visible;
        }



        public String VideoSource()
        {

            string rez = "-";
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "VIDEO Files|*.wmv;*.avi;*.mp4;*.mpeg;*.mkv;";
            ofd.Title = "Выбор видео ...";
            // string folderpath = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\videos\\";
            //  string packUri = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName; //+ "\\Images\\empty.png";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // System.IO.File.Copy(ofd.FileName, folderpath + ofd.SafeFileName, true);
                // packUri = folderpath + ofd.SafeFileName;
                return ofd.FileName;
            }
            else return "-";
        }

        private void AdvClick(object sender, RoutedEventArgs e)
        {
            if (DGadv.Visibility == Visibility.Visible)
            { DGadv.Visibility = Visibility.Collapsed; }
            else DGadv.Visibility = Visibility.Visible;
        }

        private void TickerClick(object sender, RoutedEventArgs e)
        {
            if (DGticker.Visibility == Visibility.Visible)
            { DGticker.Visibility = Visibility.Collapsed; }
            else DGticker.Visibility = Visibility.Visible;
        }



        private void DateTimeClick(object sender, RoutedEventArgs e)
        {
            if (DGtime.Visibility == Visibility.Visible)
            { DGtime.Visibility = Visibility.Collapsed; }
            else DGtime.Visibility = Visibility.Visible;
        }


        #region Изменение текста сообщения в таблице и параметров       
        String srvMessage = "";
        private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
               srvMessage = kppMsgList[messageTable.SelectedIndex].name;
               sendCurrMessage();
            }
            catch (Exception err)
            { }
        }       

        public void enableSrvMessage(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(11, srvMessage, "-") == "ok")
            {
                 //showPanel(connectPanel);
            }           
        }

        //Отправка текста сообщения удаленному компьютеру
        public void sendCurrMessage()
        {
            cl = new client(ipAdr);
            if (cl.StartClient(1111, srvMessage, "-") == "ok")
            {
            }
        }

        public void disableSRVMEssage(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(12, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        public void setSrvMsgManual(object sender, TextChangedEventArgs e)
        {
            srvMessage = userMsgBox.Text;
        }
        public void MenuItem_Click_21(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(13, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        public void MenuItem_Click_22(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(14, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        public bool backgr = false;
        public bool font = false;
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //colorpicker1.Visibility = System.Windows.Visibility.Visible;
            backgr = true;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
          //  colorpicker1.Visibility = System.Windows.Visibility.Visible;
            font = true;
        }
        private void colorpicker1_ColorChanged(object sender, RoutedEventArgs e)
        {
            if (backgr)
            {
              //  Currwin.Background = new SolidColorBrush(colorpicker1.Color);
            }
            else
            {
               // Currwin.srvMessage.Foreground = new SolidColorBrush(colorpicker1.Color);
            }

            backgr = false;
            font = false;
            //colorpicker1.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion

        #region Обработка событий рекламного блока
        String videoSource = "-";
        private void enableAdBlock(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(30, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {

            }

        }

        private void disableAdBlock(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(31, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {

            }

        }
        private void advButton_Click_1(object sender, RoutedEventArgs e)
        {
            videoSource = VideoSource();

            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(100, videoSource.Split('\\')[videoSource.Split('\\').Length-1], System.Convert.ToBase64String(System.IO.File.ReadAllBytes(videoSource)));
                //  cl.StartClient();
            }
            catch (Exception err)
            {

            }       
           
          

        }
        #endregion

       #region Обработка событий для бегущей строки
        private void enableRunningString(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(17, TickerTextBox.Text, "-") == "ok")
            {
                //  showPanel(connectPanel);
            }           
        }

        private void disableRunningString(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(18, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        private void increaseTextBlock(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(-25, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }
        private void decreaseTextBlock(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(25, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }
        private void plusRunnStr(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(26, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }
       #endregion

        #region ВРЕМЯ в углу ПАНЕЛИ
        private void enableTime(object sender, RoutedEventArgs e)
        {

            cl = new client(ipAdr);
            if (cl.StartClient(15, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        private void disableTime(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(16, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }
  
        

        private void downDateBox(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(19, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        private void decreaseDateBox(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(20, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        private void increaseDateBox(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(21, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

      

        private void UpDateBox(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(22, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        private void LeftDateBox(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(23, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }

        private void RightUpDateBox(object sender, RoutedEventArgs e)
        {
            cl = new client(ipAdr);
            if (cl.StartClient(24, "-", "-") == "ok")
            {
                //  showPanel(connectPanel);
            }
        }
        #endregion


        String bannerPath = "";
        String filePath = "";
        #region События связанные с баннерной ркламой, Загрузка баннера
        private void Banner_Click_1(object sender, RoutedEventArgs e)
        {

            //  bannerPath = getImageSource(1).ToString();
            bannerPath = getImageSource(1).ToString();
           

            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(101, filePath.Split('\\')[filePath.Split('\\').Length - 1], System.Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath)));
                
            }
            catch (Exception err)
            {

            } 

        }

        public ImageSource getImageSource(int task)
        {
            string rez = "-";
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            ofd.Title = "Выбор изображения ...";
            string folderpath = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\images\\";
            string packUri = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\empty.png";
            if (task != 0)
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = folderpath + ofd.SafeFileName;
                    System.IO.File.Copy(ofd.FileName, folderpath + ofd.SafeFileName, true);
                    packUri = folderpath + ofd.SafeFileName;
                }
            }
            return new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
        }

        //Включение баннерной рекламы
        private void enableBanner(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(33, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }

        }
        // Отключение баннерной рекламы
        private void disableBanner(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(34, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }

        private void LeftBanner(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(35, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }

        private void RightBanner(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(36, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }

        private void UpBanner(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(37, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }

        private void downBanner(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(38, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }

        private void increaseBanner(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(39, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }

        private void decreaseBanner(object sender, RoutedEventArgs e)
        {
            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(40, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }

        #endregion
        //Отображение средств домотра на кпп
        public void enableKppDevSlide(object sender, RoutedEventArgs e)
        {
            //сбор активных средств
            string queue = "";
            foreach (var it in (ObservableCollection<dev>)DGdev.DataContext)
            {
               
                if (it.showing)
                {
                    if (queue.Length > 0)
                    {
                        queue = queue + ",";
                    }
                    queue = queue + it.ID;
                }                
            }
            //---------*************
          
            
            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(7, queue, "-");             
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }


        //отправляет список отображаемых средств досмотра
        private void sendKppDevQueue()
        {
            if (DGdev.DataContext==null)
            {
                return;
            }
            //сбор активных средств
            string queue = "";
            foreach (var it in (ObservableCollection<dev>)DGdev.DataContext)
            {
                if (it.showing)
                {
                    if (queue.Length > 0)
                    {
                        queue = queue + ",";
                    }
                    queue = queue + it.ID;
                }
            }
            //---------*************
            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                string rez = cl.StartClient(71, queue, "-");
            }
            catch (Exception err)
            {
                
            }
        }

        //Выключение средств досмотра на кпп
        private void disableKppDevSlide(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(8, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }


        //Включекние списа запрещенных предметов
        private void enableDenyList(object sender, RoutedEventArgs e)
        {
            //сбор запрещенных средств
            string queue = "";
            foreach (var it in (ObservableCollection<KPPDeny.deny>)DGdeny.DataContext)
            {
                
                if (it.showing)
                {
                    if (queue.Length > 0)
                    {
                        queue = queue + ",";
                    }
                    queue = queue + it.ID;
                }
                
            }
            //---------*************
            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(9, queue, "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }
        
        //отправка списка запрещенных предметов
        private void sendDenyList()
        {
            if (DGdeny.DataContext==null)
            {
                return;
            }
            //сбор запрещенных средств
            string queue = "";
            foreach (var it in (ObservableCollection<KPPDeny.deny>)DGdeny.DataContext)
            {                
                if (it.showing)
                {
                    if (queue.Length > 0)
                    {
                        queue = queue + ",";
                    }
                    queue = queue + it.ID;
                }
            }
            //---------*************
            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(91, queue, "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }

        //Выключение списка запрещенных предметов
        public void disableDenyList(object sender, RoutedEventArgs e)
        {
            try
            {

                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(10, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка выполнения " + err.ToString());
            }
        }
        
        public bool IsEnable = false;  
        int currSec = 0;
        void timer_Tick(object sender, EventArgs e)
        {
            currSec++;
        }

        /// <summary>
        ////Остановка вспроизведения плейлиста
        /// </summary>
        public void stopQueue()
        {
            IsEnable = false;
            //dispatcherTimer.Stop();
        }

        private void startQueueButtonClick(object sender, RoutedEventArgs e)
        {
            string queue = "";
            
            System.Threading.Thread.Sleep(1000);

            foreach (var it in orderControl.lbTwo.Items)
            {
                item p = (item)it;
                queue = queue + p.ID + ",";
            }   
            if (queue.Length > 2)
            {
                queue = queue.Substring(0, queue.Length - 1);
            }
            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(5, queue, "-");
            }
            catch (Exception err)
            { 
            
            }        
        }       

        private void worker_RunWorkerCompleted(object sender,
                                       RunWorkerCompletedEventArgs e)
        {
            int a = 0;
            a++;
        }

        private void stopQueueButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                cl = new client(panel_list[Menu.SelectedLinkGroup.DisplayName]);
                cl.StartClient(6, "-", "-");
                //  cl.StartClient();
            }
            catch (Exception err)
            {

            } 
        }

        private void RemoveGroup_Click(object sender, RoutedEventArgs e)
        {
            if (this.Menu.SelectedLinkGroup != null)
            {
                panel_list.Remove(this.Menu.SelectedLinkGroup.DisplayName);
            }
        }



       
       
        private void Menu_SelectedSourceChanged(object sender, ModernUI.Windows.Controls.SourceEventArgs e)
        {
        }


        //считывает  текущие параметры панели дл яотображения
        private void ReadCurrState()
        {
            Send = false;
            currState.QUEQENABLED = startQueueButton.IsChecked.Value;
            List<item> items = new List<item>();
            foreach (var i in orderControl.lbTwo.Items)
            {
                items.Add(new item() {ID=((item)i).ID, name= ((item)i).name});
            }
            currState.QUEUEitems = items;// orderControl.lbTwo.Items.AsQueryable();
            

            ObservableCollection<dev> kppdevList = new ObservableCollection<dev>();
            foreach (var d in (ObservableCollection<dev>)DGdev.DataContext)
            {
                kppdevList.Add(new dev() {ID=d.ID, name=d.name, path=d.path, showing=d.showing});
            }
            currState.kppDevList = kppdevList;
            currState.ppDevListActive = kppDevListchBox.IsChecked.Value;
            // currState.kppDenyList = ((List<KPPDeny.deny>)DGdeny.DataContext).ToList();


            ObservableCollection<KPPDeny.deny> kppdenyList = new ObservableCollection<KPPDeny.deny>();
            foreach (var d in (ObservableCollection<KPPDeny.deny>)DGdeny.DataContext)
            {
                kppdenyList.Add(new KPPDeny.deny() { ID = d.ID, name = d.name, path = d.path, showing = d.showing });
            }
            currState.kppDenyList = kppdenyList;
            currState.kppDenyListActive = kppdenyListchBox.IsChecked.Value;


            currState.kppMsgListActive = serviceMessagechBox.IsChecked.Value;
            currState.IsAdvertisetACTIVE = advertisingBox.IsChecked.Value;
            currState.RunningStringActive = tickerBox.IsChecked.Value;
            currState.RunninngStringVALUE = TickerTextBox.Text;
            currState.DateTimeActive = datetimeBox.IsChecked.Value;
            Send = true;
        }

        //Восстановить состояние панели
        private void RestorePanleState(panelState stateToRestore)
        {
            Send = false;
            startQueueButton.IsChecked = stateToRestore.QUEQENABLED;
            FirstFloor.ModernUI.App.Content.newTabEmp.PlayList.ItemsSource = null;
            FirstFloor.ModernUI.App.Content.newTabEmp.PlayList.Items.Clear();
            FirstFloor.ModernUI.App.Content.newTabEmp.queueList.Clear();
            orderControl.lbTwo.ItemsSource = null;



            orderControl.lbTwo.ItemsSource = stateToRestore.QUEUEitems;
            kppDevListchBox.IsChecked = stateToRestore.ppDevListActive;
            kppdenyListchBox.IsChecked = stateToRestore.kppDenyListActive;

            if (stateToRestore.kppDevList != null && stateToRestore.kppDevList.Count > 1)
            {
                DGdev.DataContext = null;
                DGdev.DataContext = stateToRestore.kppDevList; 
            }

            if (stateToRestore.kppDenyList!= null && stateToRestore.kppDenyList.Count > 1)
            {
                DGdeny.DataContext = null;
                DGdeny.DataContext = stateToRestore.kppDenyList;
            }
           
            

            serviceMessagechBox.IsChecked = stateToRestore.kppMsgListActive;
            advertisingBox.IsChecked = stateToRestore.IsAdvertisetACTIVE;
            tickerBox.IsChecked = stateToRestore.RunningStringActive;
            TickerTextBox.Text = stateToRestore.RunninngStringVALUE;
            datetimeBox.IsChecked = stateToRestore.DateTimeActive;
            Send = true;
        }

        private void TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }



        public static String PanelIPToSave { get; set; }
        private String _panelNameToSave = "";
        public String PanelNameToSave { get {
                if (_panelNameToSave == null)
                {
                    return "";
                }
                else return _panelNameToSave;
            } set
            {
                _panelNameToSave = value;
            } }
        public static bool Send = true;
        //selected item of menu changing
        private void grIndex_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (Menu.SelectedLinkGroup==null)
            {
                return;
            }
            string currPanelName = Menu.SelectedLinkGroup.DisplayName;
            string selectedIP = "";
            int activePanels = panel_list.Count;
            if (activePanels>0 && panel_list.ContainsKey(currPanelName))
            {
                selectedIP = panel_list[Menu.SelectedLinkGroup.DisplayName];
            }
            if (!panel_list.ContainsKey(currPanelName))
            {
                return;
            }


            ReadCurrState();
            currState.IP_ADDRESS = PanelIPToSave;
            if (panel_states.ContainsKey(PanelNameToSave))
            {
                panel_states.Remove(PanelNameToSave);
            }
            panel_states.Add(PanelNameToSave, currState);

            if (panel_states.ContainsKey(currPanelName))
            {
                RestorePanleState(panel_states[currPanelName]);
            }
            else
            {
                currState = new panelState();
                RestorePanleState(currState);
            }
            


            PanelIPToSave = selectedIP;
            PanelNameToSave = currPanelName;
            currState = new panelState();

        }

        private void DataGrid_SelectedCellsChanged(object sender, RoutedEventArgs e)
        {
            sendKppDevQueue();
        }

        private void DataGrid_SelectedCellsChanged_1(object sender, SelectedCellsChangedEventArgs e)
        {
            sendDenyList();
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {

        }
    }
}
