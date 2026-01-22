using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialsApp.Models;

public partial class Accessory
{
    public string Article { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Unit { get; set; }

    public int Count { get; set; }

    public int SupplierId { get; set; }

    public byte[]? Image { get; set; }

    [ForeignKey(nameof(AccessoryType))]
    public int AccessoryTypeId { get; set; }

    public decimal? Price { get; set; }

    public double? Weight { get; set; }

    public virtual ICollection<AccessoriesSpec> AccessoriesSpecs { get; set; } = new List<AccessoriesSpec>();

    public virtual Supplier Supplier { get; set; } = null!;
    public virtual AccessoryType AccessoryType { get; set; } = null!;
}

public class AccessoryType
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}