using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Leaderboards.History;
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
		public Leaderboard leaderboard = new Leaderboard();

		public MainWindow()
		{
			InitializeComponent();
		}

		private void AddEntry_Click(object sender, RoutedEventArgs e)
		{
			Entry entry = new Entry
			{
				DeathType = -1
			};

			if (int.TryParse(Rank.Text, out int rank))
				entry.Rank = rank;
			if (int.TryParse(ID.Text, out int id))
				entry.ID = id;

			entry.Username = Username.Text;

			if (int.TryParse(Time.Text, out int time))
				entry.Time = time;
			if (int.TryParse(Kills.Text, out int kills))
				entry.Kills = kills;
			if (int.TryParse(Gems.Text, out int gems))
				entry.Gems = gems;
			if (int.TryParse(DeathType.Text, out int deathType))
				entry.DeathType = deathType;
			if (int.TryParse(ShotsHit.Text, out int shotsHit))
				entry.ShotsHit = shotsHit;
			if (int.TryParse(ShotsFired.Text, out int shotsFired))
				entry.ShotsFired = shotsFired;
			if (ulong.TryParse(TimeTotal.Text, out ulong timeTotal))
				entry.TimeTotal = timeTotal;
			if (ulong.TryParse(KillsTotal.Text, out ulong killsTotal))
				entry.KillsTotal = killsTotal;
			if (ulong.TryParse(GemsTotal.Text, out ulong gemsTotal))
				entry.GemsTotal = gemsTotal;
			if (ulong.TryParse(DeathsTotal.Text, out ulong deathsTotal))
				entry.DeathsTotal = deathsTotal;
			if (ulong.TryParse(ShotsHitTotal.Text, out ulong shotsHitTotal))
				entry.ShotsHitTotal = shotsHitTotal;
			if (ulong.TryParse(ShotsFiredTotal.Text, out ulong shotsFiredTotal))
				entry.ShotsFiredTotal = shotsFiredTotal;

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
				Entry entry = new Entry
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

				if (int.TryParse(values[1].Trim(',', '.'), out int time))
					entry.Time = time;
				if (int.TryParse(values[2], out int kills))
					entry.Kills = kills;
				if (int.TryParse(values[3], out int gems))
					entry.Gems = gems;

				entry.DeathType = GameInfo.GetDeathFromDeathName(values[4]).DeathType;

				if (ulong.TryParse(values[5].Trim(',', '.'), out ulong timeTotal))
					entry.TimeTotal = timeTotal;
				if (ulong.TryParse(values[6], out ulong killsTotal))
					entry.KillsTotal = killsTotal;
				if (ulong.TryParse(values[7], out ulong gemsTotal))
					entry.GemsTotal = gemsTotal;
				if (ulong.TryParse(values[8], out ulong deathsTotal))
					entry.DeathsTotal = deathsTotal;

				leaderboard.Entries.Add(entry);
			}
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (int.TryParse(Players.Text, out int players))
					leaderboard.Players = players;

				leaderboard.DateTime = HistoryUtils.HistoryJsonFileNameToDateTime(DateTime.Text);

				if (ulong.TryParse(TimeGlobal.Text, out ulong timeGlobal))
					leaderboard.TimeGlobal = timeGlobal;
				if (ulong.TryParse(KillsGlobal.Text, out ulong killsGlobal))
					leaderboard.KillsGlobal = killsGlobal;
				if (ulong.TryParse(GemsGlobal.Text, out ulong gemsGlobal))
					leaderboard.GemsGlobal = gemsGlobal;
				if (ulong.TryParse(DeathsGlobal.Text, out ulong deathsGlobal))
					leaderboard.DeathsGlobal = deathsGlobal;
				if (ulong.TryParse(ShotsHitGlobal.Text, out ulong shotsHitGlobal))
					leaderboard.ShotsHitGlobal = shotsHitGlobal;
				if (ulong.TryParse(ShotsFiredGlobal.Text, out ulong shotsFiredGlobal))
					leaderboard.ShotsFiredGlobal = shotsFiredGlobal;

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