using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Chat_winform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConnectionHandle.mainForm = this;
            popup = new Popup(this);
            popup.Owner = this;
            popup.Show(this);
            ExecuteOnMainThreadWorker.WorkerReportsProgress = false;
            ExecuteOnMainThreadWorker.WorkerSupportsCancellation = false;
            ExecuteOnMainThreadWorker.RunWorkerAsync();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public void submitData(System.String _username, System.String _IP)
        {
            username = _username;
            IP = _IP;
            string[] split = _IP.Split(':');
            TcpClient _client = new TcpClient(split[0], Convert.ToInt32(split[1]));
            connection = new Connection(_client, username);
            updateWindowTitle();
        }

        private void updateWindowTitle()
        {
            this.Text = this.username + " | Connected to: " + this.IP + System.Environment.NewLine;
        }

        private void inputBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if(inputBox.Text.Length > 0)
            {
                using (Packet _packet = new Packet((int)ClientPackets.message))
                {
                    _packet.Write(connection.id);
                    _packet.Write(inputBox.Text.Trim());
                    connection.tcp.Send(_packet);
                }
                inputBox.Text = String.Empty;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == Convert.ToChar(Keys.Enter) && ModifierKeys != Keys.Alt)
            {
                e.Handled = true;
                sendButton_Click(sender, new EventArgs());
            }
        }

        private void ExecuteOnMainThreadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            while (true)
            {
                if(DateTime.Now >= dateTime)
                {
                    dateTime = dateTime.AddMilliseconds(30);
                    ThreadManager.UpdateMain();
                }
                else
                {
                    Thread.Sleep(dateTime - DateTime.Now);
                }
            }
        }
    }
}
