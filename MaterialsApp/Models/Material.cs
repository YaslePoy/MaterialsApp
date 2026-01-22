using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialsApp.Models;

public partial class Material
{
    public string Article { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public int Count { get; set; }

    public int SupplierId { get; set; }

    public byte[]? Image { get; set; }

    [ForeignKey(nameof(MaterialType))]
    public int MaterialTypeId { get; set; }

    public decimal? Price { get; set; }

    public string? Gost { get; set; }

    public double? Length { get; set; }

    public string? Characteristics { get; set; }

    public double? MassPerMeter { get; set; }
    [ForeignKey(nameof(Warehouse))]
    public int? WarehouseId { get; set; }
    public virtual ICollection<MaterialSpec> MaterialSpecs { get; set; } = new ObservableCollection<MaterialSpec>();

    public virtual Supplier Supplier { get; set; } = null!;
    public virtual MaterialType MaterialType { get; set; } = null!;
    public virtual Warehouse? Warehouse { get; set; } = null!;
}

public class MaterialType
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString() => Name;
}

public class Warehouse
{
    protected bool Equals(Warehouse other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Warehouse)obj);
    }

    public override int GetHashCode()
    {
        return Id;
    }

    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}