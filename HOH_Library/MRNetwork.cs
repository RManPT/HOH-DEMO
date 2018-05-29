using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace HOH_DEMO_Library
{
    public class MRNetwork
    {
        public string ip;
        public int port;
        public Socket server, client;
        private string msgRcvHOH;
        private string statusMsg;
        // private SocketAsyncEventArgs e;
        public delegate void InputEventHandler(object sender, LANCBEvenArgs e);
        public event InputEventHandler InputChanged;
        public ConcurrentQueue<string> msgs = new ConcurrentQueue<string>();


        //Conection class
        public MRNetwork(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            Debug.WriteLine(ip + ":" + port.ToString());
            statusMsg = "Creating HOH Socket at " + ip + ":" + port.ToString();
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
            statusMsg = "Socket listening";
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
                if (client == null) client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (client.Connected == true) { statusMsg = "Already connected"; return true; }
                client.Connect(ip, port);
                state.workSocket = client;
                Receive(client);
                InputChanged += InputDetectedEvent;
                statusMsg = "Connection Successful";
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
                InputChanged -= InputDetectedEvent;
                statusMsg = "Disconnection successfull";
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

        private void InputDetectedEvent(object sender, LANCBEvenArgs e)
        {
            Debug.Write(e.MsgString);
            

            if (!e.MsgString.Contains("\n"))
                msgRcvHOH += e.MsgString;
            else
            {
                msgs.Enqueue(msgRcvHOH);
                msgRcvHOH = "";
            }
        }


        /// <summary>
        /// Sends a command to HOH and waits for the feedback that must be equal to exitCondition
        /// </summary>
        /// <param name="command">Integer code to send to HOH</param>
        /// <param name="exitCondition">Message that represents end of movement</param>
        public void ExecuteAndWait(string command, string exitCondition)
        {
            bool status = true;
            Send(command);
            while (true)
            {
                string result;
                if (!msgs.TryPeek(out result))
                {
                    Console.WriteLine("TryPeek failed when it should have succeeded");
                    statusMsg = "TryPeek failed when it should have succeeded";
                }
                else if (msgs.TryDequeue(out result).ToString() == exitCondition)
                {
                    status = false;
                    Debug.WriteLine("Operation concluded");
                    statusMsg = result;
                }

                Thread.Sleep(50);
            }
        }

        public string GetStatusMsg()
        {
            return statusMsg;
        }
    }
}
