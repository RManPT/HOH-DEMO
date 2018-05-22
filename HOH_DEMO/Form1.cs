using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;


namespace HOH_DEMO
{
    public partial class Form1 : Form
    {
        private MRNetwork NW;
        public delegate void InputEventHandler(object sender, MRNetwork.InputEventHandler e);
        private Thread ServerSL;
        public Form1()
        {
            InitializeComponent();
            NW = new MRNetwork("169.254.1.1", 2000);
            
         //   ServerSL = new Thread(() => AsyncServer.StartListening(10101),TextBox t);
         //   ServerSL.Start();

            //NW = new MRNetwork("0.0.0.0", 30000);
            //test connection with matlab
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
  
            if (buttonConnect.Text == "Connect")
            {
                if (NW.Connect())
                {
                    NW.InputChanged += InputDetectedEvent;
                    buttonConnect.Text = "Disconnect";
                    Debug.WriteLine("HOH Connected");
                    //test connection with matlab
                    //        NW.Send("r");

                    //inicia o servidor para o Simulink
                    //       ServerSL.Start();

                }
                else
                {
                    NW.InputChanged -= InputDetectedEvent;
                    MessageBox.Show("Connect fail");
           
                }
            }
            else
            {
                if (NW.Disconnect())
                    buttonConnect.Text = "Connect";
            }
        }

        //Lança para a consola devolvidas pelos eventos
        private void InputDetectedEvent(object sender, LANCBEvenArgs e)
        {
            Invoke(new EventHandler(delegate
            {
                //informação chega de forma assincrona
                this.textBoxLog.AppendText(e.MsgString);
            }));
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            //NW.Send(textBoxcmd.Text);
            AsyncServer.Send(AsyncServer.currentClient, "S");
         

        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            NW.Send("p");
        }

        private void buttonResume_Click(object sender, EventArgs e)
        {
            NW.Send("r");
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            NW.Send("x");
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            // NW.Send("832" + trackBarPosition.Value.ToString("000"));

            ///[finger]:All = 0, Thumb = 1, Index = 2, Middle = 3, Ring = 4, Little = 5
            //NW.Send("36");
            // NW.Send("831100");//close thumb
            NW.Send("832000");//open index
                              // NW.Send("833020");//open index
        }

        private void buttonSetAuto_Click(object sender, EventArgs e)
        {
            NW.Send("84" + trackBarPositionAuto.Value.ToString("000"));
        }

        private void buttontest_Click(object sender, EventArgs e)
        {
            NW.Send("01");
        }

        private void buttonfitting_Click(object sender, EventArgs e)
        {
            NW.Send("05");
        }

        private void buttonCPM_Click(object sender, EventArgs e)
        {
            NW.Send("07");
        }

        private void buttonfullyopen_Click(object sender, EventArgs e)
        {
            NW.Send("06");
        }

        private void buttonfullyclose_Click(object sender, EventArgs e)
        {
            NW.Send("36");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxLog.Text = "";
        }

        //Classe que lida com a conexão
        public class MRNetwork
        {
            public string ip;
            public int port;
            public Socket server, client;
            public int trial;
            private Queue response = new Queue();

            private SocketAsyncEventArgs e;
            public delegate void InputEventHandler(object sender, LANCBEvenArgs e);
            public event InputEventHandler InputChanged;

            //Conection class
            public MRNetwork(string ip, int port)
            {
                this.ip = ip;
                this.port = port;
                this.trial = 0;
                Debug.WriteLine(ip + ":" + port.ToString());
            }

            public MRNetwork(int port)
            {
                this.port = port;
            }

            public void Listen(int port)
            {
                //configures server socket
                server.Bind(new IPEndPoint(IPAddress.Any, port));
                //sets state to 'listening'
                server.Listen(port);
            }

            //Tries connecting client->server for 5s
            public bool Accept()
            {
                try
                {
                    //sets up client socket
                    client = server.Accept();
                    client.ReceiveTimeout = 5000;
                    client.SendTimeout = 5000;
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Network Connection Exception, " + e.Message);
                    return false;
                }
            }


            public bool Connect()
            {
                try
                {
                    StateObject state = new StateObject();
                    this.response = new Queue();
                    if (client == null) client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (client.Connected == true) return true;
                    client.Connect(ip, port);
                    state.workSocket = client;
                    Receive(client);
                    return true;
                }
                catch (Exception e)
                {
                    Disconnect();
                    Debug.WriteLine("HOH Network Connection Exception, " + e.Message);
                    return false;
                }
            }


            public bool Disconnect()
            {
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Disconnect(false);
                    client.Close();
                    client = null;
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            private void Receive(Socket client)
            {
                try
                {
                    // Create the state object.
                    StateObject state = new StateObject();
                    state.workSocket = client;

                    // Begin receiving the data from the remote device.
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
            }

            private void ReceiveCallback(IAsyncResult ar)
            {
                try
                {
                    // Retrieve the state object and the client socket 
                    // from the asynchronous state object.
                    StateObject state = (StateObject)ar.AsyncState;
                    Socket client = state.workSocket;
                    // Read data from the remote device.
                    int bytesRead = client.EndReceive(ar);
                    if (bytesRead > 0)
                    {
                        // There might be more data, so store the data received so far.
                        string temp = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                        //state.sb.Append(temp);
                        //Debug.WriteLine(temp);
                        if (this.InputChanged != null)
                            this.InputChanged(this, new LANCBEvenArgs(temp));
                        //  Get the rest of the data.
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            public bool Send(string msg)
            {
                lock (this)
                {
                    try
                    {
                        byte[] tempmsg = Encoding.Default.GetBytes(msg);
                        client.Send(tempmsg);
                        return true;
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(50);
                    }
                    return false;
                }
            }
        }

        //sets up comm settings
        public class StateObject
        {
            // Client socket.
            public Socket workSocket = null;
            // Size of receive buffer.
            public const int BufferSize = 256;
            // Receive buffer.
            public byte[] buffer = new byte[BufferSize];
            // Received data string.
            public StringBuilder sb = new StringBuilder();
        }

        //manages console
        public class LANCBEvenArgs : EventArgs
        {

            private string msg;
            public string MsgString
            {
                get { return this.msg; }
            }

            public LANCBEvenArgs(string msg)
            {
                this.msg = msg;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
