using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SalutemCRM.Server.ViewModels;
using SalutemCRM.Server.Views;
using SalutemCRM.Services;

namespace SalutemCRM.Server;

public partial class App : Application
{
    public static IHost? Host { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        Host =
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(services => {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<FilesUploadingService>();
                services.AddSingleton<TCPServerService>();
            })
            .Build();

        if (!Design.IsDesignMode)
            Host!.Services.GetService<TCPServerService>();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
