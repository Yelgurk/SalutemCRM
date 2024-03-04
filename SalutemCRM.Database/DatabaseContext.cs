using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Database;

public partial class DatabaseContext : DbContext
{
    public DbSet<WarehouseItem> WarehouseItems { get; set; }
    public DbSet<WarehouseSupply> WarehouseSupplying { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WarehouseItem>().HasKey(wi => wi.Id);

        modelBuilder.Entity<WarehouseSupply>().HasKey(ws => ws.Id);
        modelBuilder.Entity<WarehouseSupply>()
            .HasOne(ws => ws.WarehouseItem)
            .WithMany(wi => wi.WarehouseSupplying)
            .HasForeignKey(ws => ws.WarehouseItemForeignKey);
        modelBuilder.Entity<WarehouseSupply>()
            .Property(ws => ws.PaymentStatus)
            .HasConversion<int>();
        modelBuilder.Entity<WarehouseSupply>()
            .Property(ws => ws.DeliveryStatus)
            .HasConversion<int>();
    }
}