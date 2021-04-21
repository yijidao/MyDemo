using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows.Input;
using ReactiveUI;

namespace RxUIDemo2.ViewModels
{
    public class RecommendViewModel : ReactiveObject
    {
        private string _username;
        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        public RecommendViewModel()
        {
            CloseCommand = ReactiveCommand.Create(() => { });
        }

    }
}
