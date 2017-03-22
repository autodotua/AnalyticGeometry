using Microsoft.Win32;
using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using MathExpressionsNET;

namespace AnalyticGeometry
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnalyticGeometryGraphs : Window
    {
        string functionExpression, parametricXExpression, parametricYExpression;
        CustomCoordinate cc;
        PresentationSource ps;
      
        public AnalyticGeometryGraphs()
        {
            InitializeComponent();

        }

        private void SaveGraphBtnClickEventHandler(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "透明图片png|*.png"
            };
            sfd.FileName = DateTime.Now.ToString("yyyyMMdd");



        }
        /// <summary>
        /// 绘制之前的初始化
        /// </summary>
        private void Initialize()
        {
            cc = new CustomCoordinate(grdMain.ColumnDefinitions[1].ActualWidth,
                grdMain.ActualHeight, double.Parse(txtCanvasLeft.Text),
                double.Parse(txtCanvasRight.Text), double.Parse(txtCanvasTop.Text),
                double.Parse(txtCanvasBottom.Text));//实例化一个自定义坐标系
            CalculateGridSpacing();//重置网格间距

            ClearAllGraphs();//清空坐标系和网格

            DrawCoordinate();//绘制坐标系和网格
        }
        /// <summary>
        /// 清除
        /// </summary>
        private void ClearAllGraphs()
        {
            cvsMainCanvas.Children.Clear();
        }
        /// <summary>
        /// 获取计算引擎
        /// </summary>
        /// <returns></returns>
        private int GetCalculateType()
        {
            if (rbtnCalculateType1.IsChecked == true)
            {
                return 1;
            }
            else if (rbtnCalculateType2.IsChecked == true)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="text">错误信息</param>
        private void ShowErrorMessage(string text)
        {
            new ErrorMessageBox(text).ShowDialog();
        }

        #region 绘制

        private void DrawCoordinate()
        {
            //double  pointWidth=
            DrawLine(Brushes.Black, 1.5, -cvsMainCanvas.ActualWidth, cc.ToScreenXAxis(),
                2 * cvsMainCanvas.ActualWidth, cc.ToScreenXAxis());//X轴
            DrawLine(Brushes.Black, 1.5, cc.ToScreenYAxis(), -cvsMainCanvas.ActualHeight,
                cc.ToScreenYAxis(), 2 * cvsMainCanvas.ActualHeight);//Y轴
            List<TextBlock> txtList = new List<TextBlock>();//坐标系文字的集合
            if (chkShowGrid.IsChecked == true)//如果勾选了“绘制表格”
            {
                //左边界横坐标
                double leftLine =
                    Math.Round(cc.ToRealX(-cvsMainCanvas.ActualWidth)
                    / double.Parse(txtVerticalSeparationDistance.Text))
                    * double.Parse(txtVerticalSeparationDistance.Text);
                //右边界横坐标
                double rightLine =
                    Math.Round(cc.ToRealX(2 * cvsMainCanvas.ActualWidth)
                    / double.Parse(txtVerticalSeparationDistance.Text))
                    * double.Parse(txtVerticalSeparationDistance.Text);
                //纵向网格
                for (double i = leftLine; i <= rightLine; i += double.Parse(txtVerticalSeparationDistance.Text))
                {
                    if (Math.Round(i, 5) != 0)
                    {
                        DrawLine(Brushes.Gray, 1, cc.ToScreenX(i), -cvsMainCanvas.ActualHeight, cc.ToScreenX(i), 2 * cvsMainCanvas.ActualHeight);
                        txtList.Add(new TextBlock());
                        txtList[txtList.Count - 1].Text = Math.Round(i, 5).ToString();
                        txtList[txtList.Count - 1].Margin = new Thickness(cc.ToScreenX(i), cc.ToScreenXAxis() - 15, 0, 0);
                        cvsMainCanvas.Children.Add(txtList[txtList.Count - 1]);
                    }
                }
                //上边界纵坐标
                double topLine = Math.Round(cc.ToRealY(-cvsMainCanvas.ActualHeight) / double.Parse(txtHorizontalSeparationDistance.Text)) * double.Parse(txtHorizontalSeparationDistance.Text);
                //下边界纵坐标
                double bottomLine = Math.Round(cc.ToRealY(2 * cvsMainCanvas.ActualHeight) / double.Parse(txtHorizontalSeparationDistance.Text)) * double.Parse(txtHorizontalSeparationDistance.Text);
                //横向网格
                for (double i = bottomLine; i <= topLine; i += double.Parse(txtHorizontalSeparationDistance.Text))
                {
                    if (Math.Round(i, 5) != 0)
                    {
                        DrawLine(Brushes.Gray, 1, -cvsMainCanvas.ActualWidth, cc.ToScreenY(i), 2 * cvsMainCanvas.ActualWidth, cc.ToScreenY(i));
                        txtList.Add(new TextBlock());
                        txtList[txtList.Count - 1].Text = Math.Round(i, 5).ToString();
                        txtList[txtList.Count - 1].Margin = new Thickness(cc.ToScreenYAxis() + 6, cc.ToScreenY(i), 0, 0);
                        cvsMainCanvas.Children.Add(txtList[txtList.Count - 1]);
                    }
                }
                //for (double i = 0 + double.Parse(txtVerticalSeparationDistance.Text); i <= double.Parse(txtCanvasRight.Text); i += double.Parse(txtVerticalSeparationDistance.Text))//Y轴右
                //{
                //    drawLine(Brushes.Gray, 1, cc.ToScreenX(i), 0, cc.ToScreenX(i), cvsMainCanvas.ActualHeight);
                //    tbList.Add(new TextBlock());
                //    tbList[tbList.Count - 1].Text = i.ToString();
                //    tbList[tbList.Count - 1].Margin = new Thickness(cc.ToScreenX(i), cc.ToScreenXAxis() - 15, 0, 0);
                //    cv.Children.Add(tbList[tbList.Count - 1]);

                //}
                //for (double i = 0 - double.Parse(txtVerticalSeparationDistance.Text); i >= double.Parse(txtCanvasLeft.Text); i -= double.Parse(txtVerticalSeparationDistance.Text))//Y轴左
                //{
                //    drawLine(Brushes.Gray, 1, cc.ToScreenX(i), 0, cc.ToScreenX(i), cv.ActualHeight);
                //    tbList.Add(new TextBlock());
                //    tbList[tbList.Count - 1].Text = i.ToString();
                //    tbList[tbList.Count - 1].Margin = new Thickness(cc.ToScreenX(i), cc.ToScreenXAxis() - 15, 0, 0);
                //    cv.Children.Add(tbList[tbList.Count - 1]);
                //}
                //for (double i = 0 + double.Parse(txtHorizontalSeparationDistance.Text); i <= double.Parse(txtCanvasTop.Text); i += double.Parse(txtHorizontalSeparationDistance.Text))//X轴上
                //{
                //    drawLine(Brushes.Gray, 1, 0, cc.ToScreenY(i), cv.ActualWidth, cc.ToScreenY(i));
                //    tbList.Add(new TextBlock());
                //    tbList[tbList.Count - 1].Text = i.ToString();
                //    tbList[tbList.Count - 1].Margin = new Thickness(cc.ToScreenYAxis() + 6, cc.ToScreenY(i), 0, 0);
                //    cv.Children.Add(tbList[tbList.Count - 1]);
                //}

                //for (double i = 0 - double.Parse(txtHorizontalSeparationDistance.Text); i >= double.Parse(txtCanvasBottom.Text); i -= double.Parse(txtHorizontalSeparationDistance.Text))//X轴下
                //{
                //    drawLine(Brushes.Gray, 1, 0, cc.ToScreenY(i), cv.ActualWidth, cc.ToScreenY(i));
                //    tbList.Add(new TextBlock());
                //    tbList[tbList.Count - 1].Text = i.ToString();
                //    tbList[tbList.Count - 1].Margin = new Thickness(cc.ToScreenYAxis() + 6, cc.ToScreenY(i), 0, 0);
                //    cv.Children.Add(tbList[tbList.Count - 1]);
                //}
            }

        }
        /// <summary>
        /// 绘制图像
        /// </summary>
        private void Draw()
        {
            if (tabInput.SelectedIndex == 0)//如果选择绘制连点成线
            {
                btnFunctionOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else
            {
                btnParametricOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        /// <summary>
        /// 通过四个坐标参数绘制线段
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            Line l = new Line();
            l.Stroke = colorPicker.CurrentColor;
            l.StrokeThickness = double.Parse(txtLineThickness.Text);
            l.X1 = x1;
            l.X2 = x2;
            l.Y1 = y1;
            l.Y2 = y2;
            cvsMainCanvas.Children.Add(l);

        }
        /// <summary>
        /// 通过两个点绘制线段
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void DrawLine(Point p1, Point p2)
        {
            Line l = new Line();
            l.Stroke = colorPicker.CurrentColor;
            l.StrokeThickness = double.Parse(txtLineThickness.Text);
            l.X1 = p1.X;
            l.X2 = p2.X;
            l.Y1 = p1.Y;
            l.Y2 = p2.Y;
            cvsMainCanvas.Children.Add(l);
        }
        /// <summary>
        /// 绘制制定颜色和粗细的线段
        /// </summary>
        /// <param name="brush">线条颜色</param>
        /// <param name="thickness">线条粗细</param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void DrawLine(SolidColorBrush brush, double thickness, double x1, double y1, double x2, double y2)
        {
            Line l = new Line();
            l.Stroke = brush;
            l.StrokeThickness = thickness;
            l.X1 = x1;
            l.X2 = x2;
            l.Y1 = y1;
            l.Y2 = y2;
            cvsMainCanvas.Children.Add(l);
        }
        /// <summary>
        /// 绘制圆形
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void DrawCricle(double x, double y)
        {
            Ellipse e = new Ellipse();
            e.Stroke = colorPicker.CurrentColor;
            e.StrokeThickness = double.Parse(txtLineThickness.Text);
            e.Margin = new Thickness(x - double.Parse(txtLineThickness.Text) / 2,//左
                y - double.Parse(txtLineThickness.Text) / 2, //上
                2 * double.Parse(txtLineThickness.Text), //右
                2 * double.Parse(txtLineThickness.Text));//下
            cvsMainCanvas.Children.Add(e);
        }
        /// <summary>
        /// 绘制能够改变线条粗细的圆形
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="thickness"></param>
        private void DrawCricle(double x, double y, double thickness)
        {
            Ellipse e = new Ellipse();
            e.Stroke = colorPicker.CurrentColor;
            e.StrokeThickness = double.Parse(txtLineThickness.Text);
            e.Margin = new Thickness(x - thickness / 2,//左
                y - thickness / 2, //上
                2 * thickness, //右
                2 * thickness);//下
            cvsMainCanvas.Children.Add(e);
        }
        #endregion

        #region 函数
        /// <summary>
        /// 单击绘制函数图像按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionOKBtnClickEventHandler(object sender, EventArgs e)
        {

            colorPicker.BorderBrush = Brushes.Red;
            Initialize();

            try
            {
                //  if(chkMultipleFunction.IsChecked==true)
                // {
                //把每一行的函数单独绘制
                foreach (var i in txtFunctionInput.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Regex rgxPoint = new Regex(@"[0-9]+\.?[0-9]*,[0-9]+\.?[0-9]*");//坐标点类似“3.4,2.4”的正则
                    if (rgxPoint.IsMatch(i))//如果是个点坐标
                    {
                        DrawCricle(cc.ToScreenX(double.Parse(i.Split(new char[] { ',' })[0])), cc.ToScreenY(double.Parse(i.Split(new char[] { ',' })[1])), double.Parse(txtLineThickness.Text) * 2);
                    }
                    else//如果是个函数
                    {
                        switch (GetCalculateType())

                        {
                            case 1: functionExpression = Calculate.ReplaceExpressionPreliminary(i, txtFunctionVariable.Text); break;
                            case 2:
                            case 3: functionExpression = i; break;
                        } 
                       
                       
                        
                        DrawFunctionGraph();
                    }
                }
                //  }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 绘制函数图象
        /// </summary>
        private void DrawFunctionGraph()
        {
            //
            double realPrecision = (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) //实际宽度
                / cvsMainCanvas.ActualWidth //实际宽度/屏幕宽度获得比值
                * double.Parse(txtFunctionPrecision.Text);//每隔x个像素计算一个点坐标
            //List<double> xarray = new List<double>();
    
            List<Point> points = new List<Point>();//点坐标集合
            //最左端和最右端的横坐标
            double leftLine = Math.Round(cc.ToRealX(-cvsMainCanvas.ActualWidth)
                / double.Parse(txtVerticalSeparationDistance.Text))
                * double.Parse(txtVerticalSeparationDistance.Text);
            double rightLine = Math.Round(cc.ToRealX(2 * cvsMainCanvas.ActualWidth)
                / double.Parse(txtVerticalSeparationDistance.Text))
                * double.Parse(txtVerticalSeparationDistance.Text);
            //计算点坐标
            //Stopwatch sw = new Stopwatch();
            // sw.Start();
            if (GetCalculateType() == 1)
            {
                for (double i = leftLine; i <= rightLine; i += realPrecision)
                {
                    points.Add(new Point(
                        cc.ToScreenX(i),
                       cc.ToScreenY(double.Parse(Calculate.Eval(functionExpression, txtFunctionVariable.Text, i.ToString())))));

                }
            }
            else if (GetCalculateType() == 2)
            {
                MathExpression functionMathExpression = new MathExpression(functionExpression, txtFunctionVariable.Text);
                for (double i = leftLine; i <= rightLine; i += realPrecision)
                {
                    points.Add(new Point(
                        cc.ToScreenX(i),
                       // cc.ToScreenY(double.Parse(Calculate.Eval(functionExpression, txtFunctionVariable.Text, i.ToString())))));
                       cc.ToScreenY(functionMathExpression.Eval(i))));
                }
            }
            else
            {
                for (double i = leftLine; i <= rightLine; i += realPrecision)
                {
                    MathAssembly functionMathAssembly = new MathAssembly(functionExpression, txtFunctionVariable.Text);
                    points.Add(new Point(
                           cc.ToScreenX(i),
                          // cc.ToScreenY(double.Parse(Calculate.Eval(functionExpression, txtFunctionVariable.Text, i.ToString())))));
                          cc.ToScreenY(functionMathAssembly.Func(i))));
                } }
            //sw.Stop();
            //Debug.WriteLine(sw.ElapsedMilliseconds);
            if (rbtnGraphTypeOfLine.IsChecked == true)//如果是连线
            {
                for (int i = 1; i < points.Count; i++)
                {
                    //if (points[i - 1].Y >= -100
                    //    && points[i].Y <= cvsMainCanvas.ActualHeight + 100
                    //    && points[i - 1].Y <= cvsMainCanvas.ActualHeight + 100
                    //    && points[i].Y >= -100)

                    if (points[i - 1].Y >= -cvsMainCanvas.ActualHeight * 4
                    && points[i].Y <= cvsMainCanvas.ActualHeight * 5
                    && points[i - 1].Y <= cvsMainCanvas.ActualHeight * 5
                    && points[i].Y >= -cvsMainCanvas.ActualHeight * 4)
                    {
                        //如果这个和前一个点在视图及周围扩展的9*9-1个同样的视图中的话则连接这一个点和上一个点
                        //此举是为了防止获取到没有定义的点导致错误
                        DrawLine(points[i - 1].X, points[i - 1].Y, points[i].X, points[i].Y);
                    }

                    //else if (points[i - 1].Y < cvsMainCanvas.ActualHeight 
                    //    && points[i - 1].Y > 0 
                    //    && points[i].Y <= 0)
                    //{
                    //    DrawLine(points[i - 1].X, points[i - 1].Y, points[i - 1].X, 0);
                    //}
                    //else if (points[i - 1].Y <= 0 
                    //    && points[i].Y > 0 
                    //    && points[i].Y < cvsMainCanvas.ActualHeight)
                    //{
                    //    DrawLine(points[i].X, 0, points[i].X, points[i].Y);
                    //}

                    //else if (points[i - 1].Y < cvsMainCanvas.ActualHeight && points[i - 1].Y > 0 && points[i].Y > cvsMainCanvas.ActualHeight)
                    //{
                    //    DrawLine(points[i - 1].X, points[i - 1].Y, points[i - 1].X, cvsMainCanvas.Height);
                    //}
                    //else if (points[i - 1].Y > cvsMainCanvas.ActualHeight && points[i].Y > 0 && points[i].Y < cvsMainCanvas.ActualHeight)
                    //{
                    //    DrawLine(points[i].X, cvsMainCanvas.Height, points[i].X, points[i].Y);
                    //}

                }
            }


            else//如果是画点
            {
                for (int i = 0; i < points.Count; i++)
                {
                    DrawCricle(points[i].X, points[i].Y);
                }
            }


        }
        #endregion

        #region 参数方程
        /// <summary>
        /// 单击绘制参数方程按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParametricOKBtnClickEventHandler(object sender, EventArgs e)
        {
            Initialize();
            switch (GetCalculateType())

            {
                case 1:
                    parametricXExpression = Calculate.ReplaceExpressionPreliminary(txtParametricX.Text, txtParametricParameter.Text);
                    parametricYExpression = Calculate.ReplaceExpressionPreliminary(txtParametricY.Text, txtParametricParameter.Text);
                    break;
                case 2:
                case 3:
                    parametricXExpression = txtParametricX.Text;
                    parametricYExpression = txtParametricY.Text;
                    break;
            }
            try
            {
                DrawParametricGraph();
            }
            catch { }

        }
        /// <summary>
        /// 绘制参数方程
        /// </summary>
        private void DrawParametricGraph()
        {
            Pen p = new Pen(colorPicker.CurrentColor, int.Parse(txtLineThickness.Text));
            List<Point> points = new List<Point>();
            if (GetCalculateType() == 1)
            {
             
                for (double i = double.Parse(txtParametricStart.Text); i <= double.Parse(txtParametricEnd.Text); i += double.Parse(txtParametricPrecision.Text))
                {
                    points.Add(new Point(
                        cc.ToScreenX(double.Parse(Calculate.Eval(parametricXExpression, txtParametricParameter.Text, i.ToString()))),
                        cc.ToScreenY(double.Parse(Calculate.Eval(parametricYExpression, txtParametricParameter.Text, i.ToString())))));
                 
                }
            }
           else if (GetCalculateType() == 2)
            {
                MathExpression parametricXMathExpression = new MathExpression(parametricXExpression, txtParametricParameter.Text);
                MathExpression parametricYMathExpression = new MathExpression(parametricYExpression, txtParametricParameter.Text);

                for (double i = double.Parse(txtParametricStart.Text); i <= double.Parse(txtParametricEnd.Text); i += double.Parse(txtParametricPrecision.Text))
                {
                    points.Add(new Point(
                        // cc.ToScreenX(double.Parse(Calculate.Eval(parametricXExpression, txtParametricParameter.Text, i.ToString()))),
                        //cc.ToScreenY(double.Parse(Calculate.Eval(parametricYExpression, txtParametricParameter.Text, i.ToString())))));
                        cc.ToScreenX(parametricXMathExpression.Eval(i)),
                        cc.ToScreenY(parametricYMathExpression.Eval(i))));
                }
            }
            else
            {
                MathAssembly parametricXMathAssembly = new MathAssembly(parametricXExpression, txtParametricParameter.Text);
MathAssembly parametricYMathAssembly = new MathAssembly(parametricYExpression, txtParametricParameter.Text);

                for (double i = double.Parse(txtParametricStart.Text); i <= double.Parse(txtParametricEnd.Text); i += double.Parse(txtParametricPrecision.Text))
                {
                    points.Add(new Point(
                        // cc.ToScreenX(double.Parse(Calculate.Eval(parametricXExpression, txtParametricParameter.Text, i.ToString()))),
                        //cc.ToScreenY(double.Parse(Calculate.Eval(parametricYExpression, txtParametricParameter.Text, i.ToString())))));
                        cc.ToScreenX(parametricXMathAssembly.Func(i)),
                        cc.ToScreenY(parametricXMathAssembly.Func(i))));
                }
            }
            if (rbtnGraphTypeOfLine.IsChecked == true)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    //if (points[i - 1].Y >= -100
                    //    && points[i].Y <= cvsMainCanvas.ActualHeight + 100
                    //    && points[i - 1].Y <= cvsMainCanvas.ActualHeight + 100
                    //    && points[i].Y >= -100)

                    if (points[i - 1].Y >= -cvsMainCanvas.ActualHeight * 4
                    && points[i].Y <= cvsMainCanvas.ActualHeight * 5
                    && points[i - 1].Y <= cvsMainCanvas.ActualHeight * 5
                    && points[i].Y >= -cvsMainCanvas.ActualHeight * 4)

                    {
                        DrawLine(points[i - 1].X, points[i - 1].Y, points[i].X, points[i].Y);
                    }
                }
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    DrawCricle(points[i].X, points[i].Y);
                }
            }

        }
        #endregion

        #region 限制操作
        Dictionary<TextBox, string> lastString = new Dictionary<TextBox, string>();
        /// <summary>
        /// 只允许输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UniversalTxtEnterOnlyNumberTextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            double tryNum;
            if (((TextBox)sender).Text == "-")
            {
                return;
            }
            if (((TextBox)sender).Text != "")
            {
                try
                {
                    tryNum = double.Parse(((TextBox)sender).Text);
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
                    ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                }
            }
        }
        /// <summary>
        /// 只允许输入正数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UniversalTxtEnterOnlyPositiveNumberTextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            double tryNum;
            if (((TextBox)sender).Text != "")
            {
                try
                {
                    tryNum = double.Parse(((TextBox)sender).Text);
                    if (tryNum <= 0)
                    {
                        ((TextBox)sender).Text = lastString[(TextBox)sender];
                        return;
                    }
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
                    ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                }
            }
        }
        /// <summary>
        /// 只允许输入整数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UniversalTxtEnterOnlyIntegerTextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text != "")
            {
                double tryNum;
                try
                {
                    tryNum = double.Parse(((TextBox)sender).Text);
                    if (tryNum != Math.Round(tryNum) || tryNum <= 0)
                    {
                        ((TextBox)sender).Text = lastString[(TextBox)sender];
                        return;
                    }
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
                    ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                }
            }
        }
        /// <summary>
        /// 不允许输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UniversalTxtEnterNoNumberTextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            for (int i = 0; i <= 9; i++)
            {
                if (((TextBox)sender).Text.Contains(i.ToString()))
                {
                    ((TextBox)sender).Text = lastString[(TextBox)sender];
                    ((TextBox)sender).SelectionStart = ((TextBox)sender).Text.Length;
                    return;
                }
            }
            lastString[(TextBox)sender] = ((TextBox)sender).Text;
        }
        #endregion

        #region 键盘操作
        /// <summary>
        /// 函数图象输入框按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionOKTxtPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {

            //if (e.Key == Key.Space)
            //{
            //    e.Handled = true;
            //}
            if (e.Key == Key.Enter)
            {
                if (chkMultipleFunction.IsChecked == false)
                {
                    btnFunctionOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    e.Handled = true;
                }
                //else
                //{
                //    e.Handled = true;
                //}
            }
        }
        /// <summary>
        /// 参数方程X输入框按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtParametricXBtnPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                txtParametricY.Focus();
            }
        }
        /// <summary>
        /// 参数方程Y输入框按下按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtParametricYBtnPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (e.Key == Key.Enter)
            {
                btnParametricOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        #endregion

        #region 配置项
        /// <summary>
        /// 读注册表
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfig(string key)
        {
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("software\\AutodotuaSoftware\\Analytic-Geometry");
            try
            {
                return rk.GetValue(key).ToString();
            }
            catch (Exception)
            {
                return null;
            }

        }
        /// <summary>
        /// 写注册表
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetConfig(string key, string value)
        {
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("software\\AutodotuaSoftware\\Analytic-Geometry");
            try
            {
                rk.SetValue(key, value);
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 启动程序后要做的事
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinLoadedEventHandler(object sender, EventArgs e)
        {
            //设置窗体图标
            string tempFileName = System.IO.Path.GetTempFileName();
            using (FileStream fs = new FileStream(tempFileName, FileMode.Create))
            {
                Properties.Resources.icon.Save(fs);
            }
            this.Icon = new BitmapImage(new Uri(tempFileName));

            //让函数图象输入框获取焦点
            txtFunctionInput.Focus();

            //实例化PresentationSource为获取DPI做准备
            ps = PresentationSource.FromVisual(this);

            //逐一获取配置项并应用到控件
            if (GetConfig("haveOpened") == "1")
            {

                txtGraphColor.Text = GetConfig("txtGraphColor") == null ? "#FFFF0000" : GetConfig("txtGraphColor");
                txtFunctionPrecision.Text = GetConfig("txtFunctionPrecision");
                txtFunctionVariable.Text = GetConfig("txtFunctionVariable") == null ? "x" : GetConfig("txtFunctionVariable");
                txtCanvasTop.Text = GetConfig("txtCanvasTop");
                txtCanvasLeft.Text = GetConfig("txtCanvasLeft");
                txtCanvasRight.Text = GetConfig("txtCanvasRight");
                txtCanvasBottom.Text = GetConfig("txtCanvasBottom");
                txtVerticalSeparationDistance.Text = GetConfig("txtVerticalSeparationDistance");
                txtHorizontalSeparationDistance.Text = GetConfig("txtHorizontalSeparationDistance");
                txtLineThickness.Text = GetConfig("txtLineThickness");
                txtParametricParameter.Text = GetConfig("txtParametricParameter") == null ? "t" : GetConfig("txtParametricParameter");
                txtParametricStart.Text = GetConfig("txtParametricStart");
                txtParametricEnd.Text = GetConfig("txtParametricEnd");
                txtParametricPrecision.Text = GetConfig("txtParametricPrecision");

                switch (GetConfig("chkMultipleFunction"))
                {
                    case "true": chkMultipleFunction.IsChecked = true; break;
                    case "false": chkMultipleFunction.IsChecked = false; break;
                }

                switch (GetConfig("chkDrawGridAutomatically"))
                {
                    case "true": chkDrawGridAutomatically.IsChecked = true; break;
                    case "false": chkDrawGridAutomatically.IsChecked = false; break;
                }
                switch (GetConfig("chkShowGrid"))
                {
                    case "true": chkShowGrid.IsChecked = true; break;
                    case "false": chkShowGrid.IsChecked = false; break;
                }
                switch(GetConfig("rbtnCalculateType"))
                {
                    case "1":rbtnCalculateType1.IsChecked = true; ;break;
                    case "2":rbtnCalculateType2.IsChecked = true;break;
                    case "3":rbtnCalculateType3.IsChecked = true;break;
                    default:rbtnCalculateType1.IsChecked = true;break;
                }
                if (GetConfig("rbtnGraphTypeOfLine") == "0")
                {
                    rbtnGraphTypeOfPoint.IsChecked = true;
                    rbtnGraphTypeOfLine.IsChecked = false;
                }
            }
            else
            {
                rbtnCalculateType2.IsChecked = true;
            }
            //显示坐标系
            Initialize();
            // Draw();
        }
        //关闭程序后保存设置项
        private void WinClosingEventHandler(object sender, CancelEventArgs e)
        {
            SetConfig("txtGraphColor", txtGraphColor.Text);
            SetConfig("txtFunctionPrecision", txtFunctionPrecision.Text);
            SetConfig("txtFunctionVariable", txtFunctionVariable.Text);
            SetConfig("txtCanvasTop", txtCanvasTop.Text);
            SetConfig("txtCanvasLeft", txtCanvasLeft.Text);
            SetConfig("txtCanvasRight", txtCanvasRight.Text);
            SetConfig("txtCanvasBottom", txtCanvasBottom.Text);
            SetConfig("txtVerticalSeparationDistance", txtVerticalSeparationDistance.Text);
            SetConfig("txtHorizontalSeparationDistance", txtHorizontalSeparationDistance.Text);
            SetConfig("txtLineThickness", txtLineThickness.Text);
            SetConfig("txtParametricParameter", txtParametricParameter.Text);
            SetConfig("txtParametricParameter", txtParametricParameter.Text);
            SetConfig("txtParametricStart", txtParametricStart.Text);
            SetConfig("txtParametricEnd", txtParametricEnd.Text);
            SetConfig("txtParametricPrecision", txtParametricPrecision.Text);
            switch (chkMultipleFunction.IsChecked)
            {
                case true: SetConfig("chkMultipleFunction", "true"); break;
                case false: SetConfig("chkMultipleFunction", "false"); break;
            }

            switch (chkDrawGridAutomatically.IsChecked)
            {
                case true: SetConfig("chkDrawGridAutomatically", "true"); break;
                case false: SetConfig("chkDrawGridAutomatically", "false"); break;
            }
            switch (chkShowGrid.IsChecked)
            {
                case true: SetConfig("chkShowGrid", "true"); break;
                case false: SetConfig("chkShowGrid", "false"); break;
            }
            if(rbtnCalculateType1.IsChecked==true)
            {
                SetConfig("rbtnCalculateType", "1");
            }
            else if(rbtnCalculateType2.IsChecked==true)
            {
                SetConfig("rbtnCalculateType", "2");
            }
            else
            {
                SetConfig("rbtnCalculateType", "3");
            }
            if (rbtnGraphTypeOfLine.IsChecked == true)
            {
                SetConfig("rbtnGraphTypeOfLine", "1");
            }
            else
            {
                SetConfig("rbtnGraphTypeOfLine", "0");
            }
            SetConfig("haveOpened", "1");
        }
        /// <summary>
        /// 单击恢复初始化按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelRegeditItems(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser;
            try
            {
                rk.DeleteSubKey("software\\AutodotuaSoftware\\Analytic-Geometry");
                string strAppFileName = Process.GetCurrentProcess().MainModule.FileName;
                //重启
                Process myNewProcess = new Process();
                myNewProcess.StartInfo.FileName = strAppFileName;
                myNewProcess.StartInfo.WorkingDirectory = Process.GetCurrentProcess().MainModule.FileName;
                myNewProcess.Start();
                System.Environment.Exit(0);

            }
            catch (Exception)
            {
                ShowErrorMessage("找不到配置文件！");
            }
        }

        #endregion

        #region 缩放平移

        bool IsMouseDown = false;
        Point mousePoint;
        Point startMousePoint;
        double startCvWidth;
        double startCvHeight;
        /// <summary>
        /// 按下左移按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveCoordinateToLeftBtnClickEventHandler(object sender, EventArgs e)
        {
            string temp = txtCanvasLeft.Text;
            txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
            txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(temp)) / 5), 2).ToString();
            ClearAllGraphs();
            Initialize();
            if (double.Parse(txtFunctionPrecision.Text) >= 4)
            {
                btnFunctionOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        /// <summary>
        /// 按下上移按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveCoordinateToUpBtnClickEventHandler(object sender, EventArgs e)
        {
            string temp = txtCanvasTop.Text;
            txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) + (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) + (double.Parse(temp) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            ClearAllGraphs();
            Initialize();
            if (double.Parse(txtFunctionPrecision.Text) >= 4)
            {
                btnFunctionOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        /// <summary>
        /// 按下右移按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveCoordinateToRightBtnClickEventHandler(object sender, EventArgs e)
        {
            string temp = txtCanvasLeft.Text;
            txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
            txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(temp)) / 5), 2).ToString();
            ClearAllGraphs();
            Initialize();
            if (double.Parse(txtFunctionPrecision.Text) >= 4)
            {
                btnFunctionOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        /// <summary>
        /// 按下下移按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveCoordinateToDownBtnClickEventHandler(object sender, EventArgs e)
        {
            string temp = txtCanvasTop.Text;
            txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) - (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) - (double.Parse(temp) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            ClearAllGraphs();
            Initialize();
            if (double.Parse(txtFunctionPrecision.Text) >= 4)
            {
                btnFunctionOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        /// <summary>
        /// 鼠标滚轮缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinMouseWheelEventHandler(object sender, MouseWheelEventArgs e)
        {
            //如果是在画布部分滚轮
            if (Mouse.GetPosition(this as FrameworkElement).X > 250)
            {

                if (e.Delta > 0)
                {
                    string rawTop = txtCanvasTop.Text;
                    string rawLeft = txtCanvasLeft.Text;
                    txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) - (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
                    txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) + (double.Parse(rawTop) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
                    txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
                    txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(rawLeft)) / 5), 2).ToString();


                }
                else
                {
                    string rawTemp = txtCanvasTop.Text;
                    string rawLeft = txtCanvasLeft.Text;
                    txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) + (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
                    txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) - (double.Parse(rawTemp) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
                    txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
                    txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(rawLeft)) / 5), 2).ToString();

                }

                Draw();
            }
            //如果在操作区滚轮
            else
            {
                //如果界面高度足够就不需要滚轮来调节了了
                if (this.ActualHeight >= 770)
                    return;
                else
                {
                    if (e.Delta > 0)//向上滚
                    {
                        //  Debug.WriteLine();
                        if (grdSettings.Margin.Top < 4)
                        {
                            grdSettings.Margin = new Thickness(
                                 grdSettings.Margin.Left,
                                  grdSettings.Margin.Top + 8,
                                   grdSettings.Margin.Right,
                                    grdSettings.Margin.Bottom + 8);
                        }
                    }
                    else//向下滚
                    {
                        //Debug.WriteLine("2");
                        if (this.Height - grdSettings.Margin.Top > 750)
                        {
                            //如果滚到底了，下面没东西好显示了
                            return;
                        }
                        grdSettings.Margin = new Thickness(
                                 grdSettings.Margin.Left,
                                  grdSettings.Margin.Top - 8,
                                   grdSettings.Margin.Right,
                                    grdSettings.Margin.Bottom - 8);


                        // if () { }
                    }
                }

            }

        }
        /// <summary>
        /// 如果开启了自动网格那么久自动计算网格间距
        /// </summary>
        private void CalculateGridSpacing()
        {
            try
            {
                if (chkDrawGridAutomatically.IsChecked == true)
                {
                    double realwidth = double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text);
                    double realheight = double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text);
                    if (realwidth > 1)
                    {

                        int intrealwidth = (int)Math.Round(realwidth);
                        if (int.Parse(intrealwidth.ToString()[0].ToString()) < 3)
                        {
                            txtVerticalSeparationDistance.Text = Math.Pow(10, (intrealwidth.ToString().Length - 2)).ToString();
                        }
                        else if (int.Parse(intrealwidth.ToString()[0].ToString()) < 5)
                        {
                            txtVerticalSeparationDistance.Text = (2 * Math.Pow(10, (intrealwidth.ToString().Length - 2))).ToString();
                        }
                        else
                        {
                            txtVerticalSeparationDistance.Text = (5 * Math.Pow(10, (intrealwidth.ToString().Length - 2))).ToString();
                        }
                    }
                    if (realheight > 1)
                    {
                        int intrealheight = (int)Math.Round(realheight);
                        if (int.Parse(intrealheight.ToString()[0].ToString()) < 3)
                        {
                            txtHorizontalSeparationDistance.Text = Math.Pow(10, (intrealheight.ToString().Length - 2)).ToString();
                        }
                        else if (int.Parse(intrealheight.ToString()[0].ToString()) < 5)
                        {
                            txtHorizontalSeparationDistance.Text = (2 * Math.Pow(10, (intrealheight.ToString().Length - 2))).ToString();
                        }
                        else
                        {
                            txtHorizontalSeparationDistance.Text = (5 * Math.Pow(10, (intrealheight.ToString().Length - 2))).ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                ShowErrorMessage("缩放超出范围！将重置。");
                txtCanvasLeft.Text = "-10";
                txtCanvasRight.Text = "10";
                txtCanvasTop.Text = "10";
                txtCanvasBottom.Text = "-10";
            }
        }
        /// <summary>
        /// 单击方形网格按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MakeGridSquare(object sender, EventArgs e)
        {
            double proportion;
            double middle;
            middle = 0.5 * (double.Parse(txtCanvasTop.Text) + double.Parse(txtCanvasBottom.Text));//上下的中心点
            proportion = (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / cvsMainCanvas.ActualWidth;//计算比例
            txtCanvasBottom.Text = (middle - 0.5 * proportion * cvsMainCanvas.ActualHeight).ToString();
            txtCanvasTop.Text = (middle + 0.5 * proportion * cvsMainCanvas.ActualHeight).ToString();
            CalculateGridSpacing();
            ClearAllGraphs();
            Initialize();
            Draw();
        }


        [DllImport("user32.dll")]
        //限制鼠标在一个矩形区域内
        static extern bool ClipCursor(ref RECT lpRect);

        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public RECT(Int32 left, Int32 top, Int32 right, Int32 bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }
        RECT rect;
        /// <summary>
        /// 在窗体上按下鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinPreviewMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(cvsMainCanvas).X > 0
                && e.GetPosition(cvsMainCanvas).X < cvsMainCanvas.ActualWidth
                && e.GetPosition(cvsMainCanvas).Y > 0
                && e.GetPosition(cvsMainCanvas).Y < cvsMainCanvas.ActualHeight)//如果实在绘图区域按下的
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    IsMouseDown = true;
                    mousePoint = e.GetPosition(this);//记录鼠标按下的位置
                    startMousePoint = mousePoint;
                    startCvWidth = cvsMainCanvas.ActualWidth;
                    startCvHeight = cvsMainCanvas.ActualHeight;
                    //记录绘图区域的上下左右相对于屏幕左上角的绝对坐标
                    rect = new RECT(
                (int)((this.Left + grdMain.ColumnDefinitions[0].ActualWidth) * ps.CompositionTarget.TransformToDevice.M11) + 1,
                (int)(this.Top * ps.CompositionTarget.TransformToDevice.M11) + 1,
                (int)((this.Left + this.ActualWidth) * ps.CompositionTarget.TransformToDevice.M11) - 1,
                (int)((this.ActualHeight + this.Top) * ps.CompositionTarget.TransformToDevice.M11) - 1
                );
                }
            }
        }
        /// <summary>
        /// 窗体上的鼠标移动事件，即拖放画面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinMouseMoveEventHandler(object sender, MouseEventArgs e)
        {

            if (IsMouseDown)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    //    Debug.Write(rect.Left+"  ");
                    //    Debug.Write(rect.Right + "  ");
                    //    Debug.Write(rect.Top + "  ");
                    //    Debug.WriteLine(rect.Bottom);
                    ClipCursor(ref rect);//把鼠标限制在绘图区域内

                    Point theMousePoint = e.GetPosition(this);
                    // if (theMousePoint.X > cv.Margin.Left && theMousePoint.X < cv.Margin.Left + cv.ActualWidth && theMousePoint.Y > cv.Margin.Top && theMousePoint.Y < cv.Margin.Top+cv.ActualWidth)
                    //   {
                    //if (theMousePoint.X > 242)
                    //{
                    cvsMainCanvas.Margin = new Thickness(cvsMainCanvas.Margin.Left - (mousePoint.X - theMousePoint.X),
                        cvsMainCanvas.Margin.Top - (mousePoint.Y - theMousePoint.Y),
                        -(cvsMainCanvas.Margin.Left - (mousePoint.X - theMousePoint.X)),
                        -(cvsMainCanvas.Margin.Top - (mousePoint.Y - theMousePoint.Y)));
                    mousePoint = theMousePoint;
                    //}
                    //   }
                }
            }
        }

        /// <summary>
        /// 窗体上的鼠标抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinMouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseDown)//如果是拖放完成
            {

                IsMouseDown = false;

                double realWidth = double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text);
                double realHeight = double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text);
                //应用当前坐标到控件
                txtCanvasLeft.Text = (double.Parse(txtCanvasLeft.Text) - realWidth * (e.GetPosition(this).X - startMousePoint.X) / startCvWidth).ToString();
                txtCanvasRight.Text = (double.Parse(txtCanvasRight.Text) - realWidth * (e.GetPosition(this).X - startMousePoint.X) / startCvWidth).ToString();
                txtCanvasTop.Text = (double.Parse(txtCanvasTop.Text) + realHeight * (e.GetPosition(this).Y - startMousePoint.Y) / startCvHeight).ToString();
                txtCanvasBottom.Text = (double.Parse(txtCanvasBottom.Text) + realHeight * (e.GetPosition(this).Y - startMousePoint.Y) / startCvHeight).ToString();

                cvsMainCanvas.Margin = new Thickness(0);
                //恢复对 鼠标的限制
                RECT rect = new RECT(0, 0, (int)(SystemParameters.PrimaryScreenWidth * ps.CompositionTarget.TransformToDevice.M11), (int)(SystemParameters.PrimaryScreenHeight * ps.CompositionTarget.TransformToDevice.M11));
                ClipCursor(ref rect);

                Draw();

            }
        }

        /// <summary>
        /// 窗体大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinSizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {
            //Debug.WriteLine(this.Height);
            //Debug.WriteLine(grdSettings.Margin.Top);

            //如果发现最底端已经到底了，那么要把Margin往下调，也就是底部吸住，直到顶部完全露出来为止。
            while (this.Height - grdSettings.Margin.Top > 770 && grdSettings.Margin.Top < 4)
            {
                grdSettings.Margin = new Thickness(
                     grdSettings.Margin.Left,
                      grdSettings.Margin.Top + 1,

            grdSettings.Margin.Right,
                        grdSettings.Margin.Bottom + 1);
                //Debug.WriteLine("2     " + grdSettings.Margin.Top);
            }
            Initialize();
            //Draw();

        }
        /// <summary>
        /// 单击放大按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomInBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasRight.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) - (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) + (double.Parse(temp1) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
            txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(temp2)) / 5), 2).ToString();
            Draw();
        }
        /// <summary>
        /// 单击缩小按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomOutBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) + (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) - (double.Parse(temp1) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
            txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(temp2)) / 5), 2).ToString();
            Draw();
        }
        /// <summary>
        /// 单击拉伸高度按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomInHeightBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) - (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) + (double.Parse(temp1) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            Draw();
        }
        /// <summary>
        /// 单击收缩高度按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomOutHeightBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) + (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) - (double.Parse(temp1) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            Draw();
        }
        /// <summary>
        /// 单击拉伸宽度按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomInWidthBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
            txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(temp2)) / 5), 2).ToString();
            Draw();
        }
        /// <summary>
        /// 单击收缩宽度按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomOutWidthBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
            txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(temp2)) / 5), 2).ToString();
            Draw();
        }


        #endregion
        /// <summary>
        /// 单击显示绘图类型帮助按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowGraphTypeHelpBtnClickEventHandler(object sender, EventArgs e)
        {
            new PromptMessageBox(@"连点成线：适用于大多数图像
点：适用于有突变的图像和直线，但斜率过大部分效果不好"
                           ).ShowDialog();
        }
        #region 手势操作
        int intFingerMount = 0;
        double dblRawTop;
        double dblRawLeft;
        double dblRawRight;
        double dblRawBottom;
        double dblRawCenterX;
        double dblRawCenterY;
        bool blnScaleInsteadOfMove = false;//是否曾经进行过双指缩放
        /// <summary>
        /// 开始手势操作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinManipulationStartingEventHandler(object sender, ManipulationStartingEventArgs e)
        {
            dblRawTop = double.Parse(txtCanvasTop.Text);
            dblRawLeft = double.Parse(txtCanvasLeft.Text);
            dblRawRight = double.Parse(txtCanvasRight.Text);
            dblRawBottom = double.Parse(txtCanvasBottom.Text);
            dblRawCenterX = 0.5 * (dblRawLeft + dblRawRight);
            dblRawCenterY = 0.5 * (dblRawTop + dblRawBottom);
            e.ManipulationContainer = cvsMainCanvas;
            e.Mode = ManipulationModes.All;
            //Debug.WriteLine("start");
            blnScaleInsteadOfMove = false;
        }
        /// <summary>
        /// 进行手势操作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinManipulationDeltaEventHandler(object sender, ManipulationDeltaEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.Source;//设定元素
            try
            {
                Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;//实例化一个向量坐标系
                var deltaManipulation = e.CumulativeManipulation;//获取手势该变量

                Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);//获取中心点坐标

                double scale = 1 / (deltaManipulation.Scale.Length / Math.Sqrt(2));//获取双指缩放比例


                //txtCanvasTop.Text = Math.Round((double.Parse(rawTop) + (double.Parse(rawTop) - double.Parse(rawBottom)) *(scale-1)), 2).ToString();
                //txtCanvasBottom.Text = Math.Round((double.Parse(rawBottom) - (double.Parse(rawTop) - double.Parse(rawBottom)) * (scale - 1)), 2).ToString();
                //txtCanvasLeft.Text = Math.Round((double.Parse(rawLeft) - (double.Parse(rawRight) - double.Parse(rawLeft)) * (scale - 1)), 2).ToString();
                //txtCanvasRight.Text = Math.Round((double.Parse(rawRight) + (double.Parse(rawRight) - double.Parse(rawLeft)) * (scale - 1)), 2).ToString();
                //if (step % 10 == 0)
                //{
                //    draw();
                //}



                if (intFingerMount == 1 && blnScaleInsteadOfMove == false)//如果只有一根手指在触摸而且一直只有一根手指
                {
                    // i++;

                    //txtCanvasLeft.Text = (dblRawLeft - (deltaManipulation.Translation.X / cvsMainCanvas.ActualWidth) * (dblRawRight - dblRawLeft)).ToString();
                    //txtCanvasRight.Text = (dblRawRight - (deltaManipulation.Translation.X / cvsMainCanvas.ActualWidth) * (dblRawRight - dblRawLeft)).ToString();
                    //txtCanvasBottom.Text = (dblRawBottom + (deltaManipulation.Translation.Y / cvsMainCanvas.ActualHeight) * (dblRawTop - dblRawBottom)).ToString();
                    //txtCanvasTop.Text = (dblRawTop + (deltaManipulation.Translation.Y / cvsMainCanvas.ActualHeight) * (dblRawTop - dblRawBottom)).ToString();
                    //Debug.WriteLine((deltaManipulation.Translation.X / cvsMainCanvas.ActualWidth));

                    cvsMainCanvas.Margin = new Thickness(
                        cvsMainCanvas.Margin.Left + (deltaManipulation.Translation.X),
                        cvsMainCanvas.Margin.Top + (deltaManipulation.Translation.Y),
                        -(cvsMainCanvas.Margin.Left + (deltaManipulation.Translation.X)),
                        -(cvsMainCanvas.Margin.Top + (deltaManipulation.Translation.Y)));
                }
                else if (intFingerMount == 2)
                {
                    blnScaleInsteadOfMove = true;
                    //Debug.WriteLine(2);
                    txtCanvasLeft.Text = (dblRawCenterX - scale * (dblRawRight - dblRawLeft) / 2).ToString();
                    txtCanvasRight.Text = (dblRawCenterX + scale * (dblRawRight - dblRawLeft) / 2).ToString();
                    txtCanvasBottom.Text = (dblRawCenterY - scale * (dblRawTop - dblRawBottom) / 2).ToString();
                    txtCanvasTop.Text = (dblRawCenterY + scale * (dblRawTop - dblRawBottom) / 2).ToString();
                    Initialize();
                }


                //Stopwatch sw = new Stopwatch();
                // sw.Start();
                //Initialize();
                // sw.Stop();
                //  sw.Reset();
                //  Debug.WriteLine(sw.ElapsedMilliseconds);
                //drawCoordinate();

            }
            catch (Exception) { }
            //center = matrix.Transform(center);
            //matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
        }
        /// <summary>
        /// 结束手势操作事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinManipulationCompletedEventHandler(object sender, ManipulationCompletedEventArgs e)
        {
            //Debug.WriteLine("end");
        }

        Point startTouchPoint;//手指按下去的时候的坐标
        /// <summary>
        /// 手指按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinTouchDownEventHandler(object sender, TouchEventArgs e)
        {
            if (intFingerMount == 0)//如果第一次按下
            {
                startTouchPoint = e.GetTouchPoint(this).Position;
                startCvWidth = cvsMainCanvas.ActualWidth;
                startCvHeight = cvsMainCanvas.ActualHeight;
            }

            intFingerMount++;
            //enableOrDisableManipulation();

        }
        /// <summary>
        /// 手指抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WinTouchUpEventHandler(object sender, TouchEventArgs e)
        {


            //  sleep(200);
            intFingerMount--;
            //enableOrDisableManipulation();

            if (intFingerMount == 0)//如果所有手指离开屏幕
            {
                if (!blnScaleInsteadOfMove)
                {
                    double realWidth = double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text);
                    double realHeight = double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text);
                    //Debug.WriteLine(e.GetTouchPoint(this).Position.X);
                    txtCanvasLeft.Text = (double.Parse(txtCanvasLeft.Text) - realWidth * (e.GetTouchPoint(this).Position.X - startTouchPoint.X) / startCvWidth).ToString();
                    txtCanvasRight.Text = (double.Parse(txtCanvasRight.Text) - realWidth * (e.GetTouchPoint(this).Position.X - startTouchPoint.X) / startCvWidth).ToString();
                    txtCanvasTop.Text = (double.Parse(txtCanvasTop.Text) + realHeight * (e.GetTouchPoint(this).Position.Y - startTouchPoint.Y) / startCvHeight).ToString();
                    txtCanvasBottom.Text = (double.Parse(txtCanvasBottom.Text) + realHeight * (e.GetTouchPoint(this).Position.Y - startTouchPoint.Y) / startCvHeight).ToString();
                    cvsMainCanvas.Margin = new Thickness(0);
                }
                Draw();
            }

        }


        #endregion
        #region 按钮操作
        /// <summary>
        /// 单击普通计算器按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenOrdinaryCalculatorBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            new OrdinaryCalculator().Show();
        }
        /// <summary>
        /// 单击解方程按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenSolveEquationsBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            new SolveEquations().Show();
        }

        private void btnCalculateTypeClickEventHandler(object sender, RoutedEventArgs e)
        {
            new PromptMessageBox(
                @"JS：利用Js库来计算，速度中等，函数覆盖较全面
master：速度快，无高级函数、无乘方
KvanTTT：速度最慢"
                ).ShowDialog();
        }

        /// <summary>
        /// 单击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAllGraphsBtnClickEventHandler(object sender, EventArgs e)
        {
            ClearAllGraphs();
        }
        /// <summary>
        /// 单击保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveGraphAsFiles(object sender, EventArgs e)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(cvsMainCanvas);
            double dpi = 96d;


            RenderTargetBitmap rtb = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, dpi, dpi, System.Windows.Media.PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(cvsMainCanvas);
                dc.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }
            rtb.Render(dv);
            BitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(rtb));
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                pngEncoder.Save(ms);
                ms.Close();

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PNG图片（透明背景色）|,*.png";
                sfd.FileName = txtFunctionInput.Text;
                if (sfd.ShowDialog() == true)
                {
                    System.IO.File.WriteAllBytes(sfd.FileName, ms.ToArray());
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.ToString());
            }
        }
        #endregion

    }


}

