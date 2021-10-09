using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using PrismDemo.Service;

namespace PrismDemo.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private readonly ITest _test;
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ICommand Command1 { get; }

        public ICommand Command2 { get; }

        public HomeViewModel(ITest test)
        {
            _test = test;
            Title = "HOME";
            Command1 = new DelegateCommand(() =>
            {
                _test?.T1();
                Debug.WriteLine("Command1");
            });

            Command2 = new DelegateCommand(async () =>
            {

                await _test.AsyncT1();
                Debug.WriteLine("Command2");
            });
        }
    }
}
