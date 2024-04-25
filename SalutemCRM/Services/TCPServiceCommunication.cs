using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Services;

public enum TCPFlags : ushort {
    NONE = 11927,
    AWAIT,
    OK,
    RESPONSE,
    ERROR_NOT_TYPE,
    ERROR_INVALID_SIZE
};

public enum TCPMessage : ushort {
    NONE = 21927,
    APP_CLOSE,
    STRING,
    JSON_MATERIAL_FLOW
};

public class TCPServiceCommunication
{
    protected Socket Socket { set => _socket = value; }
    private Socket? _socket;

    private byte[] type_buf { get; } = new byte[2];
    private TCPMessage type_await = TCPMessage.NONE;

    private byte[] flag_buf { get; } = new byte[2];
    private TCPFlags flag_listener = TCPFlags.NONE;
    
    private byte[] message_buf = new byte[0];
    private int message_size = 0;

    private int received_size = 0;


    private T BytesToFlag<T>(byte[] arr) => ((ushort)((arr[1] << 8) | arr[0])).EnumCast<T>()!;
    private byte[] FlagToBytes<T>(T _flag) => BitConverter.GetBytes((ushort)(object)_flag!);
    protected int SendFlag<T>(T _flag) => _socket?.Send(FlagToBytes(_flag), 0, 2, SocketFlags.None) ?? 0;


    protected void SendMessageType(TCPMessage _messageType)
    {
        do
        {
            flag_listener = TCPFlags.AWAIT;

            SendFlag(_messageType);
            _socket?.Receive(flag_buf);
        }
        while ((flag_listener = BytesToFlag<TCPFlags>(flag_buf)) != TCPFlags.OK);
    }

    protected void SendSizeInfo(byte[] message)
    {
        do
        {
            flag_listener = TCPFlags.AWAIT;

            _socket?.Send(BitConverter.GetBytes(message.Length), 0, BitConverter.GetBytes(message.Length).Length, SocketFlags.None);
            _socket?.Receive(flag_buf);
        }
        while ((flag_listener = BytesToFlag<TCPFlags>(flag_buf)) != TCPFlags.OK);
    }

    protected TCPFlags SendMessage(byte[] message)
    {
        do
        {
            flag_listener = TCPFlags.AWAIT;

            _socket?.Send(message, 0, message.Length, SocketFlags.None);
            _socket?.Receive(flag_buf);

            flag_listener = BytesToFlag<TCPFlags>(flag_buf);
        }
        while (!(flag_listener == TCPFlags.OK || flag_listener == TCPFlags.RESPONSE));

        return flag_listener;
    }

    protected TCPMessage ReceiveMessageType()
    {
        do
        {
            flag_listener = TCPFlags.AWAIT;

            _socket?.Receive(type_buf);

            if (Enum.IsDefined(typeof(TCPMessage), BytesToFlag<TCPMessage>(type_buf)))
                SendFlag(flag_listener = TCPFlags.OK);
            else
                SendFlag(flag_listener = TCPFlags.ERROR_NOT_TYPE);
        }
        while (flag_listener != TCPFlags.OK);

        return BytesToFlag<TCPMessage>(type_buf);
    }

    protected void ReceiveSizeInfo()
    {
        received_size = 0;
        do
        {
            received_size = _socket?.Receive(message_buf = new byte[message_size = 4]) ?? 0;

            if (received_size != 4)
                SendFlag(TCPFlags.ERROR_INVALID_SIZE);
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
        received_size = 0;

        do
        {
            received_size = _socket?.Receive(message_buf = new byte[message_size]) ?? 0;

            if (received_size != message_size)
                SendFlag(TCPFlags.ERROR_INVALID_SIZE);
            else _ = _willBeResponsed switch
            {
                true => SendFlag(TCPFlags.RESPONSE),
                _ => SendFlag(TCPFlags.OK)
            };
        }
        while (received_size != message_size);

        return message_buf;
    }

    public void Send(string message) => this.Send(Encoding.UTF8.GetBytes(message), TCPMessage.STRING);

    public void Send(byte[] message, TCPMessage type)
    {
        SendMessageType(type);
        SendSizeInfo(message);

        if (SendMessage(message) == TCPFlags.RESPONSE)
            Receive();
    }

    public string ReceiveString() => Receive().Do(x => x.type == TCPMessage.STRING ? Encoding.UTF8.GetString(x.message) : "[ERROR | SERVER SEND OBJECT, NOT STRING]");

    public (TCPMessage type, byte[] message) Receive()
    {
        type_await = ReceiveMessageType();
        ReceiveSizeInfo();

        return (type_await, ReceiveMessage());
    }
}