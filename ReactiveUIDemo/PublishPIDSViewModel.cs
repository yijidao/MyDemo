using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using DynamicData;
using DynamicData.Binding;
using HandyControl.Controls;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class PublishPIDSViewModel : ReactiveObject
    {
        private ObservableCollection<LineDetailViewModel> _lines;
        public ObservableCollection<LineDetailViewModel> Lines
        {
            get => _lines;
            set => this.RaiseAndSetIfChanged(ref _lines, value);
        }

        private bool _selectedAll;
        public bool SelectedAll
        {
            get => _selectedAll;
            set => this.RaiseAndSetIfChanged(ref _selectedAll, value);
        }

        public bool SelectedAllChanging { get; set; }

        public bool SelectedChanging { get; set; }

        private bool _selectedAllStation;
        public bool SelectedAllStation
        {
            get => _selectedAllStation;
            set => this.RaiseAndSetIfChanged(ref _selectedAllStation, value);
        }

        public bool SelectedAllStationChanging { get; set; }

        public bool SelectedStationChanging { get; set; }

        private ObservableCollection<LineDetailViewModel> _selectedLines = new ObservableCollection<LineDetailViewModel>();
        public ObservableCollection<LineDetailViewModel> SelectedLines
        {
            get => _selectedLines;
            set => this.RaiseAndSetIfChanged(ref _selectedLines, value);
        }

        private ObservableCollection<StationDetailViewModel> _stations;
        public ObservableCollection<StationDetailViewModel> Stations
        {
            get => _stations;
            set => this.RaiseAndSetIfChanged(ref _stations, value);
        }

        private ObservableCollection<StationDetailViewModel> _selectedStations = new ObservableCollection<StationDetailViewModel>();
        public ObservableCollection<StationDetailViewModel> SelectedStations
        {
            get => _selectedStations;
            set => this.RaiseAndSetIfChanged(ref _selectedStations, value);
        }

        private bool _selectedAllRegions;
        public bool SelectedAllRegions
        {
            get => _selectedAllRegions;
            set => this.RaiseAndSetIfChanged(ref _selectedAllRegions, value);
        }

        public bool SelectedRegionChanging { get; set; }

        public bool SelectedAllRegionChanging { get; set; }

        private ObservableCollection<DeviceViewModel> _devices;
        public ObservableCollection<DeviceViewModel> Devices
        {
            get => _devices;
            set => this.RaiseAndSetIfChanged(ref _devices, value);
        }

        private ObservableCollection<DeviceViewModel> _selectedDevices = new ObservableCollection<DeviceViewModel>();
        public ObservableCollection<DeviceViewModel> SelectedDevices
        {
            get => _selectedDevices;
            set => this.RaiseAndSetIfChanged(ref _selectedDevices, value);
        }

        private ObservableCollection<RegionDetailViewModel> _regions;
        public ObservableCollection<RegionDetailViewModel> Regions
        {
            get => _regions;
            set => this.RaiseAndSetIfChanged(ref _regions, value);
        }

        public ReactiveCommand<Unit, Unit> ShowDevices => ReactiveCommand.Create(() =>
        {
            
            var viewModel = new OperateDeviceViewModel(new ObservableCollection<RegionDetailViewModel>(Regions.Where(x => x.Selected)));
            var view = new OperateDeviceView
            {
                ViewModel =  viewModel
            };
            new Window
            {
                Content = view, 
                Height = 800,
                Width = 600
            }.ShowDialog();

        });

        public PublishPIDSViewModel()
        {
            var lines = new[]
            {
                new LineDetailViewModel("一号线",  1),
                new LineDetailViewModel("二号线", 2),
                new LineDetailViewModel("三号线",  3),
                new LineDetailViewModel("四号线", 4),
                new LineDetailViewModel("五号线", 5),
                new LineDetailViewModel("六号线", 6),
                new LineDetailViewModel("七号线", 7),
                new LineDetailViewModel("八号线", 8),
                new LineDetailViewModel("九号线", 9),
                new LineDetailViewModel("十号线", 10),
            };

            Stations = new ObservableCollection<StationDetailViewModel>(GetStations(lines));
            Devices = new ObservableCollection<DeviceViewModel>(GetDevices(lines));
            Regions = new ObservableCollection<RegionDetailViewModel>(GetRegions(lines));

            this.WhenAnyValue(x => x.SelectedAll)
                .DistinctUntilChanged()
                .Subscribe(selectedAll =>
                {
                    if (!SelectedChanging)
                    {
                        SelectedAllChanging = true;
                        foreach (var line in lines)
                        {
                            line.Selected = selectedAll;
                        }

                        SelectedAllChanging = false;
                    }
                });


            Lines = new ObservableCollection<LineDetailViewModel>(lines);
            var changeSet = Lines.ToObservableChangeSet();
            changeSet.AutoRefresh(line => line.Selected).ToCollection()
               .Select(x => x.All(y => y.Selected))
               .DistinctUntilChanged()
               .Subscribe(e =>
               {
                   if (!SelectedAllChanging)
                   {
                       SelectedChanging = true;
                       SelectedAll = e;
                       SelectedChanging = false;
                   }
               });

            changeSet.AutoRefresh(line => line.Selected)
                .ToCollection()
                .Subscribe(lines1 =>
                {
                    var addLines = lines1.Where(line => line.Selected && !SelectedLines.Contains(line));
                    foreach (var line in addLines)
                    {
                        SelectedLines.Add(line);
                        var index = SelectedLines.Count - 1;
                        // 排序一下
                        while (index > 0 && SelectedLines.ElementAt(index).Order < SelectedLines.ElementAt(index - 1).Order)
                        {
                            var temp = SelectedLines.ElementAt(index);
                            SelectedLines[index] = SelectedLines[index - 1];
                            SelectedLines[index - 1] = temp;
                            index--;
                        }
                    }

                    var removeLines = lines1.Where(line => !line.Selected && SelectedLines.Contains(line));
                    foreach (var line in removeLines)
                    {
                        SelectedLines.Remove(line);
                        foreach (var station in line.Stations)
                        {
                            station.Selected = false;
                        }
                    }
                });



            this.WhenAnyValue(x => x.SelectedAllStation)
                .DistinctUntilChanged()
                .Subscribe(selectedAllStation =>
                {
                    if (!SelectedStationChanging)
                    {
                        SelectedAllStationChanging = true;
                        foreach (var station in Lines.Where(line => line.Selected).SelectMany(line => line.Stations))
                        {
                            station.Selected = selectedAllStation;
                        }

                        SelectedAllStationChanging = false;
                    }
                });

            var changeSet2 = SelectedLines.ToObservableChangeSet();
            changeSet2.AutoRefresh(line => line.SelectedAllStation)
                .ToCollection()
                .Select(lines2 => lines2.Count > 0 && lines2.All(line => line.SelectedAllStation))
                .DistinctUntilChanged()
                .Subscribe(selectedAllStation =>
                {
                    if (!SelectedAllStationChanging)
                    {
                        SelectedStationChanging = true;
                        SelectedAllStation = selectedAllStation;
                        SelectedStationChanging = false;
                    }
                });

            var changeSet3 = Stations.ToObservableChangeSet();
            changeSet3.AutoRefresh(station => station.Selected)
            .ToCollection()
            .Subscribe(stations =>
            {
                var adds = stations.Where(x => x.Selected && !SelectedStations.Contains(x));
                foreach (var station in adds)
                {
                    SelectedStations.Add(station);
                    var index = SelectedStations.Count - 1;
                    while (index > 0 && SelectedStations[index].CompareTo(SelectedStations[index - 1]) < 0)
                    {
                        var temp = SelectedStations[index];
                        SelectedStations[index] = SelectedStations[index - 1];
                        SelectedStations[index - 1] = temp;
                        index--;
                    }
                }

                var removes = stations.Where(x => !x.Selected && SelectedStations.Contains(x));
                foreach (var station in removes)
                {
                    SelectedStations.Remove(station);
                    foreach (var region in station.Regions)
                    {
                        region.Selected = false;
                    }
                }
            });

            this.WhenAnyValue(x => x.SelectedAllRegions)
                .DistinctUntilChanged()
                .Subscribe(selectedAllRegions =>
                {
                    if (SelectedRegionChanging) return;
                    SelectedAllRegionChanging = true;
                    foreach (var station in SelectedStations)
                    {
                        foreach (var region in station.Regions)
                        {
                            region.Selected = selectedAllRegions;
                        }
                    }
                    SelectedAllRegionChanging = false;
                });

            SelectedStations.ToObservableChangeSet()
                .AutoRefresh()
                .ToCollection()
                .Select(stations => stations.Count > 0 && stations.All(station => station.SelectedAllRegions))
                .DistinctUntilChanged()
                .Subscribe(selectedAllRegions =>
                {
                    if (SelectedAllRegionChanging) return;
                    SelectedRegionChanging = true;
                    SelectedAllRegions = selectedAllRegions;
                    SelectedRegionChanging = false;
                });






            Regions.ToObservableChangeSet()
                .AutoRefresh(region => region.Selected)
                .ToCollection()
                .Subscribe(regions =>
                {
                    var add = regions.Where(x => x.Selected).SelectMany(y => y.Devices).Where(x => x.Selected && !SelectedDevices.Contains(x));
                    foreach (var device in add)
                    {
                        SelectedDevices.Add(device);
                        var index = SelectedDevices.Count - 1;
                        while (index > 0 && SelectedDevices[index].CompareTo(SelectedDevices[index - 1]) < 0)
                        {
                            var temp = SelectedDevices[index];
                            SelectedDevices[index] = SelectedDevices[index - 1];
                            SelectedDevices[index - 1] = temp;
                            index--;
                        }
                    }

                    var remove = regions.Where(x => !x.Selected).SelectMany(y => y.Devices);
                    foreach (var device in remove)
                    {
                        if (SelectedDevices.Contains(device))
                        {
                            SelectedDevices.Remove(device);
                        }
                    }
                });

            //SelectedDevices.ToObservableChangeSet()
            //    .AutoRefresh(device => device.Selected)
            //    .ToCollection()


            Devices.ToObservableChangeSet()
                .AutoRefresh(device => device.Selected)
                .ToCollection()
                .Subscribe(devices =>
                {
                    var add = devices.Where(x => x.Selected && !SelectedDevices.Contains(x) && x.Region.Selected);
                    foreach (var device in add)
                    {
                        SelectedDevices.Add(device);
                        var index = SelectedDevices.Count - 1;
                        while (index > 0 && SelectedDevices[index].CompareTo(SelectedDevices[index - 1]) < 0)
                        {
                            var temp = SelectedDevices[index];
                            SelectedDevices[index] = SelectedDevices[index - 1];
                            SelectedDevices[index - 1] = temp;
                            index--;
                        }
                    }

                    var remove = devices.Where(x => !x.Selected && SelectedDevices.Contains(x));
                    foreach (var device in remove)
                    {
                        SelectedDevices.Remove(device);
                    }

                });

        }

        private IEnumerable<StationDetailViewModel> GetStations(IEnumerable<LineDetailViewModel> lines)
        {
            var stations = new List<StationDetailViewModel>();
            foreach (var line in lines)
            {
                stations.AddRange(line.Stations);
            }

            return stations;
        }

        private IEnumerable<DeviceViewModel> GetDevices(IEnumerable<LineDetailViewModel> lines) =>
            lines.SelectMany(line => line.Stations)
                 .SelectMany(station => station.Regions)
                 .SelectMany(region => region.Devices);

        private IEnumerable<RegionDetailViewModel> GetRegions(IEnumerable<LineDetailViewModel> lines) =>
            lines.SelectMany(line => line.Stations)
                .SelectMany(station => station.Regions);

        #region test


        #endregion
    }
}
