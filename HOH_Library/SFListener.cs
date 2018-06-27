﻿using System;
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
        private HOHEvent HOHEventObj;


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
            System.Threading.Timer theTimer = new System.Threading.Timer(this.Tick, null, 0, 1000);
            this.TimerCounter = this.WaitTime;
            Debug.WriteLine("SFLISTENER : START!");

            HOHEventObj.UpdateLogMsg("SFListener: START");
            while (ExecuteStatus)
            {
                //LastCMDReceived = AsyncServer.LastCMDReceived;
                //commandProcessed = AsyncServer.commandProcessed;

                //Código a executar quando os testes estiverem concluídos.
                //if (LastCMDReceived == SFCode && LastCMDReceived != previousCMDReceived && commandProcessed == false)
                //{ //se sinal detectado indica o movimento desejado actua em conformidade
                //    this.TargetState.execute(NW);
                //    //txtCTMLog.AppendText("\r\nWell done, closing hand!");
                //    //commandProcessed = true;    //certifica que não há comandos processados multiplas 
                //}

               //teste
                //    Debug.WriteLine("SFLISTENER : executing");
                if (this.TargetState != null && !commandProcessed)
                { 
                        this.TargetState.execute(NW);
                        commandProcessed = true;
                }
                //    ExecuteStatus = false;
                Thread.Sleep(20);
                //  break;
                commandProcessed = true;
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