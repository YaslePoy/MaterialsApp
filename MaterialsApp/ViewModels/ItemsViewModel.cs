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
    private MaterialsContext _db;
    private bool _reloadFlag;
    private bool _ignoreNavigation;
    private Warehouse? _selectedWarehouse = null;
    private Material? _selectedMaterial;
    private Accessory? _selectedAccessory;

    public ItemsViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _db = new MaterialsContext();
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
    public decimal TotalAccessoriesPrice => FilteredAccessories.Sum(i => i.Price ?? 0 * i.Count);

    public async Task LoadAsync()
    {
        Materials.AddRange(await _db.Materials.ToListAsync());
        Accessories.AddRange(await _db.Accessories.ToListAsync());
        Warehouses.AddRange(await _db.Warehouses.ToListAsync());
        FilteredMaterials.AddRange(Materials);
        FilteredAccessories.AddRange(Accessories);
        this.RaisePropertyChanged(nameof(TotalMaterialsPrice));
    }

    public async Task ReloadAsync()
    {
        if (!_reloadFlag)
        {
            _reloadFlag = true;
            return;
        }

        await _db.DisposeAsync();
        _db = new MaterialsContext();
        
        _ignoreNavigation = true;
        SelectedMaterial = null;
        SelectedAccessory = null;

        Materials.Clear();
        Accessories.Clear();
        FilteredMaterials.Clear();
        FilteredAccessories.Clear();

        Materials.AddRange(await _db.Materials.ToListAsync());
        Accessories.AddRange(await _db.Accessories.ToListAsync());
        FilteredMaterials.AddRange(Materials);
        FilteredAccessories.AddRange(Accessories);
        this.RaisePropertyChanged(nameof(TotalMaterialsPrice));
        this.RaisePropertyChanged(nameof(TotalAccessoriesPrice));

        this.RaisePropertyChanged(nameof(SelectedMaterial));
        this.RaisePropertyChanged(nameof(SelectedAccessory));
        _ignoreNavigation = false;
    }

    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public ICommand ClearFilter => ReactiveCommand.Create(() => { SelectedWarehouse = null; });

    public Material? SelectedMaterial
    {
        get => _selectedMaterial;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedMaterial, value);
            if (!_ignoreNavigation && value is not null)
                HostScreen.Router.Navigate.Execute(new MaterialViewModel(HostScreen, value));
        }
    }

    public Accessory? SelectedAccessory
    {
        get => _selectedAccessory;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedAccessory, value);
            if (!_ignoreNavigation && value is not null)
                HostScreen.Router.Navigate.Execute(new AccessoryViewModel(HostScreen, value));
        }
    }
}