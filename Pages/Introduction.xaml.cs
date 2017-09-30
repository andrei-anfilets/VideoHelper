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


using FluidKit.Controls;
using FluidKit.Samples;
using FluidKit.Helpers.Animation;

namespace FirstFloor.ModernUI.App.Pages
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : UserControl
    {

        private FluidKit.Samples.StringCollection _dataSource;

        private LayoutBase[] _layouts = {
		                                	new Wall(),
		                                	new SlideDeck(),
		                                	new CoverFlow(),
		                                	new Carousel(),
		                                	new TimeMachine2(),
		                                	new ThreeLane(),
		                                	new VForm(),
		                                	new TimeMachine(),
		                                	new RollerCoaster(),
		                                	new Rolodex(),
		                                };
        public Introduction()
        {
            InitializeComponent();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;

            if (scr.Length == 1)
            {
                monitors.BBCode = "Обнаружен [b]1[/b] монитор";
            }
            monitors.BBCode = "Обнаружено мониторов - [b]" + scr.Length.ToString() + "[/b]";

            NavigationService ns;
           // progressBar1.IsActive = false;

            dispatcherTimer.Stop();
            
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

            

           
        }
    }
}
