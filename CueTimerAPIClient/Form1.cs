using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CueTimerAPIClient
{
    public partial class Form1 : Form
    {
        NetworkStream nwStream;
        bool listening;
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient(textBoxIP.Text, int.Parse(textBoxPort.Text));
            nwStream = client.GetStream();
            listening = true;
            Task.Run(() =>
            {
                while (listening)
                {
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    Invoke((MethodInvoker)delegate () { 
                        textBoxFeedback.Text = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead); 
                    });
                }
                client.Close();
            });
            buttonDisconnect.Enabled = true;
            buttonFireNext.Enabled = true;
            buttonFireTimerWithID.Enabled = true;
            buttonSend.Enabled = true;
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendCommand(textBoxCommand.Text);
        }

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            listening = false;
        }

        private void buttonFireNext_Click(object sender, EventArgs e)
        {
            SendCommand("FireNext");
        }

        private void buttonFireTimerWithID_Click(object sender, EventArgs e)
        {
            // This will send 1 as a parameter
            // A timer with this id should be exist on the list before sending the command
            SendCommand("FireTimerWithID#1");
        }

        private void SendCommand(string command)
        {
            Console.WriteLine("Sending : " + command);
            byte[] bytesToSend = Encoding.ASCII.GetBytes(string.Format("{0}$",command));
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        
    }
}
