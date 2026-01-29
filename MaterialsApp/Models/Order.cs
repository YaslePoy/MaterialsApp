using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MaterialsApp.Models;

public partial class Order : INotifyPropertyChanged
{
    private int _statusId;

    [Key]
    public int Id { get; set; }
    public string Number { get; set; }

    public DateOnly Date { get; set; }

    public string Name { get; set; } = null!;

    public int? ProductId { get; set; }

    public int CustomerId { get; set; }

    public int? ManagerId { get; set; }

    public decimal Cost { get; set; }

    public DateOnly EndDate { get; set; } =  DateOnly.FromDateTime(DateTime.UtcNow);

    public string Schemas { get; set; } = null!;

    public virtual User Customer { get; set; } = null!;

    public virtual User? Manager { get; set; } = null!;

    public virtual Product? Product { get; set; } = null!;

    public int StatusId
    {
        get => _statusId;
        set
        {
            if (value == _statusId) return;
            _statusId = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(StatusName));
        }
    }

    [NotMapped] public string StatusName => StatusId switch
    {
        1 => "Новый",
        2 => "Отменен",
        3 => "Составление спецификации",
        4 => "Подтверждение",
        5 => "Закупка",
        6 => "Производство",
        7 => "Контроль",
        8 => "Готов",
        9 => "Закрыт",
        _ => throw new ArgumentOutOfRangeException()
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
    
    public string Description { get; set; }
    public string Sizes { get; set; }
    // public string Files { get; set; } = "";
}

public class StatusChange
{
    [Key]
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int OldStatus { get; set; }
    public int NewStatus { get; set; }
    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public virtual Order Order { get; set; }
}