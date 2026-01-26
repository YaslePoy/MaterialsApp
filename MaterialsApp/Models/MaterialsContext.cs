using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MaterialsApp.Models;

public partial class MaterialsContext : DbContext
{
    public MaterialsContext()
    {
    }

    public MaterialsContext(DbContextOptions<MaterialsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessoriesSpec> AccessoriesSpecs { get; set; }

    public virtual DbSet<Accessory> Accessories { get; set; }

    public virtual DbSet<AssemblySpec> AssemblySpecs { get; set; }

    public virtual DbSet<Equipment> Equipments { get; set; }

    public virtual DbSet<EquipmentType> EquipmentTypes { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialSpec> MaterialSpecs { get; set; }

    public virtual DbSet<OperationSpec> OperationSpecs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<MaterialType> MaterialTypes { get; set; }
    public virtual DbSet<AccessoryType> AccessoryTypes { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EmployeeOperation> EmployeeOperations { get; set; }
    public virtual DbSet<Warehouse> Warehouses { get; set; }
    public virtual DbSet<Workshop> Workshops { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;port=5432;Database=materials_db;User Id=micial;Password=1234").UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessoriesSpec>(entity =>
        {
            entity.HasIndex(e => e.AccessoriesId, "IX_AccessoriesSpecs_AccessoriesId");

            entity.HasOne(d => d.Accessories).WithMany(p => p.AccessoriesSpecs).HasForeignKey(d => d.AccessoriesId);

            entity.HasOne(d => d.Product).WithMany(p => p.AccessoriesSpecs).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Accessory>(entity =>
        {
            entity.HasKey(e => e.Article);

            entity.HasIndex(e => e.SupplierId, "IX_Accessories_SupplierId");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Accessories).HasForeignKey(d => d.SupplierId);
        });

        modelBuilder.Entity<AssemblySpec>(entity =>
        {
            entity.HasIndex(e => e.ItemId, "IX_AssemblySpecs_ItemId");

            entity.HasOne(d => d.Item).WithMany(p => p.AssemblySpecItems).HasForeignKey(d => d.ItemId);

            entity.HasOne(d => d.Product).WithMany(p => p.AssemblySpecProducts).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Mark);

            entity.HasIndex(e => e.EquipmentType, "IX_Equipments_EquipmentType");

            entity.HasOne(d => d.EquipmentTypeNavigation).WithMany(p => p.Equipment)
                .HasForeignKey(d => d.EquipmentType);
        });

        modelBuilder.Entity<EquipmentType>(entity => { entity.HasKey(e => e.Name); });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Article);

            entity.HasIndex(e => e.SupplierId, "IX_Materials_SupplierId");

            entity.Property(e => e.Gost).HasColumnName("GOST");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Materials).HasForeignKey(d => d.SupplierId);
        });

        modelBuilder.Entity<MaterialSpec>(entity =>
        {
            entity.HasIndex(e => e.MaterialId, "IX_MaterialSpecs_MaterialId");

            entity.HasOne(d => d.Material).WithMany(p => p.MaterialSpecs).HasForeignKey(d => d.MaterialId);

            entity.HasOne(d => d.Product).WithMany(p => p.MaterialSpecs).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<OperationSpec>(entity =>
        {
            entity.HasIndex(e => e.EquipmentType, "IX_OperationSpecs_EquipmentType");

            entity.HasOne(d => d.EquipmentTypeNavigation).WithMany(p => p.OperationSpecs)
                .HasForeignKey(d => d.EquipmentType);

            entity.HasOne(d => d.Product).WithMany(p => p.OperationSpecs).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

            entity.HasIndex(e => e.ManagerId, "IX_Orders_ManagerId");

            entity.HasIndex(e => e.ProductId, "IX_Orders_ProductId");

            entity.HasOne(d => d.Customer).WithMany(p => p.OrderCustomers).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.Manager).WithMany(p => p.OrderManagers).HasForeignKey(d => d.ManagerId);

            entity.HasOne(d => d.Product).WithMany(p => p.Orders).HasForeignKey(d => d.ProductId);
        });

        modelBuilder.Entity<EmployeeOperation>(entity =>
        {
            entity.HasOne(d => d.Employee).WithMany(p => p.Operations).HasForeignKey(d => d.EmployeeId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}