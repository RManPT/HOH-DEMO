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
        BindingSource conditionsBinding = new BindingSource();
        BindingSource conditionsDetailsBinding = new BindingSource();
        BindingSource exercisesBinding = new BindingSource();
        BindingSource exercisesDetailsBinding = new BindingSource();

        public ProtocolEditor(Clinic clinic)
        {
            InitializeComponent();
            this.clinic = clinic;
            

         
            conditionsBinding.DataSource = clinic.Conditions;
            conditionsDetailsBinding.DataSource = conditionsBinding;

            exercisesBinding.DataSource = clinic.Exercises;
            exercisesDetailsBinding.DataSource = exercisesBinding;
            
            

            lstConditions.DataSource = conditionsDetailsBinding;
            lstConditions.DisplayMember = "Name";
            
            txtConditionDetails1.DataBindings.Add("Text", conditionsDetailsBinding, "Name");
            txtConditionDetails2.DataBindings.Add("Text", conditionsDetailsBinding, "HOHCode");
            txtConditionDetails3.DataBindings.Add("Text", conditionsDetailsBinding, "UserMsg");
            txtConditionDetails4.DataBindings.Add("Text", conditionsDetailsBinding, "CallbackMsg");

            lstExercises.DataSource = exercisesDetailsBinding;
            lstExercises.DisplayMember = "Name";

            
            txtExerciceDetails1.DataBindings.Add("Text", exercisesDetailsBinding, "Name");
            txtExerciceDetails2.DataBindings.Add("Text", exercisesDetailsBinding, "SFCode");
            txtExerciceDetails3.DataBindings.Add("Text", exercisesDetailsBinding, "UserMsg");
            txtExerciceDetails4.DataBindings.Add("Text", exercisesDetailsBinding, "ExerciseTime");

            comboExercise1.DataSource = conditionsBinding;
            //comboExercise1.DataBindings.Add("Name", exercisesDetailsBinding, "PreCondition");
            comboExercise1.DisplayMember = "Name";
            
        }

        

        private void Testcreation_Load(object sender, EventArgs e)
        {

        }


       
    

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(clinic.Conditions.ElementAt(0).Name);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            clinic.Conditions.Add(new State());
            conditionsBinding.ResetBindings(false);
            //  conditionsDetailsBinding.ResetBindings(false);
            lstConditions.SelectedIndex = lstConditions.Items.Count - 1;

        }



        private void txtConditionDetails1_Leave(object sender, EventArgs e)
        {
            conditionsBinding.ResetBindings(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clinic.Conditions.RemoveAt(lstConditions.SelectedIndex);
            conditionsBinding.ResetBindings(false);

        }
    }
}
