namespace WinApp
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
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.labelOpen = new System.Windows.Forms.Label();
			this.labelClose = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.labelTime = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.labelData = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.labelSource = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(36, 37);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "開盤：";
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// labelOpen
			// 
			this.labelOpen.AutoSize = true;
			this.labelOpen.Location = new System.Drawing.Point(84, 37);
			this.labelOpen.Name = "labelOpen";
			this.labelOpen.Size = new System.Drawing.Size(17, 12);
			this.labelOpen.TabIndex = 1;
			this.labelOpen.Text = "---";
			// 
			// labelClose
			// 
			this.labelClose.AutoSize = true;
			this.labelClose.Location = new System.Drawing.Point(266, 37);
			this.labelClose.Name = "labelClose";
			this.labelClose.Size = new System.Drawing.Size(17, 12);
			this.labelClose.TabIndex = 3;
			this.labelClose.Text = "---";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(218, 37);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(41, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "收盤：";
			// 
			// labelStatus
			// 
			this.labelStatus.AutoSize = true;
			this.labelStatus.BackColor = System.Drawing.SystemColors.Highlight;
			this.labelStatus.ForeColor = System.Drawing.Color.White;
			this.labelStatus.Location = new System.Drawing.Point(468, 37);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(17, 12);
			this.labelStatus.TabIndex = 5;
			this.labelStatus.Text = "---";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(420, 37);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(41, 12);
			this.label4.TabIndex = 4;
			this.label4.Text = "狀態：";
			// 
			// labelTime
			// 
			this.labelTime.AutoSize = true;
			this.labelTime.Location = new System.Drawing.Point(84, 9);
			this.labelTime.Name = "labelTime";
			this.labelTime.Size = new System.Drawing.Size(17, 12);
			this.labelTime.TabIndex = 7;
			this.labelTime.Text = "---";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(36, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 12);
			this.label5.TabIndex = 6;
			this.label5.Text = "現在：";
			// 
			// labelData
			// 
			this.labelData.AutoSize = true;
			this.labelData.Location = new System.Drawing.Point(84, 78);
			this.labelData.Name = "labelData";
			this.labelData.Size = new System.Drawing.Size(17, 12);
			this.labelData.TabIndex = 9;
			this.labelData.Text = "---";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(36, 78);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 12);
			this.label6.TabIndex = 8;
			this.label6.Text = "資料：";
			// 
			// labelSource
			// 
			this.labelSource.AutoSize = true;
			this.labelSource.BackColor = System.Drawing.SystemColors.Highlight;
			this.labelSource.ForeColor = System.Drawing.Color.White;
			this.labelSource.Location = new System.Drawing.Point(96, 115);
			this.labelSource.Name = "labelSource";
			this.labelSource.Size = new System.Drawing.Size(17, 12);
			this.labelSource.TabIndex = 12;
			this.labelSource.Text = "---";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(36, 115);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 11;
			this.label7.Text = "報價源：";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.labelSource);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.labelData);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.labelTime);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.labelStatus);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.labelClose);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.labelOpen);
			this.Controls.Add(this.label1);
			this.Name = "Main";
			this.Text = "Main";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
			this.Load += new System.EventHandler(this.Main_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.Label labelOpen;
		private System.Windows.Forms.Label labelClose;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label labelTime;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labelData;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label labelSource;
		private System.Windows.Forms.Label label7;
	}
}