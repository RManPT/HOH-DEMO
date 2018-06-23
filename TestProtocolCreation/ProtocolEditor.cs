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
        BindingSource statesBinding = new BindingSource();
        BindingSource statesDetailsBinding = new BindingSource();
        BindingSource exercisesBinding = new BindingSource();
        BindingSource exercisesDetailsBinding = new BindingSource();
        BindingSource protocolsBinding = new BindingSource();
        BindingSource protocolsDetailsBinding = new BindingSource();



        public ProtocolEditor(Clinic clinic)
        {
            InitializeComponent();
            this.clinic = clinic;

            ///State
            SetupStatePanel();

            ///exercises
            SetupExercisesPanel();

            ///protocols
            SetupProtocolsPanel();
        }


        #region Custom
        private void SetupStatePanel()
        {
            statesBinding.DataSource = clinic.State;
            statesDetailsBinding.DataSource = statesBinding;


            lstStates.DataSource = statesDetailsBinding;
            lstStates.DisplayMember = "Name";

            txtConditionDetails1.DataBindings.Add("Text", statesDetailsBinding, "Name");
            txtConditionDetails2.DataBindings.Add("Text", statesDetailsBinding, "HOHCode");
            txtConditionDetails3.DataBindings.Add("Text", statesDetailsBinding, "UserMsg");
            txtConditionDetails4.DataBindings.Add("Text", statesDetailsBinding, "CallbackMsg");
        }

        private void SetupExercisesPanel()
        {
            exercisesBinding.DataSource = clinic.Exercises;
            exercisesDetailsBinding.DataSource = exercisesBinding;

            lstExercises.DataSource = exercisesDetailsBinding;
            lstExercises.DisplayMember = "Name";


            txtExerciceDetails1.DataBindings.Add("Text", exercisesDetailsBinding, "Name");
            txtExerciceDetails2.DataBindings.Add("Text", exercisesDetailsBinding, "SFCode");
            txtExerciceDetails3.DataBindings.Add("Text", exercisesDetailsBinding, "UserMsg");
            txtExerciceDetails4.DataBindings.Add("Text", exercisesDetailsBinding, "ExerciseTime");
            txtExerciceDetails5.DataBindings.Add("Text", exercisesDetailsBinding, "Repetitions");

            comboExerciseState1.BindingContext = new BindingContext();
            comboExerciseState1.DataSource = statesBinding;
            comboExerciseState1.DisplayMember = "Name";


            comboExerciseState2.BindingContext = new BindingContext();
            comboExerciseState2.DataSource = statesBinding;
            comboExerciseState2.DisplayMember = "Name";

            comboExerciseState3.BindingContext = new BindingContext();
            comboExerciseState3.DataSource = statesBinding;
            comboExerciseState3.DisplayMember = "Name";

            lblDump1.DataBindings.Add("Text", exercisesDetailsBinding, "PreState.Name");
            lblDump2.DataBindings.Add("Text", exercisesDetailsBinding, "TargetState.Name");
            lblDump3.DataBindings.Add("Text", exercisesDetailsBinding, "PostState.Name");
            //calls events context changed and selectedIndexChange
        }


        private void SetupProtocolsPanel()
        {
            protocolsBinding.DataSource = clinic.Protocols;
            protocolsDetailsBinding.DataSource = protocolsBinding;

            lstProtocols.DataSource = protocolsDetailsBinding;
            lstProtocols.DisplayMember = "Name";
        }
        #endregion


        #region Events

        private void button7_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(clinic.State.ElementAt(0).Name);
        }

        private void lstExercises_BindingContextChanged(object sender, EventArgs e)
        {
           
            Debug.WriteLine(lblDump1.Text);
            Debug.WriteLine(comboExerciseState1.FindString(lblDump1.Text));
        }

        private void lstExercises_SelectedIndexChanged(object sender, EventArgs e)
        {
           // lstExercises_BindingContextChanged(sender, e);
            comboExerciseState1.SelectedIndex = comboExerciseState1.FindString(lblDump1.Text);
            //comboExerciseState2.SelectedIndex = comboExerciseState2.FindString(lblDump2.Text);
            //comboExerciseState3.SelectedIndex = comboExerciseState3.FindString(lblDump3.Text);
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
            clinic.State.Add(new State("New state " + (clinic.State.Count + 1)));
            statesBinding.ResetBindings(false);
            lstStates.SelectedIndex = lstStates.Items.Count - 1;
        }

        private void btnStateRemove_Click(object sender, EventArgs e)
        {
            try
            {
                clinic.State.RemoveAt(lstStates.SelectedIndex);
            }
            catch { }
            statesBinding.ResetBindings(false);
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
    }
    #endregion
}
