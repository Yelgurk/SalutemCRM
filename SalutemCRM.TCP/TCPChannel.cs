using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SalutemCRM.TCP
{
    public class TCPChannel : IDisposable
    {
        public string Id { get; private set; }
        public TCPServer thisServer { get; private set; }
        private TcpClient thisClient;
        private readonly byte[] buffer;
        private NetworkStream stream;
        private bool isOpen;
        private bool disposed;
        private MessageBroker messageBroker = new();

        public Action? WhenChannelDisposing { get; set; }
        public Action? WhenChannelDisposed { get; set; }

        public TCPChannel()
        {
            thisServer = new TCPServer();
            thisClient = new TcpClient();
            thisClient.Connect(IPAddress.Parse(thisServer.IpAddress), thisServer.Port);
            buffer = new byte[256];
        }

        public TCPChannel(TCPServer server)
        {
            thisServer = server;
            buffer = new byte[256];
        }

        public void Open(TcpClient client)
        {
            thisClient = client;
            this.Open();
        }

        public void Open()
        {
            Id = thisClient.Client.RemoteEndPoint?.ToString() ?? "[unknown]";
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

                                    messageBroker.IncomingPackage(data)?
                                    .Do(x =>
                                    {
                                        var args = new DataReceivedArgs()
                                        {
                                            ReceivedBytes = x.MessageContainer.Length,
                                            Message = x.MessageContainer,
                                            MessageType = x.MessageType,
                                            ConnectionId = Id,
                                            ThisChannel = this
                                        };

                                        thisServer.OnDataIn(args);
                                    });

                                    if (!isOpen) { break; }
                                }
                            }
                            catch { Close(); }
                        }
                    }
                }
            });
        }

        public void Send(string message, MBEnums type = MBEnums.STRING)
        {
            message = $"{MessageBroker.BeginMessage}{(ushort)type}{message}{MessageBroker.EndMessage}";

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
