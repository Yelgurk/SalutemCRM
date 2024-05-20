using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalutemCRM.Control;
using SalutemCRM.ControlTemplated;
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
        //DatabaseContext.ReCreateDatabase(!Design.IsDesignMode);

        Host =
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices(services => {
                services.AddSingleton<MainWindow>();
                services.AddSingleton<ViewModelSourceNotifyService>();
                services.AddSingleton<QRCodeGeneratorService>();
                services.AddSingleton<QRCodeBleScanService>();
                services.AddSingleton<FilesContainerService>();
                services.AddSingleton<TCPChannel>();

                services.AddSingleton<OrderEditorControl>();
                services.AddSingleton<OrdersObservableControl>();
                services.AddSingleton<OrdersManagmentControl>();
                services.AddSingleton<TemplateRegionsEditor>();
                services.AddSingleton<CRUSClientControl>();
                services.AddSingleton<TemplateRegionsManagment>();
            })
            .Build();

        Host!.Services.GetService<FilesContainerService>();

        //Account.SetAccount(User.RootOrBoss);
        // Account.SetAccount(User.SalesManager);
        //Account.SetAccount(User.ManufactureManager);

        //if (false)
        if (!Design.IsDesignMode)
            Host!.Services.GetService<TCPChannel>()!
                .Do(x => x.thisServer.DataReceived += (o, e) =>
                {
                    switch (e.MessageType)
                    {
                        case MBEnums.FILE_JSON:
                            {
                                List<FileAttach>? receivedFiles = JsonSerializer.Deserialize<List<FileAttach>>(e.Message);

                                string NewFilePath = $"{FilesContainerService.ContainerPath}\\{receivedFiles![0].FileName}";

                                if (receivedFiles![0].FileFounded)
                                    receivedFiles![0]
                                    .Do(x =>
                                    {
                                        using (var fs = new FileStream(NewFilePath, FileMode.Create, FileAccess.Write))
                                            fs.Write(receivedFiles![0].Bytes!, 0, receivedFiles![0].Bytes!.Length);
                                    })
                                    .Do(x => Host!.Services.GetService<FilesContainerService>()!.OpenFile(receivedFiles![0].FileName));
                                else
                                    Debug.WriteLine("FILE WAS NOT FOUNDED IN SERVER SIDE");
                            }; break;

                        case MBEnums.USER_JSON:
                            {
                                User? signIn = JsonSerializer.Deserialize<User>(e.Message);

                                if (signIn != null)
                                {
                                    using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
                                        signIn = db.Users
                                        .Where(x => x.Login == signIn.Login)
                                        .Include(x => x.UserRole)
                                        .First();

                                    signIn.IsSuccess = true;
                                    Account.SetAccount(signIn);
                                }
                            }; break;

                        default: break;
                    }
                })
                .Do(x => x.Open());

        AvaloniaXamlLoader.Load(this);
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
