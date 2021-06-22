using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class ChangelogEntry
	{
		public Version VersionNumber { get; init; } = null!;

		public DateTime Date { get; init; }

		public IReadOnlyList<Change> Changes { get; init; } = new List<Change>();
	}
}
