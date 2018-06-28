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
        private HOHEvent HOHEventObj = new HOHEvent();


        public Protocol()
        {
            Name = "New protocol";
            
        }
        public Protocol(string name)
        {
            Name = name;
        }

        public void Execute(MRNetwork NW)
        {
            HOHEventObj.UpdateLogMsg("PROTOCOL: START!");
            HOHEventObj.UpdateProtocolState("running");
            foreach (Exercise ex in Exercises)
            {
 //               if (!ExecuteStatus) break;
                Random rnd = new Random();

                HOHEventObj.UpdateUsrMsg("Prepare to " + ex.TargetState.UserMsg.ToLower() + "...");
                HOHEventObj.UpdateExerciseName(ex.TargetState.Name);
                Thread.Sleep(5000);
                ex.Execute(NW);
                HOHEventObj.UpdateUsrMsg(Clinic.Rewards[rnd.Next(Clinic.Rewards.Count)]);
                Thread.Sleep(5000);
            }
            HOHEventObj.UpdateProtocolState("stopped");
            HOHEventObj.UpdateUsrMsg("Well done! Protocol complete.");
            HOHEventObj.UpdateLogMsg("PROTOCOL: DONE!");
        }
    }
}
