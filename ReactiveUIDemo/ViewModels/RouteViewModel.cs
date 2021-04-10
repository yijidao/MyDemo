using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace ReactiveUIDemo.ViewModels
{
    public class RouteViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        public ReactiveCommand<Unit, Unit> GoNextCommand { get; }

        public ReactiveCommand<Unit, Unit> GoBackCommand { get; }

        public RouteViewModel()
        {
            Router = new RoutingState();
            GoNextCommand = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new FirstViewModel()));
            GoBackCommand = Router.NavigateBack;
        }
    }
}
