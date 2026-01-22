using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialsApp.Models;

public partial class User
{
    [Key]
    public int Id { get; set; }
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    [ForeignKey(nameof(Role))]
    public int RoleId { get; set; }
    public virtual Role Role { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; }
    public string? Patronymic { get; set; }

    public byte[]? Image { get; set; }

    public virtual ICollection<Order> OrderCustomers { get; set; } = new ObservableCollection<Order>();

    public virtual ICollection<Order> OrderManagers { get; set; } = new ObservableCollection<Order>();
}
