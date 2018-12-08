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
			Completion completion = GetMissingProperties();

			StringBuilder global = new StringBuilder();
			global.AppendLine(completion.ToString());

			return global.ToString();
		}

		public string GetMissingUserInformation()
		{
			List<Completion> completions = new List<Completion>();
			foreach (Entry entry in Entries)
				completions.Add(entry.GetMissingProperties());

			StringBuilder user = new StringBuilder();
			foreach (KeyValuePair<string, CompletionEntry> kvp in completions[0].CompletionEntries)
			{
				int missing = 0;
				foreach (Completion completion in completions)
				{
					// TODO TryGetValue...
					try
					{
						CompletionEntry ce = completion.CompletionEntries[kvp.Key];
						if (ce == CompletionEntry.Missing)
							missing++;
					}
					catch { }
				}

				CompletionEntryCombined completionEntryCombined;
				if (missing == 0)
					completionEntryCombined = CompletionEntryCombined.Complete;
				else if (missing == completions.Count)
					completionEntryCombined = CompletionEntryCombined.Missing;
				else
					completionEntryCombined = CompletionEntryCombined.PartiallyMissing;

				if (completionEntryCombined != CompletionEntryCombined.Complete)
					user.AppendLine($"{kvp.Key} {completionEntryCombined}");
			}

			return user.ToString();
		}

		/// <summary>
		/// TODO: Get from Completion object
		/// </summary>
		/// <returns></returns>
		public float GetCompletionRate()
		{
			int total = 0;
			int missing = 0;

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
			}

			float globalCompletionRate = 1f - missing / (float)total;
			float userCompletionRate = 0;
			int totalEntries = Players == 0 ? 100 : Math.Min(Players, 100);
			foreach (Entry entry in Entries)
				userCompletionRate += entry.GetCompletionRate() * (1f / totalEntries);
			return userCompletionRate * 0.99f + globalCompletionRate * 0.01f;
		}

		public Completion GetMissingProperties()
		{
			Completion completion = new Completion();

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

				completion.CompletionEntries[info.Name] = CompletionEntry.Complete;

				if (valueString == ReflectionUtils.GetDefaultValue(type).ToString())
					completion.CompletionEntries[info.Name] = CompletionEntry.Missing;
			}

			int players = Players == 0 ? 100 : Math.Min(Players, 100);
			if (Entries.Count != players)
				completion.CompletionEntries[$"{players - Entries.Count} users"] = CompletionEntry.Missing;

			return completion;
		}
	}
}