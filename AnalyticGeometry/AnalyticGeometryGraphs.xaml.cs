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

namespace AnalyticGeometry
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AnalyticGeometryGraphs : Window
    {
        string functionExpression, ParametricXExpression, ParametricYExpression;
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

        private void Initialize()
        {
            cc = new CustomCoordinate(grdMain.ColumnDefinitions[1].ActualWidth, grdMain.ActualHeight, double.Parse(txtCanvasLeft.Text), double.Parse(txtCanvasRight.Text), double.Parse(txtCanvasTop.Text), double.Parse(txtCanvasBottom.Text));
            RefreshCoordinateGrid();
            //if (chkMultipleFunction.IsChecked == true)
            //{
            ClearAllGraphs();
            //}
            DrawCoordinate();
        }

        private void ClearAllGraphs()
        {
            cvsMainCanvas.Children.Clear();
        }


        private void ShowErrorMessage(string text)
        {
            new ErrorMessageBox(text).ShowDialog();
        }



        #region 绘制

        private void DrawCoordinate()
        {
            //double  pointWidth=
            DrawLine(Brushes.Black, 1.5, -cvsMainCanvas.ActualWidth, cc.ToScreenXAxis(), 2 * cvsMainCanvas.ActualWidth, cc.ToScreenXAxis());//X
            DrawLine(Brushes.Black, 1.5, cc.ToScreenYAxis(), -cvsMainCanvas.ActualHeight, cc.ToScreenYAxis(), 2 * cvsMainCanvas.ActualHeight);//Y
            List<TextBlock> tbList = new System.Collections.Generic.List<TextBlock>();
            if (chkShowGrid.IsChecked == true)
            {
                //左边界横坐标
                double leftLine =
                    Math.Round(cc.ToRealX(-cvsMainCanvas.ActualWidth)
                    / double.Parse(txtVerticalSeparationDistance.Text))
                    * double.Parse(txtVerticalSeparationDistance.Text);
                //
                double rightLine =
                    Math.Round(cc.ToRealX(2 * cvsMainCanvas.ActualWidth)
                    / double.Parse(txtVerticalSeparationDistance.Text))
                    * double.Parse(txtVerticalSeparationDistance.Text);
                //Y轴右半部份的
                for (double i = leftLine; i <= rightLine; i += double.Parse(txtVerticalSeparationDistance.Text))
                {
                    if (Math.Round(i, 5) != 0)
                    {
                        DrawLine(Brushes.Gray, 1, cc.ToScreenX(i), -cvsMainCanvas.ActualHeight, cc.ToScreenX(i), 2 * cvsMainCanvas.ActualHeight);
                        tbList.Add(new TextBlock());
                        tbList[tbList.Count - 1].Text = Math.Round(i, 5).ToString();
                        tbList[tbList.Count - 1].Margin = new Thickness(cc.ToScreenX(i), cc.ToScreenXAxis() - 15, 0, 0);
                        cvsMainCanvas.Children.Add(tbList[tbList.Count - 1]);
                    }
                }

                double topLine = Math.Round(cc.ToRealY(-cvsMainCanvas.ActualHeight) / double.Parse(txtHorizontalSeparationDistance.Text)) * double.Parse(txtHorizontalSeparationDistance.Text);
                double bottomLine = Math.Round(cc.ToRealY(2 * cvsMainCanvas.ActualHeight) / double.Parse(txtHorizontalSeparationDistance.Text)) * double.Parse(txtHorizontalSeparationDistance.Text);

                for (double i = bottomLine; i <= topLine; i += double.Parse(txtHorizontalSeparationDistance.Text))
                {
                    if (Math.Round(i, 5) != 0)
                    {
                        DrawLine(Brushes.Gray, 1, -cvsMainCanvas.ActualWidth, cc.ToScreenY(i), 2 * cvsMainCanvas.ActualWidth, cc.ToScreenY(i));
                        tbList.Add(new TextBlock());
                        tbList[tbList.Count - 1].Text = Math.Round(i, 5).ToString();
                        tbList[tbList.Count - 1].Margin = new Thickness(cc.ToScreenYAxis() + 6, cc.ToScreenY(i), 0, 0);
                        cvsMainCanvas.Children.Add(tbList[tbList.Count - 1]);
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

        private void Draw()
        {
            if (tabInput.SelectedIndex == 0)
            {
                btnFunctionOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
            else
            {
                btnParametricOK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            System.Windows.Shapes.Line l = new Line();
            l.Stroke = colorPicker.CurrentColor;
            l.StrokeThickness = double.Parse(txtLineThickness.Text);
            l.X1 = x1;
            l.X2 = x2;
            l.Y1 = y1;
            l.Y2 = y2;
            cvsMainCanvas.Children.Add(l);

        }
        private void DrawLine(Point p1, Point p2)
        {
            System.Windows.Shapes.Line l = new Line();
            l.Stroke = colorPicker.CurrentColor;
            l.StrokeThickness = double.Parse(txtLineThickness.Text);
            l.X1 = p1.X;
            l.X2 = p2.X;
            l.Y1 = p1.Y;
            l.Y2 = p2.Y;
            cvsMainCanvas.Children.Add(l);
        }
        private void DrawLine(SolidColorBrush brush, double tn, double x1, double y1, double x2, double y2)
        {
            System.Windows.Shapes.Line l = new Line();
            l.Stroke = brush;
            l.StrokeThickness = tn;
            l.X1 = x1;
            l.X2 = x2;
            l.Y1 = y1;
            l.Y2 = y2;
            cvsMainCanvas.Children.Add(l);
        }
        private void DrawCricle(double x, double y)
        {
            System.Windows.Shapes.Ellipse e = new Ellipse();
            e.Stroke = colorPicker.CurrentColor;
            e.StrokeThickness = double.Parse(txtLineThickness.Text);
            e.Margin = new Thickness(x - double.Parse(txtLineThickness.Text) / 2, y - double.Parse(txtLineThickness.Text) / 2, 2 * double.Parse(txtLineThickness.Text), 2 * double.Parse(txtLineThickness.Text));
            cvsMainCanvas.Children.Add(e);
        }
        #endregion

        #region 函数
        private void FunctionOKBtnClickEventHandler(object sender, EventArgs e)
        {

            colorPicker.BorderBrush = Brushes.Red;
            Initialize();

            try
            {
                //  if(chkMultipleFunction.IsChecked==true)
                // {
                foreach (var i in txtFunctionInput.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    functionExpression = Calculate.ReplaceExpressionPreliminary(i, txtFunctionVariable.Text);
                    DrawFunctionGraph();
                }
                //  }
            }
            catch
            {
            }
        }

        private void DrawFunctionGraph()
        {
            double pre = (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / cvsMainCanvas.ActualWidth * double.Parse(txtFunctionPrecision.Text);
            List<double> xarray = new List<double>();

            List<Point> points = new List<Point>();
            double leftLine = Math.Round(cc.ToRealX(-cvsMainCanvas.ActualWidth) / double.Parse(txtVerticalSeparationDistance.Text)) * double.Parse(txtVerticalSeparationDistance.Text);
            double rightLine = Math.Round(cc.ToRealX(2 * cvsMainCanvas.ActualWidth) / double.Parse(txtVerticalSeparationDistance.Text)) * double.Parse(txtVerticalSeparationDistance.Text);
            for (double i = leftLine; i <= rightLine; i += pre)
            {
                points.Add(new Point(
                    cc.ToScreenX(i),
                    cc.ToScreenY(double.Parse(Calculate.Eval(functionExpression, txtFunctionVariable.Text, i.ToString())))));
            }
            if (rbtnGraphTypeOfLine.IsChecked == true)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    if (points[i - 1].Y >= -100 && points[i].Y <= cvsMainCanvas.ActualHeight + 100 && points[i - 1].Y <= cvsMainCanvas.ActualHeight + 100 && points[i].Y >= -100)
                    {
                        DrawLine(points[i - 1].X, points[i - 1].Y, points[i].X, points[i].Y);
                    }

                    else if (points[i - 1].Y < cvsMainCanvas.ActualHeight && points[i - 1].Y > 0 && points[i].Y <= 0)
                    {
                        DrawLine(points[i - 1].X, points[i - 1].Y, points[i - 1].X, 0);
                    }
                    else if (points[i - 1].Y <= 0 && points[i].Y > 0 && points[i].Y < cvsMainCanvas.ActualHeight)
                    {
                        DrawLine(points[i].X, 0, points[i].X, points[i].Y);
                    }

                    else if (points[i - 1].Y < cvsMainCanvas.ActualHeight && points[i - 1].Y > 0 && points[i].Y > cvsMainCanvas.ActualHeight)
                    {
                        DrawLine(points[i - 1].X, points[i - 1].Y, points[i - 1].X, cvsMainCanvas.Height);
                    }
                    else if (points[i - 1].Y > cvsMainCanvas.ActualHeight && points[i].Y > 0 && points[i].Y < cvsMainCanvas.ActualHeight)
                    {
                        DrawLine(points[i].X, cvsMainCanvas.Height, points[i].X, points[i].Y);
                    }

                }
            }


            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    try
                    {
                        DrawCricle(points[i].X, points[i].Y);
                    }
                    catch (Exception)
                    {
                    }
                }
            }


        }
        #endregion

        #region 参数方程
        private void ParametricOKBtnClickEventHandler(object sender, EventArgs e)
        {
            Initialize();
            ParametricXExpression = Calculate.ReplaceExpressionPreliminary(txtParametricX.Text, txtParametricParameter.Text);
            ParametricYExpression = Calculate.ReplaceExpressionPreliminary(txtParametricY.Text, txtParametricParameter.Text);
            try
            {
                DrawParametricGraph();
            }
            catch { }

        }

        private void DrawParametricGraph()
        {
            Pen p = new Pen(colorPicker.CurrentColor, int.Parse(txtLineThickness.Text));
            List<Point> points = new List<Point>();
            for (double i = double.Parse(txtParametricEnd.Text); i <= double.Parse(txtParametricEnd.Text); i += double.Parse(txtParametricPrecision.Text))
            {
                points.Add(new Point(
                    cc.ToScreenX(double.Parse(Calculate.Eval(ParametricXExpression, txtParametricParameter.Text, i.ToString()))),
                    cc.ToScreenY(double.Parse(Calculate.Eval(ParametricYExpression, txtParametricParameter.Text, i.ToString())))));
            }
            if (rbtnGraphTypeOfLine.IsChecked == true)
            {
                for (int i = 1; i < points.Count; i++)
                {
                    if (points[i - 1].Y >= -100 && points[i].Y <= cvsMainCanvas.ActualHeight + 100 && points[i - 1].Y <= cvsMainCanvas.ActualHeight + 100 && points[i].Y >= -100)
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
        private void FunctionOKTxtPreviewKeyDownEventHandler(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
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

        private void WinLoadedEventHandler(object sender, EventArgs e)
        {
            string tempFileName = System.IO.Path.GetTempFileName();
            using (FileStream fs = new FileStream(tempFileName, FileMode.Create))
            {
                Properties.Resources.icon.Save(fs);
            }
            this.Icon = new BitmapImage(new Uri(tempFileName));
            txtFunctionInput.Focus();
            ps = PresentationSource.FromVisual(this);
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
                if (GetConfig("rbtnGraphTypeOfLine") == "0")
                {
                    rbtnGraphTypeOfPoint.IsChecked = true;
                    rbtnGraphTypeOfLine.IsChecked = false;
                }
            }
            Draw();
        }

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

        private void DelRegeditItems(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser;
            try
            {
                rk.DeleteSubKey("software\\AutodotuaSoftware\\Analytic-Geometry");
                string strAppFileName = Process.GetCurrentProcess().MainModule.FileName;
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

        private void WinMouseWheelEventHandler(object sender, MouseWheelEventArgs e)
        {
            //如果是在画布部分滚轮
            if (Mouse.GetPosition(this as FrameworkElement).X > 250)
            {

                if (e.Delta > 0)
                {
                    string temp1 = txtCanvasTop.Text;
                    string temp2 = txtCanvasLeft.Text;
                    txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) - (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
                    txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) + (double.Parse(temp1) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
                    txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
                    txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(temp2)) / 5), 2).ToString();


                }
                else
                {
                    string temp1 = txtCanvasTop.Text;
                    string temp2 = txtCanvasLeft.Text;
                    txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) + (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
                    txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) - (double.Parse(temp1) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
                    txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
                    txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(temp2)) / 5), 2).ToString();

                }

                Draw();
            }
           //如果在操作区滚轮
            else
            {
                //如果界面高度足够就不需要滚轮来调节了了
                if (this.ActualHeight >= 720)
                    return;
                else
                {
                    if (e.Delta > 0)
                    {
                      //  Debug.WriteLine();
                        if (grdSettings.Margin.Top<4)
                        {
                            grdSettings.Margin = new Thickness(
                                 grdSettings.Margin.Left,
                                  grdSettings.Margin.Top + 8,
                                   grdSettings.Margin.Right,
                                    grdSettings.Margin.Bottom + 8);
                        }
                    }
                    else
                    {
                        //Debug.WriteLine("2");
                        if(this.Height - grdSettings.Margin.Top > 710)
                        {
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

        private void RefreshCoordinateGrid()
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

        private void MakeGridSquare(object sender, EventArgs e)
        {
            double proportion;
            double middle;
            middle = 0.5 * (double.Parse(txtCanvasTop.Text) + double.Parse(txtCanvasBottom.Text));
            proportion = (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / cvsMainCanvas.ActualWidth;
            txtCanvasBottom.Text = (middle - 0.5 * proportion * cvsMainCanvas.ActualHeight).ToString();
            txtCanvasTop.Text = (middle + 0.5 * proportion * cvsMainCanvas.ActualHeight).ToString();
            RefreshCoordinateGrid();
            ClearAllGraphs();
            Initialize();
            Draw();
        }

        private void WinPreviewMouseDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.GetPosition(cvsMainCanvas).X > 0 && e.GetPosition(cvsMainCanvas).X < cvsMainCanvas.ActualWidth && e.GetPosition(cvsMainCanvas).Y > 0 && e.GetPosition(cvsMainCanvas).Y < cvsMainCanvas.ActualHeight)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    IsMouseDown = true;
                    mousePoint = e.GetPosition(this);
                    startMousePoint = mousePoint;
                    startCvWidth = cvsMainCanvas.ActualWidth;
                    startCvHeight = cvsMainCanvas.ActualHeight;
                }
            }
        }

        private void WinMouseMoveEventHandler(object sender, MouseEventArgs e)
        {

            if (IsMouseDown)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {

                    RECT rect = new RECT(
                        (int)((this.Left + grdMain.ColumnDefinitions[0].ActualWidth) * ps.CompositionTarget.TransformToDevice.M11) + 1,
                        (int)(this.Top * ps.CompositionTarget.TransformToDevice.M11) + 1,
                        (int)((this.Left + this.ActualWidth) * ps.CompositionTarget.TransformToDevice.M11) - 1,
                        (int)((this.ActualHeight + this.Top) * ps.CompositionTarget.TransformToDevice.M11) - 1
                        );
                    ClipCursor(ref rect);



                    Point theMousePoint = e.GetPosition(this);
                    // if (theMousePoint.X > cv.Margin.Left && theMousePoint.X < cv.Margin.Left + cv.ActualWidth && theMousePoint.Y > cv.Margin.Top && theMousePoint.Y < cv.Margin.Top+cv.ActualWidth)
                    //   {
                    //if (theMousePoint.X > 242)
                    //{
                    cvsMainCanvas.Margin = new Thickness(cvsMainCanvas.Margin.Left - (mousePoint.X - theMousePoint.X), cvsMainCanvas.Margin.Top - (mousePoint.Y - theMousePoint.Y), -(cvsMainCanvas.Margin.Left - (mousePoint.X - theMousePoint.X)), -(cvsMainCanvas.Margin.Top - (mousePoint.Y - theMousePoint.Y)));
                    mousePoint = theMousePoint;
                    //}
                    //   }
                }
            }
        }


        private void WinMouseUpEventHandler(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseDown)
            {

                IsMouseDown = false;

                double realWidth = double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text);
                double realHeight = double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text);

                txtCanvasLeft.Text = (double.Parse(txtCanvasLeft.Text) - realWidth * (e.GetPosition(this).X - startMousePoint.X) / startCvWidth).ToString();
                txtCanvasRight.Text = (double.Parse(txtCanvasRight.Text) - realWidth * (e.GetPosition(this).X - startMousePoint.X) / startCvWidth).ToString();
                txtCanvasTop.Text = (double.Parse(txtCanvasTop.Text) + realHeight * (e.GetPosition(this).Y - startMousePoint.Y) / startCvHeight).ToString();
                txtCanvasBottom.Text = (double.Parse(txtCanvasBottom.Text) + realHeight * (e.GetPosition(this).Y - startMousePoint.Y) / startCvHeight).ToString();

                cvsMainCanvas.Margin = new Thickness(0);
                RECT rect = new RECT(0, 0, (int)(SystemParameters.PrimaryScreenWidth * ps.CompositionTarget.TransformToDevice.M11), (int)(SystemParameters.PrimaryScreenHeight * ps.CompositionTarget.TransformToDevice.M11));
                ClipCursor(ref rect);


                Draw();

            }
        }

        [DllImport("user32.dll")]
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



        private void WinSizeChangedEventHandler(object sender, SizeChangedEventArgs e)
        {
            //Debug.WriteLine(this.Height);
            //Debug.WriteLine(grdSettings.Margin.Top);

            //如果发现最底端已经到底了，那么要把Margin往下调，也就是底部吸住，直到顶部完全露出来为止。
           while(this.Height - grdSettings.Margin.Top > 730&& grdSettings.Margin.Top<4)
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

        private void ZoomInHeightBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) - (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) + (double.Parse(temp1) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            Draw();
        }

        private void ZoomOutHeightBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasTop.Text = Math.Round((double.Parse(txtCanvasTop.Text) + (double.Parse(txtCanvasTop.Text) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            txtCanvasBottom.Text = Math.Round((double.Parse(txtCanvasBottom.Text) - (double.Parse(temp1) - double.Parse(txtCanvasBottom.Text)) / 5), 2).ToString();
            Draw();
        }

        private void ZoomInWidthBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
            txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(temp2)) / 5), 2).ToString();
            Draw();
        }

        private void ZoomOutWidthBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            string temp1 = txtCanvasTop.Text;
            string temp2 = txtCanvasLeft.Text;
            txtCanvasLeft.Text = Math.Round((double.Parse(txtCanvasLeft.Text) - (double.Parse(txtCanvasRight.Text) - double.Parse(txtCanvasLeft.Text)) / 5), 2).ToString();
            txtCanvasRight.Text = Math.Round((double.Parse(txtCanvasRight.Text) + (double.Parse(txtCanvasRight.Text) - double.Parse(temp2)) / 5), 2).ToString();
            Draw();
        }


        #endregion
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
        bool blnScaleInsteadOfMove = false;
        // int step = 0;
        private void WinManipulationDeltaEventHandler(object sender, ManipulationDeltaEventArgs e)
        {
            //step++;
            // if (e.DeltaManipulation==ManipulationDelta.)
            FrameworkElement element = (FrameworkElement)e.Source;
            // if(step%10==0)
            try
            {
                Matrix matrix = ((MatrixTransform)element.RenderTransform).Matrix;
                var deltaManipulation = e.CumulativeManipulation;

                Point center = new Point(element.ActualWidth / 2, element.ActualHeight / 2);
                // Debug.WriteLine(deltaManipulation.Translation.Length);
                double scale = 1 / (deltaManipulation.Scale.Length / Math.Sqrt(2));
                // Debug.WriteLine(scale);
                //cvsMainCanvas.Height = cvsMainCanvas.Height * scale;

                //txtCanvasTop.Text = Math.Round((double.Parse(rawTop) + (double.Parse(rawTop) - double.Parse(rawBottom)) *(scale-1)), 2).ToString();
                //txtCanvasBottom.Text = Math.Round((double.Parse(rawBottom) - (double.Parse(rawTop) - double.Parse(rawBottom)) * (scale - 1)), 2).ToString();
                //txtCanvasLeft.Text = Math.Round((double.Parse(rawLeft) - (double.Parse(rawRight) - double.Parse(rawLeft)) * (scale - 1)), 2).ToString();
                //txtCanvasRight.Text = Math.Round((double.Parse(rawRight) + (double.Parse(rawRight) - double.Parse(rawLeft)) * (scale - 1)), 2).ToString();
                //if (step % 10 == 0)
                //{
                //    draw();
                //}



                if (intFingerMount == 1 && blnScaleInsteadOfMove == false)
                {

                    //  RECT rect = new RECT(
                    //      (int)((this.Left + grdMain.ColumnDefinitions[0].ActualWidth) * ps.CompositionTarget.TransformToDevice.M11) + 1,
                    //      (int)(this.Top * ps.CompositionTarget.TransformToDevice.M11) + 1,
                    //      (int)((this.Left + this.ActualWidth) * ps.CompositionTarget.TransformToDevice.M11) - 1,
                    //      (int)((this.ActualHeight + this.Top) * ps.CompositionTarget.TransformToDevice.M11) - 1
                    //      );
                    //  ClipCursor(ref rect);
                    //  Point theMousePoint = new Point(deltaManipulation.Translation.X, deltaManipulation.Translation.Y);
                    //  // if (theMousePoint.X > cv.Margin.Left && theMousePoint.X < cv.Margin.Left + cv.ActualWidth && theMousePoint.Y > cv.Margin.Top && theMousePoint.Y < cv.Margin.Top+cv.ActualWidth)
                    //  //   {
                    //  //if (theMousePoint.X > 242)
                    //  // {
                    //  // Debug.WriteLine(deltaManipulation.Translation.X);
                    ////  Debug.WriteLine(DateTime.Now.Millisecond);
                    //      cvsMainCanvas.Margin = new Thickness(cvsMainCanvas.Margin.Left - (mousePoint.X - theMousePoint.X), 
                    //          cvsMainCanvas.Margin.Top - (mousePoint.Y - theMousePoint.Y), 
                    //          -(cvsMainCanvas.Margin.Left - (mousePoint.X - theMousePoint.X)),
                    //          -(cvsMainCanvas.Margin.Top - (mousePoint.Y - theMousePoint.Y)));
                    //      mousePoint = theMousePoint;

                    //} 
                    // Debug.WriteLine(1);
                    txtCanvasLeft.Text = (dblRawLeft - (deltaManipulation.Translation.X / cvsMainCanvas.ActualWidth) * (dblRawRight - dblRawLeft)).ToString();
                    txtCanvasRight.Text = (dblRawRight - (deltaManipulation.Translation.X / cvsMainCanvas.ActualWidth) * (dblRawRight - dblRawLeft)).ToString();

                    txtCanvasBottom.Text = (dblRawBottom + (deltaManipulation.Translation.Y / cvsMainCanvas.ActualHeight) * (dblRawTop - dblRawBottom)).ToString();
                    txtCanvasTop.Text = (dblRawTop + (deltaManipulation.Translation.Y / cvsMainCanvas.ActualHeight) * (dblRawTop - dblRawBottom)).ToString();
                }
                else if (intFingerMount == 2)
                {
                    blnScaleInsteadOfMove = true;
                    //Debug.WriteLine(2);
                    txtCanvasLeft.Text = (dblRawCenterX - scale * (dblRawRight - dblRawLeft) / 2).ToString();
                    txtCanvasRight.Text = (dblRawCenterX + scale * (dblRawRight - dblRawLeft) / 2).ToString();

                    txtCanvasBottom.Text = (dblRawCenterY - scale * (dblRawTop - dblRawBottom) / 2).ToString();
                    txtCanvasTop.Text = (dblRawCenterY + scale * (dblRawTop - dblRawBottom) / 2).ToString();
                }


                //Stopwatch sw = new Stopwatch();
                // sw.Start();
                Initialize();
                // sw.Stop();
                //  sw.Reset();
                //  Debug.WriteLine(sw.ElapsedMilliseconds);
                //drawCoordinate();

            }
            catch (Exception) { }
            //center = matrix.Transform(center);
            //matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
        }

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
            Debug.WriteLine("start")
 ; blnScaleInsteadOfMove = false;
        }

        private void WinManipulationCompletedEventHandler(object sender, ManipulationCompletedEventArgs e)
        {
            Draw();
        }

        private void WinTouchUpEventHandler(object sender, TouchEventArgs e)
        {
            //  sleep(200);
            intFingerMount--;
            //enableOrDisableManipulation();

        }

        private void WinTouchDownEventHandler(object sender, TouchEventArgs e)
        {
            intFingerMount++;
            //enableOrDisableManipulation();

        }
        #endregion

        private void OpenOrdinaryCalculatorBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            new OrdinaryCalculator().Show();
        }
        private void OpenSolveEquationsBtnClickEventHandler(object sender, RoutedEventArgs e)
        {
            new SolveEquations().Show();
        }
        private void ClearAllGraphs(object sender, EventArgs e)
        {
            ClearAllGraphs();
        }
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
                sfd.FileName = DateTime.Now.ToString();
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


    }


}

