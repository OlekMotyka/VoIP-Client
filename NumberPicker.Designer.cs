namespace BFS_VoIP
{
	partial class NumberPicker
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
			this.lFullName = new DevExpress.XtraEditors.LabelControl();
			this.SuspendLayout();
			// 
			// lFullName
			// 
			this.lFullName.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.lFullName.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
			this.lFullName.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
			this.lFullName.Location = new System.Drawing.Point(13, 12);
			this.lFullName.Name = "lFullName";
			this.lFullName.Size = new System.Drawing.Size(174, 27);
			this.lFullName.TabIndex = 0;
			this.lFullName.Text = "Marcelus Wallace";
			this.lFullName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NumberPicker_MouseDown);
			this.lFullName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NumberPicker_MouseMove);
			this.lFullName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NumberPicker_MouseUp);
			// 
			// NumberPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(200, 60);
			this.ControlBox = false;
			this.Controls.Add(this.lFullName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NumberPicker";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "NumberPicker";
			this.TopMost = true;
			this.Deactivate += new System.EventHandler(this.NumberPicker_Deactivate);
			this.Load += new System.EventHandler(this.NumberPicker_Load);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NumberPicker_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NumberPicker_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NumberPicker_MouseUp);
			this.ResumeLayout(false);

		}

		#endregion

		private DevExpress.XtraEditors.LabelControl lFullName;
	}
}