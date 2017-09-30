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


using System.Windows.Media.Animation;

using System.Configuration;

using System.Windows.Threading;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;

using System.Xml;
using System.Xml.Serialization;



namespace FirstFloor.ModernUI.App.Windows
{
    /// <summary>
    /// Логика взаимодействия для SlideShow.xaml
    /// </summary>
    public partial class SlideShow : Window
    {

        private DispatcherTimer timerImageChange;
        private Image[] ImageControls;
        public List<ImageSource> Images = new List<ImageSource>();
        private static string[] ValidImageExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
        private static string[] TransitionEffects = new[] { "Fade" };
        private string TransitionType, strImagePath = "";
        private int CurrentSourceIndex, CurrentCtrlIndex, EffectIndex = 0, IntervalTimer = 1;
 /// <summary>
 ////если 1 занчит открытие окна средств досмотра, 2 - запрещенные к проносу предметы.
 /// </summary>
 /// <param name="task"></param>
        public SlideShow(int task)
        {
            InitializeComponent();
            //Initialize Image control, Image directory path and Image timer.
            IntervalTimer = 5;
            strImagePath = "Images";
            ImageControls = new[] { myImage, myImage2 };
          //  System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromControl(new System.Windows.Forms.Control());

            //loadKppDevices();
            loadKppDeny();
           // LoadImageFolder(strImagePath);

            timerImageChange = new DispatcherTimer();
            timerImageChange.Interval = new TimeSpan(0, 0, IntervalTimer);
            timerImageChange.Tick += new EventHandler(timerImageChange_Tick);

            this.WindowStyle = System.Windows.WindowStyle.None;  
        }
        public SlideShow()
        {
            InitializeComponent();
            //Initialize Image control, Image directory path and Image timer.
            IntervalTimer = 5;
            strImagePath = "Images";
            ImageControls = new[] { myImage, myImage2 };

            loadKppDevices();
            // LoadImageFolder(strImagePath);

            timerImageChange = new DispatcherTimer();
            timerImageChange.Interval = new TimeSpan(0, 0, IntervalTimer);
            timerImageChange.Tick += new EventHandler(timerImageChange_Tick);

            this.WindowStyle = System.Windows.WindowStyle.None;
        }


        public ObservableCollection<FirstFloor.ModernUI.App.Content.dev> kppdevices;
        public ObservableCollection<FirstFloor.ModernUI.App.Content.KPPDeny.deny> kppDeny;
      

        public FirstFloor.ModernUI.App.Content.kppDevices dev;



        public void loadKppDevices()
        {
            Images.Clear();
            
            kppdevices =  new FirstFloor.ModernUI.App.Content.LoremIpsum().GetData();

            for (int i = 0; i < kppdevices.Count; i++)
            {

                
              
                if (kppdevices[i].showing == true && kppdevices[i].path.StartsWith("file")  && File.Exists(new Uri(kppdevices[i].path).LocalPath))
                {
                    Images.Add(new ImageSourceConverter().ConvertFromString(kppdevices[i].path) as ImageSource);                
                }
               else  if (kppdevices[i].showing == true && File.Exists(System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\images\\" + kppdevices[i].path))
                {
                    Images.Add(new ImageSourceConverter().ConvertFromString(System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\images\\" + kppdevices[i].path) as ImageSource);
                }

            }
            
        
        }

        public void loadKppDeny()
        {
            Images.Clear();
            kppDeny = new FirstFloor.ModernUI.App.Content.KPPDeny().GetData();
            for (int i = 0; i < kppDeny.Count; i++)
            {
                if (kppDeny[i].showing == true && kppDeny[i].path.StartsWith("file") && File.Exists(new Uri(kppDeny[i].path).LocalPath))
                {
                    Images.Add(new ImageSourceConverter().ConvertFromString(kppDeny[i].path) as ImageSource);
                }
                else if (kppDeny[i].showing == true && File.Exists(System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\images\\" + kppDeny[i].path))
                {
                    Images.Add(new ImageSourceConverter().ConvertFromString(System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\images\\" + kppDeny[i].path) as ImageSource);
                }
            }
        }

        public void LoadImageFolder(string folder)
        {
            ErrorText.Visibility = Visibility.Collapsed;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            if (!System.IO.Path.IsPathRooted(folder))
                folder = System.IO.Path.Combine(Environment.CurrentDirectory, folder);
            if (!System.IO.Directory.Exists(folder))
            {
                ErrorText.Text = "The specified folder does not exist: " + Environment.NewLine + folder;
                ErrorText.Visibility = Visibility.Visible;
                return;
            }
            Random r = new Random();
            var sources = from file in new System.IO.DirectoryInfo(folder).GetFiles().AsParallel()
                          where ValidImageExtensions.Contains(file.Extension, StringComparer.InvariantCultureIgnoreCase)
                          orderby r.Next()
                          select CreateImageSource(file.FullName, true);
            Images.Clear();
            Images.AddRange(sources);
            sw.Stop();
            Console.WriteLine("Total time to load {0} images: {1}ms", Images.Count, sw.ElapsedMilliseconds);
        }

        public ImageSource CreateImageSource(string file, bool forcePreLoad)
        {
            if (forcePreLoad)
            {
                var src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(file, UriKind.Absolute);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                src.Freeze();
                return src;
            }
            else
            {
                var src = new BitmapImage(new Uri(file, UriKind.Absolute));
                src.Freeze();
                return src;
            }
        }

        private void timerImageChange_Tick(object sender, EventArgs e)
        {
            PlaySlideShow();
        }

        private void PlaySlideShow()
        {
            try
            {
                if (Images.Count == 0)
                    return;
                var oldCtrlIndex = CurrentCtrlIndex;
                CurrentCtrlIndex = (CurrentCtrlIndex + 1) % 2;
                CurrentSourceIndex = (CurrentSourceIndex + 1) % Images.Count;

                Image imgFadeOut = ImageControls[oldCtrlIndex];
                Image imgFadeIn = ImageControls[CurrentCtrlIndex];
                ImageSource newSource = Images[CurrentSourceIndex];
                imgFadeIn.Source = newSource;

                TransitionType = TransitionEffects[EffectIndex].ToString();
               
                Storyboard StboardFadeOut = (Resources[string.Format("{0}Out", TransitionType.ToString())] as Storyboard).Clone();
                StboardFadeOut.Begin(imgFadeOut);
                Storyboard StboardFadeIn = Resources[string.Format("{0}In", TransitionType.ToString())] as Storyboard;
                StboardFadeIn.Begin(imgFadeIn);
            }
            catch (Exception ex) { }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            PlaySlideShow();
            timerImageChange.IsEnabled = true;
        }
        public bool IsEditMode = true;
        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                if (IsEditMode)
                {                   
                        this.WindowStyle = System.Windows.WindowStyle.None;                   
                    IsEditMode = false;
                }
                else if (!IsEditMode)
                {                  

                    this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;                   
                    IsEditMode = true;
                }

                // ShowCaption="False" ShowCaptionImage="False"             

            }
        }
    }
}
