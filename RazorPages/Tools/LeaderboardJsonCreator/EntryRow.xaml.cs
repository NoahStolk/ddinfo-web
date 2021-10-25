using DevilDaggersCore.Game;
using DevilDaggersWebsite.Dto;
using System.Windows;
using System.Windows.Controls;

namespace LeaderboardJsonCreator
{
	public partial class EntryRow : UserControl
	{
		private readonly Entry _entry;

		public EntryRow(Entry entry)
		{
			_entry = entry;

			InitializeComponent();

			Id.Text = entry.Id.ToString();
			Rank.Text = entry.Rank.ToString();
			Username.Text = entry.Username;
			Time.Text = entry.Time.ToString();
			Kills.Text = entry.Kills.ToString();
			Gems.Text = entry.Gems.ToString();
			DeathType.Text = $"{entry.DeathType} ({GameInfo.GetDeathByType(GameVersion.V3, entry.DeathType)?.Name ?? "Unknown"})";
			Accuracy.Text = entry.DaggersFired == 0 ? "" : $"{entry.DaggersHit / (float)entry.DaggersFired:0.00%} ({entry.DaggersHit}/{entry.DaggersFired})";
			TimeTotal.Text = entry.TimeTotal.ToString();
			KillsTotal.Text = entry.KillsTotal.ToString();
			GemsTotal.Text = entry.GemsTotal.ToString();
			DeathsTotal.Text = entry.DeathsTotal.ToString();
			AccuracyTotal.Text = entry.DaggersFiredTotal == 0 ? "" : $"{entry.DaggersHitTotal / (float)entry.DaggersFiredTotal:0.00%} ({entry.DaggersHitTotal}/{entry.DaggersFiredTotal})";
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			AddNewEntry.This.SetEntry(_entry);
		}
	}
}
