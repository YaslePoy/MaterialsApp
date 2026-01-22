using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaterialsApp.Models;

public partial class MaterialSpec
{
    [Key]
    public int Id { get; set; }
    public string MaterialId { get; set; } = null!;

    public int ProductId { get; set; }

    public int Count { get; set; }

    public virtual Material Material { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
