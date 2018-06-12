using System;
using System.Collections.Generic;

using System.Text;


namespace HOH_Library
{
    public class Exercise {

        public string Name { get; set; }
        /// <summary>
        /// Resets HOH to the proper position (optional)
        /// </summary>
        public State PreCondition { get; set; }
        /// <summary>
        /// Time allowed for each exercise. Discounts pre and post conditions times
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
        public State Movement { get; set; }
        /// <summary>
        /// Sets HOH to a position (optional)
        /// </summary>
        public State PostCondition { get; set; }

        public Exercise()
        {
            this.Name = "New exercise";
            this.PreCondition = null;
            this.ExerciseTime = 0;
            this.UserMsg = "";
            this.SFCode = "";
            this.Movement = null;
            this.PostCondition = null;
        }

        public void Execute()
        {

        }
    }
}
