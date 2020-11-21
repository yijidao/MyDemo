using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class StationDetailViewModel : ReactiveObject, IComparable<StationDetailViewModel>
    {
        public LineDetailViewModel Line { get; set; }
        public string Name { get; }

        private ObservableAsPropertyHelper<bool> _selectedAllRegions;
        public bool SelectedAllRegions => _selectedAllRegions.Value;

        public int Order { get; set; }

        public StationDetailViewModel(string name, LineDetailViewModel line, int order)
        {
            Order = order;
            Line = line;
            Name = $"{line.Name}-{name}";

            var count = new Random().Next(1, 5);
            for (int i = 0; i < count; i++)
            {
                Regions.Add(new RegionDetailViewModel($"{i}区域", this, i));
            }

            _selectedAllRegions = Regions.ToObservableChangeSet()
                .AutoRefresh(region => region.Selected)
                .ToCollection()
                .Select(regions2 => regions2.All(x => x.Selected))
                .DistinctUntilChanged()
                .ToProperty(this, nameof(StationDetailViewModel.SelectedAllRegions));
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set => this.RaiseAndSetIfChanged(ref _selected, value);
        }

        private ObservableCollection<RegionDetailViewModel> _regions = new ObservableCollection<RegionDetailViewModel>();
        public ObservableCollection<RegionDetailViewModel> Regions
        {
            get => _regions;
            set => this.RaiseAndSetIfChanged(ref _regions, value);
        }

        public int CompareTo(StationDetailViewModel other)
        {
            if (Line.Order < other.Line.Order) return -1;
            else if (Line.Order > other.Line.Order) return 1;
            else if (Order < other.Order) return -1;
            else if (Order > other.Order) return 1;
            else return 0;
        }
    }
}
