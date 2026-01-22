using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class AccessoryViewModel : ViewModelBase, IRoutableViewModel
{
    public AccessoryViewModel(IScreen hostScreen, Accessory material)
    {
        HostScreen = hostScreen;
        Inner = material;
        _ = LoadAsync();
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    public Accessory Inner { get; set; }
    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public ObservableCollection<Warehouse> Warehouses { get; set; } = [];
    public ObservableCollection<Supplier> Suppliers { get; set; } = [];

    public async Task LoadAsync()
    {
        Warehouses.AddRange(await App.Db.Warehouses.ToListAsync());
        Suppliers.AddRange(await App.Db.Suppliers.ToListAsync());
        Types.AddRange(await App.Db.AccessoryTypes.ToListAsync());
    }

    public ICommand SaveCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        if (string.IsNullOrWhiteSpace(Inner.Article))
        {
            App.Db.Accessories.Add(Inner);
        }
        else
        {
            App.Db.Accessories.Update(Inner);
        }

        try
        {
            await App.Db.SaveChangesAsync();
            HostScreen.Router.Navigate.Execute(new ItemsViewModel(HostScreen));
        }
        catch
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Проверьте правильность заполнения полей")
                .ShowAsync();
        }
    });

    public bool ReadOnly => !(App.User.RoleId is 1 or 3);
    public bool Editable => (App.User.RoleId is 1 or 3);
    public ObservableCollection<AccessoryType> Types { get; set; } = [];
}