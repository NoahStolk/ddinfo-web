using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Utils;
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
		public static MainWindow This;

		public Leaderboard leaderboard = new Leaderboard();

		public MainWindow()
		{
			InitializeComponent();

			This = this;
		}

		public void RefreshLeaderboard()
		{
			LeaderboardDateTime.Text = HistoryUtils.DateTimeToHistoryJsonFileName(leaderboard.DateTime);
			Players.Text = leaderboard.Players.ToString();
			TimeGlobal.Text = leaderboard.TimeGlobal.ToString();
			KillsGlobal.Text = leaderboard.KillsGlobal.ToString();
			GemsGlobal.Text = leaderboard.GemsGlobal.ToString();
			ShotsHitGlobal.Text = leaderboard.ShotsHitGlobal.ToString();
			ShotsFiredGlobal.Text = leaderboard.ShotsFiredGlobal.ToString();
			DeathsGlobal.Text = leaderboard.DeathsGlobal.ToString();
		}

		public void RefreshEntryList()
		{
			EntryList.Children.Clear();
			foreach (Entry entry in leaderboard.Entries)
				EntryList.Children.Add(new EntryRow(entry));
		}

		private void FileNew_Click(object sender, RoutedEventArgs e)
		{
			leaderboard = new Leaderboard();
			RefreshLeaderboard();
			RefreshEntryList();
		}

		private void FileOpen_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			bool? result = dialog.ShowDialog();

			if (result.HasValue && result.Value)
			{
				leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(dialog.FileName));
				leaderboard.Entries = leaderboard.Entries.OrderBy(en => en.Rank).ToList();
				RefreshLeaderboard();
				RefreshEntryList();
			}
		}

		private void FileSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				leaderboard.DateTime = HistoryUtils.HistoryJsonFileNameToDateTime(LeaderboardDateTime.Text);

				if (int.TryParse(Players.Text, out int players))
					leaderboard.Players = players;
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

				File.WriteAllText($@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\leaderboard-history\{LeaderboardDateTime.Text}.json", JsonConvert.SerializeObject(leaderboard), Encoding.UTF8);
				MessageBox.Show("Save successful");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Save failed");
			}
		}
	}
}