using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Common;
using ReactiveUI;
using ReactiveUIDemo.Controls;

namespace ReactiveUIDemo.ViewModels
{
    public class PictureMarkDemoViewModel : ReactiveObject
    {

        private ObservableCollection<MarkInfo> _marks;
        public ObservableCollection<MarkInfo> Marks
        {
            get => _marks;
            set => this.RaiseAndSetIfChanged(ref _marks, value);
        }

        private ObservableCollection<MarkInfo> _monitors;
        public ObservableCollection<MarkInfo> Monitors
        {
            get => _monitors;
            set => this.RaiseAndSetIfChanged(ref _monitors, value);
        }

        public PictureMarkDemoViewModel()
        {
            Monitors = new ObservableCollection<MarkInfo>
            {
                new MarkInfo(1,"1", 100, 100),
                new MarkInfo(2,"2", 100, 200),
                new MarkInfo(3,"3", 200, 200)
            };

            Marks = new ObservableCollection<MarkInfo>();
        }

        /// <summary>
        /// 保存到数据库
        /// </summary>
        public void Save()
        {

        }

    }
}
