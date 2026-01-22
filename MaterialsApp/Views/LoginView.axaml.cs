using MaterialsApp.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace MaterialsApp.Views;

public partial class LoginView : ReactiveUserControl<LoginViewModel>
{
    public LoginView()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
    }
}