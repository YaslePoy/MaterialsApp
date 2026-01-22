using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Data.Converters;
using DynamicData;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class EmployeeViewModel : ViewModelBase, IRoutableViewModel
{
    private OperationSpec? _selectedOperation;
    public Employee Inner { get; set; }

    public EmployeeViewModel(IScreen hostScreen, Employee employee)
    {
        HostScreen = hostScreen;
        Inner = employee;
        _ = LoadAsync();
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }

    public async Task LoadAsync()
    {
        Operations.AddRange(await App.Db.OperationSpecs.ToListAsync());
    }

    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public ICommand SaveCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        if (Inner.Id == 0)
        {
            App.Db.Employees.Add(Inner);
        }
        else
        {
            App.Db.Employees.Update(Inner);
        }

        try
        {
            await App.Db.SaveChangesAsync();
            HostScreen.Router.Navigate.Execute(new DirectorViewModel(HostScreen));
            this.RaisePropertyChanging(nameof(HasId));
            this.RaisePropertyChanging(nameof(HasId));
        }
        catch
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Проверьте правильность заполнения полей")
                .ShowAsync();
        }
    });

    public DateTimeOffset BirthDate
    {
        get => new(Inner.BirthDate.ToDateTime(default));
        set => Inner.BirthDate = new DateOnly(value.Year, value.Month, value.Day);
    }

    public ObservableCollection<OperationSpec> Operations { get; set; } = [];

    public OperationSpec? SelectedOperation
    {
        get => _selectedOperation;
        set => this.RaiseAndSetIfChanged(ref _selectedOperation, value);
    }

    public ICommand RemoveOperation => ReactiveCommand.CreateFromTask(async () =>
        {
            if (SelectedAddedOperation is null) return;
            

            App.Db.EmployeeOperations.Remove(SelectedAddedOperation);
            await App.Db.SaveChangesAsync();
            
        }
    );

    public ICommand AddOperation => ReactiveCommand.CreateFromTask(async () =>
    {
        if (SelectedOperation is null) return;

        if (Inner.Operations.Any(i => i.OperationId == SelectedOperation.Id)) return;

        var operation = new EmployeeOperation { EmployeeId = Inner.Id, OperationId = SelectedOperation.Id };

        App.Db.EmployeeOperations.Add(operation);
        await App.Db.SaveChangesAsync();

    });

    public bool HasId => Inner.Id != 0;
    public EmployeeOperation SelectedAddedOperation { get; set; }
}