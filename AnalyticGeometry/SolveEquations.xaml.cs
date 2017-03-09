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
    /// SolveEquations.xaml 的交互逻辑
    /// </summary>
    public partial class SolveEquations : Window
    {
        public SolveEquations()
        {
            InitializeComponent();
        }
        Dictionary<TextBox, string> lastString = new Dictionary<TextBox, string>();
        string expression = "";
        private void SolveWithDichotomy(object sender, RoutedEventArgs e)
        {
            double a = double.Parse(txtStart.Text);
            double b = double.Parse(txtEnd.Text);
            double accuracy = double.Parse(this.txtAccuracy.Text);
            double m;
            expression = Calculate.ReplaceExpressionPreliminary("(" + txtInputLeftPart.Text + ")-(" + txtInputRightPart.Text+")", txtVariable.Text);
            if (F(a) * F(b) > 0)
            {
                new ErrorMessageBox(@"区间两头函数值同号，无法二分法解方程。
请先绘制图像，保证区间两头不同号。").Show();
                return;
            }
            do
            {
                m = (a + b) / 2;
                if (F(m) == 0)
                {
                    break;
                }
                else if (F(m) * F(a) < 0)
                {
                    b = m;
                }
                else
                {
                    a = m;
                }
            }
            while((b-a)/2>=accuracy);
            txtResult.Text = Math.Round(m, txtAccuracy.Text.Length - 3).ToString();
        }
       
        private double F(double txtQuadraticResultX)
        {
            return double.Parse(Calculate.Eval(expression, txtVariable.Text, txtQuadraticResultX.ToString()));
        }

        private void QuadraticTxtTextChangedEventHandler(object sender, TextChangedEventArgs e)
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
                a = double.Parse(txtQuadraticA.Text),
                b = double.Parse(txtQuadraticB.Text),
                c = double.Parse(txtQuadraticC.Text) - double.Parse(txtQuadraticD.Text);
                double p=b*b-4*a*c;
                double greatestCommonFactor = Math.Sqrt(GetGreatestCommonFactor(Math.Abs(2 * a) * Math.Abs(2 * a), Math.Abs(b) * Math.Abs(b), Math.Abs(p)));
                if(p==0)
                {
                    txtQuadraticResultX.Text = (-b / greatestCommonFactor).ToString();
                }
                else
                {
                    txtQuadraticResultX.Text = (-b / greatestCommonFactor).ToString() + "±√(" + ((p) / (greatestCommonFactor * greatestCommonFactor)).ToString() + ")";
                }
                if (2 * a /greatestCommonFactor!= 1)
                {
                    txtQuadraticResultX.Text += System.Environment.NewLine;
                    txtQuadraticFractionBar.Clear();
                    foreach (var i in txtQuadraticResultX.Text)
                    {
                        txtQuadraticFractionBar.Text += "_";
                    }
                    txtQuadraticResultX.Text += (2 * a / greatestCommonFactor).ToString();
                }

                
                if (p >= 0)
                {
                    txtQuadraticResultX1.Text = ((-b + Math.Sqrt(p)) / (2 * a)).ToString();
                    txtQuadraticResultX2.Text = ((-b - Math.Sqrt(p)) / (2 * a)).ToString();
                }
                else
                {
                    txtQuadraticResultX1.Text = ((-b)/(2*a)).ToString()+"+"+(Math.Sqrt(-p)/(2*a)).ToString()+"i";
                    txtQuadraticResultX2.Text = ((-b)/(2*a)).ToString()+"-"+(Math.Sqrt(-p)/(2*a)).ToString()+"i";
                    txtQuadraticResultX1.Text = txtQuadraticResultX1.Text.Replace("--", "+").Replace("1i","i");
                    txtQuadraticResultX2.Text = txtQuadraticResultX2.Text.Replace("--", "+").Replace("1i","i");
                }
                //txtQuadraticResultX2.Text = cf.ToString() + "  " + p.ToString();
            }
            catch 
            {
                try
                {
                    txtQuadraticResultX.Clear();
                    txtQuadraticFractionBar.Clear();
                    txtQuadraticResultX1.Clear();
                    txtQuadraticResultX2.Clear();
                }
                catch { }
            }


        }

        private int GetGreatestCommonFactor(double num1, double num2, double num3)
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
                for (int i = (int)(Math.Round(max)); i >= 1; i--)
                {
                    if (num1 % i == 0 && num2 % i == 0 && num3 % i == 0)
                    {
                        cf = i;
                        break;
                    }
                }
            return cf;
        }

        private void DisableInputTxtPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void WinLoadedEventHandler(object sender, RoutedEventArgs e)
        {
            txtInputLeftPart.Focus();
            string tempFileName = System.IO.Path.GetTempFileName();
            using (FileStream fs = new FileStream(tempFileName, FileMode.Create))
            {
                Properties.Resources.icon.Save(fs);
            }
            this.Icon = new BitmapImage(new Uri(tempFileName));
        }
    }
}
