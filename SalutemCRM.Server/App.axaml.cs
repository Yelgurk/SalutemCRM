using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalutemCRM.Server.Services;
using SalutemCRM.Server.ViewModels;
using SalutemCRM.Server.Views;
using SalutemCRM.TCP;
using System;

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
                services.AddSingleton<TCPServer>();
            })
            .Build();

        if (!Design.IsDesignMode)
            Host!.Services.GetService<TCPServer>()!
                .DoInst(x => x.LoggingAction = (sysMessage) =>
                {
                    LogService.Push(new LogRecord()
                    {
                        Date = DateTime.Now.ToShortDateString(),
                        Time = DateTime.Now.ToLongTimeString(),
                        Message = sysMessage
                    });
                })
                .Do(x => x.DataReceived += (o, e) => x.Logging($"New message [{e.ConnectionId}]: {e.Message}"))
                //.Do(x => x.DataReceived += (o, e) => e.Message.DoIf(s => e.ThisChannel.Send("[RECEIVED LONG MESSAGE]"), s => s.Length > 10))
                .Do(x => x.Start());
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
