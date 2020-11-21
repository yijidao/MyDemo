using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class LineDetailViewModel : ReactiveObject
    {
        public string Name { get; }
        public int Order { get; }
        //public ObservableCollection<StationDetailViewModel> Stations { get; }
        private ObservableCollection<StationDetailViewModel> _stations = new ObservableCollection<StationDetailViewModel>();
        public ObservableCollection<StationDetailViewModel> Stations
        {
            get => _stations;
            set => this.RaiseAndSetIfChanged(ref _stations, value);
        }

        private ObservableAsPropertyHelper<bool> _selectedAllStation;
        public bool SelectedAllStation => _selectedAllStation.Value;

        //private ObservableCollection<StationDetailViewModel> _selectedStations = new ObservableCollection<StationDetailViewModel>();
        //public ObservableCollection<StationDetailViewModel> SelectedStations
        //{
        //    get => _selectedStations;
        //    set => this.RaiseAndSetIfChanged(ref _selectedStations, value);
        //}

        public LineDetailViewModel(string name, int order)
        {
            Name = name;
            Order = order;

            var count = new Random().Next(1,10);
            for (int i = 0; i < count; i++)
            {
                Stations.Add(new StationDetailViewModel($"{i}车站", this, i));
            }

            this.WhenAnyValue(line => line.Selected)
                .DistinctUntilChanged()
                .Where(selected => !selected)
                .Subscribe(selected =>
                {
                    foreach (var station in Stations)
                    {
                        station.Selected = false;
                    }
                });

            var changeSet = Stations.ToObservableChangeSet().AutoRefresh(station => station.Selected);

            _selectedAllStation = changeSet
                .ToCollection()
                .Select(station2 => station2.All(station => station.Selected))
                .DistinctUntilChanged()
                .ToProperty(this, nameof(LineDetailViewModel.SelectedAllStation));
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set => this.RaiseAndSetIfChanged(ref _selected, value);
        }
    }
}
