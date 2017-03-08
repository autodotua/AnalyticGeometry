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

namespace 解析几何
{
    /// <summary>
    /// 启动器.xaml 的交互逻辑
    /// </summary>
    public partial class 启动器 : Window
    {
        public 启动器()
        {
            InitializeComponent();
        }
        List<Window> w = new List<Window>();
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
          
        }

        private void minimize(object sender, RoutedEventArgs e)
        {

        } 
        private void close(object sender, RoutedEventArgs e)
        {
            if (isCloseAll.IsChecked == true)
            {
                //foreach (var i in w)
                //{
                //    try
                //    {
                //        i.Close();
                //    }
                //    catch { }
                //}

                Application.Current.Shutdown();
            }
            else
            {
                this.Close();
            }
           
        }
 private void open(object sender, RoutedEventArgs e)
        {
            
            switch (((Button)sender).Content.ToString())
            {
                case "解析几何图像":
                    w.Add(new 解析几何图像());
                    break;
                case "普通计算器":
                    w.Add(new 普通计算器());
                    break;
                case "解方程":
                    w.Add(new 解方程());
                    break;
                default:
                    w.Add(null);
                    break;
            }
            if (w[w.Count-1]!= null)
            {
                w[w.Count - 1].Show();
            }
            if (isCloseLauncher.IsChecked == true)
            {
                this.Close();
            }
        }

 private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
 {
     this.DragMove();
 } 
    }
}
