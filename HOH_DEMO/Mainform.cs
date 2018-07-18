using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using HOH_Library;
using HOH_ProtocolEditor;
using HOH_ProtocolGUI;
using System.IO;
using System.ComponentModel;

namespace HOH_DEMO
{
    public partial class Mainform : Form
    {
        private MRNetwork NW;
        public delegate void InputEventHandler(object sender, MRNetwork.InputEventHandler e);
        private Thread ServerSL, LogUpdater;
        private int CPMTimeCounter = 0;
        private int CPMCounter = 0;
        private int timeSync = 0;
        private bool runCPMWhole = false;
        private bool runCPMPinch = false;
        private bool runCTMOpen = false;
        private bool runCTM = false;
        private bool runCTMClose = false;
        private bool connectedHOH = false;
        private string msgRcvHOH;
        private int msgToSendSF = 0;
        public static int LastCMDReceived;
        private int previousCMDReceived;
        public static bool commandProcessed = true;
        private static Clinic clinic = new Clinic();
        private delegate void EventSubscribed(HOHEvent e);
        // private delegate void EventSubscribed();
        private HOHEvent HOHEventObj = new HOHEvent();
        private TabControl.TabPageCollection tabs;
        private string defaultFileName;
        private string deviceIP;
        private string devicePORT;
        private string serverPORT;
       


        BindingSource protocolsBinding = new BindingSource();
        BindingSource protocolDetailsBinding = new BindingSource();
        BindingSource protocolExerciseBindingSource;

        BindingList<Protocol> blProtocols = new BindingList<Protocol>(clinic.Protocols);

        public Mainform()
        {
            //Seed.SetupData(clinic);
          
            InitializeComponent();
            LoadProperties();
            LoadProtocols();
            Text += " - " + clinic.Name;



            NW = new MRNetwork(deviceIP, Int32.Parse(devicePORT)); //("169.254.1.1", 2000
            NW.SetLogBox(textBoxLog);
            tabControl.SelectTab(0);
            actionTimer.Start();
            comboTreatment.SelectedIndex = 0;
            //ServerSL = new Thread(() => AsyncServer.StartListening(10101));
            //ServerSL.Start();

            HOHEvent.LogUpdated += OnHOHEventUpdate;
            HOHEvent.UsrMsgUpdated += OnHOHEventUpdate;
            HOHEvent.ClinicUpdated += OnClinicEventUpdate;

            GetProtocols();

        }

        private void LoadProperties()
        {
            deviceIP = Properties.Settings.Default.deviceIP;
            devicePORT = Properties.Settings.Default.devicePORT;
            serverPORT = Properties.Settings.Default.serverPORT;
            defaultFileName = Properties.Settings.Default.defaultJsonFile;


            if (deviceIP == String.Empty) deviceIP = txtOptionsDeviceIP.Text;
            else txtOptionsDeviceIP.Text = deviceIP;

            if (devicePORT == String.Empty) devicePORT = txtOptionsDevicePort.Text;
            else txtOptionsDevicePort.Text = devicePORT;

            if (serverPORT == String.Empty) serverPORT = txtServerPort.Text;
            else txtServerPort.Text = serverPORT;

            if (defaultFileName == String.Empty) defaultFileName = txtOptionsProtocolSeedFile.Text;
            if (defaultFileName == txtOptionsProtocolSeedFile.Text)
                statusBar1.Text = "Using : " + Path.GetFileName(defaultFileName) + " (read only)";
            else
                statusBar1.Text = "Using : " + Path.GetFileName(defaultFileName);
        }

        private void OnHOHEventUpdate(object sender, HOHEvent e)
        {
            Delegate d = new EventSubscribed(updateGUI);
            this.Invoke(d, e);
        }

        private void OnClinicEventUpdate(object sender, HOHEvent e)
        {
            //clinic = null;
            // clinic = new Clinic(e.Clinic);
            clinic = e.Clinic;
            UpdateProtocols();
        }

        private void updateGUI(HOHEvent e)
        {
            if (e.LogMsg != null) txtProtocolsLog.AppendText(e.LogMsg + Environment.NewLine);
            if (e.UserMsg != null) txtProtocolsLog.AppendText(e.UserMsg + Environment.NewLine);
        }

        private void GetProtocols()
        {
            //protocolsBinding.DataSource = null;
            protocolsBinding.DataSource = clinic.Protocols;
            protocolDetailsBinding.DataSource = protocolsBinding;

            lstProtocols.DataSource = null;
            lstProtocols.DataSource = protocolDetailsBinding;
            lstProtocols.DisplayMember = "Name";

            protocolExerciseBindingSource = new BindingSource(protocolDetailsBinding, "Exercises");

            lstProtocolsExercises.DataSource = null;
            lstProtocolsExercises.DataSource = protocolExerciseBindingSource;
            lstProtocolsExercises.DisplayMember = "GetExerciseName";

            // Debug.WriteLine(protocolDetailsBinding.ToString());

            listRepetitions.DataSource = null;
            listRepetitions.DataSource = protocolExerciseBindingSource;
            listRepetitions.DisplayMember = "Repetitions";

            //lstProtocols.DataSource = blProtocols;
            //lstProtocols.DisplayMember = "Name";

            //lstProtocolsExercises.DataSource = ((Protocol)lstProtocols.SelectedItem).Exercises;
            //lstProtocolsExercises.DisplayMember = "GetExerciseName";

            //listRepetitions.DataSource = ((Protocol)lstProtocols.SelectedItem).Exercises;
            //listRepetitions.DisplayMember = "Repetitions";
        }

        private void UpdateProtocols()
        {

            //protocolDetailsBinding.DataSource = null;
            //protocolsBinding.DataSource = null;

            protocolsBinding.ResetBindings(true);
            protocolDetailsBinding.ResetBindings(true);
            protocolsBinding.DataSource = clinic.Protocols;
            protocolDetailsBinding.DataSource = protocolsBinding;

            //protocolsBinding.ResetBindings(true);
            //protocolDetailsBinding.ResetBindings(false);
            //protocolExerciseBindingSource.ResetBindings(false);

            //lstProtocols.DataSource = null;
            //lstProtocols.DataSource = protocolDetailsBinding;
            //lstProtocols.DisplayMember = "Name";



            //lstProtocolsExercises.DataSource = null;
            //lstProtocolsExercises.DataSource = protocolExerciseBindingSource;
            //lstProtocolsExercises.DisplayMember = "GetExerciseName";

            //listRepetitions.DataSource = null;
            //listRepetitions.DataSource = protocolExerciseBindingSource;
            //listRepetitions.DisplayMember = "Repetitions";

            //blProtocols.ResetBindings();

            //lstProtocols.DataSource = null;
            //lstProtocols.DataSource = blProtocols;
            //lstProtocols.DisplayMember = "Name";
            lstProtocols.Refresh();
            lstProtocols.SelectedIndex = 0;

            //lstProtocolsExercises.DataSource = null;
            //lstProtocolsExercises.DataSource = ((Protocol)lstProtocols.SelectedItem).Exercises;
            //lstProtocolsExercises.DisplayMember = "GetExerciseName";
            lstProtocolsExercises.Refresh();
            //listRepetitions.DataSource = null;
            //listRepetitions.DataSource = ((Protocol)lstProtocols.SelectedItem).Exercises;
            //listRepetitions.DisplayMember = "Repetitions";
            listRepetitions.Refresh();
        }



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
        /*     private void InputDetectedEvent(object sender, LANCBEvenArgs e)
             {
                 Invoke(new EventHandler(delegate { txtServerLog.AppendText(NW.msgs.ToString());  } ));
        /*         {
                     //informação chega de forma assincrona
                     this.textBoxLog.AppendText(e.MsgString);

                     if (!e.MsgString.Contains("\n")) msgRcvHOH += e.MsgString;
                     else
                     {
                         msgRcvHOH += e.MsgString;
                         //verifica se a mão foi testada e inicia o hand brace testing
                         if (msgRcvHOH.Contains("untested")) {
                             txtServerLog.AppendText("\r\nForcing Hand testing");
                             buttontest_Click(sender, e);
                             msgRcvHOH = "";
                         }
     */

        /*//detecta qual o movimento CPM a partir da msg
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
                            */
        //processar mensagens retornadas pela HOH em caso de modo CTM
        /*               if (runCTM)
                         {

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

                             if (runCTMOpen)
                             {
                                 if (msgRcvHOH.Contains("094%") && msgRcvHOH.Contains("Opening"))
                                 {
                                     txtCTMLog.AppendText("\r\nHOH -> OPENING HAND");
                                     //sendAll(((char)12).ToString());

                                 }
                                 if (msgRcvHOH.Contains("Exit hand closing"))
                                 {
                                     txtCTMLog.AppendText("\r\nHOH -> ClOSING HAND");
                                     //sendAll(((char)11).ToString());
                                     //FORCAR FECHO DE MAO AUTOMATICO
                                     CPMCounter++;
                                     buttonfullyclose_Click(sender, e);
                                     previousCMDReceived = 0;
                                 }
                                 //txtCTMLog.AppendText("->" + msgRcvHOH);
                                 msgRcvHOH = "";
                             }
                         }

                     }     
                 }));
             }*/

        private void ProcessCommand()
        {
            //takecare of lastReceivedCommand;
            commandProcessed = true;
        }


        public bool PrintEndOfMove(string msg)
        {
            Debug.WriteLine(msg);
            return true;
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
                    //NW.InputChanged += InputDetectedEvent;
                    buttonConnect.Text = "Disconnect";
                    Debug.WriteLine("HOH Connected");
                    connectedHOH = true;
                    textBoxLog.Text = "";
                    btnProtocolStart.Enabled = true;

                    /*TxtBoxUpdater = new MRNetworkTxtBoxUpdater(NW, textBoxLog);
                    LogUpdater = new Thread(() => TxtBoxUpdater.Run());
                    LogUpdater.Start();*/
                    //NW.ExecuteAndWait("00", "done");
                    NW.Send("00");
                    //NW.ExecuteAndWait("00", "untested");
                    Debug.WriteLine("status" + NW.GetStatusMsg());

                }
                else
                {
                    //NW.InputChanged -= InputDetectedEvent;
                    // MessageBox.Show("Connect fail");

                    var result = MessageBox.Show
                           ("Failed to find the device at " + deviceIP + ":" + devicePORT, "Connection fail!", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (result == DialogResult.Retry)
                        buttonConnect_Click(sender, e);
                    else
                    {
                        connectedHOH = false;
                        //btnProtocolStart.Enabled = false;
                    }
                }
            }
            else
            {
                //resets hand
                buttonfullyopen_Click(sender, e);
                if (NW.Disconnect())
                {
                    buttonConnect.Text = "Connect";
                    //necessário para impedir duplicação de recebimentos na callback
                    //NW.InputChanged -= InputDetectedEvent;
                    connectedHOH = false;
                    btnProtocolStart.Enabled = false;
                    //TxtBoxUpdater.Stop();
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
            // NW.Send("06");
            //criar thread para este método
            Thread exec = new Thread(() => NW.ExecuteAndWait("06", "Exit restoring", PrintEndOfMove));
            exec.Start();
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
            CPMTimeCounter = Utils.TimeToCounter(Convert.ToInt32(numericCPMUpDownMinutes.Value), Convert.ToInt32(numericCPMUpDownSeconds.Value));
            lblCPMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
            lblCPMCounter.Text = numericCPMUpDownCounter.Value.ToString();
            lblCTMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
            lblCTMCounter.Text = numericCTMUpDownCounter.Value.ToString();
            trackCTMThreshold.Value = trackCTMBaseline.Value + 5;
            buttonConnect_Click(sender, e);
            if (!connectedHOH) tabControl.SelectedTab = tabDEMO;


            tabControl.TabPages.Remove(tabCPM);            
            tabControl.TabPages.Remove(tabCTM);

          
        }

        private void Mainform_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine("All clear!");
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
            lblCPMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
            lblCPMCounter.Text = CPMCounter.ToString();

            if (runCTM)
            {
                if (rbCTMTimer.Checked)
                {
                    CPMTimeCounter--;
                    //incrementar contador de movimentos concluidos
                    if (CPMTimeCounter == 0)
                    {
                        btnCTMStart_click(sender, e); //enviar para tarefa de fim de exercício
                        if (runCTMClose) txtCTMLog.AppendText("\r\nCTM - Close Hand treatment concluded! Well done!");
                        if (runCTMOpen) txtCTMLog.AppendText("\r\nCTM - Open Hand treatment concluded! Well done!");
                    }
                }
                if (rbCTMCounter.Checked)
                {
                    CPMTimeCounter++;
                    //decrementar contador de movimentos concluidos
                }
            }
            lblCTMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
            lblCTMCounter.Text = CPMCounter.ToString();
        }

        /// <summary>
        /// Reset sync timer
        /// </summary>
        private void timerSyncReset()
        {
            ExerciceResetTimer.Stop();
            //timer_sync.Enabled = false;
            //timer_sync.Enabled = true;
            timeSync = 0;
        }

        private void actionTimer_Tick(object sender, EventArgs e)
        {
            LastCMDReceived = AsyncServer.LastCMDReceived;
            commandProcessed = AsyncServer.commandProcessed;
            if (runCTMClose)
            {
                if (LastCMDReceived == 22 && LastCMDReceived != previousCMDReceived && !commandProcessed)
                { //se sinal detectado indica o movimento desejado actua em conformidade
                    buttonfullyclose_Click(sender, e); //envia comando de CTM Close 
                    txtCTMLog.AppendText("\r\nWell done, closing hand!");
                    //commandProcessed = true;    //certifica que não há comandos processados multiplas 
                }
            }

            if (runCTMOpen)
            {
                if (LastCMDReceived == 21 && LastCMDReceived != previousCMDReceived && !commandProcessed)
                { //se sinal detectado indica o movimento desejado actua em conformidade
                    buttonfullyopen_Click(sender, e); //envia comando de CTM Close 
                    txtCTMLog.AppendText("\r\nWell done, opening hand!");
                    //commandProcessed = true;    //certifica que não há comandos processados multiplas 
                }
            }

            previousCMDReceived = LastCMDReceived;
            commandProcessed = true;

            lblServerClientsConnected.Text = "Clients connected: " + AsyncServer.MySocketList.Count.ToString();
            lblCPMClientsConnected.Text = "Clients connected: " + AsyncServer.MySocketList.Count.ToString();
        }

        private void ExerciceResetTimer_Tick(object sender, EventArgs e)
        {
            txtCPMLog.AppendText("\r\nExercise Timeout: reseting... " + msgToSendSF);
            txtCTMLog.AppendText("\r\nExercise Timeout: reseting... " + msgToSendSF);
            //  txtSTMLog.AppendText("\r\nTIMER_SYNC: " + timeSync + "s");

            ExerciceResetTimer.Stop();
            //resets HOH
            if (runCTMClose)
            {

                //sendAll(((char)11).ToString());
                //FORCAR ABERTURA DE MAO AUTOMATICO
                if (cbxCTMAutoMove.Checked)
                {
                    txtCTMLog.AppendText("\r\nHOH COMPLETE -> CLOSE HAND");
                    buttonfullyclose_Click(sender, e);
                    CPMCounter++;
                }
                if (cbxCTMAutoReset.Checked)
                {
                    txtCTMLog.AppendText("\r\nHOH RESET -> OPEN HAND");
                    buttonfullyopen_Click(sender, e);
                }
                previousCMDReceived = 0;
            }

            if (runCTMOpen)
            {
                //sendAll(((char)11).ToString());
                //FORCAR FECHO DE MAO AUTOMATICO
                if (cbxCTMAutoMove.Checked)
                {
                    txtCTMLog.AppendText("\r\nHOH COMPLETE -> OPEN HAND");
                    buttonfullyopen_Click(sender, e);
                    CPMCounter++;
                }
                if (cbxCTMAutoReset.Checked)
                {
                    txtCTMLog.AppendText("\r\nHOH RESET -> CLOSE HAND");
                    buttonfullyclose_Click(sender, e);
                }
                previousCMDReceived = 0;
            }

            //checks if reset is done and allows restart  timer
            ExerciceResetTimer.Start();



            //Sends Sync Message
            sendAll(((char)msgToSendSF).ToString());
        }






        #endregion


        #region server tab

        private void btnServerSend_Click(object sender, EventArgs e)
        {
            AsyncServer.Send(AsyncServer.currentClient, txtServerSend.Text);
        }

        private void btnServerStart_Click(object sender, EventArgs e)
        {
            serverPORT = txtServerPort.Text;
            ServerSL = new Thread(() => AsyncServer.StartListening(Int32.Parse(serverPORT)));
            AsyncServer.SetLogBox(txtServerLog);
            ServerSL.Start();
            btnServerStop.Enabled = true;
            btnServerStart.Enabled = false;
        }

        private void btnServerStop_Click(object sender, EventArgs e)
        {
            AsyncServer.StopServer();
            ServerSL = null;
            btnServerStop.Enabled = false;
            btnServerStart.Enabled = true;
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
            lblServerClientsConnected.Text = "Clients connected: " + AsyncServer.MySocketList.Count.ToString();

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
                TreatmentTimer.Start();
                txtCPMLog.AppendText("\r\nWhole Hand treatment started");

                //deve reenviar comando sempre que o movimento terminar até que haja interrupção
                //deve terminar de mao aberta
                //deve enviar indicação de alteração de movimento para sfunction assim como de cada vez que há interrupcao ou quando o exercicio termina
                ExerciceResetTimer.Start();
                buttonCPM_Click(sender, e); //envia comando de CPM 

            }
            else
            {
                btnWhole.Text = "Whole hand";
                btnPinch.Enabled = true;
                gbTimer.Enabled = true;
                gbCounter.Enabled = true;
                TreatmentTimer.Stop();
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
            CPMTimeCounter = Utils.TimeToCounter(Convert.ToInt32(numericCPMUpDownMinutes.Value), Convert.ToInt32(numericCPMUpDownSeconds.Value));
            lblCTMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
        }

        private void numericCPMUpDownSeconds_ValueChanged(object sender, EventArgs e)
        {
            CPMTimeCounter = Utils.TimeToCounter(Convert.ToInt32(numericCPMUpDownMinutes.Value), Convert.ToInt32(numericCPMUpDownSeconds.Value));
            lblCPMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
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
                CPMTimeCounter = Utils.TimeToCounter(Convert.ToInt32(numericCPMUpDownMinutes.Value), Convert.ToInt32(numericCPMUpDownSeconds.Value));
                lblCPMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
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
            CPMTimeCounter = Utils.TimeToCounter(Convert.ToInt32(numericCTMUpDownMinutes.Value), Convert.ToInt32(numericCTMUpDownSeconds.Value));
            lblCTMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
        }

        private void numericCTMUpDownSeconds_ValueChanged(object sender, EventArgs e)
        {
            CPMTimeCounter = Utils.TimeToCounter(Convert.ToInt32(numericCTMUpDownMinutes.Value), Convert.ToInt32(numericCTMUpDownSeconds.Value));
            lblCTMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
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
                CPMTimeCounter = Utils.TimeToCounter(Convert.ToInt32(numericCTMUpDownMinutes.Value), Convert.ToInt32(numericCTMUpDownSeconds.Value));
                lblCTMTimer.Text = Utils.TimeToStr(CPMTimeCounter);
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


        private void btnCTMStart_click(object sender, EventArgs e)
        {
            if (comboTreatment.SelectedItem == null)
                comboTreatment.SelectedIndex = 0;

            switch (comboTreatment.SelectedItem.ToString())
            {
                case "CTM Close":
                    runCTMClose = true;
                    break;

                case "CTM Open":
                    runCTMOpen = true;
                    break;

                default:
                    break;
            }


            if (!runCTM)
            {
                //makes sure there is an exercise selected
                btnCTMStart.Text = "Stop";
                //              btnCTMOpenHand.Enabled = false;
                gbCTMTimer.Enabled = false;
                gbCTMCounter.Enabled = false;
                if (rbCTMTimer.Checked) CPMCounter = 0;
                if (rbCTMCounter.Checked) CPMTimeCounter = 0;
                TreatmentTimer.Start();
                ExerciceResetTimer.Start();
                ExerciceResetTimer_Tick(sender, e);
                actionTimer.Start();
                if (runCTMOpen) txtCTMLog.AppendText("\r\nContinuous Triggered Mode - Open Hand treatment started");
                if (runCTMClose) txtCTMLog.AppendText("\r\nContinuous Triggered Mode - Close Hand treatment started");
                //deve reenviar comando sempre que o movimento terminar até que haja interrupção
                //deve terminar de mao aberta
                //deve enviar indicação de alteração de movimento para sfunction assim como de cada vez que há interrupcao ou quando o exercicio termina
                //desactivar outros modos

                //update msgToSendSF
                comboTreatment_SelectedIndexChanged(sender, e);
            }
            else
            {
                btnCTMStart.Text = "Start";
                //               btnCTMOpenHand.Enabled = true;
                gbCTMTimer.Enabled = true;
                gbCTMCounter.Enabled = true;
                TreatmentTimer.Stop();
                if (CPMTimeCounter != 0) txtCPMLog.AppendText("\r\nCTM Treatment paused");
                timerSyncReset();
                actionTimer.Stop();
                buttonPause_Click(sender, e);
                buttonExit_Click(sender, e);
                if (runCTMOpen) runCTMOpen = false;
                if (runCTMClose) runCTMClose = false;
            }
            runCTM = !runCTM;
        }



        #endregion

        #endregion



        private void comboTreatment_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboTreatment.SelectedItem.ToString())
            {
                case "CTM Close":
                    msgToSendSF = 22;
                    break;

                case "CTM Open":
                    msgToSendSF = 21;
                    break;

                default:
                    break;
            }

        }

        private void numericUpDownExerciceTime_ValueChanged(object sender, EventArgs e)
        {
            ExerciceResetTimer.Interval = (int)numericUpDownExerciceTime.Value * 1000;
        }

        private void textBoxLog_TextChanged(object sender, EventArgs e)
        {
            textBoxLog.SelectionStart = textBoxLog.TextLength;
            textBoxLog.ScrollToCaret();
        }

        private void Mainform_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (connectedHOH)
            {
                //resets hand
                buttonfullyopen_Click(sender, e);
                //disconnects hand socket
                buttonConnect_Click(sender, e);
                NW.Disconnect();
            }


            //stops sfunction server
            btnServerStop_Click(sender, e);
            //clears events
            HOHEvent.LogUpdated -= OnHOHEventUpdate;
            HOHEvent.UsrMsgUpdated -= OnHOHEventUpdate;
            HOHEvent.ClinicUpdated -= OnClinicEventUpdate;

            try
            {
                if (defaultFileName == txtOptionsProtocolSeedFile.Text)
                    saveProtocolsToolStripMenuItem_Click(sender, e);
                else File.WriteAllText(defaultFileName, clinic.ToJSON());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Couldn't write file : + " + openFileDialog1.FileName);
                Debug.WriteLine(ex.Message);
            }


            Properties.Settings.Default.defaultJsonFile = defaultFileName;
            Properties.Settings.Default.deviceIP = deviceIP;
            Properties.Settings.Default.devicePORT = devicePORT;
            Properties.Settings.Default.serverPORT = serverPORT;

            Properties.Settings.Default.Save();
           

        }

        private void protocolsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProtocolEditor f1 = new ProtocolEditor(clinic);
            f1.ShowDialog();
            Debug.WriteLine("form");
            //f1.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1.AppendText(clinic.States[0].Name);
        }

        private void lstProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstProtocolsExercises.DataSource = null;
            lstProtocolsExercises.DataSource = ((Protocol)lstProtocols.SelectedItem).Exercises;
            lstProtocolsExercises.DisplayMember = "GetExerciseName";

            listRepetitions.DataSource = null;
            listRepetitions.DataSource = ((Protocol)lstProtocols.SelectedItem).Exercises;
            listRepetitions.DisplayMember = "Repetitions";
        }

        private void tabProtocol_Click(object sender, EventArgs e)
        {

        }

        private void lstProtocolsExercises_Format(object sender, ListControlConvertEventArgs e)
        {
            string str1 = ((Exercise)e.ListItem).Name; 
            string str2 = ((Exercise)e.ListItem).Repetitions.ToString();
            e.Value = "(x" + str2 + ") " + str1;
        }

        private void btnProtocolStart_Click(object sender, EventArgs e)
        {


            Protocol pt = ((Protocol)lstProtocols.SelectedItem);
            ProtocolGUI protocolGUI = new ProtocolGUI(pt, NW);
            protocolGUI.Show();

            Thread ProtoRun = new Thread(() => pt.Execute(NW));
            ProtoRun.Start();

            //new Thread(new ThreadStart(((Protocol)lstProtocols.SelectedItem).Execute)).Start(); 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            txtServerLog.SelectionStart = txtServerLog.TextLength;
            txtServerLog.ScrollToCaret();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtProtocolsLog.Text = "";
        }

        private void txtProtocolsLog_TextChanged(object sender, EventArgs e)
        {
            txtServerLog.SelectionStart = txtServerLog.TextLength;
            txtServerLog.ScrollToCaret();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtProtocolsLog.Text = clinic.ToJSON();
            //Debug.WriteLine(clinic.ToJSON());
        }

        private void saveProtocolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JSON (*.json) | *.json";
            saveFileDialog1.DefaultExt = "json";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.FileName = defaultFileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveFileDialog1.FileName, clinic.ToJSON());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Couldn't write file : + " + openFileDialog1.FileName);
                    Debug.WriteLine(ex.Message);
                }
                defaultFileName = saveFileDialog1.FileName;
                statusBar1.Text = "Using : " + Path.GetFileName(defaultFileName);
                //MessageBox.Show("Protocols Saved!");
            }
        }

        private void loadProtocolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //LoadProtocols();
            openFileDialog1.Filter = "JSON (*.json) | *.json";
            openFileDialog1.FileName = defaultFileName;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string str = String.Empty;
                try
                {
                    str = File.ReadAllText(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Couldn't read file : + " + openFileDialog1.FileName);
                    Debug.WriteLine(ex.Message);
                }
                clinic.FromJSON(str);
                // clinic = new Clinic(clinic);
                defaultFileName = openFileDialog1.FileName;
                statusBar1.Text = "Using : " + Path.GetFileName(defaultFileName);
            }
            //HOHEventObj.UpdateClinic(clinic);
            UpdateProtocols();
        }

        private void lblCALThresholdFlexor_Click(object sender, EventArgs e)
        {

        }

        private void cPMToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //// ((ToolStripMenuItem)viewToolStripMenuItem.DropDownItems[0]).Checked = !((ToolStripMenuItem)viewToolStripMenuItem.DropDownItems[0]).Checked;
            // if (!((ToolStripMenuItem)viewToolStripMenuItem.DropDownItems[0]).Checked) tabControl.TabPages.Remove(tabs[0]);
            // else tabControl.TabPages.Add(tabs[0]);
            // //tabControl.TabPages[tabControl.TabPages.Count].
            //if (chkMenuProtocols.Checked) tabControl.TabPages.Remove(tabProtocol);
            //else tabControl.TabPages.Add(tabProtocol);
        }


        private void tabControl_DragOver(object sender, DragEventArgs e)
        {
            Point pos = tabControl.PointToClient(Control.MousePosition);
            for (int ix = 0; ix < tabControl.TabCount; ++ix)
            {
                if (tabControl.GetTabRect(ix).Contains(pos))
                {
                    tabControl.SelectedIndex = ix;
                    break;
                }
            }
        }

        private void chkHOHOnline_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void btnOptionsApply_Click(object sender, EventArgs e)
        {
            deviceIP = txtOptionsDeviceIP.Text;
            devicePORT = txtOptionsDevicePort.Text;
            defaultFileName = txtOptionsProtocolSeedFile.Text;

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void LoadProtocols()
        {

            Debug.WriteLine("FileName : " + defaultFileName);
            

            string msg = String.Empty;
            string str = String.Empty;
            try
            {
                Debug.WriteLine("Opening file " + defaultFileName);
                try
                {

                    str = File.ReadAllText(defaultFileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Invalid default file name : " + defaultFileName);
                    Debug.WriteLine(ex.Message);
                }
              
                if (str?.Length == 0)
                {
                    loadProtocolsToolStripMenuItem_Click(null, EventArgs.Empty);
                    //Seed.SetupData(clinic);
                    msg = "Protocols seeded";
                }
                else
                {
                    clinic.FromJSON(str);
                    msg = "Protocols loaded";
                }
            }
            catch
            {
                Seed.SetupData(clinic);
                msg = "Protocols seeded";
            }
            Debug.WriteLine(msg);
        }
    }
}
