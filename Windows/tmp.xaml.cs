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

using System.Windows.Threading;

using System.Windows.Media.Animation;

using System.Configuration;

using System.Collections;
using System.Collections.ObjectModel;
using System.IO;



using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Globalization;

using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using AForge.Video.FFMPEG;
using System.Drawing;

using AForge.Video;

namespace FirstFloor.ModernUI.App.Windows
{

   
    /// <summary>
    /// Interaction logic for tmp.xaml
    /// </summary>
    public partial class tmp : Window
    {
        public AForge.Video.FFMPEG.VideoFileSource Aforgesrc;
        public DispatcherTimer timer;

        DoubleAnimation anima;
        // Скорость анимации
        Double sp = 20;   // (20 px за 0.1 секунду)
        public tmp()
        {
            InitializeComponent();
            this.WindowStyle = System.Windows.WindowStyle.None;
            exe = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString();

            // Центрируем строку в канвасе
            Canvas.SetLeft(textBlock1, (Canvas.ActualWidth - textBlock1.ActualWidth) / 2);

            anima = new DoubleAnimation();
            anima.Duration = TimeSpan.FromSeconds(0.1);

            // При завершении анимации, запускаем функцию MyAnimation снова
            // (указано в обработчике)
            anima.Completed += myanim_Completed;
        }

        public void PlayAforgeVideo(string fileName)
        {
           Aforgesrc = new AForge.Video.FFMPEG.VideoFileSource(fileName);
           Aforgesrc.NewFrame += src_NewFrame;
           Aforgesrc.PlayingFinished += src_PlayingFinished;
           Aforgesrc.Start();
        }
        void src_PlayingFinished(object sender, ReasonToFinishPlaying reason)
        {
            VideoEnd = true;
        }


        void src_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = eventArgs.Frame;
            ImageSourceConverter c = new ImageSourceConverter();
            this.Dispatcher.Invoke(new Action(() =>
            {
                frameSRC.Source = BitmapToImageSource(bitmap);
            }));
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                return bitmapimage;
            }
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                this.Close();
            }
            if (e.Key == Key.F4)
            {
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            }
            if (e.Key == Key.F3)
            {
                this.WindowStyle = System.Windows.WindowStyle.None;
            }
        }

        public bool VideoEnd = false;
        public int currVideoIndex = 0, previndex = 0;
        public bool devQueue = false, denyQueue = false;

        public void media1_MediaEnded(object sender, RoutedEventArgs e)
        {
                    
          
            if (!mediaOpened && VideoEnd)
            {
            media1.Position = new TimeSpan(0, 0, 0); 
            media1.Play();
            }
            if (devQueue || denyList)
            {
               // media2.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                media1.Position = new TimeSpan(0, 0, 0);
                media1.Play();
                media2.Visibility = System.Windows.Visibility.Visible;
                media2.Play();
                media1.Visibility = System.Windows.Visibility.Hidden;
                media1.Pause();
            }
            VideoEnd = true;
        }

        private void media2_MediaEnded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!denyList)
            {
                media2.Position = new TimeSpan(0, 0, 0);
                media2.Play();
                media1.Play();
                media2.Pause();
                media1.Visibility = System.Windows.Visibility.Visible;

                media2.Visibility = System.Windows.Visibility.Hidden;

                VideoEnd = true;
            }
        }

        public String[] VideoFrames;
      public int FrameIndex = 0;
      ImageFast fastImg;
      public BitmapImage[] FrameArray;
      public bool denyList = false;
      string exe = "";
      BitmapImage imgCurr;
        [Cudafy]
        public void timerImageChange_Tick(object sender, EventArgs e)
        {
            // PlaySlideShow();
            if (FrameIndex == VideoFrames.Length -1 && denyList)
            {
                //FrameIndex = 0;
                //timerImageChange.IsEnabled = false;
                // FrameIndex = 0;
                denyList = false;
            }
            
                        
                
               // String path = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/" + VideoFrames[FrameIndex];
               
            
            
               /*
                DrawingVisual drawingVisual = new DrawingVisual();
                DrawingContext drawingContext = drawingVisual.RenderOpen();
                drawingContext.DrawImage(new BitmapImage(uri), new System.Windows.Rect(new Size(500,500)));
                drawingContext.Close();

                RenderTargetBitmap bmp = new RenderTargetBitmap(180, 180, 120, 96, PixelFormats.Pbgra32);
                bmp.Render(drawingVisual);
                myImage.Source = bmp;

            */
            LoadImage();            
            //myImage.Source = new ImageSourceConverter().ConvertFromString(VideoFrames[FrameIndex]) as ImageSource;
                FrameIndex++;          

        }
       
    
        [Cudafy]
        public void LoadImage()
        {
            var uri = new Uri(exe + "/" + VideoFrames[FrameIndex]);
            imgCurr = new BitmapImage(uri);
            myImage.Source = imgCurr;
        }

        public DispatcherTimer timerImageChange;
        public System.Windows.Controls.Image[] ImageControls;
        public List<ImageSource> Images = new List<ImageSource>();
        public static string[] ValidImageExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
        public static string[] TransitionEffects = new[] { "Fade" };
        public string TransitionType, strImagePath = "";
        public int CurrentSourceIndex, CurrentCtrlIndex, EffectIndex = 0, IntervalTimer = 1;
        public void PlaySlideShow()
        {
            //try
            //{
            //    if (Images.Count == 0)
            //        return;
            //    var oldCtrlIndex = CurrentCtrlIndex;
            //    CurrentCtrlIndex = (CurrentCtrlIndex + 1) % 2;
            //    CurrentSourceIndex = (CurrentSourceIndex + 1) % Images.Count;


            //   System.Drawing.Image imgFadeOut = ImageControls[oldCtrlIndex];
            //   System.Drawing.Image imgFadeIn = ImageControls[CurrentCtrlIndex];
            //    ImageSource newSource = Images[CurrentSourceIndex];
               
            //    imgFadeIn.Source = newSource;

            // //   myImage.Stretch = Stretch.None;
            //  //  myImage2.Stretch = Stretch.None;

            //   //// myImage2.Width = this.Width;
            //  //// myImage.Width = this.Width;
              
            //    TransitionType = TransitionEffects[EffectIndex].ToString();
            //    Storyboard StboardFadeOut = (Resources[string.Format("{0}Out", TransitionType.ToString())] as Storyboard).Clone();
            //    StboardFadeOut.Begin(imgFadeOut);
            //    Storyboard StboardFadeIn = Resources[string.Format("{0}In", TransitionType.ToString())] as Storyboard;
            //    StboardFadeIn.Begin(imgFadeIn);
            //}
            //catch (Exception ex) { }
        }


        public bool mediaOpened = false;
        public List<Uri> playList = new List<Uri>();
        public List<MediaElement> mediaList = new List<System.Windows.Controls.MediaElement>();
        //int mediaIndex = 0;
       
      

        private void media1_MediaOpened(object sender, RoutedEventArgs e)
        {
            //media2.Visibility = System.Windows.Visibility.Visible;
            //media2.Source = media1.Source;
            //media2.Play();
            //media1.Position = new TimeSpan(0, 0, 0);
          //  media1.Play();
          //  mediaOpened = true;
        }
        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                timerImageChange.Stop();
                FirstFloor.ModernUI.App.Content.panel1.instance.IsEnable = false;
                FirstFloor.ModernUI.App.Content.panel1.instance.stopQueue();
            }
            catch (Exception err)
            { 
            
            }
        }

        private void Window_Loaded_1(object sender, System.Windows.RoutedEventArgs e)
        {
            Canvas.SetLeft(textBlock1, (Canvas.ActualWidth - textBlock1.ActualWidth) / 2);

            animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromSeconds(0.1);

            // При завершении анимации, запускаем функцию MyAnimation снова
            // (указано в обработчике)
            animation.Completed += myanim_Completed;

            MyAnimation(Canvas.GetLeft(textBlock1), Canvas.GetLeft(textBlock1) - speed);
        }
        DoubleAnimation animation;
        // Скорость анимации
        Double speed = 15;   // (20 px за 0.1 секунду)
        private void MyAnimation(double from, double to)
        {
            // Если строка вышла за пределы канваса (отриц. Canvas.Left)
            // то возвращаем с другой стороны
            if (Canvas.GetLeft(textBlock1) + textBlock1.ActualWidth <= 0)
            {
                animation.From = Canvas.ActualWidth;
                animation.To = Canvas.ActualWidth - speed;
                textBlock1.BeginAnimation(Canvas.LeftProperty, animation);
            }
            else
            {
                animation.From = from;
                animation.To = to;
                textBlock1.BeginAnimation(Canvas.LeftProperty, animation);
            }

        }

        private void myanim_Completed(object sender, EventArgs e)
        {
            MyAnimation(Canvas.GetLeft(textBlock1), Canvas.GetLeft(textBlock1) - speed);
        }

        
    }
}
