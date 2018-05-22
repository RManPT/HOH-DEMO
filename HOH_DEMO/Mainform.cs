﻿using System;
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
    public partial class Mainform : Form
    {
        private MRNetwork NW;
        public delegate void InputEventHandler(object sender, MRNetwork.InputEventHandler e);
        private Thread ServerSL;
        private int CPMTimeCounter = 0;
        private int CPMCounter = 0;
        private int timeSync = 0;
        private bool runCPMWhole = false;
        private bool runCPMPinch = false;
        private bool runCTMOpen = false;
        private bool runCTMClose = false;
        private bool connectedHOH = false;
        private bool calibratedHOH = false;
        private string msgRcvHOH;
        private int msgToSendSF = 0;
        public static int LastCMDReceived;
        private int previousCMDReceived;
        public static bool commandProcessed = true;
        public Mainform()
        {
            InitializeComponent();
            NW = new MRNetwork("169.254.1.1", 2000);
            tabControl.SelectTab(1);
            actionTimer.Start();

            //  ServerSL = new Thread(() => AsyncServer.StartListening(10101));
            //  ServerSL.Start();

            //NW = new MRNetwork("0.0.0.0", 30000);
            //test connection with matlab
        }

        //Classe que lida com a 
        #region HOH classes
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

        #endregion


        /***********************************************************************************************************/
        /***********************************************************************************************************/
        /***********************************************************************************************************/

        //Custom functions
        #region Custom functions
        private void sendAll(string m)
        {
            foreach (Socket sock in AsyncServer.MySocketList)
                AsyncServer.Send(sock, m);
        }

        //Lança para a consola devolvidas pelos eventos
        private void InputDetectedEvent(object sender, LANCBEvenArgs e)
        {
            Invoke(new EventHandler(delegate
            {
                //informação chega de forma assincrona
                this.textBoxLog.AppendText(e.MsgString);

                if (!e.MsgString.Contains("\n")) msgRcvHOH += e.MsgString;
                else
                {
                    msgRcvHOH += e.MsgString;
                    //detecta qual o movimento CPM a partir da msg
                    if (runCPMWhole)
                    {
                        if (msgRcvHOH.Contains("003%") && msgRcvHOH.Contains("Closing"))
                        {
                            txtCPMLog.AppendText("\r\nHOH -> CLOSING HAND");
                            sendAll(((char)12).ToString());
                        }
                        if (msgRcvHOH.Contains("094%") && msgRcvHOH.Contains("Opening"))
                        {
                            txtCPMLog.AppendText("\r\nHOH -> OPENING HAND");
                            sendAll(((char)11).ToString());
                            CPMCounter++;
                        }
                        if (msgRcvHOH.Contains("Exit"))
                        {
                            txtCPMLog.AppendText("\r\nHOH -> STOPPED");
                            sendAll(((char)10).ToString());
                        }
                        //   }
                        //txtCPMLog.AppendText("->" + msgRcvHOH);
                        msgRcvHOH = "";
                    }


                    if (runCTMClose)
                    {
                        if (msgRcvHOH.Contains("003%") && msgRcvHOH.Contains("Closing"))
                        {
                            txtCTMLog.AppendText("\r\nHOH -> CLOSING HAND");
                            //sendAll(((char)12).ToString());

                        }
                        if (msgRcvHOH.Contains("Exit hand closing"))
                        {
                            txtCTMLog.AppendText("\r\nHOH -> OPENING HAND");
                            //sendAll(((char)11).ToString());
                            //FORCAR ABERTURA DE MAO AUTOMATICO
                            CPMCounter++;
                            buttonfullyopen_Click(sender, e);
                            previousCMDReceived = 0;
                        }
                        //txtCTMLog.AppendText("->" + msgRcvHOH);
                        msgRcvHOH = "";
                    }
                }     
            }));
        }

        private string timeToStr(int counter)
        {
            int min, seg;
            min = counter / 60;
            if (min != 0) seg = counter % (min * 60); else seg = counter;
            string minstr, segstr;
            if (min < 10) minstr = "0" + min.ToString(); else minstr = min.ToString();
            if (seg < 10) segstr = "0" + seg.ToString(); else segstr = seg.ToString();

            return minstr + ":" + segstr;
        }

        private string timeToStr(int min, int seg)
        {
            string minstr, segstr;
            if (min < 10) minstr = "0" + min.ToString(); else minstr = min.ToString();
            if (seg < 10) segstr = "0" + seg.ToString(); else segstr = seg.ToString();

            return minstr + ":" + segstr;
        }

        private int timeToCounter(int min, int seg)
        {
            return min * 60 + seg;
        }

        private void ProcessCommand() {
            //takecare of lastReceivedCommand;
            commandProcessed = true;
        }
        #endregion


        /***********************************************************************************************************/
        /***********************************************************************************************************/
        /***********************************************************************************************************/

        //Event functions
        #region HOH event functions

        private void buttonConnect_Click(object sender, EventArgs e)
        {

            if (!connectedHOH)
            {
                if (NW.Connect())
                {
                    NW.InputChanged += InputDetectedEvent;
                    buttonConnect.Text = "Disconnect";
                    Debug.WriteLine("HOH Connected");
                    connectedHOH = true;
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
                {
                    buttonConnect.Text = "Connect";
                    connectedHOH = false;
                }
            }
        }


        private void buttonSend_Click(object sender, EventArgs e)
        {
            NW.Send(textBoxcmd.Text);
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
        #endregion

        #region APP event functions

        #region common controls
        private void Mainform_Load(object sender, EventArgs e)
        {
            btnServerStart_Click(sender, e);
            CPMTimeCounter = timeToCounter(Convert.ToInt32(numericCPMUpDownMinutes.Value), Convert.ToInt32(numericCPMUpDownSeconds.Value));
            lblCPMTimer.Text = timeToStr(CPMTimeCounter);
            lblCPMCounter.Text = numericCPMUpDownCounter.Value.ToString();
            lblCTMTimer.Text = timeToStr(CPMTimeCounter);
            lblCTMCounter.Text = numericCTMUpDownCounter.Value.ToString();
            trackCTMThreshold.Value = trackCTMBaseline.Value + 5;
        }

        private void Mainform_FormClosed(object sender, FormClosedEventArgs e)
        {
            buttonfullyopen_Click(sender, e);
        }

        private void timer_sync_Tick(object sender, EventArgs e)
        {
            
            if (timeSync % 10 == 0 || timeSync == 0)
            {
                txtCPMLog.AppendText("\r\nTIMER_SYNC: " + timeSync + "s - " + msgToSendSF);
                txtCTMLog.AppendText("\r\nTIMER_SYNC: " + timeSync + "s - " + msgToSendSF);
                //  txtSTMLog.AppendText("\r\nTIMER_SYNC: " + timeSync + "s");

                //Sends Sync Message
                sendAll(((char)msgToSendSF).ToString());
            }
            timeSync++;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (runCPMWhole)
            {
                if (rbCPMTimer.Checked)
                {
                    CPMTimeCounter--;
                    //incrementar contador de movimentos concluidos
                    if (CPMTimeCounter == 0)
                    {
                        btnWhole_Click(sender, e); //enviar para tarefa de fim de exercício
                        txtCPMLog.AppendText("\r\nWhole Hand treatment concluded! Well done!");
                    }
                }
                if (rbCPMCounter.Checked)
                {
                    CPMTimeCounter++;
                    //decrementar contador de movimentos concluidos
                }
            }
            lblCPMTimer.Text = timeToStr(CPMTimeCounter);
            lblCPMCounter.Text = CPMCounter.ToString();

            if (runCTMClose)
            {
                if (rbCTMTimer.Checked)
                {
                    CPMTimeCounter--;
                    //incrementar contador de movimentos concluidos
                    if (CPMTimeCounter == 0)
                    {
                        btnCTMCloseHand_Click(sender, e); //enviar para tarefa de fim de exercício
                        txtCTMLog.AppendText("\r\nCTM - Close Hand treatment concluded! Well done!");
                    }
                }
                if (rbCTMCounter.Checked)
                {
                    CPMTimeCounter++;
                    //decrementar contador de movimentos concluidos
                }
            }
            lblCTMTimer.Text = timeToStr(CPMTimeCounter);
            lblCTMCounter.Text = CPMCounter.ToString();
        }

        /// <summary>
        /// Reset sync timer
        /// </summary>
        private void timerSyncReset() {
            timer_sync.Stop();
            //timer_sync.Enabled = false;
            //timer_sync.Enabled = true;
            timeSync=0;
        }

        private void actionTimer_Tick(object sender, EventArgs e)
        {
            if (runCTMClose)
                if (LastCMDReceived == 22 && LastCMDReceived != previousCMDReceived && commandProcessed == false)
                { //se sinal detectado indica o movimento desejado actua em conformidade
                    buttonfullyclose_Click(sender, e); //envia comando de CTM Close 
                    txtCTMLog.AppendText("\r\nWell done, closing hand!");
                    //commandProcessed = true;    //certifica que não há comandos processados multiplas 
                    previousCMDReceived = LastCMDReceived;
                    commandProcessed = true;
                }
        }
        #endregion


        #region server tab

        private void btnServerSend_Click(object sender, EventArgs e)
        {
            AsyncServer.Send(AsyncServer.currentClient, txtServerSend.Text);
        }

        private void btnServerStart_Click(object sender, EventArgs e)
        {
            ServerSL = new Thread(() => AsyncServer.StartListening(Int32.Parse(txtServerPort.Text), txtServerLog));
            ServerSL.Start();
            btnServerStop.Enabled = true;
            btnServerStart.Enabled = false;
        }

        private void btnServerStop_Click(object sender, EventArgs e)
        {
            AsyncServer.setrun = false;
            ServerSL = null;
            btnServerStop.Enabled = false;
            btnServerStart.Enabled = true;
            AsyncServer.listener.Shutdown(SocketShutdown.Both);
            // AsyncServer.listener.Close();

        }

        private void btnServerCtrlClear_Click(object sender, EventArgs e)
        {
            txtServerSend.Text = "";
        }

        private void btnServerLogClear_Click(object sender, EventArgs e)
        {
            txtServerLog.Text = "";
        }

        private void btnServerSetPasssive_Click(object sender, EventArgs e)
        {
            if (btnServerSetPassive.Text == "Set Passive Mode")
            {
                sendAll(((char)11).ToString());
                //AsyncServer.Send(AsyncServer.currentClient, "1");
                btnServerSetPassive.Text = "Stop Passive Mode";
                txtServerLog.AppendText("PASSIVE MODE ON\r\n");
                btnServerSetContinuous.Enabled = false;
            }
            else
            {
                sendAll(((char)10).ToString());
                //AsyncServer.Send(AsyncServer.currentClient, "0");
                btnServerSetPassive.Text = "Set Passive Mode";
                txtServerLog.AppendText("PASSIVE MODE OFF\r\n");
                btnServerSetContinuous.Enabled = true;
            }
        }

        private void btnServerSetContinuous_Click(object sender, EventArgs e)
        {
            if (btnServerSetContinuous.Text == "Set Continuous Mode")
            {
                sendAll(((char)21).ToString());
                //AsyncServer.Send(AsyncServer.currentClient, "2");
                btnServerSetContinuous.Text = "Stop Continuous Mode";
                txtServerLog.AppendText("CONTINUOS MODE ON\r\n");
                btnServerSetPassive.Enabled = false;
            }
            else
            {
                sendAll(((char)20).ToString());
                //AsyncServer.Send(AsyncServer.currentClient, "0");
                btnServerSetContinuous.Text = "Set Continuous Mode";
                txtServerLog.AppendText("CONTINUOS MODE OFF\r\n");
                btnServerSetPassive.Enabled = true;
            }
        }

        private void txtServerLog_TextChanged(object sender, EventArgs e)
        {
            txtServerLog.SelectionStart = txtServerLog.TextLength;
            txtServerLog.ScrollToCaret();
            //updates clients connected
            lblClientsConnected.Text = "Clients connected: " + AsyncServer.MySocketList.Count.ToString();

            //MAYBE NOT NEEDED!!
            //checks if latest info is a command sent from sfunction and processes it
            if (!commandProcessed) ProcessCommand();
        }

        #endregion


        #region CPM tab

        private void btnWhole_Click(object sender, EventArgs e)
        {
            if (!runCPMWhole)
            {
                btnWhole.Text = "Stop";
                btnPinch.Enabled = false;
                gbTimer.Enabled = false;
                gbCounter.Enabled = false;
                if (rbCPMTimer.Checked) CPMCounter = 0;
                if (rbCPMCounter.Checked) CPMTimeCounter = 0;
                timer1.Start();
                txtCPMLog.AppendText("\r\nWhole Hand treatment started");

                //deve reenviar comando sempre que o movimento terminar até que haja interrupção
                //deve terminar de mao aberta
                //deve enviar indicação de alteração de movimento para sfunction assim como de cada vez que há interrupcao ou quando o exercicio termina
                timer_sync.Start();
                buttonCPM_Click(sender, e); //envia comando de CPM 

            }
            else
            {
                btnWhole.Text = "Whole hand";
                btnPinch.Enabled = true;
                gbTimer.Enabled = true;
                gbCounter.Enabled = true;
                timer1.Stop();
                if (CPMTimeCounter != 0) txtCPMLog.AppendText("\r\nWhole Hand treatment paused");
                timerSyncReset();
                buttonPause_Click(sender, e);
                buttonExit_Click(sender, e);
            }
            runCPMWhole = !runCPMWhole;
        }

        private void btnCPMLogClear_Click(object sender, EventArgs e)
        {
            txtCPMLog.Text = "";
        }

        private void numericCPMUpDownMinutes_ValueChanged(object sender, EventArgs e)
        {
            CPMTimeCounter = timeToCounter(Convert.ToInt32(numericCPMUpDownMinutes.Value), Convert.ToInt32(numericCPMUpDownSeconds.Value));
            lblCTMTimer.Text = timeToStr(CPMTimeCounter);
        }

        private void numericCPMUpDownSeconds_ValueChanged(object sender, EventArgs e)
        {
            CPMTimeCounter = timeToCounter(Convert.ToInt32(numericCPMUpDownMinutes.Value), Convert.ToInt32(numericCPMUpDownSeconds.Value));
            lblCPMTimer.Text = timeToStr(CPMTimeCounter);
        }

        private void numericCPMUpDownCounter_ValueChanged(object sender, EventArgs e)
        {
            lblCPMCounter.Text = numericCPMUpDownCounter.Value.ToString();
        }

        private void rbCPMTimer_Click(object sender, EventArgs e)
        {
            if (rbCPMTimer.Checked)
            {
                if (!btnPinch.Enabled) btnPinch.Enabled = true;
                if (!btnWhole.Enabled) btnWhole.Enabled = true;
                rbCPMCounter.Checked = false;
                gbTimer.BackColor = Color.FromArgb(10, 0, 255, 0);
                gbCounter.BackColor = Color.Transparent;
                lblCPMCounter.Text = "0";
                CPMTimeCounter = timeToCounter(Convert.ToInt32(numericCPMUpDownMinutes.Value), Convert.ToInt32(numericCPMUpDownSeconds.Value));
                lblCPMTimer.Text = timeToStr(CPMTimeCounter);
            }
        }

        private void rbCPMCounter_Click(object sender, EventArgs e)
        {
            if (rbCPMCounter.Checked)
            {
                if (!btnPinch.Enabled) btnPinch.Enabled = true;
                if (!btnWhole.Enabled) btnWhole.Enabled = true;
                rbCPMTimer.Checked = false;
                gbCounter.BackColor = Color.FromArgb(10, 0, 255, 0);
                gbTimer.BackColor = Color.Transparent;
                lblCPMTimer.Text = "00:00";
                CPMCounter = Convert.ToInt32(numericCPMUpDownCounter.Value);
                lblCPMCounter.Text = CPMCounter.ToString();
            }

        }
        #endregion


        #region CTM tab
        private void numericCTMUpDownMinutes_ValueChanged(object sender, EventArgs e)
        {
            CPMTimeCounter = timeToCounter(Convert.ToInt32(numericCTMUpDownMinutes.Value), Convert.ToInt32(numericCTMUpDownSeconds.Value));
            lblCTMTimer.Text = timeToStr(CPMTimeCounter);
        }

        private void numericCTMUpDownSeconds_ValueChanged(object sender, EventArgs e)
        {
            CPMTimeCounter = timeToCounter(Convert.ToInt32(numericCTMUpDownMinutes.Value), Convert.ToInt32(numericCTMUpDownSeconds.Value));
            lblCTMTimer.Text = timeToStr(CPMTimeCounter);
        }

        private void numericCTMUpDownCounter_ValueChanged(object sender, EventArgs e)
        {
            lblCTMCounter.Text = numericCTMUpDownCounter.Value.ToString();
        }

        private void btnCTMLogClear_Click(object sender, EventArgs e)
        {
            txtCTMLog.Text = "";
        }

        private void rbCTMTimer_Click(object sender, EventArgs e)
        {
            if (rbCTMTimer.Checked)
            {
                if (!btnCTMStart.Enabled) btnCTMStart.Enabled = true;
//                if (!btnCTMOpenHand.Enabled) btnCTMOpenHand.Enabled = false;
                rbCTMCounter.Checked = false;
                gbCTMTimer.BackColor = Color.FromArgb(10, 0, 255, 0);
                gbCTMCounter.BackColor = Color.Transparent;
                lblCTMCounter.Text = "0";
                CPMTimeCounter = timeToCounter(Convert.ToInt32(numericCTMUpDownMinutes.Value), Convert.ToInt32(numericCTMUpDownSeconds.Value));
                lblCTMTimer.Text = timeToStr(CPMTimeCounter);
            }
        }

        private void rbCTMCounter_CheckedChanged(object sender, EventArgs e)
        {
            if (rbCTMCounter.Checked)
            {
                if (!btnCTMStart.Enabled) btnCTMStart.Enabled = true;
  //              if (!btnCTMOpenHand.Enabled) btnCTMOpenHand.Enabled = true;
                rbCTMTimer.Checked = false;
                gbCTMCounter.BackColor = Color.FromArgb(10, 0, 255, 0);
                gbCTMTimer.BackColor = Color.Transparent;
                lblCTMTimer.Text = "00:00";
                CPMCounter = Convert.ToInt32(numericCTMUpDownCounter.Value);
                lblCTMCounter.Text = CPMCounter.ToString();
            }
        }

        private void btnCTMCloseHand_Click(object sender, EventArgs e)
        {
            if (!runCTMClose)
            {
                btnCTMStart.Text = "Stop";
  //              btnCTMOpenHand.Enabled = false;
                gbCTMTimer.Enabled = false;
                gbCTMCounter.Enabled = false;
                if (rbCTMTimer.Checked) CPMCounter = 0;
                if (rbCTMCounter.Checked) CPMTimeCounter = 0;
                timer1.Start();
                txtCTMLog.AppendText("\r\nContinuous Triggered Mode - Open Hand treatment started");
                //deve reenviar comando sempre que o movimento terminar até que haja interrupção
                //deve terminar de mao aberta
                //deve enviar indicação de alteração de movimento para sfunction assim como de cada vez que há interrupcao ou quando o exercicio termina
                timer_sync.Start();
                //desactivar outros modos
                msgToSendSF = 22;
            }
            else
            {
                btnCTMStart.Text = "Close hand";
 //               btnCTMOpenHand.Enabled = true;
                gbCTMTimer.Enabled = true;
                gbCTMCounter.Enabled = true;
                timer1.Stop();
                if (CPMTimeCounter != 0) txtCPMLog.AppendText("\r\nCTM Close Hand treatment paused");
                timerSyncReset();
                buttonPause_Click(sender, e);
                buttonExit_Click(sender, e);
            }
            runCTMClose = !runCTMClose;
        }



        #endregion

        #endregion
    }
}
