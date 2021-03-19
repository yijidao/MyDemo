using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    public class LineViewModel : ReactiveObject
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private int _no;
        public int No
        {
            get => _no;
            set => this.RaiseAndSetIfChanged(ref _no, value);
        }

        public LineViewModel()
        {
            ChangeCommand = ReactiveCommand.Create(() =>
            {
                Name = $"{Name}_{++No}";
            });
        }

        public LineViewModel(string name) : this()
        {
            Name = name;
        }

        public ReactiveCommand<Unit, Unit> ChangeCommand { get; }
    }
}
