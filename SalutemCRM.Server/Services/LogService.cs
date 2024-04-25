using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Server.Services;

public class LogRecord
{
    public string Id { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string Message { get; set; }
}

public static class LogService 
{
    public static ObservableCollection<LogRecord> Logger { get; } = new();

    public static void Push(LogRecord record) => Logger.Add(record.DoInst(x => x.Id = $"{Logger.Count + 1}"));

    public static void Clear() => Logger.Clear();
}
