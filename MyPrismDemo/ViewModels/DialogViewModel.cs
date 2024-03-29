﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace MyPrismDemo.ViewModels
{
    public class DialogViewModel : BindableBase, IDialogAware
    {
        public DialogViewModel()
        {
            
        }

        public bool CanCloseDialog() => true;
        

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public string Title { get; }
        public event Action<IDialogResult> RequestClose;
    }
}
