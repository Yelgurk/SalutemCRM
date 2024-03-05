using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain.Model;
using SalutemCRM.Domain.Modell;
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
    public DbSet<CurrencyUnit> CurrencyUnits { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<WarehouseItem> WarehouseItems { get; set; }
    public DbSet<WarehouseSupply> WarehouseSupplying { get; set; }
    public DbSet<WarehousePayment> WarehousePayments { get; set; }
    public DbSet<WarehouseSale> WarehouseSales { get; set; }
    public DbSet<ProductTemplate> ProductTemplates { get; set; }
    public DbSet<ProductSchema> ProductSchemas { get; set; }
    public DbSet<Manufacture> Manufacture { get; set; }
    public DbSet<ManufacturerDuty> ManufacturerDuties { get; set; }
    public DbSet<ManufactureSupply> ManufactureSupplies { get; set; }
    public DbSet<ManufactureProcess> ManufactureProcesses { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<OfficeOrder> OfficeOrders { get; set; }
    public DbSet<OfficeOrderPayment> OfficeOrderPayments { get; set; }
    public DbSet<CustomerService> CustomerServices { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyUnit>(CurrencyUnit_Config);
        modelBuilder.Entity<UserRole>(UserRole_Config);
        modelBuilder.Entity<User>(User_Config);
        modelBuilder.Entity<WarehouseItem>(WarehouseItem_Config);
        modelBuilder.Entity<WarehouseSupply>(WarehouseSupply_Config);
        modelBuilder.Entity<WarehousePayment>(WarehousePayment_Config);
        modelBuilder.Entity<WarehouseSale>(WarehouseSale_Config);
        modelBuilder.Entity<ProductTemplate>(ProductTemplate_Config);
        modelBuilder.Entity<ProductSchema>(ProductSchema_Config);
        modelBuilder.Entity<Manufacture>(Manufacture_Config);
        modelBuilder.Entity<ManufacturerDuty>(ManufacturerDuty_Config);
        modelBuilder.Entity<ManufactureSupply>(ManufactureSupplies_Config);
        modelBuilder.Entity<ManufactureProcess>(ManufactureProcess_Config);
        modelBuilder.Entity<Client>(Client_Config);
        modelBuilder.Entity<OfficeOrder>(OfficeOrder_Config);
        modelBuilder.Entity<OfficeOrderPayment>(OfficeOrderPayment_Config);
        modelBuilder.Entity<CustomerService>(CustomerService_Config);
    }

    public void CurrencyUnit_Config(EntityTypeBuilder<CurrencyUnit> builder)
    {
        builder.HasKey(ur => ur.Id);
        builder.HasAlternateKey(ur => ur.Name);
    }

    public void UserRole_Config(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.Id);
        builder.HasAlternateKey(ur => ur.Name);
    }

    public void User_Config(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasAlternateKey(u => u.Login);
        builder
            .HasOne(u => u.UserRole)
            .WithMany(ur => ur.Users)
            .HasForeignKey(u => u.UserRoleForeignKey);
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

    public void WarehousePayment_Config(EntityTypeBuilder<WarehousePayment> builder)
    {
        builder.HasKey(wp => wp.Id);
        builder
            .HasOne(wp => wp.WarehouseSupply)
            .WithMany(ws => ws.WarehousePayments)
            .HasForeignKey(wp => wp.WarehouseSupplyForeignKey);
        builder
            .HasOne(wp => wp.CurrencyUnit)
            .WithMany(cu => cu.WarehousePayments)
            .HasForeignKey(wp => wp.CurrencyUnitForeignKey);
        builder
            .Property(wp => wp.UnitToBYNConversion)
            .HasDefaultValue(1);
    }

    public void WarehouseSale_Config(EntityTypeBuilder<WarehouseSale> builder)
    {
        builder.HasKey(ws => ws.Id);
        builder
            .HasOne(ws => ws.OfficeOrder)
            .WithMany(oo => oo.WarehouseSales)
            .HasForeignKey(ws => ws.OfficeOrderForeignKey);
        builder
            .HasOne(wss => wss.WarehouseSupply)
            .WithMany(ws => ws.WarehouseSales)
            .HasForeignKey(wss => wss.WarehouseSupplyForeignKey);
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
            .HasOne(ms => ms.OfficeOrder)
            .WithMany(oo => oo.Manufactures)
            .HasForeignKey(ms => ms.OfficeOrderForeignKey);
        builder
            .Property(ws => ws.DeliveryStatus)
            .HasConversion<int>()
            .HasDefaultValue(Delivery_Status.NotShipped);
        builder
            .Property(ws => ws.TaskStatus)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotAvailable);
    }
    
    public void ManufacturerDuty_Config(EntityTypeBuilder<ManufacturerDuty> builder)
    {
        builder.HasKey(ms => ms.Id);
        builder.HasAlternateKey(ms => ms.Name);
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
    
    public void ManufactureProcess_Config(EntityTypeBuilder<ManufactureProcess> builder)
    {
        builder.HasKey(ms => ms.Id);
        builder
            .HasOne(mp => mp.User)
            .WithMany(u => u.ManufactureProcesses)
            .HasForeignKey(mp => mp.UserForeignKey);
        builder
            .HasOne(mp => mp.Manufacture)
            .WithMany(mf => mf.ManufactureProcesses)
            .HasForeignKey(mp => mp.ManufactureForeignKey);
        builder
            .HasOne(mp => mp.ManufacturerDuty)
            .WithMany(md => md.ManufactureProcesses)
            .HasForeignKey(mp => mp.ManufactureDutyForeignKey);
        builder
            .Property(mp => mp.Status)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotStarted);
    }
    
    public void Client_Config(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
    }
    
    public void OfficeOrder_Config(EntityTypeBuilder<OfficeOrder> builder)
    {
        builder.HasKey(oo => oo.Id);
        builder
            .HasOne(oo => oo.Manager)
            .WithMany(u => u.OfficeOrders)
            .HasForeignKey(oo => oo.ManagerForeignKey);
        builder
            .HasOne(oo => oo.Client)
            .WithMany(c => c.OfficeOrders)
            .HasForeignKey(oo => oo.ClientForeignKey);
        builder
            .Property(oo => oo.PaymentAgreement)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.HalfPaid);
        builder
            .Property(oo => oo.PaymentStatus)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.Unpaid);
    }

    public void OfficeOrderPayment_Config(EntityTypeBuilder<OfficeOrderPayment> builder)
    {
        builder.HasKey(oop => oop.Id);
        builder
            .HasOne(oop => oop.OfficeOrder)
            .WithMany(oo => oo.OfficeOrderPayments)
            .HasForeignKey(oop => oop.OfficeOrderForeignKey);
        builder
            .HasOne(oop => oop.CurrencyUnit)
            .WithMany(cu => cu.OfficeOrderPayments)
            .HasForeignKey(oop => oop.CurrencyUnitForeignKey);
        builder
            .Property(oop => oop.UnitToBYNConversion)
            .HasDefaultValue(1);
    }

    public void CustomerService_Config(EntityTypeBuilder<CustomerService> builder)
    {
        builder.HasKey(oop => oop.Id);
        builder
            .HasOne(cs => cs.Employee)
            .WithMany(u => u.CustomerServices)
            .HasForeignKey(cs => cs.EmployeeForeignKey);
        builder
            .HasOne(cs => cs.Manufacture)
            .WithMany(mf => mf.CustomerServices)
            .HasForeignKey(cs => cs.ManufactureForeignKey);
        builder
            .Property(cs => cs.Status)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotStarted);
    }
}