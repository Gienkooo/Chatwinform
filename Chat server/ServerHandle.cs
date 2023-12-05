using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_server
{
    public static class ServerHandle
    {
        public static void HandshakeReceived(Int32 _fromClient, Packet _packet)
        {
            Server.clients[_fromClient].username = _packet.ReadString();
            Console.WriteLine($"Handshake received from {Server.clients[_fromClient].username}");
            Packet packet = new Packet((int)ServerPackets.hostMessage);
            packet.Write(Server.clients[_fromClient].username + Server.joinMessage);
            packet.WriteLength();
            Server.EnqueuePacket(packet);
        }

        public static void MessageReceived(Int32 _fromClient, Packet _packet)
        {
            Console.WriteLine("Message received!");
            Int32 _id = _packet.ReadInt32();
            Console.WriteLine($"Fetched id:{_id} | From client:{_fromClient}");
            if(_id != _fromClient)
            {
                Server.clients[_fromClient].tcp.tcpClient.Close();
                return;
            }
            String _content = _packet.ReadString();
            Console.WriteLine($"{Server.clients[_fromClient].username} : {_content}");
            Packet _messagePacket = new Packet((int)ServerPackets.message);
            _messagePacket.Write(Server.clients[_fromClient].username);
            _messagePacket.Write(_content);
            _messagePacket.WriteLength();
            Server.EnqueuePacket(_messagePacket);
        }
    }
}
