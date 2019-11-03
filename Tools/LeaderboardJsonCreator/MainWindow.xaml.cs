using DevilDaggersCore.Game;
using DevilDaggersUtilities.Tools;
using DevilDaggersUtilities.Website;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LeaderboardJsonCreator
{
	public partial class MainWindow : Window
	{
		public LeaderboardSimplified leaderboard = new LeaderboardSimplified();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void AddEntry_Click(object sender, RoutedEventArgs e)
		{
			LeaderboardEntrySimplified entry = new LeaderboardEntrySimplified
			{
				DeathType = -1
			};

			int.TryParse(Rank.Text, out entry.Rank);
			int.TryParse(ID.Text, out entry.ID);
			entry.Username = Username.Text;
			int.TryParse(Time.Text, out entry.Time);
			int.TryParse(Kills.Text, out entry.Kills);
			int.TryParse(Gems.Text, out entry.Gems);
			int.TryParse(DeathType.Text, out entry.DeathType);
			int.TryParse(ShotsHit.Text, out entry.ShotsHit);
			int.TryParse(ShotsFired.Text, out entry.ShotsFired);
			ulong.TryParse(TimeTotal.Text, out entry.TimeTotal);
			ulong.TryParse(KillsTotal.Text, out entry.KillsTotal);
			ulong.TryParse(GemsTotal.Text, out entry.GemsTotal);
			ulong.TryParse(DeathsTotal.Text, out entry.DeathsTotal);
			ulong.TryParse(ShotsHitTotal.Text, out entry.ShotsHitTotal);
			ulong.TryParse(ShotsFiredTotal.Text, out entry.ShotsFiredTotal);

			leaderboard.Entries.Add(entry);

			Entries.Text = leaderboard.Entries.Count.ToString();

			Rank.Text = (leaderboard.Entries.Count + 1).ToString();
		}

		private void EntryList_Click(object sender, RoutedEventArgs e)
		{
			string[] entries = EntryList.Text.Split('\n');
			int i = leaderboard.Entries.Count;
			foreach (string line in entries)
			{
				i++;
				LeaderboardEntrySimplified entry = new LeaderboardEntrySimplified
				{
					Rank = i,
					DeathType = -1
				};

				List<string> values = line.Split(' ').ToList();
				if (values.Count < 9)
					continue;
				while (values.Count > 9) // no accuracy
				{
					values[0] += $" {values[1]}";
					values.Remove(values[1]);
				}

				entry.Username = values[0];
				int.TryParse(values[1].Trim(',', '.'), out entry.Time);
				int.TryParse(values[2], out entry.Kills);
				int.TryParse(values[3], out entry.Gems);
				entry.DeathType = GameInfo.GetDeathFromDeathName(values[4]).DeathType;

				ulong.TryParse(values[5].Trim(',', '.'), out entry.TimeTotal);
				ulong.TryParse(values[6], out entry.KillsTotal);
				ulong.TryParse(values[7], out entry.GemsTotal);
				ulong.TryParse(values[8], out entry.DeathsTotal);

				leaderboard.Entries.Add(entry);
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int.TryParse(Players.Text, out leaderboard.Players);
				leaderboard.DateTime = LeaderboardHistoryUtils.HistoryJsonFileNameToDateTime(DateTime.Text);
				ulong.TryParse(TimeGlobal.Text, out leaderboard.TimeGlobal);
				ulong.TryParse(KillsGlobal.Text, out leaderboard.KillsGlobal);
				ulong.TryParse(GemsGlobal.Text, out leaderboard.GemsGlobal);
				ulong.TryParse(DeathsGlobal.Text, out leaderboard.DeathsGlobal);
				ulong.TryParse(ShotsHitGlobal.Text, out leaderboard.ShotsHitGlobal);
				ulong.TryParse(ShotsFiredGlobal.Text, out leaderboard.ShotsFiredGlobal);

				FileUtils.CreateText($@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history\{DateTime.Text}.json", JsonConvert.SerializeObject(leaderboard), Encoding.UTF8);
				MessageBox.Show("Save successful");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Save failed");
			}
		}
	}
}