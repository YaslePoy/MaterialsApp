using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MaterialsApp.Models;

public class Employee
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Patronymic { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Address { get; set; }
    public string Education { get; set; }
    public string Qualifications { get; set; }
    [NotMapped] public int Age => (DateTime.Today - BirthDate.ToDateTime(default)).Days / 365;
    
    public virtual ICollection<EmployeeOperation> Operations { get; set; } = new ObservableCollection<EmployeeOperation>();
    [NotMapped] public string OperationsList => string.Join(", ", Operations.Select(i => i.Operation.Operation));
}

public class EmployeeOperation
{
    [Key] public int Id { get; set; }
    [ForeignKey(nameof(Employee))] public int EmployeeId { get; set; }
    [ForeignKey(nameof(Operation))] public int OperationId { get; set; }

    public virtual Employee Employee { get; set; }
    public virtual OperationSpec Operation { get; set; }
}