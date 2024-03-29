﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    public class PollMonitorItemViewModel : ReactiveObject
    {

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set => this.RaiseAndSetIfChanged(ref _selected, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public PollMonitorItemViewModel()
        {
            ChangeCommand = ReactiveCommand.Create(ChangeSelected);
        }

        private void ChangeSelected() => Selected = !Selected;

        public ReactiveCommand<Unit, Unit> ChangeCommand { get; }

        

    }
}
