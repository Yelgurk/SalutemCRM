﻿using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Database.Trigger;
using SalutemCRM.Domain;
using SalutemCRM.Domain.Model;
using SalutemCRM.Domain.Modell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
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
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductTemplate> ProductTemplates { get; set; }
    public DbSet<ProductSchema> ProductSchemas { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDuty> OrderDuties { get; set; }
    public DbSet<OrderProcess> OrderProcesses { get; set; }
    public DbSet<Manufacture> Manufacture { get; set; }
    public DbSet<MaterialFlow> MaterialFlow { get; set; }
    public DbSet<CurrencyUnit> CurrencyUnits { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Logging> Logging { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<RegionMonitoring> RegionMonitorings { get; set; }
    public DbSet<MesurementUnit> MesurementUnits { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public void DatabaseInit()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder
        //    .UseTriggers(triggerOptions => {
        //        triggerOptions.AddTrigger<Trigger.MaterialFlowCountValidation>();
        //    });

        base.OnConfiguring(optionsBuilder);
    }

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
        modelBuilder.Entity<ProductCategory>(ProductCategory_Config);
        modelBuilder.Entity<ProductTemplate>(ProductTemplate_Config);
        modelBuilder.Entity<ProductSchema>(ProductSchema_Config);
        modelBuilder.Entity<Order>(Order_Config);
        modelBuilder.Entity<OrderDuty>(OrderDuty_Config);
        modelBuilder.Entity<OrderProcess>(OrderProcess_Config);
        modelBuilder.Entity<Manufacture>(Manufacture_Config);
        modelBuilder.Entity<MaterialFlow>(MaterialFlow_Config);
        modelBuilder.Entity<CurrencyUnit>(CurrencyUnit_Config);
        modelBuilder.Entity<MesurementUnit>(MesurementUnit_Config);
        modelBuilder.Entity<Payment>(Payment_Config);
        modelBuilder.Entity<Logging>(Logging_Config);
        modelBuilder.Entity<Country>(Country_Config);
        modelBuilder.Entity<City>(City_Config);
        modelBuilder.Entity<RegionMonitoring>(RegionMonitoring_Config);

        DemoModelCreating(modelBuilder);
    }

    private void DemoModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Беларусь" },
            new Country { Id = 2, Name = "Россия" },
            new Country { Id = 3, Name = "Казахстан" }
        );

        modelBuilder.Entity<City>().HasData(
            new City { Id = 1, Name = "Минск", CountryForeignKey = 1 },
            new City { Id = 2, Name = "Брест", CountryForeignKey = 1 },
            new City { Id = 3, Name = "Гродно", CountryForeignKey = 1 }
        );

        modelBuilder.Entity<RegionMonitoring>().HasData(
            new RegionMonitoring { Id = 1, CityForeignKey = 1, EmployeeForeignKey = 1 },
            new RegionMonitoring { Id = 2, CityForeignKey = 2, EmployeeForeignKey = 1 },
            new RegionMonitoring { Id = 3, CityForeignKey = 3, EmployeeForeignKey = 1 }
        );

        modelBuilder.Entity<CurrencyUnit>().HasData(
            new CurrencyUnit { Name = "BYN" },
            new CurrencyUnit { Name = "USD" },
            new CurrencyUnit { Name = "RUB" },
            new CurrencyUnit { Name = "CNY" }
        );

        modelBuilder.Entity<MesurementUnit>().HasData(
            new MesurementUnit { Name = "шт." },
            new MesurementUnit { Name = "м." },
            new MesurementUnit { Name = "см." },
            new MesurementUnit { Name = "кг." },
            new MesurementUnit { Name = "гр." },
            new MesurementUnit { Name = "л." },
            new MesurementUnit { Name = "мл." },
            new MesurementUnit { Name = "погон/м." },
            new MesurementUnit { Name = "погое/см." },
            new MesurementUnit { Name = "упак." }
        );

        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { Id = 1, Name = "Руководитель" },
            new UserRole { Id = 2, Name = "Бухгалтер" },
            new UserRole { Id = 3, Name = "Гл. менеджер" },
            new UserRole { Id = 4, Name = "Менеджер" },
            new UserRole { Id = 5, Name = "Гл. производства" },
            new UserRole { Id = 6, Name = "Конструктор" },
            new UserRole { Id = 7, Name = "Производство" },
            new UserRole { Id = 8, Name = "Кладовщик" },
            new UserRole { Id = 9, Name = "Отдел закупок" },
            new UserRole { Id = 10, Name = "Сервисное обсл." }
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
            new Client { Id = 1, CityForeignKey = 1, Name = "ЗАО \"МИНСКОЕ\"", Address = "Беларусь, Минск, ул. ***, **" },
            new Client { Id = 2, CityForeignKey = 1, Name = "ЗАО \"БРЕСТСКОЕ\"", Address = "Беларусь, Брест, ул. ***, **" },
            new Client { Id = 3, CityForeignKey = 1, Name = "ЗАО \"ВИТЕБСКОЕ\"", Address = "Беларусь, Витебск, ул. ***, **" },
            new Client { Id = 4, CityForeignKey = 1, Name = "ЗАО \"ГРОДНЕНСКОЕ\"", Address = "Беларусь, Гродно, ул. ***, **" },
            new Client { Id = 5, CityForeignKey = 1, Name = "ЗАО \"ГОМЕЛЬСКОЕ\"", Address = "Беларусь, Гомель, ул. ***, **" },
            new Client { Id = 6, CityForeignKey = 1, Name = "ЗАО \"МОГИЛЕВСКОЕ\"", Address = "Беларусь, Могилев, ул. ***, **" }
        );

        modelBuilder.Entity<Vendor>().HasData(
            new Client { Id = 1, CityForeignKey = 1, Name = "АО \"СЕЛЬМАШ_МИН\"", Address = "Беларусь, Минск, ул. ***, **" },
            new Client { Id = 2, CityForeignKey = 1, Name = "АО \"СЕЛЬМАШ_БРЕ\"", Address = "Беларусь, Брест, ул. ***, **" },
            new Client { Id = 3, CityForeignKey = 1, Name = "АО \"СЕЛЬМАШ_ВИТ\"", Address = "Беларусь, Витебск, ул. ***, **" },
            new Client { Id = 4, CityForeignKey = 1, Name = "АО \"СЕЛЬМАШ_ГРО\"", Address = "Беларусь, Гродно, ул. ***, **" },
            new Client { Id = 5, CityForeignKey = 1, Name = "АО \"СЕЛЬМАШ_ГОМ\"", Address = "Беларусь, Гомель, ул. ***, **" },
            new Client { Id = 6, CityForeignKey = 1, Name = "АО \"СЕЛЬМАШ_МОГ\"", Address = "Беларусь, Могилев, ул. ***, **" }
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
            new WarehouseItem { Id = 1, MesurementUnit = "шт.", InnerName = "Кнопка поворотная", InnerCode = "a0001", CountRequired = 30, WarehouseCategoryForeignKey = 1 },
            new WarehouseItem { Id = 2, MesurementUnit = "шт.", InnerName = "Кнопка рычажная", InnerCode = "a0002", CountRequired = 35, WarehouseCategoryForeignKey = 8 },
            new WarehouseItem { Id = 3, MesurementUnit = "шт.", InnerName = "Кнопка тактовая", InnerCode = "a0003", CountRequired = 40, WarehouseCategoryForeignKey = 8 },
            new WarehouseItem { Id = 4, MesurementUnit = "шт.", InnerName = "Насос 35лм", InnerCode = "a0004", CountRequired = 45, WarehouseCategoryForeignKey = 6 },
            new WarehouseItem { Id = 5, MesurementUnit = "шт.", InnerName = "Насос мембранный 45лм", InnerCode = "a0005", CountRequired = 50, WarehouseCategoryForeignKey = 6 },
            new WarehouseItem { Id = 6, MesurementUnit = "шт.", InnerName = "Привод одноколесный", InnerCode = "a0006", CountRequired = 55, WarehouseCategoryForeignKey = 5 },
            new WarehouseItem { Id = 7, MesurementUnit = "шт.", InnerName = "Привод двухколесный", InnerCode = "a0007", CountRequired = 60, WarehouseCategoryForeignKey = 5 },
            new WarehouseItem { Id = 8, MesurementUnit = "шт.", InnerName = "Клемма обжимная 1.5мм", InnerCode = "a0008", CountRequired = 65, WarehouseCategoryForeignKey = 1 },
            new WarehouseItem { Id = 9, MesurementUnit = "шт.", InnerName = "Клемма обжимная 2.0мм", InnerCode = "a0009", CountRequired = 70, WarehouseCategoryForeignKey = 7 },
            new WarehouseItem { Id = 10, MesurementUnit = "шт.", InnerName = "Клемма 5.08 EDG", InnerCode = "a0010", CountRequired = 75, WarehouseCategoryForeignKey = 2 },
            new WarehouseItem { Id = 11, MesurementUnit = "шт.", InnerName = "Реле рейловое 220В", InnerCode = "a0011", CountRequired = 80, WarehouseCategoryForeignKey = 9 },
            new WarehouseItem { Id = 12, MesurementUnit = "шт.", InnerName = "Реле пусковое 48в dc", InnerCode = "a0012", CountRequired = 30, WarehouseCategoryForeignKey = 1 },
            new WarehouseItem { Id = 13, MesurementUnit = "шт.", InnerName = "FX3U", InnerCode = "a0013", CountRequired = 35, WarehouseCategoryForeignKey = 4 },
            new WarehouseItem { Id = 14, MesurementUnit = "шт.", InnerName = "OP280", InnerCode = "a0014", CountRequired = 40, WarehouseCategoryForeignKey = 4 },
            new WarehouseItem { Id = 15, MesurementUnit = "шт.", InnerName = "SSPS3R3", InnerCode = "a0015", CountRequired = 45, WarehouseCategoryForeignKey = 4 },
            new WarehouseItem { Id = 16, MesurementUnit = "ед.", InnerName = "Обичевка пс100", InnerCode = "a0016", CountRequired = 50, WarehouseCategoryForeignKey = 10 },
            new WarehouseItem { Id = 17, MesurementUnit = "ед.", InnerName = "Обичевка пс200", InnerCode = "a0017", CountRequired = 55, WarehouseCategoryForeignKey = 10 },
            new WarehouseItem { Id = 18, MesurementUnit = "ед.", InnerName = "Обичевка пс300", InnerCode = "a0018", CountRequired = 60, WarehouseCategoryForeignKey = 3 },
            new WarehouseItem { Id = 19, MesurementUnit = "ед.", InnerName = "Обичевка рм25", InnerCode = "a0019", CountRequired = 65, WarehouseCategoryForeignKey = 1 },
            new WarehouseItem { Id = 20, MesurementUnit = "ед.", InnerName = "Обичевка рм45", InnerCode = "a0020", CountRequired = 70, WarehouseCategoryForeignKey = 11 },
            new WarehouseItem { Id = 21, MesurementUnit = "ед.", InnerName = "Обичевка рм-lite", InnerCode = "a0021", CountRequired = 75, WarehouseCategoryForeignKey = 3 },
            new WarehouseItem { Id = 22, MesurementUnit = "ед.", InnerName = "Обичевка тм100", InnerCode = "a0022", CountRequired = 80, WarehouseCategoryForeignKey = 12 },
            new WarehouseItem { Id = 23, MesurementUnit = "ед.", InnerName = "Обичевка тм100", InnerCode = "a0023", CountRequired = 30, WarehouseCategoryForeignKey = 12 },
            new WarehouseItem { Id = 24, MesurementUnit = "ед.", InnerName = "Обичевка тм100", InnerCode = "a0024", CountRequired = 35, WarehouseCategoryForeignKey = 12 }
        );

        modelBuilder.Entity<Order>().HasData(
            new Order {
                Id = 1,
                EmployeeForeignkey = 9,
                VendorForeignKey = 1,
                AdditionalInfo = "Загрузка склада при интеграции приложения",
                Currency = "BYN",
                UnitToBYNConversion = 1.0,
                DaysOnHold = 30,
                OrderType = Order_Type.WarehouseRestocking,
                PaymentAgreement = Payment_Status.FullyPaid,
                PaymentStatus = Payment_Status.FullyPaid,
                TaskStatus = Task_Status.Finished,
                PriceRequired = 0,
                PriceTotal = 0,
                RecordDT = DateTime.Now,
                DeadlineDT = DateTime.Now.AddDays(2),
                CompletedDT = DateTime.Now
            }    
        );

        modelBuilder.Entity<WarehouseSupply>().HasData(
            new WarehouseSupply { Id = 1, WarehouseItemForeignKey = 1, OrderForeignKey = 1, VendorName = "тест 1", VendorCode = "x0001", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 2, WarehouseItemForeignKey = 2, OrderForeignKey = 1, VendorName = "тест 2", VendorCode = "x0002", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 3, WarehouseItemForeignKey = 3, OrderForeignKey = 1, VendorName = "тест 3", VendorCode = "x0003", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 4, WarehouseItemForeignKey = 4, OrderForeignKey = 1, VendorName = "тест 4", VendorCode = "x0004", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 5, WarehouseItemForeignKey = 5, OrderForeignKey = 1, VendorName = "тест 5", VendorCode = "x0005", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 6, WarehouseItemForeignKey = 6, OrderForeignKey = 1, VendorName = "тест 6", VendorCode = "x0006", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 7, WarehouseItemForeignKey = 7, OrderForeignKey = 1, VendorName = "тест 7", VendorCode = "x0007", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 8, WarehouseItemForeignKey = 8, OrderForeignKey = 1, VendorName = "тест 8", VendorCode = "x0008", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 9, WarehouseItemForeignKey = 9, OrderForeignKey = 1, VendorName = "тест 9", VendorCode = "x0009", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 10, WarehouseItemForeignKey = 10, OrderForeignKey = 1, VendorName = "тест 10", VendorCode = "x0010", PriceTotal = 100.0, OrderCount = 0.0, InStockCount = 0.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 11, WarehouseItemForeignKey = 11, OrderForeignKey = 1, VendorName = "тест 11", VendorCode = "x0011", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 12, WarehouseItemForeignKey = 12, OrderForeignKey = 1, VendorName = "тест 12", VendorCode = "x0012", PriceTotal = 100.0, OrderCount = 0.0, InStockCount = 0.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 13, WarehouseItemForeignKey = 13, OrderForeignKey = 1, VendorName = "тест 13", VendorCode = "x0013", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 14, WarehouseItemForeignKey = 14, OrderForeignKey = 1, VendorName = "тест 14", VendorCode = "x0014", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 15, WarehouseItemForeignKey = 15, OrderForeignKey = 1, VendorName = "тест 15", VendorCode = "x0015", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 16, WarehouseItemForeignKey = 16, OrderForeignKey = 1, VendorName = "тест 16", VendorCode = "x0016", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 17, WarehouseItemForeignKey = 17, OrderForeignKey = 1, VendorName = "тест 17", VendorCode = "x0017", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 18, WarehouseItemForeignKey = 18, OrderForeignKey = 1, VendorName = "тест 18", VendorCode = "x0018", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 19, WarehouseItemForeignKey = 19, OrderForeignKey = 1, VendorName = "тест 19", VendorCode = "x0019", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 20, WarehouseItemForeignKey = 20, OrderForeignKey = 1, VendorName = "тест 20", VendorCode = "x0020", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 21, WarehouseItemForeignKey = 21, OrderForeignKey = 1, VendorName = "тест 21", VendorCode = "x0021", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 22, WarehouseItemForeignKey = 22, OrderForeignKey = 1, VendorName = "тест 22", VendorCode = "x0022", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 23, WarehouseItemForeignKey = 23, OrderForeignKey = 1, VendorName = "тест 23", VendorCode = "x0023", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 24, WarehouseItemForeignKey = 24, OrderForeignKey = 1, VendorName = "тест 24", VendorCode = "x0024", PriceTotal = 100.0, OrderCount = 45.0, InStockCount = 45.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 25, WarehouseItemForeignKey = 1, OrderForeignKey = 1, VendorName = "тест 25", VendorCode = "z0001", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 26, WarehouseItemForeignKey = 2, OrderForeignKey = 1, VendorName = "тест 26", VendorCode = "z0002", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 27, WarehouseItemForeignKey = 3, OrderForeignKey = 1, VendorName = "тест 27", VendorCode = "z0003", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 28, WarehouseItemForeignKey = 4, OrderForeignKey = 1, VendorName = "тест 28", VendorCode = "z0004", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 29, WarehouseItemForeignKey = 5, OrderForeignKey = 1, VendorName = "тест 29", VendorCode = "z0005", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 30, WarehouseItemForeignKey = 6, OrderForeignKey = 1, VendorName = "тест 30", VendorCode = "z0006", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 31, WarehouseItemForeignKey = 7, OrderForeignKey = 1, VendorName = "тест 31", VendorCode = "z0007", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 32, WarehouseItemForeignKey = 8, OrderForeignKey = 1, VendorName = "тест 32", VendorCode = "z0008", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 33, WarehouseItemForeignKey = 9, OrderForeignKey = 1, VendorName = "тест 33", VendorCode = "z0009", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 34, WarehouseItemForeignKey = 10, OrderForeignKey = 1, VendorName = "тест 34", VendorCode = "z0010", PriceTotal = 100.0, OrderCount = 0.0, InStockCount = 0.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 35, WarehouseItemForeignKey = 11, OrderForeignKey = 1, VendorName = "тест 35", VendorCode = "z0011", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 36, WarehouseItemForeignKey = 12, OrderForeignKey = 1, VendorName = "тест 36", VendorCode = "z0012", PriceTotal = 100.0, OrderCount = 0.0, InStockCount = 0.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 37, WarehouseItemForeignKey = 13, OrderForeignKey = 1, VendorName = "тест 37", VendorCode = "z0013", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 38, WarehouseItemForeignKey = 14, OrderForeignKey = 1, VendorName = "тест 38", VendorCode = "z0014", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 39, WarehouseItemForeignKey = 15, OrderForeignKey = 1, VendorName = "тест 39", VendorCode = "z0015", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 40, WarehouseItemForeignKey = 16, OrderForeignKey = 1, VendorName = "тест 40", VendorCode = "z0016", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 41, WarehouseItemForeignKey = 17, OrderForeignKey = 1, VendorName = "тест 41", VendorCode = "z0017", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 42, WarehouseItemForeignKey = 18, OrderForeignKey = 1, VendorName = "тест 42", VendorCode = "z0018", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 43, WarehouseItemForeignKey = 19, OrderForeignKey = 1, VendorName = "тест 43", VendorCode = "z0019", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 44, WarehouseItemForeignKey = 20, OrderForeignKey = 1, VendorName = "тест 44", VendorCode = "z0020", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 45, WarehouseItemForeignKey = 21, OrderForeignKey = 1, VendorName = "тест 45", VendorCode = "z0021", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 46, WarehouseItemForeignKey = 22, OrderForeignKey = 1, VendorName = "тест 46", VendorCode = "z0022", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 47, WarehouseItemForeignKey = 23, OrderForeignKey = 1, VendorName = "тест 47", VendorCode = "z0023", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered },
            new WarehouseSupply { Id = 48, WarehouseItemForeignKey = 24, OrderForeignKey = 1, VendorName = "тест 48", VendorCode = "z0024", PriceTotal = 100.0, OrderCount = 33.0, InStockCount = 33.0, Currency = "BYN", RecordDT = DateTime.Now, DeliveryStatus = Delivery_Status.FullyDelivered }
        );

        modelBuilder.Entity<ProductCategory>().HasData(
            new ProductCategory { Id = 1, Deep = 0, Name = "Пастеризатор" },
            new ProductCategory { Id = 2, Deep = 0, Name = "Такси" },
            new ProductCategory { Id = 3, Deep = 0, Name = "Размораживатель" },
            new ProductCategory { Id = 4, Deep = 0, Name = "СМОМ" },
            new ProductCategory { Id = 5, Deep = 0, Name = "Боксы" }
        );

        modelBuilder.Entity<ProductTemplate>().HasData(
            new ProductTemplate { Id = 1, Name = "ПС 100", ManufactureCategoryForeignKey = 1, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 2, Name = "ПС 200", ManufactureCategoryForeignKey = 1, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 3, Name = "ПС 300", ManufactureCategoryForeignKey = 1, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 4, Name = "ПС 400", ManufactureCategoryForeignKey = 1, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 5, Name = "ПС 500", ManufactureCategoryForeignKey = 1, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 6, Name = "ТМ 100", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 7, Name = "ТМ 200", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 8, Name = "ТМ 300", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 9, Name = "ТМ 400", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 10, Name = "ТМП 200", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 11, Name = "ТМП 300", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 12, Name = "ТМП 400", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 13, Name = "ТМПЭ 200", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 14, Name = "ТМПЭ 300", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 15, Name = "ТМПЭ 400", ManufactureCategoryForeignKey = 2, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 16, Name = "РМ 25", ManufactureCategoryForeignKey = 3, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 17, Name = "РМ 45", ManufactureCategoryForeignKey = 3, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 18, Name = "РМ-lite lite", ManufactureCategoryForeignKey = 3, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 19, Name = "СМОМ 1000", ManufactureCategoryForeignKey = 4, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 20, Name = "СМОМ 2000", ManufactureCategoryForeignKey = 4, AdditionalInfo = "", HaveSerialNumber = true },    
            new ProductTemplate { Id = 21, Name = "Бокс 2м*3м", ManufactureCategoryForeignKey = 5, AdditionalInfo = "", HaveSerialNumber = false },    
            new ProductTemplate { Id = 22, Name = "Бокс 2м*3.5м", ManufactureCategoryForeignKey = 5, AdditionalInfo = "", HaveSerialNumber = false },    
            new ProductTemplate { Id = 23, Name = "Бокс 3м*3.5м", ManufactureCategoryForeignKey = 5, AdditionalInfo = "", HaveSerialNumber = false },    
            new ProductTemplate { Id = 24, Name = "Бокс 3.5м*4м", ManufactureCategoryForeignKey = 5, AdditionalInfo = "", HaveSerialNumber = false }    
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

        modelBuilder.Entity<MaterialFlow>();
        modelBuilder.Entity<OrderDuty>();
        modelBuilder.Entity<OrderProcess>();
        modelBuilder.Entity<Manufacture>();
        modelBuilder.Entity<Payment>();
    }

    public void UserRole_Config(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.Id);
        builder
            .Property(ur => ur.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasIndex(ur => ur.Name)
            .IsUnique();
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
        builder
            .HasIndex(u => u.Login)
            .IsUnique();
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
            .HasOne(c => c.City)
            .WithMany(x => x.Clients)
            .HasForeignKey(c => c.CityForeignKey);
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
            .HasOne(c => c.City)
            .WithMany(x => x.Vendors)
            .HasForeignKey(c => c.CityForeignKey);
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
            .HasOne(fa => fa.Order)
            .WithMany(oo => oo.FileAttachs)
            .HasForeignKey(fs => fs.OrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(fa => fa.WarehouseItem)
            .WithMany(oo => oo.FileAttachs)
            .HasForeignKey(fs => fs.WarehouseItemForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(fa => fa.MaterialFlow)
            .WithMany(oo => oo.FileAttachs)
            .HasForeignKey(fs => fs.MaterialFlowForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(fa => fa.Payment)
            .WithMany(oo => oo.FileAttachs)
            .HasForeignKey(fs => fs.PaymentForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(fa => fa.RecordDT)
            .HasColumnType("datetime2");
        builder
            .HasIndex(fs => fs.FileName)
            .IsUnique();
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
            .HasIndex(wi => wi.InnerCode)
            .IsUnique();
        builder
            .Property(wi => wi.InnerCode)
            .HasMaxLength(200);
        builder
            .Property(wi => wi.CountRequired)
            .HasDefaultValue(0.0);
        builder
            .Property(wi => wi.MesurementUnit)
            .HasDefaultValue("шт.");
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
            .HasOne(ws => ws.Order)
            .WithMany(wo => wo.WarehouseSupplies)
            .HasForeignKey(ws => ws.OrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(ws => ws.DeliveryStatus);
        builder
            .Property(ws => ws.VendorCode)
            .HasMaxLength(200);
        builder
            .Property(ws => ws.UnitToBYNConversion)
            .HasDefaultValue(1.0);
        builder
            .Property(ws => ws.Currency)
            .HasMaxLength(200)
            .HasDefaultValue("BYN");
        builder
            .Property(ws => ws.RecordDT)
            .HasColumnType("datetime2");
        builder
            .Property(ws => ws.InStockCount)
            .HasDefaultValue(0);
    }

    public void Order_Config(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(wo => wo.Id);
        builder
            .Property(wo => wo.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(wo => wo.Employee)
            .WithMany(u => u.Orders)
            .HasForeignKey(wo => wo.EmployeeForeignkey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(wo => wo.Client)
            .WithMany(v => v.Orders)
            .HasForeignKey(wo => wo.ClientForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(wo => wo.Vendor)
            .WithMany(v => v.Orders)
            .HasForeignKey(wo => wo.VendorForeignKey)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(wo => wo.Currency)
            .HasMaxLength(200);
        builder
            .Property(wo => wo.RecordDT)
            .HasColumnType("datetime2");
        builder
            .Property(wo => wo.DeadlineDT)
            .HasColumnType("datetime2");
        builder
            .Property(wo => wo.StartedDT)
            .HasColumnType("datetime2");
        builder
            .Property(wo => wo.CompletedDT)
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

    public void OrderDuty_Config(EntityTypeBuilder<OrderDuty> builder)
    {
        builder.HasKey(md => md.Id);
        builder
            .Property(md => md.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasIndex(md => md.Name)
            .IsUnique();
        builder
            .Property(md => md.Name)
            .HasMaxLength(200);
    }

    public void OrderProcess_Config(EntityTypeBuilder<OrderProcess> builder)
    {
        builder.HasKey(mp => mp.Id);
        builder
            .Property(mp => mp.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(mp => mp.Employee)
            .WithMany(u => u.OrderProcesses)
            .HasForeignKey(mp => mp.EmployeeForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mp => mp.Manufacture)
            .WithMany(mf => mf.OrderProcesses)
            .HasForeignKey(mp => mp.ManufactureForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mp => mp.Order)
            .WithMany(mf => mf.OrderProcesses)
            .HasForeignKey(mp => mp.OrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mp => mp.OrderDuty)
            .WithMany(md => md.OrderProcesses)
            .HasForeignKey(mp => mp.OrderDutyForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
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
        builder
            .HasIndex(ms => ms.Code)
            .IsUnique();
        builder
            .HasOne(ms => ms.Order)
            .WithMany(oo => oo.Manufactures)
            .HasForeignKey(ms => ms.OrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .Property(ws => ws.Code)
            .HasMaxLength(200);
        builder
            .Property(ws => ws.Name)
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
            .HasOne(mf => mf.ReturnedMaterial)
            .WithMany(u => u.ExchangedMaterials)
            .HasForeignKey(mf => mf.ReturnedForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(mf => mf.Employee)
            .WithMany(u => u.MaterialsFlow)
            .HasForeignKey(mf => mf.EmployeeForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(ms => ms.WarehouseItem)
            .WithMany(ws => ws.MaterialFlows)
            .HasForeignKey(ms => ms.WarehouseItemForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
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
            .HasOne(mf => mf.Order)
            .WithMany(oo => oo.MaterialFlows)
            .HasForeignKey(ms => ms.OrderForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .Property(mf => mf.RecordDT)
            .HasColumnType("datetime2");
    } 
   

    public void CurrencyUnit_Config(EntityTypeBuilder<CurrencyUnit> builder)
    {
        builder
            .HasKey(cu => cu.Name);
        builder
            .Property(cu => cu.Name)
            .HasMaxLength(200);
    }

    public void MesurementUnit_Config(EntityTypeBuilder<MesurementUnit> builder)
    {
        builder.HasKey(mu => mu.Name);
        builder
            .Property(mu => mu.Name)
            .HasMaxLength(200);
    }

    public void Payment_Config(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(m => m.Id);
        builder
            .Property(m => m.Id)
            .ValueGeneratedOnAdd();
        builder
            .HasOne(m => m.Order)
            .WithMany(ws => ws.Payments)
            .HasForeignKey(m => m.OrderForeignKey)
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

    public void Country_Config(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .HasIndex(x => x.Name)
            .IsUnique();
        builder
            .Property(x => x.Name)
            .HasMaxLength(200);
    }

    public void City_Config(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .Property(x => x.Name)
            .HasMaxLength(200);

        builder
            .HasOne(x => x.Country)
            .WithMany(x => x.Cities)
            .HasForeignKey(x => x.CountryForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public void RegionMonitoring_Config(EntityTypeBuilder<RegionMonitoring> builder)
    {
        builder.HasKey(x => x.Id);
        builder
            .HasOne(x => x.Employee)
            .WithMany(x => x.RegionsMonitoring)
            .HasForeignKey(x => x.EmployeeForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
        builder
            .HasOne(x => x.City)
            .WithOne(x => x.RegionMonitoring)
            .HasForeignKey<RegionMonitoring>(x => x.CityForeignKey)
            .OnDelete(DeleteBehavior.Restrict);
    }
}