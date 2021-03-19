using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    public class Demo1ViewModel : ReactiveObject
    {

        private ReadOnlyObservableCollection<LineViewModel> _lines;
        public ReadOnlyObservableCollection<LineViewModel> Lines
        {
            get => _lines;
            set => this.RaiseAndSetIfChanged(ref _lines, value);
        }

        private LineViewModel _selectedLine;
        public LineViewModel SelectedLine
        {
            get => _selectedLine;
            set => this.RaiseAndSetIfChanged(ref _selectedLine, value);
        }

        private SourceList<LineViewModel> _lineSource;
        public SourceList<LineViewModel> LineSource
        {
            get => _lineSource;
            set => this.RaiseAndSetIfChanged(ref _lineSource, value);
        }

        private SourceList<StationViewModel> _stationSource;
        public SourceList<StationViewModel> StationSource
        {
            get => _stationSource;
            set => this.RaiseAndSetIfChanged(ref _stationSource, value);
        }

        private ReadOnlyObservableCollection<StationViewModel> _stations;
        public ReadOnlyObservableCollection<StationViewModel> Stations
        {
            get => _stations;
            set => this.RaiseAndSetIfChanged(ref _stations, value);
        }

        public int Count { get; set; }

        public Demo1ViewModel()
        {
            AddCommand = ReactiveCommand.Create(() =>
            {
                LineSource.Add(new LineViewModel($"线路{++Count}") { No = Count });
            });
            _lineSource = new SourceList<LineViewModel>();
            var share = _lineSource.Connect().Publish();
            share.ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _lines)
                .WhenAnyPropertyChanged(nameof(LineViewModel.No))
                //.WhenPropertyChanged(x => x.Name)
                .Subscribe(x =>
                {
                    
                    //var set = x as ChangeSet<LineViewModel>;
                    //if (set == null) return;
                    //foreach (var change in set)
                    //{

                    //}
                });
            share.Connect();
            share.ActOnEveryObject(Add, Remove);

            share.Connect();
            share.WhenAnyPropertyChanged(nameof(LineViewModel.ChangeCommand))
                .Subscribe(change =>
                {

                });

            

            ExecuteCommand = ReactiveCommand.Create(() => Debug.WriteLine(nameof(ExecuteCommand)), LineSource.CountChanged.Select(x => x > 0));

            RemoveCommand = ReactiveCommand.Create(() =>
            {
                var index = LineSource.Count - 1;
                if (index >= 0)
                {
                    LineSource.RemoveAt(index);
                }
            });

            ChangeCommand = ReactiveCommand.Create(() =>
            {

                if (SelectedLine == null) return;
                SelectedLine.Name = $"线路{++Count}";
            });

            ChangeNoCommand = ReactiveCommand.Create(() =>
            {
                if (SelectedLine == null) return;
                SelectedLine.No = ++Count;
            });

            InsertCommand = ReactiveCommand.Create(() =>
            {
                LineSource.Insert(LineSource.Count - 2, new LineViewModel($"线路{++Count}") { No = Count });
            });

            _stationSource = new SourceList<StationViewModel>();
            _stationSource.Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _stations)
                .Subscribe(x =>
                {
                    
                });
        }

        private void Remove(LineViewModel obj)
        {

        }

        private void Add(LineViewModel obj)
        {

        }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public ReactiveCommand<Unit, Unit> ExecuteCommand { get; }

        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        public ReactiveCommand<Unit, Unit> ChangeCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeNoCommand { get; }

        public ReactiveCommand<Unit, Unit> InsertCommand { get; }
    }
}
