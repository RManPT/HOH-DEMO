﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HOH_Library
{
    public class MRNetwork
    {
        public string ip;
        public int port;
        public Socket server, client;
        private string msgRcvHOH;
        private string statusMsg;
        private bool statusMsgChanged = true;
        // private SocketAsyncEventArgs e;
        public delegate void InputEventHandler(object sender, LANCBEvenArgs e);
        public event InputEventHandler InputChanged;
        public ConcurrentQueue<string> msgs = new ConcurrentQueue<string>();
        public ConcurrentQueue<string> logMsgs = new ConcurrentQueue<string>();
        private TextBox txtLog;
        public delegate bool StateReached(string msg);
        private static object Key = new object();
        public bool ExecuteStatus { get; set; }
        public bool Waiting { get; set; }
        public bool isConnected = false;
        private readonly HOHEvent HOHEventObj = new HOHEvent();


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

        //Tries connecting client->server for 1s
        public bool Accept()
        {
            try
            {
                //sets up client socket
                client = server.Accept();
                client.ReceiveTimeout = 1000;
                client.SendTimeout = 1000;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Network Connection Exception, " + e.Message);
                return false;
            }
        }

        public void SetLogBox(TextBox txtBox)
        {
            this.txtLog = txtBox;
        }


        public bool Connect()
        {
            //try
            //{
                StateObject state = new StateObject();
                if (client == null) client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                if (client.Connected == true) { statusMsg = "Already connected"; return true; }

                IAsyncResult resultconn = client.BeginConnect(ip, port, null, null);
                bool success = resultconn.AsyncWaitHandle.WaitOne(2000, true);

                //client.Connect(ip, port);
                if (client.Connected)
                {
                    state.workSocket = client;
                    Receive(client);
                    while (!msgs.IsEmpty)
                        msgs.TryDequeue(out string result);
                    while (!logMsgs.IsEmpty)
                        logMsgs.TryDequeue(out string result);
                    InputChanged += InputDetectedEvent;

                    new Thread(new ThreadStart(IsTested)).Start();

               
                    
                    HOHEventObj.UpdateHOHLogMsg("Connection Successful");
                    isConnected = true;
                    return true;
                }
                else
                {
                    client.Close();
                    client = null;
                //throw new ApplicationException("Failed to connect server.");
                    isConnected = false;
                    return false;

                }
            //}
            //catch (Exception e)
            //{
            //    Disconnect();
            //    Debug.WriteLine("HOH Network Connection Exception, " + e.Message);
            //    return false;
            //}
        }


        public bool Disconnect()
        {
            try
            {
                //client.Shutdown(SocketShutdown.Both);
                //client.Disconnect(false);
                client.Close();
                client = null;
                InputChanged -= InputDetectedEvent;
                HOHEventObj.UpdateHOHLogMsg("Disconnection successfull");
              
                isConnected = false;
                return true;
            }
            catch (Exception e)
            { 
                Debug.WriteLine("HOH Network Connection Exception, " + e.Message);
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
                Debug.WriteLine("Async calback error: Hand disconnected!?");
                if (isConnected)
                {
                    HOHEventObj.UpdateHOHConn("failoff");
                   // isConnected = false;
                }
                // Debug.WriteLine(e.Message);


            }
        }

        public bool Send(string msg)
        {
            //lock (this)
            //{
                try
                {
                    byte[] tempmsg = Encoding.Default.GetBytes(msg);
                    client.Send(tempmsg);
                    return true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error sending: Hand is disconnected!?");
                if (isConnected)
                {
                    HOHEventObj.UpdateHOHConn("failoff");
                   // isConnected = false;
                }
            }
                return false;
           // }
        }

        private void InputDetectedEvent(object sender, LANCBEvenArgs e)
        {
            //Debug.Write("["+e.MsgString+"]");

            foreach (char ch in e.MsgString)
            {

                if (ch != '\n')
                    msgRcvHOH += ch;
                else
                {
                    // msgRcvHOH.Replace(Environment.NewLine, "x");
                    msgRcvHOH = msgRcvHOH.Trim();
                    if (msgRcvHOH.Length!=0)
                    {
                        lock (msgs)
                        {
                            HOHEventObj.UpdateHOHLogMsg(msgRcvHOH);
                            Debug.WriteLine("ENQUEUE " + msgRcvHOH);
                            msgs.Enqueue(msgRcvHOH);
                            Waiting = false;
                        }
                        msgRcvHOH = "";
                    }
                }
            }
        }


        /// <summary>
        /// Sends a command to HOH and waits for the feedback that must be equal to exitCondition
        /// </summary>
        /// <param name="command">Integer code to send to HOH</param>
        /// <param name="exitCondition">Message that represents end of TargetState</param>
        public void ExecuteAndWait(string command, string exitCondition, StateReached next)
        {
            ExecuteStatus = true;
            Waiting = true;
            Send(command);
            Debug.WriteLine(command + "|" + exitCondition);
            if (exitCondition!=String.Empty)
            while (ExecuteStatus)
            {
                ////Debug.WriteLine("waiting");
                lock (msgs)
                {
                    if (msgs != null)
                    if (!msgs.IsEmpty)
                    {
                        if (exitCondition == "*")
                        {
                            while (Waiting) { }
                            //Thread.Sleep(50);
                            break;
                        } else
                        if (msgs.TryDequeue(out string result))
                        {
                            if (result.IndexOf(exitCondition, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                ExecuteStatus = false;
                                HOHEventObj.UpdateHOHLogMsg("Operation concluded");
                                break;
                            }
                        }
                    }
                }
            }
            if (next != null)
            {
                bool b = next("acabei");
            }
           // ExecuteStatus = false;
            Debug.WriteLine("ELVIS LEFT THE BUILDING");
            //Console.Read();
        }

        public string GetStatusMsg()
        {
            return statusMsg;
        }

        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            if (txtLog.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                try
                {
                    txtLog.Invoke(d, new object[] { text + System.Environment.NewLine });
                }
                catch (Exception)
                {
                }
            }
            else
            {
                txtLog.Text += text + System.Environment.NewLine;
            }
        }

     
    

        public void IsTested()
        {
          
            bool Execute = true;
            while(Execute)
            {
                if (msgs!=null)
                if (!msgs.IsEmpty)
                {
                    if (msgs.TryDequeue(out string result))
                    {
                        //Debug.WriteLine(result + " - untested");
                        //Debug.WriteLine("EXECUTE DISTANCE: " + Utils.Levenshtein(result.ToLower(), "hand, untested"));

                        if (result.IndexOf("untested", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Thread exec = new Thread(() => ExecuteAndWait("01", "Exit hand brace testing", null));
                            exec.Start();
                            Debug.WriteLine("ISTESTED: not tested");
                            Execute = false;
                        }
                    }
                }
                else
                {
                    Execute = false;
                }
                //Thread.Sleep(500);
            }
            Debug.WriteLine("ISTESTED: is tested");
        }
    }   
}
