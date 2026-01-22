using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class LoginViewModel : ViewModelBase, IRoutableViewModel
{
    private string _login = string.Empty;
    private string _password = string.Empty;

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

    public LoginViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        if (File.Exists("login.txt"))
        {
            Task.Run(async () =>
            {
                await using var context = new MaterialsContext();
                var file = await File.ReadAllLinesAsync("login.txt");
                App.User = await context.Users.FirstAsync(i => i.Login == file[0] && i.Password == file[1]);
                
                Dispatcher.UIThread.Invoke(() => NavToUserScreen(App.User));
            });
        }
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }

    public ICommand LoginCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        await using var context = new MaterialsContext();
        if (await context.Users.FirstOrDefaultAsync(i => i.Login == _login &&  i.Password == _password) is not { } user)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка автоизации", "Проверьте правильность данных").ShowAsync();
            return;
        }
        
        App.User = user;

        if (Remember)
        {
            File.WriteAllLines("login.txt", [Login, Password]);
        }
        
        NavToUserScreen(user);
    },this.WhenAnyValue(i => i.Login, i => i.Password,
    (s, s1) => !string.IsNullOrWhiteSpace(s) && !string.IsNullOrWhiteSpace(s1)));

    private void NavToUserScreen(User user)
    {
        IRoutableViewModel vm = user.Role.Name switch
        {
            "Заказчик" => new CustomerViewModel(HostScreen),
            "Директор" => new DirectorViewModel(HostScreen),
            _ => new RegisterViewModel(HostScreen)
        };
        
        HostScreen.Router.Navigate.Execute(vm);
    }

    public ICommand RegisterCommand => ReactiveCommand.Create(() => { HostScreen.Router.Navigate.Execute(new RegisterViewModel(HostScreen)); });
    
    public bool Remember { get; set; }
}