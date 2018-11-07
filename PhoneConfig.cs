using Sipek.Common;
using System;
using System.Collections.Generic;

namespace BFS_VoIP
{
	internal class PhoneConfig : IConfiguratorInterface
	{
		private List<IAccount> _acclist = new List<IAccount>();

		internal PhoneConfig()
		{

		}

		public List<IAccount> Accounts
		{
			get { return _acclist; }
		}

		public bool AAFlag { get; set; }

		public bool CFBFlag { get; set; }

		public string CFBNumber { get; set; }

		public bool CFNRFlag { get; set; }

		public string CFNRNumber { get; set; }

		public bool CFUFlag { get; set; }

		public string CFUNumber { get; set; }

		public List<string> CodecList { get; set; }

		public bool DNDFlag { get; set; }

		public int DefaultAccountIndex { get; set; }

		public bool IsNull
		{
			get { return false; }
		}

		public bool PublishEnabled { get; set; }

		public int SIPPort { get; set; }

		public void Save()
		{
			throw new NotImplementedException();
		}
	}
}
