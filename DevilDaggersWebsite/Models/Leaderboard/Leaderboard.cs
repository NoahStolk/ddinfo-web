using DevilDaggersWebsite.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DevilDaggersWebsite.Models.Leaderboard
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Leaderboard
	{
		[JsonProperty]
		public DateTime DateTime { get; set; } = DateTime.UtcNow;

		[JsonProperty]
		public int Players { get; set; }
		[JsonProperty]
		public ulong TimeGlobal { get; set; }
		[JsonProperty]
		public ulong KillsGlobal { get; set; }
		[JsonProperty]
		public ulong GemsGlobal { get; set; }
		[JsonProperty]
		public ulong DeathsGlobal { get; set; }
		[JsonProperty]
		public ulong ShotsHitGlobal { get; set; }
		[JsonProperty]
		public ulong ShotsFiredGlobal { get; set; }

		[JsonProperty]
		public List<Entry> Entries { get; set; } = new List<Entry>();

		public double AccuracyGlobal => ShotsFiredGlobal == 0 ? 0 : ShotsHitGlobal / (double)ShotsFiredGlobal;

		public string GetMissingGlobalInformation()
		{
			StringBuilder global = new StringBuilder();
			global.Append(GetMissingProperties());

			return global.ToString();
		}

		public string GetMissingUserInformation()
		{
			StringBuilder user = new StringBuilder();
			user.Append(Entries[0].GetMissingProperties());

			return user.ToString();
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
				if (name.Contains("accuracy") || name.Contains("entries") || name.Contains("datetime"))
					continue;

				total++;

				if (valueString == ReflectionUtils.GetDefaultValue(type).ToString())
					missing++;
				else if (name.Contains("shotsfired") && valueString == "10000")
					inaccurate++;
			}

			float globalCompletionRate = 1f - missing / (float)total - inaccurate / 2f / total;
			float userCompletionRate = 0;
			foreach (Entry entry in Entries)
				userCompletionRate += entry.GetCompletionRate() * 0.01f;
			return userCompletionRate * 0.99f + globalCompletionRate * 0.01f;
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
				if (name.Contains("accuracy") || name.Contains("entries") || name.Contains("datetime"))
					continue;

				if (valueString == ReflectionUtils.GetDefaultValue(type).ToString())
					sb.AppendLine($"{info.Name} (Missing) {(info.Name == "ID" ? "(No bans)" : "")}");
				else if (name.Contains("shotsfired") && valueString == "10000")
					sb.AppendLine($"Shots{info.Name.Substring(10)} (Inaccurate)");
			}

			if (Entries.Count != 100)
				sb.AppendLine($"{100 - Entries.Count} users (Missing)");

			return sb.ToString();
		}
	}
}