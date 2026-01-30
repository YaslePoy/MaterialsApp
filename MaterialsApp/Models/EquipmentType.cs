using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MaterialsApp.Models;

public partial class EquipmentType
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Equipment> Equipment { get; set; } = new ObservableCollection<Equipment>();

    public virtual ICollection<OperationSpec> OperationSpecs { get; set; } = new ObservableCollection<OperationSpec>();
    public override string ToString() => Name;
}
