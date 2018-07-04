using System;
using System.Collections.Generic;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;


namespace HOH_Library
{


    

    public class AsyncServer
    {
        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static String UIReceivedMsg = "";
        public static Socket currentClient;
        public static List<Socket> MySocketList = new List<Socket>();
        public static bool setrun;
        public static TextBox txtLog;
        public static Socket listener;
        public static int LastCMDReceived;
        public static bool commandProcessed;
        

        public AsyncServer()
        {
            
        }

        public static void StartListening(int port)
        {
            setrun = true;
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];
            IPAddress ipAddress = GetLocalIPAddress();
            // Establish the local endpoint for the socket.  
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            //IPEndPoint localEndPoint = new IPEndPoint(IPAddress.IPv6Any, 10101);

            // Create a TCP/IP socket.  
            //Debug.WriteLine("Created server at " + ipAddress + ":" + port);
            SetText("Created server at " + ipAddress + ":" + port);
            listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                new Thread(new ThreadStart(refreshClients)).Start();

                while (setrun)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();
                    
                    // Start an asynchronous socket to listen for connections.  
                    //Debug.WriteLine("SERVER - Waiting for a connection...");
                    SetText("SERVER - Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();

                }
               
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                SetText(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();
            if (setrun)
            { 
            // Get the socket that handles the client request.  
            listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            //Debug.WriteLine(((IPEndPoint)handler.RemoteEndPoint).Address.ToString() + " connected");
            SetText(((IPEndPoint)handler.RemoteEndPoint).Address.ToString() + " connected");

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
            if (!MySocketList.Contains(handler)) { 
                MySocketList.Add(handler);
                
            }
            currentClient = handler;
            }
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            currentClient = handler;
            if (!MySocketList.Contains(handler))
                MySocketList.Add(handler);
            // Read data from the client socket.   
            
            try
            {
                int bytesRead = handler.EndReceive(ar);
           

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far. 
                    var lastMessage = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                    state.sb.Append(lastMessage);
                

                    //prints last received byte
                    Debug.Write(lastMessage.ToString());
                    //gets the processed mode in ascii
                    int command = (int)lastMessage.ToCharArray()[0];
                    SetText("Received from client : " + command.ToString());

                    //!!!!!!ATENCAO!!!!!!
                    //deve de chamar funcao no mainform que trate do comando para nao misturar o que o server faz com o que a APP faz
                    LastCMDReceived = command;
                    commandProcessed = false;


                    // Check for end-of-file tag. If it is not there, read   
                    // more data.  
                    content = state.sb.ToString();
                    //use # as a delimiter for end of transmission
                    if (content.IndexOf("#") > -1)
                    {
                        // All the data has been read from the   
                        // client. Display it on the console.  
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            content.Length, content);
                        SetText(String.Format("Read {0} bytes from socket. \n Data : {1}",
                            content.Length, content));
                        // Echo the data back to the client.  
                        Send(handler, content);

                    }
                    else
                    {
                        // Not all data received. Get more.  
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    
                    }
                }
            }
            catch (ObjectDisposedException e)
            {
                Debug.WriteLine(e);
            }
        }

        public static void Send(Socket handler, String data)
        { 
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                SetText(String.Format("Sent {0} bytes to client.", bytesSent));
               // handler.Shutdown(SocketShutdown.Both);
               // handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }


        delegate void SetTextCallback(string text);
        private static void SetText(string text)
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

      

        public static bool IsConnected(Socket socket)
        {
            try
            {
                
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException) { return false; }
            catch (ObjectDisposedException) { return false; }
        }

        public static void refreshClients()
        {
            while(setrun)
            //checks if clients are disconnected
            for (int i = MySocketList.Count - 1; i >= 0; i--)
            {
                    try
                    {
                        if (!IsConnected(MySocketList[i]))
                        {
                            if (MySocketList[i].Connected) MySocketList[i].Disconnect(false);
                            MySocketList[i].Shutdown(SocketShutdown.Both);
                            MySocketList[i].Close();
                            MySocketList[i].Dispose();
                            //MySocketList[i] = null;
                            MySocketList.RemoveAt(i);
                        }
                    }
                    catch (ObjectDisposedException) { }
            }

            //server 
            
            for (int i = MySocketList.Count - 1; i >= 0; i--)
            {
                try
                {
                    MySocketList[i].Shutdown(SocketShutdown.Both);
                    MySocketList[i].Close();
                    MySocketList[i].Dispose();
                    // MySocketList[i].Disconnect(false);
                   // MySocketList[i] = null;
                }
                catch (ObjectDisposedException) { }
            }
            MySocketList.Clear();
            SetText("Server stopped. All clients disconnected.");
        }

        public static void SetLogBox(TextBox txtBox)
        {
            txtLog = txtBox;
        }

        public static void StopServer()
        {
            setrun = false;
            try
            {
                //listener.Shutdown(SocketShutdown.Both);
                listener.Close();
                listener.Dispose();
                //listener = null;
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public static bool IsConnected()
        {
            return MySocketList.Count>=2;
        }
    }
}
