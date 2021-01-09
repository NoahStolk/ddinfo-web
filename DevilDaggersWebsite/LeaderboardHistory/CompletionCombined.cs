﻿using System.Collections.Generic;

namespace DevilDaggersWebsite.LeaderboardHistory
{
	public class CompletionCombined
	{
		public bool Initialized { get; set; }

		public Dictionary<string, CompletionEntryCombined> CompletionEntries { get; } = new Dictionary<string, CompletionEntryCombined>();
	}
}