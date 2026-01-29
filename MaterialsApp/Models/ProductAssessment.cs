using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace MaterialsApp.Models;

public class ProductAssessment : INotifyPropertyChanged
{
    private bool _decision;
    [Key] public int Id { get; set; }
    [ForeignKey(nameof(Order))] public int OrderId { get; set; }
    public virtual Order Order { get; set; }

    public bool Decision
    {
        get => _decision;
        set
        {
            if (value == _decision) return;
            _decision = value;
            OnPropertyChanged();
        }
    }

    public string Comment { get; set; } = "";
    public string Parameters { get; set; } = "[]";
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
}