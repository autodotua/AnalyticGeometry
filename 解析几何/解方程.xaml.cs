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
    /// 解方程.xaml 的交互逻辑
    /// </summary>
    public partial class 解方程 : Window
    {
        public 解方程()
        {
            InitializeComponent();
        }
        Dictionary<TextBox, string> lastString = new Dictionary<TextBox, string>();
        string equation = "";
        private void dichotomy(object sender, RoutedEventArgs e)
        {
            double a=double.Parse(start.Text),b=double.Parse(end.Text),ac=double.Parse(accuracy.Text),m;
            equation = Calculate.replace1("(" + input1.Text + ")-(" + input2.Text+")", variable.Text);
            do
            {
                m = (a + b) / 2;
                if (f(m) * f(a) < 0)
                {
                    b = m;
                }
                else
                {
                    a = m;
                }
            }
            while((b-a)/2>=ac);
            result.Text = Math.Round(m,accuracy.Text.Length - 3).ToString();
        }


        private double f(double x)
        {
            return double.Parse(Calculate.eval(equation, variable.Text, x.ToString()));
        }

        private void ta_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (((TextBox)sender).Text != "")
            {
                double trynum;
                try
                {
                    trynum = double.Parse(((TextBox)sender).Text);
                    lastString[(TextBox)sender] = ((TextBox)sender).Text;
                }
                catch (Exception)
                {
                    try
                    {
                        ((TextBox)sender).Text = lastString[(TextBox)sender];
                    }
                    catch
                    {
                        ((TextBox)sender).Text = "";
                    }
                }

            }
            try
            {
                double
                a = double.Parse(ta.Text),
                b = double.Parse(tb.Text),
                c = double.Parse(tc.Text) - double.Parse(tc2.Text);
                double p=b*b-4*a*c;
                double cf = Math.Sqrt(getCF(Math.Abs(2 * a) * Math.Abs(2 * a), Math.Abs(b) * Math.Abs(b), Math.Abs(p)));
                if(p==0)
                {
                    x.Text = (-b / cf).ToString();
                }
                else
                {
                    x.Text = (-b / cf).ToString() + "±√(" + ((p) / (cf * cf)).ToString() + ")";
                }
                if (2 * a /cf!= 1)
                {
                    x.Text += System.Environment.NewLine;
                    x_.Clear();
                    foreach (var i in x.Text)
                    {
                        x_.Text += "_";
                    }
                    x.Text += (2 * a / cf).ToString();
                }

                
                if (p >= 0)
                {
                    x1.Text = ((-b + Math.Sqrt(p)) / (2 * a)).ToString();
                    x2.Text = ((-b - Math.Sqrt(p)) / (2 * a)).ToString();
                }
                else
                {
                    x1.Text = ((-b)/(2*a)).ToString()+"+"+(Math.Sqrt(-p)/(2*a)).ToString()+"i";
                    x2.Text = ((-b)/(2*a)).ToString()+"-"+(Math.Sqrt(-p)/(2*a)).ToString()+"i";
                    x1.Text = x1.Text.Replace("--", "+").Replace("1i","i");
                    x2.Text = x2.Text.Replace("--", "+").Replace("1i","i");
                }
                //x2.Text = cf.ToString() + "  " + p.ToString();
            }
            catch 
            {
                try
                {
                    x.Clear();
                    x_.Clear();
                    x1.Clear();
                    x2.Clear();
                }
                catch { }
            }


        }
        private int getCF(double num1, double num2, double num3)
        {
            double max;
            if (num1 >= num2)
            {
                max = num1;
            }
            else
            {
                max = num2;
            }

            if (num3 >= max)
            {
                max = num3;
            }
            int cf = 1; ;
            if(max<1e6)
                for (int i = (int)max.Round(); i >= 1; i--)
                {
                    if (num1 % i == 0 && num2 % i == 0 && num3 % i == 0)
                    {
                        cf = i;
                        break;
                    }
                }
            return cf;
        }

        private void TextBoxPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
      
   
    }
}
