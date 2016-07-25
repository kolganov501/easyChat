using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace EasyChat
{
    public partial class Form1 : Form
    {

        static private Socket Client;
        private IPAddress ip = null;
        private int port = 0;
        private Thread th;
        public Form1()
        {
            InitializeComponent();
            richTextBox2.Enabled = false;
            button2.Enabled = false;
            try
            {
                var sr = new StreamReader(@"ClientInfo/dataInfo.txt");
                string buffer = sr.ReadToEnd();
                sr.Close();
                string[] connectInfo = buffer.Split(':');
                ip = IPAddress.Parse(connectInfo[0]);
                port = int.Parse(connectInfo[1]);
                label4.BackColor = Color.Green;
                label4.Text = "Hастройки : \n IP сервера: " + connectInfo[0] + "\n Порт сервера: " + connectInfo[1];
            }
            catch(Exception ex) 
            {
                label4.BackColor = Color.Red;
                label4.Text = "Настройки не найдены!";
                Form2 form = new Form2();
                form.Show();
            }
        }
        void SendMessage(string message)
        {
            if (Client != null)
            {
                if (message != "" && message != " ")
                {
                    byte[] buffer = new byte[1024];
                    buffer = Encoding.UTF8.GetBytes(message);
                    Client.Send(buffer);
                    richTextBox2.Clear();
                }
            }
        }
        void RecvMessage()
        {
            byte[] buffer = new byte[1024];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
            for (;;)
            {
                try
                {
                    Client.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer);
                    int count = message.IndexOf(";;;5");
                    if (count == -1)
                        continue;
                    string clearMessage = "";
                    for (int i = 0; i < count; i++)
                    {
                        clearMessage += message[i];
                    }
                    
                    this.Invoke((MethodInvoker)delegate ()
                    {
                        //richTextBox1.AppendText("\n");
                        richTextBox1.AppendText(clearMessage);
                        richTextBox1.ScrollToCaret();
                    });
                }
                catch(Exception ex)
                { 
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Client == null)
            {
                if (textBox1.Text != "" && textBox1.Text != " ")
                {
                    Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (ip != null)
                    {
                        try
                        {
                            Client.Connect(ip, port);
                            th = new Thread(delegate () { RecvMessage(); });
                            th.Start();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("✡Невеверные данные для подключения!✡");
                        }
                    }

                    button2.Enabled = true;
                    richTextBox2.Enabled = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendMessage("\n" + textBox1.Text +": "+ richTextBox2.Text+ ";;;5");
            richTextBox2.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (th != null)
                th.Abort();
            if (Client != null)
                Client.Close();
        }

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {                
                SendMessage("\n" + textBox1.Text + ": " + richTextBox2.Text + ";;;5");
                richTextBox2.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (th != null)
                th.Abort();
            if (Client != null)
                Client.Close();
        }
    }
}
