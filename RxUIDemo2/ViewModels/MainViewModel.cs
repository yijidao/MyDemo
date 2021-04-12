using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using ReactiveUI;
using RxUIDemo2.Views;
using Splat;

namespace RxUIDemo2.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; } = new RoutingState();
        public ReactiveCommand<Unit, IRoutableViewModel> GoNextCommand { get; }
        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }


        public MainViewModel()
        {
            Locator.CurrentMutable.Register(()=> new FirstView(), typeof(IViewFor<FirstViewModel>));

            GoNextCommand = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new FirstViewModel(this)));

            GoBackCommand = Router.NavigateBack;

        }
    }
}
