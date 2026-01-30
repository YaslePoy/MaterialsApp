using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class ProductViewModel : ViewModelBase, IRoutableViewModel
{
    private MaterialsContext _db;

    public ProductViewModel(IScreen hostScreen, Product product)
    {
        HostScreen = hostScreen;
        _db = new MaterialsContext();
        Inner = product.Id != 0 ? _db.Products.First(i => i.Id == product.Id) : new Product { Size = "[]"};

        ProductSizes.AddRange(JsonSerializer.Deserialize<List<ProductSize>>(Inner.Size)!);
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Materials.AddRange(await _db.Materials.ToListAsync());
        Accessories.AddRange(await _db.Accessories.ToListAsync());
        Products.AddRange(await _db.Products.ToListAsync());
        Equipments.AddRange(await _db.EquipmentTypes.ToListAsync());

        Operations.AddRange(await _db.OperationSpecs.Where(i => i.ProductId == Inner.Id).ToListAsync());
        MaterialSpecs.AddRange(await _db.MaterialSpecs.Where(i => i.ProductId == Inner.Id).ToListAsync());
        AccessoriesSpecs.AddRange(await _db.AccessoriesSpecs.Where(i => i.ProductId == Inner.Id).ToListAsync());
        AssemblySpecs.AddRange(await _db.AssemblySpecs.Where(i => i.ProductId == Inner.Id).ToListAsync());
    }

    public Product Inner { get; set; }
    public ObservableCollection<Material> Materials { get; } = [];
    public ObservableCollection<Accessory> Accessories { get; } = [];
    public ObservableCollection<Product> Products { get; } = [];
    public ObservableCollection<EquipmentType> Equipments { get; } = [];
    public ObservableCollection<OperationSpec> Operations { get; } = [];
    public ObservableCollection<MaterialSpec> MaterialSpecs { get; } = [];
    public ObservableCollection<AccessoriesSpec> AccessoriesSpecs { get; } = [];
    public ObservableCollection<AssemblySpec> AssemblySpecs { get; } = [];
    public ObservableCollection<ProductSize> ProductSizes { get; } = [];

    public ICommand AddNew => ReactiveCommand.Create<object>((p) =>
    {
        if (p is null)
        {
            return;
        }
        switch (p)
        {
            case Material material:
                var matSpec = new MaterialSpec { Material = material, Product = Inner, Count = 1 };
                MaterialSpecs.Add(matSpec);
                _db.MaterialSpecs.Add(matSpec);
                break;
            case Accessory accessory:
                var accSpec = new AccessoriesSpec { Accessories = accessory, Product = Inner, Count = 1 };
                AccessoriesSpecs.Add(accSpec);
                _db.AccessoriesSpecs.Add(accSpec);
                break;
            case Product product:
                var asmSpec = new AssemblySpec { Item = product, Product = Inner, Count = 1 };
                AssemblySpecs.Add(asmSpec);
                _db.AssemblySpecs.Add(asmSpec);
                break;
            case OperationSpec:
                var operationSpec = new OperationSpec();
                operationSpec.Product = Inner;
                _db.OperationSpecs.Add(operationSpec);
                Operations.Add(operationSpec);
                break;
            case ProductSize:
                ProductSizes.Add(new ProductSize());
                break;
        }
    });

    public ICommand RemoveNew => ReactiveCommand.Create<object>((p) =>
    {
        switch (p)
        {
            case MaterialSpec material:
                MaterialSpecs.Remove(material);
                _db.MaterialSpecs.Remove(material);
                break;
            case AccessoriesSpec accessory:
                AccessoriesSpecs.Remove(accessory);
                _db.AccessoriesSpecs.Remove(accessory);
                break;
            case AssemblySpec product:
                AssemblySpecs.Remove(product);
                _db.AssemblySpecs.Remove(product);
                break;
            case OperationSpec operationSpec:
                _db.OperationSpecs.Remove(operationSpec);
                Operations.Remove(operationSpec);
                break;
            case ProductSize productSize:
                ProductSizes.Remove(productSize);
                break;
        }
    });

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }

    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public ICommand SaveCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        Inner.Size = JsonSerializer.Serialize(ProductSizes);

        if (Inner.Id == 0)
        {
            _db.Products.Add(Inner);
        }
        else
        {
            _db.Products.Update(Inner);
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
}

public class ProductSize
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Unit { get; set; }

    public ProductSize()
    {
        Name = "";
        Value = "";
        Unit = "";
    }
}