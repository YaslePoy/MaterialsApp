using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MaterialsApp.ViewModels;
using ReactiveUI.Avalonia;

namespace MaterialsApp.Views;

public partial class EmployeeView : ReactiveUserControl<EmployeeViewModel>
{
    public EmployeeView()
    {
        InitializeComponent();
    }
}