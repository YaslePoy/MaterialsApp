using System;
using System.Collections.Generic;

namespace MaterialsApp.Models;

public partial class Equipment
{
    public string Mark { get; set; } = null!;

    public string EquipmentType { get; set; } = null!;

    public string Characteristics { get; set; } = null!;

    public virtual EquipmentType EquipmentTypeNavigation { get; set; } = null!;
    public override string ToString() => Mark;
}
