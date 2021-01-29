using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Mvvm;

namespace MyPrismDemo.ViewModels
{
    public class DetailRegionViewModel :  BindableBase
    {
        private string _title = "DetailRegion";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
