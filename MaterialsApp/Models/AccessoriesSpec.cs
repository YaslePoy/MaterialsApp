using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaterialsApp.Models;

public partial class AccessoriesSpec
{
    [Key]
    public int Id { get; set; }
    public string AccessoriesId { get; set; } = null!;

    public int ProductId { get; set; }

    public int Count { get; set; }

    public virtual Accessory Accessories { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
