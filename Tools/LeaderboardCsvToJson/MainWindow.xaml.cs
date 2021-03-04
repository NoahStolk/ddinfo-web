#define COOKLE
#if !COOKLE
using DevilDaggersCore.Game;
#endif
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Dto;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace LeaderboardCsvToJson
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new()
			{
				DefaultExt = ".csv",
				InitialDirectory = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\LeaderboardCsvToJson\Content"
			};

			bool? result = dlg.ShowDialog();
			if (result.HasValue && result.Value)
			{
				Leaderboard leaderboard = new()
				{
					DateTime = HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(dlg.FileName))
				};

				using (StreamReader reader = new(dlg.FileName))
				{
					int i = 0;
					while (!reader.EndOfStream)
					{
						i++;

						string? line = reader.ReadLine();
						string[] values = line?.Split(',') ?? Array.Empty<string>();

						switch (i)
						{
							case 1:
								if (int.TryParse(values[1], out int players))
									leaderboard.Players = players;
								break;
							case 2:
								if (ulong.TryParse(values[1].Replace(".", ""), out ulong timeTotal))
									leaderboard.TimeGlobal = timeTotal;
								break;
							case 3:
								if (ulong.TryParse(values[1], out ulong killsGlobal))
									leaderboard.KillsGlobal = killsGlobal;
								break;
							case 4:
								if (ulong.TryParse(values[1], out ulong gemsGlobal))
									leaderboard.GemsGlobal = gemsGlobal;
								break;
							case 5:
								if (ulong.TryParse(values[1], out ulong deathsGlobal))
									leaderboard.DeathsGlobal = deathsGlobal;
								break;
							case 6:
								leaderboard.DaggersHitGlobal = (ulong)GetAccuracyFromPercentageString(values[1]);
								if (leaderboard.DaggersHitGlobal != 0)
									leaderboard.DaggersFiredGlobal = 10000;
								break;
						}

						if (i >= 9 && i < 109)
						{
							Entry entry = new()
							{
								DeathType = -1,
							};

							if (values.Length == 0)
								break;

							if (int.TryParse(values[0], out int rank))
								entry.Rank = rank;

							entry.Username = values[1];

							if (float.TryParse(values[2], out float f))
								entry.Time = (int)(f * 10000);

#if COOKLE
							if (int.TryParse(values[3], out int id))
								entry.Id = id;
#else
							if (int.TryParse(values[3], out int kills))
								entry.Kills = kills;
							if (int.TryParse(values[4], out int gems))
								entry.Gems = gems;

							entry.DaggersHit = (int)GetAccuracyFromPercentageString(values[5]);
							if (entry.DaggersHit != 0)
								entry.DaggersFired = 10000;

							entry.DeathType = (short)(GameInfo.GetDeathByName(GameInfo.GetGameVersionFromDate(leaderboard.DateTime) ?? GameVersion.V1, values[6])?.DeathType ?? -1);

							if (ulong.TryParse(values[7].Replace(".", ""), out ulong timeTotal))
								entry.TimeTotal = timeTotal;
							if (ulong.TryParse(values[8], out ulong killsTotal))
								entry.KillsTotal = killsTotal;
							if (ulong.TryParse(values[9], out ulong gemsTotal))
								entry.GemsTotal = gemsTotal;

							entry.DaggersHitTotal = (ulong)GetAccuracyFromPercentageString(values[10]);
							if (entry.DaggersHitTotal != 0)
								entry.DaggersFiredTotal = 10000;

							if (ulong.TryParse(values[11], out ulong deathsTotal))
								entry.DeathsTotal = deathsTotal;
#endif
							leaderboard.Entries.Add(entry);
						}
					}
				}

				string json = JsonConvert.SerializeObject(leaderboard);

				File.WriteAllText($@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite.Razor\wwwroot\leaderboard-history\{Path.GetFileNameWithoutExtension(dlg.FileName)}.json", json, Encoding.UTF8);
			}
		}

		private static float GetAccuracyFromPercentageString(string accuracyString)
		{
			float.TryParse(accuracyString.Replace("%", ""), out float percentage);
			return percentage * 100;
		}
	}
}
