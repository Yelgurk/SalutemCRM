using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain;
using SalutemCRM.Domain.Model;
using SalutemCRM.Domain.Modell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SalutemCRM.Database;

public partial class DatabaseContext : DbContext
{
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<FileAttach> FileAttachs { get; set; }
    public DbSet<WarehouseCategory> WarehouseCategories { get; set; }
    public DbSet<WarehouseItem> WarehouseItems { get; set; }
    public DbSet<WarehouseSupply> WarehouseSupplying { get; set; }
    public DbSet<WarehouseOrder> WarehouseOrders { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductTemplate> ProductTemplates { get; set; }
    public DbSet<ProductSchema> ProductSchemas { get; set; }
    public DbSet<ManufacturerDuty> ManufacturerDuties { get; set; }
    public DbSet<ManufactureProcess> ManufactureProcesses { get; set; }
    public DbSet<Manufacture> Manufacture { get; set; }
    public DbSet<MaterialFlow> MaterialFlow { get; set; }
    public DbSet<OfficeOrder> OfficeOrders { get; set; }
    public DbSet<CustomerService> CustomerServices { get; set; }
    public DbSet<CustomerServiceOrder> CustomerServiceOrders { get; set; }
    public DbSet<CurrencyUnit> CurrencyUnits { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Logging> Logging { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => base.OnConfiguring(optionsBuilder);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<ObservableObject>();

        modelBuilder.Entity<UserRole>(UserRole_Config);
        modelBuilder.Entity<User>(User_Config);
        modelBuilder.Entity<Client>(Client_Config);
        modelBuilder.Entity<Vendor>(Vendor_Config);
        modelBuilder.Entity<FileAttach>(FileAttach_Config);
        modelBuilder.Entity<WarehouseCategory>(WarehouseCategory_Config);
        modelBuilder.Entity<WarehouseItem>(WarehouseItem_Config);
        modelBuilder.Entity<WarehouseSupply>(WarehouseSupply_Config);
        modelBuilder.Entity<WarehouseOrder>(WarehouseOrder_Config);
        modelBuilder.Entity<ProductCategory>(ProductCategory_Config);
        modelBuilder.Entity<ProductTemplate>(ProductTemplate_Config);
        modelBuilder.Entity<ProductSchema>(ProductSchema_Config);
        modelBuilder.Entity<ManufacturerDuty>(ManufacturerDuty_Config);
        modelBuilder.Entity<ManufactureProcess>(ManufactureProcess_Config);
        modelBuilder.Entity<Manufacture>(Manufacture_Config);
        modelBuilder.Entity<MaterialFlow>(MaterialFlow_Config);
        modelBuilder.Entity<OfficeOrder>(OfficeOrder_Config);
        modelBuilder.Entity<CustomerService>(CustomerService_Config);
        modelBuilder.Entity<CustomerServiceOrder>(CustomerServiceOrder_Config);
        modelBuilder.Entity<CurrencyUnit>(CurrencyUnit_Config);
        modelBuilder.Entity<Payment>(Payment_Config);
        modelBuilder.Entity<Logging>(Logging_Config);

        DemoModelCreating(modelBuilder);
    }

    private void DemoModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CurrencyUnit>().HasData(
            new CurrencyUnit { Id = 1, Name = "BYN" },    
            new CurrencyUnit { Id = 2, Name = "USD" },
            new CurrencyUnit { Id = 3, Name = "RUB" },
            new CurrencyUnit { Id = 4, Name = "CNY" }
        );

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { Id = 1, Name = "Руководитель" },
            new UserRole { Id = 2, Name = "Бухгалтер" },
            new UserRole { Id = 3, Name = "Гл. менеджер" },
            new UserRole { Id = 4, Name = "Менеджер" },
            new UserRole { Id = 5, Name = "Гл. производства" },
            new UserRole { Id = 6, Name = "Конструктор" },
            new UserRole { Id = 7, Name = "Производство" },
            new UserRole { Id = 8, Name = "Кладовщик" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, UserRoleForeignKey = 1, IsActive = true, Login = "log11", PasswordMD5 = "pass11", FirstName = "Руководитель", LastName = "Xxx" },
            new User { Id = 2, UserRoleForeignKey = 2, IsActive = true, Login = "log21", PasswordMD5 = "pass21", FirstName = "Бухгалтер", LastName = "111" },
            new User { Id = 3, UserRoleForeignKey = 2, IsActive = true, Login = "log22", PasswordMD5 = "pass22", FirstName = "Бухгалтер", LastName = "222" },
            new User { Id = 4, UserRoleForeignKey = 3, IsActive = true, Login = "log31", PasswordMD5 = "pass31", FirstName = "Гл. менеджер", LastName = "111" },
            new User { Id = 5, UserRoleForeignKey = 4, IsActive = true, Login = "log41", PasswordMD5 = "pass41", FirstName = "Менеджер", LastName = "111" },
            new User { Id = 6, UserRoleForeignKey = 4, IsActive = true, Login = "log42", PasswordMD5 = "pass42", FirstName = "Менеджер", LastName = "222" },
            new User { Id = 7, UserRoleForeignKey = 4, IsActive = true, Login = "log43", PasswordMD5 = "pass43", FirstName = "Менеджер", LastName = "333" },
            new User { Id = 8, UserRoleForeignKey = 4, IsActive = true, Login = "log44", PasswordMD5 = "pass44", FirstName = "Менеджер", LastName = "444" },
            new User { Id = 9, UserRoleForeignKey = 5, IsActive = true, Login = "log51", PasswordMD5 = "pass51", FirstName = "Гл. производства", LastName = "111" },
            new User { Id = 10, UserRoleForeignKey = 6, IsActive = true, Login = "log61", PasswordMD5 = "pass61", FirstName = "Конструктор", LastName = "111" },
            new User { Id = 11, UserRoleForeignKey = 7, IsActive = true, Login = "log71", PasswordMD5 = "pass71", FirstName = "Производство", LastName = "Сварщик" },
            new User { Id = 12, UserRoleForeignKey = 7, IsActive = true, Login = "log72", PasswordMD5 = "pass72", FirstName = "Производство", LastName = "Электрик" },
            new User { Id = 13, UserRoleForeignKey = 7, IsActive = true, Login = "log73", PasswordMD5 = "pass73", FirstName = "Производство", LastName = "Полимер" },
            new User { Id = 14, UserRoleForeignKey = 7, IsActive = true, Login = "log74", PasswordMD5 = "pass74", FirstName = "Производство", LastName = "Токарь" },
            new User { Id = 15, UserRoleForeignKey = 8, IsActive = true, Login = "log81", PasswordMD5 = "pass81", FirstName = "Кладовщик", LastName = "111" }
        );

        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, Name = "ЗАО \"МИНСКОЕ\"", Address = "Беларусь, Минск, ул. ***, **" },
            new Client { Id = 2, Name = "ЗАО \"БРЕСТСКОЕ\"", Address = "Беларусь, Брест, ул. ***, **" },
            new Client { Id = 3, Name = "ЗАО \"ВИТЕБСКОЕ\"", Address = "Беларусь, Витебск, ул. ***, **" },
            new Client { Id = 4, Name = "ЗАО \"ГРОДНЕНСКОЕ\"", Address = "Беларусь, Гродно, ул. ***, **" },
            new Client { Id = 5, Name = "ЗАО \"ГОМЕЛЬСКОЕ\"", Address = "Беларусь, Гомель, ул. ***, **" },
            new Client { Id = 6, Name = "ЗАО \"МОГИЛЕВСКОЕ\"", Address = "Беларусь, Могилев, ул. ***, **" }
        );

        modelBuilder.Entity<Vendor>().HasData(
            new Client { Id = 1, Name = "АО \"СЕЛЬМАШ_МИН\"", Address = "Беларусь, Минск, ул. ***, **" },
            new Client { Id = 2, Name = "АО \"СЕЛЬМАШ_БРЕ\"", Address = "Беларусь, Брест, ул. ***, **" },
            new Client { Id = 3, Name = "АО \"СЕЛЬМАШ_ВИТ\"", Address = "Беларусь, Витебск, ул. ***, **" },
            new Client { Id = 4, Name = "АО \"СЕЛЬМАШ_ГРО\"", Address = "Беларусь, Гродно, ул. ***, **" },
            new Client { Id = 5, Name = "АО \"СЕЛЬМАШ_ГОМ\"", Address = "Беларусь, Гомель, ул. ***, **" },
            new Client { Id = 6, Name = "АО \"СЕЛЬМАШ_МОГ\"", Address = "Беларусь, Могилев, ул. ***, **" }
        );

        //modelBuilder.Entity<FileAttach>();

        modelBuilder.Entity<WarehouseCategory>().HasData(
            new WarehouseCategory { Id = 1, Deep = 0, Name = "Электрика" },
            new WarehouseCategory { Id = 2, Deep = 0, Name = "Мелочевка" },
            new WarehouseCategory { Id = 3, Deep = 0, Name = "Металл" },
            new WarehouseCategory { Id = 4, Deep = 1, Name = "ПЛК", ParentCategoryForeignKey = 1 },
            new WarehouseCategory { Id = 5, Deep = 1, Name = "Привода", ParentCategoryForeignKey = 1 },
            new WarehouseCategory { Id = 6, Deep = 1, Name = "Насосы", ParentCategoryForeignKey = 1 },
            new WarehouseCategory { Id = 7, Deep = 1, Name = "Клеммы", ParentCategoryForeignKey = 2 },
            new WarehouseCategory { Id = 8, Deep = 1, Name = "Кнопки", ParentCategoryForeignKey = 2 },
            new WarehouseCategory { Id = 9, Deep = 1, Name = "Реле", ParentCategoryForeignKey = 2 },
            new WarehouseCategory { Id = 10, Deep = 1, Name = "Для ПС", ParentCategoryForeignKey = 3 },
            new WarehouseCategory { Id = 11, Deep = 1, Name = "Для РМ", ParentCategoryForeignKey = 3 },
            new WarehouseCategory { Id = 12, Deep = 1, Name = "Для ТМ", ParentCategoryForeignKey = 3 },
            new WarehouseCategory { Id = 13, Deep = 2, Name = "Собственное производство", ParentCategoryForeignKey = 4 },
            new WarehouseCategory { Id = 14, Deep = 2, Name = "Импортное", ParentCategoryForeignKey = 4 },
            new WarehouseCategory { Id = 15, Deep = 2, Name = "Экспортное", ParentCategoryForeignKey = 4 }
        );

        modelBuilder.Entity<WarehouseItem>().HasData(
            new WarehouseItem { Id = 1, Name = "Кнопка поворотная", Code = "a0001", CountRequired = 0, WarehouseCategoryForeignKey = null },
            new WarehouseItem { Id = 2, Name = "Кнопка рычажная", Code = "a0002", CountRequired = 0, WarehouseCategoryForeignKey = 8 },
            new WarehouseItem { Id = 3, Name = "Кнопка тактовая", Code = "a0003", CountRequired = 0, WarehouseCategoryForeignKey = 8 },
            new WarehouseItem { Id = 4, Name = "Насос 35лм", Code = "a0004", CountRequired = 0, WarehouseCategoryForeignKey = 6 },
            new WarehouseItem { Id = 5, Name = "Насос мембранный 45лм", Code = "a0005", CountRequired = 0, WarehouseCategoryForeignKey = 6 },
            new WarehouseItem { Id = 6, Name = "Привод одноколесный", Code = "a0006", CountRequired = 0, WarehouseCategoryForeignKey = 5 },
            new WarehouseItem { Id = 7, Name = "Привод двухколесный", Code = "a0007", CountRequired = 0, WarehouseCategoryForeignKey = 5 },
            new WarehouseItem { Id = 8, Name = "Клемма обжимная 1.5мм", Code = "a0008", CountRequired = 0, WarehouseCategoryForeignKey = null },
            new WarehouseItem { Id = 9, Name = "Клемма обжимная 2.0мм", Code = "a0009", CountRequired = 0, WarehouseCategoryForeignKey = 7 },
            new WarehouseItem { Id = 10, Name = "Клемма 5.08 EDG", Code = "a0010", CountRequired = 0, WarehouseCategoryForeignKey = 2 },
            new WarehouseItem { Id = 11, Name = "Реле рейловое 220В", Code = "a0011", CountRequired = 0, WarehouseCategoryForeignKey = 9 },
            new WarehouseItem { Id = 12, Name = "Реле пусковое 48в dc", Code = "a0012", CountRequired = 0, WarehouseCategoryForeignKey = 1 },
            new WarehouseItem { Id = 13, Name = "FX3U", Code = "a0013", CountRequired = 0, WarehouseCategoryForeignKey = 4 },
            new WarehouseItem { Id = 14, Name = "OP280", Code = "a0014", CountRequired = 0, WarehouseCategoryForeignKey = 4 },
            new WarehouseItem { Id = 15, Name = "SSPS3R3", Code = "a0015", CountRequired = 0, WarehouseCategoryForeignKey = 4 },
            new WarehouseItem { Id = 16, Name = "Обичевка пс100", Code = "a0016", CountRequired = 0, WarehouseCategoryForeignKey = 10 },
            new WarehouseItem { Id = 17, Name = "Обичевка пс200", Code = "a0017", CountRequired = 0, WarehouseCategoryForeignKey = 10 },
            new WarehouseItem { Id = 18, Name = "Обичевка пс300", Code = "a0018", CountRequired = 0, WarehouseCategoryForeignKey = 3 },
            new WarehouseItem { Id = 19, Name = "Обичевка рм25", Code = "a0019", CountRequired = 0, WarehouseCategoryForeignKey = null },
            new WarehouseItem { Id = 20, Name = "Обичевка рм45", Code = "a0020", CountRequired = 0, WarehouseCategoryForeignKey = 11 },
            new WarehouseItem { Id = 21, Name = "Обичевка рм-lite", Code = "a0021", CountRequired = 0, WarehouseCategoryForeignKey = 3 },
            new WarehouseItem { Id = 22, Name = "Обичевка тм100", Code = "a0022", CountRequired = 0, WarehouseCategoryForeignKey = 12 },
            new WarehouseItem { Id = 23, Name = "Обичевка тм100", Code = "a0023", CountRequired = 0, WarehouseCategoryForeignKey = 12 },
            new WarehouseItem { Id = 24, Name = "Обичевка тм100", Code = "a0024", CountRequired = 0, WarehouseCategoryForeignKey = 12 }
        );

        modelBuilder.Entity<WarehouseOrder>().HasData(
            new WarehouseOrder {
                Id = 1,
                StorekeeperForeignKey = 9,
                VendorForeignKey = 1,
                AdditionalInfo = "Загрузка склада при интеграции приложения",
                Currency = "BYN",
                UnitToBYNConversion = 1.0,
                PaymentAgreement = Payment_Status.FullyPaid,
                PaymentStatus = Payment_Status.FullyPaid,
                PriceRequired = 0,
                PriceTotal = 0,
                RecordDT = DateTime.Now,
                ShipmentDeadlineDT = DateTime.Now.AddDays(2),
                ReceivedDT = DateTime.Now
            }    
        );

        modelBuilder.Entity<WarehouseSupply>().HasData(
            new WarehouseSupply { Id = 1, WarehouseItemForeignKey = 1, WarehouseOrderForeignKey = 1, Code = "x0001", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 2, WarehouseItemForeignKey = 2, WarehouseOrderForeignKey = 1, Code = "x0002", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 3, WarehouseItemForeignKey = 3, WarehouseOrderForeignKey = 1, Code = "x0003", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 4, WarehouseItemForeignKey = 4, WarehouseOrderForeignKey = 1, Code = "x0004", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 5, WarehouseItemForeignKey = 5, WarehouseOrderForeignKey = 1, Code = "x0005", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 6, WarehouseItemForeignKey = 6, WarehouseOrderForeignKey = 1, Code = "x0006", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 7, WarehouseItemForeignKey = 7, WarehouseOrderForeignKey = 1, Code = "x0007", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 8, WarehouseItemForeignKey = 8, WarehouseOrderForeignKey = 1, Code = "x0008", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 9, WarehouseItemForeignKey = 9, WarehouseOrderForeignKey = 1, Code = "x0009", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 10, WarehouseItemForeignKey = 10, WarehouseOrderForeignKey = 1, Code = "x0010", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 11, WarehouseItemForeignKey = 11, WarehouseOrderForeignKey = 1, Code = "x0011", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 12, WarehouseItemForeignKey = 12, WarehouseOrderForeignKey = 1, Code = "x0012", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 13, WarehouseItemForeignKey = 13, WarehouseOrderForeignKey = 1, Code = "x0013", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 14, WarehouseItemForeignKey = 14, WarehouseOrderForeignKey = 1, Code = "x0014", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 15, WarehouseItemForeignKey = 15, WarehouseOrderForeignKey = 1, Code = "x0015", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 16, WarehouseItemForeignKey = 16, WarehouseOrderForeignKey = 1, Code = "x0016", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 17, WarehouseItemForeignKey = 17, WarehouseOrderForeignKey = 1, Code = "x0017", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 18, WarehouseItemForeignKey = 18, WarehouseOrderForeignKey = 1, Code = "x0018", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 19, WarehouseItemForeignKey = 19, WarehouseOrderForeignKey = 1, Code = "x0019", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 20, WarehouseItemForeignKey = 20, WarehouseOrderForeignKey = 1, Code = "x0020", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 21, WarehouseItemForeignKey = 21, WarehouseOrderForeignKey = 1, Code = "x0021", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 22, WarehouseItemForeignKey = 22, WarehouseOrderForeignKey = 1, Code = "x0022", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 23, WarehouseItemForeignKey = 23, WarehouseOrderForeignKey = 1, Code = "x0023", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 24, WarehouseItemForeignKey = 24, WarehouseOrderForeignKey = 1, Code = "x0024", PriceRequired = 100.0, PriceTotal = 100.0, Count = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 25, WarehouseItemForeignKey = 1, WarehouseOrderForeignKey = 1, Code = "z0001", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 26, WarehouseItemForeignKey = 2, WarehouseOrderForeignKey = 1, Code = "z0002", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 27, WarehouseItemForeignKey = 3, WarehouseOrderForeignKey = 1, Code = "z0003", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 28, WarehouseItemForeignKey = 4, WarehouseOrderForeignKey = 1, Code = "z0004", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 29, WarehouseItemForeignKey = 5, WarehouseOrderForeignKey = 1, Code = "z0005", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 30, WarehouseItemForeignKey = 6, WarehouseOrderForeignKey = 1, Code = "z0006", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 31, WarehouseItemForeignKey = 7, WarehouseOrderForeignKey = 1, Code = "z0007", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 32, WarehouseItemForeignKey = 8, WarehouseOrderForeignKey = 1, Code = "z0008", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 33, WarehouseItemForeignKey = 9, WarehouseOrderForeignKey = 1, Code = "z0009", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 34, WarehouseItemForeignKey = 10, WarehouseOrderForeignKey = 1, Code = "z0010", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 35, WarehouseItemForeignKey = 11, WarehouseOrderForeignKey = 1, Code = "z0011", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 36, WarehouseItemForeignKey = 12, WarehouseOrderForeignKey = 1, Code = "z0012", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 37, WarehouseItemForeignKey = 13, WarehouseOrderForeignKey = 1, Code = "z0013", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 38, WarehouseItemForeignKey = 14, WarehouseOrderForeignKey = 1, Code = "z0014", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 39, WarehouseItemForeignKey = 15, WarehouseOrderForeignKey = 1, Code = "z0015", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 40, WarehouseItemForeignKey = 16, WarehouseOrderForeignKey = 1, Code = "z0016", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 41, WarehouseItemForeignKey = 17, WarehouseOrderForeignKey = 1, Code = "z0017", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 42, WarehouseItemForeignKey = 18, WarehouseOrderForeignKey = 1, Code = "z0018", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 43, WarehouseItemForeignKey = 19, WarehouseOrderForeignKey = 1, Code = "z0019", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 44, WarehouseItemForeignKey = 20, WarehouseOrderForeignKey = 1, Code = "z0020", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 45, WarehouseItemForeignKey = 21, WarehouseOrderForeignKey = 1, Code = "z0021", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 46, WarehouseItemForeignKey = 22, WarehouseOrderForeignKey = 1, Code = "z0022", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 47, WarehouseItemForeignKey = 23, WarehouseOrderForeignKey = 1, Code = "z0023", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 48, WarehouseItemForeignKey = 24, WarehouseOrderForeignKey = 1, Code = "z0024", PriceRequired = 100.0, PriceTotal = 100.0, Count = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered }
        );

        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { Id = 1, Deep = 0, Name = "Пастеризатор" },
            new ProductCategory { Id = 2, Deep = 0, Name = "Такси" },
            new ProductCategory { Id = 3, Deep = 0, Name = "Размораживатель" },
            new ProductCategory { Id = 4, Deep = 0, Name = "СМОМ" },
            new ProductCategory { Id = 5, Deep = 0, Name = "Боксы" }
        );

        modelBuilder.Entity<ProductTemplate>().HasData(
            new ProductTemplate { Id = 1, Name = "ПС", Model = "100", ManufactureCategoryForeignKey = 1, AdditionalInfo = "" },    
            new ProductTemplate { Id = 2, Name = "ПС", Model = "200", ManufactureCategoryForeignKey = 1, AdditionalInfo = "" },    
            new ProductTemplate { Id = 3, Name = "ПС", Model = "300", ManufactureCategoryForeignKey = 1, AdditionalInfo = "" },    
            new ProductTemplate { Id = 4, Name = "ПС", Model = "400", ManufactureCategoryForeignKey = 1, AdditionalInfo = "" },    
            new ProductTemplate { Id = 5, Name = "ПС", Model = "500", ManufactureCategoryForeignKey = 1, AdditionalInfo = "" },    
            new ProductTemplate { Id = 6, Name = "ТМ", Model = "100", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 7, Name = "ТМ", Model = "200", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 8, Name = "ТМ", Model = "300", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 9, Name = "ТМ", Model = "400", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 10, Name = "ТМП", Model = "200", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 11, Name = "ТМП", Model = "300", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 12, Name = "ТМП", Model = "400", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 13, Name = "ТМПЭ", Model = "200", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 14, Name = "ТМПЭ", Model = "300", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 15, Name = "ТМПЭ", Model = "400", ManufactureCategoryForeignKey = 2, AdditionalInfo = "" },    
            new ProductTemplate { Id = 16, Name = "РМ", Model = "25", ManufactureCategoryForeignKey = 3, AdditionalInfo = "" },    
            new ProductTemplate { Id = 17, Name = "РМ", Model = "45", ManufactureCategoryForeignKey = 3, AdditionalInfo = "" },    
            new ProductTemplate { Id = 18, Name = "РМ-lite", Model = "lite", ManufactureCategoryForeignKey = 3, AdditionalInfo = "" },    
            new ProductTemplate { Id = 19, Name = "СМОМ", Model = "1000", ManufactureCategoryForeignKey = 4, AdditionalInfo = "" },    
            new ProductTemplate { Id = 20, Name = "СМОМ", Model = "2000", ManufactureCategoryForeignKey = 4, AdditionalInfo = "" },    
            new ProductTemplate { Id = 21, Name = "Бокс", Model = "2м*3м", ManufactureCategoryForeignKey = 5, AdditionalInfo = "" },    
            new ProductTemplate { Id = 22, Name = "Бокс", Model = "2м*3.5м", ManufactureCategoryForeignKey = 5, AdditionalInfo = "" },    
            new ProductTemplate { Id = 23, Name = "Бокс", Model = "3м*3.5м", ManufactureCategoryForeignKey = 5, AdditionalInfo = "" },    
            new ProductTemplate { Id = 24, Name = "Бокс", Model = "3.5м*4м", ManufactureCategoryForeignKey = 5, AdditionalInfo = "" }    
        );

        modelBuilder.Entity<ProductSchema>().HasData(
            new ProductSchema { Id = 1, ProductTemplateForeignKey = 1, WarehouseItemForeignKey = 1, Count = 4 },    
            new ProductSchema { Id = 2, ProductTemplateForeignKey = 1, WarehouseItemForeignKey = 8, Count = 10 },    
            new ProductSchema { Id = 3, ProductTemplateForeignKey = 1, WarehouseItemForeignKey = 13, Count = 1 },    
            new ProductSchema { Id = 4, ProductTemplateForeignKey = 1, WarehouseItemForeignKey = 16, Count = 1 },    
            new ProductSchema { Id = 5, ProductTemplateForeignKey = 6, WarehouseItemForeignKey = 1, Count = 4 },    
            new ProductSchema { Id = 6, ProductTemplateForeignKey = 6, WarehouseItemForeignKey = 8, Count = 10 },    
            new ProductSchema { Id = 7, ProductTemplateForeignKey = 6, WarehouseItemForeignKey = 13, Count = 1 },    
            new ProductSchema { Id = 8, ProductTemplateForeignKey = 6, WarehouseItemForeignKey = 22, Count = 1 },    
            new ProductSchema { Id = 9, ProductTemplateForeignKey = 16, WarehouseItemForeignKey = 1, Count = 2 },    
            new ProductSchema { Id = 10, ProductTemplateForeignKey = 16, WarehouseItemForeignKey = 8, Count = 8 },    
            new ProductSchema { Id = 11, ProductTemplateForeignKey = 16, WarehouseItemForeignKey = 13, Count = 1 },    
            new ProductSchema { Id = 12, ProductTemplateForeignKey = 16, WarehouseItemForeignKey = 19, Count = 1 }    
        );

        modelBuilder.Entity<ManufacturerDuty>();
        modelBuilder.Entity<ManufactureProcess>();
        modelBuilder.Entity<Manufacture>();
        modelBuilder.Entity<MaterialFlow>();
        modelBuilder.Entity<OfficeOrder>();
        modelBuilder.Entity<CustomerService>();
        modelBuilder.Entity<CustomerServiceOrder>();
        modelBuilder.Entity<Payment>();
    }

    public void UserRole_Config(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.Id);
        builder
            .Property(ur => ur.Id)
            .ValueGeneratedOnAdd();
        builder.HasAlternateKey(ur => ur.Name);
        builder
            .Property(ur => ur.Name)
            .HasMaxLength(200);
    }

    public void User_Config(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder
            .Property(u => u.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(u => u.UserRole)
            .WithMany(ur => ur.Users)
            .HasForeignKey(u => u.UserRoleForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasAlternateKey(u => u.Login);
        builder
            .Property(u => u.Login)
            .HasMaxLength(200);
        builder
            .Property(u => u.IsActive)
            .HasDefaultValue(true);
        builder
            .Property(u => u.PasswordMD5)
            .HasMaxLength(200);
        builder
            .Property(u => u.FirstName)
            .HasMaxLength(200);
        builder
            .Property(u => u.LastName)
            .HasMaxLength(200);
    }

    public void Client_Config(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);
        builder
            .Property(c => c.Id)
            .ValueGeneratedOnAdd();
        builder
            .Property(c => c.Name)
            .HasMaxLength(200);
        builder
            .Property(c => c.Address)
            .HasMaxLength(200);
    }

    public void Vendor_Config(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(v => v.Id);
        builder
            .Property(v => v.Id)
            .ValueGeneratedOnAdd();
        builder
            .Property(v => v.Name)
            .HasMaxLength(200);
        builder
            .Property(v => v.Address)
            .HasMaxLength(200);
    }

    public void FileAttach_Config(EntityTypeBuilder<FileAttach> builder)
    {
        builder.HasKey(fa => fa.Id);
        builder
            .Property(fa => fa.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(fa => fa.OfficeOrder)
            .WithMany(oo => oo.FileAttachs)
            .HasForeignKey(fs => fs.OfficeOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(fa => fa.WarehouseOrder)
            .WithMany(wo => wo.FileAttachs)
            .HasForeignKey(fs => fs.WarehouseOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(fa => fa.CustomerServiceOrder)
            .WithMany(cso => cso.FileAttachs)
            .HasForeignKey(fs => fs.CustomerServiceOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(fa => fa.RecordDT)
            .HasColumnType("datetime2");
        builder
            .HasAlternateKey(fs => fs.FileName);
    }

    public void WarehouseCategory_Config(EntityTypeBuilder<WarehouseCategory> builder)
    {
        builder
            .HasKey(wc => wc.Id);
        builder
            .Property(wc => wc.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(wc => wc.ParentCategory)
            .WithMany(wc => wc.SubCategories)
            .HasForeignKey(wc => wc.ParentCategoryForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(wc => wc.Name)
            .HasMaxLength(200);
    }

    public void WarehouseItem_Config(EntityTypeBuilder<WarehouseItem> builder)
    {
        builder.HasKey(wi => wi.Id);
        builder
            .Property(wi => wi.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(wi => wi.Category)
            .WithMany(wc => wc.WarehouseItems)
            .HasForeignKey(wi => wi.WarehouseCategoryForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(wi => wi.Name)
            .HasMaxLength(200);
        builder.HasAlternateKey(wi => wi.Code);
        builder
            .Property(wi => wi.Code)
            .HasMaxLength(200);
        builder
            .Property(wi => wi.CountRequired)
            .HasDefaultValue(0.0);
    }

    public void WarehouseSupply_Config(EntityTypeBuilder<WarehouseSupply> builder)
    {
        builder.HasKey(ws => ws.Id);
        builder
            .Property(ws => ws.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(ws => ws.WarehouseItem)
            .WithMany(wi => wi.WarehouseSupplying)
            .HasForeignKey(ws => ws.WarehouseItemForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ws => ws.WarehouseOrder)
            .WithMany(wo => wo.WarehouseSupplies)
            .HasForeignKey(ws => ws.WarehouseOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(ws => ws.DeliveryStatus)
            .HasConversion<int>()
            .HasDefaultValue(Delivery_Status.NotDelivered);
        builder
            .Property(ws => ws.Code)
            .HasMaxLength(200);
        builder
            .Property(ws => ws.Currency)
            .HasMaxLength(200);
        builder
            .Property(ws => ws.RecordDT)
            .HasColumnType("datetime2");
    }

    public void WarehouseOrder_Config(EntityTypeBuilder<WarehouseOrder> builder)
    {
        builder.HasKey(wo => wo.Id);
        builder
            .Property(wo => wo.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(wo => wo.Storekeeper)
            .WithMany(u => u.WarehouseOrders)
            .HasForeignKey(wo => wo.StorekeeperForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(wo => wo.Vendor)
            .WithMany(v => v.WarehouseOrders)
            .HasForeignKey(wo => wo.VendorForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(wo => wo.PaymentAgreement)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.FullyPaid);
        builder
            .Property(wo => wo.PaymentStatus)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.Unpaid);
        builder
            .Property(wo => wo.Currency)
            .HasMaxLength(200);
        builder
            .Property(wo => wo.RecordDT)
            .HasColumnType("datetime2");
        builder
            .Property(wo => wo.ShipmentDeadlineDT)
            .HasColumnType("datetime2");
        builder
            .Property(wo => wo.ReceivedDT)
            .HasColumnType("datetime2");
    }

    public void ProductCategory_Config(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(pc => pc.Id);
        builder
            .Property(pc => pc.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(pc => pc.ParentCategory)
            .WithMany(pc => pc.SubCategories)
            .HasForeignKey(pc => pc.ParentCategoryForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(pc => pc.Name)
            .HasMaxLength(200);
    }

    public void ProductTemplate_Config(EntityTypeBuilder<ProductTemplate> builder)
    {
        builder.HasKey(pt => pt.Id);
        builder
            .Property(pt => pt.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(pt => pt.Category)
            .WithMany(pc => pc.ProductTemplates)
            .HasForeignKey(pt => pt.ManufactureCategoryForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(pt => pt.Name)
            .HasMaxLength(200);
        builder
            .Property(pt => pt.Model)
            .HasMaxLength(200);
    }

    public void ProductSchema_Config(EntityTypeBuilder<ProductSchema> builder)
    {
        builder.HasKey(ps => ps.Id);
        builder
            .Property(ps => ps.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(ps => ps.ProductTemplate)
            .WithMany(pt => pt.ProductSchemas)
            .HasForeignKey(ps => ps.ProductTemplateForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ps => ps.WarehouseItem)
            .WithMany(ws => ws.ProductSchemas)
            .HasForeignKey(ps => ps.WarehouseItemForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public void ManufacturerDuty_Config(EntityTypeBuilder<ManufacturerDuty> builder)
    {
        builder.HasKey(md => md.Id);
        builder
            .Property(md => md.Id)
            .ValueGeneratedOnAdd();
        builder.HasAlternateKey(md => md.Name);
        builder
            .Property(md => md.Name)
            .HasMaxLength(200);
    }

    public void ManufactureProcess_Config(EntityTypeBuilder<ManufactureProcess> builder)
    {
        builder.HasKey(mp => mp.Id);
        builder
            .Property(mp => mp.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(mp => mp.Employee)
            .WithMany(u => u.ManufactureProcesses)
            .HasForeignKey(mp => mp.EmployeeForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mp => mp.Manufacture)
            .WithMany(mf => mf.ManufactureProcesses)
            .HasForeignKey(mp => mp.ManufactureForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mp => mp.ManufacturerDuty)
            .WithMany(md => md.ManufactureProcesses)
            .HasForeignKey(mp => mp.ManufactureDutyForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(mp => mp.TaskStatus)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotStarted);
        builder
            .Property(mp => mp.RecordDT)
            .HasColumnType("datetime2");
        builder
            .Property(mp => mp.DeadlineDT)
            .HasColumnType("datetime2");
        builder
            .Property(mp => mp.StartedDT)
            .HasColumnType("datetime2");
        builder
            .Property(mp => mp.CompletedDT)
            .HasColumnType("datetime2");
    }

    public void Manufacture_Config(EntityTypeBuilder<Manufacture> builder)
    {
        builder.HasKey(ms => ms.Id);
        builder
            .Property(ms => ms.Id)
            .ValueGeneratedOnAdd();
        builder.HasAlternateKey(ms => ms.Code);
        builder
            .HasOne(ms => ms.OfficeOrder)
            .WithMany(oo => oo.Manufactures)
            .HasForeignKey(ms => ms.OfficeOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(ws => ws.DeliveryStatus)
            .HasConversion<int>()
            .HasDefaultValue(Delivery_Status.NotDelivered);
        builder
            .Property(ws => ws.TaskStatus)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotAvailable);
        builder
            .Property(ws => ws.Code)
            .HasMaxLength(200);
        builder
            .Property(ws => ws.Name)
            .HasMaxLength(200);
        builder
            .Property(ws => ws.Model)
            .HasMaxLength(200);
        builder
            .Property(ws => ws.ShipmentDT)
            .HasColumnType("datetime2");
    }
    
    public void MaterialFlow_Config(EntityTypeBuilder<MaterialFlow> builder)
    {
        builder.HasKey(mf => mf.Id);
        builder
            .Property(mf => mf.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(ms => ms.WarehouseSupply)
            .WithMany(ws => ws.MaterialFlows)
            .HasForeignKey(ms => ms.WarehouseSupplyForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mf => mf.Manufacture)
            .WithMany(m => m.MaterialFlows)
            .HasForeignKey(ms => ms.ManufactureForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mf => mf.OfficeOrder)
            .WithMany(oo => oo.MaterialFlows)
            .HasForeignKey(ms => ms.OfficeOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mf => mf.CustomerServiceOrder)
            .WithMany(cso => cso.MaterialFlows)
            .HasForeignKey(ms => ms.CustomerServiceForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
    } 
    
    public void OfficeOrder_Config(EntityTypeBuilder<OfficeOrder> builder)
    {
        builder.HasKey(oo => oo.Id);
        builder
            .Property(oo => oo.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(oo => oo.Manager)
            .WithMany(u => u.OfficeOrders)
            .HasForeignKey(oo => oo.ManagerForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(oo => oo.Client)
            .WithMany(c => c.OfficeOrders)
            .HasForeignKey(oo => oo.ClientForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(oo => oo.OrderType)
            .HasConversion<int>();
        builder
            .Property(oo => oo.PaymentAgreement)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.FullyPaid);
        builder
            .Property(oo => oo.PaymentStatus)
            .HasConversion<int>()
            .HasDefaultValue(Payment_Status.Unpaid);
        builder
            .Property(oo => oo.Currency)
            .HasMaxLength(200);
        builder
            .Property(oo => oo.RecordDT)
            .HasColumnType("datetime2");
        builder
            .Property(oo => oo.ShipmentDeadlineDT)
            .HasColumnType("datetime2");
        builder
            .Property(oo => oo.ShipmentDT)
            .HasColumnType("datetime2");
    }
    
    public void CustomerService_Config(EntityTypeBuilder<CustomerService> builder)
    {
        builder.HasKey(cs => cs.Id);
        builder
            .Property(cs => cs.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(cs => cs.Employee)
            .WithMany(u => u.CustomerServices)
            .HasForeignKey(cs => cs.EmployeeForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(cs => cs.CustomerServiceOrder)
            .WithMany(cso => cso.CustomerServices)
            .HasForeignKey(cs => cs.CustomerServiceOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(cs => cs.TaskStatus)
            .HasConversion<int>()
            .HasDefaultValue(Task_Status.NotStarted);
        builder
            .Property(cs => cs.StartedDT)
            .HasColumnType("datetime2");
        builder
            .Property(cs => cs.CompletedDT)
            .HasColumnType("datetime2");
    }
    
    public void CustomerServiceOrder_Config(EntityTypeBuilder<CustomerServiceOrder> builder)
    {
        builder.HasKey(cso => cso.Id);
        builder
            .Property(cso => cso.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(cso => cso.StockManager)
            .WithMany(u => u.CustomerServiceOrders)
            .HasForeignKey(cso => cso.StockManagerForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(cso => cso.Manufacture)
            .WithMany(mf => mf.CustomerServiceOrders)
            .HasForeignKey(cso => cso.ManufactureForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
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
        builder
            .Property(cso => cso.Currency)
            .HasMaxLength(200);
        builder
            .Property(cso => cso.RecordDT)
            .HasColumnType("datetime2");
        builder
            .Property(cso => cso.DeadlineDT)
            .HasColumnType("datetime2");
        builder
            .Property(cso => cso.StartedDT)
            .HasColumnType("datetime2");
        builder
            .Property(cso => cso.CompletedDT)
            .HasColumnType("datetime2");
    }

    public void CurrencyUnit_Config(EntityTypeBuilder<CurrencyUnit> builder)
    {
        builder.HasKey(ur => ur.Id);
        builder
            .Property(ur => ur.Id)
            .ValueGeneratedOnAdd();
        builder.HasAlternateKey(ur => ur.Name);
    }

    public void Payment_Config(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(m => m.Id);
        builder
            .Property(m => m.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(m => m.WarehouseOrder)
            .WithMany(ws => ws.Payments)
            .HasForeignKey(m => m.WarehouseOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(m => m.OfficeOrder)
            .WithMany(oo => oo.Payments)
            .HasForeignKey(m => m.OfficeOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(m => m.CustomerServiceOrder)
            .WithMany(cso => cso.Payments)
            .HasForeignKey(m => m.CustomerServiceOrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(m => m.Currency)
            .HasMaxLength(200);
        builder
            .Property(m => m.UnitToBYNConversion)
            .HasDefaultValue(1.0);
        builder
            .Property(m => m.RecordDT)
            .HasColumnType("datetime2");
    }

    public void Logging_Config(EntityTypeBuilder<Logging> builder)
    {
        builder.HasKey(log => log.Id);
        builder
            .Property(log => log.Id)
            .ValueGeneratedOnAdd();
        builder
            .Property(log => log.UserLogin)
            .HasMaxLength(200);
        builder
            .Property(log => log.UserFirstName)
            .HasMaxLength(200);
        builder
            .Property(log => log.UserLastName)
            .HasMaxLength(200);
        builder
            .Property(log => log.RecordDT)
            .HasColumnType("datetime2");
    }
}