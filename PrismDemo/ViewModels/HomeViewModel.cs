using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace PrismDemo.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ICommand Command1 { get; }

        public HomeViewModel()
        {
            Title = "HOME";
            Command1 = new DelegateCommand(() =>
            {
                Debug.WriteLine("Command1");
            });
        }
    }
}
