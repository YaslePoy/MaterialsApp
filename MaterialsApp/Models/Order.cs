using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaterialsApp.Models;

public partial class Order
{
    [Key]
    public int Id { get; set; }
    public int Number { get; set; }

    public DateOnly Date { get; set; }

    public string Name { get; set; } = null!;

    public int ProductId { get; set; }

    public int CustomerId { get; set; }

    public int ManagerId { get; set; }

    public decimal Cost { get; set; }

    public DateOnly EndDate { get; set; }

    public string Schemas { get; set; } = null!;

    public virtual User Customer { get; set; } = null!;

    public virtual User Manager { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
