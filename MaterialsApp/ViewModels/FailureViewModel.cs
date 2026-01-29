using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using MaterialsApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;

namespace MaterialsApp.Views;

public class FailureViewModel : ViewModelBase, IRoutableViewModel
{
    private MaterialsContext _db = new();
    private Failure _newFailure = new();

    public FailureViewModel(IScreen hostScreen)
    {
        HostScreen = hostScreen;
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        Failures.Clear();
        Failures.AddRange(await _db.Failures.ToListAsync());
        Equipments.Clear();
        Equipments.AddRange(await _db.Equipments.ToListAsync());
    }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);
    public ObservableCollection<Failure> Failures { get; } = [];

    public ICommand Solve => ReactiveCommand.CreateFromTask<Failure>(async (f) =>
    {
        _db.Failures.Remove(f);
        await _db.SaveChangesAsync();
        Failures.Remove(f);
    });

    public ObservableCollection<Equipment> Equipments { get; set; } = [];

    public Failure NewFailure
    {
        get => _newFailure;
        set => this.RaiseAndSetIfChanged(ref _newFailure, value);
    }

    public ICommand RegisterFailure => ReactiveCommand.CreateFromTask(async () =>
    {
        _db.Failures.Add(_newFailure);

        try
        {
            await _db.SaveChangesAsync();
            Failures.Add(_newFailure);
            NewFailure = null;
            this.RaisePropertyChanged(nameof(NewFailure));
        }
        catch (Exception e)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Проверьте правильность заполнения полей")
                .ShowAsync();
        }
    });
}