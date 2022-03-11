using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace MarkupDemo
{
    public class ResponsiveThicknessExtension : MarkupExtension
    {
        private static readonly Lazy<ThicknessConverter> _lazy = new();

        private ThicknessConverter _converter => _lazy.Value;

        [ConstructorArgument("value")]
        public object Value { get; set; }

        public ResponsiveThicknessExtension(object value)
        {
            if (value is string s && string.IsNullOrWhiteSpace(s)) value = "0";
            Value = value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            //((System.Windows.Controls.Border)((System.Windows.Markup.IProvideValueTarget)serviceProvider).TargetObject).BorderThickness
            var t = (Thickness)(_converter.ConvertFrom(Value) ?? new ArgumentNullException(nameof(Value)));

            t = Application.Current.ConvertForScreen(t);
            return t;
        }
    }
}
