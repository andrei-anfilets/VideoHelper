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


using System.Collections;
using System.Collections.ObjectModel;

using FirstFloor.ModernUI.App.Content;

namespace FirstFloor.ModernUI.App.Windows
{
    /// <summary>
    /// Interaction logic for messageSelector.xaml
    /// </summary>
    public partial class messageSelector : ModernDialog
    {
        public messageSelector()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };


            ObservableCollection<FirstFloor.ModernUI.App.Content.KPPMessages.msg> custdata = new FirstFloor.ModernUI.App.Content.KPPMessages().GetData();
            //Bind the DataGrid to the KppDevices data
            DG1.DataContext = custdata;
        }
    }
}
