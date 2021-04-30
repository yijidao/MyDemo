using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyPrismDemo.Extensions;
using Prism.Commands;
using Prism.Mvvm;

namespace MyPrismDemo.ViewModels
{
    public class TrainTimeTableViewModel : BindableBase
    {
        private ObservableCollection<ExpandoObject> _trainTimeInfo;
        public ObservableCollection<ExpandoObject> TrainTimeInfo
        {
            get => _trainTimeInfo;
            set => SetProperty(ref _trainTimeInfo, value);
        }


        public ICommand LoadDataCommand { get; }

        public ICommand PullDataCommand { get; }

        public string[] Stations { get; } = { "机场北", "机场南", "高增", "人和", "龙归", "嘉禾望岗", "白云大道北", "永泰", "同和", "京溪南方医院", "梅花园", "燕塘" };

        public int TrainCount { get; set; } = 10;

        public int PullIndex { get; set; }

        public int TrainIndex { get; set; } = 1;

        private bool[] _trainTimeFlags;
        public bool[] TrainTimeFlags
        {
            get => _trainTimeFlags;
            set => SetProperty(ref _trainTimeFlags, value);
        }

        private ObservableCollection<string> _trainNames;
        public ObservableCollection<string> TrainNames
        {
            get => _trainNames;
            set => SetProperty(ref _trainNames, value);
        }

        private Dictionary<string, bool> _timeoutDic = new Dictionary<string, bool>();
        public Dictionary<string, bool> TimeoutDic
        {
            get => _timeoutDic;
            set => SetProperty(ref _timeoutDic, value);
        }

        public TrainTimeTableViewModel()
        {
            LoadDataCommand = new DelegateCommand(LoadData);
            PullDataCommand = new DelegateCommand(PullData);
        }

        private void PullData()
        {
            var current = (IDictionary<string, object>)TrainTimeInfo[PullIndex];

            if (PullIndex % 2 == 0)
            {
                current[$"RealTime{TrainIndex}"] = (DateTime)current[$"PlanTime{TrainIndex}"];
            }
            else
            {
                current[$"RealTime{TrainIndex}"] = ((DateTime)current[$"PlanTime{TrainIndex}"]).AddMinutes(2);
            }

            if (PullIndex == TrainTimeInfo.Count - 1)
            {
                var last = (IDictionary<string, object>)TrainTimeInfo[PullIndex];

                for (int i = 1; i <= TrainCount; i++)
                {
                    var real = (DateTime?)last[$"RealTime{i}"];
                    var plan = (DateTime)last[$"PlanTime{i}"];

                    if (real == null || real.Value - plan < TimeSpan.FromMinutes(2)) continue;

                    foreach (var item in TrainTimeInfo)
                    {
                        var dic = (IDictionary<string, object>)item;
                        dic[$"Timeout{i}"] = true;
                    }

                    TimeoutDic[TrainNames[i-1]] = true;
                    RaisePropertyChanged(nameof(TimeoutDic));
                }
                

                PullIndex = 0;

                TrainIndex++;
                if (TrainIndex > TrainCount)
                {
                    TrainIndex = 1;
                }
            }
            else
            {
                PullIndex++;
            }
        }

        public void LoadData()
        {
            PullIndex = 0;
            var datas = GetDbTrainTimes();

            TrainNames = new ObservableCollection<string>(datas.Select(x => x.TrainName));

            TimeoutDic = TrainNames.ToDictionary(x => x, x => false);

            TrainTimeFlags = CreateTrainTimeoutFlagList(datas);

            var expandoObjects = ConvertToDynamic(datas);
            //TrainTimeInfo.AddRange(expandoObjects);
            TrainTimeInfo = new ObservableCollection<ExpandoObject>(expandoObjects);
        }

        private bool[] CreateTrainTimeoutFlagList(TrainTime[] trainTimes)
        {
            var result = new bool[trainTimes.Length];

            for (int i = 0; i < trainTimes.Length; i++)
            {
                var current = trainTimes[i];
                var lastStationTime = current.StationTimes.LastOrDefault();

                if (lastStationTime?.RealTime != null && lastStationTime.RealTime.Value - lastStationTime.PlanTime >=
                    TimeSpan.FromMinutes(2))
                {
                    result[i] = true;
                }
            }

            return result;
        }

        private ExpandoObject[] ConvertToDynamic(TrainTime[] trainTimes)
        {
            var stationDic = Stations.ToDictionary(x => x, y =>
            {
                var expandoObject = new ExpandoObject();
                var dic = (IDictionary<string, object>)expandoObject;
                dic.Add("Name", y);
                return expandoObject;
            });

            var result = stationDic.Select(x =>
            {
                var expandoObject = x.Value;
                for (int i = 0; i < trainTimes.Length; i++)
                {
                    var dic = (IDictionary<string, object>)expandoObject;
                    var current = trainTimes[i];
                    dic.Add($"PlanTime{i + 1}", current.StationTimeDic[x.Key].PlanTime); // 这两行似乎可以优化成一行
                    dic.Add($"RealTime{i + 1}", current.StationTimeDic[x.Key].RealTime);
                    dic.Add($"Timeout{i + 1}", false);
                }
                return expandoObject;
            }).ToArray();

            return result;
        }

        private TrainTime[] GetDbTrainTimes()
        {
            var list = new List<TrainTime>();
            for (var i = 1; i <= TrainCount; i++)
            {
                list.Add(new TrainTime($"车次{i}", GetDbTrainStationTimes()));
            }
            return list.ToArray();
        }

        private TrainStationTime[] GetDbTrainStationTimes() => Stations.Select((t, i) => new TrainStationTime(t, DateTime.Today.AddHours(6).AddMinutes(i * 3))).ToArray();
    }

    public class TrainTime
    {
        public string TrainName { get; set; }
        public TrainStationTime[] StationTimes { get; set; }

        public Dictionary<string, TrainStationTime> StationTimeDic { get; }

        public TrainTime(string trainName, TrainStationTime[] stationTimes)
        {
            TrainName = trainName;
            StationTimes = stationTimes;
            StationTimeDic = stationTimes.ToDictionary(x => x.Station);
        }
    }

    public class TrainStationTime
    {
        public string Station { get; set; }

        public DateTime PlanTime { get; set; }

        public DateTime? RealTime { get; set; }

        public TrainStationTime(string station, DateTime planTime, DateTime? realTime = null)
        {
            Station = station;
            PlanTime = planTime;
            RealTime = realTime;
        }
    }


}
