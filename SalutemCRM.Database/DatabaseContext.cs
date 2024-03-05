using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SalutemCRM.Database;

public partial class DatabaseContext : DbContext
{
    public DbSet<WarehouseItem> WarehouseItems { get; set; }
    public DbSet<WarehouseSupply> WarehouseSupplying { get; set; }
    public DbSet<ProductTemplate> ProductTemplates { get; set; }
    public DbSet<ProductSchema> ProductSchemas { get; set; }
    public DbSet<Manufacture> Manufacture { get; set; }
    public DbSet<ManufactureSupply> ManufactureSupplies { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WarehouseItem>(WarehouseItem_Config);
        modelBuilder.Entity<WarehouseSupply>(WarehouseSupply_Config);
        modelBuilder.Entity<ProductTemplate>(ProductTemplate_Config);
        modelBuilder.Entity<ProductSchema>(ProductSchema_Config);
        modelBuilder.Entity<Manufacture>(Manufacture_Config);
        modelBuilder.Entity<ManufactureSupply>(ManufactureSupplies_Config);
    }

    public void WarehouseItem_Config(EntityTypeBuilder<WarehouseItem> builder)
    {
        builder.HasKey(wi => wi.Id);
        builder.HasAlternateKey(wi => wi.Code);
    }

    public void WarehouseSupply_Config(EntityTypeBuilder<WarehouseSupply> builder)
    {
        builder.HasKey(ws => ws.Id);
        builder
            .HasOne(ws => ws.WarehouseItem)
            .WithMany(wi => wi.WarehouseSupplying)
            .HasForeignKey(ws => ws.WarehouseItemForeignKey);
        builder
            .Property(ws => ws.PaymentStatus)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.Unpaid);
        builder
            .Property(ws => ws.DeliveryStatus)
            .HasConversion<int>()
            .HasDefaultValue(Delivery_Status.NotShipped);
    }

    public void ProductTemplate_Config(EntityTypeBuilder<ProductTemplate> builder)
    {
        builder.HasKey(pt => pt.Id);
        builder.HasAlternateKey(pt => pt.Name);
    }

    public void ProductSchema_Config(EntityTypeBuilder<ProductSchema> builder)
    {
        builder.HasKey(ps => ps.Id);
        builder
            .HasOne(ps => ps.ProductTemplate)
            .WithMany(pt => pt.ProductSchemas)
            .HasForeignKey(ps => ps.ProductTemplateForeignKey);
        builder
            .HasOne(ps => ps.WarehouseItem)
            .WithMany(ws => ws.ProductSchemas)
            .HasForeignKey(ps => ps.WarehouseItemForeignKey);
    }

    public void Manufacture_Config(EntityTypeBuilder<Manufacture> builder)
    {
        builder.HasKey(ms => ms.Id);
        builder.HasAlternateKey(ms => ms.Code);
        builder
            .HasOne(ms => ms.ProductTemplate)
            .WithMany(pt => pt.Manufactures)
            .HasForeignKey(ms => ms.ProductTemplateForeignKey);
        builder
            .Property(ws => ws.DeliveryStatus)
            .HasConversion<int>()
            .HasDefaultValue(Delivery_Status.NotShipped);
        builder
            .Property(ws => ws.TaskStatus)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotStarted);
    }

    public void ManufactureSupplies_Config(EntityTypeBuilder<ManufactureSupply> builder)
    {
        builder.HasKey(ms => ms.Id);
        builder
            .HasOne(ms => ms.Manufacture)
            .WithMany(mf => mf.ManufactureSupplies)
            .HasForeignKey(ms => ms.ManufactureForeignKey);
        builder
            .HasOne(ms => ms.WarehouseSupply)
            .WithMany(ws => ws.ManufactureSupplies)
            .HasForeignKey(ms => ms.WarehouseSupplyForeignKey);
    }
}