using System;
using System.Threading;
using System.Windows.Forms;
using HOH_Library;

namespace HOH_ProtocolGUI
{
    public partial class ProtocolGUI : Form
    {

        private delegate void EventSubscribed(HOHEvent e);

        private Protocol protocol;
        private MRNetwork NW;
        private Clinic c;
        private HOHEvent HOHEventObj = new HOHEvent();

        public ProtocolGUI(Clinic c, Protocol protocol, MRNetwork NW)
        {
            InitializeComponent();
            this.protocol = protocol;
            this.NW = NW;
            this.c = c;
            Text = protocol.Name;
            HOHEvent.LogUpdated += OnHOHEventUpdate;
            HOHEvent.UsrMsgUpdated += OnHOHEventUpdate;
            HOHEvent.ExerciseTimerUpdated += OnHOHEventUpdate;
            HOHEvent.ExerciseNameUpdated += OnHOHEventUpdate;
            HOHEvent.ProtocolStateUpdated += OnHOHEventUpdate;
            HOHEvent.RewardLauncherUpdated += OnHOHEventUpdate;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void OnHOHEventUpdate(object sender, HOHEvent e)
        {
            Delegate d = new EventSubscribed(updateGUI);
            if (IsHandleCreated) this?.Invoke(d, e);
        }

        private void updateGUI(HOHEvent e)
        {
            Random rnd = new Random();
            if (e.ExerciseName != null)
            {
                lblExerciseName.Text = (e.ExerciseName + Environment.NewLine);
            }

            if (e.LaunchReward) lblExerciseMsg.Text = c.Rewards[rnd.Next(c.Rewards.Count)];
                //HOHEventObj.UpdateUsrMsg(c.Rewards[rnd.Next(c.Rewards.Count)]);

            if (e.UserMsg != null)
            {
                lblExerciseMsg.Text = (e.UserMsg + Environment.NewLine);
            }

            lblExerciseTime.Text = Utils.TimeToStr(e.ExerciseTimer);

            if (e.ProtocolState != null)
            {
                btnRestart.Enabled = (string.Equals(e.ProtocolState, "stopped", StringComparison.OrdinalIgnoreCase));
                lblExerciseTime.Visible = (string.Equals(e.ProtocolState, "running", StringComparison.OrdinalIgnoreCase));
                if (btnRestart.Enabled) btnStop.Text = "Exit";
            }
            Thread.Yield();
        }

        private void ProtocolGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            HOHEvent.LogUpdated -= OnHOHEventUpdate;
            HOHEvent.UsrMsgUpdated -= OnHOHEventUpdate;
            HOHEvent.ExerciseTimerUpdated -= OnHOHEventUpdate;
            HOHEvent.ExerciseNameUpdated -= OnHOHEventUpdate;
            HOHEvent.ProtocolStateUpdated -= OnHOHEventUpdate;
            HOHEventObj.UpdateProtocolState("interrupt");
            HOHEventObj.UpdateProtocolGUIStatus(false);
        }

        private void lblExerciseTime_Click(object sender, EventArgs e)
        {

        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Thread ProtoRun = new Thread(() => protocol.Execute(NW));
            ProtoRun.Start();
            btnStop.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (btnStop.Text == "Stop")
            {
                HOHEventObj.UpdateProtocolState("interrupt");
                //btnStop.Enabled = false;
                // btnRestart.Enabled = true;
                btnStop.Text = "Exit";
            }
            else {
                //ProtocolGUI_FormClosing(sender, null );
                this.Close();
            }
        }
    }
}
