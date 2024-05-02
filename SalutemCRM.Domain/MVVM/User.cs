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
    public static User Default => new User() { UserRole = new() { Id = -1, Name = "Не авторизованный пользователь" } };

    [NotMapped]
    public static User RootOrBoss => new User() { UserRole = new() { Id = 1, Name = "Руководитель" } };

    [NotMapped]
    public static User Bookkeeper => new User() { UserRole = new() { Id = 2, Name = "Бухгалтер" } };

    [NotMapped]
    public static User SeniorSalesManager => new User() { UserRole = new() { Id = 3, Name = "Гл. менеджер" } };

    [NotMapped]
    public static User SalesManager => new User() { UserRole = new() { Id = 4, Name = "Менеджер" } };

    [NotMapped]
    public static User ManufactureManager => new User() { UserRole = new() { Id = 5, Name = "Гл. производства" } };

    [NotMapped]
    public static User ConstrEngineer => new User() { UserRole = new() { Id = 6, Name = "Конструктор" } };

    [NotMapped]
    public static User ManufactureEmployee => new User() { UserRole = new() { Id = 7, Name = "Производство" } };

    [NotMapped]
    public static User Storekeeper => new User() { UserRole = new() { Id = 8, Name = "Кладовщик" } };
}
