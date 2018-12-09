using System.Collections.Generic;
using System.Text;

namespace DevilDaggersWebsite.Models.Leaderboard
{
	public class Completion
	{
		public bool Initialised { get; set; }

		public Dictionary<string, CompletionEntry> CompletionEntries { get; set; } = new Dictionary<string, CompletionEntry>();

		public float GetCompletionRate()
		{
			int total = CompletionEntries.Count;
			int missing = 0;
			foreach (KeyValuePair<string, CompletionEntry> kvp in CompletionEntries)
				if (kvp.Value == CompletionEntry.Missing)
					missing++;
			return 1f - missing / (float)total;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, CompletionEntry> kvp in CompletionEntries)
				if (kvp.Value != CompletionEntry.Complete)
					sb.AppendLine($"{kvp.Key} {kvp.Value}");
			return sb.ToString();
		}
	}
}