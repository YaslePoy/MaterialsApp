using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Avalonia;

namespace MaterialsApp.Views;

public partial class FailureView : ReactiveUserControl<FailureViewModel>
{
    public FailureView()
    {
        InitializeComponent();
    }
}