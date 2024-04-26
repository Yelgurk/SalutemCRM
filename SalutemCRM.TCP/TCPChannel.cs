using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text;

namespace SalutemCRM.TCP
{
    public class TCPChannel : IDisposable
    {
        public string Id { get; private set; }
        private TCPServer thisServer;
        private TcpClient thisClient;
        private readonly byte[] buffer;
        private NetworkStream stream;
        private bool isOpen;
        private bool disposed;

        public Action? WhenChannelDisposing { get; set; }
        public Action? WhenChannelDisposed { get; set; }

        public TCPChannel(TCPServer myServer)
        {
            thisServer = myServer;
            buffer = new byte[256];
            //Id = Guid.NewGuid().ToString();
        }

        public void Open(TcpClient client)
        {
            Id = (thisClient = client).Client.RemoteEndPoint?.ToString() ?? "[unknown]";
            isOpen = true;

            if(!thisServer.ConnectedChannels!.OpenChannels.TryAdd(Id, this))
            {
                isOpen = false;
                throw (new ChannelRegistrationException("Unable to add channel to channel list"));
            }

            string data = "";

            Task.Run(() =>
            {
                using (stream = thisClient.GetStream())
                {
                    int position;

                    while (isOpen)
                    {
                        if (clientDisconnected())
                            Close();
                        else
                        {
                            try
                            {
                                while ((position = stream.Read(buffer, 0, buffer.Length)) != 0 && isOpen)
                                {
                                    data = Encoding.UTF8.GetString(buffer, 0, position);
                                    var args = new DataReceivedArgs()
                                    {
                                        Message = data,
                                        ConnectionId = Id,
                                        ThisChannel = this
                                    };

                                    thisServer.OnDataIn(args);
                                    if (!isOpen) { break; }
                                }
                            }
                            catch { Close(); }
                        }
                    }
                }
            });
        }

        public void Send(string message)
        {
            var data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public void Close()
        {
            Dispose(false);
            isOpen = false;
            thisServer.Logging($"Connection closed [{Id}]");
            thisServer.ConnectedChannels!.OpenChannels.TryRemove(Id, out _);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    WhenChannelDisposing?.Invoke();

                stream.Close();
                thisClient.Close();
                disposed = true;

                if (disposing)
                    WhenChannelDisposed?.Invoke();
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private bool clientDisconnected() => thisClient.Client.Available == 0 && thisClient.Client.Poll(1, SelectMode.SelectRead);
    }
}
