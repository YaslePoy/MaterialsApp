using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialsApp.Models;
using MsBox.Avalonia;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class RegisterViewModel : ViewModelBase, IRoutableViewModel
{
    private string _login = string.Empty;
    private string _password = string.Empty;
    private string _name = string.Empty;

    public RegisterViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }

    public string Login
    {
        get => _login;
        set => this.RaiseAndSetIfChanged(ref _login, value);
    }

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public ICommand RegisterCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        await using var context = new MaterialsContext();
        if (context.Users.Any(i => i.Login == Login))
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пользователь с таким логином уже сущетсвует")
                .ShowAsync();
            return;
        }

        await Register(context);
    }, CanRegister);

    private async Task Register(MaterialsContext context)
    {
        try
        {
            context.Users.Add(new User { Login = Login, Password = Password, Name = Name, RoleId = 5});
            await context.SaveChangesAsync();
            HostScreen.Router.NavigateBack.Execute();
        }
        catch
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Ошибка регистрации заказчика").ShowAsync();
        }
        
    }
    
    private IObservable<bool> CanRegister => this.WhenAnyValue(i => i.Password, l => l.Login, i => i.Name,
        (p, l, n) => !string.IsNullOrWhiteSpace(n) && !string.IsNullOrWhiteSpace(l) && p.Length is >= 4 and <= 16 && p.Any(char.IsDigit) && p.Any(char.IsUpper) && p.Any(char.IsLower) && p.All(c => !"*&{}|+".Contains(c)));

    public ICommand Back => ReactiveCommand.Create(() =>
    {
        HostScreen.Router.NavigateBack.Execute();
    });
}