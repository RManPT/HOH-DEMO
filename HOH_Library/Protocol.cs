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
        public IList<ProtocolExercise> Exercises { get; set; }


        public Protocol()
        {
            this.Name = "New protocol";
        }

        public void Execute(MRNetwork NW)
        {
            Debug.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>><<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            Debug.WriteLine("PROTOCOL: START!");
            foreach (ProtocolExercise ex in Exercises)
            {
               ex.Execute(NW);
                //forçar espera por fim de execução para passar ao proximo item.
                
            }
            Debug.WriteLine("PROTOCOL: DONE!");
        }
    }
}
