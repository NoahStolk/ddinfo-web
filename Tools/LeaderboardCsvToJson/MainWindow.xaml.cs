using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Leaderboards.History;
using Microsoft.Win32;
using NetBase.Utils;
using Newtonsoft.Json;
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
			OpenFileDialog dlg = new OpenFileDialog
			{
				DefaultExt = ".csv",
				InitialDirectory = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\LeaderboardCsvToJson\Content"
			};

			bool? result = dlg.ShowDialog();
			if (result.HasValue && result.Value)
			{
				Leaderboard leaderboard = new Leaderboard
				{
					DateTime = HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(dlg.FileName))
				};

				using (StreamReader reader = new StreamReader(dlg.FileName))
				{
					int i = 0;
					while (!reader.EndOfStream)
					{
						i++;

						string line = reader.ReadLine();
						string[] values = line.Split(',');

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
								leaderboard.ShotsHitGlobal = (ulong)GetAccuracyFromPercentageString(values[1]);
								if (leaderboard.ShotsHitGlobal != 0)
									leaderboard.ShotsFiredGlobal = 10000;
								break;
						}

						if (i >= 9 && i < 109)
						{
							Entry entry = new Entry
							{
								DeathType = -1
							};

							if (int.TryParse(values[0], out int rank))
								entry.Rank = rank;

							entry.Username = values[1];

							if (int.TryParse(values[2].Replace(".", ""), out int time))
								entry.Time = time;
							if (int.TryParse(values[3], out int kills))
								entry.Kills = kills;
							if (int.TryParse(values[4], out int gems))
								entry.Gems = gems;

							entry.ShotsHit = (int)GetAccuracyFromPercentageString(values[5]);
							if (entry.ShotsHit != 0)
								entry.ShotsFired = 10000;
							
							entry.DeathType = GameInfo.GetDeathFromDeathName(values[6]).DeathType;

							if (ulong.TryParse(values[7].Replace(".", ""), out ulong timeTotal))
								entry.TimeTotal = timeTotal;
							if (ulong.TryParse(values[8], out ulong killsTotal))
								entry.KillsTotal = killsTotal;
							if (ulong.TryParse(values[9], out ulong gemsTotal))
								entry.GemsTotal = gemsTotal;

							entry.ShotsHitTotal = (ulong)GetAccuracyFromPercentageString(values[10]);
							if (entry.ShotsHitTotal != 0)
								entry.ShotsFiredTotal = 10000;

							if (ulong.TryParse(values[11], out ulong deathsTotal))
								entry.DeathsTotal = deathsTotal;

							leaderboard.Entries.Add(entry);
						}
					}
				}

				string json = JsonConvert.SerializeObject(leaderboard);

				FileUtils.CreateText($@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history\{Path.GetFileNameWithoutExtension(dlg.FileName)}.json", json, Encoding.UTF8);
			}
		}

		private static float GetAccuracyFromPercentageString(string accuracyString)
		{
			float.TryParse(accuracyString.Replace("%", ""), out float percentage);
			return percentage * 100;
		}
	}
}