using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class ProductsViewModel : ViewModelBase, IRoutableViewModel
{
    private MaterialsContext _db = new();
    private bool _refreshing;
    public ProductsViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;

        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Products.Clear();
        Products.AddRange(await _db.Products.ToListAsync());
    }

    public ObservableCollection<Product> Products { get; set; } = [];

    public ICommand ViewProductCommand => ReactiveCommand.Create<Product?>((p) =>
    {
        if (p is null)
            return;

        HostScreen.Router.Navigate.Execute(new ProductViewModel(HostScreen, p));
    });

    public ICommand Create => ReactiveCommand.Create(() =>
    {
        HostScreen.Router.Navigate.Execute(new ProductViewModel(HostScreen, new Product()));
    });

    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    
    public async Task RefreshAsync()
    {
        if (!_refreshing)
        {
            _refreshing = true;
            return;
        }

        _db = new MaterialsContext();
        await LoadAsync();
    }
}