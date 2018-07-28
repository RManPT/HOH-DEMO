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
        private HOHEvent HOHEventObj = new HOHEvent();
        private Clinic backup_clinic;

        public ProtocolEditor(Clinic clinic)
        {
            InitializeComponent();

            lstProtocolExercises.DrawItem += new DrawItemEventHandler(listBox_DrawItem);
            


            btnProtocolExercisesDown.Text = char.ConvertFromUtf32(0x2193);
            btnProtocolExercisesUP.Text = char.ConvertFromUtf32(0x2191);

            this.clinic = clinic;
            this.backup_clinic = new Clinic();
            this.backup_clinic.FromJSON(clinic.ToJSON());
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

        void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < lstProtocolExercises.Items.Count)
            {
                Graphics g = e.Graphics;

                bool itemExists = true;

                foreach (var ex in blExercises)
                {
                    if (ex.Name != ((Exercise)lstProtocolExercises.Items[itemIndex]).Name)
                    {
                        itemExists = false;
                    }
                }

                // Background Color
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.LightBlue : Color.White);
                g.FillRectangle(backgroundColorBrush, e.Bounds);

                // Set text color
                string itemText = ((Exercise)lstProtocolExercises.Items[itemIndex]).Name;

                SolidBrush itemTextColorBrush;
                if (itemExists)
                    itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
                else
                    itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.Red) : new SolidBrush(Color.Red);

                g.DrawString(itemText, e.Font, itemTextColorBrush, lstProtocolExercises.GetItemRectangle(itemIndex).Location);

                // Clean up
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }
    

        private void SetupStatePanel()
        {

            RemoveStatesEvents();

          //  lstStates.DataSource = null;
            lstStates.DataSource = blStates;
            lstStates.DisplayMember = "Name";

            if (blStates.Count!=0)
            {
                tabPage2.Enabled = true;
                btnStateRemove.Enabled = true;
                txtConditionDetails1.Text = ((State)lstStates.SelectedItem).Name;
                txtConditionDetails2.Text = ((State)lstStates.SelectedItem).HOHCode;
                txtConditionDetails3.Text = ((State)lstStates.SelectedItem).UserMsg;
                txtConditionDetails4.Text = ((State)lstStates.SelectedItem).CallbackMsg;

                txtConditionDetails1.Enabled = true;
                txtConditionDetails2.Enabled = true;
                txtConditionDetails3.Enabled = true;
                txtConditionDetails4.Enabled = true;

              
            } else
            {
                tabPage2.Enabled = false;
                tabPage3.Enabled = false;
            }
            AddStatesEvents();
        }

        private void SetupExercisesPanel()
        {
            RemoveExercisesEvents();

           // lstExercises.DataSource = null;
            lstExercises.DataSource = blExercises;
            lstExercises.DisplayMember = "Name";

            comboExerciseState1.BindingContext = new BindingContext();
            comboExerciseState1.DataSource = blStates;
            comboExerciseState1.DisplayMember = "Name";
            comboExerciseState2.BindingContext = new BindingContext();
            comboExerciseState2.DataSource = blStates;
            comboExerciseState2.DisplayMember = "Name";
            comboExerciseState3.BindingContext = new BindingContext();
            comboExerciseState3.DataSource = blStates;
            comboExerciseState3.DisplayMember = "Name";


            if (blStates.Count != 0 && blExercises.Count != 0)
            {
                tabPage3.Enabled = true;
                btnExerciseRemove.Enabled = true;
                txtExerciseDetails1.Text = ((Exercise)lstExercises.SelectedItem).Name;
                txtExerciseDetails2.Text = ((Exercise)lstExercises.SelectedItem).SFCode;
                txtExerciseDetails3.Text = ((Exercise)lstExercises.SelectedItem).UserMsg;

 
                comboExerciseState1.Text = ((Exercise)lstExercises.SelectedItem).PreState.Name;
                if (!blStates.Contains(((Exercise)lstExercises.SelectedItem).PreState)) comboExerciseState1.ForeColor = Color.Red; else comboExerciseState1.ForeColor = Color.Black;
                comboExerciseState2.Text = ((Exercise)lstExercises.SelectedItem).TargetState.Name;
                if (!blStates.Contains(((Exercise)lstExercises.SelectedItem).TargetState)) comboExerciseState2.ForeColor = Color.Red; else comboExerciseState2.ForeColor = Color.Black;
                comboExerciseState3.Text = ((Exercise)lstExercises.SelectedItem).PostState.Name;
                if (!blStates.Contains(((Exercise)lstExercises.SelectedItem).PostState)) comboExerciseState3.ForeColor = Color.Red; else comboExerciseState3.ForeColor = Color.Black;




                txtExerciseDetails1.Enabled = true;
                txtExerciseDetails2.Enabled = true;
                txtExerciseDetails3.Enabled = true;

                comboExerciseState1.Enabled = true;
                comboExerciseState2.Enabled = true;
                comboExerciseState3.Enabled = true;

               // AddExercisesEvents();
            }
            else
            {
                tabPage3.Enabled = false;
            }
             AddExercisesEvents();

        }


        private void SetupProtocolsPanel()
        {
            RemoveProtocolsEvents();

         //   lstProtocols.DataSource = null;
            lstProtocols.DataSource = blProtocols;
            lstProtocols.DisplayMember = "Name";

            comboProtocolExerciseState1.BindingContext = new BindingContext();
            comboProtocolExerciseState1.DataSource = null;
            comboProtocolExerciseState1.DataSource = blStates;
            comboProtocolExerciseState1.DisplayMember = "Name";
            comboProtocolExerciseState2.BindingContext = new BindingContext();
            comboProtocolExerciseState2.DataSource = null;
            comboProtocolExerciseState2.DataSource = blStates;
            comboProtocolExerciseState2.DisplayMember = "Name";
            comboProtocolExerciseState3.BindingContext = new BindingContext();
            comboProtocolExerciseState3.DataSource = null;
            comboProtocolExerciseState3.DataSource = blStates;
            comboProtocolExerciseState3.DisplayMember = "Name";


            if (blStates.Count != 0 && blExercises.Count != 0)
            {
                lstAvailableExercises.DataSource = null;
                lstAvailableExercises.DataSource = blExercises;
                lstAvailableExercises.DisplayMember = "Name";
                if (lstAvailableExercises.Items.Count != 0) lstAvailableExercises.SelectedIndex = 0;
            }

            if (blStates.Count != 0 && blExercises.Count != 0 && blProtocols.Count != 0)
            {
                btnProtocolDelete.Enabled = true;
                lstProtocolExercises.DataSource = null;
                lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
                lstProtocolExercises.DisplayMember = "Name";
                if (lstProtocolExercises.Items.Count != 0)
                {
                    lstProtocolExercises.SelectedIndex = 0;

                    //for (int i = 0; i < lstProtocolExercises.Items.Count; i++)
                    //{
                    //    if (!blExercises.Contains((Exercise)lstProtocolExercises.Items[i]) (ListItem)lstProtocolExercises.Items[i]
                    //}
                }



                if ((((Protocol)lstProtocols.SelectedItem).Exercises) != null)
                if (lstProtocolExercises.Items.Count !=0)
                {
                    txtProtocolExerciseDetails1.Text = ((Exercise)lstProtocolExercises.SelectedItem).Name;
                    txtProtocolExerciseDetails2.Text = ((Exercise)lstProtocolExercises.SelectedItem).UserMsg;
                    txtProtocolExerciseDetails3.Text = ((Exercise)lstProtocolExercises.SelectedItem).SFCode;
                    txtProtocolExerciseDetails4.Text = ((Exercise)lstProtocolExercises.SelectedItem).ExerciseTime.ToString();
                    txtProtocolExerciseDetails5.Text = ((Exercise)lstProtocolExercises.SelectedItem).Repetitions.ToString();



                    comboProtocolExerciseState1.Text = ((Exercise)lstProtocolExercises.SelectedItem).PreState.Name;
                    comboProtocolExerciseState2.Text = ((Exercise)lstProtocolExercises.SelectedItem).TargetState.Name;
                    comboProtocolExerciseState3.Text = ((Exercise)lstProtocolExercises.SelectedItem).PostState.Name;

                    txtProtocolName.Enabled = true;
                    txtProtocolExerciseDetails4.Enabled = true;
                    txtProtocolExerciseDetails5.Enabled = true;
                }
                
            } else
            {
                
            }
            AddProtocolsEvents();
        }
        #endregion


        #region Events

  
        private void lstExercises_BindingContextChanged(object sender, EventArgs e)
        {
           
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
            blStates.ResetBindings();
            lstStates.SelectedIndex = lstStates.Items.Count - 1;
            lstStates_SelectedIndexChanged(sender, e);

            btnStateRemove.Enabled = Convert.ToBoolean(blStates.Count+1);
            lblStatesDuplicate.Enabled = Convert.ToBoolean(blStates.Count);
            tabPage2.Enabled = true;

            AddStatesEvents();
        }

        private void btnStateRemove_Click(object sender, EventArgs e)
        {
            RemoveStatesEvents();

            try
            {
                blStates.RemoveAt(lstStates.SelectedIndex);
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
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

            btnStateRemove.Enabled = Convert.ToBoolean(blStates.Count);
            lblStatesDuplicate.Enabled = Convert.ToBoolean(blStates.Count);
           

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
            blExercises.ResetBindings();

            lstExercises.SelectedIndex = lstExercises.Items.Count - 1;
            lstExercises_SelectedIndexChanged(sender, e);
            
            btnExerciseRemove.Enabled = Convert.ToBoolean(blExercises.Count);
            lblExercisesDuplicate.Enabled = Convert.ToBoolean(blExercises.Count);
            txtExerciseDetails1.Enabled = Convert.ToBoolean(blExercises.Count);
            txtExerciseDetails2.Enabled = Convert.ToBoolean(blExercises.Count);
            txtExerciseDetails3.Enabled = Convert.ToBoolean(blExercises.Count);
            comboExerciseState1.Enabled = Convert.ToBoolean(blExercises.Count);
            comboExerciseState2.Enabled = Convert.ToBoolean(blExercises.Count);
            comboExerciseState3.Enabled = Convert.ToBoolean(blExercises.Count);
            tabPage3.Enabled = true;

            AddExercisesEvents();
        }

        private void btnExerciseRemove_Click(object sender, EventArgs e)
        {
            RemoveExercisesEvents();

            try
            {
                blExercises.RemoveAt(lstExercises.SelectedIndex);
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
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

            btnExerciseRemove.Enabled = Convert.ToBoolean(blExercises.Count);
            lblExercisesDuplicate.Enabled = Convert.ToBoolean(blExercises.Count);
            txtExerciseDetails1.Enabled = Convert.ToBoolean(blExercises.Count);
            txtExerciseDetails2.Enabled = Convert.ToBoolean(blExercises.Count);
            txtExerciseDetails3.Enabled = Convert.ToBoolean(blExercises.Count);
            comboExerciseState1.Enabled = Convert.ToBoolean(blExercises.Count);
            comboExerciseState2.Enabled = Convert.ToBoolean(blExercises.Count);
            comboExerciseState3.Enabled = Convert.ToBoolean(blExercises.Count);

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
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
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
                clinic.Protocols[lstProtocols.SelectedIndex].Exercises.RemoveAt(lstProtocolExercises.SelectedIndex);
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
            // protocolsBinding.ResetBindings(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(clinic.ToString());
        }

        private void btnProtocolExercisesUP_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();

            int index1 = lstProtocolExercises.SelectedIndex;
            int index2; 

            if (index1 == 0)
                index2 = lstProtocolExercises.Items.Count - 1;
            else index2 = index1 - 1;


            ((Protocol)lstProtocols.SelectedItem).Exercises = Utils.Swap<Exercise>(((Protocol)lstProtocols.SelectedItem).Exercises, index1, index2);

            lstProtocolExercises.DataSource = null;
            lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
            lstProtocolExercises.DisplayMember = "Name";

            lstProtocolExercises.SelectedIndex = index2;

            AddProtocolsEvents();
        }

        private void lstStates_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveStatesEvents();
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
            AddStatesEvents();
        }

        private void btnStatesApply_Click(object sender, EventArgs e)
        {
            State s = (State)lstStates.Items[StatePanelLstStates_Selected];
            
            s.Name = txtConditionDetails1.Text;
            s.HOHCode = txtConditionDetails2.Text;
            s.UserMsg = txtConditionDetails3.Text;
            s.CallbackMsg = txtConditionDetails4.Text;

            // lstStates.Items[StatePanelLstStates_Selected] = new State(s);
           // string name = backup_clinic.States[StatePanelLstStates_Selected].Name;

            //for (int i = 0; i < blExercises.Count; i++)
            //{
            //    if (clinic.Exercises[i].PreState.Name == name)
            //        clinic.Exercises[i].PreState = new State(s);
            //    if (clinic.Exercises[i].TargetState.Name == name)
            //        clinic.Exercises[i].TargetState = new State(s);
            //    if (clinic.Exercises[i].PostState.Name == name)
            //        clinic.Exercises[i].PostState = new State(s);
            //}

            //foreach (var pt in clinic.Protocols)
            //{
            //    for (int i = 0; i < pt.Exercises.Count; i++)
            //    {
            //        if (pt.Exercises[i].PreState.Name == name)
            //            pt.Exercises[i].PreState = new State(s);
            //        if (pt.Exercises[i].TargetState.Name == name)
            //            pt.Exercises[i].TargetState = new State(s);
            //        if (pt.Exercises[i].PostState.Name == name)
            //            pt.Exercises[i].PostState = new State(s);
            //    }
            //}


            blStates.ResetBindings();
            //blExercises.ResetBindings();
            //blProtocols.ResetBindings();
            
            SetupExercisesPanel();
            SetupProtocolsPanel();

            btnStatesApply.Enabled = false;
            btnExercisesApply.Enabled = false;

            HOHEventObj.UpdateClinic(clinic);
        }

        private void txtConditionDetails1_TextChanged(object sender, EventArgs e)
        {
                if (!btnStatesApply.Enabled && ((State)lstStates.SelectedItem) != null)
                    btnStatesApply.Enabled = txtConditionDetails1.Text != ((State)lstStates.SelectedItem).Name;
        }

        private void txtConditionDetails2_TextChanged(object sender, EventArgs e)
        {
            if (!btnStatesApply.Enabled && ((State)lstStates.SelectedItem) != null)
                btnStatesApply.Enabled = txtConditionDetails2.Text != ((State)lstStates.SelectedItem).HOHCode;
        }

        private void txtConditionDetails3_TextChanged(object sender, EventArgs e)
        {
            if (!btnStatesApply.Enabled && ((State)lstStates.SelectedItem) != null)
                btnStatesApply.Enabled = txtConditionDetails3.Text != ((State)lstStates.SelectedItem).UserMsg;
        }

        private void txtConditionDetails4_TextChanged(object sender, EventArgs e)
        {
            if (!btnStatesApply.Enabled && ((State)lstStates.SelectedItem) != null)
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

            btnExercisesApply.Enabled = false;
            HOHEventObj.UpdateClinic(clinic);
        }

        private void txtExerciseDetails1_TextChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled && ((Exercise)lstExercises.SelectedItem) != null)
                btnExercisesApply.Enabled = (txtExerciseDetails1.Text != ((Exercise)lstExercises.SelectedItem).Name);
           // Debug.WriteLine("txt1:" + (txtExerciseDetails1.Text != ((Exercise)lstExercises.SelectedItem).UserMsg));
        }

        private void txtExerciseDetails3_TextChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled && ((Exercise)lstExercises.SelectedItem) != null)
                btnExercisesApply.Enabled = (txtExerciseDetails3.Text != ((Exercise)lstExercises.SelectedItem).UserMsg);
           // Debug.WriteLine("txt3:" + (txtExerciseDetails3.Text != ((Exercise)lstExercises.SelectedItem).UserMsg));       
        }

        private void txtExerciseDetails2_TextChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled && ((Exercise)lstExercises.SelectedItem) != null)
                btnExercisesApply.Enabled = (txtExerciseDetails2.Text != ((Exercise)lstExercises.SelectedItem).SFCode);
           // Debug.WriteLine("txt2:" + (txtExerciseDetails2.Text != ((Exercise)lstExercises.SelectedItem).SFCode));
        }

        private void comboExerciseState1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled && ((Exercise)lstExercises.SelectedItem) != null)
                btnExercisesApply.Enabled = (comboExerciseState1.SelectedItem != ((Exercise)lstExercises.SelectedItem).PreState);
           // Debug.WriteLine("cmb1:" + (comboExerciseState1.SelectedItem != ((Exercise)lstExercises.SelectedItem).PreState));
        }

        private void comboExerciseState2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled && ((Exercise)lstExercises.SelectedItem) != null)
                btnExercisesApply.Enabled = (comboExerciseState2.SelectedItem != ((Exercise)lstExercises.SelectedItem).TargetState);
          //  Debug.WriteLine("cmb2:" + (comboExerciseState2.SelectedItem != ((Exercise)lstExercises.SelectedItem).TargetState));
        }

        private void comboExerciseState3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!btnExercisesApply.Enabled && ((Exercise)lstExercises.SelectedItem) != null)
                btnExercisesApply.Enabled = (comboExerciseState3.SelectedItem != ((Exercise)lstExercises.SelectedItem).PostState);
          //  Debug.WriteLine("cmb3:" + (comboExerciseState3.SelectedItem != ((Exercise)lstExercises.SelectedItem).PostState));
        }

        private void lstExercises_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveExercisesEvents();
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
                    {
                        comboExerciseState1.Text = ((Exercise)lstExercises.SelectedItem).PreState.Name;
                    }
                    else
                    {
                        comboExerciseState1.SelectedIndex = 0;
                        ((Exercise)lstExercises.SelectedItem).PreState = (State)comboExerciseState1.SelectedItem;
                    }


                    if (((Exercise)lstExercises.SelectedItem).TargetState != null)
                    {
                        comboExerciseState2.Text = ((Exercise)lstExercises.SelectedItem).TargetState.Name;
                    }
                    else
                    {
                        comboExerciseState2.SelectedIndex = 0;
                        ((Exercise)lstExercises.SelectedItem).TargetState = (State)comboExerciseState2.SelectedItem;
                    }

                    if (((Exercise)lstExercises.SelectedItem).PostState != null)
                    {
                        comboExerciseState3.Text = ((Exercise)lstExercises.SelectedItem).PostState.Name;
                    }
                    else
                    {
                        comboExerciseState3.SelectedIndex = 0;
                        ((Exercise)lstExercises.SelectedItem).PostState = (State)comboExerciseState3.SelectedItem;
                    }
                }
                ExercisesPanelLstExercises_Selected = lstExercises.SelectedIndex;
            }
            AddExercisesEvents();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            HOHEventObj.UpdateClinic(clinic);
            SetupStatePanel();
            SetupExercisesPanel();
            SetupProtocolsPanel();
            
            lstStates_SelectedIndexChanged(sender, e);
            lstExercises_SelectedIndexChanged(sender, e);
            lstProtocols_SelectedIndexChanged(sender, e);
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

            txtProtocolExerciseDetails4.TextChanged -= txtProtocolExerciseDetails4_TextChanged;
            txtProtocolExerciseDetails5.TextChanged -= txtProtocolExerciseDetails5_TextChanged;
            txtProtocolName.TextChanged -= txtProtocolName_TextChanged;
        }

        private void AddProtocolsEvents()
        {
            lstProtocols.SelectedIndexChanged += lstProtocols_SelectedIndexChanged;
            lstProtocolExercises.SelectedIndexChanged += lstProtocolExercises_SelectedIndexChanged;
            lstAvailableExercises.SelectedIndexChanged += lstAvailableExercises_SelectedIndexChanged;

            txtProtocolExerciseDetails4.TextChanged += txtProtocolExerciseDetails4_TextChanged;
            txtProtocolExerciseDetails5.TextChanged += txtProtocolExerciseDetails5_TextChanged;
            txtProtocolName.TextChanged += txtProtocolName_TextChanged;
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
                    txtProtocolName.Text = ((Protocol)lstProtocols.SelectedItem).Name;
                    lstProtocolExercises.DataSource = null;
                    lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
                    lstProtocolExercises.DisplayMember = "Name";
                    if ((((Protocol)lstProtocols.SelectedItem).Exercises) != null)
                    {
                        if (lstProtocolExercises.Items.Count > 0) lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
                        lstProtocolExercises_SelectedIndexChanged(sender, e);
                    }
                    else
                    {
                        Debug.WriteLine("This protocol has no exercises");
                    }
                }
                ProtocolPanelLstProtocols_Selected = lstProtocols.SelectedIndex;
            }
            AddProtocolsEvents();
        }

        private void lstProtocolExercises_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();
            //lstProtocolExercises.Refresh();
            if (lstProtocolExercises.Items.Count > 0 )
            {   
                if (lstProtocolExercises.SelectedItem!=null)
                { 
                    txtProtocolExerciseDetails1.Text = ((Exercise)lstProtocolExercises.SelectedItem).Name;
                    txtProtocolExerciseDetails2.Text = ((Exercise)lstProtocolExercises.SelectedItem).UserMsg;
                    txtProtocolExerciseDetails3.Text = ((Exercise)lstProtocolExercises.SelectedItem).SFCode;
                    txtProtocolExerciseDetails4.Text = ((Exercise)lstProtocolExercises.SelectedItem).ExerciseTime.ToString();
                    txtProtocolExerciseDetails5.Text = ((Exercise)lstProtocolExercises.SelectedItem).Repetitions.ToString();

                    if (((Exercise)lstProtocolExercises.SelectedItem).PreState != null)
                    {
                        comboProtocolExerciseState1.Text = ((Exercise)lstProtocolExercises.SelectedItem).PreState.Name;
                    }
                    else
                    {
                        comboProtocolExerciseState1.SelectedIndex = 0;
                        ((Exercise)lstProtocolExercises.SelectedItem).PreState = (State)comboProtocolExerciseState1.SelectedItem;
                    }


                    if (((Exercise)lstProtocolExercises.SelectedItem).TargetState != null)
                    {
                        comboProtocolExerciseState2.Text = ((Exercise)lstProtocolExercises.SelectedItem).TargetState.Name;
                    }
                    else
                    {
                        comboProtocolExerciseState2.SelectedIndex = 0;
                        ((Exercise)lstProtocolExercises.SelectedItem).TargetState = (State)comboProtocolExerciseState2.SelectedItem;
                    }

                    if (((Exercise)lstProtocolExercises.SelectedItem).PostState != null)
                    {
                        comboProtocolExerciseState3.Text = ((Exercise)lstProtocolExercises.SelectedItem).PostState.Name;
                    }
                    else
                    {
                        comboProtocolExerciseState3.SelectedIndex = 0;
                        ((Exercise)lstProtocolExercises.SelectedItem).PostState = (State)comboProtocolExerciseState3.SelectedItem;
                    }
                } else
                {
                    lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
                    lstProtocolExercises_SelectedIndexChanged(sender, e);
                }

                ProtocolPanelLstProtocolExercises_Selected = lstProtocolExercises.SelectedIndex;
            }
            else
            {
                txtProtocolExerciseDetails1.Text = String.Empty;
                txtProtocolExerciseDetails2.Text = String.Empty;
                txtProtocolExerciseDetails3.Text = String.Empty;
                txtProtocolExerciseDetails4.Text = String.Empty;
                txtProtocolExerciseDetails5.Text = String.Empty;

                comboProtocolExerciseState1.ResetText();
                comboProtocolExerciseState2.ResetText();
                comboProtocolExerciseState3.ResetText();
            }

            btnProtocolExercisesRemove.Enabled = Convert.ToBoolean(lstProtocolExercises.Items.Count);

            AddProtocolsEvents();
         }

        private void lstAvailableExercises_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnProtocolsApply_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();
            Debug.WriteLine("Apply - Pr : " + ProtocolPanelLstProtocols_Selected + " > Ex : " + ProtocolPanelLstProtocolExercises_Selected);
            Exercise s = (Exercise)lstProtocolExercises.Items[ProtocolPanelLstProtocolExercises_Selected];
            s.ExerciseTime = Convert.ToInt16(txtProtocolExerciseDetails4.Text.Trim());
            s.Repetitions = Convert.ToInt16(txtProtocolExerciseDetails5.Text.Trim());

            ((Protocol)lstProtocols.Items[ProtocolPanelLstProtocols_Selected]).Exercises[ProtocolPanelLstProtocolExercises_Selected] = s;
            ((Protocol)lstProtocols.Items[ProtocolPanelLstProtocols_Selected]).Name = txtProtocolName.Text;

            blProtocols.ResetBindings();
            blExercises.ResetBindings();

            lstProtocols.SelectedIndex = ProtocolPanelLstProtocols_Selected;
            lstProtocolExercises.DataSource = null;
            lstProtocolExercises.DataSource = (((Protocol)lstProtocols.Items[ProtocolPanelLstProtocols_Selected]).Exercises);
            lstProtocolExercises.DisplayMember = "Name";
            lstProtocolExercises.SelectedIndex = ProtocolPanelLstProtocolExercises_Selected;


            //lstProtocols_SelectedIndexChanged(sender, e);
            lstProtocolExercises_SelectedIndexChanged(sender, e);

            btnProtocolsApply.Enabled = false;
            AddProtocolsEvents();
            HOHEventObj.UpdateClinic(clinic);
        }

        private void btnAddProtocol_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();

            blProtocols.Add(new Protocol("New Protocol " + (clinic.Protocols.Count + 1)));

            blProtocols.ResetBindings();

            if ((((Protocol)lstProtocols.SelectedItem).Exercises) != null)
            if ((((Protocol)lstProtocols.SelectedItem).Exercises).Count!=0)
            { 
                lstProtocolExercises.DataSource = null;
                lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
                lstProtocolExercises.DisplayMember = "Name";
                lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
                lstProtocolExercises_SelectedIndexChanged(sender, e);
            }
            
            btnProtocolDelete.Enabled = Convert.ToBoolean(blProtocols.Count);
            btnProtocolExercisesRemove.Enabled = Convert.ToBoolean(blProtocols.Count);
            btnProtocolExercisesAdd.Enabled = Convert.ToBoolean(blProtocols.Count);

            lstProtocols.SelectedIndex = lstProtocols.Items.Count - 1;
            lstProtocols_SelectedIndexChanged(sender, e);
         
            AddProtocolsEvents();
        }

        private void btnProtocolDelete_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();

            try
            {
                blProtocols.RemoveAt(lstProtocols.SelectedIndex);
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
            blProtocols.ResetBindings();
            
            if (blProtocols.Count == 0)
            {
                lstProtocolExercises.DataSource = null;

                txtProtocolExerciseDetails1.Text = String.Empty;
                txtProtocolExerciseDetails2.Text = String.Empty;
                txtProtocolExerciseDetails3.Text = String.Empty;
                txtProtocolExerciseDetails4.Text = String.Empty;
                txtProtocolExerciseDetails5.Text = String.Empty;

                comboProtocolExerciseState1.ResetText();
                comboProtocolExerciseState2.ResetText();
                comboProtocolExerciseState3.ResetText();
            } else
            {
                lstProtocolExercises.DataSource = null;
                lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
                lstProtocolExercises.DisplayMember = "Name";
            }

            btnProtocolDelete.Enabled = Convert.ToBoolean(blProtocols.Count);
            btnProtocolExercisesRemove.Enabled = Convert.ToBoolean(blProtocols.Count);
            btnProtocolExercisesAdd.Enabled = Convert.ToBoolean(blProtocols.Count);
            txtProtocolExerciseDetails4.Enabled = Convert.ToBoolean(blProtocols.Count);
            txtProtocolExerciseDetails5.Enabled = Convert.ToBoolean(blProtocols.Count);
            
            AddProtocolsEvents();
           
        }

        private void btnProtocolExercisesAdd_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();
            if (lstProtocolExercises.Items.Count == 0)
            {
                txtProtocolExerciseDetails4.Enabled = true;
                txtProtocolExerciseDetails5.Enabled = true;
            }

            Exercise ex = new Exercise((Exercise)lstAvailableExercises.SelectedItem); //deepclone
            ((Protocol)lstProtocols.SelectedItem).Exercises.Add(ex);
            Debug.WriteLine("Adding Pr : " + lstProtocols.SelectedIndex + " > AvalEx : " + lstAvailableExercises.SelectedIndex);

            blProtocols.ResetBindings();
            lstProtocols.SelectedIndex = ProtocolPanelLstProtocols_Selected;

            lstProtocolExercises.DataSource = null;
            lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
            lstProtocolExercises.DisplayMember = "Name";
            
            lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
            lstProtocolExercises_SelectedIndexChanged(sender, e);
        
            AddProtocolsEvents();
          
        }

        private void btnProtocolExercisesRemove_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();
 
            try
            {
                blProtocols[lstProtocols.SelectedIndex].Exercises.RemoveAt(lstProtocolExercises.SelectedIndex);
            }
            catch(Exception ex) { Debug.WriteLine(ex.Message); }
            blProtocols.ResetBindings();

            //lstProtocolExercises.DataSource = null;
            //lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
            //lstProtocolExercises.DisplayMember = "Name";

            if (lstProtocolExercises.Items.Count == 0)
            {
                txtProtocolExerciseDetails1.Text = String.Empty;
                txtProtocolExerciseDetails2.Text = String.Empty;
                txtProtocolExerciseDetails3.Text = String.Empty;
                txtProtocolExerciseDetails4.Text = String.Empty;
                txtProtocolExerciseDetails5.Text = String.Empty;

                comboProtocolExerciseState1.ResetText();
                comboProtocolExerciseState2.ResetText();
                comboProtocolExerciseState3.ResetText();
            }

            btnProtocolExercisesRemove.Enabled = Convert.ToBoolean(lstProtocolExercises.Items.Count);
            txtProtocolExerciseDetails4.Enabled = Convert.ToBoolean(lstProtocolExercises.Items.Count);
            txtProtocolExerciseDetails5.Enabled = Convert.ToBoolean(lstProtocolExercises.Items.Count);

            lstProtocols_SelectedIndexChanged(sender, e);
            lstProtocolExercises.SelectedIndex = lstProtocolExercises.Items.Count - 1;
            //  lstProtocolExercises_SelectedIndexChanged(sender, e);

            AddProtocolsEvents();

        }

        private void txtProtocolExerciseDetails4_TextChanged(object sender, EventArgs e)
        {
             if (!btnProtocolsApply.Enabled && ((Exercise)lstProtocolExercises.SelectedItem)!=null)
            { 
                 btnProtocolsApply.Enabled = (txtProtocolExerciseDetails4.Text != ((Exercise)lstProtocolExercises.SelectedItem).ExerciseTime.ToString());
            Debug.WriteLine("txtchange " + txtProtocolExerciseDetails4.Text + " Time " + ((Exercise)lstProtocolExercises.SelectedItem).Name);
            }
            Debug.WriteLine("Pr : " + lstProtocols.SelectedIndex + " > Ex : " + lstProtocolExercises.SelectedIndex);
        }

        private void txtProtocolExerciseDetails5_TextChanged(object sender, EventArgs e)
        {
            if (!btnProtocolsApply.Enabled && ((Exercise)lstProtocolExercises.SelectedItem) != null)
            {
                btnProtocolsApply.Enabled = (txtProtocolExerciseDetails5.Text != ((Exercise)lstProtocolExercises.SelectedItem).Repetitions.ToString());
                Debug.WriteLine("txtchange " + txtProtocolExerciseDetails5.Text + " Time " + ((Exercise)lstProtocolExercises.SelectedItem).Name);
            }
            Debug.WriteLine("Pr : " + lstProtocols.SelectedIndex + " > Ex : " + lstProtocolExercises.SelectedIndex);

        }

        private void lblStatesDuplicate_Click(object sender, EventArgs e)
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

        private void lblExercisesDuplicate_Click(object sender, EventArgs e)
        {
            if (lstExercises.SelectedItem != null)
            {
                blExercises.Add(new Exercise((Exercise)lstExercises.SelectedItem));
                lstExercises.SelectedIndex = lstExercises.Items.Count - 1;
                ((Exercise)lstExercises.SelectedItem).Name += " - copy";
                blExercises.ResetBindings();
            }
        }

        private void ProtocolEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            HOHEventObj.UpdateClinic(clinic);

        }

        private void lstAvailableExercises_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txtProtocolName_TextChanged(object sender, EventArgs e)
        {
            if (!btnProtocolsApply.Enabled && ((Protocol)lstProtocols.SelectedItem) != null)
            {
                btnProtocolsApply.Enabled = (txtProtocolName.Text != ((Protocol)lstProtocols.SelectedItem).Name);
                Debug.WriteLine("txtchange " + txtProtocolName.Text + " Time " + ((Protocol)lstProtocols.SelectedItem).Name);
            }
            //Debug.WriteLine("Pr : " + lstProtocols.SelectedIndex + " > Ex : " + lstProtocolExercises.SelectedIndex);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void txtProtocolExerciseDetails1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnProtocolExercisesDown_Click(object sender, EventArgs e)
        {
            RemoveProtocolsEvents();

            int index1 = lstProtocolExercises.SelectedIndex;
            int index2;

            if (index1 == lstProtocolExercises.Items.Count - 1)
                index2 = 0;
            else index2 = index1 + 1;


            ((Protocol)lstProtocols.SelectedItem).Exercises = Utils.Swap<Exercise>(((Protocol)lstProtocols.SelectedItem).Exercises, index1, index2);

            lstProtocolExercises.DataSource = null;
            lstProtocolExercises.DataSource = (((Protocol)lstProtocols.SelectedItem).Exercises);
            lstProtocolExercises.DisplayMember = "Name";

            lstProtocolExercises.SelectedIndex = index2;

            AddProtocolsEvents();
        }
    }

    
    #endregion
}
