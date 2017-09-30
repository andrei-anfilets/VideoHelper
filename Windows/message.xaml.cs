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
    /// Логика взаимодействия для message.xaml
    /// </summary>
    public partial class message : Window
    {
        public message()
        {
            InitializeComponent();

             this.WindowStyle = System.Windows.WindowStyle.None;  
        }
        public bool IsEditMode = false;
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
            }

            else if (e.Key == Key.Add)
            {
                srvMessage.FontSize += 5;
            }
            else if (e.Key == Key.Subtract)
            {
                if (srvMessage.FontSize > 10)
                {
                    srvMessage.FontSize -= 5;
                }
            }

        }

        private void MenuItem_Click_21(object sender, RoutedEventArgs e)
        {
            srvMessage.FontSize+=5;
        }

        private void MenuItem_Click_22(object sender, RoutedEventArgs e)
        {
            srvMessage.FontSize -= 5;
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
                this.Background = new SolidColorBrush(colorpicker1.Color);
            }
            else
            {
                srvMessage.Foreground = new SolidColorBrush(colorpicker1.Color);
            }

            backgr = false;
            font = false;
            colorpicker1.Visibility = System.Windows.Visibility.Hidden;
        }


    }
}
