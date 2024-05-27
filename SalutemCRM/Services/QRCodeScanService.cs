using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ReactiveUI;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO.Ports;
using System.Timers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Avalonia;
using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Identity.Client;

namespace SalutemCRM.Services;

public static class QRCodeScanService
{
    private static SerialPort? QRScannerComPort = null;
    private static string UartBuff = "";
    private static string Port = "";

    public static bool Init()
    {
        if (QRScannerComPort is not null)
            return true;

        bool _success = false;

        try
        {
            new ConfigurationBuilder()
                .Do(x => x.SetBasePath(Directory.GetCurrentDirectory()))
                .Do(x => x.AddJsonFile("appsettings.json"))
                .Do(x => Port = x.Build().GetSection("QRCodeScanner")["Port"]!);

            QRScannerComPort = new SerialPort(Port, 115200, Parity.None, 8, StopBits.One);
            QRScannerComPort.Handshake = Handshake.None;
            QRScannerComPort.DataReceived += (o, e) => Dispatcher.UIThread.Invoke(() =>
            {
                char[] buffer = new char[QRScannerComPort.BytesToRead];
                QRScannerComPort.Read(buffer, 0, buffer.Length);

                UartBuff += string.Join("", buffer);
                if (UartBuff.Contains((char)0x0D))
                {
                    QRCodeScannedEvent?.Invoke(UartBuff.Split((char)0x0D).First());
                    UartBuff = UartBuff.Remove(0, UartBuff.Split((char)0x0D).First().Length + 1);
                }
            });
            QRScannerComPort.Open();

            _success = true;
        }
        catch { }

        return _success;
    }

    public static event Action<string>? QRCodeScannedEvent;
}
