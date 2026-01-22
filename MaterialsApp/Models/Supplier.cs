using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaterialsApp.Models;

public partial class Supplier
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string SupplyTime { get; set; } = null!;

    public virtual ICollection<Accessory> Accessories { get; set; } = new List<Accessory>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
