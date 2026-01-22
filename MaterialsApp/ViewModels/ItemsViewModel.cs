using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class ItemsViewModel : ViewModelBase, IRoutableViewModel
{
    private Warehouse? _selectedWarehouse = null;
    private Material _selectedMaterial;

    public ItemsViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _ = LoadAsync();
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }

    public Warehouse? SelectedWarehouse
    {
        get => _selectedWarehouse;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedWarehouse, value);
            FilteredMaterials.Clear();
            FilteredAccessories.Clear();

            if (value is null)
            {
                FilteredMaterials.AddRange(Materials);
                FilteredAccessories.AddRange(Accessories);
            }
            else
            {
                FilteredMaterials.AddRange(Materials.Where(i => i.WarehouseId == value.Id));
                FilteredAccessories.AddRange(Accessories.Where(i => i.WarehouseId == value.Id));
            }

            this.RaisePropertyChanged(nameof(TotalMaterialsPrice));
        }
    }

    public ObservableCollection<Warehouse> Warehouses { get; set; } = [];
    public ObservableCollection<Material> Materials { get; set; } = [];
    public ObservableCollection<Accessory> Accessories { get; set; } = [];

    public ObservableCollection<Material> FilteredMaterials { get; set; } = [];
    public ObservableCollection<Accessory> FilteredAccessories { get; set; } = [];
    public decimal TotalMaterialsPrice => FilteredMaterials.Sum(i => i.Price ?? 0 * i.Count);

    public async Task LoadAsync()
    {
        Materials.AddRange(await App.Db.Materials.ToListAsync());
        Accessories.AddRange(await App.Db.Accessories.ToListAsync());
        Warehouses.AddRange(await App.Db.Warehouses.ToListAsync());
        FilteredMaterials.AddRange(Materials);
        FilteredAccessories.AddRange(Accessories);
        this.RaisePropertyChanged(nameof(TotalMaterialsPrice));
    }

    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public ICommand ClearFilter => ReactiveCommand.Create(() => { SelectedWarehouse = null; });

    public Material SelectedMaterial
    {
        get => _selectedMaterial;
        set => HostScreen.Router.Navigate.Execute(new MaterialViewModel(HostScreen, value));
    }
}