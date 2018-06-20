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
        public State PreState { get; set; }
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
        public State TargetState { get; set; }
        /// <summary>
        /// Sets HOH to a position (optional)
        /// </summary>
        public State PostState { get; set; }

        public Exercise()
        {
            this.Name = "New exercise";
            this.PreState = null;
            this.ExerciseTime = 0;
            this.UserMsg = "";
            this.SFCode = "";
            this.TargetState = null;
            this.PostState = null;
        }

        public void Execute()
        {

        }
    }
}
