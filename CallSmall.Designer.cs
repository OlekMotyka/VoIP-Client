namespace BFS_VoIP
{
	partial class CallSmall
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
			this.lName = new DevExpress.XtraEditors.LabelControl();
			this.lPhoneNumber = new DevExpress.XtraEditors.LabelControl();
			this.btnAnswer = new DevExpress.XtraEditors.SimpleButton();
			this.btnEndCall = new DevExpress.XtraEditors.SimpleButton();
			this.SuspendLayout();
			// 
			// lName
			// 
			this.lName.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.lName.Appearance.Font = new System.Drawing.Font("Tahoma", 18F);
			this.lName.Location = new System.Drawing.Point(12, 12);
			this.lName.Name = "lName";
			this.lName.Size = new System.Drawing.Size(96, 29);
			this.lName.TabIndex = 0;
			this.lName.Text = "Nieznany";
			this.lName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseDown);
			this.lName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseMove);
			this.lName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseUp);
			// 
			// lPhoneNumber
			// 
			this.lPhoneNumber.Appearance.BackColor = System.Drawing.Color.Transparent;
			this.lPhoneNumber.Location = new System.Drawing.Point(12, 47);
			this.lPhoneNumber.Name = "lPhoneNumber";
			this.lPhoneNumber.Size = new System.Drawing.Size(54, 13);
			this.lPhoneNumber.TabIndex = 1;
			this.lPhoneNumber.Text = "321456987";
			this.lPhoneNumber.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseDown);
			this.lPhoneNumber.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseMove);
			this.lPhoneNumber.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseUp);
			// 
			// btnAnswer
			// 
			this.btnAnswer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAnswer.Appearance.BackColor = System.Drawing.Color.Green;
			this.btnAnswer.Appearance.Options.UseBackColor = true;
			this.btnAnswer.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
			this.btnAnswer.Image = global::BFS_VoIP.Properties.Resources.phone_32;
			this.btnAnswer.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnAnswer.Location = new System.Drawing.Point(12, 70);
			this.btnAnswer.Name = "btnAnswer";
			this.btnAnswer.Size = new System.Drawing.Size(120, 50);
			this.btnAnswer.TabIndex = 2;
			this.btnAnswer.Click += new System.EventHandler(this.btnAnswer_Click);
			// 
			// btnEndCall
			// 
			this.btnEndCall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnEndCall.Appearance.BackColor = System.Drawing.Color.Maroon;
			this.btnEndCall.Appearance.Options.UseBackColor = true;
			this.btnEndCall.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
			this.btnEndCall.Image = global::BFS_VoIP.Properties.Resources.phone_32;
			this.btnEndCall.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
			this.btnEndCall.Location = new System.Drawing.Point(140, 70);
			this.btnEndCall.Name = "btnEndCall";
			this.btnEndCall.Size = new System.Drawing.Size(120, 50);
			this.btnEndCall.TabIndex = 3;
			this.btnEndCall.Click += new System.EventHandler(this.simpleButton1_Click);
			// 
			// CallSmall
			// 
			this.Appearance.BackColor = System.Drawing.Color.Black;
			this.Appearance.ForeColor = System.Drawing.Color.White;
			this.Appearance.Options.UseBackColor = true;
			this.Appearance.Options.UseForeColor = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(272, 132);
			this.Controls.Add(this.btnEndCall);
			this.Controls.Add(this.btnAnswer);
			this.Controls.Add(this.lPhoneNumber);
			this.Controls.Add(this.lName);
			this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "CallSmall";
			this.Opacity = 0.7D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "CallSmall";
			this.TopMost = true;
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CallSmall_MouseUp);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DevExpress.XtraEditors.LabelControl lName;
		private DevExpress.XtraEditors.LabelControl lPhoneNumber;
		private DevExpress.XtraEditors.SimpleButton btnAnswer;
		private DevExpress.XtraEditors.SimpleButton btnEndCall;
	}
}