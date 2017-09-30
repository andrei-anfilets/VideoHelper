using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using FirstFloor.ModernUI.App.Content;

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
   public class panelState
    {


        public bool QUEQENABLED { get; set; }

        private List<item> _queue = new List<item>();
        public List<item> QUEUEitems { get {
                return _queue;
            } set {
                _queue = value;
            } }


        public String IP_ADDRESS { get; set; }
        public ObservableCollection<dev> kppDevList;
        public bool ppDevListActive { get; set; }
        public ObservableCollection<KPPDeny.deny> kppDenyList;
        public bool kppDenyListActive { get; set; }
        public ObservableCollection<KPPMessages.msg> kppMsgList;
        public bool kppMsgListActive { get; set; }
        public String UserMessageBoxValue { get; set; }

        public bool IsAdvertisetACTIVE { get; set; }

        public bool RunningStringActive { get; set; }
        public String RunninngStringVALUE { get; set; }

        public bool DateTimeActive { get; set; }




        //public int MyProperty
        //{
        //    get { return (int)GetValue(MyPropertyProperty); }
        //    set { SetValue(MyPropertyProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MyPropertyProperty =
        //    DependencyProperty.Register("MyProperty", typeof(int), typeof(ownerclass), new PropertyMetadata(0));



    }
}
