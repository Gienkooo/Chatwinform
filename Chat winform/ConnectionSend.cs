using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_winform
{
    public static class ConnectionSend
    {
        private static Int32 bufferSize = 4096;
        public static void PerformHandshake(Connection _connection)
        {
            _connection.tcp.client.ReceiveBufferSize = bufferSize;
            _connection.tcp.client.SendBufferSize = bufferSize;  

            using (Packet _packet = new Packet((int)ClientPackets.handshake))
            {
                _packet.Write(_connection.username);
                _connection.tcp.Send(_packet);
            }
        }

        public static void SendMessage(Connection _connection, string _content)
        {
            using (Packet _packet = new Packet((int)ClientPackets.message))
            {
                _packet.Write(_connection.id);
                _packet.Write(_content);
                _connection.tcp.Send(_packet);
            }
        }
    }
}
