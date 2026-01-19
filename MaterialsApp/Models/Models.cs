using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaterialsApp.Models;

public class Accessories
{
    [Key] public string Article { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Unit { get; set; }
    public int Count { get; set; }
    [ForeignKey("Supplier"), Required]
    public string SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }
    public byte[] Image { get; set; }
    [Required]
    public string ProductType { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public double Weight { get; set; }
}

public class Supplier
{
    [Key] public string Name { get; set; }
    [Required] public string Address { get; set; }
    [Required] public DateTime City { get; set; }
}

public class Material
{
    [Key] public string Article { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Unit { get; set; }
    public int Count { get; set; }
    [ForeignKey("Supplier"), Required]
    public string SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }
    public byte[] Image { get; set; }
    [Required]
    public string ProductType { get; set; }
    [Required]
    public decimal Price { get; set; }
    public string GOST { get; set; }
    [Required]
    public double Length { get; set; }
    [Required]
    public double Characteristics { get; set; }
}

public class AccessoriesSpecs
{
    [ForeignKey(nameof(Accessories)), Required]
    public string AccessoriesId { get; set; }
    public virtual Accessories Accessories { get; set; }
    [ForeignKey(nameof(Product)), Required]
    public string ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Count { get; set; }
}


public class MaterialSpecs
{
    [ForeignKey(nameof(Material)), Required]
    public string MaterialId { get; set; }
    public virtual Material Material { get; set; }
    [ForeignKey(nameof(Product)), Required]
    public string ProductId { get; set; }
    public virtual Product Product { get; set; }
    public int Count { get; set; }
}

public class Product
{
    [Key] public string Name { get; set; }
    [Required] public string Size { get; set; }
}

public class OperationSpecs
{
    [ForeignKey(nameof(Product))]
    public string ProductId { get; set; }
    public virtual Product Product { get; set; }
    public string Operation { get; set; }
    public int Number { get; set; }
    [ForeignKey(nameof(Type)), Required]
    public string EquipmentType { get; set; }
    public virtual EquipmentType Type { get; set; }
    public int OperationTime { get; set; }
}

public class Equipment
{
    [Key] public string Mark { get; set; }
    [Required, ForeignKey(nameof(Type))]
    public string EquipmentType { get; set; }
    public virtual EquipmentType Type { get; set; }
    [Required] public string Characteristics { get; set; }
}

public class EquipmentType
{
    [Key] public string Name { get; set; }
}

public class Order
{
    public int Number { get; set; }
    public DateOnly Date { get; set; }
    [Required] public string Name { get; set; }
    [ForeignKey("Product"), Required] public string ProductId { get; set; }
    public virtual Product Product { get; set; }
    [ForeignKey(nameof(Customer)), Required]
    public string CustomerId { get; set; }
    public virtual User Customer { get; set; }
    [ForeignKey(nameof(Manager)), Required]
    public string ManagerId { get; set; }
    public virtual User Manager { get; set; }
    public decimal Cost { get; set; }
    public DateOnly EndDate { get; set; }
    public string Schemas { get; set; }
}

public class User
{
    [Key] public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }
    public byte[] Image { get; set; }
}

public class AssemblySpecs
{
    [ForeignKey(nameof(Product)), Required]
    public string ProductId { get; set; }
    public virtual Product Product { get; set; }
    [ForeignKey(nameof(Item)), Required]
    public string ItemId {get; set;}
    public virtual Product Item { get; set; }
    public int Count { get; set; }
}