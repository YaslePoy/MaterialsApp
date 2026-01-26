using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MaterialsApp.Models;
using MaterialsApp.ViewModels;
using ReactiveUI.Avalonia;

namespace MaterialsApp.Views;

public partial class WorkshopView : ReactiveUserControl<WorkshopViewModel>
{
    private bool _isPlaceing;
    private bool _isRemoveing;
    private string? _placeItem;

    public WorkshopView()
    {
        InitializeComponent();
    }

    private void HandlePlacementSelect(object? sender, PointerPressedEventArgs e)
    {
        if (ViewModel.SelectedWorkshop is null)
            return;

        var image = (Image)sender!;
        var name = image.Tag as string;

        _placeItem = name;
        _isPlaceing = true;
        _isRemoveing = false;
    }

    private void PlaceCurrent(object? sender, PointerReleasedEventArgs e)
    {
        if (ViewModel.SelectedWorkshop is null)
            return;

        if (!_isPlaceing)
        {
            return;
        }

        var visual = sender as Visual;
        var position = e.GetPosition(visual);

        var selectedWorkshopParsed = ViewModel.SelectedWorkshop.Parsed;
        selectedWorkshopParsed.Add(new WorkshopPlacement
            { ItemName = _placeItem, X = position.X, Y = position.Y });
        ViewModel.SelectedWorkshop.Parsed = selectedWorkshopParsed;
        DrawWorkshop();
    }

    public void DrawWorkshop()
    {
        var workshopView = WorkshopCanvas.Children;
        var background = workshopView[0];
        
        workshopView.Clear();
        
        workshopView.Add(background);
        foreach (var current in ViewModel.SelectedWorkshop.Parsed)
        {
            var item = new Image()
            {
                Source = new Bitmap(AssetLoader.Open(new Uri($"avares://MaterialsApp/Assets/{current.ItemName}"))),
                Stretch = Stretch.Uniform,
                Width = 35,
                Height = 35,
            };
            
            item.PointerPressed += (sender, args) => HandleRemove(current);
            workshopView.Add(item);
            
            Canvas.SetLeft(item, current.X - 20);
            Canvas.SetTop(item, current.Y - 20);
        }
    }

    private void HandleRemove(WorkshopPlacement placement)
    {
        if (!_isRemoveing)
            return;
        
        var current =  ViewModel.SelectedWorkshop.Parsed;
        current.Remove(placement);
        ViewModel.SelectedWorkshop.Parsed = current;
        DrawWorkshop();
    }

    private void RemoveSelect(object? sender, RoutedEventArgs e)
    {
        _isRemoveing = true;
        _isPlaceing = false;
    }

    private void UpdateWorkshopView(object? sender, SelectionChangedEventArgs e)
    {
        DrawWorkshop();
    }
}