using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace MaterialsApp.Models;

public partial class Product
{
    [Key] public int Id { get; set; }
    public string Name { get; set; } = null!;

    public string Size { get; set; } = null!;

    public virtual ICollection<AccessoriesSpec> AccessoriesSpecs { get; set; } = new ObservableCollection<AccessoriesSpec>();

    public virtual ICollection<AssemblySpec> AssemblySpecItems { get; set; } = new ObservableCollection<AssemblySpec>();

    public virtual ICollection<AssemblySpec> AssemblySpecProducts { get; set; } = new ObservableCollection<AssemblySpec>();

    public virtual ICollection<MaterialSpec> MaterialSpecs { get; set; } = new ObservableCollection<MaterialSpec>();

    public virtual ICollection<OperationSpec> OperationSpecs { get; set; } = new ObservableCollection<OperationSpec>();

    public virtual ICollection<Order> Orders { get; set; } = new ObservableCollection<Order>();
}