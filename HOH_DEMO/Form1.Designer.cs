namespace HOH_DEMO
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.textBoxcmd = new System.Windows.Forms.TextBox();
            this.buttontest = new System.Windows.Forms.Button();
            this.buttonfitting = new System.Windows.Forms.Button();
            this.buttonfullyopen = new System.Windows.Forms.Button();
            this.buttonfullyclose = new System.Windows.Forms.Button();
            this.buttonCPM = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxInitialization = new System.Windows.Forms.GroupBox();
            this.groupBoxFunctions = new System.Windows.Forms.GroupBox();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.groupBoxCmd = new System.Windows.Forms.GroupBox();
            this.buttonExit = new System.Windows.Forms.Button();
            this.buttonResume = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.groupBoxExtra = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSetAuto = new System.Windows.Forms.Button();
            this.buttonSet = new System.Windows.Forms.Button();
            this.trackBarPositionAuto = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarPosition = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxInitialization.SuspendLayout();
            this.groupBoxFunctions.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.groupBoxCmd.SuspendLayout();
            this.groupBoxExtra.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPositionAuto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.BackColor = System.Drawing.Color.Yellow;
            this.buttonConnect.Location = new System.Drawing.Point(18, 23);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(100, 54);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = false;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Location = new System.Drawing.Point(6, 23);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(257, 113);
            this.textBoxLog.TabIndex = 1;
            // 
            // buttonSend
            // 
            this.buttonSend.BackColor = System.Drawing.Color.Yellow;
            this.buttonSend.Location = new System.Drawing.Point(113, 16);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(54, 30);
            this.buttonSend.TabIndex = 2;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = false;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxcmd
            // 
            this.textBoxcmd.Location = new System.Drawing.Point(9, 22);
            this.textBoxcmd.Name = "textBoxcmd";
            this.textBoxcmd.Size = new System.Drawing.Size(100, 20);
            this.textBoxcmd.TabIndex = 3;
            this.textBoxcmd.Text = "00";
            // 
            // buttontest
            // 
            this.buttontest.BackColor = System.Drawing.Color.Yellow;
            this.buttontest.Location = new System.Drawing.Point(18, 83);
            this.buttontest.Name = "buttontest";
            this.buttontest.Size = new System.Drawing.Size(100, 54);
            this.buttontest.TabIndex = 4;
            this.buttontest.Text = "Hand brace testing";
            this.buttontest.UseVisualStyleBackColor = false;
            this.buttontest.Click += new System.EventHandler(this.buttontest_Click);
            // 
            // buttonfitting
            // 
            this.buttonfitting.BackColor = System.Drawing.Color.Yellow;
            this.buttonfitting.Location = new System.Drawing.Point(113, 23);
            this.buttonfitting.Name = "buttonfitting";
            this.buttonfitting.Size = new System.Drawing.Size(100, 54);
            this.buttonfitting.TabIndex = 4;
            this.buttonfitting.Text = "Step mode";
            this.buttonfitting.UseVisualStyleBackColor = false;
            this.buttonfitting.Click += new System.EventHandler(this.buttonfitting_Click);
            // 
            // buttonfullyopen
            // 
            this.buttonfullyopen.BackColor = System.Drawing.Color.Yellow;
            this.buttonfullyopen.Location = new System.Drawing.Point(7, 83);
            this.buttonfullyopen.Name = "buttonfullyopen";
            this.buttonfullyopen.Size = new System.Drawing.Size(100, 54);
            this.buttonfullyopen.TabIndex = 4;
            this.buttonfullyopen.Text = "Fully open";
            this.buttonfullyopen.UseVisualStyleBackColor = false;
            this.buttonfullyopen.Click += new System.EventHandler(this.buttonfullyopen_Click);
            // 
            // buttonfullyclose
            // 
            this.buttonfullyclose.BackColor = System.Drawing.Color.Yellow;
            this.buttonfullyclose.Location = new System.Drawing.Point(113, 83);
            this.buttonfullyclose.Name = "buttonfullyclose";
            this.buttonfullyclose.Size = new System.Drawing.Size(100, 54);
            this.buttonfullyclose.TabIndex = 4;
            this.buttonfullyclose.Text = "Fully close";
            this.buttonfullyclose.UseVisualStyleBackColor = false;
            this.buttonfullyclose.Click += new System.EventHandler(this.buttonfullyclose_Click);
            // 
            // buttonCPM
            // 
            this.buttonCPM.BackColor = System.Drawing.Color.Yellow;
            this.buttonCPM.Location = new System.Drawing.Point(6, 23);
            this.buttonCPM.Name = "buttonCPM";
            this.buttonCPM.Size = new System.Drawing.Size(100, 54);
            this.buttonCPM.TabIndex = 4;
            this.buttonCPM.Text = "CPM";
            this.buttonCPM.UseVisualStyleBackColor = false;
            this.buttonCPM.Click += new System.EventHandler(this.buttonCPM_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(332, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Rehab-Robotics Company Ltd.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "HOH Demo";
            // 
            // groupBoxInitialization
            // 
            this.groupBoxInitialization.Controls.Add(this.buttonConnect);
            this.groupBoxInitialization.Controls.Add(this.buttontest);
            this.groupBoxInitialization.Location = new System.Drawing.Point(242, 59);
            this.groupBoxInitialization.Name = "groupBoxInitialization";
            this.groupBoxInitialization.Size = new System.Drawing.Size(133, 143);
            this.groupBoxInitialization.TabIndex = 7;
            this.groupBoxInitialization.TabStop = false;
            this.groupBoxInitialization.Text = "Initialization";
            // 
            // groupBoxFunctions
            // 
            this.groupBoxFunctions.Controls.Add(this.buttonCPM);
            this.groupBoxFunctions.Controls.Add(this.buttonfitting);
            this.groupBoxFunctions.Controls.Add(this.buttonfullyopen);
            this.groupBoxFunctions.Controls.Add(this.buttonfullyclose);
            this.groupBoxFunctions.Location = new System.Drawing.Point(16, 59);
            this.groupBoxFunctions.Name = "groupBoxFunctions";
            this.groupBoxFunctions.Size = new System.Drawing.Size(220, 143);
            this.groupBoxFunctions.TabIndex = 8;
            this.groupBoxFunctions.TabStop = false;
            this.groupBoxFunctions.Text = "Functions";
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Controls.Add(this.buttonClear);
            this.groupBoxLog.Controls.Add(this.textBoxLog);
            this.groupBoxLog.Location = new System.Drawing.Point(16, 483);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(359, 143);
            this.groupBoxLog.TabIndex = 9;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "Log";
            // 
            // buttonClear
            // 
            this.buttonClear.BackColor = System.Drawing.Color.Yellow;
            this.buttonClear.Location = new System.Drawing.Point(269, 112);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 25);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = false;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // groupBoxCmd
            // 
            this.groupBoxCmd.Controls.Add(this.textBoxcmd);
            this.groupBoxCmd.Controls.Add(this.buttonExit);
            this.groupBoxCmd.Controls.Add(this.buttonResume);
            this.groupBoxCmd.Controls.Add(this.buttonPause);
            this.groupBoxCmd.Controls.Add(this.buttonSend);
            this.groupBoxCmd.Location = new System.Drawing.Point(16, 424);
            this.groupBoxCmd.Name = "groupBoxCmd";
            this.groupBoxCmd.Size = new System.Drawing.Size(359, 53);
            this.groupBoxCmd.TabIndex = 10;
            this.groupBoxCmd.TabStop = false;
            this.groupBoxCmd.Text = "Command";
            // 
            // buttonExit
            // 
            this.buttonExit.BackColor = System.Drawing.Color.Yellow;
            this.buttonExit.Location = new System.Drawing.Point(289, 16);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(54, 30);
            this.buttonExit.TabIndex = 2;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // buttonResume
            // 
            this.buttonResume.BackColor = System.Drawing.Color.Yellow;
            this.buttonResume.Location = new System.Drawing.Point(231, 16);
            this.buttonResume.Name = "buttonResume";
            this.buttonResume.Size = new System.Drawing.Size(54, 30);
            this.buttonResume.TabIndex = 2;
            this.buttonResume.Text = "Resume";
            this.buttonResume.UseVisualStyleBackColor = false;
            this.buttonResume.Click += new System.EventHandler(this.buttonResume_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.BackColor = System.Drawing.Color.Yellow;
            this.buttonPause.Location = new System.Drawing.Point(173, 16);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(54, 30);
            this.buttonPause.TabIndex = 2;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = false;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // groupBoxExtra
            // 
            this.groupBoxExtra.Controls.Add(this.label8);
            this.groupBoxExtra.Controls.Add(this.label5);
            this.groupBoxExtra.Controls.Add(this.label7);
            this.groupBoxExtra.Controls.Add(this.label3);
            this.groupBoxExtra.Controls.Add(this.buttonSetAuto);
            this.groupBoxExtra.Controls.Add(this.buttonSet);
            this.groupBoxExtra.Controls.Add(this.trackBarPositionAuto);
            this.groupBoxExtra.Controls.Add(this.label6);
            this.groupBoxExtra.Controls.Add(this.trackBarPosition);
            this.groupBoxExtra.Controls.Add(this.label4);
            this.groupBoxExtra.Location = new System.Drawing.Point(16, 208);
            this.groupBoxExtra.Name = "groupBoxExtra";
            this.groupBoxExtra.Size = new System.Drawing.Size(359, 209);
            this.groupBoxExtra.TabIndex = 11;
            this.groupBoxExtra.TabStop = false;
            this.groupBoxExtra.Text = "Extra";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(183, 190);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Close";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(183, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Close";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 190);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Open";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Open";
            // 
            // buttonSetAuto
            // 
            this.buttonSetAuto.BackColor = System.Drawing.Color.Yellow;
            this.buttonSetAuto.Location = new System.Drawing.Point(244, 134);
            this.buttonSetAuto.Name = "buttonSetAuto";
            this.buttonSetAuto.Size = new System.Drawing.Size(100, 54);
            this.buttonSetAuto.TabIndex = 4;
            this.buttonSetAuto.Text = "Set";
            this.buttonSetAuto.UseVisualStyleBackColor = false;
            this.buttonSetAuto.Click += new System.EventHandler(this.buttonSetAuto_Click);
            // 
            // buttonSet
            // 
            this.buttonSet.BackColor = System.Drawing.Color.Yellow;
            this.buttonSet.Location = new System.Drawing.Point(244, 37);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(100, 54);
            this.buttonSet.TabIndex = 4;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = false;
            this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
            // 
            // trackBarPositionAuto
            // 
            this.trackBarPositionAuto.Location = new System.Drawing.Point(9, 154);
            this.trackBarPositionAuto.Maximum = 100;
            this.trackBarPositionAuto.Name = "trackBarPositionAuto";
            this.trackBarPositionAuto.Size = new System.Drawing.Size(200, 45);
            this.trackBarPositionAuto.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(201, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Last moved or all fingers reaches position";
            // 
            // trackBarPosition
            // 
            this.trackBarPosition.Location = new System.Drawing.Point(9, 56);
            this.trackBarPosition.Maximum = 100;
            this.trackBarPosition.Name = "trackBarPosition";
            this.trackBarPosition.Size = new System.Drawing.Size(200, 45);
            this.trackBarPosition.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(219, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Index finger close hand until reaches position";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(389, 641);
            this.Controls.Add(this.groupBoxExtra);
            this.Controls.Add(this.groupBoxCmd);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxFunctions);
            this.Controls.Add(this.groupBoxInitialization);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "HOH_DEMO";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxInitialization.ResumeLayout(false);
            this.groupBoxFunctions.ResumeLayout(false);
            this.groupBoxLog.ResumeLayout(false);
            this.groupBoxLog.PerformLayout();
            this.groupBoxCmd.ResumeLayout(false);
            this.groupBoxCmd.PerformLayout();
            this.groupBoxExtra.ResumeLayout(false);
            this.groupBoxExtra.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPositionAuto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPosition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxcmd;
        private System.Windows.Forms.Button buttontest;
        private System.Windows.Forms.Button buttonfitting;
        private System.Windows.Forms.Button buttonfullyopen;
        private System.Windows.Forms.Button buttonfullyclose;
        private System.Windows.Forms.Button buttonCPM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBoxInitialization;
        private System.Windows.Forms.GroupBox groupBoxFunctions;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.GroupBox groupBoxCmd;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonResume;
        private System.Windows.Forms.GroupBox groupBoxExtra;
        private System.Windows.Forms.TrackBar trackBarPosition;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonSetAuto;
        private System.Windows.Forms.TrackBar trackBarPositionAuto;
        private System.Windows.Forms.Label label6;
    }
}

