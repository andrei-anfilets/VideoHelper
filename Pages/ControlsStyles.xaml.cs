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

namespace FirstFloor.ModernUI.App.Pages
{
    /// <summary>
    /// Interaction logic for ControlsStyles.xaml
    /// </summary>
    public partial class ControlsStyles : UserControl
    {       
        //public ControlsStyles Instance { get;}
        public ControlsStyles()
        {
            InitializeComponent();   
            System.Windows.Forms.Screen[] scr = System.Windows.Forms.Screen.AllScreens;

            for (int i = 0; i < scr.Length; i++)
            {
                if (scr[i].Primary && scr.Length > 1)
                {

                }
                else if ((scr.Length == 1 || !scr[i].Primary) && MainWindow.panels.Count==0)
                {
                    FirstFloor.ModernUI.App.Windows.tmp t1 = new Windows.tmp();
                    objects.panel pan = new objects.panel(MainWindow.panels.Count + 1, 1, t1);
                    FirstFloor.ModernUI.App.MainWindow.panels.Add(pan);
                    System.Drawing.Point p = new System.Drawing.Point(scr[i].Bounds.Location.X, scr[i].Bounds.Location.Y);
                    t1.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                    t1.Left = p.X;
                    t1.Top = p.Y;
                    t1.Show();
                    ////Раскоментировать если нужно открывать панель не на весь экран
                    //if (scr.Length > 1)
                    //{
                      t1.WindowState = System.Windows.WindowState.Maximized;
                    //}
                    FirstFloor.ModernUI.App.Content.panel1.Currwin = (Windows.tmp)MainWindow.panels[0].win;

                }
            }
            //Instance = this;   
        }
    }
}
