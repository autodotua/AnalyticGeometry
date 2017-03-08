using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解析几何
{
    class CustomCoordinate
    {
        double width;
        double height;
        double left;
        double right;
        double top;
        double bottom;
        public CustomCoordinate(double _width, double _height, double _left, double _right, double _top, double _bottom)
        {
            width = _width;
            height = _height;
            left = _left;
            right = _right;
            top = _top;
            bottom = _bottom;
        }

        public double toScreenX(double x)
        {
            return (width * (x - left) / (right - left));
        }

        public double toScreenY(double y)
        {
            return (height * (y - top) / (bottom - top));
        }

        public int toScreenXAxis()
        {
            return (int)toScreenY(0);
        }

        public int toScreenYAxis()
        {
            return (int)toScreenX(0);
        }
        public double toScreenWidth(double realwidth)
        {
            return realwidth * width / (right - left);
        }
        public double toScreenHeight(double realheight)
        {
            return realheight * height / (top - bottom);
        }
        public double toRealX(double x)
        {
            return x * (right - left) / width + left;
        }
        public double toRealY(double y)
        {
            return y * (bottom - top) / height + top;
        }
    }
}
