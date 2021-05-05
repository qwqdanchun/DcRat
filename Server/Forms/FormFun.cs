using Server.Connection;
using Server.MessagePack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Forms
{
    public partial class FormFun : Form
    {
        public FormFun()
        {
            InitializeComponent();
        }
        public Form1 F { get; set; }
        internal Clients Client { get; set; }
        internal Clients ParentClient { get; set; }

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
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Taskbar+";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Taskbar-";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Desktop+";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Desktop-";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Clock+";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "Clock-";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "swapMouseButtons";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "restoreMouseButtons";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "openCD+";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "openCD-";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button18_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "blankscreen+";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button17_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "blankscreen-";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "blockInput";
            msgpack.ForcePathObject("Time").AsString = numericUpDown1.Value.ToString();
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button15_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "holdMouse";
            msgpack.ForcePathObject("Time").AsString = numericUpDown2.Value.ToString();
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button12_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "monitorOff";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button14_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "hangSystem";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
        }



        private void FormFun_FormClosed(object sender, FormClosedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Client?.Disconnected();
            });
        }

        private void button19_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "webcamlight+";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button16_Click(object sender, EventArgs e)
        {
            MsgPack msgpack = new MsgPack();
            msgpack.ForcePathObject("Pac_ket").AsString = "webcamlight-";
            ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog O = new OpenFileDialog())
            {
                O.Filter = "(*.wav)|*.wav";
                if (O.ShowDialog() == DialogResult.OK)
                {
                    byte[] wavfile = File.ReadAllBytes(O.FileName);
                    MsgPack msgpack = new MsgPack();
                    msgpack.ForcePathObject("Pac_ket").AsString = "playAudio";
                    msgpack.ForcePathObject("wavfile").SetAsBytes(wavfile);
                    ThreadPool.QueueUserWorkItem(Client.Send, msgpack.Encode2Bytes());
                }
                else
                {
                    MessageBox.Show("Please choose a wav file.");
                }
            }
        }
    }
}
