using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalutemCRM.Database;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using SalutemCRM.Views;
using System.Collections.Generic;

namespace SalutemCRM;

public partial class App : Application
{
    public static IHost? Host { get; private set; }

    public override void Initialize()
    {
        DatabaseContext.ReCreateDatabase(!Design.IsDesignMode);

        AvaloniaXamlLoader.Load(this);
        Host =
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(services => {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<ViewModelSourceNotifyService>();
                services.AddSingleton<QRCodeGeneratorService>();
                services.AddSingleton<QRCodeBleScanService>();
                services.AddSingleton<FilesUploadingService>();
                services.AddSingleton<TCPClientService>();
            })
            .Build();

        if (!Design.IsDesignMode)
            Host!.Services.GetService<TCPClientService>();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow =
                Host!
                .Services
                .GetRequiredService<MainWindow>();

            desktop.Exit += (o, e) => App.Host!.Services.GetService<TCPClientService>()!.NotifyClosed();
            desktop.Exit += (o, e) => App.Host!.Services.GetService<TCPClientService>()!.CloseConnection();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
