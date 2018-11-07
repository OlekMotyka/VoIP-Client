using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sipek.Common;

namespace BFS_VoIP
{
	internal class AccountConfig : IAccount
	{
		private string accountName;
		private string displayName;
		private string domainName;
		private string hostName;
		private string id;
		private int index;
		private string password;
		private string proxyAdress;
		private int regState;
		private ETransportMode transportMode;
		private string userName;


		public string AccountName
		{
			get
			{
				return accountName;
			}
			set
			{
				accountName = value;
			}
		}

		public string DisplayName
		{
			get
			{
				return displayName;
			}
			set
			{
				displayName = value;
			}
		}

		public string DomainName
		{
			get
			{
				return domainName;
			}
			set
			{
				domainName = value;
			}
		}

		public string HostName
		{
			get
			{
				return hostName;
			}
			set
			{
				hostName = value;
			}
		}

		public string Id
		{
			get
			{
				return id;
			}
			set
			{
				id = value;
			}
		}

		public int Index
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
			}
		}

		public string Password
		{
			get
			{
				return password;
			}
			set
			{
				password = value;
			}
		}

		public string ProxyAddress
		{
			get
			{
				return proxyAdress;
			}
			set
			{
				proxyAdress = value;
			}
		}

		public int RegState
		{
			get
			{
				return regState;
			}
			set
			{
				regState = value;
			}
		}

		public ETransportMode TransportMode
		{
			get
			{
				return transportMode;
			}
			set
			{
				transportMode = value;
			}
		}

		public string UserName
		{
			get
			{
				return userName;
			}
			set
			{
				userName = value;
			}
		}
	}
}
