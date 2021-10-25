using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Utils;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace LeaderboardJsonCreator
{
	public partial class MainWindow : Window
	{
		public Leaderboard _leaderboard = new();

		public MainWindow()
		{
			InitializeComponent();

			This = this;
		}

		public static MainWindow This { get; private set; } = null!;

		public void RefreshLeaderboard()
		{
			LeaderboardDateTime.Text = HistoryUtils.DateTimeToHistoryJsonFileName(_leaderboard.DateTime);
			Players.Text = _leaderboard.Players.ToString();
			TimeGlobal.Text = _leaderboard.TimeGlobal.ToString();
			KillsGlobal.Text = _leaderboard.KillsGlobal.ToString();
			GemsGlobal.Text = _leaderboard.GemsGlobal.ToString();
			ShotsHitGlobal.Text = _leaderboard.DaggersHitGlobal.ToString();
			ShotsFiredGlobal.Text = _leaderboard.DaggersFiredGlobal.ToString();
			DeathsGlobal.Text = _leaderboard.DeathsGlobal.ToString();
		}

		public void RefreshEntryList()
		{
			EntryList.Children.Clear();
			foreach (Entry entry in _leaderboard.Entries)
				EntryList.Children.Add(new EntryRow(entry));
		}

		private void FileNew_Click(object sender, RoutedEventArgs e)
		{
			_leaderboard = new Leaderboard();
			RefreshLeaderboard();
			RefreshEntryList();
		}

		private void FileOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new();
			bool? result = dialog.ShowDialog();

			if (result == true)
			{
				_leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(dialog.FileName)) ?? throw new("Could not deserialize leaderboard.");
				_leaderboard.Entries = _leaderboard.Entries.OrderBy(en => en.Rank).ToList();
				RefreshLeaderboard();
				RefreshEntryList();
			}
		}

		private void FileSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_leaderboard.DateTime = HistoryUtils.HistoryJsonFileNameToDateTime(LeaderboardDateTime.Text);

				if (int.TryParse(Players.Text, out int players))
					_leaderboard.Players = players;
				if (ulong.TryParse(TimeGlobal.Text, out ulong timeGlobal))
					_leaderboard.TimeGlobal = timeGlobal;
				if (ulong.TryParse(KillsGlobal.Text, out ulong killsGlobal))
					_leaderboard.KillsGlobal = killsGlobal;
				if (ulong.TryParse(GemsGlobal.Text, out ulong gemsGlobal))
					_leaderboard.GemsGlobal = gemsGlobal;
				if (ulong.TryParse(DeathsGlobal.Text, out ulong deathsGlobal))
					_leaderboard.DeathsGlobal = deathsGlobal;
				if (ulong.TryParse(ShotsHitGlobal.Text, out ulong shotsHitGlobal))
					_leaderboard.DaggersHitGlobal = shotsHitGlobal;
				if (ulong.TryParse(ShotsFiredGlobal.Text, out ulong shotsFiredGlobal))
					_leaderboard.DaggersFiredGlobal = shotsFiredGlobal;

				File.WriteAllText($@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite.Razor\wwwroot\leaderboard-history\{LeaderboardDateTime.Text}.json", JsonConvert.SerializeObject(_leaderboard), Encoding.UTF8);
				MessageBox.Show("Save successful");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Save failed");
			}
		}
	}
}
