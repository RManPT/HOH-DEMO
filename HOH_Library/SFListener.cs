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


        public SFListener(State TargetState, int command, int time, object obj)
        {
            this.WaitTime = time;
            this.TargetState = TargetState;
            this.SFCode = command;
            this.HomeThread = obj;
        }

        public void Execute(MRNetwork NW)
        {

            ExecuteStatus = true;
            //System.Threading.Timer TheTimer = new System.Threading.Timer(this.Tick, null, 0, 1000);
            Debug.WriteLine("SFLISTENER : START!");


            while (ExecuteStatus)
            {
                LastCMDReceived = AsyncServer.LastCMDReceived;
                commandProcessed = AsyncServer.commandProcessed;

                //Código a executar quando os testes estiverem concluídos.
                //if (LastCMDReceived == SFCode && LastCMDReceived != previousCMDReceived && commandProcessed == false)
                //{ //se sinal detectado indica o movimento desejado actua em conformidade
                //    this.TargetState.execute(NW);
                //    //txtCTMLog.AppendText("\r\nWell done, closing hand!");
                //    //commandProcessed = true;    //certifica que não há comandos processados multiplas 
                //}

               //teste
                    Debug.WriteLine("SFLISTENER : executing");
                    if (this.TargetState != null) this.TargetState.execute(NW);
                    commandProcessed = true;
                    ExecuteStatus = false;
                Thread.Sleep(20);
                break;
                
            }

            previousCMDReceived = LastCMDReceived;
            commandProcessed = true;
           // Monitor.Pulse(HomeThread);          //experiencia
            Debug.WriteLine("SFLISTENER : END!");
        }

        // The timer ticked.
        public void Tick(object info)
        {
            this.TimerCounter++;
        }

        public void InterruptListener(MRNetwork NW) {
            NW.ExecuteStatus = false;
            this.ExecuteStatus = false;
        }
    }
}
