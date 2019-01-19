using DevilDaggersWebsite.Models.User;
using Microsoft.AspNetCore.Html;
using NetBase.Utils;
using Newtonsoft.Json;
using System;
using System.Reflection;
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

		private Completion completion = new Completion();

		public Completion GetCompletion()
		{
			if (completion.Initialised)
				return completion;

			Type t = GetType();
			foreach (PropertyInfo info in t.GetProperties())
			{
				object value = info.GetValue(this);
				if (value == null)
					continue;

				string valueString = value.ToString();
				if (string.IsNullOrEmpty(valueString))
					continue;

				Type type = value.GetType();
				string name = info.Name.ToLower();
				if (name.Contains("accuracy") || name.Contains("completion"))
					continue;

				completion.CompletionEntries[info.Name] = CompletionEntry.Complete;

				if ((name.Contains("deathtype") && valueString == "-1") ||
					(!name.Contains("deathtype") && valueString == ReflectionUtils.GetDefaultValue(type).ToString()))
					completion.CompletionEntries[info.Name] = CompletionEntry.Missing;
			}

			completion.Initialised = true;
			return completion;
		}

		public HtmlString ToHTMLData(string flagCode)
		{
			return new HtmlString($@"
				rank='{Rank}'
				flag='{flagCode}'
				username='{HttpUtility.HtmlEncode(Username)}'
				time='{Time}'
				kills='{Kills}'
				gems='{Gems}'
				accuracy='{(Accuracy * 10000).ToString("0")}'
				death-type='{DevilDaggersCore.Game.Game.GetDeathFromDeathType(DeathType).Name}'
				total-time='{TimeTotal}'
				total-kills='{KillsTotal}'
				total-gems='{GemsTotal}'
				total-accuracy='{(AccuracyTotal * 10000).ToString("0")}'
				total-deaths='{DeathsTotal}'
			");
		}

		public HtmlString ToHTMLData(string flagCode, PlayerSetting playerSetting)
		{
			return new HtmlString($@"
				rank='{Rank}'
				flag='{flagCode}'
				username='{HttpUtility.HtmlEncode(Username)}'
				time='{Time}'
				e-dpi='{playerSetting.EDPI * 1000 ?? 0}'
				dpi='{playerSetting.DPI ?? 0}'
				in-game-sens='{playerSetting.InGameSens * 1000 ?? 0}'
				fov='{playerSetting.FOV ?? 0}'
				hand='{(!playerSetting.RightHanded.HasValue ? -1 : playerSetting.RightHanded.Value ? 1 : 0)}'
				flash='{(!playerSetting.FlashEnabled.HasValue ? -1 : playerSetting.FlashEnabled.Value ? 1 : 0)}'
			");
		}
	}
}