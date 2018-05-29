
using System;
using System.Collections.Generic;

using System.Text;


namespace HOH_DEMO_Library
{
    public class Condition
    {
        /// <summary>
        /// Code to send to HOH, represents one wanted action
        /// </summary>
        public int HOHCode { get; set; }
        /// <summary>
        /// Message that is sent to UI. 
        /// </summary>
        public string UserMsg { get; set; }
        /// <summary>
        /// What msg to detect end of move
        /// </summary>
        public string CallbackMsg { get; set; }

        public Condition()
        {

        }

        public void execute(MRNetwork NW) {
            NW.Send(HOHCode.ToString());
           
        }
    }
}
