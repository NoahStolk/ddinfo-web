using DevilDaggersWebsite.Models.Game;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace DevilDaggersWebsite.Models.Leaderboard
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Entry
	{
		[JsonProperty]
		public int Rank { get; set; }
		[JsonProperty]
		public int ID { get; set; }
		[JsonProperty]
		public string Username { get; set; }
		[JsonProperty]
		public int Time { get; set; }
		[JsonProperty]
		public int Kills { get; set; }
		[JsonProperty]
		public int Gems { get; set; }
		[JsonProperty]
		public int DeathType { get; set; }
		[JsonProperty]
		public int ShotsHit { get; set; }
		[JsonProperty]
		public int ShotsFired { get; set; }
		[JsonProperty]
		public ulong TimeTotal { get; set; }
		[JsonProperty]
		public ulong KillsTotal { get; set; }
		[JsonProperty]
		public ulong GemsTotal { get; set; }
		[JsonProperty]
		public ulong DeathsTotal { get; set; }
		[JsonProperty]
		public ulong ShotsHitTotal { get; set; }
		[JsonProperty]
		public ulong ShotsFiredTotal { get; set; }

		public double Accuracy => ShotsFired == 0 ? 0 : ShotsHit / (double)ShotsFired * 100;
		public double AccuracyTotal => ShotsFiredTotal == 0 ? 0 : ShotsHitTotal / (double)ShotsFiredTotal * 100;

		public HtmlString ToHTMLData()
		{
			return new HtmlString($@"
				rank='{Rank}'
				username='{Username}'
				time='{Time}'
				kills='{Kills}'
				gems='{Gems}'
				accuracy='{(Accuracy * 100).ToString("0")}'
				death-type='{GetDeathFromDeathType().Name}'
				total-time='{TimeTotal}'
				total-kills='{KillsTotal}'
				total-gems='{GemsTotal}'
				total-accuracy='{(AccuracyTotal * 100).ToString("0")}'
				total-deaths='{DeathsTotal}'
			");
		}

		public Death GetDeathFromDeathType()
		{
			if (DeathType < 0 || DeathType >= 16)
				return GameUtils.Unknown;
			if (UserHelper.MacroIDs.Contains(ID))
				return GameUtils.Macroed;
			return GameUtils.Deaths[DeathType];
		}
	}
}