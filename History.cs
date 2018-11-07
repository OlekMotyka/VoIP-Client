using BFS_VoIP.Properties;
using Sipek.Common;
using System;
using System.Drawing;

namespace BFS_VoIP
{
	public class History
	{
		public string Number { get; set; }
		public TimeSpan Duration { get; set; }
		public DateTime Start { get; set; }
		public bool Incoming { get; set; }
		public bool Seen { get; set; }

		public History(IStateMachine call)
		{
			Duration = call.Duration;
			Number = call.CallingNumber;
			Start = DateTime.Now - Duration;
			Incoming = call.Incoming;
			Seen = false;
		}

		public History(CallInfo call)
		{
			Number = call.Number;
			Incoming = call.Incoming;
			Duration = call.Duration;
			Start = call.Start;
		}

		public History(string p)
		{
			if (p != "")
			{
				string[] args = p.Split(";".ToCharArray(), StringSplitOptions.None);
				Number = args[0];
				Duration = TimeSpan.Parse(args[1]);
				Start = DateTime.Parse(args[2]);
				Incoming = bool.Parse(args[3]);
				Seen = true;
			}
		}

		public bool Answered
		{
			get
			{
				if (Duration == TimeSpan.Zero)
					return false;
				else
					return true;
			}
		}

		private Contact Contact
		{
			get
			{
				return Globals.FindByNumber(Number);
			}
		}

		public byte[] Picture
		{
			get
			{
				if (Contact != null)
					return Contact.Photo;
				else
					return null;
			}
		}

		public string CallInfo
		{
			get
			{
				if (Contact != null)
					return Contact.Fullname + "\r\n" + Number + "\r\n" + Time;
				else
					return Number + "\r\n" + Time;
			}
		}

		public string Time
		{
			get
			{
				return Globals.getDateString(Start) + " (" + Duration.ToString(Globals.TimeSpanFormat_MS) + ")";
			}
		}

		public Image State
		{
			get
			{
				if (Incoming && Answered)
					return Resources.arrow_down_green_16;
				else if (Incoming && !Answered)
					return Resources.arrow_down_red_16;
				else if (!Incoming && Answered)
					return Resources.arrow_up_green_16;
				else if (!Incoming && !Answered)
					return Resources.arrow_up_red_16;
				else return Resources.empty16;
			}
		}

		public string ToFileString()
		{
			return Number + ";" + Duration.ToString() + ";" + Start.ToString() + ";" + Incoming.ToString();
		}
	}

	public class CallInfo
	{
		public string Number { get; set; }
		public bool Incoming { get; set; }
		public int Session { get; set; }
		public DateTime Start { get; set; }
		public DateTime Stop { get; set; }

		public CallInfo(string number, bool incoming, int session)
		{
			Number = number;
			Incoming = incoming;
			Session = session;
		}

		public TimeSpan Duration
		{
			get { return Stop - Start; }
		}

		public void Begin()
		{
			Start = DateTime.Now;
		}

		public void End()
		{
			Stop = DateTime.Now;
			if (Start == DateTime.MinValue)
			{
				Start = Stop;
			}
		}
	}
}
