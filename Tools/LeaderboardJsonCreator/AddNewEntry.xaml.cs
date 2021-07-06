using DevilDaggersWebsite.Clients;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ToolsShared;

namespace LeaderboardJsonCreator
{
	public partial class AddNewEntry : UserControl
	{
		public Entry _entry = new() { DeathType = -1 };

		public AddNewEntry()
		{
			InitializeComponent();

			This = this;
		}

		public static AddNewEntry This { get; private set; } = null!;

		public void SetEntry(Entry entry)
		{
			_entry = entry;

			Id.Text = entry.Id.ToString();
			Rank.Text = entry.Rank.ToString();
			Username.Text = entry.Username;
			Time.Text = entry.Time.ToString();
			Kills.Text = entry.Kills.ToString();
			Gems.Text = entry.Gems.ToString();
			DaggersHit.Text = entry.DaggersHit.ToString();
			DaggersFired.Text = entry.DaggersFired.ToString();
			DeathType.Text = entry.DeathType.ToString();
			TimeTotal.Text = entry.TimeTotal.ToString();
			KillsTotal.Text = entry.KillsTotal.ToString();
			GemsTotal.Text = entry.GemsTotal.ToString();
			DaggersHitTotal.Text = entry.DaggersHitTotal.ToString();
			DaggersFiredTotal.Text = entry.DaggersFiredTotal.ToString();
			DeathsTotal.Text = entry.DeathsTotal.ToString();
		}

		private void AddEntry_Click(object sender, RoutedEventArgs e)
		{
			// TODO: Check if rank/ID already exists.
			if (int.TryParse(Rank.Text, out int rank))
				_entry.Rank = rank;
			if (int.TryParse(Id.Text, out int id))
				_entry.Id = id;

			_entry.Username = Username.Text;

			if (int.TryParse(Time.Text, out int time))
				_entry.Time = time;
			if (int.TryParse(Kills.Text, out int kills))
				_entry.Kills = kills;
			if (int.TryParse(Gems.Text, out int gems))
				_entry.Gems = gems;
			if (short.TryParse(DeathType.Text, out short deathType))
				_entry.DeathType = deathType;
			if (int.TryParse(DaggersHit.Text, out int daggersHit))
				_entry.DaggersHit = daggersHit;
			if (int.TryParse(DaggersFired.Text, out int daggersFired))
				_entry.DaggersFired = daggersFired;
			if (ulong.TryParse(TimeTotal.Text, out ulong timeTotal))
				_entry.TimeTotal = timeTotal;
			if (ulong.TryParse(KillsTotal.Text, out ulong killsTotal))
				_entry.KillsTotal = killsTotal;
			if (ulong.TryParse(GemsTotal.Text, out ulong gemsTotal))
				_entry.GemsTotal = gemsTotal;
			if (ulong.TryParse(DeathsTotal.Text, out ulong deathsTotal))
				_entry.DeathsTotal = deathsTotal;
			if (ulong.TryParse(DaggersHitTotal.Text, out ulong shotsHitTotal))
				_entry.DaggersHitTotal = shotsHitTotal;
			if (ulong.TryParse(DaggersFiredTotal.Text, out ulong shotsFiredTotal))
				_entry.DaggersFiredTotal = shotsFiredTotal;

			Entry? existingEntry = MainWindow.This._leaderboard.Entries.FirstOrDefault(en => en.Id == _entry.Id);
			if (existingEntry != null)
				existingEntry = _entry;
			else
				MainWindow.This._leaderboard.Entries.Add(_entry);
			MainWindow.This.RefreshEntryList();

			Rank.Text = (++rank).ToString();
			_entry = new Entry { DeathType = -1 };
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
			Entry? entryWithData = GetEntryWithData(leaderboards.Select(kvp => kvp.Value).ToList(), _entry, id, time);
			if (entryWithData == null)
			{
				Gems.Text = string.Empty;
				Kills.Text = string.Empty;
				DeathType.Text = string.Empty;
				DaggersHit.Text = string.Empty;
				DaggersFired.Text = string.Empty;
			}
			else
			{
				Gems.Text = entryWithData.Gems.ToString();
				Kills.Text = entryWithData.Kills.ToString();
				DeathType.Text = entryWithData.DeathType.ToString();
				DaggersHit.Text = entryWithData.DaggersHit.ToString();
				DaggersFired.Text = entryWithData.DaggersFired.ToString();
			}
		}

		private static Entry? GetEntryWithData(List<Leaderboard> leaderboards, Entry entry, int id, int time)
		{
			if (id == 0 || !HighscoreSpreadUtils.HasMissingStats(entry))
				return null;

			Leaderboard? leaderboardWithStats = leaderboards.FirstOrDefault(l => l.Entries.Any(e => e.Id == id && e.Time >= time - 1 && e.Time <= time + 1 && !HighscoreSpreadUtils.HasMissingStats(e))); // TODO: Get most complete data.
			if (leaderboardWithStats == null)
				return null;

			return leaderboardWithStats.Entries.FirstOrDefault(e => e.Id == id);
		}
	}
}
