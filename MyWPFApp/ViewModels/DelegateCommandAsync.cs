using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;

namespace MyWPFApp.ViewModels
{
    public class DelegateCommandAsync : DelegateCommand
    {
        public DelegateCommandAsync(Action executeMethod) : base(executeMethod)
        {
        }

        public DelegateCommandAsync(Action executeMethod, Func<bool> canExecuteMethod) : base(executeMethod, canExecuteMethod)
        {
        }

        public DelegateCommandAsync(ViewModelBase viewModelBase, Func<Task> executeMethod) : base(async () =>
        {
            viewModelBase.MaskVisibility = Visibility.Visible;
            await executeMethod();
            viewModelBase.MaskVisibility = Visibility.Collapsed;
        })
        {

        }

        public DelegateCommandAsync(ViewModelBase viewModelBase, Func<Task> executeMethod,
            Func<bool> canExecuteMethod) : base(
            async () =>
            {
                viewModelBase.MaskVisibility = Visibility.Visible;
                await executeMethod();
                viewModelBase.MaskVisibility = Visibility.Collapsed;
            }, canExecuteMethod)
        {
        }

    }
}
