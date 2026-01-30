using System;
using System.Windows.Input;
using MaterialsApp.Views;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class MasterViewModel : ViewModelBase, IRoutableViewModel, IScreen
{
    public MasterViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        Router = new RoutingState();
    }

    public RoutingState Router { get; set; }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    public ICommand ToOrders => ReactiveCommand.Create(() => Router.Navigate.Execute(new OrdersViewModel(this)));
    public ICommand ToOEq => ReactiveCommand.Create(() => Router.Navigate.Execute(new FailureViewModel(this)));
    public ICommand ToProds => ReactiveCommand.Create(() => Router.Navigate.Execute(new ProductsViewModel(this)));
}