using NetBase.Utils;
using Newtonsoft.Json;
using System;
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
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int.TryParse(Players.Text, out leaderboard.Players);
				leaderboard.DateTime = Utils.HistoryJsonFileNameToDateTime(DateTime.Text);
				ulong.TryParse(TimeGlobal.Text, out leaderboard.TimeGlobal);
				ulong.TryParse(KillsGlobal.Text, out leaderboard.KillsGlobal);
				ulong.TryParse(GemsGlobal.Text, out leaderboard.GemsGlobal);
				ulong.TryParse(DeathsGlobal.Text, out leaderboard.DeathsGlobal);
				ulong.TryParse(ShotsHitGlobal.Text, out leaderboard.ShotsHitGlobal);
				ulong.TryParse(ShotsFiredGlobal.Text, out leaderboard.ShotsFiredGlobal);

				FileUtils.CreateText($"{DateTime.Text}.json", JsonConvert.SerializeObject(leaderboard));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Save failed");
			}
		}
	}
}