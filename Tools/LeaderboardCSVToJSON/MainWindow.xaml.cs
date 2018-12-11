using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboard;
using DevilDaggersCore.SiteUtils;
using Microsoft.Win32;
using NetBase.Utils;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Windows;

namespace LeaderboardCSVToJSON
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
				InitialDirectory = @"C:\Users\NOAH\source\repos\DevilDaggersWebsite\LeaderboardCSVToJSON\Content"
			};

			bool? result = dlg.ShowDialog();
			if (result.HasValue && result.Value)
			{
				Leaderboard leaderboard = new Leaderboard
				{
					DateTime = LeaderboardHistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(dlg.FileName))
				};

				using (StreamReader reader = new StreamReader(dlg.FileName))
				{
					int i = 0;
					while (!reader.EndOfStream)
					{
						i++;

						string line = reader.ReadLine();
						string[] values = line.Split(',');

						if (i == 1)
							int.TryParse(values[1], out leaderboard.Players);
						else if (i == 2)
							ulong.TryParse(values[1].Replace(".", ""), out leaderboard.TimeGlobal);
						else if (i == 3)
							ulong.TryParse(values[1], out leaderboard.KillsGlobal);
						else if (i == 4)
							ulong.TryParse(values[1], out leaderboard.GemsGlobal);
						else if (i == 5)
							ulong.TryParse(values[1], out leaderboard.DeathsGlobal);
						else if (i == 6)
						{
							leaderboard.ShotsHitGlobal = (ulong)GetAccuracy(values[1]);
							if (leaderboard.ShotsHitGlobal != 0)
								leaderboard.ShotsFiredGlobal = 10000;
						}

						if (i >= 9 && i < 109)
						{
							LeaderboardEntry entry = new LeaderboardEntry
							{
								DeathType = -1
							};

							int.TryParse(values[0], out entry.Rank);
							entry.Username = values[1];
							int.TryParse(values[2].Replace(".", ""), out entry.Time);
							int.TryParse(values[3], out entry.Kills);
							int.TryParse(values[4], out entry.Gems);
							entry.ShotsHit = (int)GetAccuracy(values[5]);
							if (entry.ShotsHit != 0)
								entry.ShotsFired = 10000;
							entry.DeathType = Game.GetDeathFromDeathName(values[6]).LeaderboardType;
							ulong.TryParse(values[7].Replace(".", ""), out entry.TimeTotal);
							ulong.TryParse(values[8], out entry.KillsTotal);
							ulong.TryParse(values[9], out entry.GemsTotal);
							entry.ShotsHitTotal = (ulong)GetAccuracy(values[10]);
							if (entry.ShotsHitTotal != 0)
								entry.ShotsFiredTotal = 10000;
							ulong.TryParse(values[11], out entry.DeathsTotal);

							leaderboard.Entries.Add(entry);
						}
					}
				}

				string json = JsonConvert.SerializeObject(leaderboard);

				FileUtils.CreateText($@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history\{Path.GetFileNameWithoutExtension(dlg.FileName)}.json", json);
			}
		}

		private static float GetAccuracy(string accuracyString)
		{
			float.TryParse(accuracyString.Replace("%", ""), out float percentage);
			return percentage * 100;
		}
	}
}