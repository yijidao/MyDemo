using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace MyPrismDemo.ViewModels
{
    public class TrainTimeTableViewModel : BindableBase
    {
        private DataTable _data;
        public DataTable Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        //private ObservableCollection<TrainTimeItem> _trainTimeInfo = new ObservableCollection<TrainTimeItem>();
        //public ObservableCollection<TrainTimeItem> TrainTimeInfo
        //{
        //    get => _trainTimeInfo;
        //    set => SetProperty(ref _trainTimeInfo, value);
        //}

        private ObservableCollection<ExpandoObject> _trainTimeInfo = new ObservableCollection<ExpandoObject>();
        public ObservableCollection<ExpandoObject> TrainTimeInfo
        {
            get => _trainTimeInfo;
            set => SetProperty(ref _trainTimeInfo, value);
        }


        public ICommand LoadDataCommand { get; }

        public TrainTimeTableViewModel()
        {
            LoadDataCommand = new DelegateCommand(LoadData);
        }

        public void LoadData()
        {
            TrainTimeInfo.Add(CreateAndSetValue("机场北", DateTime.Today.AddHours(6)));
            TrainTimeInfo.Add(CreateAndSetValue("机场南", DateTime.Today.AddHours(6).AddMinutes(1)));
            TrainTimeInfo.Add(CreateAndSetValue("高增", DateTime.Today.AddHours(6).AddMinutes(5)));
            TrainTimeInfo.Add(CreateAndSetValue("人和", DateTime.Today.AddHours(6).AddMinutes(8)));
        }

        public ExpandoObject CreateAndSetValue(string name, DateTime planTime)
        {
            var model = new ExpandoObject();
            var dic = (IDictionary<string, object>)model;
            dic.Add("Name", name);
            dic.Add("PlanTime", planTime);
            return model;
        }



        //void PopulateRows(BaseTrainTimeItem[] items, Dictionary<string, object>[] customProps)
        //{
        //    var customUsers = items.Select((user, index) => new TrainTimeItem()
        //    {
        //        OtherProps = customProps[index];
        //    });
        //    grid.ItemsSource = customUsers;
        //}

        //public void LoadData()
        //{
        //    TrainTimeInfo.AddRange(new TrainTimeItem[]
        //    {
        //        new TrainTimeItem("机场北", DateTime.Today.AddHours(6)),
        //        new TrainTimeItem("机场南", DateTime.Today.AddHours(6).AddMinutes(1)),
        //        new TrainTimeItem("高增", DateTime.Today.AddHours(6).AddMinutes(5)),
        //        new TrainTimeItem("人和", DateTime.Today.AddHours(6).AddMinutes(8)),
        //        new TrainTimeItem("龙归", DateTime.Today.AddHours(6).AddMinutes(11)),
        //    });
        //}
    }

    public class BaseTrainTimeItem : BindableBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        //private DateTime _planedTime;
        //public DateTime PlanedTime
        //{
        //    get => _planedTime;
        //    set => SetProperty(ref _planedTime, value);
        //}

        //private DateTime? _realTime;
        //public DateTime? RealTime
        //{
        //    get => _realTime;
        //    set => SetProperty(ref _realTime, value);
        //}

        //public TrainTimeItem(string name, DateTime planedTime, DateTime? realTime = null)
        //{
        //    _name = name;
        //    _planedTime = planedTime;
        //    _realTime = realTime;
        //}
    }

    public class TrainTimeItem : BaseTrainTimeItem
    {
        public Dictionary<string, object> OtherProps { get; set; } = new Dictionary<string, object>();
    }

}
