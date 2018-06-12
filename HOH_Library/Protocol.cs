using System;
using System.Collections.Generic;

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

        public void Execute()
        {

        }

    }
}
