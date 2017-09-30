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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstFloor.ModernUI.App
{
    /// <summary>
    /// Interaction logic for win_options_window.xaml
    /// </summary>
    public partial class win_options_window : ModernDialog
    {
        public win_options_window()
        {
            
            InitializeComponent();
            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton};



       
        }

        private void pic1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            temp1.IsChecked = !temp1.IsChecked;
        }

        private void pic2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            temp2.IsChecked = !temp2.IsChecked;
        }

        private void pic3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            temp3.IsChecked = !temp3.IsChecked;
        }
    }
}
