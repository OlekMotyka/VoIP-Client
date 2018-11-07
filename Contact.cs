using System;
using System.Collections.Generic;
using System.Linq;

namespace BFS_VoIP
{
	public enum TelType { PREF, WORK, HOME, VOICE, FAX, MSG, CELL, PAGER, BBS, MODEM, CAR, ISDN, VIDEO, OTHER }
	public enum PlaceType { domestic, international, postal, parcel, home, work }
	public enum EmailType { aol, applelink, attmail, cis, eworld, internet, ibmmail, mcimail, powershare, prodigy, tlx, x400 }

	public class Address
	{
		public string Locality { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public bool Prefered { get; set; }
		public List<PlaceType> PlaceTypes = new List<PlaceType>();

		public Address()
		{
			PlaceTypes = new List<PlaceType>();
			Locality = "";
			City = "";
			this.Region = "";
			PostalCode = "";
			Country = "";
			Prefered = false;
		}

		public Address(string text, bool pref)
			: this()
		{
			Prefered = pref;
			Locality = text;
		}

		public Address(string vCardLine)
			: this()
		{
			if (vCardLine.StartsWith("ADR"))
			{
				string param = vCardLine.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[0].Replace(",", ";");

				string[] pars = param.Split(";=".ToArray(), StringSplitOptions.RemoveEmptyEntries);

				foreach (string p in pars)
				{
					switch (p.ToLower())
					{
						case "adr": break;
						case "type": break;
						case "domestic": PlaceTypes.Add(PlaceType.domestic); break;
						case "international": PlaceTypes.Add(PlaceType.international); break;
						case "postal": PlaceTypes.Add(PlaceType.postal); break;
						case "parcel": PlaceTypes.Add(PlaceType.parcel); break;
						case "home": PlaceTypes.Add(PlaceType.home); break;
						case "work": PlaceTypes.Add(PlaceType.work); break;
						case "pref": Prefered = true; break;
					}
				}
				string[] atrs = vCardLine.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1].Split(";".ToArray(), StringSplitOptions.None);
				Locality = atrs.Length > 2 ? atrs[2] : "";
				City = atrs.Length > 4 ? atrs[4] : "";
				Region = atrs.Length > 5 ? atrs[5] : "";
				PostalCode = atrs.Length > 6 ? atrs[6] : "";
				Country = atrs.Length > 7 ? atrs[7] : "";
			}
		}

		public override string ToString()
		{
			return Locality + "\r\n" + PostalCode + " " + City + "\r\n" + Region + " " + Country;
		}

		public string getvCardString
		{
			get
			{
				string res = "ADR";
				if (PlaceTypes.Count > 0)
					res += ";TYPE=" + String.Join(",", PlaceTypes.ToArray());
				res += ":;;" + Locality + ";" + City + ";" + Region + ";" + PostalCode + ";" + Country + ";";
				return res;
			}
		}

		public PlaceType Type
		{
			get
			{
				if (PlaceTypes != null && PlaceTypes.Count > 0)
				{
					if (PlaceTypes.Contains(PlaceType.home))
						return PlaceType.home;
					else if (PlaceTypes.Contains(PlaceType.work))
						return PlaceType.work;
					else return PlaceType.home;
				}
				else return PlaceType.home;
			}
		}

		public string TypeString
		{
			get
			{
				switch (Type)
				{
					case PlaceType.home:
						return "Dom";
					case PlaceType.work:
						return "Praca";
					default:
						return "Dom";
				}
			}
			set
			{
				switch (value)
				{
					case "Dom":
						PlaceTypes.Clear();
						PlaceTypes.Add(PlaceType.home);
						break;
					case "Praca":
						PlaceTypes.Clear();
						PlaceTypes.Add(PlaceType.work);
						break;
						
				}
			}
		}
	}

	public class TelNumber
	{
		public string Number { get; set; }
		public bool Prefered { get; set; }
		public List<TelType> TelTypes { get; private set; }

		public TelNumber(string number, TelType type)
		{
			Number = number;
			TelTypes = new List<TelType>();
			TelTypes.Add(type);
		}

		public TelNumber(string number, TelType[] types)
		{
			Number = number;
			TelTypes = new List<TelType>();
			TelTypes.AddRange(types);
		}

		public TelNumber(string vCardLine)
		{
			if (vCardLine.StartsWith("TEL"))
			{
				string param = vCardLine.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[0].Replace(",", ";");

				string[] pars = param.Split(";=".ToArray(), StringSplitOptions.RemoveEmptyEntries);

				foreach (string p in pars)
				{
					switch (p.ToLower())
					{
						case "TEL": break;
						case "TYPE": break;
						case "WORK": TelTypes.Add(TelType.WORK); break;
						case "HOME": TelTypes.Add(TelType.HOME); break;
						case "VOICE": TelTypes.Add(TelType.VOICE); break;
						case "FAX": TelTypes.Add(TelType.FAX); break;
						case "MSG": TelTypes.Add(TelType.MSG); break;
						case "CELL": TelTypes.Add(TelType.CELL); break;
						case "PAGER": TelTypes.Add(TelType.PAGER); break;
						case "BBS": TelTypes.Add(TelType.BBS); break;
						case "MODEM": TelTypes.Add(TelType.MODEM); break;
						case "CAR": TelTypes.Add(TelType.CAR); break;
						case "ISDN": TelTypes.Add(TelType.ISDN); break;
						case "VIDEO": TelTypes.Add(TelType.VIDEO); break;
						case "PREF": Prefered = true; break;
					}
				}
				Number = vCardLine.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", "").Replace("-", "").Replace("_", "").Replace("(", "").Replace(")", "");
			}
		}

		public TelType Type
		{
			get
			{
				if (TelTypes != null && TelTypes.Count > 0)
				{
					if (TelTypes.Contains(TelType.CELL))
						return TelType.CELL;
					else if (TelTypes.Contains(TelType.WORK))
						return TelType.WORK;
					else if (TelTypes.Contains(TelType.CAR))
						return TelType.CAR;
					else if (TelTypes.Contains(TelType.HOME))
						return TelType.HOME;
					else return TelType.OTHER;
				}
				else return TelType.OTHER;
			}
		}

		public string getvCardString
		{
			get
			{
				string res = "TEL";
				if (TelTypes != null && TelTypes.Count > 0)
					res += ";TYPE=" + String.Join(",", TelTypes.ToArray());
				res += ":" + Number;
				return res;
			}
		}

		public string TypeString
		{
			get
			{
				switch (Type)
				{
					case TelType.CELL: return "Komórka";
					case TelType.WORK: return "Praca";
					case TelType.CAR: return "Samochód";
					case TelType.HOME: return "Dom";
					case TelType.OTHER: return "Inne";
				}
				return "";
			}
			set
			{
				if (TelTypes == null)
					TelTypes = new List<TelType>();
				switch (value)
				{
					case "Komórka":
						TelTypes.Clear();
						TelTypes.Add(TelType.CELL);
						break;
					case "Praca":
						TelTypes.Clear();
						TelTypes.Add(TelType.WORK);
						break;
					case "Samochód":
						TelTypes.Clear();
						TelTypes.Add(TelType.CAR);
						break;
					case "Dom":
						TelTypes.Clear();
						TelTypes.Add(TelType.HOME);
						break;
					case "Inne":
						TelTypes.Clear();
						TelTypes.Add(TelType.OTHER);
						break;
				}
			}
		}

		public string FormatedString
		{
			get
			{
				return TypeString + ": " + Number;
			}
		}

	}

	public class Email
	{
		public string Address { get; set; }
		public List<EmailType> EmailTypes { get; set; }
		public bool Prefered { get; set; }

		public Email(string address, EmailType t)
		{
			Address = address;
			EmailTypes = new List<EmailType>();
			EmailTypes.Add(t);

		}

		public Email(string vCardLine)
		{
			if (vCardLine.StartsWith("EMAIL"))
			{
				EmailTypes = new List<EmailType>();
				string param = vCardLine.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[0].Replace(",", ";");

				string[] pars = param.Split(";=".ToArray(), StringSplitOptions.RemoveEmptyEntries);

				foreach (string p in pars)
				{
					switch (p.ToLower())
					{
						case "adr": break;
						case "type": break;
						case "aol": EmailTypes.Add(EmailType.aol); break;
						case "applelink": EmailTypes.Add(EmailType.applelink); break;
						case "attmail": EmailTypes.Add(EmailType.attmail); break;
						case "cis": EmailTypes.Add(EmailType.cis); break;
						case "eworld": EmailTypes.Add(EmailType.eworld); break;
						case "internet": EmailTypes.Add(EmailType.internet); break;
						case "ibmmail": EmailTypes.Add(EmailType.ibmmail); break;
						case "mcimail": EmailTypes.Add(EmailType.mcimail); break;
						case "powershare": EmailTypes.Add(EmailType.powershare); break;
						case "prodigy": EmailTypes.Add(EmailType.prodigy); break;
						case "tlx": EmailTypes.Add(EmailType.tlx); break;
						case "x400": EmailTypes.Add(EmailType.x400); break;
						case "pref": Prefered = true; break;
					}
				}
				Address = vCardLine.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
			}
		}
	}

	public class Contact
	{
		public List<TelNumber> Numbers { get; set; }
		public List<Address> Addresses { get; set; }
		public List<Email> Emails { get; set; }

		public string Fullname { get; set; }
		public List<String> Urls {get; set; }
		public DateTime BirthDay { get; set; }
		public string Note { get; set; }
		public string Organization { get; set; }
		public string RoleInOrg { get; set; }
		public string JobTitle { get; set; }
		public string TimeZone { get; set; }
		public DateTime Revision { get; set; }
		public byte[] Photo { get; set; }

		public Contact()
		{
			Numbers = new List<TelNumber>();
			Addresses = new List<Address>();
			Emails = new List<Email>();
			Urls = new List<string>();
			Fullname = "";
			BirthDay = new DateTime();
			Note = "";
			Organization = "";
			RoleInOrg = "";
			JobTitle = "";
			TimeZone = "";
			Revision = new DateTime();
		}

		public Contact(Contact con)
		{
			Numbers = new List<TelNumber>();
			Numbers.AddRange(con.Numbers);
			Addresses = new List<Address>();
			Addresses.AddRange(con.Addresses);
			Emails = new List<Email>();
			Emails.AddRange(con.Emails);
			Urls = new List<string>();
			Urls.AddRange(con.Urls);
			Fullname = con.Fullname;
			BirthDay = con.BirthDay;
			Note = con.Note;
			Organization = con.Organization;
			RoleInOrg = con.RoleInOrg;
			JobTitle = con.JobTitle;
			TimeZone = con.TimeZone;
			Revision = con.Revision;
			Photo = con.Photo;
		}

		public Contact(string fullname, string number)
			: this()
		{
			Fullname = fullname;
			Numbers.Add(new TelNumber(number, TelType.CELL));
		}

		private Contact(string SinglevCard)
			: this()
		{
			string[] lines = SinglevCard.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

			List<string> ss = new List<string>();
			foreach (string line in lines)
			{
				if (line.Contains(":")) ss.Add(line);
				else
					ss[ss.Count-1] = ss[ss.Count-1] + "\r\n" + line;
			}

			lines = ss.ToArray();

			foreach (string line in lines)
			{
				if (line.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries).Length > 1)
				{
					string param = line.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[0];
					string atr = line.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1];
					switch (param.Split(new char[] { ';' }, StringSplitOptions.None)[0])
					{
						case "BEGIN":
							break;
						case "VERSION":
							break;
						case "N":
							break;
						case "FN":
							Fullname = atr;
							break;
						case "ADR":
							Addresses.Add(new Address(line));
							break;
						case "BDAY":
							BirthDay = DateTime.Parse(atr);
							break;
						case "EMAIL":
							Emails.Add(new Email(line));
							break;
						case "NOTE":
							Note = atr;
							break;
						case "ORG":
							Organization = atr;
							break;
						case "REV":
							Revision = DateTime.Parse(atr);
							break;
						case "ROLE":
							RoleInOrg = atr;
							break;
						case "TEL":
							Numbers.Add(new TelNumber(line));
							break;
						case "TITLE":
							JobTitle = atr;
							break;
						case "TZ":
							TimeZone = atr;
							break;
						case "URL":
							Urls.Add(atr);
							break;
						case "PHOTO":
							Photo = getPhoto(line);
							break;
					}
				}
			}
		}

		public static Contact[] getContactsFromvCard(string vCard)
		{
			List<Contact> contacts = new List<Contact>();
			string[] vCards = vCard.Split(new string[] { "BEGIN:VCARD\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < vCards.Length; i++)
			{
				contacts.Add(new Contact("BEGIN:VCARD\r\n" + vCards[i]));
			}
			return contacts.ToArray();
		}

		public TelNumber getTelByNumber(string number)
		{
			foreach (TelNumber t in Numbers)
			{
				if (number.Length >= 9 && t.Number.Substring(number.Length - 9) == number.Substring(number.Length - 9))
				{
					return t;
				}
				else if (number.Length < 9 && t.Number == number)
				{
					return t;
				}
			}
			return null;
		}

		public string ShortString
		{
			get
			{
				string numbers = "";
				foreach (TelNumber tel in Numbers)
				{
					numbers += "\r\n" + tel.FormatedString;
				}
				return Fullname + numbers;
			}
		}

		public string getVCard()
		{
			string result = "BEGIN:VCARD\r\nVERSION:3.0\r\n";
			result += getNamevCard() + "\r\n"; //N:
			result += "FN:" + Fullname + "\r\n"; //FN:

			foreach (var ad in Addresses)	//ADR:
			{
				result += ad.getvCardString + "\r\n";
			}

			foreach (var em in Emails)	//EMAIL:
			{
				result += "EMAIL:" + em.Address + "\r\n";
			}

			foreach (var tel in Numbers)	//TEL:
			{
				result += tel.getvCardString + "\r\n";
			}

			foreach (string url in Urls)	//URL
			{
				result += "URL:" + url + "\r\n";
			}

			result += BirthDay.Year != 1 ? "BDAY:" + BirthDay.ToShortDateString() + "\r\n" : "";
			result += Note != "" ? "NOTE:" + Note + "\r\n" : "";
			result += Organization != "" ? "ORG:" + Organization + "\r\n" : "";
			result += RoleInOrg != "" ? "ROLE:" + RoleInOrg + "\r\n" : "";
			result += JobTitle != "" ? "TITLE:" + JobTitle + "\r\n" : "";
			result += TimeZone != "" ? "TZ:" + TimeZone + "\r\n" : "";
			result += Revision.Year == 1 ? "REV:" + Revision.ToString() + "\r\n" : "";
			result += Photo != null ? getImagevCard() : "";
			result += "\r\n";

			result += "END:VCARD\r\n";
			return result;
		}

		private string getNamevCard()
		{
			string[] name = Fullname.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			string res = "N:";
			switch (name.Length)
			{
				case 1:
					res += name[0] + ";;;;";
					break;
				case 2:
					res += name[1] + ";" + name[0] + ";;;";
					break;
				case 3:
					res += name[2] + ";" + name[0] + ";" + name[1] + ";;";
					break;
				case 4:
					res += name[3] + ";" + name[1] + ";" + name[2] + ";" + name[0] + ";";
					break;
				case 5:
					res += name[3] + ";" + name[1] + ";" + name[2] + ";" + name[0] + ";" + name[4];
					break;
			}
			return res;
		}

		private string getImagevCard()
		{
			string res = "PHOTO;ENCODING=BASE64;TYPE=JPEG:";
			res += Convert.ToBase64String(Photo, Base64FormattingOptions.InsertLineBreaks);

			return res;
		}

		private byte[] getPhoto(string vCardLine)
		{
			return Convert.FromBase64String(vCardLine.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries)[1]);
		}
	}
}
