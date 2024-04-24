using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SalutemCRM.Services;

public class TCPClientService
{
    private int _port;
    private string? _ipAddress;
    private Socket _clientConnection;
    private IPEndPoint _clientEndPoint;

    public TCPClientService()
    {
        this.ParseJSONConfig();

        _clientEndPoint     = new(IPAddress.Parse(_ipAddress!), _port);
        _clientConnection   = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _clientConnection.Connect(_clientEndPoint);
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

    public void Send(string message) => this.Send(Encoding.ASCII.GetBytes(message));

    public void Send(byte[] message)
    {
        _clientConnection.Send(message, 0, message.Length, SocketFlags.None);

        byte[] serverResponse = new byte[1024];
        int size = _clientConnection.Receive(serverResponse);
        Debug.WriteLine(Encoding.ASCII.GetString(serverResponse, 0, size));
    }

    public void CloseConnection() => _clientConnection.Close();
}