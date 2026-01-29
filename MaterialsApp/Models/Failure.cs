using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Input;
using ReactiveUI;

namespace MaterialsApp.Models;

public class Failure
{
    public int Id { get; set; }
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    [ForeignKey(nameof(Equipment))] public string EquipmentId { get; set; }
    public virtual Equipment Equipment { get; set; }
    public string Reason { get; set; }

}