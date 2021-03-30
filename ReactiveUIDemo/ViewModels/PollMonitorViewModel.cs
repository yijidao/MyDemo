using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Prism.Mvvm;
using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    public class PollMonitorViewModel : ReactiveObject
    {
        public SourceList<PollMonitorItemViewModel> PollMonitorList { get; set; } = new SourceList<PollMonitorItemViewModel>();

        private ObservableCollection<PollMonitorItemViewModel> _mainPollMonitorList = new ObservableCollection<PollMonitorItemViewModel>();
        public ObservableCollection<PollMonitorItemViewModel> MainPollMonitorList
        {
            get => _mainPollMonitorList;
            set => this.RaiseAndSetIfChanged(ref _mainPollMonitorList, value);
        }

        private ObservableCollection<PollMonitorItemViewModel> _morePollMonitorList = new ObservableCollection<PollMonitorItemViewModel>();
        public ObservableCollection<PollMonitorItemViewModel> MorePollMonitorList
        {
            get => _morePollMonitorList;
            set => this.RaiseAndSetIfChanged(ref _morePollMonitorList, value);
        }

        public int MainPollMonitorListMaximum { get; set; } = 4;

        public PollMonitorViewModel()
        {
            var set = PollMonitorList.Connect().Publish();
            LoadData();
        }

        private void LoadData()
        {
            PollMonitorItemViewModel[] datas = new[]
            {
                new PollMonitorItemViewModel{Name = "模块1"},
                new PollMonitorItemViewModel{Name = "模块2"},
                new PollMonitorItemViewModel{Name = "模块3"},
                new PollMonitorItemViewModel{Name = "模块4"},
                new PollMonitorItemViewModel{Name = "模块5"},
                new PollMonitorItemViewModel{Name = "模块6"},
                new PollMonitorItemViewModel{Name = "模块7"},
                new PollMonitorItemViewModel{Name = "模块8"},
                new PollMonitorItemViewModel{Name = "模块9"},
                new PollMonitorItemViewModel{Name = "模块10"},
            };
            MainPollMonitorList.AddRange(datas);
            MainPollMonitorList = new ObservableCollection<PollMonitorItemViewModel>(PollMonitorList.Items.Take(MainPollMonitorListMaximum));
            MorePollMonitorList = new ObservableCollection<PollMonitorItemViewModel>(PollMonitorList.Items.Skip(MainPollMonitorListMaximum));
        }
    }
}
