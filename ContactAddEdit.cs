using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BFS_VoIP
{
	public partial class ContactAddEdit : DevExpress.XtraEditors.XtraForm
	{
		private Contact Con;
		private Contact prevContact;
		private Dictionary<string, TelType> slTelType;
		private bool saved;
		private Point lastPoint;
		private bool moving;

		public ContactAddEdit()
		{
			Type type = typeof(DevExpress.LookAndFeel.LookAndFeelPainterHelper);
			FieldInfo fi = type.GetField("painters", BindingFlags.Static | BindingFlags.NonPublic);
			BaseLookAndFeelPainters[] painters = (BaseLookAndFeelPainters[])fi.GetValue(null);
			painters[(int)ActiveLookAndFeelStyle.UltraFlat] = new MyUltraFlatLookAndFeelPainters(null);
			InitializeComponent();
			Con = new Contact();
		}

		public ContactAddEdit(Contact con)
		{
			InitializeComponent();
			Con = new Contact(con);
			prevContact = con;
		}

		private void ContactAddEdit_Load(object sender, EventArgs e)
		{
			slTelType = new Dictionary<string, TelType>();
			slTelType.Add("Domowy", TelType.HOME);
			slTelType.Add("Komórka", TelType.CELL);
			slTelType.Add("Praca", TelType.WORK);
			slTelType.Add("Fax", TelType.FAX);
			slTelType.Add("Pager", TelType.PAGER);
			slTelType.Add("Inne", TelType.OTHER);

			ContactBS.DataSource = Con;
		}

		public Contact getContact()
		{
			if (saved)
				return Con;
			else return null;
		}

		public Contact getPrevContact()
		{
			if (saved)
				return prevContact;
			else return null;
		}

		#region Moving form
		private void ContactAddEdit_Deactivate(object sender, EventArgs e)
		{
			if (!saved)
				this.Close();
		}

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

		#region Add buttons
		private void btnNumAdd_Click(object sender, EventArgs e)
		{
			Con.Numbers.Add(new TelNumber("", TelType.CELL));
			gvNumbers.RefreshData();
		}

		private void btnEmailAdd_Click(object sender, EventArgs e)
		{
			Con.Emails.Add(new Email("", EmailType.internet));
			gvEmails.RefreshData();
		}

		private void btnAddAdd_Click(object sender, EventArgs e)
		{
			Con.Addresses.Add(new Address("", gvAdd.DataRowCount == 0));
			gvAdd.RefreshData();
		}

		private void btnUrlAdd_Click(object sender, EventArgs e)
		{
			Con.Urls.Add("");
			gvUrls.RefreshData();
		}
		#endregion

		#region Delete buttons
		private void repNumDel_Click(object sender, EventArgs e)
		{
			gvNumbers.DeleteRow(gvNumbers.FocusedRowHandle);
			gvNumbers.RefreshData();
		}

		private void repEmailsDel_Click(object sender, EventArgs e)
		{
			gvEmails.DeleteRow(gvEmails.FocusedRowHandle);
			gvEmails.RefreshData();
		}

		private void repAddDel_Click(object sender, EventArgs e)
		{
			gvAdd.DeleteRow(gvAdd.FocusedRowHandle);
			gvAdd.RefreshData();
		}

		private void repUrlDel_Click(object sender, EventArgs e)
		{
			gvUrls.DeleteRow(gvUrls.FocusedRowHandle);
			gvUrls.RefreshData();
		}
		#endregion

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			Con.Numbers = Con.Numbers.Where(x => x.Number != "").ToList();
			Con.Emails = Con.Emails.Where(x => x.Address != "").ToList();
			Con.Addresses = Con.Addresses.Where(x => x.Locality != "").ToList();
			Con.Urls = Con.Urls.Where(x => x != "").ToList();
			Con.Revision = DateTime.Now;
			this.saved = true;
			this.Close();
		}


		private void picPicture_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				this.Deactivate -= ContactAddEdit_Deactivate;
				picPicture.LoadImage();
				if (picPicture.Image != null)
				{
					if (picPicture.Image.Height > 500 || picPicture.Image.Width > 500)
					{
						XtraMessageBox.Show("Obrazek nie może być większy niż 500px x 500px. Zmniejsz go lub wybierz inny.", "Zbyt duży rozmiar obrazka.", MessageBoxButtons.OK, MessageBoxIcon.Information);
						picPicture.EditValue = Con.Photo;
					}
					else
					{
						MemoryStream ms = new MemoryStream();
						picPicture.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
						Con.Photo = ms.ToArray();
					}
				}
				this.Deactivate += ContactAddEdit_Deactivate;
			}
		}

		private void gvUrls_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
		{
			if (e.IsGetData == true)
			{
				e.Value = Con.Urls[e.ListSourceRowIndex];
			}
			if (e.IsSetData == true)
			{
				Con.Urls[e.ListSourceRowIndex] = (string)e.Value;
			}
		}
	}
}