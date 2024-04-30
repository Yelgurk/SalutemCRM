
using System;
using System.Linq;

namespace SalutemCRM.TCP;

public class DataReceivedArgs : EventArgs, IDisposable
{
    public string ConnectionId { get; set; }
    public TCPChannel ThisChannel { get; set; }
    public required MBEnums MessageType { get; set; }
    public required string Message { get; set; }
    public required int ReceivedBytes { get; set; }

    public void Dispose() => ((IDisposable)ThisChannel).Dispose();
}
