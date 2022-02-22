using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PrismDemo.ViewModels
{
    public class PrintPdfViewModel : BindableBase, IDialogAware
    {
        private string _template;
        public string Template
        {
            get => _template;
            set => SetProperty(ref _template, value);
        }

        private ExpandoObject _data;
        public ExpandoObject Data
        {
            get => _data;
            set => SetProperty(ref _data, value);
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            parameters.TryGetValue("template", out _template);
            parameters.TryGetValue("data", out _data);
        }

        public string Title => "预览";
        public event Action<IDialogResult> RequestClose;
    }
}
