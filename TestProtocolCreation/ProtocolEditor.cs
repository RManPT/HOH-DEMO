using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HOH_Library;

namespace HOH_ProtocolEditor
{
    public partial class ProtocolEditor : Form
    {

        private Clinic clinic;
        private BindingList<State> blStates;
        private BindingList<Exercise> blExercises;
        private BindingList<Protocol> blProtocols;

        //BindingSource statesBinding = new BindingSource();
        //BindingSource statesDetailsBinding = new BindingSource();
        //BindingSource exercisesBinding = new BindingSource();
        //BindingSource exercisesDetailsBinding = new BindingSource();
        //BindingSource protocolsBinding = new BindingSource();
        //BindingSource protocolsDetailsBinding = new BindingSource();
        //BindingSource protocolExerciseBindingSource;
        int StatePanelLstStates_Selected = 0;
        int ExercisesPanelLstExercises_Selected = 0;
        int ProtocolPanelLstProtocols_Selected = 0;
        int ProtocolPanelLstProtocolExercises_Selected = 0;

        public ProtocolEditor(Clinic clinic)
        {
            InitializeComponent();
            this.clinic = clinic;
            this.blStates = new BindingList<State>(clinic.States);
            this.blExercises = new BindingList<Exercise>(clinic.Exercises);
            this.blProtocols = new BindingList<Protocol>(clinic.Protocols);


            ///States
            SetupStatePanel();

            ///exercises
            SetupExercisesPanel();

            ///protocols
            SetupProtocolsPanel();
        }


        #region Custom
        private void SetupStatePanel()
        {

            RemoveStatesEvents();
            //statesBinding.DataSource = blState;
            //statesDetailsBinding.DataSource = statesBinding;

            //lstStates.DataSource = statesDetailsBinding;
            //lstStates.DisplayMember = "Name";
            
            //txtConditionDetails1.DataBindings.Add("Text", blState, "Name");
            //txtConditionDetails2.DataBindings.Add("Text", statesDetailsBinding, "HOHCode");
            //txtConditionDetails3.DataBindings.Add("Text", statesDetailsBinding, "UserMsg");
            //txtConditionDetails4.DataBindings.Add("Text", statesDetailsBinding, "CallbackMsg");

            lstStates.DataSource = blStates;
            lstStates.DisplayMember = "Name";

            txtConditionDetails1.Text = ((State)lstStates.SelectedItem).Name;
            txtConditionDetails2.Text = ((State)lstStates.SelectedItem).HOHCode;
            txtConditionDetails3.Text = ((State)lstStates.SelectedItem).UserMsg;
            txtConditionDetails4.Text = ((State)lstStates.SelectedItem).CallbackMsg;
            AddStatesEvents();
        }

        private void SetupExercisesPanel()
        {
            RemoveExercisesEvents();


            //exercisesBinding.DataSource = clinic.Exercises;
            //exercisesDetailsBinding.DataSource = exercisesBinding;

            //lstExercises.DataSource = exercisesDetailsBinding;
            //lstExercises.DisplayMember = "Name";


            //txtExerciceDetails1.DataBindings.Add("Text", exercisesDetailsBinding, "Name");
            //txtExerciceDetails2.DataBindings.Add("Text", exercisesDetailsBinding, "SFCode");
            //txtExerciceDetails3.DataBindings.Add("Text", exercisesDetailsBinding, "UserMsg");
            //txtExerciceDetails4.DataBindings.Add("Text", exercisesDetailsBinding, "ExerciseTime");
            //txtExerciceDetails5.DataBindings.Add("Text", exercisesDetailsBinding, "Repetitions");

            //comboExerciseState1.BindingContext = new BindingContext();
            //comboExerciseState1.DataSource = statesBinding;
            //comboExerciseState1.DisplayMember = "Name";


            //comboExerciseState2.BindingContext = new BindingContext();
            //comboExerciseState2.DataSource = statesBinding;
            //comboExerciseState2.DisplayMember = "Name";

            //comboExerciseState3.BindingContext = new BindingContext();
            //comboExerciseState3.DataSource = statesBinding;
            //comboExerciseState3.DisplayMember = "Name";

            //lblDump1.DataBindings.Add("Text", exercisesDetailsBinding, "PreState.Name");
            //lblDump2.DataBindings.Add("Text", exercisesDetailsBinding, "TargetState.Name");
            //lblDump3.DataBindings.Add("Text", exercisesDetailsBinding, "PostState.Name");
            ////calls events context changed and selectedIndexChange

            lstExercises.DataSource = blExercises;
            lstExercises.DisplayMember = "Name";

            txtExerciseDetails1.Text = ((Exercise)lstExercises.SelectedItem).Name;
            txtExerciseDetails2.Text = ((Exercise)lstExercises.SelectedItem).SFCode;
            txtExerciseDetails3.Text = ((Exercise)lstExercises.SelectedItem).UserMsg;

            comboExerciseState1.BindingContext = new BindingContext();
            comboExerciseState1.DataSource = blStates;
            comboExerciseState1.DisplayMember = "Name";
            comboExerciseState2.BindingContext = new BindingContext();
            comboExerciseState2.DataSource = blStates;
            comboExerciseState2.DisplayMember = "Name";
            comboExerciseState3.BindingContext = new BindingContext();
            comboExerciseState3.DataSource = blStates;
            comboExerciseState3.DisplayMember = "Name";

            comboExerciseState1.Text = ((Exercise)lstExercises.SelectedItem).PreState.Name;
            comboExerciseState2.Text = ((Exercise)lstExercises.SelectedItem).TargetState.Name;
            comboExerciseState3.Text = ((Exercise)lstExercises.SelectedItem).PostState.Name;

            AddExercisesEvents();

        }


        private void SetupProtocolsPanel()
        {
            RemoveProtocolsEvents();

            //protocolsBinding.DataSource = clinic.Protocols;
            //protocolsDetailsBinding.DataSource = protocolsBinding;

            //lstProtocols.DataSource = protocolsDetailsBinding;
            //lstProtocols.DisplayMember = "Name";

            //protocolExerciseBindingSource = new BindingSource(protocolsDetailsBinding, "Exercises");

            //lstProtocolExercises.DataSource = protocolExerciseBindingSource;
            //lstProtocolExercises.DisplayMember = "GetExerciseName";

            //exercisesBinding.DataSource = clinic.Exercises;
            //exercisesDetailsBinding.DataSource = exercisesBinding;

            //lstAvailableExercises.DataSource = exercisesDetailsBinding;
            //lstAvailableExercises.DisplayMember = "Name";

            lstProtocols.DataSource = blProtocols;
            lstProtocols.DisplayMember = "Name";

            

            lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
            lstProtocolExercises.DisplayMember = "Name";
            if (lstProtocolExercises.Items.Count != 0) lstProtocolExercises.SelectedIndex = 0;

            lstAvailableExercises.DataSource = blExercises;
            lstAvailableExercises.DisplayMember = "Name";
            if (lstAvailableExercises.Items.Count!=0) lstAvailableExercises.SelectedIndex = 0;

            txtProtocolExerciseDetails1.Text = ((Exercise)lstProtocolExercises.SelectedItem).Name;
            txtProtocolExerciseDetails2.Text = ((Exercise)lstProtocolExercises.SelectedItem).UserMsg;
            txtProtocolExerciseDetails3.Text = ((Exercise)lstProtocolExercises.SelectedItem).SFCode;
            txtProtocolExerciseDetails4.Text = ((Exercise)lstProtocolExercises.SelectedItem).ExerciseTime.ToString();
            txtProtocolExerciseDetails5.Text = ((Exercise)lstProtocolExercises.SelectedItem).Repetitions.ToString();

            comboProtocolExerciseState1.BindingContext = new BindingContext();
            comboProtocolExerciseState1.DataSource = blStates;
            comboProtocolExerciseState1.DisplayMember = "Name";
            comboProtocolExerciseState2.BindingContext = new BindingContext();
            comboProtocolExerciseState2.DataSource = blStates;
            comboProtocolExerciseState2.DisplayMember = "Name";
            comboProtocolExerciseState3.BindingContext = new BindingContext();
            comboProtocolExerciseState3.DataSource = blStates;
            comboProtocolExerciseState3.DisplayMember = "Name";

            comboProtocolExerciseState1.Text = ((Exercise)lstProtocolExercises.SelectedItem).PreState.Name;
            comboProtocolExerciseState2.Text = ((Exercise)lstProtocolExercises.SelectedItem).TargetState.Name;
            comboProtocolExerciseState3.Text = ((Exercise)lstProtocolExercises.SelectedItem).PostState.Name;

            AddProtocolsEvents();
        }
        #endregion


        #region Events

        private void button7_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(clinic.States.ElementAt(0).Name);
        }

        private void lstExercises_BindingContextChanged(object sender, EventArgs e)
        {
           
        }

        private void comboExercise2_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void comboExercise3_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //lstExercises.DataBindings.Clear();
            //txtExerciceDetails1.DataBindings.Clear();
            //txtExerciceDetails2.DataBindings.Clear();
            //txtExerciceDetails3.DataBindings.Clear();
            //txtExerciceDetails4.DataBindings.Clear();
            //lblDump1.DataBindings.Clear();
            //lblDump2.DataBindings.Clear();
            //lblDump3.DataBindings.Clear();
            //comboExerciseState1.DataBindings.Clear();
            //comboExerciseState2.DataBindings.Clear();
            //comboExerciseState3.DataBindings.Clear();
            //SetupExercisesPanel();
        }

        private void btnStateAdd_Click(object sender, EventArgs e)
        {
            RemoveStatesEvents();
            if (blStates.Count == 0)
            {
                txtConditionDetails1.Enabled = true;
                txtConditionDetails2.Enabled = true;
                txtConditionDetails3.Enabled = true;
                txtConditionDetails4.Enabled = true;
            }


            blStates.Add(new State("New state " + (clinic.States.Count + 1)));
            //lstStates.SelectedIndexChanged -= lstStates_SelectedIndexChanged;
            blStates.ResetBindings();
            //lstStates.SelectedIndexChanged += lstStates_SelectedIndexChanged;
            lstStates.SelectedIndex = lstStates.Items.Count - 1;
            lstStates_SelectedIndexChanged(sender, e);
            AddStatesEvents();
        }

        private void btnStateRemove_Click(object sender, EventArgs e)
        {
            RemoveStatesEvents();

            try
            {
                
                blStates.RemoveAt(lstStates.SelectedIndex);
            }
            catch { }
            blStates.ResetBindings();
            if (blStates.Count == 0)
            {
 
                txtConditionDetails1.Text = "";
                txtConditionDetails2.Text = "";
                txtConditionDetails3.Text = "";
                txtConditionDetails4.Text = "";


                txtConditionDetails1.Enabled = false; 
                txtConditionDetails2.Enabled = false; 
                txtConditionDetails3.Enabled = false; 
                txtConditionDetails4.Enabled = false; 
            }
            AddStatesEvents();
        }

        private void comboExerciseState1_Leave(object sender, EventArgs e)
        {
           //  lblDump1.Text = comboExerciseState1.GetItemText(comboExerciseState1.SelectedItem);
            Debug.WriteLine("Changed combo1");
        }

        private void comboExerciseState2_Leave(object sender, EventArgs e)
        {
           //  lblDump2.Text = comboExerciseState2.GetItemText(comboExerciseState2.SelectedItem);
            Debug.WriteLine("Changed combo2");
        }

        private void comboExerciseState3_Leave(object sender, EventArgs e)
        {
           //  lblDump3.Text = comboExerciseState3.GetItemText(comboExerciseState3.SelectedItem);
            Debug.WriteLine("Changed combo3");
        }

        private void btnExerciseAdd_Click(object sender, EventArgs e)
        {
            RemoveExercisesEvents();
            if (blExercises.Count == 0)
            {
                txtExerciseDetails1.Enabled = true;
                txtExerciseDetails2.Enabled = true;
                txtExerciseDetails3.Enabled = true;

                comboExerciseState1.Enabled = true;
                comboExerciseState2.Enabled = true;
                comboExerciseState3.Enabled = true;
            }


            blExercises.Add(new Exercise("New exercise " + (clinic.Exercises.Count + 1)));
            lstExercises.SelectedIndexChanged -= lstExercises_SelectedIndexChanged;
            blExercises.ResetBindings();
            lstExercises.SelectedIndex = lstExercises.Items.Count - 1;
            lstExercises_SelectedIndexChanged(sender, e);
            lstExercises.SelectedIndexChanged += lstExercises_SelectedIndexChanged;
            AddExercisesEvents();
        }

        private void btnExerciseRemove_Click(object sender, EventArgs e)
        {
            RemoveExercisesEvents();

            try
            {
                blExercises.RemoveAt(lstExercises.SelectedIndex);
            }
            catch { }
            blExercises.ResetBindings();
            if (blExercises.Count == 0)
            {
                txtExerciseDetails1.Text = "";
                txtExerciseDetails2.Text = "";
                txtExerciseDetails3.Text = "";

                txtExerciseDetails1.Enabled = false;
                txtExerciseDetails2.Enabled = false;
                txtExerciseDetails3.Enabled = false;

                comboExerciseState1.Text = "";
                comboExerciseState2.Text = "";
                comboExerciseState3.Text = "";

                comboExerciseState1.Enabled = false;
                comboExerciseState2.Enabled = false;
                comboExerciseState3.Enabled = false;
            }

            AddExercisesEvents();
        }

        private void label13_Click(object sender, EventArgs e)
        {
            if (lstStates.SelectedItem != null)
            { 
                blStates.Add(new State((State)lstStates.SelectedItem));
                //statesBinding.ResetBindings(false);

                lstStates.SelectedIndex = lstStates.Items.Count - 1;
                ((State)lstStates.SelectedItem).Name += " - copy";
                //statesBinding.ResetBindings(false);
                blStates.ResetBindings();
            }
        }

        private void lstStates_BindingContextChanged(object sender, EventArgs e)
        {
            //statesBinding.ResetBindings(false);
        }

        private void lstStates_BindingContextChanged_1(object sender, EventArgs e)
        {
        }

        private void label14_Click(object sender, EventArgs e)
        {
            clinic.Exercises.Add(new Exercise((Exercise)lstExercises.SelectedItem));
            //exercisesBinding.ResetBindings(false);

            lstExercises.SelectedIndex = lstExercises.Items.Count - 1;
            ((Exercise)lstExercises.SelectedItem).Name += " - copy";
            //exercisesBinding.ResetBindings(false);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            clinic.Protocols.Add(new Protocol("New protocol " + (clinic.Protocols.Count + 1)));
            //protocolsBinding.ResetBindings(false);
            lstProtocols.SelectedIndex = lstProtocols.Items.Count - 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                clinic.Protocols.RemoveAt(lstProtocols.SelectedIndex);
            }
            catch { }
           // protocolsBinding.ResetBindings(false);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ((Protocol)lstProtocols.SelectedItem).Exercises.Add((Exercise)lstAvailableExercises.SelectedItem);
            //protocolsBinding.ResetBindings(false);
            lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                clinic.Protocols.ElementAt(lstProtocols.SelectedIndex).Exercises.RemoveAt(lstProtocolExercises.SelectedIndex);
            }
            catch { }
           // protocolsBinding.ResetBindings(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(clinic.ToString());
        }

        private void btnProtocolExercisesUP_Click(object sender, EventArgs e)
        {
            
        }

        private void lstStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstStates.Items.Count !=0)
            { 
                if (btnStatesApply.Enabled)
                {
                    var result = MessageBox.Show
                            ("If not applied previous changes to 'States' will be lost!", "Want to apply?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        btnStatesApply_Click(sender, e);
                    else btnStatesApply.Enabled = false;
                        
                }
           

                btnStatesApply.Enabled = false;
                if (blStates.Count >0)
                { 
                    txtConditionDetails1.Text = ((State)lstStates.SelectedItem).Name;
                    txtConditionDetails2.Text = ((State)lstStates.SelectedItem).HOHCode;
                    txtConditionDetails3.Text = ((State)lstStates.SelectedItem).UserMsg;
                    txtConditionDetails4.Text = ((State)lstStates.SelectedItem).CallbackMsg;
                }
                StatePanelLstStates_Selected = lstStates.SelectedIndex;
           }
        }

        private void btnStatesApply_Click(object sender, EventArgs e)
        {
            State s = (State)lstStates.Items[StatePanelLstStates_Selected];
            
            s.Name = txtConditionDetails1.Text;
            s.HOHCode = txtConditionDetails2.Text;
            s.UserMsg = txtConditionDetails3.Text;
            s.CallbackMsg = txtConditionDetails4.Text;
            blStates.ResetBindings();
            btnStatesApply.Enabled = false;
            btnExercisesApply.Enabled = false;

        }

        private void txtConditionDetails1_TextChanged(object sender, EventArgs e)
        {
                if (!btnStatesApply.Enabled)
                    btnStatesApply.Enabled = txtConditionDetails1.Text != ((State)lstStates.SelectedItem).Name;
        }

        private void txtConditionDetails2_TextChanged(object sender, EventArgs e)
        {
            if (!btnStatesApply.Enabled)
                btnStatesApply.Enabled = txtConditionDetails2.Text != ((State)lstStates.SelectedItem).HOHCode;
        }

        private void txtConditionDetails3_TextChanged(object sender, EventArgs e)
        {
            if (!btnStatesApply.Enabled)
                btnStatesApply.Enabled = txtConditionDetails3.Text != ((State)lstStates.SelectedItem).UserMsg;
        }

        private void txtConditionDetails4_TextChanged(object sender, EventArgs e)
        {
            if (!btnStatesApply.Enabled)
                btnStatesApply.Enabled = txtConditionDetails4.Text != ((State)lstStates.SelectedItem).CallbackMsg;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void btnExercisesApply_Click(object sender, EventArgs e)
        {
            Exercise s = (Exercise)lstExercises.Items[ExercisesPanelLstExercises_Selected];

            s.Name = txtExerciseDetails1.Text;
            s.UserMsg = txtExerciseDetails3.Text;
            s.SFCode = txtExerciseDetails2.Text;

            s.PreState = (State)comboExerciseState1.SelectedItem;
            s.TargetState = (State)comboExerciseState2.SelectedItem;
            s.PostState = (State)comboExerciseState3.SelectedItem;

            blExercises.ResetBindings();

            //lstStates.DataSource = null;
            //lstStates.DataSource = blState;
            //lstStates.DisplayMember = "Name";

            btnExercisesApply.Enabled = false;
            

        }

        private void txtExerciseDetails1_TextChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled)
                btnExercisesApply.Enabled = (txtExerciseDetails1.Text != ((Exercise)lstExercises.SelectedItem).Name);
            Debug.WriteLine("txt1:" + (txtExerciseDetails1.Text != ((Exercise)lstExercises.SelectedItem).UserMsg));
        }

        private void txtExerciseDetails3_TextChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled)
                btnExercisesApply.Enabled = (txtExerciseDetails3.Text != ((Exercise)lstExercises.SelectedItem).UserMsg);
            Debug.WriteLine("txt3:" + (txtExerciseDetails3.Text != ((Exercise)lstExercises.SelectedItem).UserMsg));       
        }

        private void txtExerciseDetails2_TextChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled)
                btnExercisesApply.Enabled = (txtExerciseDetails2.Text != ((Exercise)lstExercises.SelectedItem).SFCode);
            Debug.WriteLine("txt2:" + (txtExerciseDetails2.Text != ((Exercise)lstExercises.SelectedItem).SFCode));
        }

        private void comboExerciseState1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled)
                btnExercisesApply.Enabled = (comboExerciseState1.SelectedItem != ((Exercise)lstExercises.SelectedItem).PreState);
            Debug.WriteLine("cmb1:" + (comboExerciseState1.SelectedItem != ((Exercise)lstExercises.SelectedItem).PreState));
        }

        private void comboExerciseState2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled)
                btnExercisesApply.Enabled = (comboExerciseState2.SelectedItem != ((Exercise)lstExercises.SelectedItem).TargetState);
            Debug.WriteLine("cmb2:" + (comboExerciseState2.SelectedItem != ((Exercise)lstExercises.SelectedItem).TargetState));
        }

        private void comboExerciseState3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled)
                btnExercisesApply.Enabled = (comboExerciseState3.SelectedItem != ((Exercise)lstExercises.SelectedItem).PostState);
            Debug.WriteLine("cmb3:" + (comboExerciseState3.SelectedItem != ((Exercise)lstExercises.SelectedItem).PostState));
        }

        private void lstExercises_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstExercises.Items.Count != 0)
            {
                if (btnExercisesApply.Enabled)
                {
                    var result = MessageBox.Show
                            ("If not applied previous changes to 'Exercises' will be lost!", "Want to apply?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        btnExercisesApply_Click(sender, e);
                    else
                        btnExercisesApply.Enabled = false;
                }


                btnExercisesApply.Enabled = false;
                if (blExercises.Count > 0)
                {
                    txtExerciseDetails1.Text = ((Exercise)lstExercises.SelectedItem).Name;
                    txtExerciseDetails2.Text = ((Exercise)lstExercises.SelectedItem).SFCode;
                    txtExerciseDetails3.Text = ((Exercise)lstExercises.SelectedItem).UserMsg;

                    if (((Exercise)lstExercises.SelectedItem).PreState != null)
                        comboExerciseState1.Text = ((Exercise)lstExercises.SelectedItem).PreState.Name;
                    else
                    {
                        comboExerciseState1.SelectedIndex = 0;
                        ((Exercise)lstExercises.SelectedItem).PreState = (State)comboExerciseState1.SelectedItem;
                    }


                    if (((Exercise)lstExercises.SelectedItem).TargetState != null)
                        comboExerciseState2.Text = ((Exercise)lstExercises.SelectedItem).TargetState.Name;
                    else
                    {
                        comboExerciseState2.SelectedIndex = 0;
                        ((Exercise)lstExercises.SelectedItem).TargetState = (State)comboExerciseState2.SelectedItem;
                    }

                    if (((Exercise)lstExercises.SelectedItem).PostState != null)
                        comboExerciseState3.Text = ((Exercise)lstExercises.SelectedItem).PostState.Name;
                    else
                    {
                        comboExerciseState3.SelectedIndex = 0;
                        ((Exercise)lstExercises.SelectedItem).PostState = (State)comboExerciseState3.SelectedItem;
                    }
                }
                ExercisesPanelLstExercises_Selected = lstExercises.SelectedIndex;
            }

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            lstStates_SelectedIndexChanged(sender, e);
            lstExercises_SelectedIndexChanged(sender, e);
        }

        private void RemoveExercisesEvents()
        {
            lstExercises.SelectedIndexChanged -= lstExercises_SelectedIndexChanged;
            txtExerciseDetails1.TextChanged -= txtExerciseDetails1_TextChanged;
            txtExerciseDetails2.TextChanged -= txtExerciseDetails2_TextChanged;
            txtExerciseDetails3.TextChanged -= txtExerciseDetails3_TextChanged;
            comboExerciseState1.SelectedIndexChanged -= comboExerciseState1_SelectedIndexChanged;
            comboExerciseState2.SelectedIndexChanged -= comboExerciseState2_SelectedIndexChanged;
            comboExerciseState3.SelectedIndexChanged -= comboExerciseState3_SelectedIndexChanged;
        }

        private void AddExercisesEvents()
        {
            lstExercises.SelectedIndexChanged += lstExercises_SelectedIndexChanged;
            txtExerciseDetails1.TextChanged += txtExerciseDetails1_TextChanged;
            txtExerciseDetails2.TextChanged += txtExerciseDetails2_TextChanged;
            txtExerciseDetails3.TextChanged += txtExerciseDetails3_TextChanged;
            comboExerciseState1.SelectedIndexChanged += comboExerciseState1_SelectedIndexChanged;
            comboExerciseState2.SelectedIndexChanged += comboExerciseState2_SelectedIndexChanged;
            comboExerciseState3.SelectedIndexChanged += comboExerciseState3_SelectedIndexChanged;
        }

        private void RemoveStatesEvents()
        {
            lstStates.SelectedIndexChanged -= lstStates_SelectedIndexChanged;
            txtConditionDetails1.TextChanged -= txtConditionDetails1_TextChanged;
            txtConditionDetails2.TextChanged -= txtConditionDetails2_TextChanged;
            txtConditionDetails3.TextChanged -= txtConditionDetails3_TextChanged;
            txtConditionDetails4.TextChanged -= txtConditionDetails4_TextChanged;
        }

        private void AddStatesEvents()
        {
            lstStates.SelectedIndexChanged += lstStates_SelectedIndexChanged;
            txtConditionDetails1.TextChanged += txtConditionDetails1_TextChanged;
            txtConditionDetails2.TextChanged += txtConditionDetails2_TextChanged;
            txtConditionDetails3.TextChanged += txtConditionDetails3_TextChanged;
            txtConditionDetails4.TextChanged += txtConditionDetails4_TextChanged;

        }

        private void RemoveProtocolsEvents()
        {
            lstProtocols.SelectedIndexChanged -= lstProtocols_SelectedIndexChanged;
            lstProtocolExercises.SelectedIndexChanged -= lstProtocolExercises_SelectedIndexChanged;
            lstAvailableExercises.SelectedIndexChanged -= lstAvailableExercises_SelectedIndexChanged;
            txtProtocolExerciseDetails4.TextChanged -= txtConditionDetails1_TextChanged;
            txtProtocolExerciseDetails5.TextChanged -= txtConditionDetails2_TextChanged;
        }

        private void AddProtocolsEvents()
        {
            lstProtocols.SelectedIndexChanged += lstProtocols_SelectedIndexChanged;
            lstProtocolExercises.SelectedIndexChanged += lstProtocolExercises_SelectedIndexChanged;
            lstAvailableExercises.SelectedIndexChanged += lstAvailableExercises_SelectedIndexChanged;
            txtProtocolExerciseDetails4.TextChanged += txtConditionDetails1_TextChanged;
            txtProtocolExerciseDetails5.TextChanged += txtConditionDetails2_TextChanged;
        }

        private void lstProtocols_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();
            if (lstProtocols.Items.Count != 0)
            {
                if (btnProtocolsApply.Enabled)
                {
                    var result = MessageBox.Show
                            ("If not applied previous changes to 'Protocols' will be lost!", "Want to apply?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        btnProtocolsApply_Click(sender, e);
                    else
                        btnProtocolsApply.Enabled = false;
                }


                btnProtocolsApply.Enabled = false;
                if (blProtocols.Count > 0)
                {
                    RemoveProtocolsEvents();
                    lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
                    lstProtocolExercises.DisplayMember = "Name";
                    if (lstProtocolExercises.Items.Count > 0) lstProtocolExercises.SelectedIndex = 0;
                    AddProtocolsEvents();
                    lstProtocolExercises_SelectedIndexChanged(sender, e);
                }
                ProtocolPanelLstProtocols_Selected = lstProtocols.SelectedIndex;
            }
            AddProtocolsEvents();
        }

        private void lstProtocolExercises_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();
            if (lstProtocolExercises.Items.Count > 0)
            {
                txtProtocolExerciseDetails1.Text = ((Exercise)lstProtocolExercises.SelectedItem).Name;
                txtProtocolExerciseDetails2.Text = ((Exercise)lstProtocolExercises.SelectedItem).UserMsg;
                txtProtocolExerciseDetails3.Text = ((Exercise)lstProtocolExercises.SelectedItem).SFCode;
                txtProtocolExerciseDetails4.Text = ((Exercise)lstProtocolExercises.SelectedItem).ExerciseTime.ToString();
                txtProtocolExerciseDetails5.Text = ((Exercise)lstProtocolExercises.SelectedItem).Repetitions.ToString();

                if (((Exercise)lstProtocolExercises.SelectedItem).PreState != null)
                    comboProtocolExerciseState1.Text = ((Exercise)lstProtocolExercises.SelectedItem).PreState.Name;
                else
                {
                    comboProtocolExerciseState1.SelectedIndex = 0;
                    ((Exercise)lstProtocolExercises.SelectedItem).PreState = (State)comboProtocolExerciseState1.SelectedItem;
                }


                if (((Exercise)lstProtocolExercises.SelectedItem).TargetState != null)
                    comboProtocolExerciseState2.Text = ((Exercise)lstProtocolExercises.SelectedItem).TargetState.Name;
                else
                {
                    comboProtocolExerciseState2.SelectedIndex = 0;
                    ((Exercise)lstProtocolExercises.SelectedItem).TargetState = (State)comboProtocolExerciseState2.SelectedItem;
                }

                if (((Exercise)lstProtocolExercises.SelectedItem).PostState != null)
                    comboProtocolExerciseState3.Text = ((Exercise)lstProtocolExercises.SelectedItem).PostState.Name;
                else
                {
                    comboProtocolExerciseState3.SelectedIndex = 0;
                    ((Exercise)lstProtocolExercises.SelectedItem).PostState = (State)comboProtocolExerciseState3.SelectedItem;
                }
            }
            ProtocolPanelLstProtocolExercises_Selected = lstProtocolExercises.SelectedIndex;
            AddProtocolsEvents();
         }

        private void lstAvailableExercises_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnProtocolsApply_Click(object sender, EventArgs e)
        {
            Exercise s = (Exercise)lstExercises.Items[ProtocolPanelLstProtocolExercises_Selected];
            s.ExerciseTime = Convert.ToInt16(txtProtocolExerciseDetails4.Text);
            s.Repetitions = Convert.ToInt16(txtProtocolExerciseDetails5.Text);

            ((Protocol)lstProtocols.Items[ProtocolPanelLstProtocols_Selected]).Exercises[ProtocolPanelLstProtocolExercises_Selected] = s;

            blExercises.ResetBindings();
            blProtocols.ResetBindings();

            //lstProtocolExercises.DataSource = null;
            //lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
            //lstProtocolExercises.DisplayMember = "Name";
            //lstProtocols_SelectedIndexChanged(sender, e);
            //lstProtocolExercises_SelectedIndexChanged(sender, e);
            //lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
            
            btnProtocolsApply.Enabled = false;
        }

        private void btnAddProtocol_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();
            if (blProtocols.Count == 0)
            {
                //txtExerciseDetails1.Enabled = true;
                //txtExerciseDetails2.Enabled = true;
                //txtExerciseDetails3.Enabled = true;

                //comboExerciseState1.Enabled = true;
                //comboExerciseState2.Enabled = true;
                //comboExerciseState3.Enabled = true;
            }


            blProtocols.Add(new Protocol("New Protocol " + (clinic.Protocols.Count + 1)));
            lstProtocols.SelectedIndexChanged -= lstProtocols_SelectedIndexChanged;
            blProtocols.ResetBindings();
            lstProtocols.SelectedIndex = lstProtocols.Items.Count - 1;
            lstProtocols_SelectedIndexChanged(sender, e);
            lstProtocols.SelectedIndexChanged += lstProtocols_SelectedIndexChanged;
            AddProtocolsEvents();

        }

        private void btnProtocolDelete_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();

            try
            {
                blProtocols.RemoveAt(lstProtocols.SelectedIndex);
            }
            catch { }
            blProtocols.ResetBindings();
            if (blProtocols.Count == 0)
            {
                //txtExerciseDetails1.Text = "";
                //txtExerciseDetails2.Text = "";
                //txtExerciseDetails3.Text = "";

                //txtExerciseDetails1.Enabled = false;
                //txtExerciseDetails2.Enabled = false;
                //txtExerciseDetails3.Enabled = false;

                //comboExerciseState1.Text = "";
                //comboExerciseState2.Text = "";
                //comboExerciseState3.Text = "";

                //comboExerciseState1.Enabled = false;
                //comboExerciseState2.Enabled = false;
                //comboExerciseState3.Enabled = false;
            }

            AddProtocolsEvents();

        }

        private void btnProtocolExercisesAdd_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();
            if (lstProtocolExercises.Items.Count == 0)
            {
                //txtExerciseDetails1.Enabled = true;
                //txtExerciseDetails2.Enabled = true;
                //txtExerciseDetails3.Enabled = true;

                //comboExerciseState1.Enabled = true;
                //comboExerciseState2.Enabled = true;
                //comboExerciseState3.Enabled = true;
            }


            //lstProtocolExercises.Items.Add(lstAvailableExercises.SelectedItem);
            ((Protocol)lstProtocols.SelectedItem).Exercises.Add((Exercise)lstAvailableExercises.SelectedItem);
            lstProtocolExercises.SelectedIndexChanged -= lstProtocolExercises_SelectedIndexChanged;

            blProtocols.ResetBindings();

            lstProtocolExercises.DataSource = null;
            lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
            lstProtocolExercises.DisplayMember = "Name";


//            lstProtocols_SelectedIndexChanged(sender, e);
          //  lstProtocolExercises_SelectedIndexChanged(sender, e);
            lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
            lstProtocolExercises_SelectedIndexChanged(sender, e);
            lstProtocolExercises.SelectedIndexChanged += lstProtocolExercises_SelectedIndexChanged;

            AddProtocolsEvents();

        }

        private void btnProtocolExercisesRemove_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();

            try
            {
                blProtocols[lstProtocols.SelectedIndex].Exercises.RemoveAt(lstProtocolExercises.SelectedIndex);
                //blProtocols.RemoveAt(lstProtocols.SelectedIndex);
            }
            catch { }
            blProtocols.ResetBindings();
            if (blProtocols.Count == 0)
            {
                //txtExerciseDetails1.Text = "";
                //txtExerciseDetails2.Text = "";
                //txtExerciseDetails3.Text = "";

                //txtExerciseDetails1.Enabled = false;
                //txtExerciseDetails2.Enabled = false;
                //txtExerciseDetails3.Enabled = false;

                //comboExerciseState1.Text = "";
                //comboExerciseState2.Text = "";
                //comboExerciseState3.Text = "";

                //comboExerciseState1.Enabled = false;
                //comboExerciseState2.Enabled = false;
                //comboExerciseState3.Enabled = false;
            }
            lstProtocolExercises.SelectedIndexChanged -= lstProtocolExercises_SelectedIndexChanged;

            blProtocols.ResetBindings();

            lstProtocolExercises.DataSource = null;
            lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
            lstProtocolExercises.DisplayMember = "Name";


            //            lstProtocols_SelectedIndexChanged(sender, e);
            //  lstProtocolExercises_SelectedIndexChanged(sender, e);
            lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
            lstProtocolExercises_SelectedIndexChanged(sender, e);
            lstProtocolExercises.SelectedIndexChanged += lstProtocolExercises_SelectedIndexChanged;

            AddProtocolsEvents();

        }

        private void txtProtocolExerciseDetails4_TextChanged(object sender, EventArgs e)
        {
            if (!btnProtocolsApply.Enabled)
                btnProtocolsApply.Enabled = (txtProtocolExerciseDetails4.Text != ((Exercise)lstProtocolExercises.SelectedItem).Name);
        }

        private void txtProtocolExerciseDetails5_TextChanged(object sender, EventArgs e)
        {
            if (!btnProtocolsApply.Enabled)
                btnProtocolsApply.Enabled = (txtProtocolExerciseDetails5.Text != ((Exercise)lstProtocolExercises.SelectedItem).Name);

        }
    }
    #endregion
}
