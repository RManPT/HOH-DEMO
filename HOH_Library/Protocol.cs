using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


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
            this.Name = "New protocol";
            
        }

        public void Execute(MRNetwork NW)
        {
            HOHEventObj.UpdateLogMsg("PROTOCOL: START!");
            foreach (Exercise ex in Exercises)
            {
                ex.Execute(NW);
            }
            HOHEventObj.UpdateLogMsg("PROTOCOL: DONE!");
        }
    }
}
