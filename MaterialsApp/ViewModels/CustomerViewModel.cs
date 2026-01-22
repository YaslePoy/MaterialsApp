using System;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class CustomerViewModel : ViewModelBase, IRoutableViewModel
{
    public CustomerViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    
    
}