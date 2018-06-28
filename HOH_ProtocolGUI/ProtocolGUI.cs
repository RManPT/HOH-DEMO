using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HOH_Library;

namespace HOH_ProtocolGUI
{
    public partial class ProtocolGUI : Form
    {

        private delegate void EventSubscribed(HOHEvent e);

        private Protocol protocol;
        private MRNetwork NW;
        private HOHEvent HOHEventObj = new HOHEvent();

        public ProtocolGUI(Protocol protocol, MRNetwork NW)
        {
            InitializeComponent();
            this.protocol = protocol;
            this.NW = NW;
            Text = protocol.Name;
            HOHEvent.LogUpdated += OnHOHEventUpdate;
            HOHEvent.UsrMsgUpdated += OnHOHEventUpdate;
            HOHEvent.ExerciseTimerUpdated += OnHOHEventUpdate;
            HOHEvent.ExerciseNameUpdated += OnHOHEventUpdate;
            HOHEvent.ProtocolStateUpdated += OnHOHEventUpdate;

        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void OnHOHEventUpdate(object sender, HOHEvent e)
        {
            Delegate d = new EventSubscribed(updateGUI);
            this.Invoke(d, e);
        }

        private void updateGUI(HOHEvent e)
        {
           if (e.ExerciseName != null) lblExerciseName.Text = (e.ExerciseName.ToString() + Environment.NewLine);
           if (e.UserMsg != null) lblExerciseMsg.Text = (e.UserMsg.ToString() + Environment.NewLine);
            lblExerciseTime.Text = Utils.TimeToStr(e.ExerciseTimer);

            if (e.ProtocolState != null)
            {
                btnRestart.Enabled = (e.ProtocolState.ToLower() == "stopped");
                lblExerciseTime.Visible = (e.ProtocolState.ToLower() == "running");
            }
            Thread.Yield();
        }

        private void ProtocolGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            HOHEvent.LogUpdated -= OnHOHEventUpdate;
            HOHEvent.UsrMsgUpdated -= OnHOHEventUpdate;
            HOHEvent.ExerciseTimerUpdated -= OnHOHEventUpdate;
            HOHEvent.ExerciseNameUpdated -= OnHOHEventUpdate;
        }

        private void lblExerciseTime_Click(object sender, EventArgs e)
        {

        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Thread ProtoRun = new Thread(() => protocol.Execute(NW));
            ProtoRun.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HOHEventObj.UpdateProtocolState("interrupt");
            btnStop.Enabled = false;
        }
    }
}
