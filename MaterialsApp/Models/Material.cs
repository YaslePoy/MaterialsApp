using System;
using System.Collections.Generic;
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

    public double? Characteristics { get; set; }

    public double? MassPerMeter { get; set; }

    public virtual ICollection<MaterialSpec> MaterialSpecs { get; set; } = new List<MaterialSpec>();

    public virtual Supplier Supplier { get; set; } = null!;
    public virtual MaterialType MaterialType { get; set; } = null!;
}

public class MaterialType
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}