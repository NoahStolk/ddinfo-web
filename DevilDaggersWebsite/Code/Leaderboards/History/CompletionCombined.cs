using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Leaderboards.History
{
	public class CompletionCombined
	{
		public bool Initialized { get; set; }

		public Dictionary<string, CompletionEntryCombined> CompletionEntries { get; } = new Dictionary<string, CompletionEntryCombined>();
	}
}