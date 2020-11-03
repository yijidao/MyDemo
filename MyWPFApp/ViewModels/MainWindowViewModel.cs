using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace MyWPFApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        public string ButtonContent { get; set; } = "命令按钮";

        public ICommand ClickCommand => new DelegateCommandAsync(this, async () =>
        {
            await Task.Delay(5000);
            Count++;

            T1(async () =>
            {
                await Task.Delay(100);
                for (int i = 0; i < 10; i++)
                {
                    LoadedCommand.Execute(null);
                }
            });
        });

        private int _count;
        public int Count
        {
            get { return _count; }
            set { SetProperty(ref _count, value); }
        }


        public ICommand LoadedCommand => new DelegateCommandAsync(this, async () =>
        {
            await Task.Delay(3000);
            Count++;

        });

        private void T1(Action f)
        {

        }

        private void T1(Func<Task> f)
        {

        }
    }
}
