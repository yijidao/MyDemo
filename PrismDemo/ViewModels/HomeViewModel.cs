﻿using System;
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

        private long? _input;
        public long? Input
        {
            get => _input;
            set => SetProperty(ref _input, value);
        }

        public ICommand Command1 { get; }

        public ICommand Command2 { get; }

        public ICommand Command3 { get; }
        public ICommand Command4 { get; }

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

            Command3 = new DelegateCommand(async () =>
            {
                var result = await _test.AsyncT2(Input);
                Debug.WriteLine($"Command3  {result}");
            });

            Command4 = new DelegateCommand(async () =>
            {
                var result = await _test.AsyncT2(Input?.ToString());
                Debug.WriteLine($"Command4  {result}");
            });
        }
    }
}
