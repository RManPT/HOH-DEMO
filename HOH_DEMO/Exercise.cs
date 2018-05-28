using System;
using System.Collections.Generic;

using System.Text;


namespace HOH_DEMO
{
    public class Exercise { 
    
        /// <summary>
        /// Resets HOH to the proper position (optional)
        /// </summary>
        public Condition PreCondition { get; set; }
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
        public int SFCode { get; set; }
        /// <summary>
        /// Code to send to HOH, represents one wanted action
        /// </summary>
        public Condition Movement { get; set; }
        /// <summary>
        /// Sets HOH to a position (optional)
        /// </summary>
        public Condition PostCondition { get; set; }

        public Exercise()
        {

        }
    }
}
