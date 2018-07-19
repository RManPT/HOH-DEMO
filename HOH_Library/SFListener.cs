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
        private int ExerciseTime;
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
            System.Threading.Timer theTimer2 = new System.Threading.Timer(this.Tick2, null, 0,100);
            this.TimerCounter = this.WaitTime;
            this.ExerciseTime = this.WaitTime*1000;
            Debug.WriteLine("SFLISTENER : START!");
            AsyncServer.LastCMDReceived = 0;
            AsyncServer.commandProcessed = true;
            previousCMDReceived = 1;

            HOHEventObj.UpdateLogMsg("SFListener: START");
            while (ExecuteStatus && this.TimerCounter > 0)
            {
                if (AsyncServer.IsConnected())
                {
                    LastCMDReceived = AsyncServer.LastCMDReceived;
                    commandProcessed = AsyncServer.commandProcessed;
                    
                    Debug.WriteLine("Last> " + LastCMDReceived + " | Process> " + commandProcessed);
                    //Código a executar quando os testes estiverem concluídos.
                    if (LastCMDReceived == SFCode && LastCMDReceived != previousCMDReceived && !commandProcessed)
                    { //se sinal detectado indica o movimento desejado actua em conformidade
                        if (firstRun)
                        {
                            Debug.WriteLine("GOT IN!");
                            this.TargetState.execute(NW);
                            //Thread.Sleep(20);
                            firstRun = false;
                        }
                        else
                        {
                           NW.Send("r");
                           
                            //Thread.Sleep(50);
                        }

                        commandProcessed = true;    //certifica que não há comandos processados multiplas 
                        previousCMDReceived = LastCMDReceived;
                    }
                    else if (!commandProcessed)
                    {
                        NW.Send("p");
                        previousCMDReceived = LastCMDReceived;
                    }
                }
                else
                {  //debug mode ... no clients connected
                    if (this.TargetState != null && !commandProcessed)
                    { 
                            this.TargetState.execute(NW);
                            commandProcessed = true;
                    }
                }
                
            }
            NW.Send("x");
            previousCMDReceived = LastCMDReceived;
            commandProcessed = true;
            theTimer.Dispose();
            theTimer2.Dispose();
            // Monitor.Pulse(HomeThread);          //experiencia
            Debug.WriteLine("SFLISTENER : END!");
            HOHEventObj.UpdateLogMsg("SFListener: END");
            HOHEventObj.UpdateExerciseState(false);
        }

        private void Tick2(object state)
        {

            this.ExerciseTime = this.ExerciseTime - 100;
            if (this.ExerciseTime <= 0) HOHEventObj.UpdateExerciseState(false);
            //Debug.WriteLine(this.ExerciseTime);
        }

        // The timer ticked.
        public void Tick(object info)
        {
            HOHEventObj.UpdateExerciseTimer(this.TimerCounter--);
        }

        public void InterruptListener(MRNetwork NW) {
            NW.ExecuteStatus = false;
            this.ExecuteStatus = false;
            HOHEventObj.UpdateExerciseTimer(0);
            HOHEventObj.UpdateExerciseState(false);
        }
    }
}
