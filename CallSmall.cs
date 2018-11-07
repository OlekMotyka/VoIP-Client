using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace BFS_VoIP
{
	public partial class CallSmall : DevExpress.XtraEditors.XtraForm
	{
		private Point lastPoint;
		private bool movingFrame = false;

		public CallSmall()
		{
			InitializeComponent();
		}

		public CallSmall(string name, string number)
		{
			InitializeComponent();
			lName.Text = name;
			lPhoneNumber.Text = number;
			this.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.Size.Width - 20, 70);
		}

		#region MovingForm
		private void CallSmall_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				lastPoint = e.Location;
				movingFrame = true;
			}
		}

		private void CallSmall_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
				movingFrame = false;
		}

		private void CallSmall_MouseMove(object sender, MouseEventArgs e)
		{
			if (movingFrame)
				this.Location = new Point(this.Left - (this.lastPoint.X - e.X), this.Top - (this.lastPoint.Y - e.Y));
		}
		#endregion

		private void btnAnswer_Click(object sender, EventArgs e)
		{
			(this.Owner as Main).btnAnswerCall.PerformClick();
			this.Close();
		}

		private void simpleButton1_Click(object sender, EventArgs e)
		{
			(this.Owner as Main).btnEndCall.PerformClick();
			this.Close();
		}
	}
}