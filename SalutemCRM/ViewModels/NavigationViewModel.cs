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
    [ObservableProperty]
    private ObservableCollection<NavigationModel> _navigationCollection = new();

    private object Get<T>() where T : class => App.Host!.Services.GetService<T>()!;


    public NavigationViewModelSource()
    {
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
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Контроль заявок", GetContent = Get<OrdersManagmentControl> });
                } break;

            case User_Permission.Bookkeeper:
                {
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Оплата счетов", GetContent = Get<OrdersObservableControl> });
                } break;

            case User_Permission.SeniorSalesManager:
                {
                    NavigationCollection.Add(new() { IconUri = "File Invoice.png", Title = "Новый счёт", GetContent = Get<OrderEditorControl> });
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Контроль заявок", GetContent = Get<OrdersManagmentControl> });
                } break;

            case User_Permission.SalesManager:
                {
                    NavigationCollection.Add(new() { IconUri = "File Invoice.png", Title = "Новый счёт", GetContent = Get<OrderEditorControl> });
                    NavigationCollection.Add(new() { IconUri = "Books.png", Title = "Контроль заявок", GetContent = Get<OrdersManagmentControl> });
                } break;

            case User_Permission.ManufactureManager:
                {
                } break;

            case User_Permission.ConstrEngineer:
                {
                } break;

            case User_Permission.ManufactureEmployee:
                {
                } break;

            case User_Permission.Storekeeper:
                {
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