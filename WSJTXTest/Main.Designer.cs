namespace WSJTXTest
{
    partial class Main
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
            DisposeAll();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.mOut = new System.Windows.Forms.TextBox();
            this.lstCQ = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(2, 293);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(113, 40);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lstLog
            // 
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new System.Drawing.Point(2, 2);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(517, 290);
            this.lstLog.TabIndex = 1;
            // 
            // mOut
            // 
            this.mOut.Location = new System.Drawing.Point(2, 335);
            this.mOut.Multiline = true;
            this.mOut.Name = "mOut";
            this.mOut.Size = new System.Drawing.Size(517, 105);
            this.mOut.TabIndex = 2;
            // 
            // lstCQ
            // 
            this.lstCQ.FormattingEnabled = true;
            this.lstCQ.Location = new System.Drawing.Point(6, 19);
            this.lstCQ.Name = "lstCQ";
            this.lstCQ.Size = new System.Drawing.Size(505, 264);
            this.lstCQ.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstCQ);
            this.groupBox1.Location = new System.Drawing.Point(525, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(517, 290);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New CQ";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 444);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mOut);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.btnStart);
            this.Name = "Main";
            this.Text = "WSJT-X Decoder v1.0";
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        public System.Windows.Forms.ListBox lstLog;
        public System.Windows.Forms.TextBox mOut;
        public System.Windows.Forms.ListBox lstCQ;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

