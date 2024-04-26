
using System;
using System.Linq;

namespace SalutemCRM.TCP;

public class DataReceivedArgs : EventArgs, IDisposable
{
    public required string ConnectionId { get; set; }
    public required string Message { get; set; }
    public required TCPChannel ThisChannel { get; set; }

    public void Dispose() => ((IDisposable)ThisChannel).Dispose();
}
