using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MaterialsApp.Models;

public partial class OperationSpec
{
    [Key]
    public int Id { get; set; }
    public int ProductId { get; set; }

    public string Operation { get; set; } = null!;

    public int Number { get; set; }

    public string EquipmentType { get; set; } = null!;

    public int OperationTime { get; set; }

    public virtual EquipmentType EquipmentTypeNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
