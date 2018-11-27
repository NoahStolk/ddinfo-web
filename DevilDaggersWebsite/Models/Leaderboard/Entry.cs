using DevilDaggersWebsite.Models.Game;
using DevilDaggersWebsite.Utils;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Text;
using System.Web;

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

		public double Accuracy => ShotsFired == 0 ? 0 : ShotsHit / (double)ShotsFired;
		public double AccuracyTotal => ShotsFiredTotal == 0 ? 0 : ShotsHitTotal / (double)ShotsFiredTotal;

		public HtmlString ToHTMLData()
		{
			return new HtmlString($@"
				rank='{Rank}'
				username='{HttpUtility.HtmlEncode(Username)}'
				time='{Time}'
				kills='{Kills}'
				gems='{Gems}'
				accuracy='{(Accuracy * 10000).ToString("0")}'
				death-type='{GetDeathFromDeathType().Name}'
				total-time='{TimeTotal}'
				total-kills='{KillsTotal}'
				total-gems='{GemsTotal}'
				total-accuracy='{(AccuracyTotal * 10000).ToString("0")}'
				total-deaths='{DeathsTotal}'
			");
		}

		public Death GetDeathFromDeathType()
		{
			if (DeathType < 0 || DeathType >= 16)
				return GameUtils.Unknown;
			if (UserUtils.MacroIDs.Contains(ID))
				return GameUtils.Macroed;
			return GameUtils.Deaths[DeathType];
		}

		public float GetCompletionRate()
		{
			int total = 0;
			int missing = 0;
			int inaccurate = 0;

			Type t = GetType();
			foreach (PropertyInfo info in t.GetProperties())
			{
				object value = info.GetValue(this);
				string valueString = value.ToString();
				if (string.IsNullOrEmpty(valueString))
					continue;

				Type type = value.GetType();
				string name = info.Name.ToLower();
				if (name.Contains("accuracy"))
					continue;

				total++;

				if (valueString == ReflectionUtils.GetDefaultValue(type).ToString())
					missing++;
				else if (name.Contains("shotsfired") && valueString == "10000")
					inaccurate++;
			}

			return 1f - missing / (float)total - inaccurate / 2f / total;
		}

		public string GetMissingProperties()
		{
			StringBuilder sb = new StringBuilder();

			Type t = GetType();
			foreach (PropertyInfo info in t.GetProperties())
			{
				object value = info.GetValue(this);
				string valueString = value.ToString();
				if (string.IsNullOrEmpty(valueString))
					continue;

				Type type = value.GetType();
				string name = info.Name.ToLower();
				if (name.Contains("accuracy"))
					continue;

				if ((name.Contains("deathtype") && valueString == "-1") ||
					(!name.Contains("deathtype") && valueString == ReflectionUtils.GetDefaultValue(type).ToString()))
					sb.AppendLine($"{info.Name} (Missing) {(info.Name == "ID" ? "(No bans)" : "")}");
				else if (name.Contains("shotsfired") && valueString == "10000")
					sb.AppendLine($"Shots{info.Name.Substring(10)} (Inaccurate)");
			}

			return sb.ToString();
		}
	}
}