using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;


namespace HOH_Library
{
    public class Protocol
    {   
        /// <summary>
        /// Protocol name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List of exercices and its repetitions that composes the protocol
        /// </summary>
        public IList<Exercise> Exercises { get; set; }
        public List<string> Rewards;
        private readonly HOHEvent HOHEventObj = new HOHEvent();
        private bool ExecuteStatus = true;

        public Protocol()
        {
            Name = "New protocol";
            Exercises = new List<Exercise>();
        }
        public Protocol(string name)
        {
            Name = name;
            Exercises = new List<Exercise>();
        }

        private void OnClinicEventUpdate(object sender, HOHEvent e)
        {
            Rewards = e.Clinic.Rewards;
        }

        public void Execute(MRNetwork NW)
        {
            HOHEvent.ClinicUpdated += OnClinicEventUpdate;
            HOHEvent.ProtocolStateUpdated += OnHOHEventUpdate;

            HOHEventObj.UpdateLogMsg("PROTOCOL: START!");
            HOHEventObj.UpdateProtocolState("running");
            foreach (Exercise ex in Exercises)
            {
               if (!ExecuteStatus) break;
                Random rnd = new Random();

                HOHEventObj.UpdateUsrMsg("\r\nPrepare for " + ex.TargetState.UserMsg.ToLower() + "...");
                HOHEventObj.UpdateExerciseName(ex.TargetState.Name);
                Thread.Sleep(5000);
                if (!ExecuteStatus) break;

                ex.Execute(NW);

                if (!ExecuteStatus) break;
          //      HOHEventObj.UpdateUsrMsg(Rewards[rnd.Next(Rewards.Count)]);
                Thread.Sleep(5000);
            }
            HOHEventObj.UpdateProtocolState("stopped");
            HOHEventObj.UpdateUsrMsg("Well done! Protocol complete.");
            HOHEventObj.UpdateLogMsg("PROTOCOL: DONE!");

        }

        private void OnHOHEventUpdate(object sender, HOHEvent e)
        {
            if (e.ProtocolState == "interrupt")
                ExecuteStatus = false;
            else
            {
                foreach (Exercise ex in Exercises)
                {
                    ex.ExecuteStatus = true;
                    ExecuteStatus = true;
                    
                }
            } 
        }
    }
}
