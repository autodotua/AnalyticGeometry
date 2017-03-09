using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticGeometry
{
    class CustomCoordinate
    {
     private double width;
     private double height;
     private double left;
     private double right;
     private double top;
     private double bottom;
        /// <summary>
        /// 自定义平面直角坐标系
        /// </summary>
        /// <param name="_width">坐标系高度</param>
        /// <param name="_height">坐标系宽度</param>
        /// <param name="_left">坐标系左端横坐标值</param>
        /// <param name="_right">坐标系右端横坐标值</param>
        /// <param name="_top">坐标系上端纵坐标值</param>
        /// <param name="_bottom">坐标系下端纵坐标值</param>
        public CustomCoordinate(double _width, double _height, double _left, double _right, double _top, double _bottom)
        {
            width = _width;
            height = _height;
            left = _left;
            right = _right;
            top = _top;
            bottom = _bottom;
        }
        /// <summary>
        /// 从自定义坐标系的横坐标转换到设计坐标系的横坐标
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double ToScreenX(double x)
        {
            return (width * (x - left) / (right - left));
        }
        /// <summary>
        /// 从自定义坐标系的纵坐标转换到设计坐标系的纵坐标
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public double ToScreenY(double y)
        {
            return (height * (y - top) / (bottom - top));
        }
        /// <summary>
        /// 取X轴的纵坐标
        /// </summary>
        /// <returns></returns>
        public int ToScreenXAxis()
        {
            return (int)ToScreenY(0);
        }
        /// <summary>
        /// 取Y轴的横坐标
        /// </summary>
        /// <returns></returns>
        public int ToScreenYAxis()
        {
            
                return (int)ToScreenX(0);
            
        }
        /// <summary>
        /// 将自定义坐标系中的宽度转换为设计坐标系的宽度
        /// </summary>
        /// <param name="realwidth"></param>
        /// <returns></returns>
        public double ToScreenWidth(double realwidth)
        {
            return realwidth * width / (right - left);
        }
        /// <summary>
        /// 将自定义坐标系中的高度转换为设计坐标系的高度
        /// </summary>
        /// <param name="realheight"></param>
        /// <returns></returns>
        public double ToScreenHeight(double realheight)
        {
            return realheight * height / (top - bottom);
        }
        /// <summary>
        /// 从设计坐标系的纵坐标转换到自定义坐标系的纵坐标
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double ToRealX(double x)
        {
            return x * (right - left) / width + left;
        }
        /// <summary>
        /// 从设计坐标系的横坐标转换到自定义坐标系的横坐标
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        public double ToRealY(double y)
        {
            return y * (bottom - top) / height + top;
        }
    }
}
