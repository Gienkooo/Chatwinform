using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_winform
{
    class ConnectionHandle
    {
        public static Form1 mainForm;

        public static void AppendToList(String text)
        {
            mainForm.BeginInvoke((Action)(() => { mainForm.logTextBox.AppendText(text + Environment.NewLine); }));
        }
        public static void HandshakeReceived(Packet _packet, Connection _connection)
        {
            _connection.id = _packet.ReadInt32();
            AppendToList($"Handshake received! | {_connection.id} | {_packet.ReadString()}");
            ConnectionSend.PerformHandshake(_connection);
            
        }

        public static void MessageReceived(Packet _packet, Connection _connection)
        {
            AppendToList(_packet.ReadString() + ":" + _packet.ReadString());
        }

        public static void HostMessageReceived(Packet _packet, Connection _connection)
        {
            AppendToList(_packet.ReadString());
        }

        public static void PingReceived(Packet _packet, Connection _connection)
        {
            //AppendToList("Server Pinged!");
        }
    }
}
