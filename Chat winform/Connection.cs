using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Chat_winform
{
    public class Connection
    {
        public Int32 id;
        public String username;
        public TCP tcp;
        public delegate void PacketHandler(Packet _packet, Connection _connection);
        public static Dictionary<int, PacketHandler> packetHandlers;

        public Connection(TcpClient _client, String _username)
        {
            username = _username;
            tcp = new TCP(_client, this);
            InitializeData();   
        }

        ~Connection()
        {
            cancellationTokenSource.Cancel();
        }

        public class TCP
        {
            public TcpClient client;
            public NetworkStream stream;
            public bool isAlive = true;
            private Packet receivedData;
            private byte[] buffer;
            private bool working;
            private Connection owner;

            public TCP(TcpClient _client, Connection _owner)
            {
                owner = _owner;
                client = _client;
                stream = _client.GetStream();
                working = false;
                buffer = new byte[4096];
                receivedData = new Packet();
            }
            ~TCP()
            {
                stream.Close();
                client.Close();
            }

            public void Send(Packet _packet)
            {
                try
                {
                    _packet.WriteLength();
                    stream.Write(_packet.ToArray(), 0, _packet.Length());
                    stream.Flush();
                }
                catch(Exception e)
                {
                    isAlive = false;
                }
            }
            public void Fetch()
            {
                if(!working)
                {
                    working = true;
                    while (stream.DataAvailable)
                    {
                        int l = stream.Read(buffer, 0, 4096);
                        byte[] data = new byte[l];
                        Array.Copy(buffer, data, l);
                        if (HandleData(data))
                        {
                            receivedData.Reset();
                        }
                    }
                    working = false;
                }
            }

            private bool HandleData(byte[] _data)
            {
                receivedData.SetBytes(_data);

                Int32 _packetLength = 0;

                if (receivedData.UnreadLength() >= sizeof(Int32))
                {
                    _packetLength = receivedData.ReadInt32();
                    if(_packetLength <= 0)
                    {
                        return true;
                    }
                }

                while(_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            Int32 _packetId = _packet.ReadInt32();
                            try
                            {
                                packetHandlers[_packetId](_packet, owner);
                            }
                            catch (Exception e)
                            {
                                ConnectionHandle.AppendToList($"Exception caught: {e.Message} | {_packetId} | {_packet.ToString()}");
                            }
                        }
                    });

                    _packetLength = 0;

                    if(receivedData.UnreadLength() >= sizeof(Int32))
                    {
                        _packetLength = receivedData.ReadInt32();
                        if(_packetLength <= 0)
                        {
                            return true;
                        }
                    }
                }

                if (_packetLength <= 1)
                {
                    return true;
                }

                return false;
            }
        }

        private Thread fetcher;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
    
        private void InitializeData()
        {
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int) ServerPackets.handshake, ConnectionHandle.HandshakeReceived },
                {(int) ServerPackets.message, ConnectionHandle.MessageReceived},
                {(int) ServerPackets.ping, ConnectionHandle.PingReceived},
                {(int) ServerPackets.hostMessage, ConnectionHandle.HostMessageReceived}
            };
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            fetcher = new Thread(() => {
                DateTime nextTick = DateTime.Now;
                Int32 tickDelay = 50;
                while (!cancellationToken.IsCancellationRequested)
                {
                    if(nextTick < DateTime.Now)
                    {
                        nextTick = nextTick.AddMilliseconds(tickDelay);
                        tcp.Fetch();
                    }
                    else
                    {
                        Thread.Sleep(nextTick - DateTime.Now);
                    }

                }
            });
            fetcher.IsBackground = true;
            fetcher.Start();
        }
    }
}
