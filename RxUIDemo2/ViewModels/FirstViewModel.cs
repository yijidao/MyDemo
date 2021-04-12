using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using Splat;

namespace RxUIDemo2.ViewModels
{
    public class FirstViewModel : ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = "first";
        public IScreen HostScreen { get; }

        public FirstViewModel(IScreen screen)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
