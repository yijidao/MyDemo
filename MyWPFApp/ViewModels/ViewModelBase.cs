using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace MyWPFApp.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        private Visibility _maskVisibility;
        public Visibility MaskVisibility
        {
            get => _maskVisibility;
            set => SetProperty(ref _maskVisibility, value);
        }


    }
}
