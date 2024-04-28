using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.DependencyInjection;
using SalutemCRM.TCP;
using SalutemCRM.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Services;

public class FilesContainerService
{
    public static string ContainerPath => $"{Directory.GetCurrentDirectory()}\\Uploaded_files";

    private string GetPath(string Name) => $"{ContainerPath}\\{Name}";

    private List<string> FilesCollection { get; } = new();

    public FilesContainerService() => Directory.CreateDirectory(ContainerPath);

    private FilesContainerService LoadFilesList() =>
        Directory.GetFiles(ContainerPath)
        .Do(x => FilesCollection.Clear())
        .DoForEach(FilesCollection.Add)
        .Do(x => this);

    private void TCPUpload(string Name) => App.Host!.Services.GetService<TCPChannel>()!.Send(Name, MBEnums.GET_FILE_JSON);

    private void Run(string Path) => new ProcessStartInfo(Path) { UseShellExecute = true }.Do(Process.Start);

    public bool OpenFile(string Name)
    {
        LoadFilesList();

        foreach (var path in FilesCollection)
            if (path.Contains(GetPath(Name)))
            {
                Run(path);
                return true;
            }

        TCPUpload(Name);

        return false;
    }
}
