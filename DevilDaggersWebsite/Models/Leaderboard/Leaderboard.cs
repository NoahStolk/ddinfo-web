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

		public int Offset { get; set; } = 1;
		public int OffsetPrevious { get; set; } = 1;

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

		public double AccuracyGlobal => ShotsFiredGlobal == 0 ? 0 : ShotsHitGlobal / (double)ShotsFiredGlobal * 100;

		public string GetMissingGlobalInformation()
		{
			StringBuilder global = new StringBuilder();
			GetMissingProperties(this, global);

			return global.ToString();
		}

		public string GetMissingUserInformation()
		{
			StringBuilder user = new StringBuilder();
			GetMissingProperties(Entries[0], user);

			return user.ToString();
		}

		public float GetCompletionRate(object p)
		{
			int total = 0;
			int missing = 0;
			int inaccurate = 0;

			Type t = p.GetType();
			foreach (PropertyInfo info in t.GetProperties())
			{
				object value = info.GetValue(p);
				Type type = value.GetType();

				try
				{
					total++;

					if ((info.Name.ToLower().Contains("deathtype") && value.ToString() == "-1") ||
						(!info.Name.ToLower().Contains("deathtype") && value.ToString() == GetDefaultValue(type).ToString()))
						missing++;
					else if (info.Name.ToLower().Contains("shotsfired") && value.ToString() == "10000")
						inaccurate++;
				}
				catch (Exception)
				{
					// nobody cares
				}
			}

			return (1f - missing / (float)total - inaccurate / 2f / total) * Entries.Count;
		}

		private static void GetMissingProperties(object p, StringBuilder sb)
		{
			Type t = p.GetType();
			foreach (PropertyInfo info in t.GetProperties())
			{
				object value = info.GetValue(p);
				Type type = value.GetType();

				try
				{
					if ((info.Name.ToLower().Contains("deathtype") && value.ToString() == "-1") ||
						(!info.Name.ToLower().Contains("deathtype") && value.ToString() == GetDefaultValue(type).ToString()))
						sb.AppendLine($"{info.Name} (Missing) {(info.Name == "ID" ? "(No bans)" : "")}");
					else if (info.Name.ToLower().Contains("shotsfired") && value.ToString() == "10000")
						sb.AppendLine($"Shots{info.Name.Substring(10)} (Inaccurate)");
				}
				catch (Exception)
				{
					// nobody cares
				}
			}

			if (string.IsNullOrEmpty(sb.ToString()))
				sb.AppendLine(RazorUtils.NAString.ToString());
		}

		private static object GetDefaultValue(Type t)
		{
			if (t.IsValueType)
				return Activator.CreateInstance(t);

			return null;
		}
	}
}