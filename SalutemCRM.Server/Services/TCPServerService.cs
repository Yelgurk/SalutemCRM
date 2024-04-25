using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain.Model;
using SalutemCRM.Server.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SalutemCRM.Services;

public class TCPServerService
{
    public static string DT => $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}.{DateTime.Now.Millisecond}:";

    private int _port;
    private string? _ipAddress;
    private Socket _serverListener;
    private IPEndPoint _serverEndPoint;

    public ObservableCollection<(Socket, Thread)> ActiveConnections { get; } = new();
    private Socket? _socketAwaiter = default(Socket);

    public TCPServerService()
    {
        LogService.Push = $"{DT} Server running.";

        this.ParseJSONConfig();

        LogService.Push = $"{DT} Config parsing.";

        _serverEndPoint = new(IPAddress.Parse(_ipAddress!), _port);
        _serverListener = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _serverListener.Bind(_serverEndPoint);
        _serverListener.Listen(100);

        LogService.Push = $"{DT} TCP listener preparation.";

        _ = this.TCPConnectionSupervisor();

        LogService.Push = $"{DT} TCP listener is active!";
    }

    private void ParseJSONConfig()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");

        var config = builder.Build();
        _port = Convert.ToInt32($"{config.GetSection("TCPConnectionStrings")["ServerPort"]}");
        _ipAddress = $"{config.GetSection("TCPConnectionStrings")["ServerIP"]}";
    }

    private async Task TCPConnectionSupervisor()
    {
        await Task.Run(() =>
        {
            while (true)
            {
                _socketAwaiter = _serverListener.Accept();
                LogService.Push = $"{DT} new connection [# {ActiveConnections.Count + 1}] - {_socketAwaiter.RemoteEndPoint?.ToString()}";

                ActiveConnections.Add((
                    _socketAwaiter,
                    new Thread(new ThreadStart(() => new ClientThread(
                        _socketAwaiter,
                        x => {
                            LogService.Push = $"{DT} disconnect [# {ActiveConnections.Count - 1}] - {x.RemoteEndPoint?.ToString()}";
                            ActiveConnections.Remove(ActiveConnections.Single(f => f.Item1 == x));
                        }
                        ))).Do(x => x.Start())
                ));
            }
        });
    }
}

public class ClientThread : TCPServiceCommunication
{
    private Action<Socket>? WhenConnectionError;
    private TCPFlags CurrentFlag = TCPFlags.NONE;
    private Socket ClientConnection;
    private bool _isRunning = true;
    private byte[] message_buf = new byte[0];

    public ClientThread(Socket ClientConnection, Action<Socket>? WhenConnectionError)
    {
        this.ClientConnection = ClientConnection;
        this.WhenConnectionError = WhenConnectionError;

        base.SetSocket(this.ClientConnection);
        _ = this.SocketListener();
    }

    private bool CloseConnection()
    {
        this.WhenConnectionError?.Invoke(ClientConnection);
        return _isRunning = false;
    }

    private void Logging(string message)
    {
        if (message.Length > 0)
        {
            Dispatcher.UIThread.Invoke(() => {
                LogService.Push = $"{TCPServerService.DT} receive message from {ClientConnection.RemoteEndPoint?.ToString()}: \"{message}\" [{message.Length}]";
            });
        }
    }

    public async Task<bool> SocketListener()
    {
        return  await Task.Run(() =>
        {
            while (_isRunning)
            {
                try { CurrentFlag = ReceiveFlag(TCPFlags.SIZE); }
                catch { return CloseConnection(); }

                _ = CurrentFlag switch
                {
                    TCPFlags.SIZE => ((Func<bool>)(() => {
                        try { ReceiveSizeInfo(); }
                        catch { return CloseConnection(); }

                        try { message_buf = ReceiveMessage(); }
                        catch { return CloseConnection(); }

                        Logging(Encoding.UTF8.GetString(message_buf));

                        return true;
                    })).Invoke(),

                    TCPFlags.APP_CLOSE => CloseConnection(),
                    _ => true
                };
            }

            return false;
        });
    }
}