using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using WpfScreenHelper;

namespace MarkupDemo
{
    public static class AppExtension
    {
        public static Thickness ConvertThicknessForScreen(this Application app, Thickness value)
        {
            var factor = app.GetFactor();
            return new Thickness(value.Left / factor, value.Top / factor, value.Right / factor, value.Bottom / factor);
        }

        public static double ConvertDoubleForScreen(this Application app, double value)
        {
            var factor = app.GetFactor();
            return value / factor;
        }

        public static CornerRadius ConvertCornerRadiusForScreen(this Application app, CornerRadius value)
        {
            var factor = app.GetFactor();
            return new CornerRadius(value.TopLeft / factor, value.TopRight / factor, value.BottomRight / factor, value.BottomLeft / factor);
        }

        public static object ConvertForScreen(this Application app, object o)
        {
            if (o is double d)
            {
                return app.ConvertDoubleForScreen(d);
            }
            else if (o is Thickness t)
            {
                return app.ConvertThicknessForScreen(t);
            }
            else if (o is CornerRadius r)
            {
                return app.ConvertCornerRadiusForScreen(r);
            }
            else
            {
                throw new NotSupportedException("不支持的转换类型");
            }
        }

        /// <summary>
        /// 获取当前应用的缩放系数
        /// 目前 4K 需要放大 2 倍
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static double GetFactor(this Application app)
        {
            var factor = app.MainWindow.GetScreen().ScaleFactor;

            var bounds = app.MainWindow.GetScreen().PixelBounds;
            if (bounds.Width == 3840 && bounds.Height == 2160)
            {
                factor /= 2;
            }

            return factor;
        }
    }
}
