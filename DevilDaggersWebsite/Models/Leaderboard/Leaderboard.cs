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
				string valueString = value.ToString();
				if (string.IsNullOrEmpty(valueString))
					continue;

				Type type = value.GetType();
				string name = info.Name.ToLower();
				if (name.Contains("accuracy") || name.Contains("entries") || name.Contains("search"))
					continue;

				total++;

				if ((name.Contains("deathtype") && valueString == "-1") ||
					(!name.Contains("deathtype") && valueString == GetDefaultValue(type).ToString()))
					missing++;
				else if (name.Contains("shotsfired") && valueString == "10000")
					inaccurate++;
			}

			return (1f - missing / (float)total - inaccurate / 2f / total) * Entries.Count;
		}

		private static void GetMissingProperties(object p, StringBuilder sb)
		{
			Type t = p.GetType();
			foreach (PropertyInfo info in t.GetProperties())
			{
				object value = info.GetValue(p);
				string valueString = value.ToString();
				if (string.IsNullOrEmpty(valueString))
					continue;

				Type type = value.GetType();
				string name = info.Name.ToLower();
				if (name.Contains("accuracy") || name.Contains("entries") || name.Contains("search"))
					continue;

				if ((name.Contains("deathtype") && valueString == "-1") ||
					(!name.Contains("deathtype") && valueString == GetDefaultValue(type).ToString()))
					sb.AppendLine($"{info.Name} (Missing) {(info.Name == "ID" ? "(No bans)" : "")}");
				else if (name.Contains("shotsfired") && valueString == "10000")
					sb.AppendLine($"Shots{info.Name.Substring(10)} (Inaccurate)");
			}

			if (string.IsNullOrEmpty(sb.ToString()))
				sb.AppendLine(RazorUtils.NAString.ToString());
		}

		private static object GetDefaultValue(Type type)
		{
			if (type.IsValueType)
				return Activator.CreateInstance(type);

			if (type == typeof(string))
				return string.Empty;

			return null;
		}

		private static object GetDefaultValue<T>()
		{
			if (typeof(T).IsValueType)
				return Activator.CreateInstance<T>();

			if (typeof(T) == typeof(string))
				return (T)(object)string.Empty;

			return null;
		}
	}
}