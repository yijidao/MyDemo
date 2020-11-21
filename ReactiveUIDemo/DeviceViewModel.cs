using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class DeviceViewModel : ReactiveObject, IComparable<DeviceViewModel>
    {
        public string Name { get; }

        public RegionDetailViewModel Region { get; }

        public int Order { get; }

        private bool _selected = true;
        public bool Selected
        {
            get => _selected;
            set => this.RaiseAndSetIfChanged(ref _selected, value);
        }

        public DeviceViewModel(string name, RegionDetailViewModel region, int order)
        {
            Region = region;
            Order = order;

            Name = $"{region.Name}-{name}";
            //DeleteCommand = ReactiveCommand.Create(() =>
            //{
            //    Selected = false;
            //});


        }
        public ReactiveCommand<Unit, Unit> DeleteCommand => ReactiveCommand.Create(() =>
        {
            Selected = false;
        });

        public int CompareTo(DeviceViewModel other)
        {
            if (Region.CompareTo(other.Region) > 0) return 1;
            else if (Region.CompareTo(other.Region) < 0) return -1;
            else if (Order > other.Order) return 1;
            else if (Order < other.Order) return -1;
            else return 0;
        }
    }
}
