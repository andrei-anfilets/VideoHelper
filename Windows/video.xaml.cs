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

namespace FirstFloor.ModernUI.App.Windows
{
    /// <summary>
    /// Логика взаимодействия для video.xaml
    /// </summary>
    public partial class video : Window
    {
        public video()
        {
            InitializeComponent();
             
            string res = VideoSource();
            if (res != "-")
            {
                media1.Source = new Uri(res, UriKind.Relative);
                media1.Play();
            }
            this.WindowStyle = System.Windows.WindowStyle.None; 
        
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            banner1.Width += 50;
        }

        private void MenuItem_Click_31(object sender, RoutedEventArgs e)
        {
            banner1.Width -= 50;
        }

        private void MenuItem_Click_33(object sender, RoutedEventArgs e)
        {
            string res = VideoSource();
            if (res != "-")
            {
                media1.Source = new Uri(res, UriKind.Relative);
                media1.Play();
            }
        }

        public String VideoSource()
        {
            string rez = "-";
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "VIDEO Files|*.wmv;*.avi;*.mp4;*.mpeg;*.mkv;";
            ofd.Title = "Выбор видео ...";
            string folderpath = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\videos\\";
            string packUri = System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName; //+ "\\Images\\empty.png";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.File.Copy(ofd.FileName, folderpath + ofd.SafeFileName, true);
                packUri = folderpath + ofd.SafeFileName;
                return packUri;
            }
            else return "-";
        }

        // 0 - empty image 
        // 1 - openfile dialog
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
                    System.IO.File.Copy(ofd.FileName, folderpath + ofd.SafeFileName, true);
                    packUri = folderpath + ofd.SafeFileName;
                }
            }
            return new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
        }

        private void media1_MediaEnded(object sender, RoutedEventArgs e)
        {
            media1.Position = new TimeSpan(0, 0, 0);
            media1.Play();
        }

        private void MenuItem_Click_35(object sender, RoutedEventArgs e)
        {
            banner1.Source = null;
            banner1.BeginInit();
            banner1.Source = getImageSource(1);
            banner1.EndInit();
        }

        private void MenuItem_Click_38(object sender, RoutedEventArgs e)
        {
            banner1.Visibility = System.Windows.Visibility.Collapsed;
        }

        public bool IsEditMode = true;
        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            //Режимы отображения
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
            }
        }
        private void MenuItem_Click_381(object sender, RoutedEventArgs e)
        {
            if (banner1.HorizontalAlignment == System.Windows.HorizontalAlignment.Right)
                banner1.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            else banner1.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
        }

        private void MenuItem_Click_382(object sender, RoutedEventArgs e)
        {
            if (banner1.VerticalAlignment == System.Windows.VerticalAlignment.Top)
                banner1.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            else banner1.VerticalAlignment = System.Windows.VerticalAlignment.Top;
        }

        private void increaseTextBlock(object sender, RoutedEventArgs e)
        {
            textBlock1.FontSize += 5;
            Canvas.Height += 5;
        }

        private void decreaseTextBlock(object sender, RoutedEventArgs e)
        {
            if (textBlock1.FontSize > 10)
            {
                textBlock1.FontSize -= 5;
                Canvas.Height -= 5;
            }
        }

        private void setTextBlockText(object sender, RoutedEventArgs e)
        {
            Windows.textInput txt = new textInput();
            if (txt.ShowDialog() == true)
            {
                textBlock1.Text = new TextRange(txt.RichText1.Document.ContentStart, txt.RichText1.Document.ContentEnd).Text;
            }
        }

        private void hideTextBlock(object sender, RoutedEventArgs e)
        {
            Canvas.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
