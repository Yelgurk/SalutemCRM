using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SalutemCRM.TCP;

public class TCPServer
{

    public event EventHandler<DataReceivedArgs>? DataReceived;
    public Channels? ConnectedChannels;
    private TcpListener Listener;

    public Action<string>? LoggingAction { private get; set; }
    public void Logging(string log) => LoggingAction?.Invoke(log);

    private bool _running;
    public bool Running
    {
        get => _running;
        private set => _running = value;
    }

    private string _ipAddress = "0.0.0.0";
    public string IpAddress
    {
        get => _ipAddress;
        private set => _ipAddress = value;
    }

    private int _port = 12321;
    public int Port
    {
        get => _port;
        private set => _port = value;
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

    public TCPServer()
    {
        ParseJSONConfig();
        Listener = new TcpListener(IPAddress.Parse(IpAddress), Port);
        ConnectedChannels = new Channels(this);
    }

    public async void Start()
    {
        Logging($"Runned as {IpAddress}:{Port}");
        Logging("TCP listener is active!");

        try
        {
            Listener.Start();
            Running = true;
            while (Running)
            {
                var client = await Listener.AcceptTcpClientAsync();
                _ = Task.Run(() => new TCPChannel(this).Open(client));

                Logging($"New user connected as [{client.Client.RemoteEndPoint?.ToString()}]");
            }

        }
        catch(SocketException)
        {
            throw;
        }
        catch(ChannelRegistrationException)
        {
            throw;
        }
    }

    public void Stop()
    {
        Listener.Stop();
        Running = false;
    }

    public void OnDataIn(DataReceivedArgs e) => DataReceived?.Invoke(this, e);
}
