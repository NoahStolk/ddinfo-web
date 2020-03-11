using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Leaderboards.History;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LeaderboardJsonIdFixer
{
	public static class Program
	{
		public static Dictionary<string, int> nameTable = new Dictionary<string, int>
		{
			{ "bowsr", 1135 },
			{ "Sojk", 229 },
			{ "k0k0sMJÖLk [38th]", 229 },
			{ "Bintr", 148788 },
			{ "DraQu", 82891 }, // Not sure why his Id is so high.
			{ "twitch.tv/draqu_", 82891 },
			{ "DraQu^WR", 82891 },
			{ "weaksauce13", 112 },
			{ "Weaksauce", 112 },
			{ "weaksauce", 112 },
			{ "b0necarver", -1 }, // No idea, but we need this for the world record graph.
			{ "m4ttbush", 1 },
			{ "A.N.T.4", 1677 },
			{ "? ANT4", 1677 },
			{ "ANT4", 1677 },
			{ "pocket", 116704 },
			{ "6,1742 ms [S1] Jump", 116704 },
			{ "P0cKeTFuLL", 116704 },
			{ "sexually reoriented", 116704 },
			{ "Philton T.", 116704 },
			{ "noahstolk.com/gg.webm", 116704 },
			{ "princess", 116704 },
			{ "KrosisOne", 1545 },
			{ "K r o s i s", 1545 },
			{ "Cookle", 93991 },
			{ "cookie the #11 dd boy", 93991 },
			{ "Cookiez", 93991 },
			{ "cokie", 93991 },
			{ "Cokie", 93991 },
			{ "devildaggers player93544", 93991 },
			{ "cokie301", 93991 },
			{ "C0okieZ", 93991 },
			{ "Co0kie", 93991 },
			{ "cokiee", 93991 },
			{ "cookle", 93991 },
			{ "C0okl3e", 93991 },
			{ "big brain", 93991 },
			{ "cookie", 93991 },
			{ "xvlv", 21854 },
			{ "Kaijt", 438 },
			{ "aircawn", 2316 },
			{ "sjorsbw", 10210 },
			{ "Chupacabra", 118832 },
			{ "aiunzovovor", 3294 },
			{ "bugg", 362 },
			{ "buggie", 362 },
			{ "kiplet", 362 },
			{ "kickflip", 362 },
			{ "succ4bucc", 362 },
			{ "kimeemaru", 362 },
			{ "Kimeemaru", 362 },
			{ "kankp", 362 },
			{ "waggysaggy", 362 },
			{ "napkin", 362 },
			{ "Power Squid", 362 },
			{ "yuh", 362 },
			{ "3 centipedes isnt enough", 362 },
			{ "Prophet Skeram", 362 },
			{ "Organ Mortar", 362 },
			{ "atro", 362 },
			{ "muscle moms anonymous", 362 },
			{ "Mummy Napkin", 362 },
			{ "Myrmeleon", 2076 },
			{ "ohsnaplol", 1029 },
			{ "pmcc", 2144 },
			{ "kudemizu", 2049 },
			{ "", 999999 },
			{ "L0s Blizt0r", 27510 },
			{ "Absolutely Not a Terrorist :))))", 27510 },
			{ "Moz", 4795 },
			{ "Blue53", 10045 },
			{ "oxysofts", 8806 },
			{ "Stephanstein", 10098 },
			{ "n2k", 23781 },
			{ "Hahmo", 48687 },
			{ "glum", 88424 },
			{ "ryan", 88424 },
			{ "moo", 88424 },
			{ "ggggglum", 88424 },
			{ "pull chord to inflate ego", 88424 },
			{ "cg_weaponBar 0/1/2/3/4", 88424 },
			{ "glu?m", 88424 },
			{ "glμm", 88424 },
			{ "gl�m", 88424 },
			{ "glum`", 88424 },
			{ "jay", 105641 },
			{ "kraken~?", 19658 },
			{ "krak~?", 19658 },
			{ "krak~", 19658 },
			{ "krak~๑ﭥ", 19658 },
			{ "Satanic", 148518 },
			{ "satan", 148518 },
			{ "Dillon", 148518 },
			{ "Cobajj", 5757 },
			{ "bartholomov cubbins", 75575 },
			{ "mrs doubtfire", 75575 },
			{ "Dkazyo", 105874 },
			{ "Pritster", 115431 },
			{ "CCT", 58654 },
			{ "wen n doubt blame curry", 81895 },
			{ "zealotsthename", 81895 },
			{ "zealotsthename † 狂热者", 81895 },
			{ "Emberstone", 82980 },
			{ "gkubiski", 65560 },
			{ "g_kub", 65560 },
			{ "Littlebert", 6425 },
			{ "ThePassiveDada", 21059 },
			{ "DanielJMus", 8561 },
			{ "ancient_geometrist", 69737 },
			{ "playthis", 66169 },
			{ "bob from food use", 117558 },
			{ "El Twig", 121754 },
			{ "X-ray", 5838 },
			{ "Faad3e_", 68151 },
			{ "LolVeR", 65193 },
			{ "LoIVeR", 65193 },
			{ "Do you like Ghostpedes?", 11356 },
			{ "Esty was here", 48348 },
			{ "Zirtonic", 113530 },
			{ "pagedMov", 65617 },
			{ "pagedMov lft", 65617 },
			{ "Evendium", 22448 },
			{ "poogi", 49387 },
			{ "hikikon", 2596 },
			{ "gLad", 134802 },
			{ "lvl100WIZARD", 87056 },
			{ "MisterToast", 20660 },
			{ "Lady Hawke", 10217 },
			{ "Arizart", 276 },
			{ "JoeDD", 1292 },
			{ "apollo", 50622 },
			{ "Kibbinz", 95006 },
			{ "BR internet > NA Internet", 95006 },
			{ "Blue Screen Of Death Kibbinz", 95006 },
			{ "Flaming Bag of Shit", 81121 },
			{ "bouliiii", 4606 },
			{ "LukeNukem", 105315 },
			{ "badumtzzz #perniciem", 5272 },
			{ "Noose", 44775 },
			{ "Legion", 15721 },
			{ "AnimalPlanet", 90050 },
			{ "Impressive", 90050 },
			{ "Bouge SI plays", 90050 },
			{ "yokshu", 14201 }, // Also played in V1, not sure if the same person?
			{ "Taake", 104032 },
			{ "nja", 129770 },
			{ "Nja", 129770 },
			{ "Wakatau", 56979 },
			{ "Nyhylanth", 2135 },
			{ "ratsoup", 71225 },
			{ "Worm", 21722 },
			{ "Whoneedspacee", 108148 },
			{ "theDGC", 665 },
			{ "Gold", 8186 },
			{ "Goldchocobo", 8186 },
			{ "Chewyy", 97317 },
			{ "Grink", 46102 },
			{ "DJ Doomz", 137044 },
			{ "DeepFear", 109449 },
			{ "joseph3000", 1097 },
			{ "uNKOE @ ", 68968 },
			{ "Diplodocus", 1213 },
			{ "Stiel", 114381 },
			{ "Pralkarz", 135153 },
			{ "Mixed", 30036 },
			{ "heyzeus =D", 8983 },
			{ "a~_Giylisa~_", 105617 },
			{ "A Violet Kemono", 105617 },
			{ "harkken", 10044 },
			{ "karppi", 101261 },
			{ "curry", 94857 },
			{ "D3D%D¿D½DD", 94857 },
			{ "Natural Born Towel", 531 },
			{ "Crypto Gary", 531 },
			{ "sameuel", 106181 },
			{ "samelon don", 106181 },
			{ "Ravenholmzombies", 86805 },
			{ "Dr. Shrimp", 22768 },
			{ "tacocat", 153452 },
			{ "FunOrange", 158257 },
			{ "EmptyMag", 118245 },
			{ "VHS", 151675 },
			{ "arizart", 161160 },
			{ "Furr Jacker", 134173 },
			{ "7stringTheory", 112710 },
			{ "Battlekiller2000", 114215 },
			{ "=PDTC= Battlekiller2000", 114215 },
			{ "Chopps The Penguin", 71157 },
			{ "LocoCaesar_IV", 142939 },
			{ "[2.FJg]LocoCaesar_IV-Hptm", 142939 },
			{ "Nezhaii", 146132 },
			{ "Caedeyrn", 24611 },
			{ "Knar", 159605 },
			{ "Knar (Saved the leaderboard)", 159605 },
			{ "YOUAINTSCARMAN", 77031 },
			{ "YEEHAW TRAIN", 77031 },
			{ "Kryptops", 85058 },
			{ "Blackjack", 25148 },
			{ "Void_Arsonist", 9428 },
			{ "CSn| Dempsey", 135839 },
			{ "MrJabber", 158227 },
			{ "Jyff", 157108 },
			{ "HooFoot", 120114 },
			{ "fushikawa", 120114 },
			{ "miZt-", 58491 },
		};

		public static void Main()
		{
			// Raven fix
			//SwapIds(new DateTime(1, 1, 1), new DateTime(2019, 10, 11), 86805, 187974);

			// pocket fix
			SwapIds(new DateTime(1, 1, 1), new DateTime(2020, 1, 14), 116704, 106722);

			//ApplyNameTable();
		}

		/// <summary>
		/// Swaps Ids for players with 2 accounts in the leaderboard history files.
		/// </summary>
		/// <param name="dateStart">The date on which the player's alt overtook their main account.</param>
		/// <param name="dateEnd">The date on which the player's main overtook their alt account.</param>
		/// <param name="id1">The first Id to swap.</param>
		/// <param name="id2">The second Id to swap.</param>
		public static void SwapIds(DateTime dateStart, DateTime dateEnd, int id1, int id2)
		{
			foreach (string leaderboardHistoryPath in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history", "*.json"))
			{
				string fileName = Path.GetFileNameWithoutExtension(leaderboardHistoryPath);
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath, Encoding.UTF8));
				if (leaderboard.DateTime < dateStart || leaderboard.DateTime > dateEnd)
					continue;

				Entry entry1 = leaderboard.Entries.FirstOrDefault(e => e.Id == id1);
				Entry entry2 = leaderboard.Entries.FirstOrDefault(e => e.Id == id2);

				if (entry1 != null)
					entry1.Id = id2;
				if (entry2 != null)
					entry2.Id = id1;

				File.WriteAllText(leaderboardHistoryPath, JsonConvert.SerializeObject(leaderboard));
				Console.WriteLine($"Wrote {fileName}.", ConsoleColor.Green);
			}
		}

		private static void ApplyNameTable()
		{
			foreach (string path in Directory.GetFiles(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history", "*.json"))
			{
				string jsonString = FileUtils.GetContents(path, Encoding.UTF8);
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(jsonString);

				List<Entry> changes = new List<Entry>();
				foreach (Entry entry in leaderboard.Entries)
				{
					if (entry.Id == 0 && nameTable.ContainsKey(entry.Username))
					{
						entry.Id = nameTable[entry.Username];
						changes.Add(entry);
					}
				}

				if (changes.Count != 0)
				{
					Console.WriteLine(HistoryUtils.HistoryJsonFileNameToDateString(Path.GetFileNameWithoutExtension(path)), ConsoleColor.Yellow);
					foreach (Entry entry in changes)
						Console.WriteLine($"Set Id to {entry.Id.ToString("D6")} for rank {entry.Rank.ToString("D3")} with name {entry.Username} and score {entry.Time / 10000f}");
					Console.WriteLine();
				}

				using (StreamWriter sw = File.CreateText(path))
				{
					sw.Write(JsonConvert.SerializeObject(leaderboard));
				}
			}
		}
	}
}