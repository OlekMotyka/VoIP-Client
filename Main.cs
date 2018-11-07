using BFS_VoIP.Properties;
using DevExpress.Utils.Paint;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.LookAndFeel;
using Sipek.Common;
using Sipek.Common.CallControl;
using Sipek.Sip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using DevExpress.Utils.Drawing;
using DevExpress.Utils;
using System.Media;
using System.Runtime.InteropServices;

namespace BFS_VoIP
{
    public partial class Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        CCallManager CallManager
        {
            get
            {
                return CCallManager.Instance;
            }
        }
        pjsipRegistrar Registrar
        {
            get
            {
                return pjsipRegistrar.Instance;
            }
        }
        pjsipStackProxy StackProxy
        {
            get
            {
                return pjsipStackProxy.Instance;
            }
        }
        PhoneConfig Config = new PhoneConfig();
        SipConfigStruct ConfigStruct
        {
            get
            {
                return SipConfigStruct.Instance;
            }
        }
        Dictionary<int, CallInfo> sesje = new Dictionary<int, CallInfo>();
        string dtmf = "";
        SoundPlayer ringtone = new SoundPlayer();

        public Main()
        {
            Type type = typeof(DevExpress.LookAndFeel.LookAndFeelPainterHelper);
            FieldInfo fi = type.GetField("painters", BindingFlags.Static | BindingFlags.NonPublic);
            BaseLookAndFeelPainters[] painters = (BaseLookAndFeelPainters[])fi.GetValue(null);
            painters[(int)ActiveLookAndFeelStyle.UltraFlat] = new MyUltraFlatLookAndFeelPainters(null);

            InitializeComponent();
            menuNotifyIcon.Renderer = new MyRenderer();
            menuContacts.Renderer = new MyRenderer();
        }

        #region DialPad
        private void ti_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            switch ((sender as TileItem).Name)
            {
                case "ti1":
                    teDialNumber.Text += "1";
                    break;
                case "ti2":
                    teDialNumber.Text += "2";
                    break;
                case "ti3":
                    teDialNumber.Text += "3";
                    break;
                case "ti4":
                    teDialNumber.Text += "4";
                    break;
                case "ti5":
                    teDialNumber.Text += "5";
                    break;
                case "ti6":
                    teDialNumber.Text += "6";
                    break;
                case "ti7":
                    teDialNumber.Text += "7";
                    break;
                case "ti8":
                    teDialNumber.Text += "8";
                    break;
                case "ti9":
                    teDialNumber.Text += "9";
                    break;
                case "ti0":
                    teDialNumber.Text += "0";
                    break;
                case "tiStar":
                    teDialNumber.Text += "*";
                    break;
                case "tiHash":
                    teDialNumber.Text += "#";
                    break;
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            teDialNumber.Text = teDialNumber.Text.Remove(teDialNumber.Text.Length - 1);
        }

        private void teDialNumber_TextChanged(object sender, EventArgs e)
        {
            btnBackspace.Enabled = teDialNumber.Text != "";
        }

        private void btnMakeCall_Click(object sender, EventArgs e)
        {
            Dial(teDialNumber.Text);
        }
        #endregion

        #region Settings
        private void bbtnRegister_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            StackProxy.initialize();
            CallManager.StackProxy = StackProxy;

            Config.CFNRFlag = false;
            Config.CFUFlag = false;
            Config.DefaultAccountIndex = 0;
            Config.AAFlag = Settings.AutoAnswer;
            Config.DNDFlag = Settings.DoNotDisturb;

            if (Config.Accounts.Count > 0)
                Registrar.unregisterAccounts();
            Config.Accounts.Clear();

            AccountConfig acc = new AccountConfig();
            acc.AccountName = Settings.UserName;
            acc.DisplayName = Settings.UserName;
            acc.DomainName = Settings.Domain;
            acc.HostName = Settings.Host;
            acc.Password = Settings.Password;
            acc.TransportMode = ETransportMode.TM_UDP;
            acc.UserName = Settings.UserName;
            acc.Id = Settings.UserName;
            acc.ProxyAddress = "";
            Config.Accounts.Add(acc);

			//ConfigStruct.listenPort = Settings.Port;
			//ConfigStruct.noUDP = false;
			//ConfigStruct.noTCP = true;
			//ConfigStruct.stunServer = "";
			//ConfigStruct.publishEnabled = false;
			//ConfigStruct.expires = 3600;
			//ConfigStruct.VADEnabled = true;
			//ConfigStruct.ECTail = 200;
			//ConfigStruct.nameServer = Settings.Host;
			//ConfigStruct.pollingEventsEnabled = false;
			

			CallManager.Config = Config;
			StackProxy.Config = Config;
            Registrar.Config = Config;
            pjsipPresenceAndMessaging.Instance.Config = Config;

            CallManager.CallStateRefresh -= CallManager_CallStateRefresh;
            CallManager.CallStateRefresh += CallManager_CallStateRefresh;
            //CallManager.IncomingCallNotification += CallManager_IncomingCallNotification;

            Registrar.AccountStateChanged -= Instance_AccountStateChanged;
            Registrar.AccountStateChanged += Instance_AccountStateChanged;

            if (CallManager.Initialize() == 0)
            {
				Registrar.registerAccounts();
            }
            else
            {
                lMessages.Caption = "Popraw ustawienia.";
                lStatus.Caption = "Nie połączono";
                timerMessages.Start();
            }

			int noOfCodecs = StackProxy.getNoOfCodecs();
            for (int i = 0; i < noOfCodecs; i++)
            {
				string codecname = StackProxy.getCodec(i);
				StackProxy.setCodecPriority(codecname, 128);
            }

			StackProxy.DtmfDigitReceived -= Instance_DtmfDigitReceived;
			StackProxy.DtmfDigitReceived += Instance_DtmfDigitReceived;
        }
        #endregion

        #region Contacts
        private List<Contact> Contacts
        {
            get { return Globals.Contacts; }
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            ContactAddEdit conae = new ContactAddEdit();
            conae.FormClosed += cae_FormClosed;
            conae.Show();
        }

        void cae_FormClosed(object sender, FormClosedEventArgs e)
        {
            ContactAddEdit cae = sender as ContactAddEdit;
            if (cae.getContact() != null)
            {
                if (Contacts.Contains(cae.getPrevContact()))
                    Contacts.RemoveAt(Contacts.IndexOf(cae.getPrevContact()));
                Contacts.Add(cae.getContact());
            }

			string vCards = "";
			foreach (Contact c in Contacts)
			{
				vCards += c.getVCard();
			}
			StreamWriter stream = new StreamWriter(Globals.PathContacts, false);
			stream.Write(vCards);
			stream.Close();

            gvContacts.RefreshData();
        }

        private void teContactSearch_TextChanged(object sender, EventArgs e)
        {
            if (teContactSearch.Text != "")
            {
                ColFullName.Visible = false;
                colShortString.Visible = true;
                colShortString.VisibleIndex = 2;
                colMakeCall.VisibleIndex = 3;

                gvContacts.ApplyColumnsFilter();
                colShortString.FilterInfo = new ColumnFilterInfo("Contains([ShortString], '" + teContactSearch.Text + "')");

            }
            else
            {
                gvContacts.ClearColumnsFilter();
                ColFullName.Visible = true;
                ColFullName.VisibleIndex = 2;
                colShortString.Visible = false;
            }
        }

        private void gvContacts_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == colShortString)
            {
                XPaint.Graphics.DrawMultiColorString(e.Cache, e.Bounds, e.DisplayText, teContactSearch.Text, e.Appearance, Color.Aqua, e.Appearance.BackColor, false, e.DisplayText.IndexOf(teContactSearch.Text, StringComparison.CurrentCultureIgnoreCase));
                e.Handled = true;
            }
        }

        private void gvContacts_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column == colMakeCall)
            {
                e.Value = Resources.Open_32x32;
            }
        }

        private void repMakeCall_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (Contacts[gvContacts.GetFocusedDataSourceRowIndex()].Numbers.Count > 1)
            {
                NumberPicker np = new NumberPicker(Contacts[gvContacts.GetFocusedDataSourceRowIndex()]);
                np.Disposed += np_Disposed;
                np.Show();
            }
            else if (Contacts[gvContacts.GetFocusedDataSourceRowIndex()].Numbers.Count == 1)
            {
                string n = Contacts[gvContacts.GetFocusedDataSourceRowIndex()].Numbers[0].Number;
                if (IsTransfer)
                    Transfer(n);
                else
                    Dial(n);
            }
        }

        void np_Disposed(object sender, EventArgs e)
        {
            if ((sender as NumberPicker).Number != null)
            {
                string n = (sender as NumberPicker).Number;
                if (IsTransfer)
                    Transfer(n);
                else
                    Dial(n);
            }
        }

        private void bbiImportContacts_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.DefaultExt = ".vcf";
            ofd.Filter = "Pliki kontaktów vCard (*vcf)|*.vcf";
            ofd.Title = "Import kontaktów";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    StreamReader stream = new StreamReader(file);
                    Contacts.AddRange(Contact.getContactsFromvCard(stream.ReadToEnd()));
                    stream.Close();
                }
                gvContacts.RefreshData();
            }
        }

        private void bbiExportContacts_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.CheckFileExists = false;
            sfd.OverwritePrompt = true;
            sfd.Filter = "Pliki kontaktów vCard (*vcf)|*.vcf";
            sfd.DefaultExt = ".vcf";
            sfd.Title = "Eksport kontaktów";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string vCards = "";
                foreach (Contact c in Contacts)
                {
                    vCards += c.getVCard();
                }
                StreamWriter stream = new StreamWriter(sfd.FileName, false);
                stream.Write(vCards);
                stream.Close();
            }
        }

        private void menuContacts_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gvContacts.FocusedRowHandle >= 0)
            {
                Contact c = Contacts[gvContacts.GetFocusedDataSourceRowIndex()];
                telefonyToolStripMenuItem.DropDownItems.Clear();
                emaileToolStripMenuItem.DropDownItems.Clear();
                adresyToolStripMenuItem.DropDownItems.Clear();
                stronyToolStripMenuItem.DropDownItems.Clear();
                telefonyToolStripMenuItem.Visible = c.Numbers.Count > 0;
                foreach (TelNumber t in c.Numbers)
                {
                    telefonyToolStripMenuItem.DropDownItems.Add(getMenuItem(telefonyToolStripMenuItem, t.FormatedString, t.Number));
                }
                emaileToolStripMenuItem.Visible = c.Emails.Count > 0;
                foreach (Email t in c.Emails)
                {
                    emaileToolStripMenuItem.DropDownItems.Add(getMenuItem(emaileToolStripMenuItem, t.Address, t.Address.Replace("\\", "")));
                }
                adresyToolStripMenuItem.Visible = c.Addresses.Count > 0;
                foreach (Address t in c.Addresses)
                {
                    adresyToolStripMenuItem.DropDownItems.Add(getMenuItem(adresyToolStripMenuItem, t.ToString().Replace("\r\n", " ").Trim(), t.ToString().Replace("\r\n", " ").Trim()));
                }
                stronyToolStripMenuItem.Visible = c.Urls.Count > 0;
                foreach (string t in c.Urls)
                {
                    stronyToolStripMenuItem.DropDownItems.Add(getMenuItem(stronyToolStripMenuItem, t, t));
                }
            }
        }

        private ToolStripItem getMenuItem(ToolStripMenuItem tss, string text, object tag)
        {
            ToolStripMenuItem tsmi = new ToolStripMenuItem(text);
            tsmi.Tag = tag;
            tsmi.Click += tsmi_Click;
            tsmi.ForeColor = Color.White;
            tsmi.BackColor = Color.DimGray;
            return tsmi;
        }

        private void tsmi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (sender as ToolStripMenuItem);
            if (tsmi.OwnerItem == stronyToolStripMenuItem)
                try
                {
                    if (((string)tsmi.Tag).StartsWith("http://"))
                        System.Diagnostics.Process.Start((string)(tsmi.Tag));
                    else
                        System.Diagnostics.Process.Start("http://" + (string)(tsmi.Tag));
                }
                catch { XtraMessageBox.Show("Podana strona internetowa jest niepoprawna.", "Błąd otwarcia linku."); }
            else if (tsmi.OwnerItem == adresyToolStripMenuItem)
                try
                {
                    System.Diagnostics.Process.Start("https://www.google.pl/maps/place/" + ((string)(tsmi.Tag)).Replace(" ", "+"));
                }
                catch { XtraMessageBox.Show("Podany adres jest niepoprawny.", "Błąd otwarcia linku."); }
            else if (tsmi.OwnerItem == emaileToolStripMenuItem)
                try
                {
                    System.Diagnostics.Process.Start("mailto:" + (string)(tsmi.Tag));
                }
                catch { XtraMessageBox.Show("Podany e-mail jest niepoprawny.", "Błąd otwarcia linku."); }
            else
                try
                {
                    Dial((string)(tsmi.Tag));
                }
                catch { XtraMessageBox.Show("Podany numer telefonu jest niepoprawny.", "Błąd otwarcia linku."); }
        }

        private void edytujToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContactAddEdit cae = new ContactAddEdit(Contacts[gvContacts.GetFocusedDataSourceRowIndex()]);
            cae.FormClosed += cae_FormClosed;
            cae.Show();
        }

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Contacts.RemoveAt(gvContacts.GetFocusedDataSourceRowIndex());
            gvContacts.RefreshData();
        }
        #endregion

        #region History
        private List<History> Histories { get { return Globals.Histories; } }

        private void repHistMakeCall_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (IsTransfer)
                Transfer((string)gvHistory.GetFocusedRowCellValue(colHisNumber));
            else
                Dial((string)gvHistory.GetFocusedRowCellValue(colHisNumber));

        }
        #endregion

        #region ActiveCall
        private void btnEndCall_Click(object sender, EventArgs e)
        {
            EndCall();
        }

        private void timerCallDuration_Tick(object sender, EventArgs e)
        {
            lTime.Text = CallManager.getCallInState(EStateId.ACTIVE).RuntimeDuration.ToString(@"mm\:ss");
        }

        private void btnAnswerCall_Click(object sender, EventArgs e)
        {
            if (CallManager.getNoCallsInState(EStateId.INCOMING) > 0)
                CallManager.onUserAnswer(CallManager.getCallInState(EStateId.INCOMING).Session);
        }

        private void btnTransferCall_Click(object sender, EventArgs e)
        {
            IsTransfer = !IsTransfer;
            if (IsTransfer)
                tabControll.SelectedTabPage = tabContacts;
        }

        private bool _isTransfer;
        private bool IsTransfer
        {
            get
            {
                return _isTransfer;
            }
            set
            {
                _isTransfer = value;
                if (value)
                {
                    repMakeCall.Buttons[0].Image = Resources.call_transfer_256;
                    repHistMakeCall.Buttons[0].Image = Resources.call_transfer_256;
                }
                else
                {

                    repMakeCall.Buttons[0].Image = Resources.phone_32;
                    repHistMakeCall.Buttons[0].Image = Resources.phone_32;
                }

            }
        }
        #endregion

        #region VoIP
        private void Instance_DtmfDigitReceived(int callId, int digit)
        {
            if (dtmf.Length == 0 && sesje.Count > 0)
                dtmf = sesje[callId].Number + ";";
            dtmf += (char)digit;
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    timerDtmf.Stop();
                    timerDtmf.Start();
                });
            }
            else
            {
                timerDtmf.Stop();
                timerDtmf.Start();
            }
        }

        private void timerDtmf_Tick(object sender, EventArgs e)
        {
            timerDtmf.Stop();
            Send("DTMF:" + dtmf);
            //XtraMessageBox.Show(dtmf.Replace(";", "\r\n"), "Otrzymano sygnały DTMF", MessageBoxButtons.OK, MessageBoxIcon.None);
            dtmf = "";
        }

        private void Instance_AccountStateChanged(int accountId, int accState)
        {
            lStatus.ResetSuperTip();
            lStatus.SuperTip = new DevExpress.Utils.SuperToolTip();
            lStatus.SuperTip.Items.Add(new ToolTipTitleItem());
            lStatus.SuperTip.Items.Add(new ToolTipItem());
            if (CallManager.Config.Accounts[CallManager.Config.DefaultAccountIndex].RegState == 200)
            {
				timerReRegister.Stop();
                lStatus.ItemAppearance.Normal.ForeColor = Color.Green;
                lStatus.Caption = "Zarejestrowany";
                (lStatus.SuperTip.Items[0] as ToolTipTitleItem).Text = "Pomyślnie zarejestrowano w usłudze VOiP.";
				if (this.InvokeRequired)
					this.Invoke((MethodInvoker)delegate
					{
						this.Icon = notifyIcon.Icon = Resources.phone_green_256;
					});
				else
					this.Icon = notifyIcon.Icon = Resources.phone_green_256;
            }
            else
            {
                lStatus.ItemAppearance.Normal.ForeColor = Color.Red;
                lStatus.Caption = "Niezarejestrowany";
                (lStatus.SuperTip.Items[0] as ToolTipTitleItem).Text = "Nieudana rejestracja w usłudze VOiP.";
                lMessages.Caption = CallManager.Config.Accounts[CallManager.Config.DefaultAccountIndex].RegState.ToString();
				timerMessages.Start();
				timerReRegister.Start();
				if (this.InvokeRequired)
					this.Invoke((MethodInvoker)delegate
					{
						this.Icon = notifyIcon.Icon = Resources.phone_red_256;
					});
				else
					this.Icon = notifyIcon.Icon = Resources.phone_red_256;
            }
            (lStatus.SuperTip.Items[1] as ToolTipItem).Text = "(" + CallManager.Config.Accounts[CallManager.Config.DefaultAccountIndex].RegState + ") " + Globals.GetVOiPMessage(CallManager.Config.Accounts[CallManager.Config.DefaultAccountIndex].RegState);
        }

		private void timerReRegister_Tick(object sender, EventArgs e)
		{
			if (CallManager.IsInitialized)
			{
				Registrar.unregisterAccounts();
				Registrar.registerAccounts();
			}
			else
				bbtnRegister_ItemClick(null, null);
		}

        private void CallManager_CallStateRefresh(int sessionId)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new DCallStateRefresh(OnCallStateChanged), new object[] { sessionId });
            }
            else
            {
                OnCallStateChanged(sessionId);
            }
        }

        public void Dial(string number)
        {
            Action method = new Action(() =>
            {
                if (CallManager.IsInitialized)
                {
                    if (CallManager.getNoCallsInState(EStateId.ACTIVE) + CallManager.getNoCallsInState(EStateId.INCOMING) > 0)
                    {
                        if (IsTransfer)
                            Transfer(teDialNumber.Text);
                        else
                        {
                            foreach (char ch in number)
                            {
                                CallManager.onUserDialDigit(CallManager.getCallInState(EStateId.ACTIVE).Session, ch.ToString(), EDtmfMode.DM_Outband);
                                Thread.Sleep(50);
                            }
                        }
                    }
                    else
                    {
                        string num = Globals.NormalizeTelNumber(number);
                        if (number.Length >= 9 && Settings.Prefix0)
                            CallManager.createOutboundCall("0" + num);
                        else
                            CallManager.createOutboundCall(num);
                    }
                }
                else
                {
                    lMessages.Caption = "Zarejestruj się.";
                    timerMessages.Start();
                }
            });

            if (this.InvokeRequired) this.Invoke((MethodInvoker)delegate { method(); });
            else method();
        }

        public void Transfer(string number)
        {
            Action method = new Action(() =>
            {
                string num = Globals.NormalizeTelNumber(number);
                if (number.Length >= 9 && Settings.Prefix0)
                    num = "0" + num;
                IStateMachine call;
                if (CallManager.getNoCallsInState(EStateId.ACTIVE) > 0)
                {
                    call = CallManager.getCallInState(EStateId.ACTIVE);
                }
                else
                {
                    call = CallManager.getCallInState(EStateId.INCOMING);
                }
                ICallProxyInterface proxy = StackProxy.createCallProxy();
                proxy.SessionId = call.Session;
                if (call.StateId == EStateId.ACTIVE)
                {
                    proxy.holdCall();
                    proxy.xferCall(num);
                }
                else
                    CallManager.onUserTransfer(call.Session, num);
            });

            if (this.InvokeRequired) this.Invoke((MethodInvoker)delegate { method(); });
            else method();
        }

        public void EndCall()
        {
            Action method = new Action(() =>
            {
                if (CallManager.getNoCallsInState(EStateId.ACTIVE) > 0)
                    CallManager.onUserRelease(CallManager.getCallInState(EStateId.ACTIVE).Session);

                else if (CallManager.getNoCallsInState(EStateId.ALERTING) > 0)
                    CallManager.onUserRelease(CallManager.getCallInState(EStateId.ALERTING).Session);
                else if (CallManager.getNoCallsInState(EStateId.INCOMING) > 0)
                    CallManager.onUserRelease(CallManager.getCallInState(EStateId.INCOMING).Session);
                else if (CallManager.getNoCallsInState(EStateId.HOLDING) > 0)
                    CallManager.onUserRelease(CallManager.getCallInState(EStateId.HOLDING).Session);
            });

            if (this.InvokeRequired) this.Invoke((MethodInvoker)delegate { method(); });
            else method();
        }

        public void AcceptCall()
        {
            Action method = new Action(() =>
               {
                   if (CallManager.getNoCallsInState(EStateId.INCOMING) > 0)
                       CallManager.onUserAnswer(CallManager.getCallInState(EStateId.INCOMING).Session);
               });

            if (this.InvokeRequired) this.Invoke((MethodInvoker)delegate { method(); });
            else method();
        }

        private void OnCallStateChanged(int sessionId)
        {
            IStateMachine call = CallManager.getCall(sessionId);
            //MessageBox.Show(call.Session.ToString() + ": " + call.StateId.ToString());
            if (call.StateId == EStateId.INCOMING)
            {
                try
                {
                    if (Settings.PlaySound)
                    {
                        //SetVolume(10);

                        ringtone.SoundLocation = Settings.Ringtone;
                        ringtone.PlayLooping();
                    }
                    else
                    {
                        //SetVolume(0);

                        StackProxy.setSoundDevice("0", "0");
                    }
                }
                catch { }
                try
                {
                    OnNewCall(sessionId);
                }
                catch { }

            }
            else if (call.StateId == EStateId.ALERTING)
            {
                OnNewCall(sessionId);
            }
            else if (call.StateId == EStateId.ACTIVE)
            {
                ringtone.Stop();
                timerCallDuration.Start();
                lTime.Visible = true;
                btnAnswerCall.Visible = false;
                lCallStatus.Text = "Rozmowa";
                tlpACButtons.SetColumn(btnEndCall, 0);
                tlpACButtons.SetColumnSpan(btnEndCall, 4);
                sesje[sessionId].Begin();
                Send("ACTIVECALL:" + sesje[sessionId].Number + ";" + sesje[sessionId].Incoming.ToString() + ";" + sesje[sessionId].Start.ToString() + ";" + sesje[sessionId].Duration.ToString());
            }
            else if (call.StateId == EStateId.NULL)
            {
                if (sesje.ContainsKey(sessionId))
                {
                    ringtone.Stop();
                    tabControll.SelectedTabPage = tabActiveCall;
                    sesje[sessionId].End();
                    timerCallDuration.Stop();
                    timerEndCall.Start();
                    lCallStatus.Text = "Rozmowa zakończona";
                    lCallStatus.BackColor = Color.Maroon;
                    lTime.BackColor = Color.Maroon;
                    btnAnswerCall.Visible = false;
                    btnEndCall.Visible = false;
                    btnTransferCall.Visible = false;
                    teDialNumber.Text = "";
                    Send("ENDCALL:" + sesje[sessionId].Number + ";" + sesje[sessionId].Incoming.ToString() + ";" + sesje[sessionId].Start.ToString() + ";" + sesje[sessionId].Duration.ToString());
                    Histories.Add(new History(sesje[sessionId]));
                    gvHistory.RefreshData();
                    sesje.Remove(sessionId);
                    dtmf = "";
                    IsTransfer = false;
                    //call.destroy();
                }
            }
        }

        private void OnNewCall(int sessionId)
        {
            try
            {
                timerEndCall.Stop();
                IStateMachine call = CallManager.getCall(sessionId);
                string number = Globals.NormalizeTelNumber(call.CallingNumber);
                if (sesje.ContainsKey(sessionId)) sesje.Remove(sessionId);
                sesje.Add(sessionId, new CallInfo(number, call.Incoming, sessionId));
                if (Globals.FindByNumber(number) != null)
                {
                    Contact con = Globals.FindByNumber(number);
                    lACFullName.Text = con.Fullname;
                    TelNumber tn = con.getTelByNumber(number);
                    lACNumber.Text = tn.Number;
                    lACTelType.Text = tn.TypeString + ":";
                    picAvatar.EditValue = con.Photo;
                }
                else
                {
                    if (call.CallingNumber == "")
                        lACFullName.Text = "Nieznany";
                    else
                        lACFullName.Text = number;
                    lACNumber.Text = "";
                    lACTelType.Text = "";
                    picAvatar.EditValue = Resources.phone_256;
                }
                if (call.Incoming)
                {
					CallSmall callSmall = new CallSmall(lACFullName.Text, lACNumber.Text);
					callSmall.Show(this);

                    tlpACButtons.SetColumnSpan(btnEndCall, 2);
                    tlpACButtons.SetColumn(btnEndCall, 2);
                    btnAnswerCall.Visible = btnEndCall.Visible = btnTransferCall.Visible = true;
                    btnAnswerCall.Enabled = true;
                }
                else
                {
                    btnAnswerCall.Visible = false;
                    tlpACButtons.SetColumn(btnEndCall, 0);
                    tlpACButtons.SetColumnSpan(btnEndCall, 4);
                    btnEndCall.Visible = true;
                    btnTransferCall.Visible = true;
                }
                btnAnswerCall.Visible = call.Incoming;
                btnEndCall.Visible = true;
                btnTransferCall.Visible = true;
                lCallStatus.Text = call.Incoming ? "Przychodzące" : "Łączenie";
                lTime.Text = "00:00";
                lTime.Visible = false;
                lCallStatus.BackColor = Color.Green;
                lTime.BackColor = Color.Green;
                tabActiveCall.PageVisible = true;
                tabControll.SelectedTabPage = tabActiveCall;
                Send("NEWCALL:" + number + ";" + call.Incoming.ToString() + ";" + DateTime.Now.ToString() + ";" + TimeSpan.Zero.ToString());
                dtmf = "";
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void timerEndCall_Tick(object sender, EventArgs e)
        {
            timerEndCall.Stop();
            tabControll.SelectedTabPage = tabHistory;
			tabActiveCall.PageVisible = false;

			Globals.SaveHistoryToFile(Globals.PathHistory);
        }
        #endregion

        #region Form
        private void Form1_Load(object sender, EventArgs e)
        {
            Globals.InitializeGlobals();

            teDialNumber.Text = "";

            if (File.Exists(Globals.PathSettings))
            {
                Settings.LoadSettings(Globals.PathSettings);
            }
            else
            {
                Settings.SaveSettings(Globals.PathSettings);
                Settings.LoadSettings(Globals.PathSettings);
            }

            gcContacts.DataSource = Contacts;
            gcHistory.DataSource = Histories;

            StartServer();
        }

        private void timerMessages_Tick(object sender, EventArgs e)
        {
            timerMessages.Stop();
            lMessages.Caption = "";
        }

        private void tabControll_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == tabHistory)
            {
                foreach (History h in Histories)
                {
                    h.Seen = true;
                }
                gvHistory.RefreshData();
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (Settings.AutoRegister)
            {
                bbtnRegister_ItemClick(null, null);
            }
        }

        private void bbiClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.Exit(new System.ComponentModel.CancelEventArgs(false));
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            else
            {
                StopServer();

                if (Config.Accounts.Count > 0)
					Registrar.unregisterAccounts();
                e.Cancel = false;
            }
        }

        private void bbtnSettings_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            tsmiShow.PerformClick();
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.FormClosed += settingsForm_FormClosed;
            settingsForm.Location = new Point(Location.X + 30, Location.Y + 10);
            settingsForm.Show();
        }

        void settingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (((SettingsForm)sender).saved)
            {
                lMessages.Caption = "Zapisano";
                timerMessages.Start();
                bbtnReset_ItemClick(null, null);
                bbtnRegister_ItemClick(null, null);
            }
        }
        #endregion

        #region Pipe
        private NamedPipeServer Server { get; set; }

        private void StartServer()
        {
            Server = new NamedPipeServer("BFS_" + Settings.PipeName);
            Server.OnReceivedMessage += Server_OnReceivedMessage;
            Server.Start();
        }

        void Server_OnReceivedMessage(object sender, BSF_VoIP.ReceivedMessageEventArgs e)
        {
            string text = e.Message.Trim();
            if (text.Contains(":"))
            {
                string command = text.Split(":".ToCharArray(), StringSplitOptions.None)[0];
                string args = text.Split(":".ToCharArray(), StringSplitOptions.None)[1];

                switch (command.ToUpper())
                {
                    case "CALL":
                        Dial(args);
                        break;
                    case "DTMF":
                        Dial(args);
                        break;
                    case "TRANSFER":
                        Transfer(args);
                        break;
                    case "ENDCALL":
                        EndCall();
                        break;
                    case "ACCEPT":
                        AcceptCall();
                        break;
                    case "TEST":
                        Send("TESTOK");
                        break;
                    case "RESTART":
                        Server.Stop();
                        Server.Start();
                        break;
                    case "USERNAME":
                        Server.Write("USERNAME:" + Settings.UserName);
                        break;
                }
            }
            else
            {
                switch (text.ToUpper())
                {
                    case "ENDCALL":
                        EndCall();
                        break;
                    case "ACCEPT":
                        AcceptCall();
                        break;
                    case "TEST":
                        Send("TESTOK");
                        break;
                    case "RESTART":
                        Server.Stop();
                        Server.Start();
                        break;
                    case "USERNAME":
                        Server.Write("USERNAME:" + Settings.UserName);
                        break;
                }
            }
        }

        private void Send(string text)
        {
            Server.Write(text);
        }

        private void StopServer()
        {
            if (Server != null)
                Server.Stop();
        }
        #endregion

        #region notify icon
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            tsmiShow.PerformClick();
        }

        private void tsmiShow_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.ShowInTaskbar = true;
        }

        private void tsmiHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        private void tsmiRestart_Click(object sender, EventArgs e)
        {
            bbtnReset.PerformClick();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit(new System.ComponentModel.CancelEventArgs(false));
        }

        private void tsmiCall_Click(object sender, EventArgs e)
        {
            tsmiCall.Text = "";
        }

        private void tsmiCall_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && tsmiCall.Text != "")
            {
                Dial(tsmiCall.Text);
                menuNotifyIcon.Hide();
            }
        }

        private void tsmiContacts_Click(object sender, EventArgs e)
        {
            tsmiShow.PerformClick();
            tabControll.SelectedTabPage = tabContacts;
        }

        private void tsmiHistory_Click(object sender, EventArgs e)
        {
            tsmiShow.PerformClick();
            tabControll.SelectedTabPage = tabHistory;
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            bbtnSettings.PerformClick();
        }

        private void menuNotifyIcon_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
			tsteKonto.Text = "Konto: " + Settings.UserName;
            tsmiCall.Text = "Zadzwoń";
        }
        #endregion

        private void bbtnReset_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Server.Stop();
            Server.Start();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (CallManager.getNoCallsInState(EStateId.HOLDING) == 0)
            {
                CallManager.onUserHoldRetrieve(CallManager.getCallInState(EStateId.ACTIVE).Session);
                CallManager.getCallInState(EStateId.ACTIVE).changeState(EStateId.HOLDING);
            }
            else
                CallManager.onUserHoldRetrieve(CallManager.getCallInState(EStateId.HOLDING).Session);
        }

        [DllImport("winmm.dll")]
        private static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        private static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        /// <summary>
        /// Returns volume from 0 to 10
        /// </summary>
        /// <returns>Volume from 0 to 10</returns>
        public static int GetVolume()
        {
            uint CurrVol = 0;
            waveOutGetVolume(IntPtr.Zero, out CurrVol);
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
            int volume = CalcVol / (ushort.MaxValue / 10);
            return volume;
        }

        /// <summary>
        /// Sets volume from 0 to 10
        /// </summary>
        /// <param name="volume">Volume from 0 to 10</param>
        public static void SetVolume(int volume)
        {
            int NewVolume = ((ushort.MaxValue / 10) * volume);
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
		}
    }
}
