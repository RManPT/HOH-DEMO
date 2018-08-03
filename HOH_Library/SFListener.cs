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
            //System.Threading.Timer theTimer2 = new System.Threading.Timer(this.Tick2, null, 0,100);
            this.TimerCounter = this.WaitTime;
            this.ExerciseTime = this.WaitTime*1000;
            Debug.WriteLine("SFLISTENER : START!");
            previousCMDReceived = 1;

            
            HOHEventObj.UpdateLogMsg("SFListener: START");
            AsyncServer.SendAll(((char)SFCode).ToString());

            while (ExecuteStatus && this.TimerCounter >= -1)
            {
                
                if (firstRun || NW.ExecuteStatus)
                {

                    if (AsyncServer.IsConnected())
                    {
                        LastCMDReceived = AsyncServer.LastCMDReceived;
                        commandProcessed = AsyncServer.commandProcessed;
                        Debug.WriteLine("Last> " + LastCMDReceived + " | Process> " + commandProcessed + " | Firstrun> " + firstRun + " | ExecStatus " + NW.ExecuteStatus);

                        if (LastCMDReceived == SFCode && LastCMDReceived != previousCMDReceived && !commandProcessed)
                        { //se sinal detectado indica o movimento desejado actua em conformidade
                            if (firstRun)
                            {
                                Debug.WriteLine("GOT IN!");
                               // this.TargetState.execute(NW);

                                Thread SFThread = new Thread(() => this.TargetState.execute(NW));
                                SFThread.Start();

                                //garante que comando incial é processado pela mão (procurar solucao dinamica)
                                Thread.Sleep(100);
                                firstRun = false;
                                commandProcessed = true;
                                previousCMDReceived = LastCMDReceived;
                            }
                            else
                            {
                                NW.ExecuteAndWait("r","*", null);
                                //NW.Send("r");
                                //Necessario para dar tempo à mão para processar a excepção
                                //Thread.Sleep(40);
                                commandProcessed = true;
                                previousCMDReceived = LastCMDReceived;
                                Debug.WriteLine("Resuming (right signal)");
                            }
                        }
                        else if (LastCMDReceived != SFCode && !commandProcessed)
                        {
                            NW.ExecuteAndWait("p", "*",null);
                            //NW.Send("p");
                            //Thread.Sleep(40);
                            commandProcessed = true;
                            previousCMDReceived = LastCMDReceived;
                            Debug.WriteLine("Holding (wrong signal)");
                        }
                    }
                    else
                    {  //debug mode ... no clients connected
                        if (this.TargetState != null && !commandProcessed)
                        {
                            this.TargetState.execute(NW);
                        }
                    }
                }
                else if (!firstRun)
                {
                    //NW.Send("p");
                    //Thread.Sleep(40);
                    NW.ExecuteAndWait("p", "*", null);
                    commandProcessed = true;
                    previousCMDReceived = LastCMDReceived;
                    Debug.WriteLine("Holding (last 10%)");
                }
            }
            //NW.Send("r");
            //Thread.Sleep(100);
            NW.ExecuteAndWait("r", "*", null);
                
            //NW.Send("x");
            //Thread.Sleep(100);
            NW.ExecuteAndWait("x", "*", null);
            //previousCMDReceived = LastCMDReceived;
            //commandProcessed = true;
            theTimer.Dispose();

            Debug.WriteLine("SFLISTENER : END!");
            HOHEventObj.UpdateLogMsg("SFListener: END");
            HOHEventObj.UpdateExerciseState(false);
           // HOHEventObj.UpdateExerciseTimer(0);
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
            if (this.TimerCounter < 0) HOHEventObj.UpdateExerciseState(false);
        }

        public void InterruptListener(MRNetwork NW) {
            NW.ExecuteStatus = false;
            this.ExecuteStatus = false;
            HOHEventObj.UpdateExerciseTimer(0);
            HOHEventObj.UpdateExerciseState(false);
        }
    }
}
