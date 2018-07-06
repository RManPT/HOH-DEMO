using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HOH_Library
{
    public class SFListener
    {
        public int WaitTime { get; set; }
        public int LastCMDReceived { get; set; }
        public bool commandProcessed { get; set; }
        public int previousCMDReceived { get; set; }
        public State TargetState { get; set; }
        public int SFCode { get; set; }
        private int TimerCounter;
        private object HomeThread;
        private bool ExecuteStatus { get; set; }
        private readonly HOHEvent HOHEventObj;
        


        public SFListener(State targetState, int command, int time, object obj)
        {
            this.WaitTime = time;
            this.TargetState = targetState;
            this.SFCode = command;
            this.HomeThread = obj;
            HOHEventObj = new HOHEvent();
        }

        public void Execute(MRNetwork NW)
        {
            ExecuteStatus = true;
            bool firstRun = true;
            System.Threading.Timer theTimer = new System.Threading.Timer(this.Tick, null, 0, 1000);
            this.TimerCounter = this.WaitTime;
            Debug.WriteLine("SFLISTENER : START!");

            HOHEventObj.UpdateLogMsg("SFListener: START");
            while (ExecuteStatus)
            {
                if (AsyncServer.IsConnected())
                {
                    LastCMDReceived = AsyncServer.LastCMDReceived;
                    commandProcessed = AsyncServer.commandProcessed;

                    //Código a executar quando os testes estiverem concluídos.
                    if (LastCMDReceived == SFCode && LastCMDReceived != previousCMDReceived && !commandProcessed)
                    { //se sinal detectado indica o movimento desejado actua em conformidade
                        if (firstRun)
                        {
                            this.TargetState.execute(NW);
                            firstRun = false;
                        }
                        else
                        {
                            NW.Send("r");
                        }

                        commandProcessed = true;    //certifica que não há comandos processados multiplas 
                        previousCMDReceived = LastCMDReceived;
                    }
                    else
                    {
                        NW.Send("p");
                    }
                }
                else
                { 
                    if (this.TargetState != null && !commandProcessed)
                    { 
                            this.TargetState.execute(NW);
                            commandProcessed = true;
                    }
                }
                Thread.Sleep(20);
            }

            previousCMDReceived = LastCMDReceived;
            commandProcessed = true;
            theTimer.Dispose();
            // Monitor.Pulse(HomeThread);          //experiencia
            Debug.WriteLine("SFLISTENER : END!");
            HOHEventObj.UpdateLogMsg("SFListener: END");
        }

        // The timer ticked.
        public void Tick(object info)
        {
            HOHEventObj.UpdateExerciseTimer(this.TimerCounter--);
        }

        public void InterruptListener(MRNetwork NW) {
            NW.ExecuteStatus = false;
            this.ExecuteStatus = false;
        }
    }
}
