using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_server
{
    public static class ServerSend
    {
        private static Int32 bufferSize = 4096;
        private static String welcomeMessage = "Welcome user!";
        

        public static void PerformHandshake(Client _client)
        {
            _client.tcp.tcpClient.ReceiveBufferSize = bufferSize;
            _client.tcp.tcpClient.SendBufferSize = bufferSize;

            using (Packet _packet = new Packet((int)ServerPackets.handshake))
            {
                _packet.Write(_client.id);
                _packet.Write(welcomeMessage);
                _packet.WriteLength();
                _client.tcp.Send(_packet);
            }
            Console.WriteLine("Handshake sent");
        }

        public static void RefuseConnection(Client _client)
        {
            using (Packet _packet = new Packet((int)ServerPackets.refuseConnection))
            {
                _client.tcp.Send(_packet);
            }
        }

        public static void Ping(Client _client)
        {
            using (Packet _packet = new Packet((int)ServerPackets.ping))
            {
                _client.tcp.Send(_packet);
            }
        }

        public static void PingAll()
        {
            foreach (var c in Server.clients)
            {
                Ping(c.Value);
            }
        }

        public static void BroadcastPacket(Packet _packet, Int32 _id = -1)
        {
            foreach (var c in Server.clients)
            {
                if(c.Key != _id)
                {
                    c.Value.tcp.Send(_packet);
                }
            }
        }
    }
}
