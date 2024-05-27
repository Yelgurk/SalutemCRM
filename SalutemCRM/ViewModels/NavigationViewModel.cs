using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using DynamicData.Kernel;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.Control;
using SalutemCRM.ControlTemplated;
using SalutemCRM.Domain.Model;
using SalutemCRM.Domain.Modell;
using SalutemCRM.FunctionalControl;
using SalutemCRM.Reactive;
using SalutemCRM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace SalutemCRM.ViewModels;

public partial class NavigationModel : ObservableObject
{
    private string AvaresPath(string ImageName) => $"avares://SalutemCRM/Assets/Icon/{ImageName}";

    public Bitmap Icon => new Bitmap(AssetLoader.Open(new Uri(AvaresPath(IconUri))));

    public object? Content => GetContent?.Invoke();

    public Type? ContentType => Content?.GetType();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Icon))]
    public string _iconUri = "";

    [ObservableProperty]
    public string _title = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Content))]
    public Func<object>? _getContent;
}

public partial class NavigationViewModelSource : ReactiveControlSource<NavigationModel>
{
    private static NavigationViewModelSource _currentNavigation { get; set; }

    public static void SetNonRegWindowContent<T>() where T : class
    {
        _currentNavigation?.Do(x => x.SelectedItem = null);
        _currentNavigation?.Do(x => x.SelectedItem = new() { GetContent = Get<T> });
    }

    private static object Get<T>() where T : class => App.Host!.Services.GetService<T>()!;

    public static void SetRegisteredWindowContent<T>() where T : class
    {
        _currentNavigation?.NavigationCollection
            .SingleOrDefault(x => x.ContentType == typeof(T))?
            .Do(x => _currentNavigation.SelectedItem = x);
    }

    [ObservableProperty]
    private ObservableCollection<NavigationModel> _navigationCollection = new();


    public NavigationViewModelSource()
    {
        _currentNavigation = this;

        Account.OnUserChangedTrigger = SetNavigationAccess;

        if (Design.IsDesignMode)
        {
            NavigationCollection.Add(new()
            {
                IconUri = "File Invoice.png",
                Title = "Новый счёт",
                GetContent = Get<OrderEditorControl>
            });

            NavigationCollection.Add(new()
            {
                IconUri = "Books.png",
                Title = "Оплата счетов",
                GetContent = Get<OrdersObservableControl>
            });

            NavigationCollection.Add(new()
            {
                IconUri = "Books.png",
                Title = "Контроль заявок",
                GetContent = Get<OrdersManagmentControl>
            });
        }
    }

    public bool SetNavigationAccess(User SignIn)
    {
        NavigationCollection.Clear();

        if (SignIn is null)
            return false;

        switch(SignIn.Permission)
        {
            case User_Permission.Boss:
                {
                    NavigationCollection.Add(new() { IconUri = "File Invoice.png", Title = "Новая заявка", GetContent = Get<OrderEditorControl> });
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Контроль заявок", GetContent = Get<OrdersManagmentControl> });
                    NavigationCollection.Add(new() { IconUri = "Photo gallery.png", Title = "База клиентов", GetContent = Get<CRUSClientControl> });
                    NavigationCollection.Add(new() { IconUri = "Photo gallery.png", Title = "База поставщиков", GetContent = Get<CRUSVendorControl> });
                    NavigationCollection.Add(new() { IconUri = "Warehouse.png", Title = "Склад", GetContent = Get<WarehouseGeneral> });
                    NavigationCollection.Add(new() { IconUri = "Manufacture.png", Title = "Конструктор изделий", GetContent = Get<ProductTemplateBuilder> });
                    NavigationCollection.Add(new() { IconUri = "Settings.png", Title = "База регионов", GetContent = Get<TemplateRegionsEditor> });
                    NavigationCollection.Add(new() { IconUri = "Settings.png", Title = "Назначенные регионы", GetContent = Get<TemplateRegionsManagment> });
                    NavigationCollection.Add(new() { IconUri = "Edit.png", Title = "Иные параметры", GetContent = Get<TemplateMasterSettingsPanel> });
                } break;

            case User_Permission.Bookkeeper:
                {
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Оплата счетов", GetContent = Get<OrdersObservableControl> });
                } break;

            case User_Permission.SeniorSalesManager:
                {
                    NavigationCollection.Add(new() { IconUri = "File Invoice.png", Title = "Новая заявка", GetContent = Get<OrderEditorControl> });
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Контроль заявок", GetContent = Get<OrdersManagmentControl> });
                    NavigationCollection.Add(new() { IconUri = "Photo gallery.png", Title = "Мои клиенты", GetContent = Get<CRUSClientControl> });
                    NavigationCollection.Add(new() { IconUri = "Settings.png", Title = "База регионов", GetContent = Get<TemplateRegionsEditor> });
                    NavigationCollection.Add(new() { IconUri = "Settings.png", Title = "Мои регионы", GetContent = Get<TemplateRegionsManagment> });
                } break;

            case User_Permission.SalesManager:
                {
                    NavigationCollection.Add(new() { IconUri = "File Invoice.png", Title = "Новая заявка", GetContent = Get<OrderEditorControl> });
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Контроль заявок", GetContent = Get<OrdersManagmentControl> });
                    NavigationCollection.Add(new() { IconUri = "Photo gallery.png", Title = "Мои клиенты", GetContent = Get<CRUSClientControl> });
                    NavigationCollection.Add(new() { IconUri = "Settings.png", Title = "База регионов", GetContent = Get<TemplateRegionsEditor> });
                    NavigationCollection.Add(new() { IconUri = "Settings.png", Title = "Мои регионы", GetContent = Get<TemplateRegionsManagment> });
                } break;

            case User_Permission.ManufactureManager:
                {
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Заявки на производство", GetContent = Get<OrdersManagmentControl> });
                } break;

            case User_Permission.ConstrEngineer:
                {
                } break;

            case User_Permission.ManufactureEmployee:
                {
                } break;

            case User_Permission.Storekeeper:
                {
                    NavigationCollection.Add(new() { IconUri = "Manufacture.png", Title = "Заявки на склад", GetContent = Get<WarehouseKeeperOrders> });
                    NavigationCollection.Add(new() { IconUri = "Warehouse.png", Title = "Весь склад", GetContent = Get<WarehouseGeneral> });
                } break;
        };

        return true;
    }
}

public class NavigationViewModel : ViewModelBase<NavigationModel, NavigationViewModelSource>
{
    public NavigationViewModel() : base(new() { PagesCount = 1 })
    {
    }
}