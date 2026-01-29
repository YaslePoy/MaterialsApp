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

public class OrdersViewModel : ViewModelBase, IRoutableViewModel
{
    private MaterialsContext _db;
    public bool _refreshing;

    public OrdersViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _db = new MaterialsContext();
        _ = LoadAsync();
    }


    private async Task LoadAsync()
    {
        await Task.Delay(500);
        Func<Order, bool> filter;
        switch (App.User.RoleId)
        {
            case 5:
                filter = order => order.CustomerId == App.User.Id;
                break;
            case 3:
                filter = order => order.ManagerId == App.User.Id || order.ManagerId == null;
                break;
            case 2:
                filter = order => order.StatusId == 3;
                break;
            case 4:
                filter = order => order.StatusId is 5 or 6;
                break;
            default:
                filter = _ => true;
                break;
        }

        _orders = (await _db.Orders.ToListAsync()).Where(filter).ToList();
        Orders.AddRange(_orders);
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    private List<Order> _orders;
    public ObservableCollection<Order> Orders { get; set; } = [];
    public Order? SelectedOrder { get; set; }

    public ICommand Create => ReactiveCommand.Create(() =>
    {
        HostScreen.Router.Navigate.Execute(new CreateOrderViewModel(HostScreen));
    });

    public ICommand SetupStatus => ReactiveCommand.Create<string>((istr) =>
    {
        var i = int.Parse(istr);
        if (SelectedOrder is null)
            return;


        bool update = true;


        if (i == 6)
            DoProduct();
        else if (i == 8)
        {
            
            HostScreen.Router.Navigate.Execute(new QualityViewModel(HostScreen, SelectedOrder));
            return;
        }

        if (update)
        {
            _db.StatusChanges.Add(new StatusChange
            {
                OldStatus = SelectedOrder.StatusId, NewStatus = i, Timestamp = DateTime.UtcNow,
                OrderId = SelectedOrder.Id
            });
            SelectedOrder!.StatusId = i;
            _db.Update(SelectedOrder);
            _db.SaveChanges();
        }
    });

    private async Task DoProduct()
    {
        var product = SelectedOrder.Product;
        var all = GetAllForProduct(product);
        foreach (var item in all)
        {
            switch (item)
            {
                case MaterialSpec ms:
                    ms.Material.Count -= ms.Count;
                    _db.Update(ms.Material);
                    break;
                case AccessoriesSpec acs:
                    acs.Accessories.Count -= acs.Count;
                    _db.Update(acs.Accessories);
                    break;
            }
        }

        await _db.SaveChangesAsync();
    }

    private List<object> GetAllForProduct(Product product)
    {
        var list = new List<object>();
        foreach (var materialSpec in product.MaterialSpecs)
        {
            list.Add(materialSpec);
        }

        foreach (var accessoriesSpec in product.AccessoriesSpecs)
        {
            list.Add(accessoriesSpec);
        }

        foreach (var itemSpec in product.AssemblySpecItems)
        {
            var inner = GetAllForProduct(itemSpec.Item);
            list.AddRange(inner);
        }

        return list;
    }

    public bool Visible1 => true;
    public bool Visible2 => true;
    public bool Visible3 => true;
    public bool Visible4 => true;
    public bool Visible5 => true;
    public bool Visible6 => true;
    public bool Visible7 => true;
    public bool Visible8 => true;
    public bool Visible9 => true;

    public ICommand ShowAll => ReactiveCommand.Create(() =>
    {
        Orders.Clear();
        Orders.AddRange(_orders);
    });

    public ICommand ShowNew => ReactiveCommand.Create(() =>
    {
        Orders.Clear();
        Orders.AddRange(_orders.Where(i => i.StatusId is 1 or 3 or 4));
    });

    public ICommand ShowCompleted => ReactiveCommand.Create(() =>
    {
        Orders.Clear();
        Orders.AddRange(_orders.Where(i => i.StatusId is 8 or 9));
    });

    public ICommand ShowCurrent => ReactiveCommand.Create(() =>
    {
        Orders.Clear();
        Orders.AddRange(_orders.Where(i => i.StatusId is 5 or 6 or 7));
    });

    public ICommand ShowCanceled => ReactiveCommand.Create(() =>
    {
        Orders.Clear();
        Orders.AddRange(_orders.Where(i => i.StatusId is 2));
    });

    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public async Task RefreshAsync()
    {
        if (!_refreshing)
        {
            _refreshing = true;
            return;
        }

        _db = new MaterialsContext();
        Orders.Clear();
        await LoadAsync();
    }
}