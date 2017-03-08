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
    /// 提示框.xaml 的交互逻辑
    /// </summary>
    public partial class 提示框 : Window
    {
        public 提示框(string hint)
        {
            InitializeComponent();
            tb.Text = hint;
        }

        private void close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
