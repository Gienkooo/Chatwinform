using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace Chat_server
{
    public class Client
    {
        public Int32 id;
        public String username;
        public TCP tcp;
        public bool ready = false;

        public Client(TcpClient _client, Int32 _id)
        {
            tcp = new TCP(_client, this);
            id = _id;
        }

        ~Client()
        {
            Console.WriteLine($"Destroying client. id:{id}");
        }

        public class TCP
        {   
            public TcpClient tcpClient;
            public NetworkStream stream;
            public bool isAlive = true;
            private Packet receivedData;
            private byte[] buffer;
            private Client owner;
            private bool working;

            public TCP(TcpClient _client, Client _owner)
            {
                tcpClient = _client;
                stream = _client.GetStream();
                receivedData = new Packet();
                working = false;
                buffer = new byte[4096];
                owner = _owner;
            }
            ~TCP()
            {
                stream.Close();
                tcpClient.Close();
            }

            public void Send(Packet _packet)
            {
                try
                {
                    stream.Write(_packet.ToArray(), 0, _packet.Length());
                    stream.Flush();
                }
                catch (System.IO.IOException)
                {
                    Console.WriteLine($"Client disconnected. | id:{owner.id}");
                    isAlive = false;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    isAlive = false;
                }
            }
            public void Fetch()
            {
                try
                {
                    if (!working)
                    {
                        working = true;
                        while (stream.DataAvailable)
                        {
                            int l = stream.Read(buffer, 0, 4096);
                            byte[] data = new byte[l];
                            Array.Copy(buffer, data, l);
                            receivedData.Reset(HandleData(data));
                        }
                        working = false;
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    isAlive = false;
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
                    Console.WriteLine($"Received packet of length {_packetLength}");
                }

                while(_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {
                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                    ThreadManager.ExecuteOnMainThread(() =>
                    {
                        using (Packet _packet = new Packet(_packetBytes))
                        {
                            ClientPackets packetType = (ClientPackets) _packet.ReadInt32();
                            Console.WriteLine($"Handling packet type {packetType}");
                            Server.packetHandlers[(int)packetType](owner.id, _packet);
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

                    if (_packetLength <= 1)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}