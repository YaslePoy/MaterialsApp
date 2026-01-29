using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using DynamicData;
using MaterialsApp.Models;
using MsBox.Avalonia;
using ReactiveUI;


namespace MaterialsApp.ViewModels;

public class QualityViewModel : ViewModelBase, IRoutableViewModel
{
    private MaterialsContext _db;

    public QualityViewModel(IScreen hostScreen, Order selectedOrder)
    {
        HostScreen = hostScreen;
        _db = new MaterialsContext();

        if (_db.ProductAssessments.FirstOrDefault(i => i.OrderId == selectedOrder.Id) is { } assessment)
        {
            ProductAssessment = assessment;
        }
        else
        {
            ProductAssessment = new ProductAssessment { Order = _db.Orders.First(i => i.Id == selectedOrder.Id), Parameters = "[]" };
        }

        QualityParameters.AddRange(JsonSerializer.Deserialize<List<QualityParameter>>(ProductAssessment.Parameters) ??
                                   []);
        
        ProductAssessment.Decision = QualityParameters.All(i => i.IsOk);
        CanComment = !ProductAssessment.Decision;
    }

    public ProductAssessment ProductAssessment { get; set; }
    public ObservableCollection<QualityParameter> QualityParameters { get; set; } = [];
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString();
    public IScreen HostScreen { get; }
    public ICommand NavigateBackCommand => ReactiveCommand.Create(HostScreen.Router.NavigateBack.Execute);

    public ICommand SaveCommand => ReactiveCommand.CreateFromTask(async () =>
    {
        ProductAssessment.Parameters = JsonSerializer.Serialize(QualityParameters.ToArray());
        if (ProductAssessment.Id == 0)
        {
            _db.ProductAssessments.Add(ProductAssessment);
        }
        else
        {
            _db.ProductAssessments.Update(ProductAssessment);
        }

        try
        {
            ProductAssessment.Order.StatusId = 8;
            await _db.SaveChangesAsync();
            HostScreen.Router.NavigateBack.Execute();
        }
        catch (Exception e)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Проверьте правильность заполнения полей")
                .ShowAsync();
        }
    });

    public ICommand NewParam => ReactiveCommand.Create(() => { QualityParameters.Add(new QualityParameter()); });

    public ICommand CalculateQuality => ReactiveCommand.Create(() =>
    {
        ProductAssessment.Decision = QualityParameters.All(i => i.IsOk);
        CanComment = !ProductAssessment.Decision;
        this.RaisePropertyChanged(nameof(ProductAssessment.Decision));
        this.RaisePropertyChanged(nameof(CanComment));
    });

    public ICommand DeleteParam => ReactiveCommand.Create<QualityParameter>((p) => { QualityParameters.Remove(p); });
    public bool CanComment { get; set; } = true;
}

public class QualityParameter
{
    public string Name { get; set; }
    public string Value { get; set; }
    public bool IsOk { get; set; } = true;
}