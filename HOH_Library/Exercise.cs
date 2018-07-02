using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace HOH_Library
{
    public class Exercise {

        public string Name { get; set; }
        /// <summary>
        /// Resets HOH to the proper position (optional)
        /// </summary>
        public State PreState { get; set; }
        /// <summary>
        /// Time allowed for each exercise. Discounts pre and post State times
        /// </summary>
        public int ExerciseTime { get; set; }
        /// <summary>
        /// Message that is sent to UI. 
        /// </summary>
        public string UserMsg { get; set; }
        /// <summary>
        /// Code to send and to expect from S-Function
        /// </summary>
        public string SFCode { get; set; }
        /// <summary>
        /// Code to send to HOH, represents one wanted action
        /// </summary>
        public State TargetState { get; set; }
        public int Repetitions { get; set; }
        /// <summary>
        /// Sets HOH to a position (optional)
        /// </summary>
        public State PostState { get; set; }
        public string GetExerciseName
        {
            get { return Name; }
        }

        private SFListener sf;
        private MRNetwork NW;
        private HOHEvent HOHEventObj = new HOHEvent();
 

        public Exercise()
        {
            this.Name = "New exercise";
            this.PreState = null;
            this.ExerciseTime = 0;
            this.UserMsg = "";
            this.SFCode = "";
            this.TargetState = null;
            this.Repetitions = 1;
            this.PostState = null;
           
        }

        public Exercise(string name)
        {
            this.Name = name;
            this.PreState = null;
            this.ExerciseTime = 0;
            this.UserMsg = "";
            this.SFCode = "";
            this.TargetState = null;
            this.Repetitions = 1;
            this.PostState = null;
        }

        public Exercise(Exercise ex)
        {
            this.Name = ex.GetExerciseName;
            this.PreState = ex.PreState;
            this.ExerciseTime = ex.ExerciseTime;
            this.UserMsg = ex.UserMsg;
            this.SFCode = ex.SFCode;
            this.TargetState = ex.TargetState;
            this.Repetitions = ex.Repetitions;
            this.PostState = ex.PostState;
  
        }

        public void Execute(MRNetwork NW)
        {
            this.NW = NW;
            for (int i = 0; i < this.Repetitions; i++)
            {
                HOHEventObj.UpdateLogMsg("EXERCISE: START!");
                HOHEventObj.UpdateLogMsg("Executing exercise: " + this.Name);

                if (this.PreState != null)
                { 
                    HOHEventObj.UpdateLogMsg("Setting Prestate: " + this.PreState.Name);
                    this.PreState.execute(NW);
                }

                if (this.TargetState != null)
                {
                    HOHEventObj.UpdateLogMsg("Target State: " + this.TargetState.Name);
                    HOHEventObj.UpdateUsrMsg(this.UserMsg);
                    SFListener sf = new SFListener(this.TargetState, Int32.Parse(this.SFCode), this.ExerciseTime, this);
                    Thread SFThread = new Thread(() => sf.Execute(NW));
                    SFThread.Start();

                    //this.TargetState.execute(NW);
                    Thread.Sleep(this.ExerciseTime * 1000);
                    sf.InterruptListener(NW);
                }

                if (this.PostState != null)
                {
                    HOHEventObj.UpdateLogMsg("Setting PostState: " + this.PostState.Name);
                    this.PostState.execute(NW);
                }
                HOHEventObj.UpdateLogMsg("EXERCISE: DONE!");
            }
        }

        private void OnHOHEventUpdate(object sender, HOHEvent e)
        {
            if (e.ProtocolState == "interrupt")
            {
                sf?.InterruptListener(NW);
            }
        }
    }
}
