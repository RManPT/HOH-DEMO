using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace HOH_Library
{
    [Serializable]
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
        public bool ExecuteStatus = true;
        private bool exerciseRunning = false;

        private readonly HOHEvent HOHEventObj = new HOHEvent();


        public Exercise()
        {
            this.Name = "New exercise";
            this.PreState = null;
            this.ExerciseTime = 0;
            this.UserMsg = "";
            this.SFCode = "1";
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
            this.SFCode = "1";
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


        // Deep clone
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }


        public void Execute(MRNetwork NW)
        {
            ExecuteStatus = true;
            exerciseRunning = true;


            HOHEvent.ProtocolStateUpdated += OnHOHEventUpdate;
            HOHEvent.ExerciseStateUpdated += OnExerciseStateUpdated;

            this.NW = NW;
            for (int i = 0; i < this.Repetitions; i++)
            {
                HOHEventObj.UpdateLogMsg("EXERCISE: START!");
                HOHEventObj.UpdateLogMsg("Executing exercise: " + this.Name);

                if (this.PreState != null && ExecuteStatus)
                { 
                    HOHEventObj.UpdateLogMsg("Setting Prestate: " + this.PreState.Name);
                    this.PreState.execute(NW);
                }

                if (this.TargetState != null && ExecuteStatus)
                {
                    HOHEventObj.UpdateLogMsg("Target State: " + this.TargetState.Name);
                    HOHEventObj.UpdateUsrMsg(this.UserMsg);
                    sf = new SFListener(this.TargetState, Int32.Parse(this.SFCode), this.ExerciseTime, this);
                    Thread SFThread = new Thread(() => sf.Execute(NW));
                    SFThread.Start();
                    exerciseRunning = true;
                    while (exerciseRunning)
                    { }
                   // SFThread.Interrupt();
                    //Thread.Sleep(this.ExerciseTime * 1000);
                    sf.InterruptListener(NW);
                    //SFThread.Interrupt();
                    //sf = null;
                }

                if (this.PostState != null && ExecuteStatus)
                {
                    HOHEventObj.UpdateLogMsg("Setting PostState: " + this.PostState.Name);
                    this.PostState.execute(NW);
                }

              
                HOHEventObj.UpdateLogMsg("EXERCISE: DONE!");
            }
            HOHEvent.ProtocolStateUpdated -= OnHOHEventUpdate;
            HOHEvent.ExerciseStateUpdated -= OnExerciseStateUpdated;
            HOHEventObj.UpdateExerciseState(true);
            HOHEventObj.UpdateExerciseTimer(0);
        }

        private void OnHOHEventUpdate(object sender, HOHEvent e)
        {
            if (e.ProtocolState == "interrupt")
            {
                sf?.InterruptListener(NW);
                ExecuteStatus = false;
                NW.ExecuteStatus = false;
                
            }
            else {
                ExecuteStatus = true;
                NW.ExecuteStatus = true;
            }
        }

        private void OnExerciseStateUpdated(object sender, HOHEvent e)
        {
            exerciseRunning = e.ExerciseRunning;
            //ExecuteStatus = exerciseRunning;
             
        }

        public override bool Equals(object obj)
        {
            var newObj = obj as Exercise;

            if (null != newObj)
            {
                return this.Name == newObj.Name
                    && this.PreState.Equals(newObj.PreState)
                    //&& this.ExerciseTime == newObj.ExerciseTime
                    && this.UserMsg == newObj.UserMsg
                    && this.SFCode == newObj.SFCode
                    && this.TargetState.Equals(newObj.TargetState)
                    //&& this.Repetitions == newObj.Repetitions
                    && this.PostState.Equals(newObj.PostState);    
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }
}
