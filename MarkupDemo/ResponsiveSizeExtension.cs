using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace MarkupDemo
{
    public class ResponsiveSizeExtension : MarkupExtension
    {
        private static readonly Lazy<DoubleConverter> _lazyDouble = new();
        private static readonly Lazy<ThicknessConverter> _lazyThickness = new();
        private static readonly Lazy<CornerRadiusConverter> _lazyCornerRadius = new();


        private DoubleConverter _doubleConverter => _lazyDouble.Value;
        private ThicknessConverter _thickConvert => _lazyThickness.Value;
        private CornerRadiusConverter _cornerRadiusConvert => _lazyCornerRadius.Value;

        [ConstructorArgument("value")]
        public object Value { get; set; }

        public ResponsiveSizeExtension(object value)
        {
            if (value is string s && string.IsNullOrWhiteSpace(s)) value = "0";
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Type type;

            var targetObject = ((IProvideValueTarget)serviceProvider).TargetObject;
            var targetProperty = ((IProvideValueTarget)serviceProvider).TargetProperty;

            if (targetObject is Setter setter)
            {
                type = setter.Property.PropertyType;
            }
            else if (targetProperty is DependencyProperty dp)
            {
                type = dp.PropertyType;
            }
            else
            {
                throw new NotSupportedException($"不是 setter 对象或者依赖属性");
            }
            TypeConverter converter;
            if (type == typeof(double))
            {
                converter = _doubleConverter;
            }

            else if (type == typeof(Thickness))
            {
                converter = _thickConvert;
            }

            else if (type == typeof(CornerRadius))
            {
                converter = _cornerRadiusConvert;
            }
            else
            {
                throw new NotSupportedException($"{type} 类型不支持");
            }


            var result = converter.ConvertFrom(Value) ?? throw new ArgumentException(nameof(Value));
            return Application.Current.ConvertForScreen(result);
        }
    }
}
