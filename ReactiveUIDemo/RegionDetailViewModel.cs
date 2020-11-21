using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class RegionDetailViewModel : ReactiveObject, IComparable<RegionDetailViewModel>
    {
        public string Name { get; }
        public int Order { get; }
        public StationDetailViewModel Station { get; }
        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set => this.RaiseAndSetIfChanged(ref _selected, value);
        }

        private ObservableCollection<DeviceViewModel> _devices = new ObservableCollection<DeviceViewModel>();
        public ObservableCollection<DeviceViewModel> Devices
        {
            get => _devices;
            set => this.RaiseAndSetIfChanged(ref _devices, value);
        }

        public RegionDetailViewModel(string name, StationDetailViewModel station, int order)
        {
            Name = $"{station.Name}-{name}";
            Station = station;
            Order = order;

            var count = new Random().Next(1, 5);
            for (int i = 0; i < count; i++)
            {
                Devices.Add(new DeviceViewModel($"{i}设备", this, i));
            }
        }

        public int CompareTo(RegionDetailViewModel other)
        {
            //throw new NotImplementedException();
            if (Station.CompareTo(other.Station) > 0) return 1;
            else if (Station.CompareTo(other.Station) < 0) return -1;
            else if (Order > other.Order) return 1;
            else if (Order < other.Order) return -1;
            else return 0;
        }
    }
}
