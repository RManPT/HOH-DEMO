﻿
using System;
using System.Collections.Generic;

using System.Text;


namespace HOH_Library
{
    public class State
    {
        public string Name { get; set; }
        /// <summary>
        /// Code to send to HOH, represents one wanted action
        /// </summary>
        public string HOHCode { get; set; }
        /// <summary>
        /// Message that is sent to UI. 
        /// </summary>
        public string UserMsg { get; set; }
        /// <summary>
        /// What msg to detect end of move
        /// </summary>
        public string CallbackMsg { get; set; }

        public State()
        {
            this.Name = "New state";
            this.HOHCode = "";
            this.UserMsg = "";
            this.CallbackMsg = "";
        }

        public void execute(MRNetwork NW) {
            //NW.ExecuteAndWait(HOHCode.ToString(), CallbackMsg);  
        }
    }
}