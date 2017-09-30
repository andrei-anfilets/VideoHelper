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

using System.Windows.Forms;

namespace FirstFloor.ModernUI.App.Windows
{
    /// <summary>
    /// Interaction logic for addKppDevice.xaml
    /// </summary>
    public partial class addKppDevice : ModernDialog
    {
        public addKppDevice()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new System.Windows.Controls.Button[] { this.OkButton, this.CancelButton };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            image1.Source = getImageSource(1);
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
                    System.IO.File.Copy(ofd.FileName, folderpath + ofd.SafeFileName, true);
                    packUri = folderpath + ofd.SafeFileName;
                }
            }
            return new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
        }
    }
}
