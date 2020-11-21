using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class OperateDeviceViewModel : ReactiveObject
    {
        private ObservableCollection<RegionDetailViewModel> _regions;
        public ObservableCollection<RegionDetailViewModel> Regions
        {
            get => _regions;
            set => this.RaiseAndSetIfChanged(ref _regions, value);
        }

        public OperateDeviceViewModel(ObservableCollection<RegionDetailViewModel> regions)
        {
            Regions = regions;
        }
    }
}
