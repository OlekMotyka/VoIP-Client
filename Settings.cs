using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace BFS_VoIP
{
	public static class Settings
	{
		public static string UserName { get; set; }
		public static string Password { get; set; }
		public static string Domain { get; set; }
		public static string Host { get; set; }
		public static string Ringtone { get; set; }
		public static string PipeName { get; set; }

		public static int Port { get; set; }

		public static bool AutoRegister { get; set; }
		public static bool AutoAnswer { get; set; }
		public static bool DoNotDisturb { get; set; }
		public static bool Prefix0 { get; set; }
		public static bool PlaySound { get; set; }
		public static bool CallForm { get; set; }


		public static void LoadSettings(string Path)
		{
			if (Path == "")
			{
				Path = Globals.PathSettings;
			}
			try
			{
				XmlDocument Xml = new XmlDocument();
				Xml.Load(Path);

				UserName = Xml.SelectSingleNode("settings/UserName").InnerText;
				Password = Xml.SelectSingleNode("settings/Password").InnerText;
				Domain = Xml.SelectSingleNode("settings/Domain").InnerText;
				Host = Xml.SelectSingleNode("settings/Host").InnerText;
				Port = int.Parse(Xml.SelectSingleNode("settings/Port").InnerText);
				AutoRegister = bool.Parse(Xml.SelectSingleNode("settings/AutoRegister").InnerText);
				AutoAnswer = bool.Parse(Xml.SelectSingleNode("settings/AutoAnswer").InnerText);
				DoNotDisturb = bool.Parse(Xml.SelectSingleNode("settings/DoNotDisturb").InnerText);
				Prefix0 = bool.Parse(Xml.SelectSingleNode("settings/Prefix0").InnerText);
				CallForm = bool.Parse(Xml.SelectSingleNode("settings/CallForm").InnerText);
				PlaySound = bool.Parse(Xml.SelectSingleNode("settings/PlaySound").InnerText);
				Ringtone = Xml.SelectSingleNode("settings/Ringtone").InnerText;
				PipeName = Xml.SelectSingleNode("settings/PipeName").InnerText;
			}
			catch
			{
				UserName = Password = Domain = Host = PipeName = "";
				AutoRegister = AutoAnswer = DoNotDisturb = DoNotDisturb = false;
				Ringtone = "ring.wav";
				Port = 5060;
				SaveSettings("");
				LoadSettings("");
			}
		}

		public static void SaveSettings(string Path)
		{
			XmlDocument Xml = new XmlDocument();
			Xml.AppendChild(Xml.CreateNode(XmlNodeType.Element, "settings", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "UserName", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "Password", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "Domain", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "Host", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "Port", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "AutoRegister", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "AutoAnswer", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "DoNotDisturb", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "Prefix0", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "CallForm", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "PlaySound", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "Ringtone", ""));
			Xml.SelectSingleNode("settings").AppendChild(Xml.CreateNode(XmlNodeType.Element, "PipeName", ""));

			Xml.SelectSingleNode("settings/UserName").InnerText = UserName;
			Xml.SelectSingleNode("settings/Password").InnerText = Password;
			Xml.SelectSingleNode("settings/Domain").InnerText = Domain;
			Xml.SelectSingleNode("settings/Host").InnerText = Host;
			Xml.SelectSingleNode("settings/Port").InnerText = Port.ToString();
			Xml.SelectSingleNode("settings/AutoRegister").InnerText = AutoRegister.ToString();
			Xml.SelectSingleNode("settings/AutoAnswer").InnerText = AutoAnswer.ToString();
			Xml.SelectSingleNode("settings/DoNotDisturb").InnerText = DoNotDisturb.ToString();
			Xml.SelectSingleNode("settings/Prefix0").InnerText = Prefix0.ToString();
			Xml.SelectSingleNode("settings/CallForm").InnerText = CallForm.ToString();
			Xml.SelectSingleNode("settings/PlaySound").InnerText = PlaySound.ToString();
			Xml.SelectSingleNode("settings/Ringtone").InnerText = Ringtone;
			Xml.SelectSingleNode("settings/PipeName").InnerText = PipeName;

			if (Path == "")
			{
				Xml.Save(Globals.PathSettings);
			}
			else
			{
				Xml.Save(Path);
			}
		}
	}
}
