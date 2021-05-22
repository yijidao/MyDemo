using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using ReactiveUI;

namespace RxUIDemo2.ViewModels
{
    public class MainViewModel2 : ReactiveObject
    {
        public ReactiveCommand<string, Unit> ClickCommand { get; }
        public ReactiveCommand<Unit, Unit> SwitchCommand { get; }

        public MainViewModel2()
        {
            ClickCommand = ReactiveCommand.Create<string>(value =>
            {
                Debug.WriteLine(value);
            });
            SwitchCommand = ReactiveCommand.Create(RxSwitch);
        }

        private void RxSwitch()
        {
            //Observable.Generate();
        }


        private void Log()
        {
            Debug.WriteLine("click");
        }
    }
}
