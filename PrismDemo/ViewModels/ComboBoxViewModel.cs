using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicData;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace PrismDemo.ViewModels
{
    public class ComboBoxViewModel : BindableBase, IDialogAware
    {

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if (args.PropertyName == nameof(SearchText))
            {
                Debug.WriteLine(SearchText);
            }
        }

        public ComboBoxViewModel()
        {
            //var b = new SourceList<Book>();
            //b.Connect().Bind()

        }


        #region dialog
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public string Title { get; } = "ComboBoxView";
        public event Action<IDialogResult> RequestClose;


        #endregion

    }
}
