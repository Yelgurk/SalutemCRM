
using System.Collections.Concurrent;

namespace SalutemCRM.TCP;

public class Channels
{
    public ConcurrentDictionary<string, TCPChannel> OpenChannels;
    private readonly TCPServer thisServer;
    
    public Channels(TCPServer myServer)
    {
        OpenChannels = new ConcurrentDictionary<string, TCPChannel>();
        thisServer = myServer;
    }
}