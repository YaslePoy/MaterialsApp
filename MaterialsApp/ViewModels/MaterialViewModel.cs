using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class MaterialViewModel : ViewModelBase, IRoutableViewModel
{
    private MaterialsContext _db;
    public MaterialViewModel(IScreen hostScreen, Material material)
    {
        HostScreen = hostScreen;
        _db = new MaterialsContext();
        _ = LoadAsync(material);
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    public Material Inner { get; set; }
    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public ObservableCollection<Warehouse> Warehouses { get; set; } = [];
    public ObservableCollection<Supplier> Suppliers { get; set; } = [];

    public async Task LoadAsync(Material material)
    {
        Inner = await _db.Materials.FirstAsync(i => i.Article == material.Article);
        Warehouses.AddRange(await _db.Warehouses.ToListAsync());
        Suppliers.AddRange(await _db.Suppliers.ToListAsync());
        Types.AddRange(await _db.MaterialTypes.ToListAsync());
        this.RaisePropertyChanged(nameof(Inner));
    }
    
    public ICommand SaveCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        if (string.IsNullOrWhiteSpace(Inner.Article))
        {
            _db.Materials.Add(Inner);
        }
        else
        {
            _db.Materials.Update(Inner);
        }

        try
        {
            await _db.SaveChangesAsync();
            HostScreen.Router.NavigateBack.Execute();
        }
        catch
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Проверьте правильность заполнения полей")
                .ShowAsync();
        }
    });

    public bool ReadOnly => App.User.RoleId is not (1 or 3);
    public bool Editable => App.User.RoleId is 1 or 3;
    public ObservableCollection<MaterialType> Types { get; set; } = [];

}