using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using SalutemCRM.TCP;
using SalutemCRM.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

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
                services.AddSingleton<TCPChannel>();
            })
            .Build();

        //if (false)
        if (!Design.IsDesignMode)
            Host!.Services.GetService<TCPChannel>()!
                .Do(x => x.thisServer.DataReceived += (o, e) => Debug.WriteLine(e.Message))
                .Do(x => x.Open());

        SendFile();
    }

    private void SendFile()
    {
        FileAttach myFile = new FileAttach()
        {

            FileName = "Diagramm_28_04_2024.png",
            FileLocalPath = "C:\\Users\\Xell\\Desktop\\image_2024-04-04_18-56-04.png"
        };

        new FileStream(myFile.FileLocalPath, FileMode.Open, FileAccess.Read)
            .DoInst(x => x.Read(myFile.Bytes = new byte[x.Length], 0, Convert.ToInt32(x.Length)))
            .Do(x => x.Close());

        Host!.Services.GetService<TCPChannel>()!.Send(JsonSerializer.Serialize(myFile), MBEnums.FILE_JSON);
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
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
