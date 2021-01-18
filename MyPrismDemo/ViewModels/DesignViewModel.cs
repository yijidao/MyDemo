using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MyPrismDemo.Views;
using Prism.Mvvm;

namespace MyPrismDemo.ViewModels
{
    public class DesignViewModel : BindableBase
    {
        private ObservableCollection<ToolBoxItemViewModel> _tbivms = new ObservableCollection<ToolBoxItemViewModel>();
        public ObservableCollection<ToolBoxItemViewModel> ToolBoxItems
        {
            get => _tbivms;
            set => SetProperty(ref _tbivms, value);
        }

        public DesignViewModel()
        {
            ToolBoxItems.Add(new ToolBoxItemViewModel(typeof(BusinessView)));
        }
    }

    public class ToolBoxItemViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Type _type;
        public Type ItemType
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public ToolBoxItemViewModel(Type itemType)
        {
            var n = itemType.GetAttributeValue((DisplayNameAttribute dna) => dna.DisplayName);
            _name = n;
            _type = itemType;
        }
    }

    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }

            return default(TValue);
        }
    }
}
