using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace Chat_server
{
    static class IdIssuer
    {
        private static Int32 id = 1;
        private static object lockObj = new object();
        public static Int32 Next()
        {
            lock (lockObj)
            {
                return id++;
            }
        }
        public static Int32 Any()
        {
            return -1;
        }
    }
    class Server
    {
        public static ConcurrentDictionary<Int32, Client> clients;
        public delegate void PacketHandler(int _fromClient, Packet _packet);
        public static Dictionary<int, PacketHandler> packetHandlers;
        public static String joinMessage = " has joined the chat!";
        public static String leaveMessage = " has disconnected from the chat!";

        public static void Run()
        {
            InitializeServerData();   
            listener.Start();
            listener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);
            Console.WriteLine($"Server started on port {Port}!");
            while (true)
            {
                if (nextLoop < DateTime.Now)
                {
                    nextLoop = nextLoop.AddMilliseconds(TickDelay);

                    BroadcastPackets();
                    if (nextFetch < DateTime.Now)
                    {
                        TryFetch();
                        nextFetch = nextFetch.AddMilliseconds(TryFetchDelay);
                    }
                    if(nextPing < DateTime.Now)
                    {
                        ServerSend.PingAll();
                        nextPing = nextPing.AddMilliseconds(PingDelay);
                    }
                    if (nextRecollect < DateTime.Now)
                    {
                        Recollect();
                        nextRecollect = nextRecollect.AddMilliseconds(RecollectDelay);
                    }
                }
                else
                {
                    Thread.Sleep(nextLoop - DateTime.Now);
                }
            }
        }

        public static void TcpConnectionCallback(IAsyncResult ar)
        {
            TcpClient _tcpClient = listener.EndAcceptTcpClient(ar);
            listener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectionCallback), null);
            Console.WriteLine($"Incoming connection from {_tcpClient.Client.RemoteEndPoint.ToString()}");
            if (clients.Count >= MaxUsers)
            {
                Console.WriteLine($"Server is full. Refusing connection");
                Client _refusedClient = new Client(_tcpClient, -1);
                ServerSend.RefuseConnection(_refusedClient);
            }

            Client _client = new Client(_tcpClient, IdIssuer.Next());
            if (clients.TryAdd(_client.id, _client))
            {
                Console.WriteLine($"Client {_client.id} successfully added | {_client.tcp.tcpClient.Client.LocalEndPoint.ToString()}");
                ServerSend.PerformHandshake(_client);
            }
            else
            {
                Console.WriteLine($"Client {_client.id} already exists");
            }
        }

        public static void EnqueuePacket(Packet _packet)
        {
            packetQueue.Enqueue(_packet);
        }

        static TcpListener listener;
        static ConcurrentQueue<Packet> packetQueue;
        static object clientLock;
        static private bool isBroadcast = false;
        static Int32 MaxUsers = 10;
        static Int32 Port = 2137;
        static Int32 RecollectDelay = 200;
        static Int32 TryFetchDelay = 50;
        static Int32 PingDelay = 50;
        static Int32 TicksPerSecond = 2;
        static Int32 TickDelay = 1000 / TicksPerSecond;
        static DateTime nextLoop = DateTime.Now;
        static DateTime nextFetch = DateTime.Now;
        static DateTime nextPing = DateTime.Now;
        static DateTime nextRecollect = DateTime.Now;

        private static void InitializeServerData()
        {
            packetQueue = new ConcurrentQueue<Packet>();
            clients = new ConcurrentDictionary<Int32, Client>();
            listener = new TcpListener(IPAddress.Any, Port);
            clientLock = new object();
            packetHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.handshake, ServerHandle.HandshakeReceived},
                {(int)ClientPackets.message, ServerHandle.MessageReceived}
            };
        }

        private static void Recollect()
        {
            foreach(var c in clients)
            {
                if (!c.Value.tcp.isAlive)
                {
                    Client _removedClient;
                    clients.TryRemove(c.Key, out _removedClient);
                    Packet _packet = new Packet((int)ServerPackets.hostMessage);
                    _packet.Write(_removedClient.username + leaveMessage);
                    _packet.WriteLength();
                    EnqueuePacket(_packet);
                    Console.WriteLine($"Client removed | id:{_removedClient.id}");
                }
            }
        }

        private static void TryFetch()
        {
            foreach (var c in clients)
            {
                c.Value.tcp.Fetch();
            }
        }

        private static void BroadcastPackets()
        {
            if (Server.clients.IsEmpty || isBroadcast || packetQueue.IsEmpty)
            {
                return;
            }
            isBroadcast = true;
            Packet _packet;
            Console.WriteLine("Packet queue not empty. Performing broadcast!");
            while (!packetQueue.IsEmpty)
            {
                packetQueue.TryDequeue(out _packet);
                ServerSend.BroadcastPacket(_packet);
            }
            isBroadcast = false;
        }
    }
}
