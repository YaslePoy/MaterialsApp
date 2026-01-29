using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MaterialsApp.ViewModels;
using ReactiveUI.Avalonia;

namespace MaterialsApp.Views;

public partial class OrdersView : ReactiveUserControl<OrdersViewModel>
{
    public OrdersView()
    {
        InitializeComponent();
    }
    
    
    protected override void OnLoaded(RoutedEventArgs e)
    {
        _ = ViewModel?.RefreshAsync();
    }
}