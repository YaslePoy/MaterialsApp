using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MaterialsApp.ViewModels;
using ReactiveUI.Avalonia;

namespace MaterialsApp.Views;

public partial class AccessoryView : ReactiveUserControl<AccessoryViewModel>
{
    public AccessoryView()
    {
        InitializeComponent();
    }
}