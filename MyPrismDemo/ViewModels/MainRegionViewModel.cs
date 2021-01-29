using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace MyPrismDemo.ViewModels
{
    public class MainRegionViewModel : BindableBase
    {
        private string _title = "MainRegion";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}
