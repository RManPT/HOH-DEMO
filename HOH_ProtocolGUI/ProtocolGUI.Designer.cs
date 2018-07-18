namespace HOH_ProtocolGUI
{
    partial class ProtocolGUI
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
            this.lblExerciseTime = new System.Windows.Forms.Label();
            this.lblExerciseName = new System.Windows.Forms.Label();
            this.lblExerciseMsg = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblExerciseTime
            // 
            this.lblExerciseTime.AutoSize = true;
            this.lblExerciseTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExerciseTime.Location = new System.Drawing.Point(209, 185);
            this.lblExerciseTime.Name = "lblExerciseTime";
            this.lblExerciseTime.Size = new System.Drawing.Size(87, 31);
            this.lblExerciseTime.TabIndex = 1;
            this.lblExerciseTime.Text = "00:00";
            this.lblExerciseTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblExerciseTime.Click += new System.EventHandler(this.lblExerciseTime_Click);
            // 
            // lblExerciseName
            // 
            this.lblExerciseName.AutoSize = true;
            this.lblExerciseName.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblExerciseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExerciseName.Location = new System.Drawing.Point(0, 0);
            this.lblExerciseName.Name = "lblExerciseName";
            this.lblExerciseName.Size = new System.Drawing.Size(197, 31);
            this.lblExerciseName.TabIndex = 2;
            this.lblExerciseName.Text = "Exercise Name";
            this.lblExerciseName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExerciseMsg
            // 
            this.lblExerciseMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblExerciseMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExerciseMsg.Location = new System.Drawing.Point(0, 0);
            this.lblExerciseMsg.Name = "lblExerciseMsg";
            this.lblExerciseMsg.Size = new System.Drawing.Size(510, 270);
            this.lblExerciseMsg.TabIndex = 3;
            this.lblExerciseMsg.Text = "Wait for instruction...";
            this.lblExerciseMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRestart
            // 
            this.btnRestart.Enabled = false;
            this.btnRestart.Location = new System.Drawing.Point(423, 235);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 23);
            this.btnRestart.TabIndex = 4;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(342, 235);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.button1_Click);
            // 
            // ProtocolGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 270);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.lblExerciseName);
            this.Controls.Add(this.lblExerciseTime);
            this.Controls.Add(this.lblExerciseMsg);
            this.Name = "ProtocolGUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProtocolGUI_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblExerciseTime;
        private System.Windows.Forms.Label lblExerciseName;
        private System.Windows.Forms.Label lblExerciseMsg;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnStop;
    }
}

