using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
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
        public ReactiveCommand<RoutedEventArgs, Unit> LoadedCommand { get; }


        public MainViewModel()
        {
            //Locator.CurrentMutable.Register(()=> new FirstView(), typeof(IViewFor<FirstViewModel>));
            
            //GoNextCommand = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new FirstViewModel(this)));
            GoNextCommand = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new FirstViewModel()));
            GoBackCommand = Router.NavigateBack;
            LoadedCommand = ReactiveCommand.Create<RoutedEventArgs>(e =>
            {

            });

            //LoadedCommand = ReactiveCommand.Create<RoutedEventArgs>(e =>
            //{

            //}, Observable.Return(false));

        }
    }
}
