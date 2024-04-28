using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalutemCRM.Domain.Model;
using SalutemCRM.Server.Services;
using SalutemCRM.Server.ViewModels;
using SalutemCRM.Server.Views;
using SalutemCRM.TCP;
using System;
using System.IO;
using System.Text.Json;

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
                .Do(x => x.DataReceived += (o, e) =>
                {
                    switch (e.MessageType)
                    {
                        case MBEnums.STRING: { x.Logging($"New message [{e.ThisChannel.Id}]: {e.Message}"); }; break;
                        case MBEnums.FILE_JSON:
                            {
                                FileAttach? receivedFile = JsonSerializer.Deserialize<FileAttach>(e.Message);
                                
                                using (var fs = new FileStream($"{Directory.GetCurrentDirectory()}\\{receivedFile?.FileName}", FileMode.Create, FileAccess.Write))
                                    fs.Write(receivedFile!.Bytes!, 0, receivedFile!.Bytes!.Length);

                                x.Logging($"New file received [{e.ThisChannel.Id}]: {receivedFile!.FileName}\nFile path: {Directory.GetCurrentDirectory()}\\{receivedFile?.FileName}");

                            }; break;
                        default: break;
                    }
                })
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
