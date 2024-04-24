using Microsoft.Extensions.Configuration;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SalutemCRM.Services;

public class TCPConnectionService
{
    private int _port;
    private string _ipAddress;

    private TcpClient? _tcpClient;
    private TcpListener? _tcpListener;
    private Thread? _listenerThread;

    NetworkStream? stream { get; set; }
    byte[] buffer { get; set; } = new byte[256];
    string? response { get; set; }

    public TCPConnectionService()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");

        var config = builder.Build();
        _port       = Convert.ToInt32($"{config.GetSection("TCPConnectionStrings")["ServerPort"]}");
        _ipAddress  = $"{config.GetSection("TCPConnectionStrings")["ServerIP"]}";
    }

    public TCPConnectionService StartAsServer()
    {
        _tcpListener = new(IPAddress.Parse(_ipAddress), _port);
        _tcpListener.Start();
        this.CreateListenerThread(ServerSideListenerLogic);

        return this;
    }

    public TCPConnectionService StartAsClient()
    {
        _tcpClient = new(_ipAddress, _port);
        this.CreateListenerThread(ClientSideListenerLogic);

        return this;
    }

    private void CreateListenerThread(Action listenerLogic)
    {
        _listenerThread = new(() => listenerLogic());
        _listenerThread.Start();
    }

    private void ServerSideListenerLogic()
    {
        try
        {
            while (true)
            {
                Debug.Write("Waiting for a connection... ");

                TcpClient client = _tcpListener!.AcceptTcpClient();
                Debug.WriteLine("Connected!");

                NetworkStream stream = client.GetStream();

                int i;

                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    response = Encoding.ASCII.GetString(buffer, 0, i);
                    Debug.WriteLine("Received: {0}", response);

                    response = response.ToUpper();

                    byte[] msg = Encoding.ASCII.GetBytes(response);

                    stream.Write(msg, 0, msg.Length);
                    Debug.WriteLine("Sent: {0}", response);
                }

                client.Close();
            }
        }
        catch (SocketException e)
        {
            Debug.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            _tcpListener?.Stop();
            _tcpListener = new(IPAddress.Parse(_ipAddress), _port);
            _tcpListener.Start();

            Debug.WriteLine("Listener socket reinit");
        }
    }

    private void ClientSideListenerLogic()
    {
        int counter = 0;

        try
        {
            NetworkStream stream = _tcpClient?.GetStream()!;

            while (true)
            {
                buffer = Encoding.ASCII.GetBytes($"Counter send {++counter}");

                stream.Write(buffer, 0, buffer.Length);

                buffer = new Byte[256];

                Int32 bytes = stream.Read(buffer, 0, buffer.Length);
                response = Encoding.ASCII.GetString(buffer, 0, bytes);

                Console.WriteLine("Received: {0}", response);

                System.Threading.Thread.Sleep(5000); // Пингуем соединение каждые 5 секунд
            }
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }
}
