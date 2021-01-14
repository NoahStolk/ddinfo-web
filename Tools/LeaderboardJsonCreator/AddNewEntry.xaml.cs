using DevilDaggersCore.Leaderboards;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToolsShared;

namespace LeaderboardJsonCreator
{
	public partial class AddNewEntry : UserControl
	{
		public static AddNewEntry This;

		public Entry entry = new Entry { DeathType = -1 };

		public AddNewEntry()
		{
			InitializeComponent();

			This = this;
		}

		public void SetEntry(Entry entry)
		{
			this.entry = entry;

			Id.Text = entry.Id.ToString();
			Rank.Text = entry.Rank.ToString();
			Username.Text = entry.Username;
			Time.Text = entry.Time.ToString();
			Kills.Text = entry.Kills.ToString();
			Gems.Text = entry.Gems.ToString();
			ShotsHit.Text = entry.ShotsHit.ToString();
			ShotsFired.Text = entry.ShotsFired.ToString();
			DeathType.Text = entry.DeathType.ToString();
			TimeTotal.Text = entry.TimeTotal.ToString();
			KillsTotal.Text = entry.KillsTotal.ToString();
			GemsTotal.Text = entry.GemsTotal.ToString();
			ShotsHitTotal.Text = entry.ShotsHitTotal.ToString();
			ShotsFiredTotal.Text = entry.ShotsFiredTotal.ToString();
			DeathsTotal.Text = entry.DeathsTotal.ToString();
		}

		private void AddEntry_Click(object sender, RoutedEventArgs e)
		{
			// TODO: Check if rank/ID already exists.
			if (int.TryParse(Rank.Text, out int rank))
				entry.Rank = rank;
			if (int.TryParse(Id.Text, out int id))
				entry.Id = id;

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

			Entry existingEntry = MainWindow.This.leaderboard.Entries.FirstOrDefault(en => en.Id == entry.Id);
			if (existingEntry != null)
				existingEntry = entry;
			else
				MainWindow.This.leaderboard.Entries.Add(entry);
			MainWindow.This.RefreshEntryList();

			Rank.Text = (++rank).ToString();
			entry = new Entry { DeathType = -1 };
		}

		private void Username_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (NameData.NameTable.ContainsKey(Username.Text))
				Id.Text = NameData.NameTable[Username.Text].ToString();
		}

		private void Time_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!int.TryParse(Id.Text, out int id))
				return;
			if (!int.TryParse(Time.Text, out int time))
				return;
			if (time < 4500000)
				return;

			Dictionary<string, Leaderboard> leaderboards = HighscoreSpreadUtils.GetAllLeaderboards();
			Entry entryWithData = HighscoreSpreadUtils.GetEntryWithData(leaderboards.Select(kvp => kvp.Value).ToList(), entry, id, time);
			if (entryWithData == null)
			{
				Gems.Text = string.Empty;
				Kills.Text = string.Empty;
				DeathType.Text = string.Empty;
				ShotsHit.Text = string.Empty;
				ShotsFired.Text = string.Empty;
			}
			else
			{
				Gems.Text = entryWithData.Gems.ToString();
				Kills.Text = entryWithData.Kills.ToString();
				DeathType.Text = entryWithData.DeathType.ToString();
				ShotsHit.Text = entryWithData.ShotsHit.ToString();
				ShotsFired.Text = entryWithData.ShotsFired.ToString();
			}
		}
	}
}
