namespace DevilDaggersWebsite.Models.Leaderboard
{
	public class Entry
	{
		public int Rank { get; set; }
		public int ID { get; set; }
		public string Username { get; set; }
		public int Time { get; set; }
		public int Kills { get; set; }
		public int Gems { get; set; }
		public int DeathType { get; set; }
		public int ShotsHit { get; set; }
		public int ShotsFired { get; set; }
		public ulong TimeTotal { get; set; }
		public ulong KillsTotal { get; set; }
		public ulong GemsTotal { get; set; }
		public ulong DeathsTotal { get; set; }
		public ulong ShotsHitTotal { get; set; }
		public ulong ShotsFiredTotal { get; set; }

		public double Accuracy => ShotsFired == 0 ? 0 : ShotsHit / (double)ShotsFired * 100;
		public double AccuracyTotal => ShotsFiredTotal == 0 ? 0 : ShotsHitTotal / (double)ShotsFiredTotal * 100;

		public string ToHTMLData()
		{
			return $@"
				rank='{Rank}'
				username='{Username}'
				time='{Time}'
				kills='{Kills}'
				gems='{Gems}'
				accuracy='{Accuracy}'
				death-type='{DeathType}'
				total-time='{TimeTotal}'
				total-kills='{KillsTotal}'
				total-gems='{GemsTotal}'
				total-accuracy='{AccuracyTotal}'
				total-deaths='{DeathsTotal}'
			";
		}
	}
}