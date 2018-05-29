using HOH_DEMO_Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HOH_Library
{
    public class MRNetworkTxtBoxUpdater
    {

        private MRNetwork NW;
        private string msgRcvHOH;
        private TextBox textBox;
        private bool status;

        public MRNetworkTxtBoxUpdater(MRNetwork obj, TextBox txtb)
        {
            this.NW = obj;
            this.textBox = txtb;
            this.status = true;
        }

        public void Run()
        { 
            while (status)
            {
                string msg = NW.GetStatusMsg();
                if (msgRcvHOH != msg)
                { 
                    msgRcvHOH = msg;
                    if (msgRcvHOH != "") SetText(this.textBox, msgRcvHOH);
                }
                //Debug.WriteLine("Operation concluded");
                Thread.Sleep(500);
            }
        }


        delegate void SetTextCallback(TextBox textBox, string text);
        private void SetText(TextBox textBox, string text)
        {
            if (textBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                textBox.Invoke(d, new object[] { textBox, text });
            }
            else
            {
                textBox.Text += text + System.Environment.NewLine;
            }
        }

        public void Stop()
        {
            this.status = false;
        }
    }
}
