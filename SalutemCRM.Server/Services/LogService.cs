using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Server.Services;

public static class LogService 
{
    public static ObservableCollection<string> Logger { get; } = new();

    public static string Push { set => Logger.Add($"{Logger.Count + 1}. {value}"); }

    public static void Clear() => Logger.Clear();
}
