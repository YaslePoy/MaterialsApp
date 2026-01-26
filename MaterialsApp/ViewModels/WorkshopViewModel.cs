using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace MaterialsApp.ViewModels;

public class WorkshopViewModel : ViewModelBase, IRoutableViewModel
{
    private MaterialsContext _db;

    public WorkshopViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _db = new MaterialsContext();
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Workshops.AddRange(await _db.Workshops.ToListAsync());
    }

    public ObservableCollection<Workshop> Workshops { get; set; } = [];

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    public Workshop? SelectedWorkshop { get; set; }
    public ICommand SaveCommand => ReactiveCommand.Create((() =>
    {
        _db.SaveChanges();
    }));

    public ICommand BackCommand => ReactiveCommand.Create(() =>
    {
        HostScreen.Router.NavigateBack.Execute();
    });

}