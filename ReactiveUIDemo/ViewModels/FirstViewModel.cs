using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    public class FirstViewModel :ReactiveObject, IRoutableViewModel
    {
        public string? UrlPathSegment { get; } = "first";
        public IScreen HostScreen { get; }

        public FirstViewModel(IScreen screen)
        {
            HostScreen = screen;
        }
    }
}
