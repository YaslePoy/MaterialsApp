using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaterialsApp.Models;

public partial class AssemblySpec
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; } 

    public int ItemId { get; set; }

    public int Count { get; set; }

    public virtual Product Item { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
