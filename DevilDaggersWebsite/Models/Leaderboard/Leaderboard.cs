using DevilDaggersWebsite.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

		private Completion completion = new Completion();

		public bool HasBlankName { get { return Entries.Any(e => string.IsNullOrEmpty(e.Username)); } }

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
				if (name.Contains("accuracy") || name.Contains("entries") || name.Contains("datetime") || name.Contains("completion") || name.Contains("hasblankname"))
					continue;

				completion.CompletionEntries[info.Name] = CompletionEntry.Complete;

				if (valueString == ReflectionUtils.GetDefaultValue(type).ToString())
					completion.CompletionEntries[info.Name] = CompletionEntry.Missing;
			}

			int players = Players == 0 ? 100 : Math.Min(Players, 100);
			if (Entries.Count != players)
				completion.CompletionEntries[$"{players - Entries.Count} users"] = CompletionEntry.Missing;

			completion.Initialised = true;
			return completion;
		}

		public string GetMissingGlobalInformation()
		{
			StringBuilder global = new StringBuilder();
			global.AppendLine(GetCompletion().ToString());

			return global.ToString();
		}

		public string GetMissingUserInformation()
		{
			List<Completion> completions = new List<Completion>();
			foreach (Entry entry in Entries)
				completions.Add(entry.GetCompletion());

			StringBuilder user = new StringBuilder();
			foreach (KeyValuePair<string, CompletionEntry> kvp in completions[0].CompletionEntries)
			{
				int total = HasBlankName ? completions.Count - 1 : completions.Count;
				int missing = 0;
				for (int i = 0; i < completions.Count; i++)
				{
					if (string.IsNullOrEmpty(Entries[i].Username))
						continue; // Skip the blank name

					if (completions[i].CompletionEntries.TryGetValue(kvp.Key, out CompletionEntry ce) && ce == CompletionEntry.Missing)
						missing++;
				}

				CompletionEntryCombined completionEntryCombined;
				if (missing == 0)
					completionEntryCombined = CompletionEntryCombined.Complete;
				else if (missing == total)
					completionEntryCombined = CompletionEntryCombined.Missing;
				else
					completionEntryCombined = CompletionEntryCombined.PartiallyMissing;

				if (completionEntryCombined != CompletionEntryCombined.Complete)
					user.AppendLine($"{kvp.Key} {completionEntryCombined}");
			}

			return user.ToString();
		}

		public float GetCompletionRate()
		{
			int total = HasBlankName ? 99 : 100;

			float globalCompletionRate = GetCompletion().GetCompletionRate();
			float userCompletionRate = 0;
			int totalEntries = Players == 0 ? total : Math.Min(Players, total);
			foreach (Entry entry in Entries)
				if (!string.IsNullOrEmpty(entry.Username)) // Skip the blank name
					userCompletionRate += entry.GetCompletion().GetCompletionRate() * (1f / totalEntries);
			return userCompletionRate * 0.99f + globalCompletionRate * 0.01f;
		}
	}
}