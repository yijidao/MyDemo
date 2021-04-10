using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private Visibility _moreContentVisibility = Visibility.Collapsed;
        public Visibility MoreContentVisibility
        {
            get => _moreContentVisibility;
            set => this.RaiseAndSetIfChanged(ref _moreContentVisibility, value);
        }

        private Visibility _moreContentVisibilityReverse = Visibility.Collapsed;
        public Visibility MoreContentVisibilityReverse
        {
            get => _moreContentVisibilityReverse;
            set => this.RaiseAndSetIfChanged(ref _moreContentVisibilityReverse, value);
        }

        public int MainPollMonitorListMaximum { get; set; } = 4;

        public PollMonitorViewModel()
        {
            PollMonitorList.Connect()
                .WhenAnyPropertyChanged(nameof(PollMonitorItemViewModel.Selected))
                .Subscribe(change =>
                {
                    if (change.Selected)
                    {
                        foreach (var item in PollMonitorList.Items.Where(x => x.Selected && x != change))
                        {
                            item.Selected = false;
                        }
                    }
                });

            TestCommand = ReactiveCommand.Create(LoadData);
            ClearCommand = ReactiveCommand.Create(Clear);
            AddCommand = ReactiveCommand.Create(Add);
            RemoveCommand = ReactiveCommand.Create(Remove);
            MoveForwardCommand = ReactiveCommand.Create(MoveForward);
            MoveBackCommand = ReactiveCommand.Create(MoveBack);

        }

        private void LoadData()
        {
            var datas = new List<PollMonitorItemViewModel>();
            for (int i = 1; i <= Count; i++)
            {
                datas.Add(new PollMonitorItemViewModel {Name =  $"六字测试模块{i}"});
            }

            PollMonitorList.Clear();
            PollMonitorList.AddRange(datas);
            MainPollMonitorList = new ObservableCollection<PollMonitorItemViewModel>(PollMonitorList.Items.Take(MainPollMonitorListMaximum));
            MorePollMonitorList = new ObservableCollection<PollMonitorItemViewModel>(PollMonitorList.Items.Skip(MainPollMonitorListMaximum));

            MoreContentVisibility = MainPollMonitorList.Count == MainPollMonitorListMaximum
                ? Visibility.Visible
                : Visibility.Collapsed;
            MoreContentVisibilityReverse =
                MoreContentVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Clear()
        {
            PollMonitorList.Clear();
        }

        private void Add()
        {
            var c = PollMonitorList.Items.Count() + 1;
            PollMonitorList.Add(new PollMonitorItemViewModel{Name = $"六字测试模块{c}"});
        }

        private void Remove()
        {
            if (PollMonitorList.Count == 0) return;
            PollMonitorList.RemoveAt(0);
        }

        private void MoveForward()
        {

        }

        private void MoveBack()
        {

        }


        #region 测试代码

        private int _count = 10;
        public int Count
        {
            get => _count;
            set => this.RaiseAndSetIfChanged(ref _count, value);
        }

        public ReactiveCommand<Unit, Unit> TestCommand { get; }

        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        public ReactiveCommand<Unit, Unit> AddCommand { get; }

        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        public ReactiveCommand<Unit, Unit> MoveForwardCommand { get; }

        public ReactiveCommand<Unit, Unit> MoveBackCommand { get; }

        #endregion
    }
}
