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
    ERROR
};

public enum TCPMessage : ushort {
    NONE = 21927,
    STRING,
    JSON_MATERIAL,
    APP_CLOSE,
    END_TRANSMITTION,
    SEND_BEGIN,
    SEND_TYPE,
    SEND_SIZE,
    SEND_MESSAGE,
    SEND_END,
    GET_BEGIN,
    GET_TYPE,
    GET_SIZE,
    GET_MESSAGE,
    GET_END
};

public class TCPResult
{
    public TCPMessage type { get; set; } = TCPMessage.NONE;
    public byte[] message { get; set; } = new byte[0];
}

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

    private ushort BytesToUShort(byte[] arr) => (ushort)((arr[1] << 8) | arr[0]);
    private T BytesToFlag<T>(byte[] arr) => BytesToUShort(arr).EnumCast<T>()!;
    private byte[] FlagToBytes<T>(T _flag) => BitConverter.GetBytes((ushort)(object)_flag!);
    protected int SendFlag<T>(T _flag) => _socket?.Send(FlagToBytes(_flag), 0, 2, SocketFlags.None) ?? 0;


    protected bool SendMessageType(TCPMessage _messageType)
    {
        SendFlag(_messageType);
        _socket?.Receive(flag_buf);

        return BytesToFlag<TCPFlags>(flag_buf) == TCPFlags.OK;
    }

    protected bool SendSizeInfo(byte[] message)
    {
        _socket?.Send(BitConverter.GetBytes(message.Length), 0, BitConverter.GetBytes(message.Length).Length, SocketFlags.None);
        _socket?.Receive(flag_buf);

        return BytesToFlag<TCPFlags>(flag_buf) == TCPFlags.OK;
    }

    protected bool SendMessage(byte[] message)
    {
        _socket?.Send(message, 0, message.Length, SocketFlags.None);
        _socket?.Receive(flag_buf);

        return BytesToFlag<TCPFlags>(flag_buf) == TCPFlags.OK;
    }

    protected bool ReceiveMessageType()
    {
        type_await = TCPMessage.NONE;

        _socket?.Receive(type_buf);

        if (Enum.IsDefined(typeof(TCPMessage), BytesToUShort(type_buf)))
            SendFlag(flag_listener = TCPFlags.OK);
        else
            SendFlag(flag_listener = TCPFlags.ERROR);

        type_await = BytesToFlag<TCPMessage>(type_buf);

        return flag_listener == TCPFlags.OK;
    }

    protected bool ReceiveSizeInfo()
    {
        if ((received_size = _socket?.Receive(message_buf = new byte[4]) ?? 0) != 4)
            SendFlag(TCPFlags.ERROR);
        else
        {
            message_size = BitConverter.ToInt32(message_buf, 0);
            SendFlag(TCPFlags.OK);
        }

        return received_size == 4;
    }

    protected bool ReceiveMessage()
    {
        if ((received_size = _socket?.Receive(message_buf = new byte[message_size]) ?? 0) != message_size)
            SendFlag(TCPFlags.ERROR);
        else
            SendFlag(TCPFlags.OK);

        return received_size == message_size;
    }

    private bool Send(byte[] message, TCPMessage type)
    {
        if (!SendMessageType(type))
            return true;

        if (!SendSizeInfo(message))
            return true;

        if (!SendMessage(message))
            return true;

        return false;
    }

    private bool Receive()
    {
        if (!ReceiveMessageType())
            return true;
        else
        if (type_await == TCPMessage.END_TRANSMITTION)
            return false;

        if (!ReceiveSizeInfo())
            return true;

        if (!ReceiveMessage())
            return true;

        return false;
    }

    public async Task<TCPResult?> SendAsync(string message) => await this.SendAsync(Encoding.UTF8.GetBytes(message), TCPMessage.STRING);

    public async Task<TCPResult?> SendAsync(byte[] message, TCPMessage type)
    {
        while (await Task.Run(() => Send(message, type))) ;

        return await ReceiveAsync();
    }

    public async Task<TCPResult?> ReceiveAsync()
    {
        while (await Task.Run(() => Receive())) ;

        return type_await switch
        {
            TCPMessage.END_TRANSMITTION => null,
            _ => new TCPResult() { type = type_await, message = message_buf }
        };
    }
}