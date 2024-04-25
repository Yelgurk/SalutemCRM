using Avalonia.Threading;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
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

public enum TCPFlags : ushort { NONE = 11927, AWAIT, RESPONSE, SIZE, OK, ERROR, REPEAT, APP_CLOSE };
public enum TCPMessage : ushort { STRING = 21927, JSON_MATERIAL_FLOW };

public class TCPServiceCommunication
{
    private Socket? _socket;
    private TCPFlags _flagResponse = TCPFlags.NONE;

    private byte[] message_buf = new byte[0];
    private int message_size = 0;

    private byte[] FlagToBytes(TCPFlags _flag) => BitConverter.GetBytes((ushort)_flag);
    
    private TCPFlags BytesToFlag(byte[] arr) => ((ushort)((arr[1] << 8) | arr[0])).EnumCast<TCPFlags>();

    protected void SetSocket(Socket _socket) => this._socket = _socket;

    protected int SendFlag(TCPFlags _flag) => _socket?.Send(FlagToBytes(_flag), 0, 2, SocketFlags.None) ?? 0;

    protected void SendSizeFlag()
    {
        do
        {
            _flagResponse = TCPFlags.AWAIT;

            SendFlag(TCPFlags.SIZE);
            _socket?.Receive(message_buf = new byte[message_size = 2]);
        }
        while ((_flagResponse = BytesToFlag(message_buf)) != TCPFlags.OK);
    }

    protected void SendSizeInfo(byte[] message)
    {
        do
        {
            _flagResponse = TCPFlags.AWAIT;

            _socket?.Send(BitConverter.GetBytes(message.Length), 0, BitConverter.GetBytes(message.Length).Length, SocketFlags.None);
            _socket?.Receive(message_buf = new byte[message_size = 2]);
        }
        while ((_flagResponse = BytesToFlag(message_buf)) != TCPFlags.OK);
    }

    protected TCPFlags SendMessage(byte[] message)
    {
        do
        {
            _flagResponse = TCPFlags.AWAIT;

            _socket?.Send(message, 0, message.Length, SocketFlags.None);
            _socket?.Receive(message_buf = new byte[message_size = 2]);

            Debug.WriteLine(BytesToFlag(message_buf).ToString());
        }
        while ((_flagResponse = BytesToFlag(message_buf)) != TCPFlags.OK);

        return _flagResponse;
    }

    protected TCPFlags ReceiveFlag(TCPFlags _awaitFlag = TCPFlags.NONE)
    {
        do
        {
            _socket?.Receive(message_buf = new byte[message_size = 2]);

            if (_awaitFlag == TCPFlags.NONE || BytesToFlag(message_buf) == _awaitFlag)
                SendFlag(TCPFlags.OK);
            else
                SendFlag(TCPFlags.ERROR);
        }
        while (_awaitFlag == TCPFlags.NONE ? false : (BytesToFlag(message_buf) != _awaitFlag));

        return BytesToFlag(message_buf);
    }

    protected void ReceiveSizeInfo()
    {
        int received_size = 0;
        do
        {
            received_size = _socket?.Receive(message_buf = new byte[message_size = 4]) ?? 0;

            if (message_size != 4)
                SendFlag(TCPFlags.ERROR);
            else
            {
                message_size = BitConverter.ToInt32(message_buf, 0);
                SendFlag(TCPFlags.OK);
            }
        }
        while (received_size != 4);
    }

    protected byte[] ReceiveMessage(bool _willBeResponsed = false)
    {
        int final_size = 0;

        do
        {
            final_size = _socket?.Receive(message_buf = new byte[message_size]) ?? 0;

            Debug.WriteLine($"wait = {message_size}; receive = {final_size}");

            if (final_size != message_size)
                SendFlag(TCPFlags.ERROR);
            else _ = _willBeResponsed switch
            {
                true => SendFlag(TCPFlags.RESPONSE),
                _ => SendFlag(TCPFlags.OK)
            };
        }
        while (final_size != message_size);

        return message_buf;
    }
}

public class TCPClientService : TCPServiceCommunication
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

        base.SetSocket(_clientConnection);
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

    public void NotifyClosed() => this.SendFlag(TCPFlags.APP_CLOSE);

    public void Send(string message) => this.Send(Encoding.UTF8.GetBytes(message));

    public void Send(byte[] message)
    {
        SendSizeFlag();
        SendSizeInfo(message);

        if (SendMessage(message) == TCPFlags.RESPONSE)
            Receive();
    }

    public string ReceiveString() => Encoding.UTF8.GetString(Receive());

    public byte[] Receive()
    {
        ReceiveFlag(TCPFlags.SIZE);
        ReceiveSizeInfo();
        return ReceiveMessage();
    }

    public void CloseConnection() => _clientConnection.Close();
}