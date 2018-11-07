using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace BFS_VoIP
{
	public partial class NumberPicker : DevExpress.XtraEditors.XtraForm
	{
		private string _number;
		private Contact contact;

		private Point lastPoint;
		private bool movingFrame = false;

		public string Number
		{
			get
			{
				return _number;
			}
			internal set
			{
				_number = value;
				this.Close();
			}
		}
		private int count = 0;

		public NumberPicker()
		{
			Type type = typeof(DevExpress.LookAndFeel.LookAndFeelPainterHelper);
			FieldInfo fi = type.GetField("painters", BindingFlags.Static | BindingFlags.NonPublic);
			BaseLookAndFeelPainters[] painters = (BaseLookAndFeelPainters[])fi.GetValue(null);
			painters[(int)ActiveLookAndFeelStyle.UltraFlat] = new MyUltraFlatLookAndFeelPainters(null);
			InitializeComponent();
		}

		public NumberPicker(Contact contact) : this()
		{
			this.contact = contact;

		}

		private void NumberPicker_Load(object sender, EventArgs e)
		{
			if (contact.Numbers.Count == 1)
			{
				Number = contact.Numbers[0].Number;
			}
			else
			{
				foreach (TelNumber tel in contact.Numbers)
				{
					this.Controls.Add(getButton(tel));
				}
				this.Size = new Size(200, 60 + count * 50);
				this.Location = System.Windows.Forms.Cursor.Position;
				lFullName.Text = contact.Fullname;
			}
		}

		private SimpleButton getButton(TelNumber tel)
		{
			SimpleButton btn = new SimpleButton();
			btn.Appearance.BackColor = System.Drawing.Color.Gray;
			btn.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			btn.Appearance.ForeColor = System.Drawing.Color.White;
			btn.Appearance.Options.UseBackColor = true;
			btn.Appearance.Options.UseFont = true;
			btn.Appearance.Options.UseForeColor = true;
			btn.Appearance.Options.UseTextOptions = true;
			btn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
			btn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
			btn.Location = new System.Drawing.Point(20, 45 + count * 50);
			btn.Size = new System.Drawing.Size(160, 40);
			btn.Tag = tel.Number;
			btn.Text = tel.FormatedString.Replace(" ", "\r\n        ");
			btn.Click += btn_Click;
			count++;
			return btn;
		}

		void btn_Click(object sender, EventArgs e)
		{
			Number = (sender as SimpleButton).Tag.ToString();
		}

		private void NumberPicker_Deactivate(object sender, EventArgs e)
		{
			this.Close();
		}

		private void NumberPicker_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				lastPoint = e.Location;
				movingFrame = true;
			}
		}

		private void NumberPicker_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
				movingFrame = false;
		}

		private void NumberPicker_MouseMove(object sender, MouseEventArgs e)
		{
			if (movingFrame)
				this.Location = new Point(this.Left - (this.lastPoint.X - e.X), this.Top - (this.lastPoint.Y - e.Y));
		}
	}
}