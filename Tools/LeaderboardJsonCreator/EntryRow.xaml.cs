using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using System.Windows;
using System.Windows.Controls;

namespace LeaderboardJsonCreator
{
	public partial class EntryRow : UserControl
	{
		private readonly Entry entry;

		public EntryRow(Entry entry)
		{
			this.entry = entry;

			InitializeComponent();

			Id.Text = entry.Id.ToString();
			Rank.Text = entry.Rank.ToString();
			Username.Text = entry.Username;
			Time.Text = entry.Time.ToString();
			Kills.Text = entry.Kills.ToString();
			Gems.Text = entry.Gems.ToString();
			DeathType.Text = $"{entry.DeathType} ({GameInfo.GetDeathFromDeathType(entry.DeathType).Name})";
			Accuracy.Text = entry.ShotsFired == 0 ? "" : $"{entry.ShotsHit / (float)entry.ShotsFired:0.00%} ({entry.ShotsHit}/{entry.ShotsFired})";
			TimeTotal.Text = entry.TimeTotal.ToString();
			KillsTotal.Text = entry.KillsTotal.ToString();
			GemsTotal.Text = entry.GemsTotal.ToString();
			DeathsTotal.Text = entry.DeathsTotal.ToString();
			AccuracyTotal.Text = entry.ShotsFiredTotal == 0 ? "" : $"{entry.ShotsHitTotal / (float)entry.ShotsFiredTotal:0.00%} ({entry.ShotsHitTotal}/{entry.ShotsFiredTotal})";
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			AddNewEntry.This.SetEntry(entry);
		}
	}
}