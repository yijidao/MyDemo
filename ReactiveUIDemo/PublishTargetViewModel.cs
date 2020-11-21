using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class PublishTargetViewModel : ReactiveObject
    {
        public LineDetailViewModel Line { get; }
        public StationDetailViewModel Station { get; }
        public RegionDetailViewModel Region { get; }
        public DeviceViewModel Device { get; }

        public PublishTargetViewModel(LineDetailViewModel line, StationDetailViewModel station, RegionDetailViewModel region, DeviceViewModel device)
        {
            Line = line;
            Station = station;
            Region = region;
            Device = device;
        }
    }
}
