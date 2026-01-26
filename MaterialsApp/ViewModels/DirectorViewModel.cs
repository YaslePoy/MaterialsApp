using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class DirectorViewModel : ViewModelBase, IRoutableViewModel
{
    public DirectorViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _ = LoadAsync();
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }

    public ICommand Exit => ReactiveCommand.Create(() =>
    {
        if (File.Exists("login.txt")) File.Delete("login.txt");
        HostScreen.Router.NavigateBack.Execute();
    });

    public ObservableCollection<Employee> Employees { get; set; } = [];

    public async Task LoadAsync()
    {
        Employees.AddRange(await App.Db.Employees.ToListAsync());
    }

    public ICommand Create => ReactiveCommand.Create(() =>
        {
            HostScreen.Router.Navigate.Execute(new EmployeeViewModel(HostScreen, new Employee()));
        }
    );
    
    public Employee? SelectedEmployee { get; set; }
    
    public ICommand Edit => ReactiveCommand.Create(() =>
        {
            if (SelectedEmployee != null)
                HostScreen.Router.Navigate.Execute(new EmployeeViewModel(HostScreen, SelectedEmployee));
        }
    );

    public ICommand Delete => ReactiveCommand.Create(() =>
        {
            if (SelectedEmployee == null) return;
            App.Db.Employees.Remove(SelectedEmployee);
            App.Db.SaveChanges();
            Employees.Remove(SelectedEmployee);
        }
    );

    public ICommand ToItemsCommand => ReactiveCommand.Create(()=> { HostScreen.Router.Navigate.Execute(new ItemsViewModel(HostScreen)); });
    public ICommand ToWorkshopsCommand => ReactiveCommand.Create(()=> { HostScreen.Router.Navigate.Execute(new WorkshopViewModel(HostScreen)); });
}