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


using System.Collections.ObjectModel;
using System.Collections;

using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;


namespace FirstFloor.ModernUI.App.Content
{
    /// <summary>
    /// Interaction logic for panel1.xaml
    /// </summary>
    
    public partial class panel1 : UserControl
    {
        
        public static Windows.tmp Currwin;
        public static bool remote = false;

        

        bool devS = false;
        bool denyS = false;
        bool advS = false;

       public bool msgS = false;

       public static panel1 instance;
        public static KPPMessages msg;  
        public panel1()
        {
            InitializeComponent();
            //dev
            LoremIpsum lorem = new LoremIpsum();
            kppDevList = lorem.kppdevices;
            DGdev.DataContext = null;
            DGdev.DataContext = kppDevList;
            DGdev.Visibility = Visibility.Collapsed;
            //deny
            KPPDeny den = new KPPDeny();
            kppDenyList = den.kkppdensource;
            DGdeny.DataContext = null;
            DGdeny.DataContext = kppDenyList;
            DGdeny.Visibility = Visibility.Collapsed;
            //msg
            msg = new KPPMessages();
            kppMsgList = msg.kkppdensource;
            DGmsg.DataContext = null;
            DGmsg.DataContext = kppMsgList;
            DGmsg.Visibility = Visibility.Collapsed;

            DGadv.Visibility = Visibility.Collapsed;
            DGticker.Visibility = Visibility.Collapsed;
            DGtime.Visibility = Visibility.Collapsed;

            instance = this;

           
          //  dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
           // dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
          //  dispatcherTimer.Start();

            MainWindow.ControlPanel = this;
        }
        public ObservableCollection<dev> kppDevList;
        public ObservableCollection<KPPDeny.deny> kppDenyList;
        public ObservableCollection<KPPMessages.msg> kppMsgList;
        
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        private void DGdev_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DGdev.Visibility == Visibility.Visible)
            { DGdev.Visibility = Visibility.Collapsed;
             kppDevList = (ObservableCollection<dev>)DGdev.DataContext;
            }
            else DGdev.Visibility = Visibility.Visible;
          
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

        private void DenyDevicesClick(object sender, RoutedEventArgs e)
        {
            if (DGdeny.Visibility == Visibility.Visible)
            { DGdeny.Visibility = Visibility.Collapsed;
            kppDenyList = (ObservableCollection<KPPDeny.deny>)DGdeny.DataContext;
            
            }
            else DGdeny.Visibility = Visibility.Visible;
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


            //Изменение текста сообщения в таблице и параметров
           public String srvMessage = "";
            private void DataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    srvMessage = kppMsgList[messageTable.SelectedIndex].name;
                    Currwin.srvMessage.Text = srvMessage;
                }
                catch(Exception err)
                {}
            }

            public void enableSrvMessage(object sender, RoutedEventArgs e)
            {
                msgS = true;
                Currwin.mainGrid.Background = Brushes.White;
                Currwin.srvMessage.Visibility = System.Windows.Visibility.Visible;
                Currwin.srvMessage.Text = srvMessage;
            }

            public void disableSRVMEssage(object sender, RoutedEventArgs e)
            {
            try
            {                
                msgS = false;
                Currwin.srvMessage.Visibility = System.Windows.Visibility.Collapsed;
                Currwin.Background = new SolidColorBrush(Color.FromScRgb(255, 255, 255, 255));
                //Currwin.srvMessage.Foreground = null;
                Currwin.Style = null;
            }
            catch (Exception)
            {

            }
                
            }
            public void SRVMESSAGEEvent(object sender, RoutedEventArgs e)
            {
                IEnumerable<item> Query =
       from it in activeListClick
       where it.ID == 4
       select it;
                if (Query.Count() > 0)
                {
                    // 
                    activeListClick.Remove(Query.ToList()[0]);
                    if (activeListClick.Count == 0) stopQueueButtonClick(sender, e);
                }
                else
                {
                    activeListClick.Clear();
                    activeListClick.Add(new item(4, "Сообщение"));
                    if (activeListClick.Count > 0 && !IsEnable) StartQueu();
                }       
            }


            private void setSrvMsgManual(object sender, TextChangedEventArgs e)
            {
                srvMessage = userMsgBox.Text;
                Currwin.srvMessage.Text = srvMessage;
            }
            public void MenuItem_Click_21(object sender, RoutedEventArgs e)
            {
                Currwin.srvMessage.FontSize += 5;
            }

            public void MenuItem_Click_22(object sender, RoutedEventArgs e)
            {
                Currwin.srvMessage.FontSize -= 5;
            }

            public bool backgr = false;
            public bool font = false;
            private void MenuItem_Click_1(object sender, RoutedEventArgs e)
            {
                colorpicker1.Visibility = System.Windows.Visibility.Visible;
                backgr = true;
            }

            private void MenuItem_Click_2(object sender, RoutedEventArgs e)
            {
                colorpicker1.Visibility = System.Windows.Visibility.Visible;
                font = true;
            }
            private void colorpicker1_ColorChanged(object sender, RoutedEventArgs e)
            {
                if (backgr)
                {
                    Currwin.Background = new SolidColorBrush(colorpicker1.Color);
                }
                else
                {
                   Currwin.srvMessage.Foreground = new SolidColorBrush(colorpicker1.Color);
                }

                backgr = false;
                font = false;
                colorpicker1.Visibility = System.Windows.Visibility.Collapsed;
            }

           public String videoSource = "-";
        //Обработка событий рекламного блока
        public String MediaSource = "-";
        public String MediaSourceNext = "-";

            public void enableAdBlock(object sender, RoutedEventArgs e)
            {
                advS = true;
                Currwin.media1.Visibility = System.Windows.Visibility.Visible;
                if (videoSource != "-")
                {
                    Currwin.media1.Source = new Uri(MediaSource, UriKind.Relative);
                    Currwin.media1.Play();
                    Currwin.Background = new SolidColorBrush(Color.FromScRgb(0,0,0,0));
                }
            }

            public void disableAdBlock(object sender, RoutedEventArgs e)
            {               
                advS = false;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    Currwin.media1.Visibility = System.Windows.Visibility.Hidden;
                    Currwin.media1.Source = null;
                    Currwin.media2.Visibility = System.Windows.Visibility.Hidden;
                    Currwin.media2.Source = null;
                }));   
               
            }
            public void AdBlockEvent(object sender, RoutedEventArgs e)
            {
                IEnumerable<item> Query =
       from it in activeListClick
       where it.ID == 3
       select it;
                if (Query.Count() > 0)
                {
                    // 
                    activeListClick.Remove(Query.ToList()[0]);
                    if (activeListClick.Count == 0) stopQueueButtonClick(sender, e);
                }
                else
                {

                    activeListClick.Add(new item(3, "Реклама"));
                    if (activeListClick.Count > 0 && !IsEnable) StartQueu();
                }
            }

            private void advButton_Click_1(object sender, RoutedEventArgs e)
            {
                MediaSource = VideoSource();
                advButton1.Content = MediaSource;
            }

        //Обработка событий для бегущей строки
        public void enableRunningString(object sender, RoutedEventArgs e)
        {

            Currwin.Canvas.Visibility = Visibility.Visible;
            Currwin.IsEnabled = true;
            Currwin.textBlock1.Content = TickerTextBox.Text;
        }

        public void disableRunningString(object sender, RoutedEventArgs e)
        {
            Currwin.Canvas.Visibility = Visibility.Collapsed;
            Currwin.IsEnabled = false;
        }

        private void increaseTextBlock(object sender, RoutedEventArgs e)
        {
           
        }

        public void decreaseTextBlock(object sender, RoutedEventArgs e)
        {
            if (Currwin.textBlock1.FontSize > 10)
            {
                Currwin.textBlock1.FontSize -= 5;
                Currwin.Canvas.Height -= 5;
            }          
        }

        public void enableTime(object sender, RoutedEventArgs e)
        {            
            Currwin.dateText.Visibility = System.Windows.Visibility.Visible;
            Currwin.timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                Currwin.dateText.Text = DateTime.Now.ToString(FormatBox.Text);
            }, Currwin.Dispatcher);
        }

        public void disableTime(object sender, RoutedEventArgs e)
        {
            Currwin.dateText.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void plusRunnStr(object sender, RoutedEventArgs e)
        {
            Currwin.textBlock1.FontSize += 5;
            Currwin.Canvas.Height += 5;
        }

        public void minusRunnStr(object sender, RoutedEventArgs e)
        {
            Currwin.textBlock1.FontSize -= 5;
            Currwin.Canvas.Height -= 5;
        }


        public void downDateBox(object sender, RoutedEventArgs e)
        {
            Currwin.dateText.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
        }

        public void decreaseDateBox(object sender, RoutedEventArgs e)
        {
            if (Currwin.dateText.FontSize > 10)
            {
                Currwin.dateText.FontSize -= 5;
            }
        }

        public void increaseDateBox(object sender, RoutedEventArgs e)
        {
            Currwin.dateText.FontSize += 5;
        }

        private void ModernButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        public void UpDateBox(object sender, RoutedEventArgs e)
        {
            Currwin.dateText.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }

        public void LeftDateBox(object sender, RoutedEventArgs e)
        {
            Currwin.dateText.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        }

        public void RightUpDateBox(object sender, RoutedEventArgs e)
        {
            Currwin.dateText.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        }


        public String bannerPath="";// =System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\logo.png";

        //Загрузка баннера
        private void Banner_Click_1(object sender, RoutedEventArgs e)
        {

            bannerPath = getImageSource(1).ToString();
         //   ImageSource src = getImageSource(1);
            // отправление файла          
            if (Currwin.banner1.Visibility == System.Windows.Visibility.Visible)
            {
                Currwin.banner1.Source = new ImageSourceConverter().ConvertFromString(bannerPath) as ImageSource;// new BitmapImage(new Uri(Path.get "\\bin\\Debug\\"+bannerPath, UriKind.Relative));
              
            }
        }

        public string getImageSource(int task)
        {
            string rez = "-";
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            ofd.Title = "Выбор изображения ...";
          //  string folderpath = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\images\\";
          //  string packUri = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\empty.png";
            if (task != 0)
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                   // System.IO.File.Copy(ofd.FileName, folderpath + ofd.SafeFileName, true);
                   // packUri = folderpath + ofd.SafeFileName;
                }
            }
          //  MessageBox.Show("выбрана картинка - " + ofd.FileName);
            return ofd.FileName;
        }

        //Включение баннерной рекламы
        public void enableBanner(object sender, RoutedEventArgs e)
        {
          //  MessageBox.Show("отображаетсЯ картинка - " + bannerPath);
            if (bannerPath == "")
            {
                bannerPath = getImageSource(1).ToString();
            }
            try
            {  
                ImageSourceConverter cnv = new ImageSourceConverter();
                Currwin.banner1.Source = cnv.ConvertFromString(bannerPath) as ImageSource;// new BitmapImage(new Uri(Path.get "\\bin\\Debug\\"+bannerPath, UriKind.Relative));
                Currwin.banner1.Visibility = System.Windows.Visibility.Visible;
            }
            catch(Exception err)
            {
                MessageBox.Show("Ошибка - " + err.ToString());            
            }
        }
        // Отключение баннерной рекламы
        public void disableBanner(object sender, RoutedEventArgs e)
        {
            Currwin.banner1.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void LeftBanner(object sender, RoutedEventArgs e)
        {
            Currwin.banner1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
        }

        public void RightBanner(object sender, RoutedEventArgs e)
        {
            Currwin.banner1.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        }

        public void UpBanner(object sender, RoutedEventArgs e)
        {
            Currwin.banner1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }

        public void downBanner(object sender, RoutedEventArgs e)
        {
            Currwin.banner1.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
        }

        public void increaseBanner(object sender, RoutedEventArgs e)
        {
            Currwin.banner1.Width += 50;
        }

        public void decreaseBanner(object sender, RoutedEventArgs e)
        {
            Currwin.banner1.Width  -= 50;
        }

        //Отображение средств домотра на кпп
        public void enableKppDevSlide(object sender, RoutedEventArgs e)
        {
            String exe = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();
            devS = true;          
            Currwin.slideCanvas.Visibility = System.Windows.Visibility.Visible;          
            Currwin.strImagePath = "Images\\videos\\"+videoSource;
          // Currwin.ImageControls = new[] { Currwin.myImage, Currwin.myImage2 };
            String[] files = System.IO.Directory.GetFiles(Currwin.strImagePath);
            Currwin.FrameArray = new BitmapImage[files.Length];
           // for (int i = 0; i < files.Length; i++)
           // {
           //     var uri = new Uri(exe + "/" + files[i]);
           //     Currwin.FrameArray[i] = new BitmapImage(uri);
          //  }
            Currwin.VideoFrames = files;
            Currwin.myImage.Visibility = Visibility.Visible;         
            Currwin.timerImageChange = new DispatcherTimer();
            Currwin.timerImageChange.Interval = new TimeSpan(0, 0, 0, 0, 33);
           Currwin.timerImageChange.Tick += new EventHandler(Currwin.timerImageChange_Tick);
           Currwin.timerImageChange.Start();
        }


        public void OnDeviceChecked(object sender, RoutedEventArgs e)
        {

             }
       public static List<item> activeListClick = new List<item>();
        public void KppDevInListAction(object sender, RoutedEventArgs e)
        {
            stopQueue();
            //   worker.CancelAsync();
            Currwin.Background = Brushes.White;
            if (devS) disableKppDevSlide(new object(), new RoutedEventArgs());
            if (denyS) disableDenyList(new object(), new RoutedEventArgs());
            if (advS) disableAdBlock(new object(), new RoutedEventArgs());
            if (msgS) disableSRVMEssage(new object(), new RoutedEventArgs());    
           
            IEnumerable<item> Query =
    from it in activeListClick
    where it.ID == 1
    select it;
            if (Query.Count() > 0)
            {
               // 
                activeListClick.Remove(Query.ToList()[0]);
                if (activeListClick.Count == 0) stopQueueButtonClick(sender, e);
               
            }
            else if (kppDevListchBox.IsChecked == true)
            {
                activeListClick.Clear();
                activeListClick.Add(new item(1, "Технические средства"));
                if (activeListClick.Count > 0 && !IsEnable) 
                    StartQueu();
            }           
        }
        public void KppDenyInListAction(object sender, RoutedEventArgs e)
        {            
            IEnumerable<item> Query =
    from it in activeListClick
    where it.ID == 2
    select it;
            if (Query.Count() > 0)
            {
               
                activeListClick.Remove(Query.ToList()[0]);
                if (activeListClick.Count == 0) stopQueueButtonClick(sender, e);
            }
            else if (kppdenyListchBox.IsChecked == true)
            {
                activeListClick.Clear();
                activeListClick.Add(new item(2, "Запрещенные предметы"));
                if (activeListClick.Count > 0 && !IsEnable) 
                    StartQueu();
                
            }
           
        }
        //Выключение средств досмотра на кпп
        public void disableKppDevSlide(object sender, RoutedEventArgs e)
        {
            try
            {
                devS = false;
                Currwin.timerImageChange.IsEnabled = false;
                Currwin.slideCanvas.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception err)
            { 
            
            }
           // Currwin.timer.Stop();
        }


        //Включекние списа запрещенных предметов
        public void enableDenyList(object sender, RoutedEventArgs e)
        {
            denyS = true;
            Currwin.slideCanvas.Visibility = System.Windows.Visibility.Visible;
            Windows.SlideShow slide = new Windows.SlideShow(1);
            Currwin.IntervalTimer = 5;
            Currwin.strImagePath = "Images\\videos\\ico\\"+videoSource;
           // Currwin.ImageControls = new[] { Currwin.myImage, Currwin.myImage2 };
            String[] files = System.IO.Directory.GetFiles(Currwin.strImagePath);
            Currwin.VideoFrames = files;
            Currwin.myImage.Visibility = Visibility.Visible;          
            Currwin.timerImageChange = new DispatcherTimer();
            Currwin.timerImageChange.Interval = new TimeSpan(0, 0, 0, 0, 43);
            Currwin.timerImageChange.Tick += new EventHandler(Currwin.timerImageChange_Tick);
            if (files.Length > 0)  Currwin.timerImageChange.IsEnabled = true;
            
        }



        //Выключение списка запрещенных предметов
        public void disableDenyList(object sender, RoutedEventArgs e)
        {
            try
            {
                denyS = false;
          //  Currwin.timerImageChange.IsEnabled = false;
            Currwin.Aforgesrc.Stop();
            Currwin.frameSRC.Visibility = System.Windows.Visibility.Collapsed;
            //Currwin.slideCanvas.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception err)
            {

            }
        }




private void dispatcherTimer_Tick(object sender, EventArgs e)
{
  
}

        DispatcherTimer timer;
        public bool IsEnable = false;
        Uri first, second;
        bool wasFirst = false;
        Queue<Uri> playList = new Queue<Uri>();
        List<Uri> srs = new List<Uri>();
        /// <summary>
        ////Запуск воспроизведения составденного плейлиста (выполнение в другом потоке)
        /// </summary>
        /// <param name="list"></param>       
        public void startQueue(List<item> list)
        {
          

            while (IsEnable && list != null)
            {
                list = activeListClick;
                if (!IsEnable || list.Count == 0) break;
               
                for (int i = 0; i < list.Count; i++)
                {
                    if (!IsEnable) break;
                    if (list.Count==0)
                    {
                        break;
                    }
                    
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                       
                        if (devS) disableKppDevSlide(new object(), new RoutedEventArgs());
                        if (denyS) disableDenyList(new object(), new RoutedEventArgs());
                        if (advS) disableAdBlock(new object(), new RoutedEventArgs());
                        if (msgS) disableSRVMEssage(new object(), new RoutedEventArgs());
                        Currwin.Background = new SolidColorBrush(Color.FromScRgb(255, 255, 255, 255));
                        Currwin.mainGrid.Background = null;
                    }));

#region KPPDEVCASE
                    if (list[i].ID == 1)
                    {
                        bool m1 = true;

                        ObservableCollection<dev> ActiveList = new ObservableCollection<dev>();
                        foreach (var item in kppDevList)
                        {
                            if (item.showing)
                            ActiveList.Add(item);
                        }
                     
                        for (int vid = 0; vid < ActiveList.Count; vid++)
                        {
                            if (!(bool)ActiveList[vid].showing)
                            {
                                
                            }
                            else
                            {
                                /////new version

                                MediaSource = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\Images\\videos\\" + ActiveList[vid].path + ".mp4";
                                first = new Uri(MediaSource, UriKind.Relative);
                                if (vid + 1 < ActiveList.Count)
                                {
                                    MediaSourceNext = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\Images\\videos\\" + ActiveList[vid + 1].path + ".mp4";
                                    second = new Uri(MediaSourceNext, UriKind.Relative);
                                }
                                else MediaSourceNext = "-";

                                if (videoSource == MediaSource && MediaSourceNext != "-")
                                {
                                    videoSource = MediaSourceNext;

                                }
                                else
                                {
                                    videoSource = MediaSource;
                                }
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    Currwin.devQueue = true;
                                    
                                        //Currwin.media2.Pause();
                                        Currwin.media1.Source = first;
                                        Currwin.media1.Play();
                                      //  m1 = false;
                                      //  Currwin.media2.Source = second;
                                       // Currwin.media2.Play();
                                       // Currwin.media2.Pause();
                                    




                                    Currwin.VideoEnd = false;
                                    advS = true;

                                    // Currwin.Background = new SolidColorBrush(Color.FromScRgb(0, 0, 0, 0));

                                }));

                                Int64 tmpVal = -1;
                                Int64 currVal = 0;
                                Int64 maxVal = -1;
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                  //  Currwin.media2.Visibility = System.Windows.Visibility.Collapsed;
                                    Currwin.media1.Visibility = System.Windows.Visibility.Visible;
                                   
                                    
                                    // Currwin.mediaOpened = true;
                                }));


                                while (!Currwin.VideoEnd && IsEnable)
                                {

                                }

                                //////---new ver

                               // MediaSource = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\Images\\videos\\" + ActiveList[vid].path + ".mp4";
                               //videoSource = MediaSource;
                               // this.Dispatcher.Invoke(new Action(() =>
                               // {
                               //     enableAdBlock(new object(), new RoutedEventArgs());
                               // }));

                               // Int64 tmpVal = -1;
                               // Int64 currVal = 0;
                               // Int64 maxVal = -1;
                               // this.Dispatcher.Invoke(new Action(() =>
                               // {
                               //     currVal = (Int64)Currwin.media1.Position.TotalSeconds;
                               //     Currwin.mediaOpened = true;
                               // }));


                               // while (tmpVal <= currVal && advS)
                               // {
                               //     this.Dispatcher.Invoke(new Action(() =>
                               //     {
                               //         currVal = (int)Currwin.media1.Position.TotalSeconds;
                               //     }));
                               //     if ((tmpVal == currVal && tmpVal > 5) || !advS)
                               //     {
                               //         this.Dispatcher.Invoke(new Action(() =>
                               //         {
                               //             try
                               //             {
                               //                 maxVal = (int)Currwin.media1.NaturalDuration.TimeSpan.TotalSeconds;
                               //             }
                               //             catch (Exception err)
                               //             { }
                               //         }));
                               //         if (currVal == maxVal)
                               //         {
                               //             this.Dispatcher.Invoke(new Action(() =>
                               //             {
                               //                 Currwin.mediaOpened = true;
                               //             }));
                               //             break;
                               //         }
                               //     }
                               //     tmpVal = currVal;
                               // }


                               // this.Dispatcher.Invoke(new Action(() =>
                               // {
                               //     Currwin.media1.Stop();
                                  
                               // }));
                            }
                            
                        }
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            Currwin.media1.Stop();
                            Currwin.media2.Stop();
                            disableAdBlock(new object(), new RoutedEventArgs());
                            Currwin.devQueue = false;
                        }));
                    }
#endregion
#region denyCase
                    else if (list[i].ID == 2)
                    {
                        for (int ii = 0; ii < kppDenyList.Count; ii++)
                        {
                            MediaSource = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString() + "\\Images\\videos\\" + kppDenyList[ii].path + ".mp4";
                            srs.Add(new Uri(MediaSource, UriKind.Relative));
                            playList.Enqueue(new Uri(MediaSource, UriKind.Relative));
                        }
                        Currwin.denyList = true;
                          this.Dispatcher.Invoke(new Action(() =>
                                {                                                                  
                                    Currwin.frameSRC.Visibility = System.Windows.Visibility.Visible;
                                    denyS = true;
                                }));

                          bool m1 = true;

                          ObservableCollection<KPPDeny.deny> ActiveList = new ObservableCollection<KPPDeny.deny>();
                          foreach (var item in kppDenyList)
                          {
                              if (item.showing)
                                  ActiveList.Add(item);
                          }

                        for (int vid = 0; vid < ActiveList.Count; vid++)
                        {
                            Currwin.denyList = true;
                            
                            if (!ActiveList[vid].showing)
                            {

                            }
                            else
                            {
                                String path = System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName + "\\Images\\videos\\";
                               // first = new Uri(MediaSource, UriKind.Relative);                             
                                this.Dispatcher.Invoke(new Action(() =>
                                {                           ///d:\Prog\WPFApp\VIDEOhelper-3.0-files_LAST\FirstFloor.ModernUI.App\bin\Debug\Images\videos\gase.mp4
                                    Currwin.PlayAforgeVideo(path + ActiveList[vid].path + @".mp4");
                                    Currwin.VideoEnd = false;                                    
                                    // Currwin.PlayAforgeVideo(System.IO.Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName + @"\Images\videos\"+ ActiveList[vid].path + @".mp4");
                                }));
                                while (!Currwin.VideoEnd && IsEnable && denyS)
                                {
                                   
                                }
                            }
                        }
                        this.Dispatcher.Invoke(new Action(() =>
                                {
                                    denyS = false;
                                    Currwin.frameSRC.Visibility = System.Windows.Visibility.Collapsed;
                        disableAdBlock(new object(), new RoutedEventArgs());
                                }));
                    }
#endregion

                    else if (list[i].ID == 3)
                    {

                        advS = true;
                       
                        if (MediaSource != "-")
                        {
                            this.Dispatcher.Invoke(new Action(() =>
                    {
                        Currwin.media1.Visibility = System.Windows.Visibility.Visible;
                        Currwin.media1.Source = new Uri(MediaSource, UriKind.Relative);
                        Currwin.media1.Play();
                        Currwin.Background = new SolidColorBrush(Color.FromScRgb(0, 0, 0, 0));
                    }));
                        }

                        Int64 tmpVal = -1;
                        Int64 currVal = 0;
                        Int64 maxVal = -1;
                         this.Dispatcher.Invoke(new Action(() =>
                               {
                                   currVal = (Int64)Currwin.media1.Position.TotalSeconds;
                                   Currwin.media1.Visibility = System.Windows.Visibility.Visible;
                                   Currwin.mediaOpened = false;
                               }));
                         

                        while (tmpVal <= currVal && advS)
                        {                           
                                this.Dispatcher.Invoke(new Action(() =>
                               {
                                   currVal = (int)Currwin.media1.Position.TotalSeconds;
                               }));
                                if ((tmpVal == currVal && tmpVal > 5) || !advS)
                                {
                                    this.Dispatcher.Invoke(new Action(() =>
                                    {
                                        try
                                        {
                                      maxVal = (int)Currwin.media1.NaturalDuration.TimeSpan.TotalSeconds;
                                        }
                                        catch(Exception err)
                                        {}
                                    }));
                                    if (currVal == maxVal)
                                    {
                                        this.Dispatcher.Invoke(new Action(() =>
                                        {
                                            Currwin.mediaOpened = true;
                                        }));
                                        break;
                                    }
                                }
                                tmpVal = currVal;
                        }
                       

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                             Currwin.media1.Stop();
                             Currwin.media2.Stop();
                            disableAdBlock(new object(), new RoutedEventArgs());
                            
                        })); 
                    }

                    else if (list[i].ID == 4)
                    {
                        currSec = 0;
                        this.Dispatcher.Invoke(new Action(() =>
                        {                              
                            enableSrvMessage(new object(), new RoutedEventArgs());
                        }));

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                        timer = new DispatcherTimer();
                        timer.Interval = TimeSpan.FromSeconds(5);
                        timer.Tick +=timer_Tick;
                        timer.Start();
                        }));
                        while (currSec < 2 && msgS)
                        {
                            if (currSec >= 2 || !msgS) break;
                        }

                        this.Dispatcher.Invoke(new Action(() =>
                        {
                            disableSRVMEssage(new object(), new RoutedEventArgs());
                            timer.IsEnabled = false;
                        }));                               
                    }
                }               
            }
            this.Dispatcher.Invoke(new Action(() =>
            {
                Currwin.mainGrid.Background = new ImageBrush(new BitmapImage(new Uri(BaseUriHelper.GetBaseUri(this), "/Images/berg.jpg")));
            }));
        }

        void media1_MediaOpened(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                Currwin.media1.Position = new TimeSpan(0, 0, 0);
                 
                // Currwin.media1.BringIntoView();
               
                //Currwin.media1.Play();

                 Currwin.media1.Visibility = System.Windows.Visibility.Visible;
                // Currwin.media2.Visibility = System.Windows.Visibility.Hidden;
            }));
        }

        void media2_MediaOpened(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                // Currwin.media1.Play();
               // Currwin.media1.Visibility = System.Windows.Visibility.Hidden;
                // Currwin.mediaOpened = true;
            Currwin.media2.Pause();
            Currwin.media2.Position = new TimeSpan(0,0,0);
            Currwin.media2.Visibility = System.Windows.Visibility.Visible;
            }));
        }
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
            this.Dispatcher.Invoke(new Action(() =>
            {
                Currwin.media1.Stop();
                Currwin.media2.Stop();
                Currwin.media1.Visibility = System.Windows.Visibility.Collapsed;
                Currwin.media2.Visibility = System.Windows.Visibility.Collapsed;
            }));

            IsEnable = false;           
            //activeListClick.Clear();           
            dispatcherTimer.Stop();
        }

        private BackgroundWorker worker = new BackgroundWorker();
        public void startQueueButtonClick(object sender, RoutedEventArgs e)
        {
            
           activeListClick = newTabEmp.queueList;
            if (newTabEmp.PlayList != null)
            {
                activeListClick.Clear();
                foreach (var it in newTabEmp.PlayList.Items)
                {
                    item p = (item)it;
                    activeListClick.Add(p);
                }
            }
            StartQueu();
        }
       public void StartQueu()
        {
          //  stopQueueButtonClick(this.serviceMessagechBox, new RoutedEventArgs());
            // kppDevListchBox.IsChecked = false;
            // kppdenyListchBox.IsChecked = false;
            // advertisingBox.IsChecked = false;
            //serviceMessagechBox.IsChecked = false;          
           
            if (worker != null)
            {
                try
                {
                    worker.Dispose();
                }
                catch (Exception err)
                {

                }
            }

            IsEnable = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            try
            {
                worker.RunWorkerAsync();
            }
            catch (Exception err)
            {
            }
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            
            startQueue(activeListClick);
        }

        private void worker_RunWorkerCompleted(object sender,
                                       RunWorkerCompletedEventArgs e)
        {
            int a = 0;
            a++;
        }

        public void stopQueueButtonClick(object sender, RoutedEventArgs e)
        {
            stopQueue();

         //   worker.CancelAsync();
           // Currwin.Background = Brushes.White;
            if (devS) disableKppDevSlide(new object(), new RoutedEventArgs());
            if (denyS) disableDenyList(new object(), new RoutedEventArgs());
            if (advS) disableAdBlock(new object(), new RoutedEventArgs());
            if (msgS) disableSRVMEssage(new object(), new RoutedEventArgs());
            activeListClick.Clear();
        }

        private void DataGrid_CellEditEnding_1(object sender, DataGridCellEditEndingEventArgs e)
        {

        }



    }
}
