using Microsoft.EntityFrameworkCore;

namespace MaterialsApp.Models;

public class MaterialsContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Server=micialware.ru;Port=5432;Database=store_db;User Id=postgres;Password=fwdcmwqxfi;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessoriesSpecs>().HasKey(e => new { e.ProductId, e.AccessoriesId });
        modelBuilder.Entity<MaterialSpecs>().HasKey(e => new { e.ProductId, e.MaterialId });
        modelBuilder.Entity<AssemblySpecs>().HasKey(e => new { e.ProductId, e.ItemId });
        modelBuilder.Entity<Order>().HasKey(e => new { e.Date, e.Number });
        modelBuilder.Entity<OperationSpecs>().HasKey(e => new { e.ProductId, e.Operation, e.Number });
    }
    
    public DbSet<Material> Materials { get; set; }
    public DbSet<Accessories> Accessories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<AccessoriesSpecs>  AccessoriesSpecs { get; set; }
    public DbSet<MaterialSpecs> MaterialSpecs { get; set; }
    public DbSet<AssemblySpecs>  AssemblySpecs { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Equipment> Equipments { get; set; }
    public DbSet<EquipmentType>  EquipmentTypes { get; set; }
    public DbSet<OperationSpecs> OperationSpecs { get; set; }
}