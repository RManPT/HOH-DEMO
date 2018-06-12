using System;
using System.Collections.Generic;
using System.Text;

namespace HOH_Library
{
    public class LANCBEvenArgs : EventArgs
    {

        private string msg;
        public string MsgString
        {
            get { return this.msg; }
        }

        public LANCBEvenArgs(string msg)
        {
            this.msg = msg;
        }
    }
}
