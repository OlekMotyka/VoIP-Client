using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace BFS_VoIP
{
	public static class Globals
	{
		public static List<Contact> Contacts { get; private set; }
		public static List<History> Histories { get; private set; }

		public static string TimeSpanFormat_MS = @"mm\:ss";
		public static string TimeSpanFormat_hm = @"hh\:mm";
		public static string DateTimeFormat_dMhm = @"d MMM HH\:mm";

		public static string PathHistory = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\History.csv");
		public static string PathSettings = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\settings.xml");
		public static string PathContacts = string.Concat(Path.GetDirectoryName(Application.ExecutablePath), "\\Contacts.vcf");


		public static bool InitializeGlobals()
		{
			try
			{
				Contacts = new List<Contact>();
				if (File.Exists(PathContacts))
				{
					StreamReader stream = new StreamReader(PathContacts);
					Contacts.AddRange(Contact.getContactsFromvCard(stream.ReadToEnd()));
					stream.Close();
				}

				Histories = new List<History>();
				if (File.Exists(PathHistory))
				{
					StreamReader reader = new StreamReader(PathHistory);
					while (!reader.EndOfStream)
					{
						Histories.Add(new History(reader.ReadLine()));
					}
					reader.Close();
				}

				return true;
			}
			catch { return false; }
		}

		public static Contact FindByNumber(string number)
		{
			string num = NormalizeTelNumber(number);
			foreach (Contact c in Contacts)
				if (c.getTelByNumber(number) != null)
					return c;
			return null;
		}

		public static string getDateString(DateTime dt)
		{
			if (dt.Date == DateTime.Now.Date)
			{
				return dt.TimeOfDay.ToString(TimeSpanFormat_hm);
			}
			else if ((DateTime.Now.Date - dt.Date).Days == 1)
			{
				return "Wczoraj " + dt.TimeOfDay.ToString(TimeSpanFormat_hm);
			}
			else if ((DateTime.Now.Date - dt.Date).Days > 1 && (DateTime.Now.Date - dt.Date).Days < 7)
			{
				string dzien = "";
				switch (dt.DayOfWeek)
				{
					case DayOfWeek.Monday:
						dzien = "Poniedziałek";
						break;
					case DayOfWeek.Tuesday:
						dzien = "Wtorek";
						break;
					case DayOfWeek.Wednesday:
						dzien = "Środa";
						break;
					case DayOfWeek.Thursday:
						dzien = "Czwartek";
						break;
					case DayOfWeek.Friday:
						dzien = "Piątek";
						break;
					case DayOfWeek.Saturday:
						dzien = "Sobota";
						break;
					case DayOfWeek.Sunday:
						dzien = "Niedziela";
						break;
				}
				return dzien + " " + dt.TimeOfDay.ToString(TimeSpanFormat_hm);
			}
			else
			{
				return dt.ToString(DateTimeFormat_dMhm);
			}
		}

		public static void SaveHistoryToFile(string path)
		{
			File.Delete(path);
			StreamWriter writer = new StreamWriter(path, true);
			foreach (History h in Histories)
			{
				writer.WriteLine(h.ToFileString());
			}
			writer.Close();
		}

		public static string NormalizeTelNumber(string num)
		{
			if (num.Length > 0)
			{
				string res = num;
				if (res.StartsWith("0")) res = res.Remove(0, 1);
				return res.Trim().Replace(" ", "").Replace("(", "").Replace(")", "").Replace("+", "");
			}
			else return "";
		}

		public static string GetVOiPMessage(int i)
		{
			switch (i)
			{
				case 100:
					return "Sprawdzenie dostępności";
				case 180:
					return "Dzwonienie";
				case 181:
					return "Połączenie jest przełączone";
				case 182:
					return "Kolejkownie";
				case 183:
					return "Przebieg sesji";
				case 200:
					return "OK";
				case 202:
					return "OK";
				case 300:
					return "Wiele wyborów";
				case 301:
					return "Przeniesiony na stałe";
				case 302:
					return "Przeniesiony tymczasowo";
				case 305:
					return "Użyj Proxy";
				case 380:
					return "Usługa zastępcza";
				case 400:
					return "Błędne żądanie";
				case 401:
					return "Nieautoryzowane: Używne tylko przez rejestratorów.";
				case 402:
					return "Wymagana opłata";
				case 403:
					return "Niedozwolone";
				case 404:
					return "Użytkownik nie znaleziony";
				case 405:
					return "Metoda nie jest dozwolona";
				case 406:
					return "Nie do przyjęcia";
				case 407:
					return "Konieczna autentyfikacja pośrednika (proxy)";
				case 408:
					return "Żadanie przekroczyło limit czasu: użytkownik nie został znaleziony w czasie";
				case 410:
					return "Nieobecny: Użytkownik istniał w przeszłości, ale nie jest już tutaj dostępny";
				case 413:
					return "Jednostka żądania zbyt duża";
				case 414:
					return "URI żądania zbyt długie";
				case 415:
					return "Nie obsługiwany typ nośnika";
				case 416:
					return "Nie obsługiwany schemat URI";
				case 420:
					return "Niewłaściwe rozszerzenie: użyte błędne rozszerzenie protokołu SIP, nie rozpoznane przez serwer";
				case 421:
					return "Rozszerzenie konieczne";
				case 423:
					return "Przerwa zbyt krótka";
				case 480:
					return "Czasowo niedostępny";
				case 481:
					return "Połączenie / tranzakcja nie istnieje";
				case 482:
					return "Wykryta pętla";
				case 483:
					return "Zbyt wiele przeskoków";
				case 484:
					return "Niekompletny adres";
				case 485:
					return "Niejasne";
				case 486:
					return "Zajęte";
				case 487:
					return "Żądanie sfinalizowane";
				case 488:
					return "Nie do przyjęcia tutaj";
				case 491:
					return "Żądanie oczekujące na obsługę";
				case 493:
					return "Nie do rozszyfrowania: nie można rozszyfrować treści S/MiME";
				case 500:
					return "Wewnętrzny błąd serwera";
				case 501:
					return "Nie zrealizowane: Metoda żądania SIP nie jest tutaj realizowana";
				case 502:
					return "Niewłaściwa bramka";
				case 503:
					return "Usługa nie jest dostępna";
				case 504:
					return "Przeterminowanie serwera";
				case 505:
					return "Wersja nie obsługiwana: serwer nie współpracuje z tą wersją protokołu SIP";
				case 513:
					return "Zbyt duża wiadomość";
				case 600:
					return "Zajęte wszędzie";
				case 603:
					return "Odrzuć";
				case 604:
					return "Nigdzie nie istnieje";
				case 606:
					return "Nie do przyjęcia";
				default:
					return "";
			}
		}
	}
}
