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
    public DbSet<ProductTemplate> ProductTemplates { get; set; }
    public DbSet<ProductSchema> ProductSchemas { get; set; }
    public DbSet<Manufacture> Manufacture { get; set; }
    public DbSet<ManufacturerDuty> ManufacturerDuties { get; set; }
    public DbSet<MaterialFlow> MaterialFlow { get; set; }
    public DbSet<ManufactureProcess> ManufactureProcesses { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<OfficeOrder> OfficeOrders { get; set; }
    public DbSet<WarehouseOrder> WarehouseOrders { get; set; }
    public DbSet<CustomerService> CustomerServices { get; set; }
    public DbSet<CustomerServiceOrder> CustomerServiceOrders { get; set; }
    public DbSet<Payment> Payments { get; set; }

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
        modelBuilder.Entity<ProductTemplate>(ProductTemplate_Config);
        modelBuilder.Entity<ProductSchema>(ProductSchema_Config);
        modelBuilder.Entity<Manufacture>(Manufacture_Config);
        modelBuilder.Entity<ManufacturerDuty>(ManufacturerDuty_Config);
        modelBuilder.Entity<MaterialFlow>(MaterialFlow_Config);
        modelBuilder.Entity<ManufactureProcess>(ManufactureProcess_Config);
        modelBuilder.Entity<Client>(Client_Config);
        modelBuilder.Entity<Vendor>(Vendor_Config);
        modelBuilder.Entity<OfficeOrder>(OfficeOrder_Config);
        modelBuilder.Entity<WarehouseOrder>(WarehouseOrder_Config);
        modelBuilder.Entity<CustomerService>(CustomerService_Config);
        modelBuilder.Entity<CustomerServiceOrder>(CustomerServiceOrder_Config);
        modelBuilder.Entity<Payment>(Payment_Config);
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
        builder
            .Property(wi => wi.CountRequired)
            .HasDefaultValue(0.0);
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

    public void MaterialFlow_Config(EntityTypeBuilder<MaterialFlow> builder)
    {
        builder.HasKey(mf => mf.Id);
        builder
            .HasOne(ms => ms.WarehouseSupply)
            .WithMany(ws => ws.MaterialFlows)
            .HasForeignKey(ms => ms.WarehouseSupplyForeignKey);
        builder
            .HasOne(mf => mf.Manufacture)
            .WithMany(m => m.MaterialFlows)
            .HasForeignKey(ms => ms.ManufactureForeignKey);
        builder
            .HasOne(mf => mf.OfficeOrder)
            .WithMany(oo => oo.MaterialFlows)
            .HasForeignKey(ms => ms.OfficeOrderForeignKey);
        builder
            .HasOne(mf => mf.CustomerServiceOrder)
            .WithMany(cso => cso.MaterialFlows)
            .HasForeignKey(ms => ms.CustomerServiceForeignKey);
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
            .Property(mp => mp.TaskStatus)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotStarted);
    }
    
    public void Client_Config(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
    }
    
    public void Vendor_Config(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(v => v.Id);
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
            .Property(oo => oo.OrderType)
            .HasConversion<int>();
        builder
            .Property(oo => oo.PaymentAgreement)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.HalfPaid);
        builder
            .Property(oo => oo.PaymentStatus)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.Unpaid);
    }
    
    public void WarehouseOrder_Config(EntityTypeBuilder<WarehouseOrder> builder)
    {
        builder.HasKey(wo => wo.Id);
        builder
            .HasOne(wo => wo.Storekeeper)
            .WithMany(u => u.WarehouseOrders)
            .HasForeignKey(wo => wo.StorekeeperForeignKey);
        builder
            .HasOne(wo => wo.Vendor)
            .WithMany(v => v.WarehouseOrders)
            .HasForeignKey(wo => wo.VendorForeignKey);
        builder
            .Property(wo => wo.PaymentAgreement)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.FullyPaid);
        builder
            .Property(wo => wo.PaymentStatus)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.Unpaid);
    }

    public void CustomerService_Config(EntityTypeBuilder<CustomerService> builder)
    {
        builder.HasKey(oop => oop.Id);
        builder
            .HasOne(cs => cs.CustomerServiceOrder)
            .WithMany(cso => cso.CustomerServices)
            .HasForeignKey(cs => cs.CustomerServiceOrderForeignKey);
        builder
            .Property(cs => cs.TaskStatus)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotStarted);
    }
    
    public void CustomerServiceOrder_Config(EntityTypeBuilder<CustomerServiceOrder> builder)
    {
        builder.HasKey(cso => cso.Id);
        builder
            .HasOne(cso => cso.StockManager)
            .WithMany(u => u.CustomerServiceOrders)
            .HasForeignKey(cso => cso.StockManagerForeignKey);
        builder
            .HasOne(cso => cso.Manufacture)
            .WithMany(mf => mf.CustomerServiceOrders)
            .HasForeignKey(cso => cso.ManufactureForeignKey);
        builder
            .Property(cso => cso.PaymentAgreement)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.FullyPaid);
        builder
            .Property(cso => cso.PaymentStatus)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.Unpaid);
        builder
            .Property(cso => cso.TaskStatus)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotStarted);
    }

    public void Payment_Config(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(moneyzzz => moneyzzz.Id);
        builder
            .HasOne(moneyzzz => moneyzzz.WarehouseOrder)
            .WithMany(ws => ws.Payments)
            .HasForeignKey(moneyzzz => moneyzzz.WarehouseOrderForeignKey);
        builder
            .HasOne(moneyzzz => moneyzzz.OfficeOrder)
            .WithMany(oo => oo.Payments)
            .HasForeignKey(moneyzzz => moneyzzz.OfficeOrderForeignKey);
        builder
            .Property(oop => oop.UnitToBYNConversion)
            .HasDefaultValue(1);
    }

}