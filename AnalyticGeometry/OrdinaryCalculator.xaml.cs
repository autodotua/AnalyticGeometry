using System;
using System.Collections.Generic;
using System.IO;
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

namespace AnalyticGeometry
{
    /// <summary>
    /// OrdinaryCalculator.xaml 的交互逻辑
    /// </summary>
    public partial class OrdinaryCalculator : Window
    {
        public OrdinaryCalculator()
        {
            InitializeComponent();
        }
        private void InputAreaTextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            txtOutput.Clear();
            try
            {

                string[] str = ((TextBox)sender).Text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string eachStr in str)
                {
                    txtOutput.Text += Calculate.Eval(eachStr);
                    txtOutput.Text += System.Environment.NewLine;
                }
                txtOutput.Text = txtOutput.Text.Replace("System.__ComObject", "");
            }
            catch { }
        }

        private void WinLoadedEventHandler(object sender, RoutedEventArgs e)
        {
            string tempFileName = System.IO.Path.GetTempFileName();
            using (FileStream fs = new FileStream(tempFileName, FileMode.Create))
            {
                Properties.Resources.icon.Save(fs);
            }
            this.Icon = new BitmapImage(new Uri(tempFileName));
            txtInput.Focus();
        }

    }
}
