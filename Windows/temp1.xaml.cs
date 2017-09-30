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

using NPOI;

namespace FirstFloor.ModernUI.App.Windows
{
    /// <summary>
    /// Логика взаимодействия для temp1.xaml
    /// </summary>
    public partial class temp1 : Window
    {


        public bool IsEditMode = true;
        public List<DevExpress.Xpf.Docking.LayoutPanel> panels = new List<DevExpress.Xpf.Docking.LayoutPanel>();

        public temp1()
        {        

            InitializeComponent();
            //panel1.Background = new SolidColorBrush(Colors.Transparent);
            panels.Add(panel1);
            panels.Add(panel2);
            panels.Add(panel3);
            panels.Add(panel4);
            panels.Add(panel5);
            panels.Add(panel6);
            panels.Add(panel7);
            panels.Add(panel8);

            parameters prm = new parameters();
            try
            {
                prm = new parameters(System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\data.xls");
            }
            catch (Exception err)
            {
                MessageBox.Show("Не найден файл параметров приложения. Восстановите недостающие файлы или переустановите программу.", "Ошибка открытия файла с параметрами!", MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
            }
                //1
            try
            {
                image1_panel1.Source = new ImageSourceConverter().ConvertFromString(prm.LoadPanelSrc("panel1")) as ImageSource;
            }
            catch {
                image1_panel1.Source = getImageSource(0);
            }
            //2
            try
            {
                image1_panel2.Source = new ImageSourceConverter().ConvertFromString(prm.LoadPanelSrc("panel2")) as ImageSource;
            }
            catch
            {
                image1_panel2.Source = getImageSource(0);
            }
            //3
            try
            {
                image1_panel3.Source = new ImageSourceConverter().ConvertFromString(prm.LoadPanelSrc("panel3")) as ImageSource;
            }
            catch
            {
                image1_panel3.Source = getImageSource(0);
            }
            //4
            try
            {
                image1_panel4.Source = new ImageSourceConverter().ConvertFromString(prm.LoadPanelSrc("panel4")) as ImageSource;
            }
            catch
            {
                image1_panel4.Source = getImageSource(0);
            }
            //5 media element
            try
            {                
                media1.Source = new Uri(prm.LoadPanelSrc("panel5"));
                media1.Play();
            }
            catch (Exception err)
            { 
            
            }

            //6
            try
            {
                image1_panel6.Source = new ImageSourceConverter().ConvertFromString(prm.LoadPanelSrc("panel6")) as ImageSource;
            }
            catch
            {
                image1_panel6.Source = getImageSource(0);
            }
            //7
            try
            {
                image1_panel7.Source = new ImageSourceConverter().ConvertFromString(prm.LoadPanelSrc("panel7")) as ImageSource;
            }
            catch
            {
                image1_panel7.Source = getImageSource(0);
            }
            //8
            try
            {
                image1_panel8.Source = new ImageSourceConverter().ConvertFromString(prm.LoadPanelSrc("panel8")) as ImageSource;
            }
            catch
            {
                image1_panel8.Source = getImageSource(0);
            }

           
          
           

            //System.IO.Stream str;
            //// Create and save the layout of dock panels to a new memory stream 
            //str = new System.IO.FileStream("manager.txt", System.IO.FileMode.Open);
            //str.Seek(0, System.IO.SeekOrigin.Begin);
            //manager.RestoreLayoutFromStream(str);

            if (System.IO.File.Exists(MainWindow.specificFolder + "\\mgr_templ_1.xml"))
            {
                manager.RestoreLayoutFromXml(MainWindow.specificFolder + "\\mgr_templ_1.xml");
            }

        }

        public void SaveSettings()
        { 
        



        }

        private void Grid_KeyDown_1(object sender, KeyEventArgs e)
        {
        

        }

        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            // progressBar1.IsActive = true;

            //Режимы отображения
            if (e.Key == Key.F2)
            {
                if (IsEditMode)
                {
                    for (int i = 0; i < panels.Count; i++)
                    {
                        panels[i].ShowCaption = false;
                        panels[i].ShowCaptionImage = false;
                        this.WindowStyle = System.Windows.WindowStyle.None;                        
                    }
                    IsEditMode = false;
                }
                else if (!IsEditMode)
                {
                    for (int i = 0; i < panels.Count; i++)
                    {
                        panels[i].ShowCaption = true;
                        panels[i].ShowCaptionImage = true;
                        
                        this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;                        
                    }
                    IsEditMode = true;
                }

                // ShowCaption="False" ShowCaptionImage="False"             

            }

            else if (e.Key == Key.F4)
            {
                panels.Clear();
             panels.Add(panel1);
            panels.Add(panel2);
            panels.Add(panel3);
            panels.Add(panel4);
            panels.Add(panel5);
            panels.Add(panel6);
            panels.Add(panel7);
            panels.Add(panel8);


            manager.SaveLayoutToXml(MainWindow.specificFolder + "\\mgr_templ_1.xml");
            //System.IO.Stream str;
            //// Create and save the layout of dock panels to a new memory stream 
            //str = new System.IO.MemoryStream();
            //manager.SaveLayoutToStream(str);
            //str.Seek(0, System.IO.SeekOrigin.Begin);
            //System.IO.FileStream fs = new System.IO.FileStream("manager.txt", System.IO.FileMode.OpenOrCreate);
            //str.CopyTo(fs);
            //// Load the layout of dock panels from the memory stream 
           


                parameters prm = new parameters (System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\data.xls");
              
                prm.SavePanelParameters(panels[0].Name, image1_panel1.Source.ToString());
                prm.SavePanelParameters(panels[1].Name, image1_panel2.Source.ToString());
                prm.SavePanelParameters(panels[2].Name, image1_panel3.Source.ToString());
                prm.SavePanelParameters(panels[3].Name, image1_panel4.Source.ToString());
                prm.SavePanelParameters(panels[4].Name, media1.Source.ToString());
                prm.SavePanelParameters(panels[5].Name, image1_panel6.Source.ToString());
                prm.SavePanelParameters(panels[6].Name, image1_panel7.Source.ToString());
                prm.SavePanelParameters(panels[7].Name, image1_panel8.Source.ToString());
              
              
                prm.save();
            }
        }

        //клик  по контекстному меню - картинка
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (manager.ActiveDockItem.Caption.ToString() == "Панель1")
            {
                
                image1_panel1.Source = null;
                image1_panel1.BeginInit();
                image1_panel1.Source = getImageSource(1);
                image1_panel1.EndInit();
            }
            else if (manager.ActiveDockItem.Caption.ToString() == "Панель2")
            {
                image1_panel2.Source = null;
                image1_panel2.BeginInit();
                image1_panel2.Source = getImageSource(1);
                image1_panel2.EndInit();
            }
            else if (manager.ActiveDockItem.Caption.ToString() == "Панель3")
            {
                image1_panel3.Source = null;
                image1_panel3.BeginInit();
                image1_panel3.Source = getImageSource(1);
                image1_panel3.EndInit();
            }
            else if (manager.ActiveDockItem.Caption.ToString() == "Панель4")
            {
                image1_panel4.Source = null;
                image1_panel4.BeginInit();
                image1_panel4.Source = getImageSource(1);
                image1_panel4.EndInit();
            }
            else if (manager.ActiveDockItem.Caption.ToString() == "Панель6")
            {
                image1_panel6.Source = null;
                image1_panel6.BeginInit();
                image1_panel6.Source = getImageSource(1);
                image1_panel6.EndInit();
            }
            else if (manager.ActiveDockItem.Caption.ToString() == "Панель7")
            {
                image1_panel7.Source = null;
                image1_panel7.BeginInit();
                image1_panel7.Source = getImageSource(1);
                image1_panel7.EndInit();
            }
            else if (manager.ActiveDockItem.Caption.ToString() == "Панель8")
            {
                image1_panel8.Source = null;
                image1_panel8.BeginInit();
                image1_panel8.Source = getImageSource(1);
                image1_panel8.EndInit();
            }
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
            string packUri =System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\Images\\empty.png";
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

        /// <summary>
        ////Получение сохраненного источника воспроизведения видео для панели, содержащей медиа элемент
        /// </summary>
        /// <returns></returns>
        /// 
        public String GetVideoSource()
        { 
        String src = "-";





        return src;
        
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

        //ВИДЕО
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
               if (manager.ActiveDockItem.Caption.ToString() == "Панель5")
            {
                string res = VideoSource();
                if (res != "-")
                {
                    media1.Source = new Uri(res, UriKind.Relative);
                    media1.Play();
                }
            }
        }


        //ЗВУК
        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            media1.IsMuted = !media1.IsMuted;
        }

        private void media1_MediaEnded(object sender, RoutedEventArgs e)
        {
            media1.Position = new TimeSpan(0, 0, 0);
            media1.Play();
        }    
    }
}
