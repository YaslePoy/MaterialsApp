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

public class CreateOrderViewModel : ViewModelBase, IRoutableViewModel
{
    private MaterialsContext _db = new();

    public Order Order { get; set; } = new()
    {
        Date = DateOnly.FromDateTime(DateTime.Now),
        StatusId = App.User.RoleId switch
        {
            3 => 3,
            _ => 1
        }
    };

    public CreateOrderViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Customers.AddRange(await _db.Users.Where(i => i.RoleId == 5).ToListAsync());
        Products.AddRange(await _db.Products.ToListAsync());
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }

    public User? Customer
    {
        get => Order.Customer;
        set
        {
            Order.Customer = value;
            this.RaisePropertyChanged();
            this.RaisePropertyChanged(nameof(Number));
        }
    }

    public string Number
    {
        get
        {
            if (Customer == null)
            {
                return string.Empty;
            }

            var first = string.IsNullOrWhiteSpace(Customer.Name) ? "_" : Customer.Name[0].ToString().ToUpper();
            var second = string.IsNullOrWhiteSpace(Customer.Surname) ? "_" : Customer.Surname[0].ToString().ToUpper();
            var datetime = Order.Date.ToString("yyyyMMdd");
            var index = _db.Orders.Count(i => i.CustomerId == Customer.Id).ToString();

            if (index.Length > 2)
            {
                index = index[^2..];
            }
            else if (index.Length == 1)
            {
                index = "0" + index;
            }

            Order.Number =$"{first}{second}{datetime}{index}";
            return Order.Number;
        }
    }

    public ObservableCollection<User> Customers { get; set; } = [];
    public ObservableCollection<Product> Products { get; set; } = [];

    public DateTimeOffset OrderEndDate
    {
        get => new(Order.EndDate.ToDateTime(default));
        set => Order.EndDate = new DateOnly(value.Year, value.Month, value.Day);
    }

    public ICommand Create => ReactiveCommand.CreateFromTask(async () =>
    {
        try
        {
            if (App.User.RoleId == 3)
            {
                Order.Manager = App.User;
            }
            
            _db.Orders.Add(Order);
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Проверьте правильность данных").ShowAsync();
        }
    });

    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);
    public ICommand CreateConsumer => ReactiveCommand.Create(() => HostScreen.Router.Navigate.Execute(new RegisterViewModel(HostScreen)));
}