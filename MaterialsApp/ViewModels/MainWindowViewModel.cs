using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    public RoutingState Router { get; }

    public MainWindowViewModel()
    {
        Router = new RoutingState();
        Router.Navigate.Execute(new LoginViewModel(this));
    }
}