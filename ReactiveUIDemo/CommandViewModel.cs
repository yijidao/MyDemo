using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace ReactiveUIDemo
{
    public class CommandViewModel : ReactiveObject
    {
        public CommandViewModel()
        {
            //ButtonCommand = ReactiveCommand.CreateFromTask(ButtonFunc, 
            //    this.WhenAnyValue(x => x.TextContent)
            //        .Select(x => !string.IsNullOrEmpty(TextContent))
            //    );
            ButtonCommand = ReactiveCommand.CreateFromTask(ButtonFunc,
                this.WhenAnyValue(x => x.List1)
                    //.ToObservableChangeSet()
                    //.AutoRefresh()
                    .Select(x => x.Count > 0));

            GenerateList1Command = ReactiveCommand.CreateFromTask(async () =>
            {
                await Task.Delay(500);
                List1 = List1.Count > 0 ? new ObservableCollection<string>() : new ObservableCollection<string> { "1", "2" };
                //if (List1.Count > 0)
                //{
                //    List1.Clear();
                //}
                //else
                //{
                //    List1.AddRange(new []{"1","2"});
                //}
            }, outputScheduler:RxApp.MainThreadScheduler);
        }

        public int Count { get; set; }

        private string _textContent;
        public string TextContent
        {
            get => _textContent;
            set => this.RaiseAndSetIfChanged(ref _textContent, value);
        }

        private ObservableCollection<string> _list1 = new ObservableCollection<string>();
        public ObservableCollection<string> List1
        {
            get => _list1;
            set => this.RaiseAndSetIfChanged(ref _list1, value);
        }

        public async Task ButtonFunc()
        {
            await Task.Delay(500);
            System.Diagnostics.Debug.WriteLine($"Count：{++Count}");
        }

        public ReactiveCommand<Unit, Unit> ButtonCommand { get; }

        public ReactiveCommand<Unit, Unit> GenerateList1Command { get; }

    }
}
