using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using SalutemCRM.Domain.Modell;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SalutemCRM.Domain.Model;

public partial class User
{
    [NotMapped]
    public string FullName => $"{this.FirstName} {this.LastName}";

    [NotMapped]
    public string FullNameWithLogin => $"{this.FirstName} {this.LastName} [{this.Login}]";


    [NotMapped]
    public static User Default => new User() { Id = -1, UserRole = new() { Name = "Не авторизованный пользователь" } };

    [NotMapped]
    public static User RootOrBoss => new User() { Id = 1, UserRole = new() { Name = "Руководитель" } };

    [NotMapped]
    public static User Bookkeeper => new User() { Id = 2, UserRole = new() { Name = "Бухгалтер" } };

    [NotMapped]
    public static User SeniorSalesManager => new User() { Id = 3, UserRole = new() { Name = "Гл. менеджер" } };

    [NotMapped]
    public static User SalesManager => new User() { Id = 4, UserRole = new() { Name = "Менеджер" } };

    [NotMapped]
    public static User ManufactureManager => new User() { Id = 5, UserRole = new() { Name = "Гл. производства" } };

    [NotMapped]
    public static User ConstrEngineer => new User() { Id = 6, UserRole = new() { Name = "Конструктор" } };

    [NotMapped]
    public static User ManufactureEmployee => new User() { Id = 7, UserRole = new() { Name = "Производство" } };

    [NotMapped]
    public static User Storekeeper => new User() { Id = 8, UserRole = new() { Name = "Кладовщик" } };
}
