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
    /// 普通计算器.xaml 的交互逻辑
    /// </summary>
    public partial class 普通计算器 : Window
    {
        public 普通计算器()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            t.Clear();
            try
            {

                string[] str = ((TextBox)sender).Text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string eachStr in str)
                {
                    t.Text += Calculate.eval(eachStr);
                    t.Text += System.Environment.NewLine;
                }
                t.Text = t.Text.Replace("System.__ComObject", "");
            }
            catch  {}
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            input.Focus();
        }
    }
}
