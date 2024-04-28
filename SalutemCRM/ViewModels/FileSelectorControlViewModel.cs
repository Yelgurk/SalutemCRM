using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using SalutemCRM.Database;
using SalutemCRM.Domain.Model;
using SalutemCRM.Reactive;
using SalutemCRM.Services;
using SalutemCRM.TCP;
using SalutemCRM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reactive;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SalutemCRM.ViewModels;

public partial class FileSelectorControlViewModelSource : ReactiveControlSource<FileAttach>
{
    [ObservableProperty]
    private bool _isAddNewAvailable = true;

    [ObservableProperty]
    private bool _isClearAllAvailable = true;

    [ObservableProperty]
    private bool _isRemoveFileAvailable = true;

    public static ObservableCollection<FileAttach> FilesCollection { get; } = new();

     public static void FillCollection(List<FileAttach> Files) => Files
        .Do(x => FilesCollection.Clear())
        .Do(x => x.DoForEach(file => FilesCollection.Add(file)));

    private static void RemoveNotLocalFiles() => FilesCollection.DoForEach(x => x.DoIf(file => FilesCollection.Remove(file), file => file.FileLocalPath == ""));

    private static void SendFilesToTCPServer()
    {
        App.Host!.Services.GetService<TCPChannel>()!.Send(JsonSerializer.Serialize(FilesCollection.ToList()), MBEnums.FILE_JSON);
        FilesCollection.Clear();
    }

    public static void AttachFilesTo(Order order) => FilesCollection
        .Do(x => RemoveNotLocalFiles())
        .DoInst(x => x.DoForEach(file => file.OrderForeignKey = order.Id))
        .DoIf(x => SendFilesToTCPServer(), x => x.Count > 0);

    public static void AttachFilesTo(WarehouseItem warehouseItem) => FilesCollection
        .Do(x => RemoveNotLocalFiles())
        .DoInst(x => x.DoForEach(file => file.WarehouseItemForeignKey = warehouseItem.Id))
        .DoIf(x => SendFilesToTCPServer(), x => x.Count > 0);

    public static void AttachFilesTo(MaterialFlow materialFlow) => FilesCollection
        .Do(x => RemoveNotLocalFiles())
        .DoInst(x => x.DoForEach(file => file.MaterialFlowForeignKey = materialFlow.Id))
        .DoIf(x => SendFilesToTCPServer(), x => x.Count > 0);
}

public partial class FileSelectorControlViewModel : ViewModelBase<FileAttach, FileSelectorControlViewModelSource>
{
    public ReactiveCommand<Unit, Unit>? RemoveAllFilesCommand { get; protected set; }
    public ReactiveCommand<Unit, Unit>? UploadFilesToServer { get; protected set; }

    public FileSelectorControlViewModel() : base(new() { PagesCount = 1 })
    {
        AddNewCommand = ReactiveCommand.Create(() => {
            Task.Run(async () => {
                var topLevel = TopLevel.GetTopLevel(App.Host!.Services.GetService<MainWindow>()!);

                var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Выберите файл",
                    AllowMultiple = false
                });

                if (files.Count >= 1)
                {
                    FileAttach newFile = new();

                    await using var stream = await files[0].OpenReadAsync();
                    stream
                        .DoInst(x => x.Read(newFile.Bytes = new byte[x.Length], 0, Convert.ToInt32(x.Length)))
                        .Do(x => x.Close());

                    FileSelectorControlViewModelSource.FilesCollection.Add(newFile.SetLocalFileNamePath(files[0].Path.ToString()));
                }
            });
        });

        RemoveCommand = ReactiveCommand.Create<FileAttach>(x => {
            FileSelectorControlViewModelSource.FilesCollection.Remove(x);
        });

        RemoveAllFilesCommand = ReactiveCommand.Create(() => {
            FileSelectorControlViewModelSource.FilesCollection.Clear();
        });

        UploadFilesToServer = ReactiveCommand.Create(() => {
            //App.Host!.Services.GetService<TCPChannel>()!.Send("Two_Feet_-_BBY_(basovtut.ru).mp3", MBEnums.GET_FILE_JSON);
            App.Host!.Services.GetService<FilesContainerService>()!.OpenFile("Two_Feet_-_BBY_(basovtut.ru).mp3");
            App.Host!.Services.GetService<FilesContainerService>()!.OpenFile("Безымянный.png");
        });
    }
}
