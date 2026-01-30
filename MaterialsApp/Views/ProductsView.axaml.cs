using Avalonia.Interactivity;
using MaterialsApp.ViewModels;
using ReactiveUI.Avalonia;

namespace MaterialsApp.Views;

public partial class ProductsView : ReactiveUserControl<ProductsViewModel>
{
    public ProductsView()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        _ = ViewModel?.RefreshAsync();
    }
}