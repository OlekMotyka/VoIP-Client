using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace BFS_VoIP
{
	public partial class SettingsForm : DevExpress.XtraEditors.XtraForm
	{
		private Point lastPoint;
		private bool moving;
		public bool saved = false;

		public SettingsForm()
		{
			InitializeComponent();
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			teUserName.Text = Settings.UserName;
			tePassword.Text = Settings.Password;
			teDomain.Text = Settings.Domain;
			teHost.Text = Settings.Host;
			tePipeName.Text = Settings.PipeName;
			btnRingtone.Text = Settings.Ringtone;
			tsAA.IsOn = Settings.AutoAnswer;
			tsAutoRegister.IsOn = Settings.AutoRegister;
			tsDND.IsOn = Settings.DoNotDisturb;
			tsPrefix0.IsOn = Settings.Prefix0;
			tePort.EditValue = Settings.Port;
			tsPlaySound.IsOn = Settings.PlaySound;
			tsCallForm.IsOn = Settings.CallForm;
		}

		private void btnRingtone_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
		{
			ofdRingtone.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			if (ofdRingtone.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				btnRingtone.Text = ofdRingtone.FileName;
			}
		}

		private void btnSaveSettings_Click(object sender, EventArgs e)
		{
			Settings.UserName = teUserName.Text;
			Settings.Password = tePassword.Text;
			Settings.Domain = teDomain.Text;
			Settings.Host = teHost.Text;
			Settings.PipeName = tePipeName.Text;
			Settings.Ringtone = btnRingtone.Text;
			Settings.AutoAnswer = tsAA.IsOn;
			Settings.AutoRegister = tsAutoRegister.IsOn;
			Settings.DoNotDisturb = tsDND.IsOn;
			Settings.Prefix0 = tsPrefix0.IsOn;
			Settings.PlaySound = tsPlaySound.IsOn;
			Settings.Port = Convert.ToInt32(tePort.EditValue);
			Settings.CallForm = tsCallForm.IsOn;

			Settings.SaveSettings("");
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			saved = true;
			this.Close();
		}

		private void btnImport_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.DefaultExt = ".xml";
			ofd.Filter = "Dokumenty XML (*xml)|*.xml";
			ofd.Title = "Import ustawień";
			ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			ofd.Multiselect = false;
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Settings.LoadSettings(ofd.FileName);
				this.SettingsForm_Load(null, null);
			}
		}

		private void btnExport_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.AddExtension = true;
			sfd.CheckFileExists = false;
			sfd.OverwritePrompt = true;
			sfd.Filter = "Dokumenty XML (*xml)|*.xml";
			sfd.DefaultExt = ".xml";
			sfd.Title = "Eksport ustawień";
			sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				Settings.SaveSettings(sfd.FileName);
			}
		}

		#region Moving form
		private void ContactAddEdit_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				this.moving = true;
				this.lastPoint = e.Location;
			}
		}

		private void ContactAddEdit_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				this.moving = false;
			}
		}

		private void ContactAddEdit_MouseMove(object sender, MouseEventArgs e)
		{
			if (moving)
			{
				this.Location = new Point(this.Left - (this.lastPoint.X - e.X), this.Top - (this.lastPoint.Y - e.Y));
			}
		}
		#endregion

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}